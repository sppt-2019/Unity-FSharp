namespace UnityFunctional
open UnityEngine
open System
open UnityFunctional.Interfaces

[<AllowNullLiteral>]
type F_StateMachineMaterials() =
    inherit MonoBehaviour()
    
    [<SerializeField>]
    let mutable stateMaterials:StateMaterial[] = Array.empty
    
    member this.StateMaterial with get() = stateMaterials

type FMBRanger() =
    inherit MonoBehaviour()
    
    [<SerializeField>]
    let mutable Speed = 0.0f
    [<SerializeField>]
    let mutable RotationSpeed = 25.0f
    [<SerializeField>]
    let mutable ShotPrefab:GameObject = null
    
    let mutable StateMaterials:StateMaterialsComponent = null
    
    let mutable MoveTarget:Vector3 = Vector3.zero
    let mutable shotsBeforeStateChange:int = 5
    let mutable attackTarget:Transform = null
    let mutable moveTarget:Vector3 = Vector3.zero
    let mutable cooldowner = 0.0f
    
    let mutable state:State = State.Moving
    let mutable Cooldowner = 2.0f
    let ShotCooldown = 2.0f
    let mutable ShotsBeforeStateChange = 5
    
    member this.Start () =
        StateMaterials <- GameObject.FindGameObjectWithTag("Plastic").GetComponent<StateMaterialsComponent>();
        this.InitEntityState state
    
    member this.Move () =
        let trans = this.transform
        if Vector3.Distance(trans.position, MoveTarget) < 0.1f then
            State.Attacking
        else
            trans.LookAt(MoveTarget)
            trans.position <- trans.position + (trans.forward * Speed * Time.deltaTime)
            State.Moving

    member this.ShootAt (target:Transform) =
        let pos = this.transform.position
        let lookAtTransform = new Vector3(target.position.x, pos.y, target.position.z)
        let shot = GameObject.Instantiate(ShotPrefab) :?> GameObject
        shot.transform.position <- pos
        shot.transform.LookAt(lookAtTransform, Vector3.up)

    member this.Shoot () =
        Cooldowner <- Cooldowner - Time.deltaTime

        if ShotsBeforeStateChange = 0 then
            State.Moving
        elif Cooldowner <= 0.0f then
            this.ShootAt attackTarget
            ShotsBeforeStateChange <- ShotsBeforeStateChange - 1
            Cooldowner <- ShotCooldown
            State.Attacking
        else
            State.Attacking
            
    member this.Flee () =
        Cooldowner <- Cooldowner - Time.deltaTime
        if Cooldowner <= 0.0f then
            State.Attacking
        else
            let trans = this.transform
            let step = Speed * Time.deltaTime * 2.0f
            trans.Rotate(0.0f, UnityEngine.Random.Range(-RotationSpeed, RotationSpeed), 0.0f)
            trans.position <- trans.position + trans.forward * step
            State.Fleeing
    
    member this.InitEntityState (state:State) =
        let stateMaterial = StateMaterials.StateMaterials |> Array.find (fun sm -> sm.State = state)
        this.GetComponent<Renderer>().material <- stateMaterial.Material

        match state with
        | State.Moving -> 
            MoveTarget <- Vector3(Random.Range(-3.7f, 5.7f), this.transform.position.y, Random.Range(-6.0f, 3.4f))
        | State.Fleeing ->
            Cooldowner <- 4.0f;
        | State.Attacking ->
            ShotsBeforeStateChange <- 5;
            Cooldowner <- 0.0f;
            attackTarget <- GameObject.FindGameObjectWithTag("Tower").transform;
        | _ -> invalidArg "state" "Out of bounds in enum State. Correct values are Moving, Fleeing or Shooting"

    member this.Update () =
        let newState =
            match state with
            | State.Moving -> this.Move ()
            | State.Attacking -> this.Shoot ()
            | State.Fleeing -> this.Flee ()
            | _ -> invalidArg "state" "Out of bounds in enum State. Correct values are Moving, Fleeing or Shooting"
            
        if newState = state |> not then
            this.InitEntityState newState
            state <- newState
    
    member this.OnCollisionEnter (collision:Collision) =
        if collision.collider.name = "Boundary" then
            this.transform.Rotate(0.0f, 180.0f, 0.0f)
        else   
            let s = collision.collider.GetComponent<FRP_Shot>()
            if s = null |> not && s.HasExitedSpawnerCollider then
                state <- State.Fleeing
                this.InitEntityState state