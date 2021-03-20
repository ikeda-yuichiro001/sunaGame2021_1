using UnityEngine;

public class ItemButton_C : MonoBehaviour
{

    Item item_rigidbody;
    public GameObject C_3D;

    public void OnClick()
    {
        if (item_rigidbody.C_s > 0) C_3D.SetActive(true);
    }

    void Start()
    {
        item_rigidbody = GetComponent<Item>();
        C_3D.SetActive(false);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) C_3D.SetActive(false);
    }

}
