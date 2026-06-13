using System.Windows.Input;

namespace LargeFileCleaner.Core;

/// <summary>
/// 同步命令实现，用于把按钮等界面操作绑定到 ViewModel 方法。
/// </summary>
public sealed class RelayCommand : ICommand
{
    /// <summary>
    /// 命令真正执行的同步逻辑。
    /// </summary>
    private readonly Action<object?> _execute;

    /// <summary>
    /// 判断命令当前是否允许执行的可选逻辑。
    /// </summary>
    private readonly Predicate<object?>? _canExecute;

    /// <summary>
    /// 创建不需要命令参数的同步命令。
    /// </summary>
    public RelayCommand(Action execute, Func<bool>? canExecute = null)
        : this(_ => execute(), canExecute is null ? null : _ => canExecute())
    {
    }

    /// <summary>
    /// 创建可接收命令参数的同步命令。
    /// </summary>
    public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    /// <summary>
    /// 通知 WPF 重新查询命令可用状态的事件。
    /// </summary>
    public event EventHandler? CanExecuteChanged;

    /// <summary>
    /// 返回命令在当前参数下是否可以执行。
    /// </summary>
    public bool CanExecute(object? parameter)
    {
        return _canExecute?.Invoke(parameter) ?? true;
    }

    /// <summary>
    /// 执行绑定到命令的同步业务逻辑。
    /// </summary>
    public void Execute(object? parameter)
    {
        _execute(parameter);
    }

    /// <summary>
    /// 主动通知界面刷新按钮等控件的启用状态。
    /// </summary>
    public void NotifyCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
