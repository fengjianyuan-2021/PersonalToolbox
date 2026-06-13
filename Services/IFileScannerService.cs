using LargeFileCleaner.Models;

namespace LargeFileCleaner.Services;

/// <summary>
/// 大文件扫描服务接口，负责枚举目录并返回符合大小条件的文件。
/// </summary>
public interface IFileScannerService
{
    /// <summary>
    /// 异步扫描指定目录，返回不小于最小字节数的文件列表，并通过进度回调报告扫描状态。
    /// </summary>
    Task<IReadOnlyList<FileItem>> ScanAsync(string rootFolder, long minimumBytes, CancellationToken cancellationToken, IProgress<ScanProgress> progress);
}
