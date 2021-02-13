using UnityEngine;

public class ship : MonoBehaviour
{
    public float speed = 1;
    public float torque = 1;
    public float heightAdjusted = 1;
    private float AngleY;

    void Update()
    {
        //操作部分
        if (Input.GetKey(KeyCode.UpArrow   )) transform.Translate(speed * Time.deltaTime * Vector3.forward, Space.Self);
        if (Input.GetKey(KeyCode.DownArrow )) transform.Translate(speed * Time.deltaTime * Vector3.back, Space.Self);
        if (Input.GetKey(KeyCode.LeftArrow )) AngleY -= torque * Time.deltaTime;
        if (Input.GetKey(KeyCode.RightArrow)) AngleY += torque * Time.deltaTime;

        //位置制御(xz軸はオブジェクトの座標データを用いるが、ｙ軸は波の高さを使用する)
        transform.position = new Vector3(transform.position.x, Wave.GetSurfaceHeight(transform.position) + heightAdjusted, transform.position.z);

        //Quaternion型では加算が*になるためこの様に記述している。Eulerでｙ回転軸のみ取り波の角度でｘｚ軸を制御している。
         transform.rotation = Wave.GetSurfaceNormal(transform.position) * Quaternion.Euler(0, AngleY, 0); 
    }
}

// transform.rotation = Quaternion.Euler(0, AngleY, 0);
