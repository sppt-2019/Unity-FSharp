# F# in Unity - Eight Scenes that Examine if it Makes Sense

This repository contains eight scenes that are used to test the use of F# in Unity. We use the [F# Unity Integration plugin](https://github.com/sppt-2k19/unity-fsharp-integration) to compile and include F# in Unity.

## Getting Started

As part of the project we have written [a small guide that introduces Unity developers to F# in Unity](https://sppt-2019.github.io/unity-fsharp-introduction/). Each scene in this project is associated with a task tbat is meant to explore a single aspect of F# in Unity. The tasks descriptions can be found [here](https://sppt-2019.github.io/unity-fsharp-introduction/tasks) (currently only in Danish, but please open an issue if we should translate them).

The project can be opened in Unity and you can use either Visual Studio or Rider for the F# and C# code. The [F# Unity Integration plugin](https://github.com/sppt-2k19/unity-fsharp-integration) adds a new menu in Unity, where you may open, configure and compile the F# project.

## Functional Reactive Programming

We have added a simple functional reactive programming system in Unity. This may be accessed by inheriting your components from `FRPBehaviour`. This enables the use of the `ReactTo` function:
```fsharp
type Jumper() =
  inherit FRPBehaviour()
  
  [<SerializeField>]
  let mutable JumpForce = 200.0f
  
  member this.Start() =
    this.ReactTo(
      FRPEvent.Keyboard,
      (fun () -> Input.GetKeyDown(KeyCode.Space)),
      (fun () ->
        let up = this.transform.up * JumpForce
        this.transform.AddForce(up)))
```
A word of caution though, the FRP system currently introduces quite a large overhead as it is naïvely implemented. We have started an optimised implementation on the frp-engine branch, which aims at optimising performance. We have an introduction to the use of FRP in Unity [here](https://sppt-2019.github.io/unity-fsharp-introduction/frp.html) (currently only in Danish, but please open an issue if you want a translation).

## Building & Running

The F# project can be compiled from Unity by pressing <kbd>F6</kbd>. Afterwards, all components implemented in F# may be found under the 'Add Component' menu. The project can be started by pressing the play button in Unity.
