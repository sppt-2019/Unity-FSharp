namespace UnityFunctional

open UnityEngine

type Talent(strength, intellect, agility) =
    let mutable str = strength
    let mutable int = intellect
    let mutable agi = agility

    member this.Strength
        with get() = str
        and set(v) = str <- v
    member this.Agility
        with get() = agi
        and set(v) = agi <- v
    member this.Intellect
        with get() = int
        and set(v) = int <- v

type FRP_TalentTree() = 
    inherit FRPBehaviour()



