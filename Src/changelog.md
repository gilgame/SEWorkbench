Beta 12
================================
 * Updated for 01.130
 * Added an update feature. Workbench will now automatically check for updates and prompt the user to update. This can be disabled in the preferences window.
 * Added a backup feature. Workbench can periodically backup unsaved documents and restore them in the event that the project isn't properly closed (crash, power outage). This can be configured from the preferences window. Note that this will invalidate any existing docking layouts for this release.
 * Made changes to hopefully address some startup issues.
 * Added some more error handling to make it easier to report crashes.

Beta 11
================================
 * Updated for 01.129

Beta 10
================================
 * Updated for 01.128
 * Support for 32-bit operating systems has been dropped.

Beta 9
================================
 * Updated for Space Engineers 01.126
 * Gave the program block editor a scrollbar.
 * Changes to the text editor configuration will now be made immediately.
 * Fixed renaming folders and collections.
 * Close and Close All menu items will now be gray when disabled.
 * Project Explorer will now detect missing files and folders and visibly mark the icon.
 * Added some highlighting for generics.
 * Auto Completion will actively refresh when a import reference is added or removed.
 * Fixed a crash caused by double clicking an error in the Output View when the line number isn't available.
 * Gave the Class View some love (work in progress).

Beta 8
================================
 * Updated for 01.120
 * Projects will still open if blueprints failed to load.
 * Project Explorer panel will now will now remember which nodes are expanded between sessions.
 * Panel layout will now persist between sessions.
 * Added keyboard shortcut to remove trailing whitespace (ctrl+shift+k).
 * Quick Find window will now get focus the first time it is opened.
 * Fixed broken Close menu item.
 * Removed phantom button from Class View panel.

Beta 7
================================
 * Updated for Space Engineers 01.118
 * Added quick find window.
 * Added configuration dialog.
 * Import preprocessor will now appear in the auto-complete window.
 * For now, projects cannot be opened or created if an existing project is still open.
 * Blueprint will now be cleared when a project is closed.
 * Renaming Folders and Collections is temporarily disabled.
 * Added missing reference to VRage.Game.Entity namespace.

Beta 6
================================
 * Updated for Space Engineers 01.117
 * References can now be added and removed from project by right clicking on References in the project view.
 * Files from disk can now be imported from reference (inclusions issue 3) using '#import reference_name' (auto-completion will be updated by saving the file)
 * Will now detect files changed outside the environment.
 * Will now always use latest code from disk (issue 14).
 * Note: Auto-completion will be updated by saving the file.

Beta 5
================================
 * Fixed an issue where Workbench was unable to copy sandbox or allow the user to find it manually.

Beta 4
================================
 * Program will now request elevated priviledges to copy sandbox.
 * Blocks can now be renamed in the blueprint view via context menu (issue 13).
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
