using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PersonalToolbox.Core;

/// <summary>
/// ViewModel 基类，统一提供属性变更通知能力。
/// </summary>
public abstract class ViewModelBase : INotifyPropertyChanged
{
    /// <summary>
    /// 当绑定属性发生变化时通知 WPF 刷新界面。
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// 更新字段值，并在值确实变化时触发属性变更通知。
    /// </summary>
    protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return false;
        }

        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    /// <summary>
    /// 主动触发指定属性的变更通知。
    /// </summary>
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
