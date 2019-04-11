namespace UnityFunctional
open UnityEngine
open System

type FRP_FPSController() =
    inherit FRPBehaviour()

    [<SerializeField>]
    let mutable Speed = 200.0f

    [<SerializeField>]
    let mutable JumpPower = 200.0f

    [<SerializeField>]
    let mutable MouseYSensitivity = 1.0f

    [<SerializeField>]
    let mutable MouseXSensitivity = 1.0f

    let mutable MovementVector = new Vector3()


    member this.Start() =
        Debug.Log("Hello from F#")
        this.ReactTo (
            FRPEvent.Keyboard,                            //Event type
            (fun () -> Input.GetKeyDown(KeyCode.W)),  //Filtrering
            (fun () -> 
            MovementVector <- Vector3.forward*Speed
            this.GetComponent<Rigidbody>().velocity <- MovementVector 
            )
            )               //Handler

        this.ReactTo<float32*float32> (
                FRPEvent.MouseMove,
                (fun m ->
                let (x, y) = m.Deconstruct()
                this.transform.Rotate(x,y,0.0f)         
                )
        
        )
        
        

   