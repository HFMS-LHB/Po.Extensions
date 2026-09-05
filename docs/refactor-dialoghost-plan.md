# Po.DialogHost.Avalonia 重构方案

## 目标

让 `Po.DialogHost.Avalonia` 不再直接依赖 `DialogHost.Avalonia` 这一具体控件库，同时保留 `TaskDialogBase` / `PoDialogBase` 的完整生命周期，并能够方便地接入 Ursa 等第三方弹窗控件。

## 现状分析

当前项目的耦合点（基于 `src/Po.DialogHost.Avalonia` 源码）：

| 耦合点 | 位置 | 说明 |
|---|---|---|
| 直接调用 `DialogHostAvalonia.DialogHost` 静态 API | `PoDialogService.cs` | `Show` / `Close` / `IsDialogOpen` 全部硬编码 |
| 继承 `DialogHostAvalonia.DialogHost` | `PoDialogHost.cs` | 控件宿主直接依赖 DialogHost |
| 使用 `DialogHostAvalonia.DialogSession` | `IPoDialogSessionAware.cs` / `PoDialogBase.cs` | VM 通过具体 Session 关闭弹窗 |
| 接口暴露 DialogHost 专有事件参数 | `IPoDialogService.cs` | `DialogOpenedEventArgs` / `DialogClosingEventArgs` / `DialogOpenedEventHandler` / `DialogClosingEventHandler` |
| 包引用 | `.csproj` | 直接引用 `DialogHost.Avalonia` |

## 核心设计

引入一层**适配器抽象**。核心库只保留：

1. `IPoDialogService`：对外服务契约。
2. `PoDialogBase<TData>` / `FormDialogBase<TData>` / `TaskDialogBase<TData>`：VM 基类。
3. `IPoDialogPolicy` / `IPoDialogSessionAware`：生命周期与关闭策略。
4. `IDialogHostAdapter` / `IPoDialogSession`：与具体弹窗控件解耦的适配器接口。

具体控件（DialogHost.Avalonia、Ursa 等）通过实现 `IDialogHostAdapter` 接入。

## 关键抽象

```csharp
// 替代 DialogHostAvalonia.DialogSession
public interface IPoDialogSession
{
    void Close(object? result = null);
    bool IsClosed { get; }
}

// 弹窗宿主适配器
public interface IDialogHostAdapter
{
    Task<object?> ShowAsync(
        object content,
        string? hostIdentifier = null,
        CancellationToken cancellationToken = default);

    void Close(string? hostIdentifier = null, object? parameter = null);
    bool IsDialogOpen(string? hostIdentifier = null);
}
```

`IPoDialogSessionAware` 改为：

```csharp
public interface IPoDialogSessionAware
{
    void SetSession(IPoDialogSession session);
}
```

## 第一阶段：在现有项目内重构

建议先在 `Po.DialogHost.Avalonia` 项目内完成拆分，不改变包结构，保持对现有使用者的最大兼容。

### 1. 新增/修改核心接口

文件：`src/Po.DialogHost.Avalonia/Interfaces/`

- 新增 `IPoDialogSession.cs`
- 新增 `IDialogHostAdapter.cs`
- 修改 `IPoDialogSessionAware.cs`：参数改为 `IPoDialogSession`
- 修改 `IPoDialogService.cs`：移除带 `DialogOpenedEventArgs` / `DialogClosingEventArgs` 的 overload，其余保留

### 2. 重构 VM 基类

- `PoDialogBase<TData>`：把 `DialogSession? _session` 换成 `IPoDialogSession? _session`
- `TaskDialogBase<TData>` / `FormDialogBase<TData>`：只通过 `IPoDialogService` 关闭，无需改动逻辑

### 3. 实现 DialogHost.Avalonia 适配器

新增文件夹 `src/Po.DialogHost.Avalonia/Adapters/DialogHostAvalonia/`：

- `DialogHostAvaloniaAdapter.cs`：实现 `IDialogHostAdapter`
- `DialogHostAvaloniaSession.cs`：包装 `DialogSession`，实现 `IPoDialogSession`
- 事件处理逻辑（原 `PoDialogHost.OnDialogOpened` / `OnDialogClosing`）迁移到适配器的 `ShowAsync` 事件回调里

`PoDialogHost.cs` 可以保留为薄的子类（兼容 XAML），但不再承担策略逻辑。

### 4. 重构 `PoDialogService`

`PoDialogService` 改为依赖注入 `IDialogHostAdapter`：

```csharp
public class PoDialogService(IDialogHostAdapter adapter) : IPoDialogService
{
    public Task<object?> ShowAsync(object content, string? hostIdentifier = "Main")
        => adapter.ShowAsync(content, hostIdentifier);

    public void Close(string? hostIdentifier = "Main", object? parameter = null)
        => adapter.Close(hostIdentifier, parameter);

    public bool IsDialogOpen(string? hostIdentifier = "Main")
        => adapter.IsDialogOpen(hostIdentifier);

    // ... 其余 overload
}
```

### 5. DI 注册

