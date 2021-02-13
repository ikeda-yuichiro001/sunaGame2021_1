/*-----------------------------------------------
   海の処理におけるメインのクラス
    
   
 ------------------------------------------------*/


using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif



public class Wave : MonoBehaviour
{

    [Header("◆ポリゴン数の１辺の数")]
    [Space(30)]
    [SerializeField, Range(100, 256)]public int VerticesToVector = 100;


    [Header("◆波の頂点操作パラメータ")]
    [Space(30)]
    public float WaveSpeed = 1f;
    public float WaveHeight = 0.1f;
    public float WaveArea = 100f;
    public float WaveInterval = 0.1f;
    public float WaveRing = 1; 
    public float WaveRingHeight = 1;
    public float WhiteHeight = 1;
    [Space(2)]
    public float UnderSeaView_AdjustedValue = 1.31f;

    [Header("◆マテリアルの動作速度に関数パラメータ")] 
    [Space(30)]
    [SerializeField] VectorXZ Waveheight_PlaneAxis = new VectorXZ() { x = 1, z = 1 };


    [Header("◆マテリアルの動作速度に関数パラメータ")]
    [Space(30)]
    public float MaterialMoveMaster;
    public SeaMaterial[] MaterialMove;
    [Range(1, 4)]
    public float SePow = 1.4f;
    public SeaMaterial[] SeaShadow; 

    [Header("#水中描画対象カメラ-------------------")]
    public Camera targetCamera;
    //ReadOnlyArea------------------------
    [Header("#読み取り専用パラメータ-------------------")]
    [Space(30)]
    [SerializeField] float Scale;
    [SerializeField] float PolyCount;

    public float GetScale => Scale;

    //-------------------------------------- 
    float Cnt;
    Vector3 startPos;
    MeshFilter meshFilter;
    MeshRenderer meshRender;
    private Mesh mesh;
    List<Vector3> vertexList;
    private List<int> indexList;
    List<Vector2> uvList;
    AudioSource waveSe;
    int cssbcnt;
    bool SetListFlag = true;

    [System.NonSerialized]
    public MeshRenderer CastSeaShadowBaseObj;

    [System.NonSerialized]
    public List<MeshRenderer> CastSeaShadowBaseList = new List<MeshRenderer>();

    static float WaveUp;
    public static Wave wave;  
    public static List<Transform> bubbleList = new List<Transform>();
   

    //エディタ用変数
    const bool IsActive = false;
    static int Len, st, en, cn; 
    const int perFlame = 10;
     
    /// <summary>
    /// 波の角度を返す
    /// </summary>
    /// <param name="d">座標(y軸無視)</param>
    /// <param name="tar"></param>
    /// <returns></returns>
    public static Quaternion GetSurfaceNormal(Vector3 d)
    {
        float w = wave.VerticesToVector * wave.WaveArea / 2;
        Vector3 c1 = wave.transform.position - new Vector3(w / 2, 0, w / 2);
        Vector3 c2 = wave.transform.position + new Vector3(w / 2, 0, w / 2);
        float ux = (d.z - c1.z) / (c2.z - c1.z) * wave.VerticesToVector; //0~1で表記した際の対象ｙとなるｘ座標
        float uy = (d.x - c1.x) / (c2.x - c1.x) * wave.VerticesToVector;//0~1で表記した際の対象ｙとなるｙ座標

        int x = (int)ux;
        int y = (int)uy;
        int wx = x + 1;
        int wy = y + 1;
        if (x < 0) x = 0; else if (x >= wave.VerticesToVector) x = wave.VerticesToVector - 1;
        if (y < 0) y = 0; else if (y >= wave.VerticesToVector) y = wave.VerticesToVector - 1;
        if (wx < 0) wx = 0; else if (wx >= wave.VerticesToVector) wx = wave.VerticesToVector - 1;
        if (wy < 0) wy = 0; else if (wy >= wave.VerticesToVector) wy = wave.VerticesToVector - 1;



        Vector3[] pointHeight = new Vector3[4]
        {
            wave.vertexList[y * wave.VerticesToVector + x] + Vector3.up * (WaveUp + Wave.wave.transform.position.y),
            wave.vertexList[y * wave.VerticesToVector + wx] + Vector3.up * (WaveUp + Wave.wave.transform.position.y),
            wave.vertexList[wy * wave.VerticesToVector + x] + Vector3.up * (WaveUp + Wave.wave.transform.position.y),
            wave.vertexList[wy * wave.VerticesToVector + wx] + Vector3.up * (WaveUp + Wave.wave.transform.position.y)
        };

        //float ez = Mathf.Asin(Mathf.Abs(pointHeight[1].y - pointHeight[2].y) / Vector3.Distance(pointHeight[1], pointHeight[2]));
        //float ex = Mathf.Asin(Mathf.Abs(pointHeight[0].y - pointHeight[3].y) / Vector3.Distance(pointHeight[0], pointHeight[3]));

        return Quaternion.Euler
        (
            (GetNormal(pointHeight[0], pointHeight[1], pointHeight[2]) +
            GetNormal(pointHeight[0], pointHeight[1], pointHeight[2]))
            *-0.5f
         );

        //return  Quaternion.Euler(ex*-90, 0, ez*90);
        /*こんな位置関係
                  x         wx
                  |          |
               y-[0]--------[1]--
                  |          |
                  |          |
                  |          |
                  |          |
                  |          |
              wy-[2]--------[3]-
                  |          |

     
      [?] = pointHeight
      [0]と[3]，[1]と[2]の高度差と距離を用いて計算する

       */

    }

