# DictationAssistant v4.x — ROADMAP

v4 的核心定位：**跨平台（Avalonia + .NET 8）**。在不牺牲跨平台性的前提下，尽量把 v3.x 的交互与信息架构对齐，确保老用户"打开就会用"。

## 原则

- **跨平台优先**：避免引入只能在 Windows 跑的依赖（除非有替代实现/可选后端）。
- **UI 对齐是"体验对齐"**：布局、文案、分组、快捷键尽量 1:1；像素级细节放在后期打磨。
- **Core 纯净**：`DictationAssistant.Core` 保持无 UI、无平台依赖；平台相关放到 `DictationAssistant.App` 的 `Services/`。
- **可迭代**：先把壳子对齐 + 功能闭环，再逐步补强（语音、设置、导出、打包）。

## 架构概览

### Voice 层
- **`IVoice`**：核心接口，`text + VoiceSynthesisOptions → PcmAudio`
- **`IPreloadableVoice`**：扩展接口，支持预加载
- **`CachedVoice`**（抽象基类）：LRU 缓存 + 自动预加载，适合在线引擎
- **`IVoiceFactory` / `IVoiceFactoryProvider`**：发现与创建 Voice
- **`VoiceAggregator`**：聚合所有 Provider 的结果为扁平列表

### 各平台后端

| 平台 | Voice 实现 | 基类 | 合成方式 |
|------|-----------|------|---------|
| Windows | `SapiVoice` | `IVoice` | COM SAPI（含 OneCore 注册表路径） |
| macOS | `MacSayVoice` | `IVoice` | `say -o wav` → PCM |
| Linux | `EspeakNgVoice` | `IVoice` | `espeak-ng --stdout` → PCM |
| Linux | `Pico2WaveVoice` | `IVoice` | `pico2wave` → WAV → PCM |
| 全平台 | `EdgeTtsVoice` | `CachedVoice` | Edge TTS WebSocket → MP3 → PCM（带 LRU 缓存 + 预加载） |
| 装饰器 | `ImprovedVoice` | `IVoice` | 资源目录文件命中 → ManagedBass 解码；miss → 委托内部 Voice |
| — | `NullVoice` | `IVoice` | 返回空（兜底） |

### 音频播放
- **`IAudioPlayer` / `SdlPcmPlayer`**：SDL2 播放 S16LE PCM，音量在播放层统一控制

## 已完成的里程碑

### M0 主窗口 UI 对齐 ✅
- 主窗口布局对齐 v3（菜单 + 右侧控制栏 + 中间词表编辑器）
- AvaloniaEdit 集成、文件打开/保存、自动滚动/高亮

### M1 补齐窗口 ✅
- 关于窗口、偏好设置窗口、保存音频窗口

### M2 设置持久化 ✅
- JSON 持久化到 OS 合适路径
- 窗口大小/位置/状态、词表开关、字体、语音、音量/语速

### M3 语音引擎 ✅
- Voice/VoiceFactory/Provider 架构
- 各平台后端：SAPI（含 OneCore）、macOS say、espeak-ng、pico2wave、Edge TTS
- ImprovedVoice 装饰器（资源目录 → ManagedBass 解码）
- CachedVoice LRU 缓存 + 预加载（在线引擎）
- 表达式等待时间（NCalc，支持 `length * 0.5 + 1` 等动态表达式）
- 中文/英文引擎快捷切换按钮

### M4 词表编辑体验 ✅
- 右侧工具栏、右键菜单（读选定/查词典/从此处播报）
- 背景行高亮（LightGreen，不干扰选择）、平滑滚动
- 撤销/重做（Ctrl+Z/Y）、拖拽打开文件

### M5 音频基础设施 ✅
- SDL2 播放（`SdlPcmPlayer`，音量在播放层控制）
- ManagedBass 音频解码（wav/flac/ape/m4a/opus/aac/mp3/ogg 等）
- WAV 导出 pipeline（遍历词表 → 合成 → 拼接 + 静音间隔 → WAV + 可选 LRC 字幕）

## 待完成

### M6 打包与发布

偏好：**self-contained 打包，随包携带 native 依赖**（SDL2、libbass 等）。

- [ ] Windows：MSIX / NSIS / zip
- [ ] macOS：dmg / zip
- [ ] Linux：AppImage / Flatpak
- [ ] self-contained publish（按 RID：win-x64 / osx-x64 / osx-arm64 / linux-x64 / linux-arm64）
- [x] SDL2 native 依赖：已通过 Hexa.NET.SDL2 NuGet 包自动携带（Windows/Linux/macOS）
- [ ] libbass native 依赖携带策略
- [ ] GitHub Actions：多平台 build + release artifacts

### M7 质量与维护

- [ ] Core 单元测试（播放状态机/进度/暂停恢复/边界条件）— 框架已搭建，部分测试有 timing issue 待修
- [ ] 最小 smoke test
- [ ] 文档：v3→v4 差异说明、跨平台依赖说明

### M8 UI 对齐修复 ✅

- [x] 词表编辑框工具栏图标：v3 使用 ToolBarTray + 图标按钮，v4 使用文字按钮，需改为图标
- [x] 高亮跟随逻辑修复：当前高亮跟随光标位置，应只跟随播报状态（NextWordIndex）
- [x] 语法高亮：v3 有 WordlistHighlighting 资源用于单词着色，v4 缺失

### 可选增强

- [ ] mp3/opus 导出（需外部编码器或 NuGet 库）
- [ ] Linux piper 离线 TTS 支持
- [ ] macOS NSSpeechSynthesizer（替代 CLI `say`）
