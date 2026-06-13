using System.Diagnostics;
using System.IO;
using PersonalToolbox.Models;

namespace PersonalToolbox.Services;

/// <summary>
/// 调用 Windows 资源管理器定位指定文件。
/// </summary>
public sealed class FileExplorerService : IFileExplorerService
{
    /// <summary>
    /// 打开资源管理器并选中文件；如果目录不存在则不执行任何操作。
    /// </summary>
    public void SelectFile(FileItem file)
    {
        if (!Directory.Exists(file.Folder))
        {
            return;
        }

        Process.Start(new ProcessStartInfo("explorer.exe", $"/select,\"{file.Path}\"") { UseShellExecute = true });
    }
}
