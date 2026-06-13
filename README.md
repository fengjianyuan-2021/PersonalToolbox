# PersonalToolbox / 个人工具箱

[中文说明](#中文说明) | [English](#english)

## 中文说明

PersonalToolbox 是一个基于 WPF 的 Windows 桌面工具箱项目，用来把常用的本地效率工具集中到一个统一客户端中。当前版本已经实现菜单首页和大文件清理工具，后续可以继续接入更多工具页面。

### 当前功能

- 菜单首页：集中展示已经开发好的工具入口。
- 大文件清理：选择目录后扫描大文件，并按文件大小展示结果。
- 删除保护：勾选文件后先二次确认，再移入 Windows 回收站。
- 后台扫描：扫描过程不会阻塞界面，并支持停止扫描。
- MVVM 结构：使用 `Views`、`ViewModels`、`Models`、`Services`、`Core` 分层。
- 中文代码注释：类、字段、属性、构造函数和方法均按项目规范补充说明。

### 技术栈

- .NET 8
- WPF
- C#
- MVVM
- Git / GitHub CLI

### 项目结构

```text
PersonalToolbox/
  Core/          MVVM 基础类和命令
  Models/        页面数据模型
  Services/      文件扫描、删除、弹窗、资源管理器等系统服务
  ViewModels/    页面状态和命令逻辑
  Views/         WPF 页面视图
  App.xaml       应用资源
  MainWindow.*   主窗口和导航 Shell
```

### 构建

```powershell
dotnet build
```

### 发布 Windows x64 版本

```powershell
dotnet publish -c Release -r win-x64 --self-contained false
```

发布后的可执行文件位于：

```text
bin/Release/net8.0-windows/win-x64/publish/PersonalToolbox.exe
```

### 使用方式

1. 启动 `PersonalToolbox.exe`。
2. 在左侧菜单或首页选择“大文件清理”。
3. 选择要扫描的目录。
4. 设置最小文件大小，默认 `100 MB`。
5. 点击“开始扫描”。
6. 勾选要删除的文件。
7. 点击“删除选中文件”，确认后文件会移入回收站。

### 后续扩展

新增工具时建议遵守现有 MVVM 结构：

1. 在 `Views/` 新增页面。
2. 在 `ViewModels/` 新增页面 ViewModel。
3. 把系统操作放到 `Services/`。
4. 在 `MainViewModel` 注册新的 `ToolNavigationItem`。

## English

PersonalToolbox is a WPF-based Windows desktop toolbox for collecting local productivity utilities in one client application. The current version includes a home menu and a large file cleanup tool, with a structure ready for more tools.

### Features

- Home menu: shows available tools in one place.
- Large file cleanup: scans a selected folder and lists large files by size.
- Safer deletion: selected files are confirmed first, then moved to the Windows Recycle Bin.
- Responsive scanning: long-running scans run in the background and can be cancelled.
- MVVM architecture: organized into `Views`, `ViewModels`, `Models`, `Services`, and `Core`.
- Chinese developer comments: classes, fields, properties, constructors, and methods are documented according to the project convention.

### Tech Stack

- .NET 8
- WPF
- C#
- MVVM
- Git / GitHub CLI

### Project Structure

```text
PersonalToolbox/
  Core/          MVVM base classes and commands
  Models/        Page data models
  Services/      File scanning, deletion, dialogs, Explorer integration
  ViewModels/    Page state and command logic
  Views/         WPF page views
  App.xaml       Application resources
  MainWindow.*   Main shell and navigation
```

### Build

```powershell
dotnet build
```

### Publish for Windows x64

```powershell
dotnet publish -c Release -r win-x64 --self-contained false
```

The published executable is generated at:

```text
bin/Release/net8.0-windows/win-x64/publish/PersonalToolbox.exe
```

### Usage

1. Launch `PersonalToolbox.exe`.
2. Open the “Large File Cleanup” tool from the side menu or home page.
3. Choose a folder to scan.
4. Set the minimum file size. The default is `100 MB`.
5. Click “Start Scan”.
6. Select files to delete.
7. Click “Delete Selected Files”; confirmed files are moved to the Recycle Bin.

### Extending

To add a new tool:

1. Add a view under `Views/`.
2. Add a ViewModel under `ViewModels/`.
3. Put system or external operations under `Services/`.
4. Register a new `ToolNavigationItem` in `MainViewModel`.
