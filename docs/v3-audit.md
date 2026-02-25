# DictationAssistant v3 (WPF) 代码审计文档

本文档详细记录了 DictationAssistant v3 的 UI、特性、行为，用于验证 v4 的功能完整性。

---

## 1. Window Inventory

### 1.1 MainWindow

**基本信息：**
- **Title**: "自动默写"
- **Dimensions**: Height=600, Width=800
- **Startup Location**: Default (CenterScreen)
- **Icon**: DictationAssistant.ico
- **AllowDrop**: True (支持拖拽文件)
- **Shutting Down**: OnMainWindowClose

### 1.2 PreferenceWindow

**基本信息：**
- **Title**: "偏好设置"
- **Dimensions**: Height=450, Width=800

### 1.3 AboutWindow

**基本信息：**
- **Title**: "AboutWindow"
- **Dimensions**: Height=450, Width=800

### 1.4 SaveAudioDialog

**基本信息：**
- **Title**: "SaveAudioDialog"
- **Dimensions**: Height=240, Width=300

---

## 2. Main Window Detailed Layout

### 2.1 Menu Bar

位于顶部，结构如下：

| 菜单 | 菜单项 | 快捷键 | 处理器 |
|------|--------|--------|--------|
| 文件(_F) | 新建(_N)... | Ctrl+N | OnNewExecuted |
| 文件(_F) | 打开(_O)... | Ctrl+O | OnOpenExecuted |
| 文件(_F) | 保存(_S)... | Ctrl+S | OnSaveExecuted |
| 文件(_F) | 退出(_E) | - | ExitMenuItem_Click |
| 编辑(_E) | 显示/隐藏词语列表 | - | ShowOrHideWordlist_Click |
| 编辑(_E) | 剪切 | Ctrl+X | 绑定到 WordlistEditor |
| 编辑(_E) | 复制 | Ctrl+C | 绑定到 WordlistEditor |
| 编辑(_E) | 粘贴 | Ctrl+V | 绑定到 WordlistEditor |
| 编辑(_E) | 撤销 | Ctrl+Z | 绑定到 WordlistEditor |
| 编辑(_E) | 重做 | Ctrl+Y | 绑定到 WordlistEditor |
| 编辑(_E) | 全选 | Ctrl+A | 绑定到 WordlistEditor |
| 选项(_O) | 偏好设置(_P) | - | PreferenceMenu_Click |
| 选项(_O) | 自动翻页 | - | Checkbox (默认勾选) |
| 选项(_O) | 高亮跟随 | - | Checkbox (默认勾选) |
| 辅助(_A) | 保存音频 | - | SaveAudioMenu_Click |
| 帮助(_H) | 关于(_A) | - | AboutMenu_Click |

### 2.2 Right-Side Control Panel (宽度 264)

#### 2.2.1 播报信息 GroupBox

| 控件 | 类型 | 数据绑定 | 描述 |
|------|------|----------|------|
| 本词语播报次数 | Label | `ElapsedTimes` | 显示当前词语已播报次数 |
| 自动播报间隔/秒 | TextBox | `CurrentWaitingTimeCalculator` | 等待时间计算器，支持表达式 |
| 自动播报次数/词 | TextBox | `AutoMode_TimesPerWord` | 每个词播报的次数 |
| 状态显示 | Label | 多重数据绑定 | 显示自动播报状态/暂停/播报中/等待播报 |

#### 2.2.2 引擎设置 GroupBox

| 控件 | 类型 | 数据绑定 | 描述 |
|------|------|----------|------|
| 音量 | Slider | `Volume` | 范围 0-100，步长 5 |
| 语速 | Slider | `Rate` | 范围 -10 到 10，步长 1 |
| 引擎 | ComboBox | `BaseVoice` | 显示可用语音引擎 |
| 中文引擎 | Button | - | SwitchToChineseVoice_Click |
| 英文引擎 | Button | - | SwitchToEnglishVoice_Click |

#### 2.2.3 播报控制 GroupBox

| 控件 | 类型 | 描述 |
|------|------|------|
| 开始播报(_S) | Button | StartSpeakingButton_Click |
| 停止播报(_D) | Button | StopSpeakingButton_Click |
| 暂停/恢复自动播报 | Button | PauseSpeakingButton_Click (动态文本) |
| 报下一个(_N) | Button | SpeakNextButton_Click |
| 记录归零(_C) | Button | ResetRecordButton_Click |
| 再报一遍(_M) | Button | SpeakButton_Click |
| 报上一个(_L) | Button | SpeakPreviousButton_Click |

