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

Next, we need to copy the Stardew's files to the location when the project expects them to be. Firstly, create a new folder called "Staging" in the root of your solution. You'll need to Copy+Paste yourr entire Stardew game files, including any DLLs and the Content folder.

######Compiling

Now open the project's solution file. Note that if you do not have Sandcastle or XNA Game Studio installed, you'll receive an error about two of the projects failing to load, this can be ignored unless you are wanting to generate documentation (Sandcastle) or compile the shader test mod (XNA Game Studio).

Once the solution is open, the Stardew Farmhand executable can be compiled by **Rebuild**ing the BuildFarmhandFinal project.

######Project Dependencies and Gotcha's

Because of how the project is structured, there are 3 tiers of dependencies in the core project.
- Tier 1: The first tier includes projects without any dependencies. This tier consists mostly of Farmhand.csproj, and is compiled into the Teir 2 dependency by the BuildFarmhandIntermediate project.

- Tier 2: These projects (Libraries/Farmhand{X}) depend on the build output from BuildFarmhandIntermediate (StardewFarmhand.int1.dll). These libraries build upon modifications made by the Tier 1 changes, and are themselves a part of Stardew Farmhand after being compiled into the Tier 3 dependency, the final executable, by the BuildFarmhandFinal project.

- Tier 3: Tier 3 libraries are ones which depend on the final Stardew Farmhand.exe, these include things such as all the mods and the SMAPI Extension project.

Because of these distinct dependency layers, changing a project in a lower layer wont be reflected in one in a higher level until you have rebuilt the layer. For example, if you add a new class in Farmhand that you want to be accessible in FarmhandGame, you need to have **rebuilt** BuildFarmhandIntermediate so it is in the intermediate DLL it actually references. If you needed to access our example class from a mod, you'd need to rebuild BuildFarmhandFinal so it's in the final executable which the mod references.

####Linux

Coming Soon!

##Working on the core API

The core API is located inside Libraries/Farmhand. For changes to this library to be reflected, you'll need to manually trigger BuildFarmhandFinal. Alternatively you can add BuildFarmhandFinal as a build dependency to Tools/FarmhandDebugger. This is not done as default as it would lower iteration times for developers working on mods - who do not need to constantly rebuild the API.

Note, that if you are working on both Farmhand and a second-pass API library (Farmhand UI), you will need to manually build BuildFarmhandIntermediate to fix intellisence reference warnings. However, these warnings can just be ignored and will do away upon trigging a final build if the code is correct.

##Build Tasks

These projects are responsible for handling build flow. 

- BuildFarmhandIntermediate packages Farmhand Core into Stardew and applies the appropriate hooks. This is the project you should click "Rebuild" on when altering the engine, as it will force all builds to trigger in the correct order.

- BuildFarmhandFinal packages the projects dependent on the results of BuildFarmhandIntermediate into Stardew and produces the final Stardew Farmhand.exe

##Installers

- Farmhand Installer - Console (EXE). This runs FarmhandPatcher, expecting the Stardew Valley and Stardew Farmhand files to be adjacent to it. 

- Farmhand Installer - UI (EXE). This runs FarmhandPatcher too. Once it's complete, this will be the file we actually distribute out. It should automatically bundle the required binaries (Farmhand, FarmhandUI, FarmhandPatcherCommon, FarmhandPatcherFirstPass, FarmhandPatcherSecondPass) and extract them at runtime. At the final stages, this should allow for updated binaries to be fetched from the internet.

- FarmhandPatcherCommon/FirstPass/SecondPass. These are used by the installer.They are separated into their own libraries to prevent conflicts due to the missing (not yet built) Stardew Farmhand intermediate

##Libraries

- Farmhand (DLL). The core code which is injected into Stardew

##Tools

- Farmhand Debugger (EXE). Just serves as an entry point for me to step through and debug StardewR code in the event I inject something incorrectly. This should be set as your startup project when not debugging the build process.

