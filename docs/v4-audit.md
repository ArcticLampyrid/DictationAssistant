# DictationAssistant v4 (Avalonia) 代码审计文档

本文档详细记录了 DictationAssistant v4 的架构设计、核心接口、音频处理流程及与 v3 的主要差异。

---

## 1. 项目架构

### 1.1 整体架构

v4 版本采用跨平台架构，使用 Avalonia UI 框架实现多平台支持：

```
DictationAssistant/
├── DictationAssistant.Core/        # 核心业务逻辑（跨平台）
│   ├── Abstractions/              # 接口定义
│   ├── Audio/                     # 音频数据结构
│   ├── Models/                    # 数据模型
│   ├── Services/                  # 核心服务
│   └── Helpers/                    # 辅助工具
└── DictationAssistant.App/         # 平台特定实现
    ├── Services/
    │   ├── Audio/                  # 音频播放/解码
    │   ├── Voice/                  # 语音合成实现
    │   └── Settings/               # 设置管理
    └── ViewModels/                 # MVVM ViewModels
```

### 1.2 模块划分

| 模块 | 职责 |
|------|------|
| **Core.Abstractions** | 定义核心接口（IVoice, IAudioPlayer, IDictationPlayer 等） |
| **Core.Audio** | PCM 音频数据结构（PcmAudio, PcmFormatInfo, PcmSampleFormat） |
| **Core.Models** | 数据模型（DictationSettings, DictationState, VoiceInfo 等） |
| **Core.Services** | 核心业务逻辑（DictationPlayer, CachedVoice, NullVoice） |
| **App.Services.Audio** | 平台特定的音频实现（SDL 播放, BASS 解码） |
| **App.Services.Voice** | 平台特定的语音实现（Edge TTS, SAPI, ImprovedVoice 等） |
| **App.ViewModels** | MVVM 视图模型 |

---

## 2. 核心接口

### 2.1 IVoice

语音合成接口，负责将文本转换为 PCM 音频数据：

```csharp
public interface IVoice
{
    string Name { get; }

    Task<PcmAudio?> SynthesizePcmAsync(string text, VoiceSynthesisOptions options, CancellationToken ct);
}
```

**实现类：**

| 类 | 描述 |
|----|------|
| EdgeTtsVoice | Microsoft Edge TTS 语音合成 |
| SapiVoice | Windows SAPI 语音合成 |
| MacSayVoice | macOS say 命令封装 |
| Pico2WaveVoice | Pico TTS 封装（Linux） |
| EspeakNgVoice | eSpeak NG 封装（Linux） |
| ImprovedVoice | 增强语音（本地文件优先） |
| NullVoice | 空实现，返回空音频 |
| CachedVoice | 抽象基类，提供缓存功能 |

### 2.2 IAudioPlayer

音频播放接口：

```csharp
public interface IAudioPlayer
{
    Task PlayAsync(PcmAudio audio, int volume, CancellationToken ct);
}
```

**实现类：**

| 类 | 描述 |
|----|------|
| SdlPcmPlayer | 基于 SDL2 的音频播放实现 |

### 2.3 IDictationPlayer

听写播放器核心接口，管理自动/手动播报流程：

```csharp
public interface IDictationPlayer
{
    DictationState State { get; }
    DictationSettings Settings { get; }
    DictationProgress Progress { get; }
    IVoice Voice { get; set; }
    IWaitingTimeCalculator? WaitingTimeCalculator { get; set; }

    event EventHandler<DictationProgress>? ProgressChanged;
    event EventHandler<DictationState>? StateChanged;

    Task SpeakPreviousAsync(CancellationToken cancellationToken = default);
    Task SpeakAgainAsync(CancellationToken cancellationToken = default);
    Task SpeakNextAsync(CancellationToken cancellationToken = default);
    Task SpeakAtAsync(int index, CancellationToken cancellationToken = default);
    Task StartAutoAsync(int startIndex = 0, CancellationToken cancellationToken = default);
    void PauseAuto();
    void ResumeAuto();
    Task StopAsync();
    Task<SaveAudioResult> SaveAudioAsync(SaveAudioRequest request, CancellationToken cancellationToken = default);
}
```

