namespace UnityFunctional
open UnityEngine
open System

type FRP_MagnetismController() = 
    inherit UnityEngine.MonoBehaviour()

    member this.Start() = 
        let Balls = GameObject.FindGameObjectsWithTag("Magnetic")
        Debug.Log("There are " + Balls.Length.ToString() + " magnetic balls")

