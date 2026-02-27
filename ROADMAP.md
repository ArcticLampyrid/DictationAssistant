# DictationAssistant v4.x — Roadmap

## v3.x 对齐（功能补齐）

### 高优先级
- [ ] 命令行参数打开文件 (`DictationAssistant wordlist.txt`)
- [ ] BASS 插件加载（启动时扫描 `bass_plugin` 目录）
- [ ] 音频导出格式选项（使用对话框中选择的采样率/声道数）

### 中优先级
- [ ] 工具栏添加删除按钮
- [ ] espeak-ng / pico2wave 平台检测完善

### 低优先级
- [ ] 语音预加载优化（v4.x 已有 IPreloadableVoice 骨架，需确认生效）

## v4.x 独立改进

### 打包与发布
- [ ] Windows：MSIX / NSIS / zip
- [ ] macOS：dmg / zip
- [ ] Linux：AppImage / Flatpak
- [ ] self-contained publish（win-x64 / osx-x64 / osx-arm64 / linux-x64 / linux-arm64）
- [ ] libbass native 依赖携带策略
- [ ] GitHub Actions：多平台 build + release artifacts

### 质量与维护
- [ ] 最小 smoke test
- [ ] 文档：跨平台依赖说明

### 可选增强
- [ ] Linux piper 离线 TTS 支持
- [ ] macOS NSSpeechSynthesizer（替代 CLI `say`）
- [ ] CachedVoice 可配置缓存策略
- [ ] UI 字符串国际化
