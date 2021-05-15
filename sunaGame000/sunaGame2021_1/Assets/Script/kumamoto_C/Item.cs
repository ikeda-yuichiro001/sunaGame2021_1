using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public GameObject itemran;
    public GameObject itemname;
    public int[] ItemCount = new int[2];
    public GameObject[] item = new GameObject[2];
    public Text[] ItemText = new Text[2];

    void Start()
    {
        itemran.SetActive(false); //起動時にアイテムの欄全体を非表示に
        itemname.SetActive(false);
        if (ItemCount[0] == 0) item[0].SetActive(false);
        if (ItemCount[1] == 0) item[1].SetActive(false);
        if (ItemCount[2] == 0) item[2].SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) ItemCount[0] += 1;//アイテムの個数を1個づつ増やす実際のプレイには非搭載
        if (Input.GetKeyDown(KeyCode.A)) ItemCount[1] += 1;
        if (Input.GetKeyDown(KeyCode.Z)) ItemCount[2] += 1;

        if (Input.GetKeyDown(KeyCode.W)) ItemCount[0] -= 1;//アイテムの個数を1個づつ増やす実際のプレイには非搭載
        if (Input.GetKeyDown(KeyCode.S)) ItemCount[1] -= 1;
        if (Input.GetKeyDown(KeyCode.X)) ItemCount[2] -= 1;

        if (Input.GetKeyDown(KeyCode.E))
        {
            itemran.SetActive(true);
            itemname.SetActive(true);

            if (ItemCount[0] >= 1)
            {
                item[0].SetActive(true);
                ItemText[0].text = ItemCount[0].ToString() + " 個";
            }

            if (ItemCount[1] >= 1)
            {
                item[1].SetActive(true);
                ItemText[1].text = ItemCount[1].ToString() + " 個";
            }

            if (ItemCount[2] >= 2)
            {
                item[2].SetActive(true);
                ItemText[2].text = ItemCount[2].ToString() + " 個";
            }
        }

        if (Input.GetKeyDown(KeyCode.I))
        {

            item[0].SetActive(false);
            item[1].SetActive(false);
            item[2].SetActive(false);

            itemran.SetActive(false);
            itemname.SetActive(false);
        }
    }

    /*
        public GameObject itemran;
        public GameObject itemname;
        public GameObject A_rist;
        public GameObject B_rist;
        public GameObject C_rist;
        public Text A_count;
        public Text B_count;
        public Text C_count;
        public int A_s, B_s, C_s;

        void Start()
        {
            itemran.SetActive(false); //起動時にアイテムの欄全体を非表示に
            itemname.SetActive(false);
            if (A_s == 0) A_rist.SetActive(false); 
            if (B_s == 0) B_rist.SetActive(false);
            if (C_s == 0) C_rist.SetActive(false);
        }

        void Update()
        {

            //if (A_s == 0) A_rist.SetActive(false); //アイテムがなくなったときは常に表示しないようにする
            //if (B_s == 0) B_rist.SetActive(false);
            //if (C_s == 0) C_rist.SetActive(false);

            if (Input.GetKeyDown(KeyCode.Q)) A_s += 1;//アイテムの個数を1個づつ増やす実際のプレイには非搭載
            if (Input.GetKeyDown(KeyCode.A)) B_s += 1;
            if (Input.GetKeyDown(KeyCode.Z)) C_s += 1;

            if (Input.GetKeyDown(KeyCode.W)) A_s -= 1;//アイテムの個数を1個づつ増やす実際のプレイには非搭載
            if (Input.GetKeyDown(KeyCode.S)) B_s -= 1;
            if (Input.GetKeyDown(KeyCode.X)) C_s -= 1;

            if (Input.GetKeyDown(KeyCode.R))
            {
                itemran.SetActive(true);
                itemname.SetActive(true);

                if (A_s >= 1)
                {
                    A_rist.SetActive(true);
                    A_count.text = A_s.ToString() + " 個";
                }

                if (B_s >= 1)
                {
                    B_rist.SetActive(true);
                    B_count.text = B_s.ToString() + " 個";
                }

                if (C_s >= 1)
                {
                    C_rist.SetActive(true);
                    C_count.text = C_s.ToString() + " 個";
                }
            }

            if (Input.GetKeyDown(KeyCode.I))
            {

                A_rist.SetActive(false);
                B_rist.SetActive(false);
                C_rist.SetActive(false);

                itemran.SetActive(false);
                itemname.SetActive(false);
            }

            //アイテムを合成とかの機能が欲しい
            //アイテムの個数を表示したい ⇒　一度storing型にしてtextに表示してるのでそのあといじれない・・
            //画面サイズに合わせたアイテム欄の表示をさせたい　⇒　キャンバスのスケール調整で対応
        }
    */

}

