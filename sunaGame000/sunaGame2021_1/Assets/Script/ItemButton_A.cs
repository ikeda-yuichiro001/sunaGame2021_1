using UnityEngine;

public class ItemButton_A : MonoBehaviour
{

    Item item_rigidbody;
    public GameObject A_3D;

    public void OnClick()
    {
        if (item_rigidbody.A_s > 0) A_3D.SetActive(true);
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