### 2.4 IVoiceFactory / IVoiceFactoryProvider

语音工厂接口，用于动态创建语音实例：

```csharp
public interface IVoiceFactory
{
    VoiceInfo Info { get; }
    IVoice Create();
}

public interface IVoiceFactoryProvider
{
    Task<IReadOnlyList<IVoiceFactory>> GetFactoriesAsync(CancellationToken ct);
}
```

### 2.5 IWaitingTimeCalculator

等待时间计算器接口：

```csharp
public interface IWaitingTimeCalculator
{
    int CalculateWaitingTime(string word);
}
```

---

## 3. 音频处理

### 3.1 PcmAudio

v4 引入的全新流式音频数据结构：

```csharp
public sealed class PcmAudio : IDisposable
{
    public static PcmAudio Empty => new() 
    { 
        Data = Stream.Null, 
        Format = new PcmFormatInfo(44100, 2, PcmSampleFormat.S16LE) 
    };

    public required Stream Data { get; init; }
    public required PcmFormatInfo Format { get; init; }

    public byte[] ToArray();
    public void Dispose();
}
```

**关键特性：**
- 使用 `Stream` 替代 `byte[]`，支持流式处理
- `PcmFormatInfo` 存储采样率、声道数、采样格式
- `ToArray()` 方法支持按需转换为字节数组

### 3.2 PcmFormatInfo / PcmSampleFormat

```csharp
public class PcmFormatInfo
{
    public int SampleRate;    // 采样率
    public byte Channels;     // 声道数
    public PcmSampleFormat SampleFormat; // 采样格式
}

public enum PcmSampleFormat : ushort
{
    S16LE = 0x8010  // Signed 16-bit Little Endian
}
```

### 3.3 BassDecodeStream

基于 ManagedBass 的流式解码器：

```csharp
public sealed class BassDecodeStream : Stream
{
    public PcmFormatInfo Format { get; }

    public static BassDecodeStream? CreateFromFile(string filePath);
    public static BassDecodeStream? CreateFromStream(Stream inputStream);
}
```

**特点：**
- 继承 `Stream`，支持流式读取
- 按需解码，避免一次性加载整个文件到内存
- 支持从文件或 `Stream` 创建解码流

**支持的格式：**
- wav, flac, ape, m4a, opus, aac, mp3, mp2, mp1, ogg, wma, aif, mp4

### 3.4 SdlPcmPlayer

基于 SDL2 的音频播放实现：

```csharp
public sealed class SdlPcmPlayer : IAudioPlayer, IDisposable
{
    public Task PlayAsync(PcmAudio audio, int volume, CancellationToken ct);
}
```

**特性：**
- 支持 16-bit 有符号 PCM（S16LE）
- 内置音量控制（0-100）
- 异步播放，任务取消支持
- 使用 SDL2 的 `QueueAudio` 进行音频排队播放

### 3.5 音频处理流程

```
文本输入
    ↓
IVoice.SynthesizePcmAsync()
    ↓
┌─────────────────────────────────────────┐
│ EdgeTtsVoice / SapiVoice / ...         │
│ (生成 MP3/其他格式)                      │
└─────────────────────────────────────────┘
    ↓
BassAudioDecoder.DecodeStream()
    ↓
BassDecodeStream (流式解码为 PCM)
    ↓
PcmAudio (封装 Stream + Format)
    ↓
SdlPcmPlayer.PlayAsync()
    ↓
SDL2 音频设备播放
```

---

## 4. v3 → v4 迁移

### 4.1 主要 API 变化

#### 4.1.1 语音合成接口

**v3:**
```csharp
public interface IVoice
{
    PcmStreamWithInfo Speak(string text, SpeakParam param);
    string Name { get; }
    CultureInfo Culture { get; }
}
```

