namespace UnityFunctional
open UnityEngine
open System

type FRP_ThirdPersonController() =
    inherit FRPBehaviour()

    member this.Start() =
        Debug.Log("Hellow from 3rd person controller")