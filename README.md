# Unity F#

This repository contains a setup that allows the programmer to write F# components for a Unity game.

## Getting started
Open up the sub-project called UnityFunctional in Visual Studio. This project includes a post-build script that moves the generated 
assemblies from the UnityFunctional project and into the Unity project. You will need to change this by **Selecting UnityFunctional**
and hitting <kbd>Alt</kbd>+<kbd>Enter</kbd>, which brings up the project properties panel. Navigate to **Build Events** and change:
```bash
move /Y compiled_dll_path unity_frameworks_path
```
To the corresponding paths on your system. I had to use absolute paths and the compiled dll will end up under `Unity-F#\UnityFunctional\UnityFunctional\bin\Release\net45\UnityFunctional.dll`.

Now open up the Unity project.

## Building
When you have written some F# code and wish to run it in Unity you click <kbd>Ctrl</kbd>+<kbd>Shift</kbd>+<kbd>B</kbd> and the sources
are compiled and moved to Unity. When you open Unity again, Unity will take a short while to compile the new dll and should become 
responsive again fairly quickly.

Any types you have defined in F# that inherits from MonoBehaviour should automatically be available from the 'Add Component' dropdown.

## Running
Add (and configure) any F# components you want and hit the play button in Unity.
