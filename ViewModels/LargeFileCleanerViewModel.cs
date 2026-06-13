using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using PersonalToolbox.Core;
using PersonalToolbox.Models;
using PersonalToolbox.Services;

namespace PersonalToolbox.ViewModels;

/// <summary>
/// 大文件清理页面 ViewModel，负责扫描条件、扫描结果和删除命令。
/// </summary>
public sealed class LargeFileCleanerViewModel : ViewModelBase
{
    /// <summary>
    /// 执行大文件扫描的服务依赖。
    /// </summary>
    private readonly IFileScannerService _fileScanner;

    /// <summary>
    /// 执行文件回收站删除的服务依赖。
    /// </summary>
    private readonly IFileDeleteService _fileDelete;

    /// <summary>
    /// 打开资源管理器并定位文件的服务依赖。
    /// </summary>
    private readonly IFileExplorerService _fileExplorer;

    /// <summary>
    /// 打开系统文件夹选择对话框的服务依赖。
    /// </summary>
    private readonly IFolderPickerService _folderPicker;

    /// <summary>
    /// 显示提示和确认弹窗的服务依赖。
    /// </summary>
    private readonly IMessageDialogService _messageDialog;

    /// <summary>
    /// 当前扫描任务的取消令牌源，用于用户点击停止时取消后台扫描。
    /// </summary>
    private CancellationTokenSource? _scanCancellation;

    /// <summary>
    /// 用户输入或选择的扫描目录路径。
    /// </summary>
    private string _folderPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

    /// <summary>
    /// 用户输入的最小文件大小，单位为 MB。
    /// </summary>
    private string _minimumSizeMbText = "100";

    /// <summary>
    /// 页面底部展示的扫描、删除和校验状态文本。
    /// </summary>
    private string _statusText = "请选择要扫描的目录。";

    /// <summary>
    /// 标记当前是否正在扫描，用于控制按钮和进度条状态。
    /// </summary>
    private bool _isScanning;

    /// <summary>
    /// DataGrid 当前选中的文件，用于打开所在目录。
    /// </summary>
    private FileItem? _selectedFile;

    /// <summary>
    /// 初始化大文件清理页面依赖，并创建所有界面命令。
    /// </summary>
    public LargeFileCleanerViewModel(
        IFileScannerService fileScanner,
        IFileDeleteService fileDelete,
        IFileExplorerService fileExplorer,
        IFolderPickerService folderPicker,
        IMessageDialogService messageDialog)
    {
        _fileScanner = fileScanner;
        _fileDelete = fileDelete;
        _fileExplorer = fileExplorer;
        _folderPicker = folderPicker;
        _messageDialog = messageDialog;

        BrowseCommand = new RelayCommand(BrowseFolder, () => !IsScanning);
        ScanCommand = new AsyncRelayCommand(ScanAsync, () => !IsScanning);
        CancelCommand = new RelayCommand(CancelScan, () => IsScanning);
        SelectAllCommand = new RelayCommand(SelectAll, () => Files.Count > 0);
        ClearSelectionCommand = new RelayCommand(ClearSelection, () => Files.Count > 0);
        DeleteSelectedCommand = new RelayCommand(DeleteSelected, () => !IsScanning && Files.Any(file => file.IsSelected));
        OpenFolderCommand = new RelayCommand(OpenSelectedFolder, () => SelectedFile is not null);
    }

    /// <summary>
    /// 扫描结果文件集合，绑定到页面文件列表。
    /// </summary>
    public ObservableCollection<FileItem> Files { get; } = [];

    /// <summary>
    /// 浏览按钮命令，用于选择扫描目录。
    /// </summary>
    public RelayCommand BrowseCommand { get; }

    /// <summary>
    /// 开始扫描按钮命令，用于异步检索大文件。
    /// </summary>
    public AsyncRelayCommand ScanCommand { get; }

