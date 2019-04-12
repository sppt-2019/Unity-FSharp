namespace UnityFunctional
open UnityEngine
open UnityEngine.UI

type DNode(dial:string,left:DNode,right:DNode) =
    let dialogue : string = dial
    let left  : DNode = left
    let right : DNode = right
    
   
type FRP_DialogueTree() =
    inherit FRPBehaviour()
    
    let mutable treeRoot : DNode = DNode("",,)
        
    let NogetAndet (dia:Node, name:string) = 
        if dia.ChildNames.Count > 0 then
            DNode(dia.Name, NogetAndet(dia)
        else
            [dia]

    member this.Start() =
        let dialogOptions = Node.GetExampleDialog()
        treeRoot <- this.Parse(dialogOptions)
        ()

    member this.Parse (dialogue:Node list) =
        let child = List.find (fun l -> dialogue.[0].ChildNames.[0] == l.Name) dialogue
        DNode(dialogue.[0].Name, [DNode(child.Name, [])])

    
    