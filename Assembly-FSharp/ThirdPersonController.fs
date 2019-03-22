namespace UnityFunctional
open UnityEngine
open System

type FRP_ThirdPersonController() =
    inherit FRPBehaviour()

    [<SerializeField>]
    let mutable JumpSpeed = 3.0f
    [<SerializeField>]
    let mutable MoveSpeed = 3.0f

    [<SerializeField>]
    let mutable RotationSpeed = 25.0f
    [<SerializeField>]
    let mutable Camera:Camera = null
    [<SerializeField>]
    let mutable HorizontalRotator:Transform = null
    [<SerializeField>]
    let mutable CameraRotationSpeed = 50.0f

    member this.Start() =
        this.ReactTo (
            FRPEvent.Keyboard, 
            (fun () -> Input.GetKeyDown KeyCode.Space && not (Input.GetKey KeyCode.LeftControl)),
            (fun () -> 
                let rb = this.GetComponent<Rigidbody>()
                rb.velocity <- rb.velocity + (Vector3.up * JumpSpeed)
        ))
        this.ReactTo<float32*float32> (
            FRPEvent.MoveAxis,
            (fun s ->
                let (deltaX, deltaY) = s.Deconstruct()
                let rb = this.GetComponent<Rigidbody>()

                rb.MovePosition(rb.transform.position + rb.transform.forward * (MoveSpeed * Time.deltaTime * deltaY))
                rb.transform.Rotate(0.0f, RotationSpeed * Time.deltaTime * deltaX, 0.0f))
        )

        this.ReactTo (
            FRPEvent.Keyboard,
            (fun () -> Input.GetKey KeyCode.LeftControl && Input.GetKeyDown KeyCode.Space),
            (fun () -> 
                Debug.Log("Ctrl + Space")
            )
        )

        this.ReactTo<float32*float32> (
            FRPEvent.MouseMove,
            (fun m -> 
                let (deltaX, deltaY) = m.Deconstruct()
                Camera.transform.RotateAround(this.transform.position, Vector3.up, CameraRotationSpeed * Time.deltaTime * deltaX)
                Camera.transform.RotateAround(this.transform.position, Camera.transform.right, CameraRotationSpeed * Time.deltaTime * deltaY)
                //Todo: Rotate camera with vertical mouse movements
            )
        )