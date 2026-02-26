# v4.x vs v3.x 完整审计报告

## 3.1 功能对比表

| 功能 | v3.x 状态 | v4.x 状态 | 备注 |
|------|-----------|-----------|------|
| **文件操作** ||||
| 新建 | ✅ 支持 | ✅ 支持 | |
| 打开 | ✅ 支持 | ✅ 支持 | v4.x 使用现代文件选择器 |
| 保存 | ✅ 支持 | ✅ 支持 | |
| 命令行参数打开 | ✅ 支持 | ❌ 缺失 | v3.x 支持拖拽文件打开，v4.x 仅支持拖放 |
| **编辑器功能** ||||
| 剪切/复制/粘贴 | ✅ 支持 | ✅ 支持 | |
| 撤销/重做 | ✅ 支持 | ✅ 支持 | |
| 全选 | ✅ 支持 | ✅ 支持 | |
| 字数统计 | ✅ 支持 | ✅ 支持 | |
| 语法高亮 | ✅ AvalonEdit | ✅ AvaloniaEdit | |
| 字体设置 | ✅ 支持 | ✅ 支持 | |
| **播报控制** ||||
| 开始播报 | ✅ 支持 | ✅ 支持 | |
| 停止播报 | ✅ 支持 | ✅ 支持 | |
| 暂停/恢复 | ✅ 支持 | ✅ 支持 | |
| 报下一个 | ✅ 支持 | ✅ 支持 | |
| 报上一个 | ✅ 支持 | ✅ 支持 | |
| 再报一遍 | ✅ 支持 | ✅ 支持 | |
| 记录归零 | ✅ 支持 | ✅ 支持 | |
| **自动播报** ||||
| 自动播报间隔 | ✅ 表达式/固定 | ✅ 表达式/固定 | |
| 自动播报次数/词 | ✅ 支持 | ✅ 支持 | |
| 等待时间显示 | ✅ 支持 | ✅ 支持 | v4.x 显示倒计时 |
| **右键菜单** ||||
| 读选定词语 | ✅ 支持 | ✅ 支持 | |
| Bing词典查看 | ✅ 支持 | ✅ 支持 | |
| 从此处开始自动播报 | ✅ 支持 | ✅ 支持 | |
| **保存音频** ||||
| WAV格式 | ✅ 支持 | ✅ 支持 | |
| MP3格式 | ✅ 支持 | ✅ 支持 | |
| Opus格式 | ✅ 支持 | ✅ 支持 | |
| LRC歌词 | ✅ 支持 | ✅ 支持 | |
| **偏好设置** ||||
| 字体选择 | ✅ 支持 | ✅ 支持 | |
| 默认中文引擎 | ✅ 支持 | ✅ 支持 | |
| 默认英文引擎 | ✅ 支持 | ✅ 支持 | |
| 音源增强目录 | ✅ 支持 | ✅ 支持 | ImprovedVoice 支持 |
| **语音引擎** ||||
| Windows SAPI | ✅ 原生 COM | ✅ 原生 COM | v4.x 有平台检测 |
| Edge TTS | ❌ 缺失 | ✅ 支持 | v4.x 新增 |
| espeak-ng | ❌ 缺失 | ✅ 支持 | v4.x 新增 |
| pico2wave | ❌ 缺失 | ✅ 支持 | v4.x 新增 |
| Mac Say | ❌ 缺失 | ✅ 支持 | v4.x 新增 |
| ImprovedVoice | ✅ 支持 | ✅ 支持 | 音频文件增强 |
| **音频播放** ||||
| SDL2 播放 | ✅ 支持 | ✅ 支持 | v4.x 使用 Hexa.NET.SDL2 |
| BASS 解码 | ✅ 支持 | ✅ 支持 | 用于音频导出解码 |
| **等待时间计算** ||||
| 固定等待时间 | ✅ 支持 | ✅ 支持 | |
| 表达式等待时间 | ✅ 支持 | ✅ 支持 | 支持 length, wordcount 等 |
| **UI功能** ||||
| 自动翻页 | ✅ 支持 | ✅ 支持 | |
| 高亮跟随 | ✅ 支持 | ✅ 支持 | |
| 音量控制 | ✅ 支持 | ✅ 支持 | |
| 语速控制 | ✅ 支持 | ✅ 支持 | |
| 窗口大小保存 | ✅ 部分 | ✅ 完整 | v4.x 保存位置和窗口状态 |
| **关于对话框** | ✅ 支持 | ✅ 支持 | |

## 3.2 行为差异

### 3.2.1 窗口管理
- **v3.x**: 窗口大小通过 WPF Properties.Settings 保存，仅保存 Width/Height
- **v4.x**: 完整保存窗口位置 (X/Y)、大小、窗口状态 (Normal/Maximized/Minimized)

