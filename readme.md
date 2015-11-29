Space Engineers Workbench
=================================
SE Workbench is a development environment designed to help facilitate the creation and organization of in-game scripts, out of game. The focus of this project is to provide most of the tools one would expect from a development platform without the hassle of configuring Visual Studio. The goal is to overcome technical hurdles and make scripting in Space Engineers something anyone can begin learning to do, regardless of their background or experience.

Features
=================================
These are just a few of the features that have been implemented so far. I think the project is beginning to reach a beta stage where additional features will slow down and serious release builds will be deployed. SE Workbench is fairly stable and can be trusted with the file system.

 * Syntax Highlighting and Auto-Completion
 * An out-of-game script checker that tests scripts based on the actual parameters and limitations set forth by the game
 * Script collections that allow the developer to separate larger scripts into multiple files and folders
 * Blueprint program importing and editing
 * Basic integrated IL viewer for allowed namespaces
 * Script Organization Based on Project -> Blueprint (Grid)

Code completion:
![Screenshot](https://raw.githubusercontent.com/gilgame/SEWorkbench/master/Doc/seworkbench-1.png)

Script and collection script checker:
![Screenshot](https://raw.githubusercontent.com/gilgame/SEWorkbench/master/Doc/seworkbench-2.png)

Blueprint program editing:
![Screenshot](https://raw.githubusercontent.com/gilgame/SEWorkbench/master/Doc/seworkbench-3.png)

Caveats
=================================
 * Having SE Workbench running while updating Space Engineers through steam may cause Steam to think the installation is corrupt because of files being in use.
 * SE Workbench does currently require that Space Engineers is installed on the host machine.

Attribution
=================================
This project makes use of works derived from:
 * https://github.com/lukebuehler/NRefactory-Completion-Sample
 * https://github.com/icsharpcode/NRefactory
 * https://github.com/icsharpcode/SharpDevelop
