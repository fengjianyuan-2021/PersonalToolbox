namespace PersonalToolbox.Models;

/// <summary>
/// 删除操作结果，包含成功文件和失败原因。
/// </summary>
/// <param name="DeletedFiles">已经成功移入回收站的文件集合。</param>
/// <param name="FailedMessages">删除失败的路径和错误说明。</param>
public sealed record DeleteResult(IReadOnlyList<FileItem> DeletedFiles, IReadOnlyList<string> FailedMessages)
{
    /// <summary>
    /// 成功删除的文件数量，用于删除完成提示。
    /// </summary>
    public int DeletedCount => DeletedFiles.Count;
}
