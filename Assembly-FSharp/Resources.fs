﻿namespace UnityFunctional

open UnityEngine
open System

type FRP_Resources() =
    inherit FRPBehaviour()

    [<SerializeField>]
    let mutable DildSild:int = 32
    [<SerializeField>]
    let mutable MildAbild:int = 45
    [<SerializeField>]
    let mutable Vildild:int = 5
    [<DefaultValue>]
    val mutable ResourceController:FRP_ResourceController

    member this.Start() =
        this.ResourceController.UpdateResources DildSild MildAbild Vildild
        this.ResourceController.UpdateResults 0 0 0