默认注册 `DialogHostAvaloniaAdapter`，保证现有项目升级后行为不变：

```csharp
public static IServiceCollection AddPoDialogHost(this IServiceCollection services)
{
    services.TryAddSingleton<IDialogHostAdapter, DialogHostAvaloniaAdapter>();
    services.TryAddSingleton<IPoDialogService, PoDialogService>();
    return services;
}
```

## 第二阶段：接入 Ursa

在解决方案中新增项目 `Po.DialogHost.Ursa`（或先放在同一项目的 `Adapters/Ursa/` 文件夹），引用：

- `Po.DialogHost.Avalonia`（核心）
- `Ursa.Avalonia`
- `Irihi.Avalonia.Shared.Contracts`（`IDialogContext` 所在包）

### Ursa 适配器要点

Ursa 的弹窗通过 `IDialogContext` 触发关闭：`VM` 实现 `IDialogContext` 并引发 `RequestClose` 事件。为了不把 `IDialogContext` 污染到核心，使用一个**代理控件**桥接：

```csharp
public class UrsaDialogProxy : ContentControl, IDialogContext
{
    public UrsaDialogSession Session { get; }
    public event EventHandler<object?>? RequestClose;

    public UrsaDialogProxy(object viewModel)
    {
        DataContext = viewModel;
        Session = new UrsaDialogSession(this);
        // 通过 DataTemplate 解析 View 并赋给 Content
    }

    public void Close() => RequestClose?.Invoke(this, Session.PendingResult);
}

public class UrsaDialogSession : IPoDialogSession
{
    private readonly UrsaDialogProxy _proxy;
    public object? PendingResult { get; private set; }
    public bool IsClosed { get; private set; }

    public void Close(object? result = null)
    {
        if (IsClosed) return;
        PendingResult = result;
        _proxy.Close();
    }
}
```

`UrsaDialogAdapter.ShowAsync` 流程：

1. 用 VM 创建 `UrsaDialogProxy`。
2. 如果 VM 是 `IPoDialogSessionAware`，调用 `SetSession(proxy.Session)`。
3. 如果 VM 是 `IPoDialogPolicy`，调用 `OnDialogOpened()`。
4. 调用 `OverlayDialog.ShowCustomAsync(proxy, hostId, options, cancellationToken)` 或 `Dialog.ShowCustomAsync(...)`。
5. 关闭时通过 `RequestClose` 回调触发 `IPoDialogPolicy.OnDialogClosed()` 并返回结果。

### 注意事项

- Ursa 的 `OverlayDialogOptions` / `DialogOptions` 是适配器私有的，可通过 `hostIdentifier` 做全局映射，或后续扩展 `ShowAsync` 的 options 参数。
- Ursa 对“取消关闭”的支持与 DialogHost 不同：如果后端无法拦截 `RequestClose`，则 `CanClose=false` 需要以“关闭按钮隐藏 + light dismiss 关闭 + VM 重新打开”等方式做最佳-effort 模拟，需在适配器文档中说明。

## 第三阶段：长期可选拆分

如果希望核心完全不引用 Avalonia 控件，可进一步拆包：

| 项目 | 内容 | 依赖 |
|---|---|---|
| `Po.DialogHost.Core` | `IPoDialogService`、VM 基类、适配器接口 | `CommunityToolkit.Mvvm`、`Microsoft.Extensions.DependencyInjection.Abstractions` |
| `Po.DialogHost.Avalonia` | `DialogHostAvaloniaAdapter`、`PoDialogHost` | Core + `DialogHost.Avalonia` |
| `Po.DialogHost.Ursa` | `UrsaDialogAdapter` | Core + `Ursa.Avalonia` |

当前建议先完成第一阶段，保留包名不变，后续再决定是否物理拆分。

## 影响面与 Breaking Changes

1. `IPoDialogSessionAware.SetSession(DialogSession)` → `SetSession(IPoDialogSession)`。自定义实现需要更新。
2. `IPoDialogService` 移除 DialogHost 专有事件 overload。如需保留，可作为 `DialogHostAvaloniaAdapter` 的扩展方法。
3. `PoDialogHost` 不再负责策略逻辑，仅作为兼容控件存在。
4. `.csproj` 仍引用 `DialogHost.Avalonia`（第一阶段），但所有直接调用都被隔离到适配器。

## 验证清单

- [ ] `PoDialogBase` / `FormDialogBase` / `TaskDialogBase` 不再引用 `DialogHostAvalonia`。
- [ ] `PoDialogService` 只通过 `IDialogHostAdapter` 操作弹窗。
- [ ] 现有 Demo 或测试能正常编译运行：关闭参数、取消、CanClose、CloseOnClickAway 行为一致。
- [ ] 新增 Ursa 适配器项目并编写最小示例。

## 建议的下一步

1. 确认是否接受第一阶段“在项目内拆分、保留包名”的方案。
2. 确认 Ursa 使用场景：优先支持 `OverlayDialog` 还是 `Dialog`（Window）？
3. 我可以先实现第一阶段的核心重构（不改业务逻辑），再实现 Ursa 适配器。

