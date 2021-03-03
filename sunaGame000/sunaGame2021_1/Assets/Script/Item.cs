using UnityEngine;

public class Item : MonoBehaviour
{
    public GameObject itemran;
    public GameObject A_rist;
    public GameObject B_rist;
    public GameObject C_rist;
    int A_s , B_s, C_s ;

    void Start()
    {
        itemran.SetActive(false); //起動時にアイテムの欄自体を非表示に
        if (A_s == 0) A_rist.SetActive(false);
        if (B_s == 0) B_rist.SetActive(false);
        if (C_s == 0) C_rist.SetActive(false);

    }

    void Update()
    {
        if (Input.GetKeyDown("i"))
        {
            itemran.SetActive(true);

            A_s = 1;//アイテムの個数を定義（今回は仮に1個を想定）
            B_s = 1;
            C_s = 1;

            if (A_s >= 1 )A_rist.SetActive(true);//アイテムが1個以上の時に表示する以下も同じ
            if (B_s >= 1) B_rist.SetActive(true);
            if (C_s >= 1) C_rist.SetActive(true);
        }

        if (Input.GetKeyDown("r"))

        {
            itemran.SetActive(false);

            A_s = 0;
            B_s = 0;
            C_s = 0;

            if (A_s == 0) A_rist.SetActive(false);
            if (B_s == 0) B_rist.SetActive(false);
            if (C_s == 0) C_rist.SetActive(false);
        }
    }

    //アイテムを合成とかの機能が欲しい
    //アイテムの個数を表示したい
    //画面サイズに合わせたアイテム欄の表示をさせたい
}
