namespace PersonalToolbox.Services;

/// <summary>
/// 文件夹选择服务接口，隔离 WinForms 对话框依赖。
/// </summary>
public interface IFolderPickerService
{
    /// <summary>
    /// 打开系统文件夹选择对话框，并返回用户选择的目录。
    /// </summary>
    string? PickFolder(string initialFolder);
}
