namespace UnityFunctional

open UnityEngine
open System

type Resources() =
    inherit MonoBehaviour()

    [<SerializeField>]
    let mutable DildSild:int = 32
    [<SerializeField>]
    let mutable MildAbild:int = 45
    [<SerializeField>]
    let mutable Vildild:int = 5
    [<DefaultValue>]
    val mutable ResourceController:ResourceController

    member this.Start() =
        this.ResourceController.UpdateResources DildSild MildAbild Vildild
        this.ResourceController.UpdateResults 0 0 0
