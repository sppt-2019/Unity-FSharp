namespace UnityFunctional
open UnityEngine

type Node() =
    let mutable name:string = ""
    let mutable line:string = ""
    let mutable children:string list = []
    
    member val Name = name with get, set    
    member val Line = line with get, set
    member val Children = children with get, set
    