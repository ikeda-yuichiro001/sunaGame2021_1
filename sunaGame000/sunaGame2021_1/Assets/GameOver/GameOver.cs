using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : UnityEngine.MonoBehaviour
{ 
    void Start() => Invoke("NEXT", 3.5f);
    void NEXT() => SceneLoader.Load("Logo");
}
