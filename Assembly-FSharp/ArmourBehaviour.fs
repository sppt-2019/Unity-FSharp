namespace UnityFunctional
open UnityEngine
open System

type FRP_ArmourBehaviour() =
    inherit FRPBehaviour()

    member this.Start() =
        let allItems = ItemStore.AllItems()
        Debug.Log(String.Join("\n", allItems))

        let groupedItems = ItemStore.GroupedItems()
        Debug.Log(String.Join("\n", groupedItems))

        let agilityTotal = ItemStore.AllItems() |> Array.map(fun item -> item.Agility) |> Array.sum
        Debug.Log(agilityTotal.ToString())

        let intTotal = ItemStore.AllItems() |> Array.map(fun item -> item.Intellect) |> Array.sum
        Debug.Log(intTotal.ToString())

        let strengthTotal = ItemStore.AllItems() |> Array.map(fun item -> item.Strength) |> Array.sum
        Debug.Log(strengthTotal.ToString())

        let total (items:Item[]) (getter:Item->int) =
            items |> Array.map(fun item -> getter(item)) |> Array.sum

        Debug.Log(total allItems (fun item -> item.Agility))
               
        let agiMod = (List.ofSeq groupedItems.[0].Items) |> List.map(fun item -> item.AgilityMod) |> List.sum
        
        Debug.Log(agiMod.ToString())

        let getGroupMod (items:Item list) (getter:Item->float32) =
            items |> List.map(fun item -> getter(item)) |> List.sum

        let agiMod = getGroupMod (List.ofSeq groupedItems.[0].Items) (fun item -> item.AgilityMod)
        let agiTotal = total (Array.ofSeq groupedItems.[0].Items) (fun item -> item.Agility)
        let agiTotalWithMod = agiMod * (float32) agiTotal

        Debug.Log(agiTotalWithMod.ToString())

        let getTotalWithMod (items:Item list) (attribute:Item->int) (attributeMod:Item->float32) =
            let modifier = getGroupMod items attributeMod
            let attributeTotal = total (Array.ofSeq items) attribute
            modifier * (float32) attributeTotal

        Debug.Log((getTotalWithMod (List.ofSeq groupedItems.[0].Items) (fun item -> item.Agility) (fun item -> item.AgilityMod)).ToString())

        Debug.Log("Last")