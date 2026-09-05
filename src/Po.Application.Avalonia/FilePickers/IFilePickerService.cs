using Avalonia.Controls;
using Avalonia.Platform.Storage;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Po.Application.Avalonia.FilePickers;

public interface IFilePickerService
{
    public IStorageProvider? GetStorageProvider(Window? window = null);
    public Task<IReadOnlyList<IStorageFile>?> OpenFileAsync(FilePickerOpenOptions options, Window? window = null);
    public Task<IStorageFile?> SaveFileAsync(FilePickerSaveOptions options, Window? window = null);
}
