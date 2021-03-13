using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMove : MonoBehaviour
{
    status status;
    [SerializeField]
    public PlayerState State;
    public bool IsCtrl;
    [SerializeField]
    Transform UsingCamera;

    public bool IsGround;

    [Range(0.01f, 2f)]
    public float IsGroundThreshold;

    Vector3 jumppingPoint;

    public float moveSpeed = 0.16f;
    public float spinSpeed = 6.60f;
    public float runSpeed  = 0.40f;
    public bool IsJamp;
    public float jumppings, jumppingPower, failSpeed;
    public Animator animator;

    float csq;

    void Start()
    {
        status = GetComponent<status>();
    } 



    void FixedUpdate()
    {
        if (!IsCtrl)//ユーザ操作不能時
        {

            return;
        }

        //キー検知 ==========================================================
        var c = (Vector2)Key.JoyStickL;
        var r = (Vector2)Key.JoyStickR;
        var jamp = Key.A.Down;
        var sq   = Key.B.Down;

        switch (State)
        {
            case PlayerState.WAIT:
                if (jamp) JumppingMove();
                if ((Mathf.Abs(c.x) > 0.2 || Mathf.Abs(c.y) > 0.2))
                    State = PlayerState.WALK;
                if (sq)
                {
                    State = PlayerState.SQUAT;
                    animator?.SetBool("_SQUAT_", true);
                }
                break;

            case PlayerState.WALK:
                if (jamp) JumppingMove();
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                    State = PlayerState.RUN;
                //座標と回転移動
                Vector3 i = ((Mathf.Abs(c.x) > 0.2 || Mathf.Abs(c.y) > 0.2) && (UsingCamera != null)) ?
                Key.JoyStickL.Get.y * Vector3.Scale(UsingCamera.transform.forward, new Vector3(1, 0, 1)).normalized * moveSpeed + Key.JoyStickL.Get.x * UsingCamera.right * moveSpeed :
                Vector3.zero;
                transform.position += new Vector3(i.x, 0, i.z);
                if (new Vector3(i.x, 0, i.z).sqrMagnitude > 0.001f)
                    transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, new Vector3(i.x, 0, i.z), spinSpeed * Time.deltaTime, 0));
                animator?.SetBool("_WALK_", i.magnitude > 0.001f);
                if (i.magnitude < 0.001f)
                    State = PlayerState.WAIT;

                break;


            case PlayerState.RUN:
                if (jamp) JumppingMove();
                if (!(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
                    State = PlayerState.WALK;
                //座標と回転移動
                Vector3 ri = ((Mathf.Abs(c.x) > 0.2 || Mathf.Abs(c.y) > 0.2) && (UsingCamera != null)) ?
                Key.JoyStickL.Get.y * Vector3.Scale(UsingCamera.transform.forward, new Vector3(1, 0, 1)).normalized * runSpeed + Key.JoyStickL.Get.x * UsingCamera.right * moveSpeed :
                Vector3.zero;
                transform.position += new Vector3(ri.x, 0, ri.z);
                if (new Vector3(ri.x, 0, ri.z).sqrMagnitude > 0.001f)
                    transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, new Vector3(ri.x, 0, ri.z), spinSpeed * Time.deltaTime, 0));

                break;

            case PlayerState.JUMP:

                var Lj = Mathf.Sin(jumppings / 2);
                jumppings += Time.deltaTime *5;
                transform.position += Vector3.up * (Mathf.Sin(jumppings / 2) - Lj) * jumppingPower;
                Vector3 ccv  = ((Mathf.Abs(c.x) > 0.2 || Mathf.Abs(c.y) > 0.2) && (UsingCamera != null)) ?
                    Key.JoyStickL.Get.y * Vector3.Scale(UsingCamera.transform.forward, new Vector3(1, 0, 1)).normalized * moveSpeed + Key.JoyStickL.Get.x * UsingCamera.right * moveSpeed :
                    Vector3.zero;

                transform.position += new Vector3(0, 0, ccv.z);

                if (jumppings > 1)
                { 
                    animator?.SetBool("_JUMP_", false);
                    State = PlayerState.WAIT;
                }
                break;

            case PlayerState.FAIL:
                RaycastHit xx;
                Physics.Raycast(transform.position, Vector3.down, out xx, 100000000000000000) ;

                if (xx.distance >= failSpeed * Time.deltaTime)// + IsGroundThreshold)
                { 
                    transform.position -= Vector3.up * (failSpeed * Time.deltaTime);
                }
                else
                {
                    transform.position -= Vector3.up * (xx.distance - IsGroundThreshold + 0.01f);
                    State = PlayerState.WAIT;
                }
                break;

            case PlayerState.SQUAT:
                csq += Time.deltaTime;
                if (csq > 3)
                    if (sq)
                    {
                        csq = 0;
                        animator?.SetBool("_SQUAT_", false);
                        State = PlayerState.WAIT;
                    }
                break;
                
            default:
                break;
        }

        //地面検知 ==========================================================
        RaycastHit rr;
        var __IsGround = Physics.Raycast(transform.position, Vector3.down, out rr, IsGroundThreshold);
        if (State != PlayerState.JUMP)
        { 
            if (IsGround != __IsGround)
            {
                //接地時に落下地点と飛び降り地点の高度差でダメージ判定
                if (__IsGround)
                {
                    float p = jumppingPoint.y - transform.position.y;
                    if (p > 0)
                    {
                        float DamageV = 3;//高さ(m)とダメージ量の倍率
                        status.HP -= (int)((p > 2 ? p - 2 : 0) * DamageV);   //ダメージ判定                
                    }
                } 
                else
                {
                    jumppingPoint = transform.position; //飛び降り地点の保存
                }
            }

            IsGround = __IsGround;
        }

        

        if (!IsGround)
        {
            State = PlayerState.FAIL;
        }

    }
    void JumppingMove()
    {
        if (IsGround)
        {
            State = PlayerState.JUMP;
            animator?.SetBool("_JUMP_", true);
            jumppings = 0;
        }
    }
     

    public enum PlayerState
    {
        WALK,
        RUN,
        WAIT,
        FAIL,
        JUMP,
        SQUAT,
    }
}





//しゃがむ  

//3/20 ---------
//走るアニメーション
//座る
//段差登る
//はしご

//3/27-------------------
//階段
//斜面

//それいこう-----------------------------------
//アニメーションのブラッシュアップ
//操作性向上
//最終的な仕上げ
