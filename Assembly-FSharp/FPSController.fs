namespace UnityFunctional
open UnityEngine
open System

type FRP_FPSController() =
    inherit FRPBehaviour()

    [<SerializeField>]
    let mutable MoveSpeed = 5.0f
    [<SerializeField>]
    let mutable RotationSpeed = 25.0f
    [<SerializeField>]
    let mutable JumpSpeed = 8.0f

    [<SerializeField>]
    let mutable Camera:Camera = null

    member this.Start() =
        let rigidbody = this.GetComponent<Rigidbody>()

        this.ReactTo (
            FRPEvent.Keyboard, 
            (fun () -> Input.GetKeyDown KeyCode.Space),
            (fun () -> 
                rigidbody.velocity <- rigidbody.velocity + (Vector3.up * JumpSpeed)
        ))

        this.ReactTo<float32*float32>(FRPEvent.MoveAxis,
            (fun a -> 
                let (hori, vert) = a.Deconstruct()
                let rec getStep axis = Time.deltaTime * MoveSpeed * axis
                let moveVec = (this.transform.forward * getStep vert) + (this.transform.right * getStep hori)
                rigidbody.MovePosition(rigidbody.transform.position + moveVec)
            )
        )

        this.ReactTo<float32*float32>(FRPEvent.MouseMove,
            (fun mouseAxes ->
                let (hori, vert) = mouseAxes.Deconstruct()
                let rec getRotationStep axis = Time.deltaTime * RotationSpeed * axis
                Camera.transform.Rotate(-getRotationStep vert, 0.0f, 0.0f)
                this.transform.Rotate(0.0f, getRotationStep hori, 0.0f)
            )
        )
