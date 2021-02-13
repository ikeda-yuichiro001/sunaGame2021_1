using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class status : MonoBehaviour
{

    public int HP,LHP,Spst;
    public string Pstatus;

    public void HPmove(int a)
    {
        HP += a;
        if (LHP != HP)
        {

            //HPが減った時は「ダメージ」増えた時は「回復」
            if (HP >= 100) HP = 100;                            //HPの最大値を100
            if (HP <= 0) HP = 0;                                //HPの最小値を0

            if (HP > 80) Pstatus = "健康";                       //HPが80～100の時は健康状態
            else if (HP > 60) Pstatus = "少し不安";              //HPが50～80の時は少し不安状態
            else if (HP > 40) Pstatus = "不安";                  //HPが20～50の時は不安状態
            else if (HP > 20) Pstatus = "危険";                 //HPが1～20の時は危険状態
            else if (HP <= 0) Pstatus = "ゲームオーバー";       //HPが0になるとゲームオーバー
                                                                //時間経過による回復
                                                                //アイテムによる回復
        }
        LHP = HP;
    }

    public void Keydamegemove()
    {
        //Xbox用のキー操作Key.A;
        if (Input.GetKeyDown(KeyCode.D))
        {
            HPmove(-10);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            HPmove(10);
        }
    }

    void Start()
    {
        //HPの最大値を100に
        //HPの最小値を0に
        //ダメージ関数
        //ほかのステータスを追加できるようにする
        Pstatus = "健康";
        HP = 100;        
    }

    void Update()
    {
        Keydamegemove();        

    }
}
