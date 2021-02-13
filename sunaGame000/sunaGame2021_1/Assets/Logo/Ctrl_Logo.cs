using UnityEngine;

public class Ctrl_Logo : MonoBehaviour
{
    void Start() => Invoke("NEXT", 2.4f);
    void NEXT() => SceneLoader.Load("Title");
}
