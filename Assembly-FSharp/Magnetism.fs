namespace UnityFunctional
open UnityEngine
open System

type FRP_MagnetismController() = 
    inherit FRPBehaviour()

    [<SerializeField>]
    let mutable speed = 5.0f

    let moveMagneticBalls (obj:GameObject) (center:GameObject) = async{
        obj.transform.LookAt(center.transform)
        obj.transform.Translate(obj.transform.forward * Time.deltaTime * speed)
        ()
        }

    let tmp (objs:GameObject[]) (center:GameObject) =
        objs
        |> Array.map (fun i -> moveMagneticBalls i center)
        |> Async.Parallel
        |> Async.RunSynchronously
        ()

    member this.Start() = 
        let center = GameObject.Instantiate<GameObject>(new GameObject())
        let Balls = GameObject.FindGameObjectsWithTag("Magnetic")
        Debug.Log("There are " + Balls.Length.ToString() + " magnetic balls")
        this.ReactTo (
            Update,
            (fun () -> tmp Balls center ))

    



