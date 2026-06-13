using System.Windows;

namespace PersonalToolbox.Services;

/// <summary>
/// 基于 WPF MessageBox 的消息服务。
/// </summary>
public sealed class MessageDialogService : IMessageDialogService
{
    /// <summary>
    /// 消息框所属窗口，用于让弹窗居中并跟随主窗口。
    /// </summary>
    private readonly Window _owner;

    /// <summary>
    /// 创建消息服务，并绑定弹窗所属窗口。
    /// </summary>
    public MessageDialogService(Window owner)
    {
        _owner = owner;
    }

    /// <summary>
    /// 显示普通信息弹窗。
    /// </summary>
    public void ShowInfo(string message, string title)
    {
        System.Windows.MessageBox.Show(_owner, message, title, MessageBoxButton.OK, MessageBoxImage.Information);
    }

    /// <summary>
    /// 显示警告弹窗。
    /// </summary>
    public void ShowWarning(string message, string title)
    {
        System.Windows.MessageBox.Show(_owner, message, title, MessageBoxButton.OK, MessageBoxImage.Warning);
    }

    /// <summary>
    /// 显示危险操作确认弹窗，并返回用户是否同意继续。
    /// </summary>
    public bool ConfirmWarning(string message, string title)
    {
        return System.Windows.MessageBox.Show(_owner, message, title, MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No) == MessageBoxResult.Yes;
    }
}
