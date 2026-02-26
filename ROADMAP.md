# DictationAssistant — 待办事项

## 当前需要完成的功能

### 打包与发布
- Windows：MSIX / NSIS / zip
- macOS：dmg / zip
- Linux：AppImage / Flatpak
- self-contained publish（按 RID：win-x64 / osx-x64 / osx-arm64 / linux-x64 / linux-arm64）
- libbass native 依赖携带策略
- GitHub Actions：多平台 build + release artifacts

### 质量与维护
- 最小 smoke test
- 文档：跨平台依赖说明

### 可选增强
- Linux piper 离线 TTS 支持
- macOS NSSpeechSynthesizer（替代 CLI `say`）