**v4:**
```csharp
public interface IVoice
{
    string Name { get; }
    Task<PcmAudio?> SynthesizePcmAsync(string text, VoiceSynthesisOptions options, CancellationToken ct);
}
```

**差异说明：**
- 同步 → 异步：`Speak()` → `SynthesizePcmAsync()`
- 返回 `Stream` → 返回 `PcmAudio`（封装 Stream + Format）
- 新增 `CancellationToken` 支持取消操作

#### 4.1.2 音频播放接口

**v3:**
```csharp
public interface ISpeaker
{
    ISpeakStateControler Speak(string text);
}
```

**v4:**
```csharp
public interface IAudioPlayer
{
    Task PlayAsync(PcmAudio audio, int volume, CancellationToken ct);
}
```

**差异说明：**
- 事件驱动 → 任务式：使用 `Task` 替代 `PlayCompleted` 事件
- 新增 `CancellationToken` 支持取消操作

#### 4.1.3 听写播放器

**v3:** 依赖 WPF/SpeechLib

**v4:** `IDictationPlayer` 接口，完全解耦 UI

### 4.2 新增特性

| 特性 | 描述 |
|------|------|
| **异步设计** | 所有 I/O 操作均支持异步和取消 |
| **流式处理** | 使用 `Stream` 替代 `byte[]`，降低内存占用 |
| **跨平台支持** | 基于 Avalonia + .NET 10.0 |
| **多语音后端** | Edge TTS, SAPI, macOS say, Linux TTS |
| **缓存机制** | `CachedVoice` 基类提供内存缓存 |
| **预加载机制** | `IPreloadableVoice` 接口支持预加载下一个词 |
| **表达式等待时间** | 支持复杂的等待时间计算公式 |

### 4.3 待完成功能

| 功能 | 状态 | 描述 |
|------|------|------|
| MP3/Opus 导出 | ❌ 待实现 | v4 当前仅支持 WAV 格式导出 |
| 音频编码器集成 | ❌ 待实现 | lame.exe/opusenc.exe 集成 |
| 歌词文件 (LRC) 生成 | ⚠️ 部分 | SaveAudio 支持 LRC，但需完善 |
| SAPI 语音列表 | ⚠️ 待验证 | SapiVoice 需测试 |
| Linux TTS 支持 | ⚠️ 待验证 | Pico2Wave/EspeakNg 需测试 |
| macOS say 支持 | ⚠️ 待验证 | MacSayVoice 需测试 |

---

## 5. 技术栈

### 5.1 框架与运行时

| 技术 | 版本 | 用途 |
|------|------|------|
| .NET | 10.0 | 运行时 |
| Avalonia | 11.3.12 | 跨平台 UI 框架 |
| CommunityToolkit.Mvvm | 8.4.0 | MVVM 框架 |

### 5.2 音频处理

| 技术 | 用途 |
|------|------|
| ManagedBass | 音频解码（bass.dll 封装） |
| Hexa.NET.SDL2 | 音频播放（SDL2 封装） |

### 5.3 语音合成

| 技术 | 平台 | 描述 |
|------|------|------|
| EdgeTTS.DotNet | 跨平台 | Microsoft Edge TTS |
| SAPI (SpeechLib) | Windows | Windows 语音合成 |
| say | macOS | macOS 系统语音 |
| Pico2Wave | Linux | Pico TTS |
| eSpeak NG | Linux | eSpeak NG |

### 5.4 其他依赖

| 技术 | 用途 |
|------|------|
| NCalcSync | 表达式求值（等待时间计算） |
| Avalonia.AvaloniaEdit | 文本编辑器 |

---

## 6. 核心服务

### 6.1 DictationPlayer

v4 核心听写播放器实现，支持：

- **手动模式**：报上一个、再报一遍、报下一个
- **自动模式**：自动连续播报，支持暂停/恢复
- **预加载**：通过 `IPreloadableVoice` 预加载下一个词
- **音频导出**：保存为 WAV 格式

