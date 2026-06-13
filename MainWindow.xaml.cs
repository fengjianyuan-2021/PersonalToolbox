using System.Windows;
using PersonalToolbox.Services;
using PersonalToolbox.ViewModels;

namespace PersonalToolbox;

/// <summary>
/// 应用主窗口，只负责装载 Shell 级 ViewModel 和导航容器。
/// </summary>
public partial class MainWindow : Window
{
    /// <summary>
    /// 初始化主窗口、创建服务依赖，并设置主界面 DataContext。
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();

        var folderPicker = new FolderPickerService();
        var messageDialog = new MessageDialogService(this);
        DataContext = new MainViewModel(
            new LargeFileCleanerViewModel(
                new FileScannerService(),
                new FileDeleteService(),
                new FileExplorerService(),
                folderPicker,
                messageDialog));
    }
}
