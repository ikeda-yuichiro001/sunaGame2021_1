using UnityEngine;

public class ItemButton_C : MonoBehaviour
{

    Item item_rigidbody;
    public GameObject C_3D;

    public void OnClick()
    {
        if (item_rigidbody.ItemCount[2] > 0)
        {
            C_3D.SetActive(true);
            C_3D.transform.rotation = new Quaternion(0, 0, 0, 0);
            item_rigidbody.item[0].active = false;
            item_rigidbody.item[1].active = false;
        }
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
