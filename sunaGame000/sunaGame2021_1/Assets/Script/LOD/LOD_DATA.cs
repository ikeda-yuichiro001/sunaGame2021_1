using UnityEngine;

[System.Serializable]
public class LOD_DATA
{
    public Transform ROOT, HIGH, MID, LOW;

    public LOD_DATA(Transform t)
    {
        ROOT = t;
        HIGH = t.Find("High");
        MID = t.Find("Mid");
        LOW = t.Find("Low");
    }
}