namespace UnityFunctional

open UnityEngine
open System
open Interfaces

[<Serializable>]
type State = Fleeing=0 | Moving=1 | Shooting=2

[<Serializable>]
type StateMaterial() = 
    [<SerializeField>]
    let mutable State:State = State.Fleeing
    [<SerializeField>]
    let mutable Material:Material = null

    member this.GetState() = State
    member this.GetMaterial() = Material

type FRP_StateMachine() =
    inherit FRPBehaviour()

    [<SerializeField>]
    let mutable StateMaterials:StateMaterial[] = null
    [<SerializeField>]
    let mutable ShotPrefab:GameObject = null
    
    let move (entity:IStateMachineEntity) =
        if Vector3.Distance(entity.transform.position, entity.MoveTarget) < 0.1f then
            (State.Shooting, entity)
        else
            entity.transform.LookAt(entity.MoveTarget)
            entity.transform.position <- entity.transform.position + (entity.transform.forward * entity.Speed * Time.deltaTime)
            (State.Moving, entity)

    let shootAt (entity:IStateMachineEntity) (target:Transform) =
        let lookAtTransform = new Vector3(target.position.x, entity.transform.position.y, target.position.z)
        let shot = GameObject.Instantiate(ShotPrefab) :?> GameObject
        shot.transform.position <- entity.transform.position
        shot.transform.LookAt(lookAtTransform, Vector3.up)

    let shoot (entity:IStateMachineEntity) =
        entity.Cooldowner <- entity.Cooldowner - Time.deltaTime

        if entity.ShotsBeforeStateChange = 0 then
            (State.Moving, entity)
        elif entity.Cooldowner <= 0.0f then
            shootAt entity entity.AttackTarget
            entity.ShotsBeforeStateChange <- entity.ShotsBeforeStateChange - 1
            entity.Cooldowner <- entity.ShotCooldown
            (State.Shooting, entity)
        else
            (State.Shooting, entity)
            
    let flee (entity:IStateMachineEntity) =
        entity.Cooldowner <- entity.Cooldowner - Time.deltaTime
        if entity.Cooldowner <= 0.0f then
            (State.Shooting, entity)
        else 
            let step = entity.Speed * Time.deltaTime * 2.0f
            entity.transform.Rotate(0.0f, UnityEngine.Random.Range(-entity.RotationSpeed, entity.RotationSpeed), 0.0f)
            entity.transform.position <- entity.transform.position + entity.transform.forward * step
            (State.Fleeing, entity)
               
    let initEntityState (entity:IStateMachineEntity) (state:State) =
        let stateMaterial = StateMaterials |> Array.find (fun sm -> sm.GetState() = state)
        entity.transform.GetComponent<Renderer>().material <- stateMaterial.GetMaterial()

        match state with
        | State.Moving -> 
            let newTarget = new Vector3(Random.Range(-3.7f, 5.7f), entity.transform.position.y, Random.Range(-6.0f, 3.4f))
            entity.MoveTarget <- newTarget;
        | State.Fleeing ->
            entity.Cooldowner <- 4.0f;
        | State.Shooting ->
            entity.ShotsBeforeStateChange <- 5;
            entity.Cooldowner <- 0.0f;
            entity.AttackTarget <- GameObject.FindGameObjectWithTag("Tower").transform;
        | _ -> invalidArg "state" "Out of bounds in enum State. Correct values are Moving, Fleeing or Shooting"

    let mutable entities:Tuple<State,IStateMachineEntity> list = []

    member this.UpdateEntities ():Unit =
        let newEntities =
            entities
            |> List.map 
                (fun e -> 
                    match e with
                    | (State.Moving, entity) -> move entity
                    | (State.Shooting, entity) -> shoot entity
                    | (State.Fleeing, entity) -> flee entity
                    | _ -> invalidArg "state" "Out of bounds in enum State. Correct values are Moving, Fleeing or Shooting")
        entities
        |> List.zip newEntities
        |> List.filter (fun sp ->
            let ((st1, _), (st2, _)) = sp.Deconstruct()
            not (st1 = st2))
        |> List.iter (fun se ->
            let ((state, entity), _) = se.Deconstruct()
            this.TransferState entity state)
    
    member this.TransferState (entity:IStateMachineEntity) (state:State) =
        entities <- entities |> List.map (fun e ->
            let (st, ent) = e.Deconstruct()
            if ent = entity then
                (state, ent)
            else
                e)

        initEntityState entity state

    member this.JoinState (entity:IStateMachineEntity) (state:State) = 
        entities <- List.append [(state, entity)] entities
            
        initEntityState entity state

    member this.Start() =
        Debug.Log("Ding dong motherfucker")
        this.ReactTo(FRPEvent.Update, this.UpdateEntities)