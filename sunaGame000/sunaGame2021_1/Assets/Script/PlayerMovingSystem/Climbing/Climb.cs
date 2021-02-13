using System.Collections.Generic;
using UnityEngine;

public class Climb : MonoBehaviour
{ 
    public float searchGap;
    public float searchheight;
    public float searchheight_min;
    public float searchArea;
    [Range(0, 1f), SerializeField]
    float DirectionMixingRatio;
    [Range(0,1f), SerializeField]
    float truncationThreshold;

    [Range(0.1f, 1f), SerializeField]
    float DetectionBoxSideLength = 1;

    [Range(0.1f, 1f), SerializeField]
    float FrontDistanceCorrection;

    [SerializeField]
    Confirmations confirmation = new Confirmations();

    public float MoveThresholdToUpdate;

    [Space(20 )]
    public List<ClimbTargetObject> targetObj;

    Vector3 PointL;
    void Update()
    {
        LIMIT();
        if (Vector3.Distance(PointL, transform.position) < MoveThresholdToUpdate) return; 
        PointL = transform.position;
        targetObj = new List<ClimbTargetObject>();

        Vector3[] s = GetPoint;
        var up_ = Vector3.up * (searchheight + searchGap + DetectionBoxSideLength);
        for (var i = 0; i < s.Length; i++)
        { 
            RaycastHit hit;
            if (Physics.BoxCast(s[i] + up_, Vector3.one * DetectionBoxSideLength, Vector3.down, out hit, Quaternion.identity, searchGap + searchheight))
            {
                if (hit.point.y - transform.position.y > searchheight_min && hit.point.y - transform.position.y < searchheight)
                {
                    if (hit.transform.root != transform.root)
                    {
                        if (hit.transform.GetComponent<NotClimbingObjects>() == null)
                        {
                            var priority_ = ((searchArea - Vector3.Distance(transform.position, new Vector3(hit.point.x, transform.position.y, hit.point.z))) / searchArea) * (1 - DirectionMixingRatio) + //距離近
                                            (Vector3.Dot(transform.forward, (new Vector3(hit.point.x, transform.position.y, hit.point.z) - transform.position).normalized)) * DirectionMixingRatio; //正面か(0 <= 真横より後ろ、　真正面 <= 1)
                            if (priority_ * priority_ > truncationThreshold)
                            {
                                targetObj.Add(new ClimbTargetObject() { point = hit.point, transform = hit.transform, priority = priority_ * priority_ });
                            }
                        }
                    }
                }
            }
        }
         
        if (targetObj.Count > 1 )
        {
            List<ClimbTargetObject> deleteTarget = new List<ClimbTargetObject>();
            for (var c = 0; c < targetObj.Count; c++)
                for (var b = 0; b < targetObj.Count; b++)
                    if (c != b) 
                        if (targetObj[c].transform == targetObj[b].transform)
                            if (targetObj[c].priority > targetObj[b].priority)
                                deleteTarget.Add(targetObj[b]);
                
            foreach (var v in deleteTarget)
                targetObj.Remove(v);
        } 

    }

    void LIMIT()
    {
        if (searchheight < searchheight_min)
            searchheight = searchheight_min + 0.001f;
        if (searchArea <= 0.1f)
            searchArea = 0.1f;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        LIMIT();
        if(!UnityEditor.EditorApplication.isPlaying) Update();
        Gizmos.color = Color.black;
        Vector3[] p = GetPoint;
       
        for (int r = 0; r < confirmation.lap; r++)
        {
            for (int x = 0; x < confirmation.perLap; x++)
            { 
                Gizmos.DrawLine(p[r * confirmation.perLap + x], p[r * confirmation.perLap + x] + Vector3.up * 0.2f);
                Gizmos.DrawWireSphere(p[r * confirmation.perLap + x] + Vector3.up * 0.2f, 0.012f);
                if (r > 0)
                    Gizmos.DrawLine(p[r * (confirmation.perLap) + (x + 0) % confirmation.perLap], p[(r * confirmation.perLap) + (x + 1) % confirmation.perLap]);
                
                if (r < confirmation.lap-1) 
                     Gizmos.DrawLine(p[r * confirmation.perLap + x], p[(r + 1) * confirmation.perLap + x]);                
            }
        } 
      
        //Gizmos.DrawWireSphere(transform.position, new Vector3()); //エリア表示

        Gizmos.DrawWireCube(transform.position + Vector3.up * (searchheight + searchGap) , new Vector3(0.6f, 0.01f, 0.6f)); //エリア表示
        Gizmos.DrawWireCube(transform.position + Vector3.up * searchheight_min, new Vector3(0.3f, 0.05f, 0.3f)); //エリア表示
        Gizmos.DrawWireCube(transform.position + Vector3.up * searchheight, new Vector3(0.3f, 0.05f, 0.3f)); //エリア表示 
        Gizmos.DrawLine(transform.position + Vector3.up * searchheight_min, transform.position + Vector3.up * searchheight);

        if (targetObj != null && targetObj.Count > 0)
        { 
            Gizmos.color = Color.cyan;
            foreach (var c in targetObj)
            {
                Gizmos.DrawLine(c.point, c.point + Vector3.up * c.priority);
                Gizmos.DrawWireCube(c.point + Vector3.up , new Vector3(DetectionBoxSideLength, 0, DetectionBoxSideLength)/3);
                Gizmos.DrawWireCube(c.point, Vector3.one* DetectionBoxSideLength);
            }
        }
    }
#endif

    /// <summary>
    /// RayCast生成座標(y軸加算前)
    /// </summary>
    Vector3[] GetPoint
    {
        get
        {
            List<Vector3> p = new List<Vector3>();
            for (int r = 0; r < confirmation.lap; r++)
                for (int x = 0; x < confirmation.perLap; x++)
                    p.Add(transform.position + (new Vector3(Mathf.Sin(2f / confirmation.perLap * x * Mathf.PI), 0, Mathf.Cos(2f / confirmation.perLap * x * Mathf.PI)) + Vector3.Scale(transform.forward, new Vector3(1, 0, 1)) * FrontDistanceCorrection) * searchArea / confirmation.lap * r);
            return p.ToArray();
        }
    }
}

[System.Serializable]
public class ClimbTargetObject
{ 
    public Transform transform;
    public Vector3 point;
    public float priority; 
}


[System.Serializable]
public class Confirmations
{
    [SerializeField, Range(3,100)]
    public int lap ,perLap;
    public Confirmations() { lap = perLap = 3; }
}