### 2.3 Word List Area

使用 AvalonEdit 文本编辑器：

| 属性 | 值 |
|------|-----|
| Name | WordlistEditor |
| FontSize | 28 |
| ShowLineNumbers | True |
| FlowDirection | LeftToRight |
| Encoding | UTF-8 |
| 语法高亮 | 自定义 XML 定义 (WordlistHighlighting) |

#### Toolbar (Vertical, 右侧)

| 按钮 | 图像资源 | 命令绑定 |
|------|----------|----------|
| 打开 | Icon_Open | Open |
| 保存 | Icon_Save | Save |
| 剪切 | Icon_Cut | Cut (绑定到 WordlistEditor) |
| 复制 | Icon_Copy | Copy (绑定到 WordlistEditor) |
| 粘贴 | Icon_Paste | Paste (绑定到 WordlistEditor) |
| 删除 | Icon_Delete | Delete (绑定到 WordlistEditor) |
| 新建 | Icon_Clear | New |
| 计数 | Icon_Count | CountButton_Click |

#### Context Menu (右键菜单)

| 菜单项 | 处理器 |
|--------|--------|
| 读选定词语 | SpeakSelectionMenu_Click |
| 在Bing词典中查看 | ViewSelectionInBingDictionaryMenu_Click |
| 从此处开始自动播报 | StartFromSelectionMenu_Click |
| 剪切 | Command: Cut |
| 复制 | Command: Copy |
| 粘贴 | Command: Paste |
| 撤销 | Command: Undo |
| 重做 | Command: Redo |
| 全选 | Command: SelectAll |

### 2.4 Status Bar / Info Display

- **显示/隐藏按钮**: 底部按钮，根据 TabControl 的 SelectedIndex 动态切换文本
  - "隐藏词语列表(_W)" / "显示词语列表(_W)"

---

## 3. PreferenceWindow

### 3.1 设置组

#### 3.1.1 编辑器 GroupBox

| 控件 | 类型 | 数据绑定 | 描述 |
|------|------|----------|------|
| 字体 | FontPicker | `EditorFontInfo` | 使用 QIQI.WpfFontPicker 库 |

#### 3.1.2 引擎 GroupBox

| 控件 | 类型 | 数据绑定 | 描述 |
|------|------|----------|------|
| 默认中文引擎 | ComboBox | `DefaultChineseVoiceName` | 从可用语音列表选择 |
| 默认英文引擎 | ComboBox | `DefaultEnglishVoiceName` | 从可用语音列表选择 |
| 音源增强目录 | TextBox + Button | `PathForImprovedResource` | 增强语音文件目录，带文件夹验证 |

**验证规则：**
- `FolderValidationRule`: 验证目录是否存在，允许为空 (Emptiable=true)

---

## 4. SaveAudioDialog

### 4.1 参数选项

| 参数 | 控件类型 | 数据绑定 | 默认值 | 可选值 |
|------|----------|----------|--------|--------|
| 声道 | ComboBox | `Channels` | 2 (Stereo) | Mono(1), Stereo(2) |
| 位宽 | ComboBox | `SampleFormat` | Signed 16bit | Unsigned 8bit, Signed 16bit |
| 采样率 | ComboBox (可编辑) | `Freq` | 44100 | 6000, 7333, 8000, 11025, 16000, 22050, 24000, 32000, 44100, 48000 |
| 输出格式 | ComboBox | `EncoderInfo` | MPEG Audio Layer 3 (mp3) | 见 4.2 |
| 字幕模式 | ComboBox | `LyricType` | Lrc File | Dismiss, Lrc File |
| 目标文件 | TextBox + Button | `TargetPath` | - | 文件保存路径 |

### 4.2 支持的输出格式及编码器配置

| 格式名称 | 扩展名 | 编码器 | 参数 |
|----------|--------|--------|------|
| Waveform Audio | wav | WaveEncoder | 无 (内置) |
| MPEG Audio Layer 3 | mp3 | lame.exe | `--ta 自动默写 -V0 --ignorelength --quiet - "{0}"` |
| Opus Audio | opus | opusenc.exe | `--artist 自动默写 --ignorelength --quiet - "{0}"` |

