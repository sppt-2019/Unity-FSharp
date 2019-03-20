module UnityFunctional.Interfaces

open UnityEngine

type IStateMachineEntity = 
    abstract member name:string
    abstract member Speed:float32
    abstract member RotationSpeed:float32
    abstract member transform:Transform with get
    abstract member ShotsBeforeStateChange:int with get, set
    abstract member AttackTarget:UnityEngine.Transform with get, set
    abstract member MoveTarget:UnityEngine.Vector3 with get, set
    abstract member ShotCooldown:float32
    abstract member Cooldowner:float32 with get, set