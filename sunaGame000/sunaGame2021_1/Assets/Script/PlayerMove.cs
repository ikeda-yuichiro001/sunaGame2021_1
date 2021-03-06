using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMove : MonoBehaviour
{ 
    [SerializeField]
    Transform UsingCamera;

    public bool IsGround;

    [Range(0.1f, 2f)]
    public float IsGroundThreshold;

    Vector3 jumppingPoint;
    
    public float moveSpeed = 3f;
     

    void Start()
    { 

    } 



    void FixedUpdate()
    { 
        //キー検知 ==========================================================
        var c = (Vector2)Key.JoyStickL;
        var r = (Vector2)Key.JoyStickR;
        //地面検知 ==========================================================
        RaycastHit rr;
        var __IsGround = Physics.Raycast(transform.position, Vector3.down, out rr, IsGroundThreshold);

        if (IsGround != __IsGround)
        {
            //接地時に落下地点と飛び降り地点の高度差でダメージ判定
            if (__IsGround)
            {
                float p = jumppingPoint.y - transform.position.y;
                if (p > 0)
                {
                    //ダメージ判定
                }
            }
            //飛び降り地点の保存
            else
            {
                jumppingPoint = transform.position;
            }
        }

        IsGround = __IsGround;
        //移動 ==============================================================
        transform.rotation = Quaternion.Lerp(UsingCamera.rotation, UsingCamera.rotation * Quaternion.Euler(0, 90, 0), c.x);

        if (Mathf.Abs(c.x) > 0.2 || Mathf.Abs(c.y) > 0.2)
        { 
            transform.LookAt(transform.position + new Vector3(-UsingCamera.right.x, 0, UsingCamera.forward.z));
            transform.Translate(Key.JoyStickL.Get.x, 0, Key.JoyStickL.Get.y, Space.Self); 
        }


    }



}

//移動
//ジャンプ
//しゃがむ
//落下ダメージ
//段差
//段差登る
//座る
//階段
//はしご
//泳ぐ