### 6.2 CachedVoice

抽象基类，提供 LRU 缓存功能：

```csharp
public abstract class CachedVoice : IPreloadableVoice
{
    protected CachedVoice(int cacheCapacity = 4);
    public abstract string Name { get; }
    protected abstract Task<PcmAudio?> SynthesizePcmDirectAsync(string text, VoiceSynthesisOptions options, CancellationToken ct);
    public async Task<PcmAudio?> SynthesizePcmAsync(string text, VoiceSynthesisOptions options, CancellationToken ct);
    public async Task PreloadAsync(string text, VoiceSynthesisOptions options, CancellationToken ct);
}
```

### 6.3 ImprovedVoice

增强语音实现，优先使用本地音频文件：

```csharp
public sealed class ImprovedVoice : IVoice
{
    public ImprovedVoice(IVoice inner, string resourceDirectory);
    public string Name { get; }
    public Task<PcmAudio?> SynthesizePcmAsync(string text, VoiceSynthesisOptions options, CancellationToken ct);
}
```

**查找顺序：**
1. 在资源目录中查找与文本同名的音频文件
2. 遍历扩展名：`wav, flac, ape, m4a, opus, aac, mp3, mp2, mp1, ogg, wma, aif, mp4`
3. 如未找到，回退到内部语音

### 6.4 VoiceAggregator

聚合多个语音提供者：

```csharp
public sealed class VoiceAggregator
{
    public VoiceAggregator(params IVoiceFactoryProvider[] providers);
    public async Task<IReadOnlyList<IVoiceFactory>> GetAllFactoriesAsync(CancellationToken ct);
}
```

---

## 7. 数据模型

### 7.1 DictationSettings

```csharp
public sealed class DictationSettings
{
    public string IntervalExpression { get; set; } = "3";    // 等待时间表达式
    public int TimesPerWord { get; set; } = 2;               // 每词播报次数
    public bool HighlightCurrentLine { get; set; } = true;
    public bool AutoScrollToCurrentLine { get; set; } = true;
    public int Volume { get; set; } = 100;                    // 0-100
    public int Rate { get; set; } = 0;                       // -10 到 10
    public string DefaultChineseVoiceName { get; set; } = "";
    public string DefaultEnglishVoiceName { get; set; } = "";
}
```

### 7.2 DictationState

```csharp
public enum DictationState
{
    Stopped,         // 停止
    ManualSpeaking,  // 手动播报中
    AutoRunning,     // 自动播报运行中
    AutoPaused       // 自动播报暂停
}
```

### 7.3 VoiceInfo

```csharp
public sealed class VoiceInfo
{
    public required string Id { get; init; }
    public required string DisplayName { get; init; }
    public string? LocaleOrLanguage { get; init; }
    public string? ProviderName { get; init; }
}
```

### 7.4 VoiceSynthesisOptions

```csharp
public sealed class VoiceSynthesisOptions
{
    public int? Rate { get; init; }  // 语速，-10 到 10
}
```

---

## 8. 待办事项

### 8.1 音频导出

- [ ] 实现 MP3 导出（集成 lame.exe）
- [ ] 实现 Opus 导出（集成 opusenc.exe）
- [ ] 完善 LRC 歌词文件生成

### 8.2 语音支持

- [ ] 测试并修复 SAPI 语音列表获取
- [ ] 完善 macOS say 命令支持
- [ ] 完善 Linux Pico2Wave/eSpeak NG 支持

### 8.3 UI/UX

- [ ] 实现字体选择器
- [ ] 实现高亮跟随功能
- [ ] 实现自动翻页功能

### 8.4 已知问题

- Edge TTS 依赖网络连接，离线不可用
- BASS 库需加载对应的插件才能支持更多格式
- 音频导出目前仅支持 WAV 格式

---

*本文档基于 DictationAssistant v4.x (Avalonia) 源代码审计生成。*
