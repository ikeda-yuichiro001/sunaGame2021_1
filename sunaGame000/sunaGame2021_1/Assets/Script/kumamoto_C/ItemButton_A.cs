using UnityEngine;

public class ItemButton_A : MonoBehaviour
{

    Item item_rigidbody;
    public GameObject A_3D;

    public void OnClick()
    {
        if (item_rigidbody.ItemCount[0] > 0)
        {
            A_3D.SetActive(true);
            A_3D.transform.rotation = new Quaternion(0,0,0,0);
            item_rigidbody.item[1].active = false;
            item_rigidbody.item[2].active = false;
        }
    }

    public void Start()
    {
        item_rigidbody = GetComponent<Item>();
        A_3D.SetActive(false);
    }


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) A_3D.SetActive(false);
    }

}
