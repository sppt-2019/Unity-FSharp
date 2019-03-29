namespace UnityFunctional
open UnityEngine

type Dialog =
| Option of Line:string * Children:Dialog list
| Consequence of Line:string * Reaction:Node.DialogOutcome

type DialougeTree () =
    inherit FRPBehaviour()
    
    let rec BuildTree (node:Node) (nodes:Node list) =
        if node.ChildNames = null || node.ChildNames.Count = 0 then
            Consequence (node.Line, node.Outcome)
        else
            Option(node.Line,
                   node.ChildNames
                   |> List.ofSeq
                   |> List.map (fun struct (name, line) -> BuildTree (nodes |> List.find (fun n -> n.Name = name)) nodes))
            
    let rec walk node func =
        match node with
        | Option (_, c) ->
            func node
            List.iter (fun n -> walk n func) c
        | Consequence (_, _) ->
            func node
    
    member this.Start() =
        let nodes = Node.GetTree()
        let t = BuildTree nodes.[0] (List.ofSeq nodes)
        match t with
        | Option (l, c) ->
            Debug.Log(l + " has " + c.Length.ToString() + " children")
        | Consequence (l, r) ->
            Debug.Log(l + " ends the dialog with reaction: " + r.ToString())
        
        let mutable giftNodes = []
        walk t (fun n ->
            match n with
            | Consequence (_, Node.DialogOutcome.Gift) -> giftNodes <- List.append [n] giftNodes
            | _ -> ())