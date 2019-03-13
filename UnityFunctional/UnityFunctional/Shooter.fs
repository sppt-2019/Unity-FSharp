namespace UnityFunctional
open UnityEngine
open System
open Interfaces

type FRP_Shooter() =
    inherit MonoBehaviour()
    
    [<SerializeField>]
    let mutable Speed:float = 0.0
    [<SerializeField>]
    let mutable RotationSpeed:float = 25.0

    let mutable shotsBeforeStateChange:int = 5
    let mutable attackTarget:Transform = null
    let mutable moveTarget:Vector3 = Vector3.zero
    let mutable cooldowner = 0.0

    interface IStateMachineEntity with
        member this.name = base.name
        [<SerializeField>]
        member this.Speed = Speed
        [<SerializeField>]
        member this.RotationSpeed= RotationSpeed
        [<SerializeField>]
        member this.ShotsBeforeStateChange
            with get() = shotsBeforeStateChange
            and set(v)= shotsBeforeStateChange <- v
        [<SerializeField>]
        member this.AttackTarget
            with get() = attackTarget
            and set(v) = attackTarget <- v
        [<SerializeField>]
        member this.MoveTarget
            with get() = moveTarget
            and set(v) = moveTarget <- v
        member this.ShotCooldown = 2.0
        member this.Cooldowner
            with get() = cooldowner
            and set(v) = cooldowner <- v

    member this.Start() =
        let stateMachine = GameObject.FindGameObjectWithTag("StateMachine").GetComponent<FRP_StateMachine>();
        stateMachine.JoinState(this, State.Moving)