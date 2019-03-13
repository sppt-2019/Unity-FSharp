namespace UnityFunctional

open UnityEngine
open System

type FRP_ResourceController() =
    inherit MonoBehaviour()

    [<SerializeField>]
    let mutable DildSild:UnityEngine.UI.Text = null
    [<SerializeField>]
    let mutable MildAbild:UnityEngine.UI.Text = null
    [<SerializeField>]
    let mutable Vildild:UnityEngine.UI.Text = null

    [<SerializeField>]
    let mutable DildSildResult:UnityEngine.UI.Text = null
    [<SerializeField>]
    let mutable MildAbildResult:UnityEngine.UI.Text = null
    [<SerializeField>]
    let mutable VildildResult:UnityEngine.UI.Text = null

    member this.UpdateResources dildSild mildAbild vildild =
        DildSild.text <- dildSild.ToString()
        MildAbild.text <- mildAbild.ToString()
        Vildild.text <- vildild.ToString()

    member this.UpdateResults dildSild mildAbild vildild =
        DildSildResult.text <- dildSild.ToString()
        MildAbildResult.text <- mildAbild.ToString()
        VildildResult.text <- vildild.ToString()