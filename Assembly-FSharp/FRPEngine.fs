namespace UnityFunctional
open UnityEngine
open System

type FRPEvent =
    | Keyboard
    | MouseClick
    | MouseMove
    | Update
    | MoveAxis
    | CollisionEnter
    | CollisionExit
    | TriggerEnter
    | TriggerExit

type MouseButton =
    | Left = 0
    | Middle = 1
    | Right = 2
    
[<AllowNullLiteral>]
type IFRPBehaviour = 
    abstract member GetCollisionEvent<'T> : FRPEvent->IEvent<'T>

[<Sealed>]
type FRPEngine () =
    inherit MonoBehaviour()
    
    let KeyboardEvent = new Event<_>()
    let MouseClickEvent = new Event<_>()
    let MouseMoveEvent = new Event<float32*float32>()
    let MoveAxisEvent = new Event<float32*float32>()
    let UpdateEvent = new Event<_>()

    let AnyMouseButton () =
        let mouseButtons = Enum.GetValues(typeof<MouseButton>)
        [mouseButtons.GetLowerBound 0..mouseButtons.GetUpperBound 0]
        |> List.exists (fun b -> Input.GetMouseButton b)

    let AnyKeyboardKey () =
        let keys = Enum.GetValues (typeof<KeyCode>)
        let lb = keys.GetLowerBound 0
        let ub = keys.GetUpperBound 0
        [lb..ub]
        |> List.exists (fun k -> Input.GetKey (enum<KeyCode> k))
    
    [<DefaultValue>]
    static val mutable private Instance:FRPEngine
        
    member this.GetEvent<'T> (behaviour:IFRPBehaviour) (event:FRPEvent):IEvent<'T> =
        match event with
        | Keyboard  -> KeyboardEvent.Publish :?> IEvent<'T>
        | MouseClick-> MouseClickEvent.Publish :?> IEvent<'T>
        | MouseMove -> MouseMoveEvent.Publish :?> IEvent<'T>
        | Update    -> UpdateEvent.Publish :?> IEvent<'T>
        | MoveAxis  -> MoveAxisEvent.Publish :?> IEvent<'T>
        | _ -> behaviour.GetCollisionEvent<'T> event

    static member ConditionalReactTo<'T> (behaviour:IFRPBehaviour) (event:FRPEvent) (condition:'T -> bool) (handler:'T -> _) =
        let e = FRPEngine.Instance.GetEvent<'T> behaviour event
        Event.filter (condition) e
        |> Event.add (handler)

    static member ReactTo<'T> (behaviour:IFRPBehaviour) (event:FRPEvent) (handler:'T -> _) =
        let e = FRPEngine.Instance.GetEvent<'T> behaviour event
        Event.add (handler) e
        
    member this.Start() =
        FRPEngine.Instance <- this
        
    member this.Update() =
        if AnyKeyboardKey() then
            KeyboardEvent.Trigger ()
        if AnyMouseButton() then
            MouseClickEvent.Trigger ()
        let mouseX = Input.GetAxis("Mouse X")
        let mouseY = Input.GetAxis("Mouse Y")
        if Mathf.Abs(mouseX) > 0.0f || Mathf.Abs(mouseY) > 0.0f then
            MouseMoveEvent.Trigger(mouseX,mouseY)
        let axisX = Input.GetAxis("Horizontal")
        let axisY = Input.GetAxis("Vertical")
        if Mathf.Abs(axisX) > 0.0f || Mathf.Abs(axisY) > 0.0f then
            MoveAxisEvent.Trigger(axisX,axisY)
    
        UpdateEvent.Trigger()