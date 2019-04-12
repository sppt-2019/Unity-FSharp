namespace UnityFunctional
open UnityEngine
open System

type FRP_MagnetismController() = 
    inherit FRPBehaviour()

    [<SerializeField>]
    let magnitude = 0.1f;

    let update (balls:GameObject[]) (centre:Vector3) =
        for i in 0 .. balls.Length - 1 do
            balls.[i].transform.position <- balls.[i].transform.position * (1.0f - magnitude) + new Vector3(centre.x, balls.[i].transform.position.y, centre.z) * magnitude;

    member this.Start() = 
        let Balls = GameObject.FindGameObjectsWithTag("Magnetic")
        Debug.Log("There are " + Balls.Length.ToString() + " magnetic balls")
        let mutable minZ = Mathf.Infinity
        let mutable minX = Mathf.Infinity
        let mutable maxZ = Mathf.NegativeInfinity
        let mutable maxX = Mathf.NegativeInfinity
        for i in 0 .. Balls.Length - 1 do
            if minZ > Balls.[i].transform.position.z then
                minZ <- Balls.[i].transform.position.z
            if minX > Balls.[i].transform.position.x then
                minX <- Balls.[i].transform.position.x
            if maxZ < Balls.[i].transform.position.z then
                maxZ <- Balls.[i].transform.position.z
            if maxX < Balls.[i].transform.position.x then
                maxX <- Balls.[i].transform.position.x
        let centre = new Vector3((maxX - minX) * 0.5f, 0.0f, (maxZ - minZ) * 0.5f);
        this.ReactTo (
            FRPEvent.Update,
            (fun () -> true),
            (fun () -> update Balls centre))
            