---

## 实施记录

已完成第一阶段（项目内拆分）与第二阶段（Ursa 适配器）。

### 新增/修改文件

**核心抽象**

- `src/Po.DialogHost.Avalonia/Interfaces/IPoDialogSession.cs`
- `src/Po.DialogHost.Avalonia/Interfaces/IDialogHostAdapter.cs`
- `src/Po.DialogHost.Avalonia/Interfaces/IPoDialogSessionAware.cs`（签名改为 `IPoDialogSession`）
- `src/Po.DialogHost.Avalonia/Interfaces/IPoDialogService.cs`（移除 DialogHost 专有事件 overload）

**DialogHost.Avalonia 适配器**

- `src/Po.DialogHost.Avalonia/Adapters/DialogHostAvaloniaAdapter.cs`
- `src/Po.DialogHost.Avalonia/Adapters/DialogHostAvaloniaSession.cs`
- `src/Po.DialogHost.Avalonia/PoDialogHost.cs`（更新为使用 `IPoDialogSession`）
- `src/Po.DialogHost.Avalonia/PoDialogService.cs`（改为依赖 `IDialogHostAdapter`）
- `src/Po.DialogHost.Avalonia/Dialogs/PoDialogBase.cs`（使用 `IPoDialogSession`）
- `src/Po.DialogHost.Avalonia/DependencyInjection/ServiceCollectionExtensions.cs`（默认注册 DialogHost 适配器，并新增 `AddPoDialogHost<TAdapter>`）

**Ursa 适配器**

- `src/Po.DialogHost.Ursa/Po.DialogHost.Ursa.csproj`
- `src/Po.DialogHost.Ursa/UrsaDialogAdapter.cs`
- `src/Po.DialogHost.Ursa/UrsaDialogProxy.cs`
- `src/Po.DialogHost.Ursa/UrsaDialogSession.cs`
- `src/Po.DialogHost.Ursa/DependencyInjection/ServiceCollectionExtensions.cs`
- `src/Po.DialogHost.Ursa/README.md`

### 验证结果

```powershell
dotnet build Po.Extensions.slnx -p:UsedAvaloniaProducts=''
```

整个解决方案编译通过（net8.0 / net10.0），无错误。

### 使用方式

**继续使用 DialogHost.Avalonia（默认，向后兼容）**

```csharp
services.AddPoDialogHost();
```

**切换到 Ursa OverlayDialog**

```csharp
services.AddPoDialogHostWithUrsa();
```

XAML 中需要放置 `OverlayDialogHost`：

```xml
<u:OverlayDialogHost HostId="Main" />
```

### 已知限制

- Ursa 适配器的 `Close(hostIdentifier, parameter)` 与 `IsDialogOpen(hostIdentifier)` 依赖适配器内部维护的栈；因为 Ursa 没有提供静态的 `Close`/`IsDialogOpen` API。
- `CanClose=false` 在 Ursa 中通过隐藏关闭按钮、禁用 light dismiss 来模拟；Ursa 不支持拦截 `RequestClose`。

---

## 2026-09-05 更新：已完成包重命名与物理拆分

根据进一步决策，已将 `Po.DialogHost.Avalonia` 拆分为三个独立项目：

| 项目 | PackageId | 说明 | 依赖 |
|---|---|---|---|
| `Po.DialogHost.Core` | `Po.DialogHost.Core` | 核心框架，完全不含任何具体弹窗控件 | Avalonia、CommunityToolkit.Mvvm、MS.DI.Abstractions |
| `Po.DialogHost.DialogHostAvalonia` | `Po.DialogHost.DialogHostAvalonia` | DialogHost.Avalonia 官方适配器 | Core + DialogHost.Avalonia |
| `Po.DialogHost.Ursa` | `Po.DialogHost.Ursa` | Ursa OverlayDialog 官方适配器 | Core + Irihi.Ursa |

### 注册方式变化

```csharp
// 旧写法（已移除）
services.AddPoDialogHost();

// DialogHost.Avalonia
services.AddPoDialogHostWithDialogHostAvalonia();

// Ursa
services.AddPoDialogHostWithUrsa();

// 自定义适配器
services.AddPoDialogHost<MyAdapter>();
```

### 命名空间变化

- `Po.DialogHost.Avalonia` → `Po.DialogHost.Core`
- `Po.DialogHost.Avalonia.Interfaces` → `Po.DialogHost.Core.Interfaces`
- `Po.DialogHost.Avalonia.Dialogs` → `Po.DialogHost.Core.Dialogs`
- `Po.DialogHost.Avalonia.DependencyInjection` → `Po.DialogHost.Core.DependencyInjection`
- 新增 `Po.DialogHost.DialogHostAvalonia`
- 新增 `Po.DialogHost.DialogHostAvalonia.DependencyInjection`
- `Po.DialogHost.Ursa` 命名空间保持不变

### 验证

```powershell
dotnet build Po.Extensions.slnx -p:UsedAvaloniaProducts=''
```

9 个项目全部编译通过，0 错误。
