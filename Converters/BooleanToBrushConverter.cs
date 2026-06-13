using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace PersonalToolbox.Converters;

/// <summary>
/// 将布尔选中状态转换为菜单按钮背景色。
/// </summary>
public sealed class BooleanToBrushConverter : IValueConverter
{
    /// <summary>
    /// 根据菜单是否选中返回高亮或普通背景画刷。
    /// </summary>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is true
            ? new SolidColorBrush(System.Windows.Media.Color.FromRgb(37, 99, 235))
            : new SolidColorBrush(System.Windows.Media.Color.FromRgb(37, 48, 74));
    }

    /// <summary>
    /// 不支持从背景画刷反向推导选中状态。
    /// </summary>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
