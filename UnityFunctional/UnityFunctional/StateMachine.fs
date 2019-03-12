namespace UnityFunctional
open UnityEngine
open System

type State = Fleeing | Moving | Shooting

type FRP_StateMachine() =
    inherit MonoBehaviour()

    member this.JoinState(gameObject:GameObject, state:State) = 
        Debug.Log(gameObject.name + " joined state " + state.ToString());
