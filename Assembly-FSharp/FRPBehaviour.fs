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
type FRPBehaviour() =
    inherit MonoBehaviour()

    let KeyboardEvent = new Event<_>()
    let MouseClickEvent = new Event<_>()
    let MouseMoveEvent = new Event<float32*float32>()
    let MoveAxisEvent = new Event<float32*float32>()
    let UpdateEvent = new Event<_>()
    let CollisionEnterEvent = new Event<Collision>()
    let CollisionExitEvent = new Event<Collision>()
    let TriggerEnterEvent = new Event<Collider>()
    let TriggerExitEvent = new Event<Collider>()
    
    static let mouseButtons =
        let mb = Enum.GetValues(typeof<MouseButton>)
        [mb.GetLowerBound 0 .. mb.GetUpperBound 0]

    static let keyboardButtons =
        let keys = Enum.GetValues (typeof<KeyCode>)
        [keys.GetLowerBound 0 .. keys.GetUpperBound 0]
        
    
    let AnyMouseButton () =
        mouseButtons
        |> List.exists (fun b -> Input.GetMouseButton b)

    let AnyKeyboardKey () =
        keyboardButtons
        |> List.exists (fun k -> Input.GetKey (enum<KeyCode> k))

    member this.GetEvent<'T> (event:FRPEvent):IEvent<'T> =
        match event with
        | Keyboard  -> KeyboardEvent.Publish :?> IEvent<'T>
        | MouseClick-> MouseClickEvent.Publish :?> IEvent<'T>
        | MouseMove -> MouseMoveEvent.Publish :?> IEvent<'T>
        | Update    -> UpdateEvent.Publish :?> IEvent<'T>
        | MoveAxis  -> MoveAxisEvent.Publish :?> IEvent<'T>
        | TriggerEnter -> TriggerEnterEvent.Publish :?> IEvent<'T>
        | TriggerExit-> TriggerExitEvent.Publish :?> IEvent<'T>
        | CollisionEnter -> CollisionEnterEvent.Publish :?> IEvent<'T>
        | CollisionExit -> CollisionExitEvent.Publish :?> IEvent<'T>

    member this.ReactTo<'T> (event:FRPEvent, condition:('T -> bool), handler:('T -> _)) =
        let e = this.GetEvent<'T> event
        e
        |> Event.filter (condition)
        |> Event.add (handler)

    member this.ReactTo<'T> (event:FRPEvent, handler:('T -> _)) =
        let e = this.GetEvent<'T> event
        e
        |> Event.add (handler)

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

    member this.OnCollisionEnter(collision:Collision) =
        CollisionEnterEvent.Trigger(collision)

    member this.OnCollisionExit(collision:Collision) =
        CollisionExitEvent.Trigger(collision)

    member this.OnTriggerEnter(collider:Collider) =
        TriggerEnterEvent.Trigger(collider)

    member this.OnTriggerExut(collider:Collider) =
        TriggerExitEvent.Trigger(collider)