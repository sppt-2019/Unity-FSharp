namespace UnityFunctional
open UnityEngine
open System
open System.Linq

[<AbstractClass; Sealed>]
type ItemStore private () =
    static member AllItems() =
       Resources.LoadAll<Item>("Items")

    static member GroupedItems() =
        let items = ItemStore.AllItems()
        [
            Group(ItemGroup.Hands, items.Where(fun i -> i.Group = ItemGroup.Hands));
            Group(ItemGroup.Head, items.Where(fun i -> i.Group = ItemGroup.Head));
            Group(ItemGroup.Legs, items.Where(fun i -> i.Group = ItemGroup.Legs));
            Group(ItemGroup.Torso, items.Where(fun i -> i.Group = ItemGroup.Torso))
        ]