**编码器路径**: `{AppDomain.CurrentDomain.SetupInformation.ApplicationBase}/encoder/`

### 4.3 歌词文件 (LRC)

- **生成器**: `LyricWriter`
- **ID Tags**: 
  - `ar`: "自动默写"
  - `by`: "自动默写"
- **格式**: 标准 LRC 格式，带毫秒级时间戳

---

## 5. Keyboard Shortcuts

### 5.1 Menu 快捷键 (Access Keys)

使用 WPF 标准的 Alt+Key 访问键，格式为 `_(字符)`：

| 菜单 | 访问键 | 完整快捷键 |
|------|--------|------------|
| 文件(_F) | Alt+F | - |
| 新建(_N) | Alt+F, N | Ctrl+N |
| 打开(_O) | Alt+F, O | Ctrl+O |
| 保存(_S) | Alt+F, S | Ctrl+S |
| 退出(_E) | Alt+F, E | - |
| 编辑(_E) | Alt+E | - |
| 选项(_O) | Alt+O | - |
| 偏好设置(_P) | Alt+O, P | - |
| 辅助(_A) | Alt+A | - |
| 保存音频 | Alt+A, S | - |
| 帮助(_H) | Alt+H | - |
| 关于(_A) | Alt+H, A | - |

### 5.2 编辑器命令绑定

| 命令 | 快捷键 | 绑定目标 |
|------|--------|----------|
| New | Ctrl+N | CommandBinding |
| Open | Ctrl+O | CommandBinding |
| Save | Ctrl+S | CommandBinding |
| Cut | Ctrl+X | WordlistEditor TextArea |
| Copy | Ctrl+C | WordlistEditor TextArea |
| Paste | Ctrl+V | WordlistEditor TextArea |
| Undo | Ctrl+Z | WordlistEditor TextArea |
| Redo | Ctrl+Y | WordlistEditor TextArea |
| SelectAll | Ctrl+A | WordlistEditor TextArea |
| Delete | (无默认) | WordlistEditor TextArea |

### 5.3 自定义按钮快捷键 (通过按钮文本)

| 按钮 | 文本快捷键 | 功能 |
|------|------------|------|
| 开始播报 | Alt+S | 启动自动播报 |
| 停止播报 | Alt+D | 停止播报 |
| 暂停自动播报/恢复自动播报 | Alt+P/Alt+R | 暂停/恢复自动播报 |
| 报下一个 | Alt+N | 播报下一个词 |
| 记录归零 | Alt+C | 重置进度 |
| 再报一遍 | Alt+M | 重新播报当前词 |
| 报上一个 | Alt+L | 播报上一个词 |
| 显示/隐藏词语列表 | Alt+W | 切换词表显示 |

---

## 6. Core Features & Behavior

### 6.1 Dictation Playback Modes

#### 6.1.1 Auto Mode (自动模式)

- **启动**: `StartAuto(int index = 0)` - 从指定索引开始自动播报
- **流程**:
  1. 设置 `AutoMode = true`，启动 1 秒间隔计时器
  2. 播报当前词
  3. 播放完成后触发 `PlayCompleted` 事件
  4. 增加 `ElapsedTimes` 计数
  5. 如果 `ElapsedTimes >= AutoMode_TimesPerWord`，进入等待状态
  6. 等待 `GetWaitingTime(word)` 秒后，移动到下一个词
  7. 重复直到所有词播报完成

#### 6.1.2 Manual Mode (手动模式)

- **报下一个**: `SpeakNext()` - 播报下一个词
- **再报一遍**: `SpeakAgain()` - 重新播报当前词
- **报上一个**: `SpeakPrevious()` - 播报上一个词
- **停止**: `StopThis()` - 停止当前播放

#### 6.1.3 Pause/Resume

- **暂停**: `PauseAuto()` - 停止计时器，停止当前播放，设置 `IsPausedAutoMode = true`
- **恢复**: `ResumeAuto()` - 重新启动计时器，设置 `IsPausedAutoMode = false`

### 6.2 ImprovedVoice (增强语音)

**工作原理**:

```csharp
public ImprovedVoice(IVoice voiceForNoFile, string resForImproving)
```

