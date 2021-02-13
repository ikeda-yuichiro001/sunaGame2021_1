using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform target;
    public float sensivirity;
    public float distance;
    Vector2 cv;

    void Start() => Update();

    void Update()
    {
        var vv = Time.deltaTime * sensivirity;
        transform.position = target.position - target.forward * distance;
        transform.LookAt(target); 
        cv += (Vector2)Key.JoyStickR;
        transform.RotateAround(target.position, Vector3.up,    cv.x);
        transform.RotateAround(target.position, Vector3.right, cv.y);
    }
}