    static Vector3 GetNormal(Vector3 v0, Vector3 v1, Vector3 v2)
    { 
        Vector3 a = new Vector3(v1.x - v0.x, v1.y - v0.y, v1.z - v0.z);
        Vector3 b = new Vector3(v2.x - v0.x, v2.y - v0.y, v2.z - v0.z);   
        return new Vector3 (a.y*b.z-a.z*b.y, a.z*b.x-a.x*b.z, a.x*b.y-a.y*b.x);
    }

    /// 波の角度を返す
    /// </summary>
    /// <param name="d">座標(y軸無視)</param> 
    /// <returns></returns>
    public static float GetSurfaceHeight(Vector3 d)
    {
        float w = wave.VerticesToVector * wave.WaveArea / 2;
        Vector3 c1 = wave.transform.position - new Vector3(w / 2, 0, w / 2);
        Vector3 c2 = wave.transform.position + new Vector3(w / 2, 0, w / 2);
        float ux = (d.z - c1.z) / (c2.z - c1.z) * wave.VerticesToVector; //0~1で表記した際の対象ｙとなるｘ座標
        float uy = (d.x - c1.x) / (c2.x - c1.x) * wave.VerticesToVector;//0~1で表記した際の対象ｙとなるｙ座標

        int x = (int)ux;
        int y = (int)uy;
        int wx = x + 1;
        int wy = y + 1;
        if (x < 0) x = 0; else if (x >= wave.VerticesToVector) x = wave.VerticesToVector - 1;
        if (y < 0) y = 0; else if (y >= wave.VerticesToVector) y = wave.VerticesToVector - 1;
        if (wx < 0) wx = 0; else if (wx >= wave.VerticesToVector) wx = wave.VerticesToVector - 1;
        if (wy < 0) wy = 0; else if (wy >= wave.VerticesToVector) wy = wave.VerticesToVector - 1;
       
        

        Vector3[] pointHeight = new Vector3[4]
        {
            wave.vertexList[y * wave.VerticesToVector + x] + Vector3.up * (WaveUp + Wave.wave.transform.position.y),
            wave.vertexList[y * wave.VerticesToVector + wx] + Vector3.up * (WaveUp + Wave.wave.transform.position.y),
            wave.vertexList[wy * wave.VerticesToVector + x] + Vector3.up * (WaveUp + Wave.wave.transform.position.y),
            wave.vertexList[wy * wave.VerticesToVector + wx] + Vector3.up * (WaveUp + Wave.wave.transform.position.y)
        };

        

        Vector2 pointLen = new Vector2((ux - x) / (wx - x), (uy - y) / (wy - y));

        Vector3[] LineHeight = new Vector3[4]
        {
            pointHeight[0] + (pointHeight[1]-pointHeight[0])*pointLen.x,
            pointHeight[0] + (pointHeight[2]-pointHeight[0])*pointLen.y,
            pointHeight[2] + (pointHeight[3]-pointHeight[2])*pointLen.x,
            pointHeight[1] + (pointHeight[3]-pointHeight[1])*pointLen.y
        };



        // Vector3 AveX = LineHeight[0] + (LineHeight[2] - LineHeight[0]) * pointLen.y;
        // Vector3 AveY = LineHeight[1] + (LineHeight[3] - LineHeight[1]) * pointLen.x;

        /*こんな位置関係
                    x         wx
                    |  LH[0]   |
                 y-[0]---X----[1]--
                    |    |     |
                    |    |     |
             LH[1]  Y----T-----| LH[3]
                    |    |     |
                    |    |     |
                wy-[2]--------[3]-
                    |  LH[2]   |

        LH = LineHeight
        [?] = pointHeight
        X,Y = pointLen
        
        LH[0]とLH[2]のY交差点の高度をAveX,
        LH[3]とLH[1]のX交差点の高度をAveYとする
        AveXとAveYの値は実質同じ（もしくは差は極小的なもの）であるため
        AveXとAveYの値の平均に海面フェースを座標的に動かしているWaveUp変数の値を足したものを
        高度として使用する。
         */ 
        return ((LineHeight[0] + LineHeight[1] + LineHeight[2] + LineHeight[3]) / 4).y;
    }



