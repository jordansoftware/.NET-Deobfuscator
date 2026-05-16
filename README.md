# 🔧 Universal .NET Defender Remover

![.NET](https://img.shields.io/badge/.NET-8.0-purple)
![Windows Forms](https://img.shields.io/badge/Windows-Forms-blue)
![License](https://img.shields.io/badge/License-MIT-green)

**Universal .NET Defender Remover** is a Windows GUI tool for deobfuscating .NET assemblies. It automatically detects protections (ConfuserEx, Agile.NET, SmartAssembly, etc.) and applies the necessary actions to make the code readable.

## 🎯 Features

| Feature | Description |
|---------|-------------|
| 🔍 Auto Detection | Identifies the obfuscator used (ConfuserEx, Agile.NET, SmartAssembly, Eazfuscator, .NET Reactor) |
| ⚡ Quick Mode | Automatic deobfuscation with optimal parameters |
| 🛠️ Expert Mode | Manual selection of actions (strings, control flow, anti-tamper, etc.) |
| 📦 Drag & Drop | Drag and drop your EXE/DLL file directly into the application |
| 📝 Real-time Logs | Detailed console output for each step |
| 💾 Auto Save | Cleaned file saved with `_Slayed.exe` suffix |


## 🚀 Installation

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download) or higher
- Windows 10/11 (64-bit)

### Build from Source

```bash
git clone https://github.com/your-account/UniversalNetRemover.git
cd UniversalNetRemover
dotnet restore
dotnet build
dotnet run
