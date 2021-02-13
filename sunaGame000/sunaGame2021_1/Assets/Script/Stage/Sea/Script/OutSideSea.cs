using UnityEngine;

public class OutSideSea : MonoBehaviour
{
    public Transform VectorCtrlPointer;
    public Transform Sea0, Sea1, Sea2, Sea3, Sea4, Sea5, Sea6, Sea7;

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (UnityEditor.EditorApplication.isPlaying)
            return;
        Start();
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(VectorCtrlPointer.position, 5);
        Gizmos.DrawLine(VectorCtrlPointer.up * 10, -VectorCtrlPointer.up * 10);
        Gizmos.DrawLine(VectorCtrlPointer.right * 10, -VectorCtrlPointer.right * 10);
        Gizmos.DrawLine(VectorCtrlPointer.forward * 10, -VectorCtrlPointer.forward * 10); 

        Gizmos.DrawLine(VectorCtrlPointer.position, Sea0.position);
        Gizmos.DrawLine(VectorCtrlPointer.position, Sea1.position);
        Gizmos.DrawLine(VectorCtrlPointer.position, Sea2.position);
        Gizmos.DrawLine(VectorCtrlPointer.position, Sea3.position);
        Gizmos.DrawLine(VectorCtrlPointer.position, Sea4.position);
        Gizmos.DrawLine(VectorCtrlPointer.position, Sea5.position);
        Gizmos.DrawLine(VectorCtrlPointer.position, Sea6.position);
        Gizmos.DrawLine(VectorCtrlPointer.position, Sea7.position); 
    }
#endif

    void Start()
    {
        Vector3 p = Wave.wave.transform.position + Vector3.up * 0.1f;
        Vector3 c = Vector3.one * Wave.wave.GetScale;
        Sea0.position = new Vector3(-c.x, p.y, -c.z);
        Sea1.position = new Vector3(-c.x, p.y,  p.z);
        Sea2.position = new Vector3(-c.x, p.y,  c.z);
        Sea3.position = new Vector3( c.x, p.y, -c.z);
        Sea4.position = new Vector3( c.x, p.y,  p.z);
        Sea5.position = new Vector3( c.x, p.y,  c.z);
        Sea6.position = new Vector3( p.x, p.y, -c.z);
        Sea7.position = new Vector3( p.x, p.y,  c.z); 
    } 

}
