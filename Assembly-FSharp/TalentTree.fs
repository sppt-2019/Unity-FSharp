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

type Tree = 
| Node of TalentValue:Talent * Picked:bool * Children:Tree list
| Leaf of TalentValue:Talent * Picked:bool

type FRP_TalentTree() = 
    inherit FRPBehaviour()

    let tree = 
        Node (Talent(5, 5, 5), true,
            [
                Node(Talent(15,0,0), true,
                    [
                    Node(Talent(0,0,20), true,
                        [
                        Leaf(Talent(30,0,0), false);
                        Leaf(Talent(20,0,20), true)
                        ]
                    );
                    Node(Talent(0,20,0), true,
                        [
                        Leaf(Talent(40,0,0), false);
                        Leaf(Talent(15,15,0), true)
                        ]
                    )
                    ]
                );
                Node(Talent(0,0,15), false,
                    [
                    Node(Talent(0,0,30), false,
                        [
                        Leaf(Talent(0,0,50), false);
                        Leaf(Talent(15,15,15), false)
                        ]
                    );
                    Leaf(Talent(0,20,0), false)
                    ]
                );
                Node(Talent(0,15,0), false,
                    [
                    Node(Talent(0,20,0), false,
                        [
                        Leaf(Talent(0,45,0), false);
                        Leaf(Talent(0,25,25), false)
                        ]
                    );
                    Node(Talent(15,15,0), false,
                        [
                        Leaf(Talent(20,20,20), false);
                        Leaf(Talent(50,0,0), false)
                        ]
                    )
                    ]
                )
            ]
        )

    let rec walkTree (tree:Tree) lambda =
        match tree with
        | Node (_, _, c) ->
            lambda tree
            List.iter (fun child -> walkTree child lambda) c
        | _ ->
            lambda tree

    member this.Start() =
        let totalTalent = Talent(0,0,0)
        walkTree tree (fun n -> 
            match n with
            | Node (t, _, _) ->
                totalTalent.Strength <- totalTalent.Strength + t.Strength
                totalTalent.Agility <- totalTalent.Agility + t.Agility
                totalTalent.Intellect <- totalTalent.Intellect + t.Intellect
            | Leaf (t, _) ->
                totalTalent.Strength <- totalTalent.Strength + t.Strength
                totalTalent.Agility <- totalTalent.Agility + t.Agility
                totalTalent.Intellect <- totalTalent.Intellect + t.Intellect
        )

        Debug.Log("Maximum values of talents:")
        Debug.Log("Strength: " + totalTalent.Strength.ToString())
        Debug.Log("Agility: " + totalTalent.Agility.ToString())
        Debug.Log("Intellect: " + totalTalent.Intellect.ToString())

        let playerBonus = Talent(0,0,0)
        walkTree tree (fun n -> 
            match n with
            | Node (t, true, _) ->
                playerBonus.Strength <- playerBonus.Strength + t.Strength
                playerBonus.Agility <- playerBonus.Agility + t.Agility
                playerBonus.Intellect <- playerBonus.Intellect + t.Intellect
            | Leaf (t, true) ->
                playerBonus.Strength <- playerBonus.Strength + t.Strength
                playerBonus.Agility <- playerBonus.Agility + t.Agility
                playerBonus.Intellect <- playerBonus.Intellect + t.Intellect
            | _ -> ()
        )

        Debug.Log("Player bonus values of talents:")
        Debug.Log("Strength: " + playerBonus.Strength.ToString())
        Debug.Log("Agility: " + playerBonus.Agility.ToString())
        Debug.Log("Intellect: " + playerBonus.Intellect.ToString())



