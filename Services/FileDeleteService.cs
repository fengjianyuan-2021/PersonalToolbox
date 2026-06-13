using System.IO;
using LargeFileCleaner.Models;
using Microsoft.VisualBasic.FileIO;

namespace LargeFileCleaner.Services;

/// <summary>
/// 使用 Windows 回收站删除文件，降低误删后的恢复成本。
/// </summary>
public sealed class FileDeleteService : IFileDeleteService
{
    /// <summary>
    /// 逐个将文件移入回收站，单个文件失败不会中断后续文件处理。
    /// </summary>
    public DeleteResult MoveToRecycleBin(IReadOnlyList<FileItem> files)
    {
        var deletedFiles = new List<FileItem>();
        var failed = new List<string>();

        foreach (var file in files)
        {
            try
            {
                if (File.Exists(file.Path))
                {
                    FileSystem.DeleteFile(file.Path, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                    deletedFiles.Add(file);
                }
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or System.Security.SecurityException)
            {
                failed.Add($"{file.Path}: {ex.Message}");
            }
        }

        return new DeleteResult(deletedFiles, failed);
    }
}
