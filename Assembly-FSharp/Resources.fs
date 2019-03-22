namespace UnityFunctional

open UnityEngine
open System

type FRP_Resources() =
    inherit FRPBehaviour()

    [<SerializeField>]
    let mutable DildSild:int = 32
    [<SerializeField>]
    let mutable MildAbild:int = 45
    [<SerializeField>]
    let mutable Vildild:int = 5
    [<DefaultValue>]
    val mutable ResourceController:FRP_ResourceController

    let convRatio = [17*8;17;1]

    let sumCurrency (currencies:int list) =
        currencies
        |> List.zip convRatio
        |> List.map (fun (elm) ->
            let (q, cr) = elm.Deconstruct()
            q * cr)
        |> List.sum

    let combine totalDildSils =
        let rVildIld = totalDildSils / convRatio.[0]
        let rMildAbild = (totalDildSils - (rVildIld * convRatio.[0])) / convRatio.[1]
        let rDildSild = totalDildSils - (rVildIld * convRatio.[0]) - (rMildAbild * convRatio.[1])
        (rDildSild, rMildAbild, rVildIld)

    let canBuy dildSild mildAbild vildIld =
        (sumCurrency [Vildild;MildAbild;DildSild]) >= (sumCurrency [vildIld;mildAbild;dildSild])

    member this.buy dildSild mildAbild vildIld =
        if canBuy dildSild mildAbild vildIld then
            let price = sumCurrency [vildIld;mildAbild;dildSild]
            let wallet = sumCurrency [Vildild;MildAbild;DildSild]
            let rem = wallet - price
            let (ds,ma,vi) = combine rem
            Debug.Log("Buying (" + dildSild.ToString() + ", " + mildAbild.ToString() + ", " + vildIld.ToString() + ")")
            DildSild <- ds
            MildAbild <- ma
            Vildild <- vi
            this.ResourceController.UpdateResults ds ma vi
        else
            Debug.Log ("Can't buy (" + dildSild.ToString() + ", " + mildAbild.ToString() + ", " + vildIld.ToString() + ")")

    member this.Start() =
        this.ResourceController.UpdateResources DildSild MildAbild Vildild
        Debug.Log("Total DildSild: " + (sumCurrency [Vildild;MildAbild;DildSild]).ToString())
        
        let (ds, ma, vi) = combine (sumCurrency [Vildild;MildAbild;DildSild])
        this.ResourceController.UpdateResults ds ma vi

        this.buy 10 10 10
        this.buy 1 2 3
        this.buy 15 0 0
