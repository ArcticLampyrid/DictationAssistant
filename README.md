# DictationAssistant
自动默写（DictationAssistant）是一个用于辅助学习的工具，它可以利用自动化手段帮助您默写单词、短语、句子等。软件通过语音合成技术以给定的间隔、重复次数朗读指定的文本，以帮助您在传统纸张或电子设备上默写。

本仓库目前同时保留：
- **v3（旧版）**：C# + WPF + .NET Framework 4.0（Windows only）
- **v4（新版）**：C# + Avalonia + .NET 8（Windows/macOS/Linux）

> v3 代码保留在 `DictationAssistant/` 目录，未删除。
> v4 代码位于 `src/DictationAssistant.App` 和 `src/DictationAssistant.Core`。

## 历史版本

| 版本   | 分支   | 技术方案                 |
| :----- | :----- | :----------------------- |
| 4.x    | 4.x    | C# + Avalonia + .NET 8   |
| 3.x    | 3.x    | C# + WPF                 |
| 2.x    | 2.x    | C# + WinForm             |
| 1.x    | 1.x    | VB.NET + WinForm         |
| Legacy | legacy | VB6                      |

## 开发
### v4（跨平台）

v4 使用 .NET 8 + Avalonia，推荐在 Linux/macOS/Windows 使用 `dotnet` CLI：

```bash
dotnet restore DictationAssistant.V4.slnx
dotnet build DictationAssistant.V4.slnx
dotnet run --project src/DictationAssistant.App/DictationAssistant.App.csproj
```

主要能力（v4）:
- 文本词表编辑（带行号）、加载/保存 `.txt`
- 手动播报：上一条 / 重播 / 下一条
- 自动模式：每词间隔 + 每词重复次数，支持暂停/恢复
- 进度显示，当前行高亮和自动滚动开关
- 跨平台 TTS 引擎抽象（Windows/macOS/Linux + No-op fallback）
- 预留“保存音频”API（当前为跨平台 TODO 脚手架）

### v3（旧版 Windows WPF）

DictationAssistant 的 3.x 实现使用 C# + WPF + .NET Framework 4.0，请使用 [Visual Studio](https://visualstudio.microsoft.com/) 打开。 

注意：
- 出于兼容性考虑我们仍然在使用 .NET Framework 4.0，而 Visual Studio 2022 已默认不支持 .NET Framework 4.0，因此建议使用 Visual Studio 2019。
- 或者，您也可以参照 [在 VS2022 中编辑 .NET 4.0 项目](https://alampy.com/2024/02/16/develop-dotnet-v4-project-with-vs2022/) 的方法，使用 Visual Studio 2022 打开。

## 贡献
欢迎提交 Pull Request 或 Issue。

## 许可证
    Copyright (C) 2013-2024 alampy.com

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU Affero General Public License as published
    by the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Affero General Public License for more details.

    You should have received a copy of the GNU Affero General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.

本项目根据 AGPL-3.0 或更高版本的许可证授权。请参阅 [LICENSE](LICENSE.md) 文件以获取详尽的信息。
