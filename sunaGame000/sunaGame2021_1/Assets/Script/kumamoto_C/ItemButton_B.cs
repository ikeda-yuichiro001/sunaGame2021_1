using UnityEngine;

public class ItemButton_B : MonoBehaviour
{

    Item item_rigidbody;
    public GameObject B_3D;

    public void OnClick()
    {
        if (item_rigidbody.ItemCount[1] > 0)
        {
            B_3D.SetActive(true);
            B_3D.transform.rotation = new Quaternion(0, 0, 0, 0);
            item_rigidbody.item[0].active = false;
            item_rigidbody.item[2].active = false;
        }
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
