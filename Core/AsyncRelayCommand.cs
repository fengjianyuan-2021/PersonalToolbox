using System.Windows.Input;

namespace PersonalToolbox.Core;

/// <summary>
/// 异步命令实现，避免耗时操作阻塞 WPF UI 线程。
/// </summary>
public sealed class AsyncRelayCommand : ICommand
{
    /// <summary>
    /// 命令真正执行的异步逻辑。
    /// </summary>
    private readonly Func<Task> _executeAsync;

    /// <summary>
    /// 判断命令当前是否允许执行的可选逻辑。
    /// </summary>
    private readonly Func<bool>? _canExecute;

    /// <summary>
    /// 标记异步命令是否正在运行，防止用户重复触发。
    /// </summary>
    private bool _isRunning;

    /// <summary>
    /// 创建一个异步命令，并可选提供可执行状态判断。
    /// </summary>
    public AsyncRelayCommand(Func<Task> executeAsync, Func<bool>? canExecute = null)
    {
        _executeAsync = executeAsync;
        _canExecute = canExecute;
    }

    /// <summary>
    /// 通知 WPF 重新查询命令可用状态的事件。
    /// </summary>
    public event EventHandler? CanExecuteChanged;

    /// <summary>
    /// 返回异步命令当前是否可以执行。
    /// </summary>
    public bool CanExecute(object? parameter)
    {
        return !_isRunning && (_canExecute?.Invoke() ?? true);
    }

    /// <summary>
    /// 执行异步任务，并在运行前后刷新命令状态。
    /// </summary>
    public async void Execute(object? parameter)
    {
        if (!CanExecute(parameter))
        {
            return;
        }

        try
        {
            _isRunning = true;
            NotifyCanExecuteChanged();
            await _executeAsync();
        }
        finally
        {
            _isRunning = false;
            NotifyCanExecuteChanged();
        }
    }

    /// <summary>
    /// 主动通知界面刷新按钮等控件的启用状态。
    /// </summary>
    public void NotifyCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
