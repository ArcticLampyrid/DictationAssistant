# DictationAssistant v4.x — ROADMAP

v4 的核心定位：**跨平台（Avalonia + .NET）**。在不牺牲跨平台性的前提下，尽量把 v3.x 的交互与信息架构对齐，确保老用户“打开就会用”。

## 原则

- **跨平台优先**：避免引入只能在 Windows 跑的依赖（除非有替代实现/可选后端）。
- **UI 对齐是“体验对齐”**：布局、文案、分组、快捷键尽量 1:1；像素级细节放在后期打磨。
- **Core 纯净**：`DictationAssistant.Core` 保持无 UI、无平台依赖；平台相关放到 `DictationAssistant.App` 的 `Services/`。
- **全平台复用 v3 的关键体验**：
  - **ImprovedVoice**（“音源增强目录”：命中则播文件，否则回落到 TTS）需要在 **Windows/macOS/Linux 全平台可用**。
  - 音频播放/解码尽量使用跨平台 NuGet 库替换旧的手写 P/Invoke 包装，以便维护与打包。
- **可迭代**：先把壳子对齐 + 功能闭环，再逐步补强（语音引擎、设置、导出、打包）。

## 里程碑 / TODO

### M0（已完成）主窗口 UI 对齐（v3 → v4）

- [x] v4 主窗口整体布局对齐 v3（菜单 + 右侧控制栏 + 中间“词语列表”）
- [x] 集成 AvaloniaEdit 作为词表编辑器
- [x] 词表数据源改为“编辑器文档驱动”（编辑即生效）
- [x] 文件打开/保存对话框（Avalonia StorageProvider）
- [x] 自动滚动/当前行高亮（基础实现）

> 相关提交：`feat(app): align v4 main window with v3 layout`

### M1（进行中）补齐 v3 其它窗口入口（先对齐壳子）

- [x] **关于**窗口（v3 AboutWindow 对齐：标题/版本/版权）
- [x] **偏好设置**窗口（v3 PreferenceWindow 对齐：编辑器字体/默认中英语音/资源目录等）
- [x] **保存音频**窗口（v3 SaveAudioDialog 对齐：参数选择 + 输出路径）
  - [x] v4 暂可先保留“未实现”提示，但 UI 与入口先到位

### M2 设置持久化（跨平台）

- [x] 引入 v4 `AppSettings`（json）并持久化到 OS 合适路径（Windows/macOS/Linux）
- [ ] 保存/恢复：窗口大小位置、隐藏词表开关、自动翻页/高亮跟随、间隔/次数
- [x] 保存/恢复：编辑器字体设置（等价于 v3 FontInfo）
- [x] 保存/恢复：默认语音（中文/英文）与音量/语速

### M3 语音引擎（跨平台后端）

- [ ] `ITtsEngine` 支持枚举 voices + 选择 voice（供偏好设置窗口使用）
- [ ] **预加载接口设计（仅下一条）**
  - [ ] 在 Core 抽象层引入可选的 preload 能力（例如 `IPreloadableTtsEngine`），允许引擎对“下一条文本”做 best-effort 预取
  - [ ] DictationPlayer 侧预留 hook：每次开始播报第 N 条时，后台触发预加载第 N+1 条（只保留 1 条预加载槽位）
  - [ ] 失败策略：预加载失败/在线 TTS 失败均静默，仅日志记录，不影响当前播报流程
- [ ] Windows：**保留/迁移 v3 的“原生引擎体系”**
  - [ ] SAPI/OneCore voices（v3 的 `SpeechLib` 路线：枚举 TokenId + 选择 voice + Rate）
  - [ ] ImprovedVoice（“音源增强目录”：命中则播文件，否则回落到系统 voice）
  - [ ] （可选）System.Speech（当前 v4 的 PowerShell `System.Speech.Synthesis` 回落方案）
- [ ] macOS：NSSpeechSynthesizer / `say`（可枚举 voices + 选择）
- [ ] Linux：优先考虑 `espeak-ng` / `piper`（离线）
- [ ] **全平台：支持 Edge TTS（在线）**
  - [ ] 调研并选型 NuGet：`edge-tts-net` 或 `EdgeTTS`（二选一，优先维护更活跃/接口更稳定者）
  - [ ] 集成 Edge TTS engine：可选 voice、可调语速/音量（按 Edge TTS 语义映射）
  - [ ] **失败处理**：在线 TTS 失败时“静默不打断流程”（不弹窗），仅记录日志/状态栏提示（可选），**不自动回落到其它引擎**