    /// <summary>
    /// 停止按钮命令，用于取消当前扫描任务。
    /// </summary>
    public RelayCommand CancelCommand { get; }

    /// <summary>
    /// 全选按钮命令，用于勾选所有扫描结果。
    /// </summary>
    public RelayCommand SelectAllCommand { get; }

    /// <summary>
    /// 取消选择按钮命令，用于清空所有文件勾选状态。
    /// </summary>
    public RelayCommand ClearSelectionCommand { get; }

    /// <summary>
    /// 删除选中文件命令，用于将勾选文件移入回收站。
    /// </summary>
    public RelayCommand DeleteSelectedCommand { get; }

    /// <summary>
    /// 打开所在目录命令，用于在资源管理器中定位当前选中文件。
    /// </summary>
    public RelayCommand OpenFolderCommand { get; }

    /// <summary>
    /// 扫描目录路径，绑定到目录输入框。
    /// </summary>
    public string FolderPath
    {
        get => _folderPath;
        set => SetProperty(ref _folderPath, value);
    }

    /// <summary>
    /// 最小文件大小文本，绑定到 MB 输入框。
    /// </summary>
    public string MinimumSizeMbText
    {
        get => _minimumSizeMbText;
        set => SetProperty(ref _minimumSizeMbText, value);
    }

    /// <summary>
    /// 页面底部状态文本，提示当前扫描或删除结果。
    /// </summary>
    public string StatusText
    {
        get => _statusText;
        private set => SetProperty(ref _statusText, value);
    }

    /// <summary>
    /// 当前是否正在扫描，用于禁用危险操作并显示进度条。
    /// </summary>
    public bool IsScanning
    {
        get => _isScanning;
        private set
        {
            if (SetProperty(ref _isScanning, value))
            {
                OnPropertyChanged(nameof(IsNotScanning));
                RefreshCommandStates();
            }
        }
    }

    /// <summary>
    /// 当前是否不在扫描中，供界面绑定反向状态时使用。
    /// </summary>
    public bool IsNotScanning => !IsScanning;

    /// <summary>
    /// 列表当前选中的文件，用于打开所在目录。
    /// </summary>
    public FileItem? SelectedFile
    {
        get => _selectedFile;
        set
        {
            if (SetProperty(ref _selectedFile, value))
            {
                OpenFolderCommand.NotifyCanExecuteChanged();
            }
        }
    }

    /// <summary>
    /// 打开文件夹选择器，并把用户选择的目录写回扫描路径。
    /// </summary>
    private void BrowseFolder()
    {
        var selectedFolder = _folderPicker.PickFolder(FolderPath);
        if (!string.IsNullOrWhiteSpace(selectedFolder))
        {
            FolderPath = selectedFolder;
        }
    }

    /// <summary>
    /// 校验扫描条件，异步扫描大文件，并把结果填充到列表。
    /// </summary>
    private async Task ScanAsync()
    {
        if (!Directory.Exists(FolderPath))
        {
            _messageDialog.ShowWarning("请选择一个存在的目录。", "目录无效");
            return;
        }

        if (!double.TryParse(MinimumSizeMbText.Trim(), NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out var minimumSizeMb) ||
            minimumSizeMb < 0)
        {
            _messageDialog.ShowWarning("最小大小请输入大于或等于 0 的数字。", "输入无效");
            return;
        }

        Files.Clear();
        SelectedFile = null;
        IsScanning = true;
        _scanCancellation = new CancellationTokenSource();

        var progress = new Progress<ScanProgress>(p =>
        {
            StatusText = $"已检查 {p.ScannedCount:N0} 个文件，找到 {p.MatchedCount:N0} 个大文件。当前：{p.CurrentPath}";
        });

        try
        {
            var minimumBytes = (long)(minimumSizeMb * 1024 * 1024);
            var results = await _fileScanner.ScanAsync(FolderPath, minimumBytes, _scanCancellation.Token, progress);

            foreach (var item in results)
            {
                item.PropertyChanged += (_, _) => RefreshCommandStates();
                Files.Add(item);
            }

            UpdateStatus();
        }
        catch (OperationCanceledException)
        {
            StatusText = $"扫描已停止，当前保留 {Files.Count:N0} 个结果。";
        }
        finally
        {
            _scanCancellation?.Dispose();
            _scanCancellation = null;
            IsScanning = false;
            RefreshCommandStates();
        }
    }

