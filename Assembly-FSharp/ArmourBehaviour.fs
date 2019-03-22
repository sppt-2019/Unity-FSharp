namespace UnityFunctional
open UnityEngine
open System

type FRP_ArmourBehaviour() =
    inherit FRPBehaviour()

    let sum (triplet1:'a*'a*'a) (triplet2:'a*'a*'a) =
        let (a1, b1, c1) = triplet1.Deconstruct()
        let (a2, b2, c2) = triplet2.Deconstruct()
        (a1+a2,b1+b2,c1+c2)
        
    let fSum (triplet1:float32*float32*float32) (triplet2:float32*float32*float32) =
        let (a1, b1, c1) = triplet1.Deconstruct()
        let (a2, b2, c2) = triplet2.Deconstruct()
        (a1+a2,b1+b2,c1+c2)
    
    let totalStats (armour:Item[]) =
        armour
        |> Array.map (fun a -> (a.Agility, a.Intellect, a.Strength))
        |> Array.reduce (fun acc elm ->
            sum acc elm)
        
    let scaledStats (groups:Group list) =
        groups
        |> List.map (fun g ->
                      List.ofSeq(g.Items)
                      |> List.map (fun i -> (i.Group,(i.AgilityMod, i.IntellectMod, i.StrengthMod),(i.Agility, i.Intellect, i.Strength)))
                      |> List.reduce (fun acc elm ->
                          let (gr,aMod,aStats) = acc.Deconstruct()
                          let (gr,md,stats) = elm.Deconstruct()
                          (gr,(fSum aMod md), (sum aStats stats))))
        |> List.map (fun i ->
            let (grp,(agiMod,intMod,strMod),(agi,int,str)) = i.Deconstruct()
            (grp, float32(agi) * agiMod, float32(int)*intMod, float32(str) * strMod))
    
    member this.Start() =
        let i = ItemStore.AllItems()
        let (agi, int, str) = totalStats(i)
        Debug.Log("Agility: " + agi.ToString())
        Debug.Log("Intellect: " + int.ToString())
        Debug.Log("Strength: " + str.ToString())

        let g = ItemStore.GroupedItems()
        let ss = scaledStats(g)
        
        ss
        |> List.iter (fun gs ->
            let (gr, sAgi,sInt,sStr) = gs.Deconstruct()
            Debug.Log(gr.ToString() + "\n" + String.Join("\n", ["Agility: " + sAgi.ToString(); "Intellect: " + sInt.ToString(); "Strength: "+ sStr.ToString()])))