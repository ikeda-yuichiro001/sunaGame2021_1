using UnityEngine;

public class ItemButton_B : MonoBehaviour
{

    Item item_rigidbody;
    public GameObject B_3D;

    public void OnClick()
    {
        if (item_rigidbody.B_s > 0) B_3D.SetActive(true);
    }

    void Start()
    {
        item_rigidbody = GetComponent<Item>();
        B_3D.SetActive(false);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) B_3D.SetActive(false);
    }

}
