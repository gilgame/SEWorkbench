Space Engineers Workbench
=================================
SE Workbench is a development environment designed to help facilitate the creation and organization of in-game scripts, out of game. The focus of this project is to provide most of the tools one would expect from a development platform without the hassle of configuring Visual Studio. The goal is to overcome technical hurdles and make scripting in Space Engineers something anyone can begin learning to do, regardless of their background or experience.

Features
=================================
These are just a few of the features that have been implemented so far. I think the project is beginning to reach a beta stage where additional features will slow down and serious release builds will be deployed. SE Workbench is fairly stable and can be trusted with the file system.

 * Syntax Highlighting and Auto-Completion.
 * Functions as an out-of-game script checker that builds scripts based on the limitations set forth by Space Engineers.
 * [Script collections](https://github.com/gilgame/SEWorkbench/wiki/Script-Collections) that allow the developer to separate larger scripts into multiple files and folders.
 * [Inclusions](https://github.com/gilgame/SEWorkbench/wiki/References) allow developers to include the contents of other scripts in their current program.
 * [Blueprint](https://github.com/gilgame/SEWorkbench/wiki/Blueprints) program importing and editing.
 * Basic integrated [IL viewer](https://github.com/gilgame/SEWorkbench/wiki/Classes-Explorer) for allowed namespaces.
 * Script Organization using a directory structure.

Screenshots
=================================
Click on an image to enlarge.
![Screenshot](https://raw.githubusercontent.com/gilgame/SEWorkbench/master/Doc/seworkbench-1.png)
![Screenshot](https://raw.githubusercontent.com/gilgame/SEWorkbench/master/Doc/seworkbench-2.png)

Caveats
=================================
 * Workbench requires that Space Engineers is owned and installed on your computer. Workbench functions as a mod and is subject to the same restrictions.
 * Having Workbench running while updating Space Engineers through Steam could cause Steam to think the installation is corrupt because of files being in use.
 * Currently, Keen is moving a lot of things around in the Space Engineers libraries that Workbench needs in order to function. This could potentially render Workbench broken every Thursday until they are finished.
 * Support for 32-bit operating systems has been dropped.

Attribution
=================================
This project makes use of works derived from:
 * https://github.com/lukebuehler/NRefactory-Completion-Sample
 * https://github.com/icsharpcode/NRefactory
 * https://github.com/icsharpcode/SharpDevelop
 * http://avalondock.codeplex.com/
 * http://www.fatcow.com/free-icons