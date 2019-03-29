namespace UnityFunctional
open UnityEngine
open UnityEngine.UI
    
type FRP_DialogueTree() =
    inherit FRPBehaviour()
    
    member this.Start() =
        let dialogOptions = Node.GetExampleDialog()
        ()