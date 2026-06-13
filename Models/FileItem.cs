using System.Globalization;
using PersonalToolbox.Core;

namespace PersonalToolbox.Models;

/// <summary>
/// 扫描得到的大文件信息，包含列表展示和删除勾选状态。
/// </summary>
public sealed class FileItem : ViewModelBase
{
    /// <summary>
    /// 记录用户是否在列表中勾选了该文件。
    /// </summary>
    private bool _isSelected;

    /// <summary>
    /// 创建一个文件展示项，并保存路径、大小和修改时间。
    /// </summary>
    public FileItem(string path, string name, string folder, long sizeBytes, DateTime lastWriteTime)
    {
        Path = path;
        Name = name;
        Folder = folder;
        SizeBytes = sizeBytes;
        LastWriteTime = lastWriteTime;
    }

    /// <summary>
    /// 表示该文件是否被用户选中用于后续删除。
    /// </summary>
    public bool IsSelected
    {
        get => _isSelected;
        set => SetProperty(ref _isSelected, value);
    }

    /// <summary>
    /// 文件完整路径，用于删除和资源管理器定位。
    /// </summary>
    public string Path { get; }

    /// <summary>
    /// 文件名，用于列表中的主标题展示。
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// 文件所在目录，用于列表展示和打开所在目录。
    /// </summary>
    public string Folder { get; }

    /// <summary>
    /// 文件原始字节大小，用于排序、统计和删除确认。
    /// </summary>
    public long SizeBytes { get; }

    /// <summary>
    /// 适合用户阅读的文件大小文本。
    /// </summary>
    public string SizeText => FileSizeFormatter.Format(SizeBytes);

    /// <summary>
    /// 文件最后修改时间，用于判断文件是否可能还能删除。
    /// </summary>
    public DateTime LastWriteTime { get; }

    /// <summary>
    /// 适合列表展示的最后修改时间文本。
    /// </summary>
    public string LastWriteTimeText => LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CurrentCulture);
}