    /// <summary>
    /// 请求取消当前扫描任务。
    /// </summary>
    private void CancelScan()
    {
        _scanCancellation?.Cancel();
    }

    /// <summary>
    /// 将当前列表中的所有文件标记为选中。
    /// </summary>
    private void SelectAll()
    {
        foreach (var item in Files)
        {
            item.IsSelected = true;
        }
    }

    /// <summary>
    /// 清除当前列表中所有文件的选中状态。
    /// </summary>
    private void ClearSelection()
    {
        foreach (var item in Files)
        {
            item.IsSelected = false;
        }
    }

    /// <summary>
    /// 二次确认后将勾选文件移入回收站，并从列表移除成功删除的文件。
    /// </summary>
    private void DeleteSelected()
    {
        var selectedItems = Files.Where(item => item.IsSelected).ToList();
        if (selectedItems.Count == 0)
        {
            _messageDialog.ShowInfo("请先勾选要删除的文件。", "未选择文件");
            return;
        }

        var totalSize = selectedItems.Sum(item => item.SizeBytes);
        var confirmed = _messageDialog.ConfirmWarning(
            $"将把 {selectedItems.Count:N0} 个文件移入回收站，合计 {FileSizeFormatter.Format(totalSize)}。\n\n是否继续？",
            "确认删除");

        if (!confirmed)
        {
            return;
        }

        var result = _fileDelete.MoveToRecycleBin(selectedItems);
        foreach (var item in result.DeletedFiles)
        {
            Files.Remove(item);
        }

        UpdateStatus();
        RefreshCommandStates();

        if (result.FailedMessages.Count > 0)
        {
            _messageDialog.ShowWarning($"已删除 {result.DeletedCount:N0} 个文件，{result.FailedMessages.Count:N0} 个文件删除失败。\n\n{string.Join("\n", result.FailedMessages.Take(8))}", "部分失败");
        }
        else
        {
            _messageDialog.ShowInfo($"已将 {result.DeletedCount:N0} 个文件移入回收站。", "删除完成");
        }
    }

    /// <summary>
    /// 在资源管理器中打开并选中当前列表选中的文件。
    /// </summary>
    private void OpenSelectedFolder()
    {
        if (SelectedFile is null)
        {
            _messageDialog.ShowInfo("请先在列表中选择一个文件。", "未选择文件");
            return;
        }

        _fileExplorer.SelectFile(SelectedFile);
    }

    /// <summary>
    /// 根据当前文件列表重新计算状态栏中的数量和总大小。
    /// </summary>
    private void UpdateStatus()
    {
        var total = Files.Sum(item => item.SizeBytes);
        StatusText = Files.Count == 0
            ? "没有扫描结果。请选择目录后开始扫描。"
            : $"共找到 {Files.Count:N0} 个文件，合计 {FileSizeFormatter.Format(total)}。";
    }

    /// <summary>
    /// 刷新所有命令的可用状态，确保按钮启用状态与当前页面状态一致。
    /// </summary>
    private void RefreshCommandStates()
    {
        BrowseCommand.NotifyCanExecuteChanged();
        ScanCommand.NotifyCanExecuteChanged();
        CancelCommand.NotifyCanExecuteChanged();
        SelectAllCommand.NotifyCanExecuteChanged();
        ClearSelectionCommand.NotifyCanExecuteChanged();
        DeleteSelectedCommand.NotifyCanExecuteChanged();
        OpenFolderCommand.NotifyCanExecuteChanged();
    }
}
