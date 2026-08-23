using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Po.Application.Core.Lifecycle;

/// <summary>
/// 表示一个一次性的异步信号。
/// 调用 <see cref="Set"/> 后，所有等待者都会继续执行，且之后的等待会立即完成。
/// </summary>
public interface IAsyncSignal
{
    /// <summary>
    /// 等待信号。
    /// </summary>
    Task WaitAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 发出信号。
    /// </summary>
    void Set();

    /// <summary>
    /// 是否已发出信号。
    /// </summary>
    bool IsSet { get; }
}