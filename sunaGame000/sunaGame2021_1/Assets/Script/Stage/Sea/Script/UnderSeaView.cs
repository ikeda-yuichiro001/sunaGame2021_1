using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 //

public class UnderSeaView : MonoBehaviour
{ 
    Vector3 Scale = new Vector3(1.5f, 1f, 1);
    Texture2D raw;
    RawImage rawImg;
    Camera cam_; 
    const uint GetSample = 4;

    void Update()
    {
        if (cam_ == null)
            SetCamera(Wave.wave.targetCamera.transform.GetComponent<Camera>());
        DrawCamera();
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (!UnityEditor.EditorApplication.isPlaying) 
            Update();
    }
#endif

    void SetCamera(Camera cam)
    {
        rawImg = transform.Find("img").GetComponent<RawImage>();
        raw = rawImg.texture as Texture2D;
        transform.GetComponent<Canvas>().worldCamera = cam;
        cam_ = cam;
    }

    void DrawCamera()
    {
        Texture2D r = raw;

        //プレーン部分の波の高さをサンプリングする-----
        Vector3[] data = new Vector3[GetSample];
        Vector3 leftSide  = rawImg.transform.position + rawImg.transform.right * Scale.x / 2;
        Vector3 rightSide = rawImg.transform.position - rawImg.transform.right * Scale.x / 2;
        
        for (int f = 0; f < GetSample; f++)
        {
            data[f] = Vector3.Lerp(leftSide, rightSide, (1f * f)/ (GetSample-1));
            data[f].y = Wave.GetSurfaceHeight(data[f]); 
        }

        float[] point = new float[raw.width];//124
        for (var d = 0; d < raw.width; d++)
            point[d] = data[d/((int)(raw.width / GetSample))].y; 
        Texture2D tc = new Texture2D(r.width, r.height, TextureFormat.ARGB32, false);
        tc.SetPixels(r.GetPixels());
        r = tc;

        //画像の作成------------------------------------- 
        for (var d = 0; d < raw.width; d++)
        { 
            float xx = (point[d] - rawImg.transform.position.y) * Scale.y;
            xx += 1;
            xx *= Wave.wave.UnderSeaView_AdjustedValue;//補正値
            xx /= 2;
            xx = 1 - xx;
            if (xx < 0) xx = 0;  else if (xx > 1) xx = 1;
            int H = (int)(raw.height * xx);
            Color[] cc = new Color[H]; 
            if (H > 0) r.SetPixels(d, raw.height - H, 1, H, cc);  
        }

        //再割り当て--------------------------
        r.Apply(); 
        rawImg.texture = r;
    } 
}
