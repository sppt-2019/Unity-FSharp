namespace UnityFunctional

open UnityEngine

type Talent(strength, intellect, agility) =
    let Strength = strength
    let Intellect = intellect
    let Agility = agility

type Tree = 
    | Node of TalentValue:Talent * Children:Tree list * Picked:bool
    | Leaf of TalentValue:Talent

type FRP_TalentTree() = 
    inherit FRPBehaviour()



