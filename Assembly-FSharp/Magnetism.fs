namespace UnityFunctional
open UnityEngine
open System

type FRP_MagnetismController() = 
    inherit FRPBehaviour()

    [<SerializeField>]
    let mutable MoveSpeed = 5.0f

    let getCenter (balls:GameObject[]):Vector3 =
        let summedPositions = 
            balls
            |> Array.map (fun b -> b.transform.position)
            |> Array.reduce (fun acc elm -> acc + elm)
        
        let fLength = float32(balls.Length)
        new Vector3(summedPositions.x / fLength, summedPositions.y / fLength, summedPositions.z / fLength)

    let lookAt(ball:GameObject, point:Vector3):GameObject = 
        ball.transform.LookAt(point)
        ball

    let step (ball:GameObject):GameObject =
        let getStep = Time.deltaTime * MoveSpeed
        ball.GetComponent<Rigidbody>().MovePosition(ball.transform.position + ball.transform.forward * getStep)
        ball

    member this.Start() = 
        let balls = GameObject.FindGameObjectsWithTag("Magnetic")

        this.ReactTo FRPEvent.Update
            (fun () -> 
                let center:Vector3 = getCenter(balls)
                let updatedBalls = 
                    balls
                    |> Array.map (fun b -> lookAt(b, center))
                    |> Array.map step
                Debug.Log("Them balls were updated")
            )
