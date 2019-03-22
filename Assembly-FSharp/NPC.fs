namespace UnityFunctional
open UnityEngine
open System.Linq

type SpeechNode(name:string, line:string) =
    let mutable children = null
    
    member this.Name with get():string = name
    member this.Line with get():string = line
    member this.Children
        with get() = children
        and set(v) = children <- v

type FRP_NPC() =
    inherit FRPBehaviour()
    let tree = Node.GetTree()
    
    let mutable frontline = []
    let mutable expandedNodes = []
    
    [<SerializeField>]
    let mutable OutText:UnityEngine.UI.Text = null
    
//    let rec unfold (node:Node) (visitedNodes:SpeechNode list) (depth:int) =
//        frontline <- [node]
//            
//        while not List.empty frontline do
//            let n = List.head frontline
//            if List.contains n expandedNodes then ()
//            else
//                let sn = SpeechNode(node.Name, node.Line)
//                frontline <- List.append frontline n.ChildNameNames.Select(fun n -> tree.Find(fun tn -> tn.Name = n))
//            
//            let c = node.ChildNameNames.Select(fun cn ->
//                let en = visitedNodes |> List.tryFind (fun en -> en.Name = cn)
//                match en with
//                | Some e -> e
//                | None -> unfold (tree.Find(fun n -> n.Name = cn)) (n :: visitedNodes) (depth + 1))
//                
//            n.Children <- c
//            n
//    
//    member this.Start() =
//        Debug.Log("Dialog Tree")
//        Debug.Log(tree)
//        
//        let graph = unfold tree.[0] [] 0
//        
//        OutText.text <- graph.Name + ":\n" + graph.Line