using System.IO;
using LargeFileCleaner.Models;

namespace LargeFileCleaner.Services;

/// <summary>
/// 基于文件系统的大文件扫描服务；无法访问的目录会被跳过，不中断整体扫描。
/// </summary>
public sealed class FileScannerService : IFileScannerService
{
    /// <summary>
    /// 在后台线程扫描目录，避免阻塞 WPF 界面线程。
    /// </summary>
    public Task<IReadOnlyList<FileItem>> ScanAsync(string rootFolder, long minimumBytes, CancellationToken cancellationToken, IProgress<ScanProgress> progress)
    {
        return Task.Run(() => Scan(rootFolder, minimumBytes, cancellationToken, progress), cancellationToken);
    }

    /// <summary>
    /// 遍历目录树，收集达到大小阈值的文件并按体积从大到小返回。
    /// </summary>
    private static IReadOnlyList<FileItem> Scan(string rootFolder, long minimumBytes, CancellationToken cancellationToken, IProgress<ScanProgress> progress)
    {
        var results = new List<FileItem>();
        var scanned = 0;
        var pendingFolders = new Stack<string>();
        pendingFolders.Push(rootFolder);

        while (pendingFolders.Count > 0)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var folder = pendingFolders.Pop();

            foreach (var childFolder in SafeEnumerateDirectories(folder))
            {
                pendingFolders.Push(childFolder);
            }

            foreach (var filePath in SafeEnumerateFiles(folder))
            {
                cancellationToken.ThrowIfCancellationRequested();
                scanned++;

                try
                {
                    var info = new FileInfo(filePath);
                    if (info.Exists && info.Length >= minimumBytes)
                    {
                        results.Add(new FileItem(info.FullName, info.Name, info.DirectoryName ?? string.Empty, info.Length, info.LastWriteTime));
                    }
                }
                catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or PathTooLongException or System.Security.SecurityException)
                {
                    continue;
                }

                if (scanned % 100 == 0 || (results.Count > 0 && results.Count % 25 == 0))
                {
                    progress.Report(new ScanProgress(scanned, results.Count, filePath));
                }
            }
        }

        progress.Report(new ScanProgress(scanned, results.Count, rootFolder));
        return results.OrderByDescending(item => item.SizeBytes).ToList();
    }

    /// <summary>
    /// 安全枚举子目录，遇到权限或路径异常时返回空集合。
    /// </summary>
    private static IEnumerable<string> SafeEnumerateDirectories(string folder)
    {
        try
        {
            return Directory.EnumerateDirectories(folder);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or PathTooLongException or System.Security.SecurityException)
        {
            return [];
        }
    }

    /// <summary>
    /// 安全枚举文件，遇到权限或路径异常时返回空集合。
    /// </summary>
    private static IEnumerable<string> SafeEnumerateFiles(string folder)
    {
        try
        {
            return Directory.EnumerateFiles(folder);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or PathTooLongException or System.Security.SecurityException)
        {
            return [];
        }
    }
}
