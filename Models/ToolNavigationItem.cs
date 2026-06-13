using LargeFileCleaner.Core;

namespace LargeFileCleaner.Models;

/// <summary>
/// 工具菜单项，描述一个可跳转的功能页面。
/// </summary>
public sealed class ToolNavigationItem : ViewModelBase
{
    /// <summary>
    /// 记录菜单项当前是否处于选中状态。
    /// </summary>
    private bool _isSelected;

    /// <summary>
    /// 创建一个菜单项，并提供页面 ViewModel 的创建逻辑。
    /// </summary>
    public ToolNavigationItem(string key, string title, string description, Func<object> createPage)
    {
        Key = key;
        Title = title;
        Description = description;
        CreatePage = createPage;
    }

    /// <summary>
    /// 工具唯一标识，用于区分首页和各个功能页面。
    /// </summary>
    public string Key { get; }

    /// <summary>
    /// 菜单中展示的工具名称。
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// 菜单中展示的工具用途说明。
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// 创建或返回该工具对应页面 ViewModel 的委托。
    /// </summary>
    public Func<object> CreatePage { get; }

    /// <summary>
    /// 表示该菜单项是否是当前页面。
    /// </summary>
    public bool IsSelected
    {
        get => _isSelected;
        set => SetProperty(ref _isSelected, value);
    }
}
