namespace LargeFileCleaner.Services;

/// <summary>
/// 消息框服务接口，便于 ViewModel 请求确认或提示。
/// </summary>
public interface IMessageDialogService
{
    /// <summary>
    /// 显示普通信息提示，用于告知操作完成或缺少选择。
    /// </summary>
    void ShowInfo(string message, string title);

    /// <summary>
    /// 显示警告提示，用于告知输入错误或部分操作失败。
    /// </summary>
    void ShowWarning(string message, string title);

    /// <summary>
    /// 显示带确认按钮的警告框，用于删除等危险操作前的二次确认。
    /// </summary>
    bool ConfirmWarning(string message, string title);
}
