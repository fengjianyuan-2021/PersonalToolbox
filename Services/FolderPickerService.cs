using System.IO;
using WinForms = System.Windows.Forms;

namespace LargeFileCleaner.Services;

/// <summary>
/// 使用系统文件夹选择对话框获取扫描目录。
/// </summary>
public sealed class FolderPickerService : IFolderPickerService
{
    /// <summary>
    /// 显示文件夹浏览窗口，并在用户确认后返回选择路径。
    /// </summary>
    public string? PickFolder(string initialFolder)
    {
        using var dialog = new WinForms.FolderBrowserDialog
        {
            Description = "选择要扫描的目录",
            UseDescriptionForTitle = true,
            SelectedPath = Directory.Exists(initialFolder) ? initialFolder : Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
        };

        return dialog.ShowDialog() == WinForms.DialogResult.OK ? dialog.SelectedPath : null;
    }
}
