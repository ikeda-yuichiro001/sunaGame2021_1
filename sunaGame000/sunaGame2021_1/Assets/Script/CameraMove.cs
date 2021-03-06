using UnityEngine;

public enum CameraCtrlMode
{
    User,
    Resetting,
    ProgramCtrl
}

public class CameraMove : MonoBehaviour
{
    public CameraCtrlMode Mode
    {
        get => mode;
        set
        {
            mode = value;
            switch (mode)
            {
                case CameraCtrlMode.User:
                    break;

                case CameraCtrlMode.Resetting:
                    resetT = 0;
                    RTY = transform.rotation.ToEuler();
                    break;

                case CameraCtrlMode.ProgramCtrl:

                    break;
            }
        }
           
            
    }

    CameraCtrlMode mode;

    public Transform target;
    public float sensivirity;      // R
    public float distance;         // R
    public bool IsReset;           
    public float upArea, downArea; // R
    public float ResetSpeed;    
    public float CenterCorrection; // R

    float resetT;
    Transform cameras;
    float yd;
    Vector2 pp;
    Vector3 RTY;

    void Start()
    {
        cameras = transform.Find("Main Camera");
    }


    public void RESET_()
    {
        IsReset = true;
        resetT = 0;
        RTY = transform.rotation.ToEuler();
    }



    void Update()
    {
        transform.position = target.position;

        if (IsReset)
        {
            resetT += ResetSpeed * Time.deltaTime;
            if (resetT > 1) IsReset = false;
            transform.rotation = Quaternion.Lerp(Quaternion.Euler(RTY), target.rotation, resetT);
        }
        else
        { 
            var vv = Time.deltaTime * sensivirity * Key.JoyStickR.Get * sensivirity;
            transform.Rotate(Vector3.up * vv.x * 3.14f * 10, Space.World);
            yd += vv.y;
            yd = yd < downArea ? downArea : (yd > upArea ? upArea : yd);
            cameras.position = transform.position - transform.forward * distance + Vector3.up * (yd + CenterCorrection); 
        }

        cameras.LookAt(target.position + Vector3.up * CenterCorrection);
    }

}
