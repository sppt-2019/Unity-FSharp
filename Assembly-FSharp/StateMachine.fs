namespace UnityFunctional

open UnityEngine
open System
open Interfaces

[<Serializable>]
type State = Fleeing=0 | Moving=1 | Shooting=2

[<Serializable>]
type StateMaterial() = 
    [<SerializeField>]
    let mutable State:State = State.Fleeing
    [<SerializeField>]
    let mutable Material:Material = null

type FRP_StateMachine() =
    inherit MonoBehaviour()

    [<SerializeField>]
    let mutable StateMaterials:StateMaterial[] = null
    [<SerializeField>]
    let mutable ShotPrefab:GameObject = null

    member this.JoinState(ranger:IStateMachineEntity, state:State) = 
        Debug.Log(ranger.name + " joined state " + state.ToString());