- [ ] 统一“音量/语速”的语义映射（不同后端范围不同）

### M4 词表编辑体验打磨（贴近 v3）

- [ ] 右侧工具栏补齐/强化：打开、保存、剪切/复制/粘贴、删除、新建、计数
- [ ] 右键菜单行为对齐：读选定词语、从此处开始自动播报、查词
- [ ] 高亮实现升级：从“选中当前行”改为更接近 v3 的“背景高亮”（不干扰选择）
- [ ] 自动翻页更稳：播报推进时滚动到可视区域（不要跳太猛）
- [ ] 可选：语法高亮（如果词表里需要特殊格式）

### M5 音频 / 播放 / 解码基础设施（跨平台复用 v3 逻辑）

目标：让 v4 能在 **全平台** 复用 v3 的“播文件/播 PCM/TTS”关键链路，把旧的 SDL2/BASS 手写 P/Invoke 换成更稳的 NuGet 库。

- [ ] **音频播放：SDL2（跨平台）**
  - [ ] 选型并引入跨平台 SDL2 包装（NuGet）
  - [ ] 用 NuGet 包替换 v3 的 `SDL2.dll` P/Invoke（`PcmPlayer`/`SdlAudio`）
  - [ ] 保持现有 `PcmPlayer(PcmStreamWithInfo, volume)` 语义，尽量复用调用方逻辑

- [ ] **音频解码：ManagedBass（跨平台）**
  - [ ] 选型并引入 `ManagedBass`（及需要的 codec 扩展包）
  - [ ] 用 ManagedBass 替换 v3 的 `bass.dll` P/Invoke（`AudioFileDecodeStream`）
  - [ ] 确保支持 v3 的常见扩展名（wav/flac/ape/m4a/opus/aac/mp3/ogg/wma/aif/mp4…）

- [ ] **ImprovedVoice 全平台化**
  - [ ] 把 ImprovedVoice 的“命中音频文件则播放”能力迁到 v4（平台无关，全平台都支持）
  - [ ] 解码走 ManagedBass → 输出 PCM → 播放走 SDL2（NuGet 包装，替换手写 P/Invoke）
  - [ ] 未命中则回落到当前选择的 TTS engine

- [ ] 在此基础上，再实现 **导出（SaveAudio）**
  - [ ] 明确 v4 导出的“最小可用”目标（WAV 先行）
  - [ ] 设计导出 pipeline：TTS/ImprovedVoice → PCM → 编码器（WAV/MP3/AAC…）
  - [ ] 进度条 + 可取消 + （可选）歌词/字幕输出

### M6 打包与发布（跨平台分发）

偏好：**self-contained 打包，并尽量携带依赖的 native 库**（SDL2 / 音频解码库等），降低用户安装负担。

- [ ] Windows：MSIX / NSIS / zip（择一）
- [ ] macOS：dmg / zip（签名后续）
- [ ] Linux：AppImage / Flatpak（择一）
- [ ] self-contained publish（按 RID：win-x64/osx-x64/osx-arm64/linux-x64/linux-arm64…）
- [ ] 将所需 native 依赖随包携带（或用 NuGet 自带 runtimes/native 的方案）
- [ ] GitHub Actions：多平台 build + release artifacts

### M7 质量与维护

- [ ] Core 单元测试（播放状态机/进度/暂停恢复/边界条件）
- [ ] 最小 smoke test（启动、加载词表、单次播报、自动播报、暂停恢复）
- [ ] 文档：v3→v4 差异说明、跨平台依赖说明（例如 Linux 需要安装哪个 TTS 后端）

## 近期优先级（下一步建议）

1) **M1：把关于/偏好/保存音频三个窗口先补齐入口与 UI**（功能可先 stub）
2) **M2：做设置持久化**（否则用户每次都要重新调）
3) **M3：语音引擎选择与 voices 枚举**（真正跨平台的关键）

---

（小雪会定期回来看这个 ROADMAP，把完成项打勾并推进下一步。）
