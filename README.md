[![Stardew-Farmhand on discord](https://img.shields.io/badge/discord-stardew--farmhand-blue.svg?style=flat-square)](https://discord.gg/0t3fh2xhHVdOFmtI)

## Current Build Status

| Branch        | Windows CI    | Static Analysis  |
| ------------- | ------------  | --- |
| Master      | [![Build status](https://ci.appveyor.com/api/projects/status/isjopx9h6gv9vv2x/branch/master?svg=true)](https://ci.appveyor.com/project/ClxS/stardew-farmhand/branch/master) | |
| Development      | [![Build status](https://ci.appveyor.com/api/projects/status/isjopx9h6gv9vv2x/branch/development?svg=true)](https://ci.appveyor.com/project/ClxS/stardew-farmhand/branch/development) |  [![Static Analysis](https://scan.coverity.com/projects/10466/badge.svg?flat=1)](https://scan.coverity.com/projects/10466) |

# Project Details

Stardew Farmhand is the new in-development API intended to replace SMAPI

##License

SMAPI Compatibility Layer: LGPL

Farmhand: MIT

xml2json: Not Specified (http://www.bjelic.net/2012/08/01/coding/convert-xml-to-json-using-xslt/)

##Branching Policy

The master branch is marked as protected and it's what all release builds will be compiled against. 
Make your pull requests against the development or feature branches, pull requests directly into master probably will not be accepted.

##Compiling the project

Note that this only applies to those wanting to develop the API. Mod developers do not need to go through this and the installer provides you with prebuilt packages you can use for mod development.

###Windows

######Populating Submodules
(This isn't nessecary unless you want to compile the SMAPI compatibility extension.)

After cloning the repository, the first thing you'll need to do is download the SMAPI submodule. 
To do this, execute the PopulateSubmodules.bat file, or use the following command:

```
git submodule update --init --recursive
```

######Preparing Staging Files

Next we need to prepare a folder so the project knows where to locate Stardew files to inject into:

- Create a folder in the project root directory called 'Staging'.

- Create a folder in this new Staging folder called 'Windows'.

- Copy your entire Stardew Valley install, including dlls, executables, and the Content folder, into this directory.

- If you wish to also build a Linux compatible version, create a folder called 'Linux' in the new staging folder and copy a Linux version of the game to it.

######Compiling

Now open the project's solution file. Note that if you do not have Sandcastle or XNA Game Studio installed, you'll receive an error about two of the projects failing to load, this can be ignored unless you are wanting to generate documentation (Sandcastle) or compile the shader test mod (XNA Game Studio).

Once the solution is open, the Stardew Farmhand executable can be compiled by **Rebuild**ing the BuildFarmhandFinal project.

######Project Dependencies and Gotcha's

Because of how the project is structured, there are 3 tiers of dependencies in the core project.
- Tier 1: The first tier includes projects without any dependencies. This tier consists mostly of Farmhand.csproj, and is compiled into the Teir 2 dependency by the BuildFarmhandIntermediate project.

- Tier 2: These projects (Libraries/Farmhand{X}) depend on the build output from BuildFarmhandIntermediate (StardewFarmhand.int1.dll). These libraries build upon modifications made by the Tier 1 changes, and are themselves a part of Stardew Farmhand after being compiled into the Tier 3 dependency, the final executable, by the BuildFarmhandFinal project.

- Tier 3: Tier 3 libraries are ones which depend on the final Stardew Farmhand.exe, these include things such as all the mods and the SMAPI Extension project.

Because of these distinct dependency layers, changing a project in a lower layer wont be reflected in one in a higher level until you have rebuilt the layer. For example, if you add a new class in Farmhand that you want to be accessible in FarmhandGame, you need to have **rebuilt** BuildFarmhandIntermediate so it is in the intermediate DLL it actually references. If you needed to access our example class from a mod, you'd need to rebuild BuildFarmhandFinal so it's in the final executable which the mod references.

Because MSBuild cannot track dependencies properly on projects such as this, you'll need to Rebuild the BuildTask projects, so that the build commands that handle packaging are
actually executed.

####Linux

To build the project on Linux, follow the steps above, constructing only a Linux staging folder instead of a Windows folder.

There are a few issues which are only present in the Linux build:

- When opening the project in an IDE (MonoDevelop), you will get a warning about the Farmhand (Sandcastle) and VignetteMod projects. Just click to ignore these issues. You will not
be able to build either of these projects but neither are vital.

- MonoDevelop will frequently just cease to work with the project, and cause random compile errors. If the error reports that it is due to an internal MSBuild error, clean the solution and build again; for other errors relating to undefined references in the project files, restarting MonoDevelop resolves these. Please let me know if you find an IDE which works better with the project!

##Project Structure

###Build Tasks

These projects create a MSBuild-able flow for the project, so manually patching is not required.

######BuildFarmhandIntermediate
Packages Farmhand Core into Stardew and applies the appropriate hooks. This is the project you should click "Rebuild" on when altering the engine, as it will force all builds to trigger in the correct order.

######BuildFarmhandFinal
Packages the projects dependent on the results of BuildFarmhandIntermediate into Stardew and produces the final Stardew Farmhand.exe

###Documentation

######Farmhand
Compiles documentation using SandCastleFileBuilder into HTML

###Installers

######FarmhandPatcherCommon
Contains common patcher code, and is the core library responsible for modifying the Stardew executable.

######FarmhandPatcherFirstPass and FarmhandPatcherSecondPass
Thin libraries used to ensure the correct libraries are used for each injection pass.

######FarmhandInstaller-Console
A command line interface used for running the patcher. This will not be included as a public installer and instead is just used as a part of the build process.

- Farmhand Installer - UI (EXE). This runs FarmhandPatcher too. Once it's complete, this will be the file we actually distribute out. It should automatically bundle the required binaries (Farmhand, FarmhandUI, FarmhandPatcherCommon, FarmhandPatcherFirstPass, FarmhandPatcherSecondPass) and extract them at runtime. At the final stages, this should allow for updated binaries to be fetched from the internet.

- FarmhandPatcherCommon/FirstPass/SecondPass. These are used by the installer.They are separated into their own libraries to prevent conflicts due to the missing (not yet built) Stardew Farmhand intermediate

##Libraries

- Farmhand (DLL). The core code which is injected into Stardew

##Tools

- Farmhand Debugger (EXE). Just serves as an entry point for me to step through and debug StardewR code in the event I inject something incorrectly. This should be set as your startup project when not debugging the build process.