    //---------------------------------------//
    //エディタ―関連の処理-------------------//
    //---------------------------------------//

#if UNITY_EDITOR

    /// <summary>
    /// この変数はエディタ拡張用の変数です
    /// </summary>
    static int editorCnt;

    [InitializeOnLoadMethod]
    static void EDITOR_SET()
    {
        EditorApplication.hierarchyWindowChanged += EDITORUPDATE; 
    }

    void OnValidate()
    {
        if (!EditorApplication.isPlaying && Application.isEditor)
        { 
            MakeMesh();
            EDITORUPDATE();
        }
    }

    //水中にあるオブジェクトに影を付ける
    static void EDITORUPDATE()
    { 
        if (GameObject.Find("Sea") == null) return;
        if (wave == null) return;

        if (EditorApplication.isPlaying) return;
        wave.transform.Find("Canvas_UnderSeaView").GetComponent<Canvas>().worldCamera = wave.targetCamera;

        Len = UnityEngine.Object.FindObjectsOfType(typeof(GameObject)).Length;
        cn++; 
        st = cn * Len/ perFlame;
        en = (cn+1) * Len / perFlame;
        if (en >= Len)
        {
            en = Len - 1;
            cn = 0;
        }
         
    }
     
#endif

    /// <summary>
    /// wave変数をセットする
    /// </summary>
    void SetWaveComponent()
    {
        var w = GameObject.Find("Sea");
        if (w != null)
        {
            if (wave   == null) wave   = w.GetComponent<Wave>();
            if (waveSe == null) waveSe = w.GetComponent<AudioSource>();
        }
    }

    /// <summary>
    /// 波のメッシュ生成
    /// </summary>
    void MakeMesh()
    { 
        SetWaveComponent();
        transform.localScale = new Vector3(1, 1, 1);
        meshFilter = GetComponent<MeshFilter>();
        meshRender = GetComponent<MeshRenderer>(); 
        mesh = CreatePlaneMesh(); 
        meshFilter.mesh = mesh;
        Scale = VerticesToVector * WaveArea/2;
        PolyCount = VerticesToVector * VerticesToVector;
    }



    /// <summary>
    /// MakeMeshで呼ばれる
    /// </summary>
    /// <returns></returns>
    private Mesh CreatePlaneMesh()
    {
        var mesh = new Mesh();
        vertexList = new List<Vector3>();
        indexList = new List<int>();
        List<Vector2> uvList = new List<Vector2>();

        for (int x = 0; x < VerticesToVector; x++)
        {
            for (int z = 0; z < VerticesToVector; z++)
            {
                Vector3 f = new Vector3(x - VerticesToVector / 2, 0, z - VerticesToVector / 2) * WaveArea / 2;
                vertexList.Add(transform.position + f);
                uvList.Add(new Vector2(x * 1f / VerticesToVector, z * 1f / VerticesToVector));
            }
        }

        mesh.SetVertices(vertexList);//meshに頂点群をセット
        mesh.SetUVs(0, uvList);

        //サブメッシュの設定----------------------------------------------------
        for (int x = 0; x < VerticesToVector - 1; x++)
        {
            for (int z = 0; z < VerticesToVector - 1; z++)
            {
                //0,2,1,1,2,3の順
                int V0 = x + (z * VerticesToVector);
                int V1 = V0 + 1;
                int V2 = V0 + VerticesToVector;
                int V3 = V2 + 1;
                indexList.Add(V0);
                indexList.Add(V2);
                indexList.Add(V1);
                indexList.Add(V1);
                indexList.Add(V2);
                indexList.Add(V3);
            }
        }

        mesh.SetIndices(indexList.ToArray(), MeshTopology.Triangles, 0);//メッシュにどの頂点の順番で面を作るかセット

        return mesh;
    }


    



