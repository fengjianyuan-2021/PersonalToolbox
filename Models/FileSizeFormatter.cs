namespace PersonalToolbox.Models;

/// <summary>
/// 文件大小格式化工具，统一列表和状态栏中的容量显示。
/// </summary>
public static class FileSizeFormatter
{
    /// <summary>
    /// 将字节数转换成带单位的可读文本。
    /// </summary>
    public static string Format(long bytes)
    {
        string[] units = ["B", "KB", "MB", "GB", "TB"];
        double size = bytes;
        var unitIndex = 0;

        while (size >= 1024 && unitIndex < units.Length - 1)
        {
            size /= 1024;
            unitIndex++;
        }

        return $"{size:N2} {units[unitIndex]}";
    }
}
