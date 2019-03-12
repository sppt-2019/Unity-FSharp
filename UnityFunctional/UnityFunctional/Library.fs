namespace UnityFunctional
open UnityEngine
open System


type TestComponent() = 
    inherit UnityEngine.MonoBehaviour()
    [<SerializeField>]
    let mutable speed:float32 = 2.0f
    [<SerializeField>]
    let mutable height = 25.0f

    member this.Start() =
        this.transform.localScale <- this.transform.localScale + new Vector3(height, 0.0f);

    member this.Update() =
        this.transform.Rotate(Vector3.down, speed * Time.deltaTime);