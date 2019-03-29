namespace UnityFunctional
open UnityEngine
open System

type FRP_FPSController() =
    inherit FRPBehaviour()

    member this.Start() =
        Debug.Log("Hello from F#")

