namespace UnityFunctional
open UnityEngine
open System

type FRP_FPSController() =
    inherit MonoBehaviour()

    member this.start() =
        Debug.Log("Hello from F#")

