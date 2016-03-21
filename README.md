# Project Details

##Build Tasks

- These projects are responsible for handling build flow. BuildRevolutionIntermediate packages Revolution Core into Stardew and applies the appropriate hooks.

- BuildRevolutionFinal packages the projects dependent on the results of BuildRevolutionIntermediate into Stardew and produces the final Stardew Revolution.exe

##Installers

- Revolution Installer - Console (EXE). This runs RevolutionPatcher, expecting the Stardew Valley and Stardew Revolution files to be adjacent to it. This project has a post-build task setup to automatically repackage the Stardew Revolution.exe binary on successful build.

- Revolution Installer - UI (EXE). This runs RevolutionPatcher too. Though it's not complete, the completed version is the file we should distribute to users. Using ILRepacker, it's dependency DLLs (RevolutionPacker, Revolution) can be self contained. This one will also support downloading of the most recent Revolution DLL from the internet. 

##Mods

- Logging Mod (DLL). A mod, just used to log events when they fire

- Mod Loader Mod (DLL). A mod which will provide ingame loading and unloading of other mods.

##Libraries

- RevolutionPatcherCommon/FirstPass/SecondPass. These are used by the installer.They are separated into their own libraries to prevent conflicts due to the missing (not yet built) Stardew Revolution intermediate

- Revolution (DLL). The core code which is injected into Stardew

- RevolutionUI (DLL). This is an extension of Revolution which provides easy to use overrides for the inbuild UI classes.

##Tools

- Revolution Debugger (EXE). Just serves as an entry point for me to step through and debug StardewR code in the event I inject something incorrectly. This should be set as your startup project when not debugging the build process.



