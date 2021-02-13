using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform target;
    public float sensivirity;
    Vector3 c;

    void Start() => Update();

    void Update()
    {
        var vv = Time.deltaTime * sensivirity;
        transform.position += target.transform.position - c;
        c = target.transform.position;

        if (Input.GetMouseButton(1))
        {
            Vector2 JS_R = Key.JoyStickR; 
            transform.RotateAround(c, Vector3.up, JS_R.x);
            transform.RotateAround(c, transform.right, JS_R.y);
        }
    }
}
