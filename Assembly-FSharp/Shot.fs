namespace UnityFunctional

open UnityEngine
open System

[<AllowNullLiteral>]
type FRP_Shot() = 
    inherit FRPBehaviour()

    [<SerializeField>]
    let mutable Speed:float32 = 5.0f
    let mutable _hasExitedSpawnerCollider = false;

    member this.HasExitedSpawnerCollider 
        with get() = _hasExitedSpawnerCollider

    member this.Start() =
        GameObject.Destroy (this.gameObject, 5.0f)
        Debug.Log("shot.Start()")

        this.ReactTo (FRPEvent.Update, 
            (fun () -> 
                let trans = this.transform
                trans.position <- trans.position + (trans.forward * Speed * Time.deltaTime)))

        this.ReactTo<Collision> (FRPEvent.CollisionEnter,
            (fun c -> this.HasExitedSpawnerCollider),
            (fun c -> GameObject.Destroy(this.gameObject))
        )

        this.ReactTo<Collision> (FRPEvent.CollisionExit,
            (fun c -> 
                Debug.Log("Collision in shot")
                _hasExitedSpawnerCollider <- true)
        )