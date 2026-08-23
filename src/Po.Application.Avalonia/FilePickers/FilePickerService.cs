using Avalonia.Controls;
using Avalonia.Platform.Storage;

using Po.Application.Avalonia.Windows;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Po.Application.Avalonia.FilePickers;

public class FilePickerService : IFilePickerService
{
    private readonly IMainWindowProvider _windowProvider;

    public FilePickerService(IMainWindowProvider windowProvider)
    {
        _windowProvider = windowProvider;
    }

    public IStorageProvider? GetStorageProvider(Window? window = null)
    {
        var ownerWindow = window ?? _windowProvider.MainWindow;
        if (ownerWindow == null) return null;
        return ownerWindow.StorageProvider;
    }

    public async Task<IReadOnlyList<IStorageFile>?> OpenFileAsync(FilePickerOpenOptions options, Window? window = null)
    {
        var ownerWindow = window ?? _windowProvider.MainWindow;
        if (ownerWindow == null) return null;
        return await ownerWindow.StorageProvider.OpenFilePickerAsync(options);
    }

    public async Task<IStorageFile?> SaveFileAsync(FilePickerSaveOptions options, Window? window = null)
    {
        var ownerWindow = window ?? _windowProvider.MainWindow;
        if (ownerWindow == null) return null;
        return await ownerWindow.StorageProvider.SaveFilePickerAsync(options);
    }
}
