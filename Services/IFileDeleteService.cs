using PersonalToolbox.Models;

namespace PersonalToolbox.Services;

/// <summary>
/// 文件删除服务接口，统一封装危险操作。
/// </summary>
public interface IFileDeleteService
{
    /// <summary>
    /// 将指定文件移入回收站，并返回成功和失败明细。
    /// </summary>
    DeleteResult MoveToRecycleBin(IReadOnlyList<FileItem> files);
}
