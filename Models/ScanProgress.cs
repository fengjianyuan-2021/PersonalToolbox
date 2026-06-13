namespace PersonalToolbox.Models;

/// <summary>
/// 文件扫描进度，用于从服务层向 ViewModel 汇报当前状态。
/// </summary>
/// <param name="ScannedCount">已经检查过的文件数量。</param>
/// <param name="MatchedCount">已经匹配到的大文件数量。</param>
/// <param name="CurrentPath">当前正在处理或刚处理过的路径。</param>
public sealed record ScanProgress(int ScannedCount, int MatchedCount, string CurrentPath);
