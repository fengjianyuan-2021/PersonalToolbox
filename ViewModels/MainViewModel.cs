using System.Collections.ObjectModel;
using System.Reflection;
using PersonalToolbox.Core;
using PersonalToolbox.Models;

namespace PersonalToolbox.ViewModels;

/// <summary>
/// 主界面 ViewModel，管理左侧菜单和右侧当前页面。
/// </summary>
public sealed class MainViewModel : ViewModelBase
{
    /// <summary>
    /// 当前显示在右侧内容区的页面 ViewModel。
    /// </summary>
    private object _currentPage;

    /// <summary>
    /// 初始化工具菜单，并默认进入菜单首页。
    /// </summary>
    public MainViewModel(LargeFileCleanerViewModel largeFileCleanerViewModel)
    {
        var homeViewModel = new HomeViewModel();
        Tools =
        [
            new ToolNavigationItem("home", "菜单首页", "查看当前可用工具", () => homeViewModel),
            new ToolNavigationItem("large-file-cleaner", "大文件清理", "扫描大文件并移入回收站", () => largeFileCleanerViewModel)
        ];

        homeViewModel.Tools = Tools.Where(tool => tool.Key != "home").ToList();
        homeViewModel.OpenToolCommand = new RelayCommand(parameter =>
        {
            if (parameter is ToolNavigationItem tool)
            {
                Navigate(tool);
            }
        });

        NavigateCommand = new RelayCommand(parameter =>
        {
            if (parameter is ToolNavigationItem tool)
            {
                Navigate(tool);
            }
        });

        _currentPage = homeViewModel;
        Navigate(Tools[0]);
    }

    /// <summary>
    /// 左侧导航菜单中的全部工具入口。
    /// </summary>
    public ObservableCollection<ToolNavigationItem> Tools { get; }

    /// <summary>
    /// 左侧菜单点击时执行的页面跳转命令。
    /// </summary>
    public RelayCommand NavigateCommand { get; }

    /// <summary>
    /// 应用版本显示文本，从程序集信息版本读取。
    /// </summary>
    public string VersionText
    {
        get
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version?.ToString(3)
                ?? "未知版本";
            return $"v{version}";
        }
    }

    /// <summary>
    /// 当前展示的页面 ViewModel，交给 ContentControl 自动套用 DataTemplate。
    /// </summary>
    public object CurrentPage
    {
        get => _currentPage;
        private set => SetProperty(ref _currentPage, value);
    }

    /// <summary>
    /// 切换当前页面，并同步更新菜单选中状态。
    /// </summary>
    private void Navigate(ToolNavigationItem selectedTool)
    {
        foreach (var tool in Tools)
        {
            tool.IsSelected = tool == selectedTool;
        }

        CurrentPage = selectedTool.CreatePage();
    }
}
