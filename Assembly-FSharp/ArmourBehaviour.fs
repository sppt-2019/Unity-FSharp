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
        
        let mutable inte = 0
        let mutable agi = 0
        let mutable str = 0
        
        for it in i do
            inte <- inte + it.Intellect
            agi <- agi + it.Agility
            str <- str + it.Strength
        
        Debug.Log(inte)
        Debug.Log(agi)
        Debug.Log(str)