    private void Awake()
    {
        MakeMesh();
        bubbleList = new List<Transform>();
        CastSeaShadowBaseList = new List<MeshRenderer>();
        CastSeaShadowBaseObj = null;
    }




    private void Start()
    { 
        meshFilter = GetComponent<MeshFilter>();
        meshRender = GetComponent<MeshRenderer>();
        wave = null;
        SetWaveComponent(); 
        startPos = transform.position;
        Material[] a = new Material[MaterialMove.Length];
        for (int g = 0; g < a.Length; g++)
            a[g] = MaterialMove[g].material;
        meshRender.materials = a;
        SetListFlag = true;
    }



    public void Update()
    {
        if (CastSeaShadowBaseList != null && SetListFlag && CastSeaShadowBaseList.Count > 0)
        {
            SetListFlag = false;
            cssbcnt = CastSeaShadowBaseObj.sharedMaterials.Length - SeaShadow.Length;
            CastSeaShadowBaseList = null;    
        }
         
        SetWaveComponent(); 
        waveSe.volume = Mathf.Pow((Vector3.Distance(new Vector3(wave.targetCamera.transform.position.x,0, wave.targetCamera.transform.position.z),Vector3.zero)/ Mathf.Abs(wave.targetCamera.transform.position.y/10) / (Scale / 2)), SePow);
        WaveUp = WaveHeight / 22 * Mathf.Sin(Cnt);
        Cnt += Time.deltaTime * WaveSpeed;
        transform.position = new Vector3(startPos.x, startPos.y + WaveUp, startPos.z);
      
        for (int x = 0; x < VerticesToVector - 1; x++)
        {
            for (int z = 0; z < VerticesToVector - 1; z++)
            {
                int i = z * VerticesToVector + x;
                var v = vertexList[i];
                float d = ((float)(x - VerticesToVector / 2) * (x - VerticesToVector / 2) + (float)(z - VerticesToVector / 2) * (z - VerticesToVector / 2)) ;
                float sinx = Mathf.Sin(WaveInterval * x * Cnt);
                float sinz = Mathf.Sin(WaveInterval * z * Cnt);
                
                //ここで波を加工 -----------------------------
                v.y = WaveHeight * (d / VerticesToVector + 1) * 
                (
                    Mathf.Pow(sinx, 2) * Waveheight_PlaneAxis.x +
                    Mathf.Pow(sinz, 2) * Waveheight_PlaneAxis.z +
                    Mathf.Sin(WaveInterval * (d + 1) * WaveRing * Mathf.Sin(Cnt)) * WaveRingHeight
                );
                //--------------------------------------------
                vertexList[i] = v;
            }
        }
        mesh.SetVertices(vertexList); 


        //マテリアル設定－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－
        int cc = meshRender.materials.Length >= MaterialMove.Length ? meshRender.materials.Length : MaterialMove.Length;
        for (int e = 0; e < cc; e++)
            meshRender.materials[e].mainTextureOffset += MaterialMoveMaster * MaterialMove[e].move * Time.deltaTime;
        //泡がらみ 
        if (GetSurfaceHeight(wave.targetCamera.transform.position) > wave.targetCamera.transform.position.y)
        { 
            foreach (Transform t in bubbleList.ToArray())
                t.gameObject.active = Vector3.Distance(wave.targetCamera.transform.position, t.position) < 50;
        }
        else
        {
            foreach (Transform t in bubbleList.ToArray())
                t.gameObject.active = false;
        }


        //影がらみ
        if (CastSeaShadowBaseList != null && CastSeaShadowBaseList.Count > 0)
        {
            for (int c = 0; c < SeaShadow.Length; c++)
            {
                CastSeaShadowBaseObj.sharedMaterials[c+ cssbcnt].mainTextureOffset += SeaShadow[c].move;
            }
        }
    }
    
}



[System.Serializable]
public class VectorXZ
{
    public float x, z;
}



[System.Serializable]
public class SeaMaterial
{
    public Vector2 move;
    public Material material;
}


