namespace UnityFunctional
open UnityEngine
open System
open UnityEngine

type FRP_FPSController() =
    inherit FRPBehaviour()
    
    [<SerializeField>]
    let mutable Speed = 5.0f
    
    [<SerializeField>]
    let mutable JumpPower = 5.0f
    
    [<DefaultValue>]
    val mutable Cam:Camera


    member this.Start() =
        Debug.Log("Hello from F#")
        
        this.ReactTo<float32*float32> (
            FRPEvent.MoveAxis,
            (fun m ->
                let (h, v) = m.Deconstruct()
                let mSpeed = new Vector3(Speed * h * Time.deltaTime, 0.0f, Speed * v * Time.deltaTime)
                Debug.Log(mSpeed)
                this.transform.Translate(mSpeed)
            )
        )
        
        this.ReactTo<float32*float32> (
            FRPEvent.MouseMove,
            (fun m ->
                let (h, v) = m.Deconstruct()
                let mSpeed = new Vector3(Speed * v * Time.deltaTime,  Speed * h * Time.deltaTime, 0.0f)
                this.Cam.transform.Rotate(mSpeed)
            )
        )
        
        
        this.ReactTo (
            FRPEvent.Keyboard,                            //Event type
            (fun () -> Input.GetKeyDown(KeyCode.Space)),  //Filtrering
            (fun () -> this.GetComponent<Rigidbody>().AddForce(Vector3.up * JumpPower)))       
        
    