### 3.2.2 文件拖放
- **v3.x**: 支持通过命令行参数直接打开文件 (e.g., `DictationAssistant.exe wordlist.txt`)
- **v4.x**: 支持拖放文件到窗口打开，但不支持命令行参数

### 3.2.3 播报状态显示
- **v3.x**: 显示"即将播报第X个"
- **v4.x**: 额外显示倒计时 "即将播报第X个 (X秒后)"

### 3.2.4 音频导出进度
- **v3.x**: 使用 Windows Forms ProgressDialog 显示进度
- **v4.x**: 在对话框内显示进度百分比

### 3.2.5 错误处理
- **v3.x**: 静默失败，部分错误弹窗
- **v4.x**: 所有错误写入 Status 状态栏，更友好

### 3.2.6 语音切换逻辑
- **v3.x**: 使用语言代码 "CHS"/"CHT"/"ENG"/"ENU"
- **v4.x**: 使用区域前缀匹配 "zh"/"en"

## 3.3 v3.x 特性缺失 (v4.x)

### 3.3.1 功能缺失
1. **命令行文件参数**: v3.x 支持 `DictationAssistant.exe wordlist.txt` 直接打开
2. **计算绑定 (CalcBinding)**: v3.x 使用 CalcBinding 实现复杂 UI 绑定，v4.x 用 ViewModel 属性替代

### 3.3.2 细节差异
1. **v3.x 工具栏按钮**: v3.x 有垂直工具栏 (新建/打开/保存/剪切/复制/粘贴/删除/新建/计数)
   - v4.x 仅有右侧小图标按钮 (缺少部分如"删除"按钮在工具栏)
2. **BASS 插件加载**: v3.x 在启动时加载 bass_plugin 目录，v4.x 未完整实现
3. **进度对话框取消**: v3.x 支持取消导出，v4.x 需确认

## 3.4 v4.x 改进

### 3.4.1 跨平台支持
- ✅ Windows/Linux/macOS
- ✅ 多语音引擎支持 (EdgeTTS, espeak-ng, pico2wave, MacSay)

### 3.4.2 架构改进
- ✅ 现代化异步架构 (async/await)
- ✅ 依赖注入模式
- ✅ CommunityToolkit.Mvvm MVVM 框架
- ✅ 更好的代码组织 (Abstractions/Services/ViewModels 分离)

### 3.4.3 音频改进
- ✅ Hexa.NET.SDL2 库 (NuGet) 替代原始 DllImport
- ✅ ManagedBass 库进行音频解码
- ✅ FFmpeg 音频编码器封装

### 3.4.4 UI 改进
- ✅ Avalonia 现代化 UI 框架
- ✅ 完整窗口状态持久化
- ✅ 实时倒计时显示

### 3.4.5 设置持久化
- ✅ JSON 文件格式 (appsettings.json)
- ✅ 更好的默认值处理

## 3.5 代码质量问题

### v3.x 问题
1. **死代码**: `LanguageSpecificStringConverter.cs` - 未在 XAML 中使用
2. **平台限制**: 大量 Windows 特定代码 (WinForms 对话框, COM)
3. **混用架构**: WPF + WinForms 对话框混用
4. **原始 DllImport**: 直接使用 SDL2.dll/bass.dll，无 NuGet 包装
5. **Threading**: 使用 `System.Timers.Timer` 而非 `System.Threading.Timer`

### v4.x 问题Missing Bass 插件加载
1. ****: AudioExporter 中未实现 BASS 插件加载 (v3.x 在 App.xaml.cs)
2. **Null 处理**: `ExpressionBasedWaitingTime` 和 `FixedWaitingTime` 需验证
3. **硬编码值**: `EditorFontFamily = "Noto Sans CJK SC"` 是硬编码默认值
4. **平台检测**: 部分语音引擎无平台检测 (EspeakNgVoice, Pico2WaveVoice)

### 通用问题
1. **ImprovedVoice**: 两个版本实现略有差异，v4.x 使用 `IWordListSource`，v3.x 使用 `IWordListAdapter`
2. **音频导出**: v4.x 目前仅支持 44100Hz/立体声固定输出

## 3.6 推荐下一步

### 高优先级
1. **实现命令行文件参数支持**: 在 v4.x 中添加启动参数处理
2. **完善 BASS 插件加载**: 在 App 启动时加载 bass_plugin
3. **实现音频格式选项**: 导出时允许选择采样率和声道数 (当前硬编码)

### 中优先级
1. **添加工具栏删除按钮**: 右侧按钮栏增加删除功能
2. **改进错误处理**: 统一错误报告机制
3. **完善平台检测**: 为 espeak-ng/pico2wave 添加平台检测

### 低优先级
1. **性能优化**: 预加载下一词语音
2. **缓存改进**: CachedVoice 实现可配置缓存
3. **国际化**: 提取 UI 字符串到资源文件