1. **文件查找**: 对于每个要播报的词，构造文件路径：
   - 清理文件名中的非法字符: `\ / : * ? " < > |`
   - 在 `resForImproving` 目录中查找文件
   - 支持扩展名: `wav, flac, ape, m4a, opus, aac, mp3, mp2, mp1, ogg, wma, aif, mp4`

2. **Fallback**: 如果找不到文件或发生异常，使用 `voiceForNoFile` (MicrosoftSpeechVoice)

### 6.3 Waiting Time Calculation

#### 6.3.1 FixedWaitingTime

```csharp
public FixedWaitingTime(int seconds) // 默认值: 3 秒
```

返回固定的秒数。

#### 6.3.2 ExpressionBasedWaitingTime

支持表达式语法 (使用 NCalc 库)：

**内置参数**:
- `length` - 词字符串长度
- `wordcount` - 英文单词数 (使用 `WaitingTimeHelper.GetWordCount`)
- `text` - 原始文本

**内置函数**:
- `Length(string)` - 字符串长度
- `WordCount(string)` - 英文单词数
- `RegexReplace(input, pattern, replacement, [ignoreCase])` - 正则替换
- `RegexMatch(input, pattern, [ignoreCase])` - 正则匹配

**示例**:
- `3` - 固定 3 秒
- `length * 0.5 + 1` - 长度 * 0.5 + 1 秒
- `wordcount * 2` - 每词 2 秒

### 6.4 Word List Adapter

**接口**: `IWordListAdapter`

```csharp
public interface IWordListAdapter
{
    string this[int index] { get; }
    int Length { get; }
}
```

**实现**: `WordListAdapterForAvalonEdit`

- 封装 `TextEditor` 控件
- 通过 `Dispatcher` 线程安全访问文档
- `this[int index]` - 获取指定行的文本 (索引从 0 开始)
- `Length` - 返回总行数

### 6.5 Settings Persistence

**存储机制**: .NET Application Settings (user.config)

**文件**: `Properties/Settings.Designer.cs`

**持久化设置**:

| 设置项 | 类型 | 默认值 | 描述 |
|--------|------|--------|------|
| Voice | string | "" | 当前选中的语音名称 |
| PathForImprovedResource | string | "" | 增强语音文件目录 |
| DefaultChineseVoiceName | string | "" | 默认中文语音 |
| DefaultEnglishVoiceName | string | "" | 默认英文语音 |
| FontInfo | PackedFontInfo (Binary) | null | 编辑器字体设置 |

**保存时机**: `MainWindow.Window_Closed` 事件

---

## 7. TTS / Audio Architecture

### 7.1 Voice System

#### 7.1.1 IVoice Interface

```csharp
public interface IVoice
{
    PcmStreamWithInfo Speak(string text, SpeakParam param);
    string Name { get; }
    CultureInfo Culture { get; }
}
```

#### 7.1.2 SpeakParam

```csharp
public class SpeakParam
{
    public sbyte Rate = 0; // -10 ~ 20
}
```

#### 7.1.3 Implementations

| 类 | 描述 |
|----|------|
| MicrosoftSpeechVoice | Windows SAPI 语音合成，输出 44.1kHz 16bit Stereo PCM |
| ImprovedVoice | 文件查找 + Fallback 到 MicrosoftSpeechVoice |
| NullVoice | 静默实现，返回空 PCM 流 |

#### 7.1.4 MicrosoftSpeechVoice

**语音来源** (注册表):
- `HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Speech\Voices`
- `HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Speech Server\v11.0\Voices`
- `HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Speech_OneCore\Voices`

**输出格式**:
- 采样率: 44100 Hz
- 声道: 2 (Stereo)
- 位宽: 16-bit

**处理**:
- 移除前后 200ms 的静音片段

### 7.2 Audio Pipeline

#### 7.2.1 ISpeaker / ISpeakStateControler

```csharp
public interface ISpeaker
{
    ISpeakStateControler Speak(string text);
}

public interface ISpeakStateControler
{
    void StopSpeak();
    event EventHandler PlayCompleted;
}
```

#### 7.2.2 Speaker

- 持有 `Volume`, `Rate`, `Voice` 属性
- `Speak()` 方法创建 `PcmPlayer` 并播放
- 内部类 `SpeakStateControler` 管理播放状态

