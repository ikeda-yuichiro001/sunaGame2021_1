using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Itembutton : MonoBehaviour
{
    Item item_rigidbody;
    public GameObject A_3D, B_3D, C_3D;

    public void Start()
    {
        item_rigidbody = GetComponent<Item>();
        A_3D.SetActive(false);
        B_3D.SetActive(false);
        C_3D.SetActive(false);
    }

    public void OnButtonClick()
    {

        if (item_rigidbody.A_s > 0)
        {
            A_3D.SetActive(true);
        }

        if (item_rigidbody.B_s > 0)
        {
            B_3D.SetActive(true);
        }

        if (item_rigidbody.C_s > 0)
        {
            C_3D.SetActive(true);
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            A_3D.SetActive(false);
            B_3D.SetActive(false);
            C_3D.SetActive(false);
        }
    }
}
