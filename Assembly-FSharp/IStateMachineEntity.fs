module UnityFunctional.Interfaces
open UnityEngine

type IStateMachineEntity = 
    abstract member name:string
    abstract member Speed:float
    abstract member RotationSpeed:float
    abstract member ShotsBeforeStateChange:int with get, set
    abstract member AttackTarget:UnityEngine.Transform with get, set
    abstract member MoveTarget:UnityEngine.Vector3 with get, set
    abstract member ShotCooldown:float
    abstract member Cooldowner:float with get, set