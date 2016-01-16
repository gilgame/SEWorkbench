Beta 6
================================
 * Updated for Space Engineers 01.117
 * References can now be added and removed from project by right clicking on References in the project view.
 * Files from disk can now be imported from reference (inclusions) using '#import <reference_name>' (auto-completion will be updated by saving the file)
 * Will now detect files changed outside the environment.
 * Will now always use latest code from disk.
 * Note: Auto-completion will be updated by saving the file.

Beta 5
================================
 * Fixed an issue where Workbench was unable to copy sandbox or allow the user to find it manually.

Beta 4
================================
 * Program will now request elevated priviledges to copy sandbox.
 * Blocks can now be renamed in the blueprint view via context menu.
 * Blueprint view will now be cleared when a blueprint is removed from the project.
 * Pressing Enter will now be the same as clicking 'OK' for most dialogs that take a name for input.
 * Fixed broken serializers for Space Engineers 01.116
 * Fixed an issue that was preventing the blueprint view context menu items from updating when selection changed.

Beta 3
=================================
 * Scripts now inherit from Sandbox.ModAPI.Ingame.MyGridProgram (issue 12).
 * Fixed the registry path for space engineers so it isn't ignoring 32 bit paths.
 * Fixed a null reference exception if the SE registry key can't be found for some reason.

Beta 2
=================================
 * Code Completion window should now only list items for types allowed by the SE API.
 * Failure to open a project should now show an error instead of crash the program.

Beta 1
=================================
 * Initial Beta release.
