# Project Details

##Libraries

- Logging Mod (DLL). A mod, just used to log events when they fire

- Revolution (DLL). The core code which is injected into Stardew

- Revolution Patcher (DLL). Is responsible for injecting the core code, and connecting events. This also packages Newtonsoft's JSON utilities with it as we rely on it to load mod configuration manifests.

##Executables

- Revolution UnitTests/Debugger (EXE). Just serves as an entry point for me to step through and debug StardewR code in the event I inject something incorrectly

- Revolution Installer - Console (EXE). This runs RevolutionPatcher, expecting the Stardew Valley and Stardew Revolution files to be adjacent to it. This project has a post-build task setup to automatically repackage the Stardew Revolution.exe binary on successful build.

- Revolution Installer - UI (EXE). This runs RevolutionPatcher too. Though it's not complete, the completed version is the file we should distribute to users. Using ILRepacker, it's dependency DLLs (RevolutionPacker, Revolution) can be self contained. This one will also support downloading of the most recent Revolution DLL from the internet. 

