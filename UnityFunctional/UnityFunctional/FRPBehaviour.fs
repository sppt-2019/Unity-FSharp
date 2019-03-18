namespace UnityFunctional
open UnityEngine
open System
open System.Linq

type FRPEvent =
    | Keyboard
    | MouseClick
    | MouseMove
    | Update
    | MoveAxis

type MouseButton =
    | Left = 0
    | Middle = 1
    | Right = 2

type FRPBehaviour() =
    inherit MonoBehaviour()

    let KeyboardEvent = new Event<string>()
    let MouseClickEvent = new Event<string>()
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

    member this.GetEvent<'T> (event:FRPEvent):IEvent<'T> =
        match event with
        | Keyboard  -> KeyboardEvent.Publish :?> IEvent<'T>
        | MouseClick-> MouseClickEvent.Publish :?> IEvent<'T>
        | MouseMove -> MouseMoveEvent.Publish :?> IEvent<'T>
        | Update    -> UpdateEvent.Publish :?> IEvent<'T>
        | MoveAxis  -> MoveAxisEvent.Publish :?> IEvent<'T>

    member this.ReactTo<'T> (event:FRPEvent, condition:('T -> bool), handler:('T -> unit)) =
        let e = this.GetEvent<'T> event
        e
        |> Event.filter (condition)
        |> Event.add (handler)

    member this.ReactTo<'T> (event:FRPEvent, handler:('T -> unit)) =
        let e = this.GetEvent<'T> event
        e
        |> Event.add (handler)

    member this.Update() =
        if AnyKeyboardKey() then
            KeyboardEvent.Trigger ("Keyboard Event")
        if AnyMouseButton() then
            MouseClickEvent.Trigger ("Mouse Click Event")
        let mouseX = Input.GetAxis("Mouse X")
        let mouseY = Input.GetAxis("Mouse Y")
        if Mathf.Abs(mouseX) > 0.0f || Mathf.Abs(mouseY) > 0.0f then
            MouseMoveEvent.Trigger(mouseX,mouseY)
        let axisX = Input.GetAxis("Horizontal")
        let axisY = Input.GetAxis("Vertical")
        if Mathf.Abs(axisX) > 0.0f || Mathf.Abs(axisY) > 0.0f then
            MoveAxisEvent.Trigger(axisX,axisY)

        UpdateEvent.Trigger()