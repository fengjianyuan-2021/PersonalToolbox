using LargeFileCleaner.Models;

namespace LargeFileCleaner.Services;

/// <summary>
/// 资源管理器服务接口，用于打开文件所在目录。
/// </summary>
public interface IFileExplorerService
{
    /// <summary>
    /// 在 Windows 资源管理器中选中指定文件。
    /// </summary>
    void SelectFile(FileItem file);
}
