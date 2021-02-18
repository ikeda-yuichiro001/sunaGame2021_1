using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

    //　アイテムの種類を設定
    public enum item
    {
        A,
        B,
        C,
    }

    Dictionary<Item, int> itemDictionary = new Dictionary<Item, int>();

    public static Item A { get; private set; }
    public static Item B { get; private set; }

    // Use this for initialization
    void Start()
    {
        itemDictionary[Item.A] = 2;
        itemDictionary[Item.B] = 3;

        foreach (var item in itemDictionary)
        {
            Debug.Log(item.Key + " : " + GetNum(item.Key));
        }
    }

    //　アイテムをどれだけ持っているかの数を返す
    public int GetNum(Item key)
    {
        return itemDictionary[key];
    }
}
