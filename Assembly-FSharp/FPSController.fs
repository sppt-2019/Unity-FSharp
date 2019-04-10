namespace UnityFunctional
open UnityEngine
open System

type FRP_FPSController() =
    inherit FRPBehaviour()

    [<SerializeField>]
    let mutable _velocity = 5.0f;

    member this.Start() =
        Debug.Log("Hello from F#")
        this.ReactTo(FRPEvent.Update, fun()->this.MyUpdate());
        this.ReactTo(FRPEvent.Keyboard, (fun()->Input.GetKey(KeyCode.W)),(fun()->this.HandleMoveForward()))
        this.ReactTo(FRPEvent.Keyboard, (fun()->Input.GetKey(KeyCode.S)),(fun()->this.HandleMoveBack()))
        this.ReactTo(FRPEvent.Keyboard, (fun()->Input.GetKey(KeyCode.A)),(fun()->this.HandleMoveLeft()))
        this.ReactTo(FRPEvent.Keyboard, (fun()->Input.GetKey(KeyCode.D)),(fun()->this.HandleMoveRight()))
        this.ReactTo(FRPEvent.Keyboard, (fun()->Input.GetKey(KeyCode.Space)),(fun()->this.HandleJump()))

    
    member this.MyUpdate() =
        Debug.Log("Update")

    member this.HandleMoveForward() =
        let newPosition = this.transform.position + new Vector3(0.0f, 0.0f, _velocity)
        this.transform.position <- newPosition

    member this.HandleMoveBack() =
        let newPosition = this.transform.position + new Vector3(0.0f, 0.0f, -_velocity)
        this.transform.position <- newPosition

    member this.HandleMoveRight() =
        let newPosition = this.transform.position + new Vector3(_velocity, 0.0f, 0.0f)
        this.transform.position <- newPosition

    member this.HandleMoveLeft() =
        let newPosition = this.transform.position + new Vector3(-_velocity, 0.0f, 0.0f)
        this.transform.position <- newPosition

    member this.HandleJump() =
        let jumpForce = new Vector3(0.0f, _velocity, 0.0f)
        this.GetComponent<Rigidbody>().AddForce(jumpForce)