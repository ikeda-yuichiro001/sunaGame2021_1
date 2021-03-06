using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTest : MonoBehaviour
{
    public bool FLAG;
    CameraMove test;

    void Start()
    {
        test = GetComponent<CameraMove>();
    }

    void Update()
    {
        if (FLAG)
        {
            FLAG = false;
            test.RESET_();
        }
    }
}
