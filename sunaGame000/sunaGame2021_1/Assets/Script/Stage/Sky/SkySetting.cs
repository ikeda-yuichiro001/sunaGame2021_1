using UnityEditor;
using UnityEngine;

public class SkySetting : MonoBehaviour
{
    public Light SunLight;
    public Gradient SunColor; 
    void Update() => Draw();
    public float Tilt = -30;
    [Range(0,1)]
    public float alpha = 0;

    public float _SunVector;
    public Material skyMaterial;

#if UNITY_EDITOR

    const int WIDTH = 16;

    [UnityEditor.InitializeOnLoadMethod]
    static void Example()
    {
        UnityEditor.SceneView.duringSceneGui += OnGui;
    }

    private static void OnGui(UnityEditor.SceneView s)
    {
        UnityEditor.Handles.BeginGUI();
        bool c = Lightmapping.giWorkflowMode == Lightmapping.GIWorkflowMode.Iterative;

        if (!c)
        { 
            if (GUI.Button(new Rect(s.position.size.x - 120, s.position.size.y - 52, 120, 16), "手動ベイク"))
            {
             　　 Lightmapping.BakeAsync();
            }
        }

        if (GUI.Button(new Rect(s.position.size.x -120, s.position.size.y - 36,120,16), "AutoBakeを" + !c + "に"))
        {
            Lightmapping.giWorkflowMode = c ? Lightmapping.GIWorkflowMode.OnDemand : Lightmapping.GIWorkflowMode.Iterative;

        }
        // ここに UIを描画する処理を記述

        UnityEditor.Handles.EndGUI();
    } 


    void OnDrawGizmos()
    {
        Draw(); 
        float VR = 100;
        float VLL = 400;
        var ccc = SunLight.transform.position;
        var ccc1 = ccc + SunLight.transform.forward * VLL;

        Gizmos.color = SunLight.color;
        Gizmos.DrawWireSphere(ccc, VR);
        Gizmos.DrawLine(ccc, ccc1);
        for (int qx = -1; qx <= 1; qx++)
            for (int qy = -1; qy <= 1; qy++)
                for (int qz = -1; qz <= 1; qz++)
                    Gizmos.DrawLine(ccc, ccc + new Vector3(qx, qy, qz).normalized * VR * 1.4f);
        var c11 = 120f;
        Vector3 qa = -SunLight.transform.forward.normalized;

        var cc5 = ccc1 + qa * 10;
        var dd = 22f;
        Vector3 sa1 = ccc1 + (ccc1 - ccc) * 0.1f;
        Vector3 sa3 = ccc1 + (ccc1 - ccc) * 0.4f;
        for (int w = 0; w < 4; w++)
        {
            Vector3 c = new Vector3(Random.Range(-1f, 1f),  0, Random.Range(-1f, 1f));
            Gizmos.DrawLine(sa1 + -1 * c * dd, sa3 + -1 * c * dd);
        }
        Gizmos.DrawLine(sa1 + -1 * SunLight.transform.right * dd, sa3 + -1 * SunLight.transform.right * dd);
        Gizmos.DrawLine(sa1 + 00 * SunLight.transform.right * dd, sa3 + 00 * SunLight.transform.right * dd);
        Gizmos.DrawLine(sa1 + 01 * SunLight.transform.right * dd, sa3 + 01 * SunLight.transform.right * dd);
        Gizmos.DrawLine(sa1 + -1 * SunLight.transform.up * dd, sa3 + -1 * SunLight.transform.up * dd);
        Gizmos.DrawLine(sa1 + 01 * SunLight.transform.up * dd, sa3 + 01 * SunLight.transform.up * dd);
    }

#endif


    void Draw()
    {
        var v = TimeData.TimePoint;
        var t = SunColor.Evaluate(v);
        if (SunLight == null)
            SunLight = GameObject.Find("Sun Light").GetComponent<Light>();
        SunLight.transform.rotation = Quaternion.Euler(360 * v - 90, Tilt, 0);
        SunLight.color = t;
        SunLight.intensity = (1- SunLight.color.grayscale) * alpha;
        RenderSettings.skybox = skyMaterial;
        var c = RenderSettings.skybox;
        c.SetColor("_Tint",new Color(t.r,t.g,t.b,alpha));
        RenderSettings.skybox = c;
        _SunVector = v;
    }
}

//http://corevale.com/unity/6307