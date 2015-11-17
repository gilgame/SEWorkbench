Space Engineers Workbench
=================================
SE Workbench is a development environment designed to help facilitate the creation and organization of in-game scripts, out of game. The focus of this project is to provide most of the tools one would expect from a development platform without the hassle of configuring Visual Studio.

Features
=================================
As this project is currently in an early alpha development stage, not all features have been implemented. This list represents the goals set forth by the developer(s) to meet a minimum expectation of what this application should deliver.

 * Syntax Highlighting, Auto-Completion, and Refactoring
 * Blueprint to Object Conversion (GridTerminalSystem) for Auto-Completion Parser
 * Out-Of-Game Script Compilation Checker
 * Script Organization Based on Project -> Blueprint (Grid)

![Screenshot](https://raw.githubusercontent.com/gilgame/SEWorkbench/master/Doc/seworkbench-1.png)
![Screenshot](https://raw.githubusercontent.com/gilgame/SEWorkbench/master/Doc/seworkbench-2.png)

Caveats
=================================
Currently, the project requires that Space Engineers is installed on the computer. Since Space Engineers is now open source, this could change in the future. As I'm not sure if SteamSDK.dll and steam_api.dll can be distributed with this project, steam_api.dll must be copied over to the working directory of the exe. The Visual Studio project will attempt to do this itself as long as Space Engineers is installed on the developer's computer.

Attribution
=================================
This project makes use of works derived from:
 * https://github.com/lukebuehler/NRefactory-Completion-Sample
 * https://github.com/icsharpcode/NRefactory
 * https://github.com/icsharpcode/SharpDevelop
