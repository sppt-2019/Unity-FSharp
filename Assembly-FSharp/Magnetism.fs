namespace UnityFunctional
open UnityEngine
open System

type FRP_MagnetismController() = 
    inherit FRPBehaviour()

    member this.Start() = 
        let Balls = GameObject.FindGameObjectsWithTag("Magnetic")
        Debug.Log("There are " + Balls.Length.ToString() + " magnetic balls")

