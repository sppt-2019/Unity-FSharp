namespace UnityFunctional
open UnityEngine
open System

[<AllowNullLiteral>]
[<AbstractClass>]
type FRPBehaviour() =
    inherit MonoBehaviour()
    
    let CollisionEnterEvent = new Event<Collision>()
    let CollisionExitEvent = new Event<Collision>()
    let TriggerEnterEvent = new Event<Collider>()
    let TriggerExitEvent = new Event<Collider>()
    
    interface IFRPBehaviour with
        member this.GetCollisionEvent<'T> (event:FRPEvent):IEvent<'T> =
            match event with
            | TriggerEnter -> TriggerEnterEvent.Publish :?> IEvent<'T>
            | TriggerExit-> TriggerExitEvent.Publish :?> IEvent<'T>
            | CollisionEnter -> CollisionEnterEvent.Publish :?> IEvent<'T>
            | CollisionExit -> CollisionExitEvent.Publish :?> IEvent<'T>
            | _ -> invalidArg "event" "The given event type is not collision-based."
    
    member this.ConditionalReactTo<'T> (event:FRPEvent) (condition:'T -> bool) (handler:'T -> _) =
        FRPEngine.ConditionalReactTo this event condition handler
    
    member this.ReactTo<'T> (event:FRPEvent) (handler:'T -> _) =
        FRPEngine.ReactTo this event handler
    
    member this.OnCollisionEnter(collision:Collision) =
        CollisionEnterEvent.Trigger(collision)
    
    member this.OnCollisionExit(collision:Collision) =
        CollisionExitEvent.Trigger(collision)
    
    member this.OnTriggerEnter(collider:Collider) =
        TriggerEnterEvent.Trigger(collider)
    
    member this.OnTriggerExut(collider:Collider) =
        TriggerExitEvent.Trigger(collider)