#### 7.2.3 PcmPlayer

**音频播放**: 使用 SDL2.dll

**功能**:
- 打开音频设备: `SDL_OpenAudioDevice`
- 播放/暂停: `SDL_PauseAudioDevice`
- 混音: `SDL_MixAudioFormat`
- 音量控制: `SdlVolume = volume * 128 / 100`

**播放完成**: 触发 `PlayCompleted` 事件

#### 7.2.4 BASS Decoder (音频文件解码)

**用途**: ImprovedVoice 读取音频文件

**支持的格式** (通过 bass_plugin):
- wav, flac, ape, m4a, opus, aac, mp3, mp2, mp1, ogg, wma, aif, mp4

**API**:
- `BASS_StreamCreateFile` - 创建流
- `BASS_ChannelGetData` - 获取 PCM 数据
- `BASS_StreamFree` - 释放流

**初始化** (App.OnStartup):
```csharp
BASS.Init(0, 44100, 0, IntPtr.Zero)
// 加载 bass_plugin 目录下的所有插件
```

### 7.3 PCM Format

#### 7.3.1 PcmFormatInfo

```csharp
public class PcmFormatInfo
{
    public int Freq;        // 采样率
    public byte Channels;   // 声道数
    public PcmSampleFormat SampleFormat; // 采样格式
}
```

#### 7.3.2 PcmSampleFormat

```csharp
public enum PcmSampleFormat : ushort
{
    U8 = 0x8,     // Unsigned 8-bit
    S16 = 0x8010  // Signed 16-bit (Little Endian)
}
```

### 7.4 Supported Audio Formats for ImprovedVoice

| 扩展名 | 描述 |
|--------|------|
| wav | PCM Wave |
| flac | Free Lossless Audio Codec |
| ape | Monkey's Audio |
| m4a | MPEG-4 Audio |
| opus | Opus Audio |
| aac | Advanced Audio Coding |
| mp3 | MPEG-1/2 Audio Layer 3 |
| mp2 | MPEG-1/2 Audio Layer 2 |
| mp1 | MPEG-1 Audio Layer 1 |
| ogg | Ogg Vorbis |
| wma | Windows Media Audio |
| aif | Audio Interchange File Format |
| mp4 | MPEG-4 Part 14 |

### 7.5 Supported Export Formats

#### 7.5.1 WaveEncoder (wav)

- 输出: 未压缩 PCM Wave 文件
- Wave Header 自动更新文件大小

#### 7.5.2 External Audio Encoders

**MP3 (lame.exe)**:
- 参数: `-V0 --ignorelength --quiet`
- 比特率: V0 (约 190-210 kbps)

**Opus (opusenc.exe)**:
- 参数: `--artist 自动默写 --ignorelength --quiet`
- 默认 Opus 设置

#### 7.5.3 PcmWriter

**功能**:
- 格式转换 (采样率、声道、位宽)
- 使用 SDL2 AudioCVT 进行重采样
- 支持写入静音片段 (用于词间间隔)

---

## 8. Additional Components

### 8.1 HighlightedLineBackgroundRenderer

- 实现 `IBackgroundRenderer`
- 在当前播报行显示高亮背景 (LightGreen)
- 通过 `LineNumber` 属性控制

### 8.2 ExpressionBasedWaitingTimeValueConverter

- XAML 值转换器
- `Convert`: IWaitingTimeCalculator -> string
- `ConvertBack`: string -> ExpressionBasedWaitingTime

### 8.3 ExpressionBasedWaitingTimeValidationRule

- XAML 验证规则
- 验证表达式语法有效性

---

## 9. 第三方依赖

| 库 | 用途 |
|----|------|
| AvalonEdit | 文本编辑器 |
| CalcBinding | 计算绑定表达式 |
| NCalc | 表达式求值 |
| QIQI.WpfFontPicker | 字体选择器 |
| BetterFolderBrowser | 文件夹浏览对话框 |
| SDL2.dll | 音频播放 |
| bass.dll + bass_plugin/* | 音频解码 |
| lame.exe | MP3 编码 |
| opusenc.exe | Opus 编码 |
| SpeechLib (COM) | Windows 语音合成 |

---

*本文档基于 DictationAssistant v3 (WPF) 源代码审计生成。*
