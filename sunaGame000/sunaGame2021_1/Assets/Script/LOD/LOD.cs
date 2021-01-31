using UnityEngine;

public class LOD : MonoBehaviour
{  
    void Start()
    {
        LOD_Manager.ADDLOD(transform);
        Destroy(this);
    } 
}
