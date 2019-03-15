namespace UnityFunctional
open UnityEngine
open System

type FRP_ThirdPersonController() =
    inherit FRPBehaviour()

    member this.Start() =
        this.ReactTo (
            FRPEvent.Keyboard, 
            (fun a -> Input.GetKeyDown(KeyCode.Space)),
            (fun s -> 
                Debug.Log("FRP message: " + s)))

        this.ReactTo (
            FRPEvent.MouseClick,
            (fun a -> Input.GetMouseButtonDown(0)),
            (fun s ->
                Debug.Log("FRP message: " + s)
        ))
