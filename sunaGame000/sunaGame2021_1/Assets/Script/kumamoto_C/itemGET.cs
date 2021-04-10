using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//動かすキャラクターにrigidbodyをつけてこいつをつける
//アイテムにはtagにItemを設定する
//tagにItemがついているオブジェクトに衝突するとコンポーネントを取得する

public class itemGET : MonoBehaviour
{
    public ItemStatus itemstatus;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Item")
        {
            itemstatus = transform.root.gameObject.GetComponent<ItemStatus>();
        }
    }

    void Update()
    {
        
    }
}
