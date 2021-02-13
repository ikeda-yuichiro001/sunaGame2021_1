using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    Camera UsingCamera;
    float moveSpeed = 3f;

    public bool IsGround;
    [Range(0.1f, 2f)]
    public float IsGroundThreshold;
    Vector3 jumppingPoint;

    void Start()
    {

    }


    void FixedUpdate()
    { 
        //キー検知 ==========================================================
        var c = (Vector2)Key.JoyStickL;
        
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
        var f = Vector3.Scale(UsingCamera.transform.forward, new Vector3(1, 0, 1)).normalized * c.x + Camera.main.transform.right * c.y;
        transform.Translate ((f * moveSpeed + new Vector3(0,IsGround? 0 : -9, 0)) / Time.deltaTime);
        if (f != Vector3.zero) transform.rotation = Quaternion.LookRotation(f);

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