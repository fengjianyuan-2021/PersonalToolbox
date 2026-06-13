using System.Windows.Input;
using LargeFileCleaner.Core;
using LargeFileCleaner.Models;

namespace LargeFileCleaner.ViewModels;

/// <summary>
/// 菜单首页 ViewModel，展示已经开发好的功能入口。
/// </summary>
public sealed class HomeViewModel : ViewModelBase
{
    /// <summary>
    /// 首页中展示的工具入口集合。
    /// </summary>
    private IReadOnlyList<ToolNavigationItem> _tools = [];

    /// <summary>
    /// 首页卡片点击后打开工具页面的命令。
    /// </summary>
    private ICommand? _openToolCommand;

    /// <summary>
    /// 首页卡片列表，不包含“菜单首页”自身。
    /// </summary>
    public IReadOnlyList<ToolNavigationItem> Tools
    {
        get => _tools;
        set => SetProperty(ref _tools, value);
    }

    /// <summary>
    /// 用户点击首页工具卡片时触发的导航命令。
    /// </summary>
    public ICommand? OpenToolCommand
    {
        get => _openToolCommand;
        set => SetProperty(ref _openToolCommand, value);
    }
}
