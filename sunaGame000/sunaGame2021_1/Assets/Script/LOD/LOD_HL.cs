using UnityEngine;

public class LOD_HL : MonoBehaviour
{
    void Start()
    {
        LOD_Manager.ADDLOD_HL(transform);
        Destroy(this);
    }
}