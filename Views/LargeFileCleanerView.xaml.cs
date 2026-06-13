namespace LargeFileCleaner.Views;

/// <summary>
/// 大文件清理视图，所有业务行为通过绑定命令交给 ViewModel。
/// </summary>
public partial class LargeFileCleanerView : System.Windows.Controls.UserControl
{
    /// <summary>
    /// 初始化大文件清理页面视图组件。
    /// </summary>
    public LargeFileCleanerView()
    {
        InitializeComponent();
    }
}
