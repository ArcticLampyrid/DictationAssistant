# DictationAssistant v4 (Avalonia) - Comprehensive Audit

## Table of Contents
1. [Window Inventory](#1-window-inventory)
2. [Main Window Detailed Layout](#2-main-window-detailed-layout)
3. [PreferenceWindow](#3-preferencewindow)
4. [SaveAudioWindow](#4-saveaudiowindow)
5. [Keyboard Shortcuts](#5-keyboard-shortcuts)
6. [Core Features & Behavior](#6-core-features--behavior)
7. [TTS / Audio Architecture](#7-tts--audio-architecture)
8. [What's NOT Implemented Yet](#8-whats-not-implemented-yet)

---

## 1. Window Inventory

### MainWindow
- **Dimensions**: Default 980x680, Min 860x560
- **Title**: "自动默写"
- **Startup Location**: Persisted (saved/restored from settings)
- **DataContext**: `MainWindowViewModel`

### PreferenceWindow
- **Dimensions**: Default 700x480, Min 580x380
- **Title**: "偏好设置"
- **Startup Location**: CenterOwner (centered on owner)
- **DataContext**: `PreferenceWindowViewModel`

### AboutWindow
- **Dimensions**: Default 640x420, Min 520x320
- **Title**: "关于"
- **Resizable**: No (CanResize="False")
- **Startup Location**: CenterOwner
- **DataContext**: `AboutWindowViewModel`

### SaveAudioWindow
- **Dimensions**: Default 440x360, Min 380x300
- **Title**: "保存音频"
- **Startup Location**: CenterOwner
- **DataContext**: `SaveAudioWindowViewModel`

---

## 2. Main Window Detailed Layout

### Menu Bar
| Menu | Item | Shortcut | Handler |
|------|------|----------|---------|
| 文件(_F) | 新建(_N)... | Ctrl+N | `NewWordList_Click` |
| | 打开(_O)... | Ctrl+O | `OpenFile_Click` |
| | 保存(_S)... | Ctrl+S | `SaveFile_Click` |
| | --- | | |
| | 退出(_E) | | `Exit_Click` |
| 编辑(_E) | 显示/隐藏词语列表 | | `ToggleWordListCommand` |
| | --- | | |
| | 剪切 | Ctrl+X | `Cut_Click` |
| | 复制 | Ctrl+C | `Copy_Click` |
| | 粘贴 | Ctrl+V | `Paste_Click` |
| | --- | | |
| | 全选 | Ctrl+A | `SelectAll_Click` |
| 选项(_O) | 偏好设置(_P)... | | `Preference_Click` |
| | --- | | |
| | 自动翻页 | (toggle) | Bound to `AutoScrollCurrentLine` |
| | 高亮跟随 | (toggle) | Bound to `HighlightCurrentLine` |
| 辅助(_A) | 保存音频 | | `SaveAudio_Click` |
| 帮助(_H) | 关于(_A) | | `About_Click` |

### Right-Side Control Panel (Width: 264px)

#### 播报信息 Section (Border with padding)
| Control | Type | Purpose | Binding |
|---------|------|---------|---------|
| 本词语播报次数 | TextBlock | Display current repeat count | `{Binding CurrentRepeat, StringFormat=本词语播报次数：{0}}` |
| 自动播报间隔/秒 | TextBox | Input interval between words | `{Binding IntervalSeconds, Mode=TwoWay}` |
| 自动播报次数/词 | TextBox | Repeats per word | `{Binding TimesPerWord, Mode=TwoWay}` |
| (State display) | TextBlock | Shows current state | `{Binding SpeakStateText}` |

#### 引擎设置 Section (Border with padding)
| Control | Type | Purpose | Binding |
|---------|------|---------|---------|
| 音量 | Slider | Volume 0-100 | `{Binding Volume, Mode=TwoWay}` |
| 语速 | Slider | Rate -10 to 10 | `{Binding Rate, Mode=TwoWay}` |
| 引擎 | TextBlock | Display engine name | `{Binding TtsEngineName}` |
| 语音 | ComboBox | Voice selection | **TODO/Stub**: Only shows "默认语音（占位）" |

#### 播报控制 Section (Border with padding)
| Control | Purpose |
|---------|---------|
| 开始播报(_S) | `StartAutoCommand` |
| 停止播报(_D) | `StopCommand` |
| 暂停/恢复自动播报 | `PauseOrResumeAutoCommand` (Text changes based on state) |
| 报下一个(_N) | `SpeakNextCommand` |
| 记录归零(_C) | `ResetRecord_Click` handler |
| 再报一遍(_M) | `SpeakAgainCommand` |
| 报上一个(_L) | `SpeakPreviousCommand` |

### Word List Area (AvaloniaEdit TextEditor)

#### Toolbar Buttons (Right side, 72px width)
| Button | Handler |
|--------|---------|
| 打开 | `OpenFile_Click` |
| 保存 | `SaveFile_Click` |
| 剪切 | `Cut_Click` |
| 复制 | `Copy_Click` |
| 粘贴 | `Paste_Click` |
| 删除 | `Delete_Click` |
| 新建 | `NewWordList_Click` |
| 计数 | `Count_Click` |

#### Context Menu Items
| Item | Handler |
|------|---------|
| 读选定词语 | `SpeakSelection_Click` |
| 在Bing词典中查看 | `ViewSelectionInBingDictionary_Click` |
| 从此处开始自动播报 | `StartFromSelection_Click` |
| --- | |
| 剪切 | `Cut_Click` |
| 复制 | `Copy_Click` |
| 粘贴 | `Paste_Click` |
| --- | |
| 全选 | `SelectAll_Click` |

#### Editor Properties
- ShowLineNumbers: True
- FontFamily: Bound to `{Binding EditorFontFamily, Mode=TwoWay}` (default: "Noto Sans CJK SC")
- FontSize: Bound to `{Binding EditorFontSize, Mode=TwoWay}` (default: 28)

### Status Display
- Status bar shows: File load/save status, word count, current progress

---

## 3. PreferenceWindow

### UI Elements

#### Editor Section
| Control | Type | Binding | Persists To |
|---------|------|---------|-------------|
| 字体 | TextBox | `{Binding EditorFontFamily, Mode=TwoWay}` | `Preference.EditorFontFamily` |
| 字号 | NumericUpDown (8-72) | `{Binding EditorFontSize, Mode=TwoWay}` | `Preference.EditorFontSize` |

#### Engine Section
| Control | Type | Binding | Persists To |
|---------|------|---------|-------------|
| 默认中文语音 | ComboBox | `{Binding DefaultChineseVoiceName, Mode=TwoWay}` | `Preference.DefaultChineseVoiceName` |
| 默认英文语音 | ComboBox | `{Binding DefaultEnglishVoiceName, Mode=TwoWay}` | `Preference.DefaultEnglishVoiceName` |
| 音源增强目录 | TextBox + Button | `{Binding ImprovedResourcePath, Mode=TwoWay}` | `Preference.ImprovedResourcePath` |

### Buttons
- **确定 (OK)**: `OkButton_Click` - Closes with result `true`
- **取消 (Cancel)**: `CancelButton_Click` - Closes with result `false`

### Preference Apply/Cancel Flow
1. On OK click: `ViewModel.ToSettings()` creates `PreferenceSettings` object
2. `ResultSettings` property is set and window closes with `true`
3. MainWindow receives result and calls `vm.ApplyPreferenceSettings(dialog.ResultSettings)`
4. Settings are automatically persisted via `partial void On*Changed` handlers in MainWindowViewModel

---

## 4. SaveAudioWindow

### UI Elements

| Control | Type | Options | Default | Binding |
|---------|------|---------|---------|---------|
| 声道 | ComboBox | Mono, Stereo | Stereo | `{Binding Channel, Mode=TwoWay}` |
| 位宽 | ComboBox | Unsigned 8bit, Signed 16bit | Signed 16bit | `{Binding SampleFormat, Mode=TwoWay}` |
| 采样率 | ComboBox (editable) | 8000, 11025, 16000, 22050, 24000, 32000, 44100, 48000 | 44100 | `{Binding Frequency, Mode=TwoWay}` |
| 输出格式 | ComboBox | wav, mp3, opus | wav | `{Binding OutputFormat, Mode=TwoWay}` |
| 字幕模式 | ComboBox | Dismiss, Lrc File | Lrc File | `{Binding LyricMode, Mode=TwoWay}` |
| 目标文件 | TextBox + Button | | dictation.wav | `{Binding TargetPath, Mode=TwoWay}` |

### OK Click Flow
1. `OkButton_Click` handler calls `vm.ExportAsync(CancellationToken.None)`
2. ViewModel creates `SaveAudioRequest`:
   - `OutputPath`: From `TargetPath`
   - `SampleRate`: Parsed from `Frequency` (default 44100)
   - `Channels`: 1 for Mono, 2 for Stereo
   - `OutputFormat`: From selection
   - `LyricMode`: From selection
   - `LyricsOutputPath`: If "Lrc File", set to `.lrc` extension of target path
3. Calls `DictationPlayer.SaveAudioAsync(request, cancellationToken)`
4. In `DictationPlayer.SaveAudioInternalAsync`:
   - Validates word count > 0
   - Validates output path specified
   - **Currently only WAV format is supported** (returns `SaveAudioResult.NotSupported` for others)
   - Iterates through all words, synthesizing with TTS engine
   - Writes PCM data with silence intervals between words
   - Optionally generates LRC file with timestamps
   - Writes WAV header and data to output file
5. Returns result to window, closes with success/failure

### Supported Export Formats
- **WAV**: Fully implemented
- **MP3/Opus**: UI exists but returns "目前仅支持 WAV 格式导出"

---

## 5. Keyboard Shortcuts

| Shortcut | Action | Location |
|----------|--------|----------|
| Ctrl+N | 新建 (New Word List) | Menu: 文件 |
| Ctrl+O | 打开 (Open) | Menu: 文件 |
| Ctrl+S | 保存 (Save) | Menu: 文件 |
| Ctrl+X | 剪切 (Cut) | Menu: 编辑 |
| Ctrl+C | 复制 (Copy) | Menu: 编辑 |
| Ctrl+V | 粘贴 (Paste) | Menu: 编辑 |
| Ctrl+A | 全选 (Select All) | Menu: 编辑 |

---

## 6. Core Features & Behavior

### DictationPlayer State Machine

**States** (`DictationState` enum):
- `Stopped`: Initial/idle state
- `ManualSpeaking`: Single word is being spoken manually
- `AutoRunning`: Automatic dictation in progress
- `AutoPaused`: Automatic dictation paused

**Methods**:
| Method | Behavior |
|--------|----------|
| `SpeakPreviousAsync()` | Speak word at index-1 |
| `SpeakAgainAsync()` | Re-speak current word |
| `SpeakNextAsync()` | Speak word at index+1 |
| `SpeakAtAsync(index)` | Speak specific word (stops auto first) |
| `StartAutoAsync(startIndex)` | Begin automatic dictation from index |
| `PauseAuto()` | Pause auto dictation (state -> AutoPaused) |
| `ResumeAuto()` | Resume paused dictation (state -> AutoRunning) |
| `StopAsync()` | Stop all playback |

**Auto Playback Flow** (`RunAutoAsync`):
1. Loop through words from startIndex to Count-1
2. For each word, repeat TimesPerWord times:
   - Wait if paused (checks `_pauseSignal`)
   - Speak word with TTS
   - If not last repeat, wait IntervalSeconds
3. Update progress after each word
4. On completion, set state to Stopped

**Voice Resolution** (`ResolveVoiceName`):
- If text contains ASCII letters (A-Z, a-z) AND `DefaultEnglishVoiceName` is set → use English voice
- Else if `DefaultChineseVoiceName` is set → use Chinese voice
- Otherwise → use engine default

### ImprovedVoiceTtsEngine

**File Lookup Logic** (`FindFile`):
1. Sanitize text (remove \ / : * ? " < > |)
2. Construct path: `{resourceDirectory}/{sanitized_text}`
3. Try extensions in order: wav, flac, ape, m4a, opus, aac, mp3, mp2, mp1, ogg, wma, aif, mp4
4. If found, decode with `BassAudioDecoder.DecodeFile`
5. If decode succeeds, return PcmAudio
6. Otherwise, fall back to TTS engine

**Fallback Chain**: File lookup → Decode with BASS → Fallback TTS engine

### Waiting Time / Interval Configuration

**Settings in DictationSettings**:
| Property | Default | Clamp Range |
|----------|---------|-------------|
| IntervalSeconds | 3 | 0-600 |
| TimesPerWord | 2 | 1-20 |

**UI Controls**:
- IntervalSeconds: TextBox in "播报信息" section
- TimesPerWord: TextBox in "播报信息" section
- Volume: Slider 0-100
- Rate: Slider -10 to 10

### Settings Persistence

**AppSettings Structure**:
```
AppSettings
├── MainWindow
│   ├── Width (default: 980)
│   ├── Height (default: 680)
│   ├── X (nullable)
│   ├── Y (nullable)
│   ├── WindowState (default: "Normal")
│   └── WordListVisible (default: true)
├── Dictation
│   ├── IntervalSeconds (default: 3)
│   ├── TimesPerWord (default: 2)
│   ├── HighlightCurrentLine (default: true)
│   ├── AutoScrollCurrentLine (default: true)
│   ├── Volume (default: 100)
│   └── Rate (default: 0)
└── Preference
    ├── EditorFontFamily (default: "Noto Sans CJK SC")
    ├── EditorFontSize (default: 28)
    ├── ImprovedResourcePath (default: "")
    ├── DefaultChineseVoiceName (default: "")
    └── DefaultEnglishVoiceName (default: "")
```

**Storage Location**: `%APPDATA%/DictationAssistant/settings.json` (Windows), similar paths on macOS/Linux

**Persistence Flow**:
1. On startup: `AppSettingsStore.Load()` reads JSON
2. During runtime: `partial void On*Changed` handlers update AppSettings
3. On window closing: `MainWindow.Closing` handler saves via `settingsStore.Save(appSettings)`
4. On app exit: `desktop.Exit` handler also saves

### Word List Source

**EditorDocumentWordListSource**:
- Attaches to AvaloniaEdit `TextDocument`
- Listens to `DocumentChanged` event
- Splits text by newlines (\n)
- Exposes `Count`, `GetWordAt(index)`, `GetWords()`
- Fires `Changed` event on any document modification

---

## 7. TTS / Audio Architecture

### TTS Engine Hierarchy

```
ITtsEngine (base interface)
├── Name: string
├── SpeakAsync(text, ct): Task
└── SynthesizeAudioAsync(text, ct): Task<byte[]?>

IConfigurableTtsEngine : ITtsEngine
├── ListVoicesAsync(ct): Task<IReadOnlyList<TtsVoiceInfo>>
├── SpeakAsync(text, options, ct): Task
└── SynthesizeAudioAsync(text, options, ct): Task<byte[]?>

IPcmTtsEngine (PCM synthesis interface)
├── Name: string
├── ListVoicesAsync(ct): Task<IReadOnlyList<TtsVoiceInfo>>
└── SynthesizePcmAsync(text, options, ct): Task<PcmAudio?>

IPreloadableTtsEngine (preloading interface)
├── PreloadAsync(text, options, ct): Task
└── TryConsumePreloadedAsync(text, options, ct): Task<PcmAudio?>
```

### Engine Implementations

| Engine | Platform | Interfaces | Description |
|--------|----------|------------|-------------|
| `WindowsSapiComPcmTtsEngine` | Windows | IPcmTtsEngine | COM-based SAPI, enumerates voices via SpVoice, synthesizes to memory stream |
| `MacSayTtsEngine` | macOS | ITtsEngine, IConfigurableTtsEngine, IPcmTtsEngine | Uses `say` CLI, synth to WAV then decode |
| `LinuxEspeakNgTtsEngine` | Linux | ITtsEngine, IConfigurableTtsEngine, IPcmTtsEngine | Uses `espeak-ng` CLI |
| `LinuxPico2WaveTtsEngine` | Linux | ITtsEngine, IConfigurableTtsEngine, IPcmTtsEngine | Uses `pico2wave` CLI |
| `EdgeTtsPcmEngine` | Cross-platform | IPcmTtsEngine, IPreloadableTtsEngine | Online Edge TTS, supports preloading |
| `ImprovedVoiceTtsEngine` | Cross-platform | IPcmTtsEngine | Wraps another engine, looks up audio files first |
| `NullPcmTtsEngine` | Cross-platform | IPcmTtsEngine | No-op placeholder |
| `NullTtsEngine` | Cross-platform | ITtsEngine | No-op placeholder |
| `WindowsSystemSpeechTtsEngine` | Windows | ITtsEngine, IConfigurableTtsEngine | PowerShell-based System.Speech (not actively used) |

### TtsEngineFactory

```csharp
public static class TtsEngineFactory
{
    // Creates default PCM engine based on OS
    public static IPcmTtsEngine CreateDefaultPcmEngine()
    {
        if (Windows) → WindowsSapiComPcmTtsEngine
        if (macOS)   → MacSayTtsEngine (if 'say' on PATH) else NullPcmTtsEngine
        if (Linux)   → LinuxEspeakNgTtsEngine (if 'espeak-ng' on PATH)
                       else LinuxPico2WaveTtsEngine (if 'pico2wave' on PATH)
                       else NullPcmTtsEngine
    }
    
    // Wraps engine with ImprovedVoice capability
    public static IPcmTtsEngine CreateImprovedVoiceEngine(
        IPcmTtsEngine fallback, 
        string resourceDirectory)
    {
        return new ImprovedVoiceTtsEngine(fallback, resourceDirectory);
    }
    
    // Creates Edge TTS engine
    public static EdgeTtsPcmEngine CreateEdgeTtsEngine()
    {
        return new EdgeTtsPcmEngine();
    }
}
```

### Audio Pipeline

**SdlPcmPlayer** (`IAudioPlayer`):
- Uses SDL2 for cross-platform audio playback
- Opens audio device with desired format
- Queues PCM data via `SDL_QueueAudio`
- Applies volume by scaling samples before queuing
- Blocks until queue is empty
- Only supports S16LE format

**BassAudioDecoder**:
- Uses ManagedBass library for decoding
- Supports: wav, flac, ape, m4a, opus, aac, mp3, ogg, wma, aif, mp4
- Creates decode stream with `Bass.CreateStream`
- Reads all data into MemoryStream
- Returns `PcmAudio` with S16LE format

### Edge TTS Integration

**EdgeTtsPcmEngine**:
- Uses `EdgeTTS` NuGet (fysh711426) for synthesis
- Uses `MP3Sharp` for MP3→PCM decoding
- Implements `IPreloadableTtsEngine` for performance
- Maps rate: rate * 10 → Edge format (+X%/-X%)
- Maps volume: clamp -10~10 → Edge format
- Default voice: "zh-CN-XiaoyiNeural"

**Preload Implementation**:
- Stores one preloaded audio item (key: text+voice+rate)
- When speaking word N, preloads word N+1 in background
- On playback, checks cache first before synthesizing

---

## 8. What's NOT Implemented Yet

### UI/Feature Stubs

1. **Voice Selection ComboBox** (MainWindow.axaml:76-78)
   - Shows static text "默认语音（占位）"
   - Not functional - no voice selection capability wired up

2. **Non-WAV Export** (DictationPlayer.cs:183-186)
   - SaveAudioRequest accepts mp3/opus formats
   - But `SaveAudioInternalAsync` returns `NotSupported` for anything except "wav"

3. **Default Chinese/English Voice in Preference**
   - UI exists but voice selection ComboBoxes are populated from engine's voice list
   - If engine doesn't enumerate voices, shows placeholder text

### ROADMAP.md Unchecked Items

| Milestone | Item | Status |
|-----------|------|--------|
| M3 | Windows: OneCore voices enumeration | Not implemented |
| M3 | macOS: NSSpeechSynthesizer | Not implemented (uses say CLI only) |
| M3 | Linux: piper | Not implemented |
| M4 | Syntax highlighting for word list | Not implemented |
| M5 | SDL2 native dependency packaging | Not fully resolved |
| M5 | ManagedBass codec packaging | Not fully resolved |
| M6 | Windows packaging (MSIX/NSIS) | Not implemented |
| M6 | macOS packaging (dmg) | Not implemented |
| M6 | Linux packaging (AppImage) | Not implemented |
| M7 | Core unit tests | Partially implemented |
| M7 | Smoke tests | Not implemented |
| M7 | Documentation | Not implemented |

### Code with TODOs

The codebase does not contain explicit "TODO" markers in the C# code. The unimplemented features are documented in ROADMAP.md as unchecked items.

---

## Appendix: Key File Locations

| Component | File Path |
|-----------|-----------|
| MainWindow XAML | `src/DictationAssistant.App/MainWindow.axaml` |
| MainWindow Code-behind | `src/DictationAssistant.App/MainWindow.axaml.cs` |
| MainWindowViewModel | `src/DictationAssistant.App/ViewModels/MainWindowViewModel.cs` |
| PreferenceWindow | `src/DictationAssistant.App/PreferenceWindow.axaml` |
| PreferenceWindowViewModel | `src/DictationAssistant.App/ViewModels/PreferenceWindowViewModel.cs` |
| SaveAudioWindow | `src/DictationAssistant.App/SaveAudioWindow.axaml` |
| SaveAudioWindowViewModel | `src/DictationAssistant.App/ViewModels/SaveAudioWindowViewModel.cs` |
| AboutWindow | `src/DictationAssistant.App/AboutWindow.axaml` |
| AboutWindowViewModel | `src/DictationAssistant.App/ViewModels/AboutWindowViewModel.cs` |
| DictationPlayer | `src/DictationAssistant.Core/Services/DictationPlayer.cs` |
| TtsEngineFactory | `src/DictationAssistant.App/Services/Tts/TtsEngineFactory.cs` |
| ImprovedVoiceTtsEngine | `src/DictationAssistant.App/Services/Tts/ImprovedVoiceTtsEngine.cs` |
| EdgeTtsPcmEngine | `src/DictationAssistant.App/Services/Tts/EdgeTtsPcmEngine.cs` |
| SdlPcmPlayer | `src/DictationAssistant.App/Services/Audio/SdlPcmPlayer.cs` |
| BassAudioDecoder | `src/DictationAssistant.App/Services/Audio/BassAudioDecoder.cs` |
| AppSettings | `src/DictationAssistant.App/Services/Settings/AppSettings.cs` |
| AppSettingsStore | `src/DictationAssistant.App/Services/Settings/AppSettingsStore.cs` |
| EditorDocumentWordListSource | `src/DictationAssistant.App/Services/EditorDocumentWordListSource.cs` |
| IPcmTtsEngine | `src/DictationAssistant.Core/Abstractions/IPcmTtsEngine.cs` |
| IPreloadableTtsEngine | `src/DictationAssistant.Core/Abstractions/IPreloadableTtsEngine.cs` |
| IDictationPlayer | `src/DictationAssistant.Core/Abstractions/IDictationPlayer.cs` |
| DictationState | `src/DictationAssistant.Core/Models/DictationState.cs` |
| ROADMAP | `ROADMAP.md` |
