# v4.x 架构优化建议书

## 一、总体评估

**整体架构评分：6.5/10**

v4.x 经历了从 Core/App 分离架构到单体架构的简化过程（b36d4d8 提交），虽然简化了构建复杂度，但引入了一些架构退化和命名混乱问题。当前代码结构功能完整，但在可维护性、扩展性方面存在改进空间。

---

## 二、发现的问题

### 1. 命名空间组织混乱

**问题描述：**
同一个项目中存在 7 个不同的命名空间：
- `DictationAssistant` (核心模型/服务)
- `DictationAssistant.App` (UI 层)
- `DictationAssistant.App.Services`
- `DictationAssistant.App.Services.Voice`
- `DictationAssistant.App.Services.Settings`
- `DictationAssistant.Models`
- `DictationAssistant.Abstractions`
- `DictationAssistant.Audio`
- `DictationAssistant.Helpers`

**影响：**
- 代码归属不清晰，难以快速定位
- Core 被合并后命名空间未统一清理
- 新开发者难以理解项目结构

**建议：**
- 统一为双层命名空间：`DictationAssistant.Core` + `DictationAssistant.App`
- 或者简化为单层：`DictationAssistant` (Core逻辑) + `DictationAssistant.Desktop` (UI)

---

### 2. 空目录残留

**问题描述：**
存在空的文件夹但未删除：
- `CoreAudio/`
- `CoreHelpers/`
- `CoreModels/`

**影响：**
- 造成混淆，让人不确定是否应该使用这些目录

**建议：**
- 删除这些空目录

---

### 3. MainWindowViewModel 过于庞大

**问题描述：**
`MainWindowViewModel.cs` 长达 **567 行**，承担了过多职责：
- UI 状态管理
- 命令处理
- 业务逻辑协调
- 设置持久化

**影响：**
- 难以测试和维护
- 单一职责原则 (SRP) 违反

**建议：**
- 拆分出独立的 `SettingsService` 处理设置同步
- 提取 `VoiceSelectionService` 处理语音加载逻辑
- 考虑使用 MVVM 分层进一步解耦

---

### 4. 依赖注入未使用

**问题描述：**
`App.axaml.cs` 中直接使用 `new` 实例化所有服务：
```csharp
var settingsStore = new AppSettingsStore();
var wordListSource = new EditorDocumentWordListSource();
var audioPlayer = new SdlPcmPlayer();
var aggregator = new VoiceAggregator(providers.ToArray());
```

**影响：**
- 难以替换实现（如单元测试时使用 Mock）
- 服务生命周期管理困难

**建议：**
- 引入 `Microsoft.Extensions.DependencyInjection`
- 使用构造注入
- 便于测试时替换为 Mock 对象

---

### 5. 日志系统不完善

**问题描述：**
仅使用 `System.Diagnostics.Trace.WriteLine`，未使用结构化日志：
- 无日志级别控制
- 无法输出到文件/控制台
- 无法按模块过滤

**影响：**
- 线上问题难以排查
- 日志格式不统一

**建议：**
- 引入 `Microsoft.Extensions.Logging`
- 添加 `ConsoleLogger` / `FileLogger`
- 统一日志格式

---

### 6. 测试被删除

**问题描述：**
b36d4d8 提交中删除了测试目录。

**影响：**
- 重构风险增加
- 无法自动化验证功能

**建议：**
- 恢复测试项目
- 至少为核心逻辑（DictationPlayer, AudioExporter, WaitingTimeParser）编写单元测试

---

### 7. 错误处理不一致

**问题描述：**
- `VoiceAggregator.GetAllFactoriesAsync` 静默吞掉异常
- `EdgeTtsVoiceFactoryProvider.GetFactoriesAsync` 返回空列表
- 其他地方使用 `Trace.WriteLine`

**影响：**
- 错误难以追踪
- 用户可能不知道语音引擎加载失败

**建议：**
- 统一错误处理策略
- 使用日志记录代替 Trace
- 考虑添加用户通知机制

---

### 8. 项目引用配置与实际不符

**问题描述：**
`DictationAssistant.V4.slnx` 引用了不存在的 `DictationAssistant.Core.csproj`：
```xml
<Project Path="src/DictationAssistant.Core/DictationAssistant.Core.csproj" />
```

**影响：**
- 新手困惑
- 可能导致构建失败

**建议：**
- 修正 slnx 文件，或创建 Core 项目

---

## 三、优化建议优先级

### P0 - 必须修复

1. **修正解决方案文件** - 删除或创建 Core 项目
2. **删除空目录** - CoreAudio, CoreHelpers, CoreModels

### P1 - 强烈建议

3. **统一命名空间** - 规范化为双层结构
4. **引入依赖注入** - 使用 DI 容器
5. **恢复测试** - 至少添加核心业务逻辑测试

### P2 - 可选优化

6. **拆分 MainWindowViewModel** - 提取独立服务类
7. **完善日志系统** - 引入结构化日志
8. **统一错误处理** - 规范化异常处理策略

---

## 四、具体建议

### 4.1 命名空间统一方案

```
DictationAssistant/                    ← 根命名空间
├── Core/                              ← 核心业务逻辑
│   ├── Models/                        ← 数据模型
│   ├── Abstractions/                 ← 接口定义
│   ├── Audio/                        ← 音频处理
│   └── Services/                     ← 核心服务
│       ├── DictationPlayer.cs
│       ├── AudioExporter.cs
│       └── Voice/
└── Desktop/                          ← Avalonia UI
    ├── Views/
    ├── ViewModels/
    └── Services/                     ← UI 相关服务
```

### 4.2 DI 改造示例

```csharp
// App.axaml.cs
var services = new ServiceCollection();
services.AddSingleton<AppSettingsStore>();
services.AddSingleton<ITextFileService, LocalTextFileService>();
services.AddSingleton<IAudioPlayer, SdlPcmPlayer>();
services.AddSingleton<IDictationPlayer, DictationPlayer>();

var provider = services.BuildServiceProvider();
var viewModel = provider.GetRequiredService<MainWindowViewModel>();
```

### 4.3 日志改造示例

```csharp
services.AddLogging(builder =>
{
    builder.AddConsole();
    builder.SetMinimumLevel(LogLevel.Debug);
});

// 使用
_logger.LogInformation("Voice loaded: {VoiceName}", voiceName);
_logger.LogError(ex, "TTS synthesis failed for word: {Word}", word);
```

---

## 五、总结

v4.x 当前架构 **功能完整**，核心业务逻辑（语音合成、音频播放、播报控制）设计合理，接口抽象良好。但架构上有以下明显问题需要改进：

1. **命名空间混乱** - 需要统一规范
2. **缺少依赖注入** - 影响可测试性
3. **测试缺失** - 增加维护风险
4. **空目录残留** - 需要清理

建议优先修复 P0 问题，然后逐步推进 P1 优化，最终达到 8/10 的架构评分喵～
