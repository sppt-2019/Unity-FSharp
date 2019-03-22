namespace UnityFunctional
open UnityEngine
open System

type FRP_ArmourBehaviour() =
    inherit FRPBehaviour()

    member this.Start() =
        let i = ItemStore.AllItems()
        Debug.Log(String.Join("\n", i))

        let g = ItemStore.GroupedItems()
        Debug.Log(String.Join("\n", g))