# ILSpy Visual Studio Code Extension [![Join the chat at https://gitter.im/icsharpcode/ILSpy](https://badges.gitter.im/icsharpcode/ILSpy.svg)](https://gitter.im/icsharpcode/ILSpy?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge) [![Build status](https://ci.appveyor.com/api/projects/status/qd6rbpfsparfnwh6/branch/master?svg=true)](https://ci.appveyor.com/project/icsharpcode/ilspy-vscode/branch/master) [![Twitter Follow](https://img.shields.io/twitter/follow/ILSpy.svg?label=Follow%20@ILSpy)](https://twitter.com/ilspy)

You can install the extension in Visual Studio Code via the [marketplace](https://marketplace.visualstudio.com/items?itemName=icsharpcode.ilspy-vscode)

## Develop

The extension consists of two parts: The VSCode extension itself (written in TypeScript) and a "backend" server process (written in C#), which provides a bridge to ILSpy functionality.

If first time

```
npm i vsce -g
```

Compile and package all parts:

```
./build vsix
```

An installable `.vsix` file should be generated in `vscode-extension` folder, if everything is fine.

Compile only backend server from console:

```
./build backend
```

Or open `backend/ILSpy-server.sln` in Visual Studio 2019 (>= 16.9) or another .NET IDE.

Compile VSCode extension itself:

```
./build CompileExtension
./build TestExtension
```

To develop and debug the VSCode extension, install [Visual Studio Code](https://code.visualstudio.com/),
then run

```
cd vscode-extension
code .
```
