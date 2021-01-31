using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LOD_Manager : LOD_Conig
{

    public static Transform _camera; //カメラのトランスフォーム
    Vector3 LastLodPosition;

    static List<LOD_DATA>  objList; //LODのオブジェクト
    static List<Transform> objHL; //2値的なLODのオブジェクト
    int cnt_;
    static int[,] pointData;
    static bool FLAG; //trueならカメラ距離確認, false はLOD処理 
    static bool init;
   
    const float HLR = HL_RangeRadius * HL_RangeRadius; //HL_RangeRadiusの２乗(処理の軽減のため)
    const float HLR2 = HighLevelRangeRadius * HighLevelRangeRadius; //HighLevelRangeRadiusの２乗(処理の軽減のため)
    const float MLR2 = MidLevelRangeRadius * MidLevelRangeRadius; //MidLevelRangeRadiusの２乗(処理の軽減のため)
    const float LLR2 = LowLevelRangeRadius * LowLevelRangeRadius; //LowLevelRangeRadiusの２乗(処理の軽減のため)
    const float AdjustedFULL = AdjustedMaximumAltitude - AdjustedMiniimumAltitude;

    float High, Mid, Low;
    /// <summary>
    /// LODのリストに追加する
    /// </summary>　
    public static void ADDLOD(Transform t)
    {
        if(!init)
            objList.Add(new LOD_DATA(t));
    }


    /// <summary>
    /// LOD_HLのリストに追加する
    /// </summary>　
    public static void ADDLOD_HL(Transform t)
    {
        if (!init)
            objHL.Add(t);
    }



    void Awake()
    { 
        objList = new List<LOD_DATA>();
        objHL = new List<Transform>(); 
    }

     
    float h_1 = 0;
    void Update()
    { 


        if (!init)
        {
            pointData = new int[2,UpdatesPerWhatFrame];
            for (int h = 0; h < UpdatesPerWhatFrame; h++)
            {
                pointData[0, h] = objList.Count / UpdatesPerWhatFrame;
                pointData[1, h] = objHL.Count / UpdatesPerWhatFrame;
            }
            int c1 = objList.Count - ((int)(objList.Count / UpdatesPerWhatFrame)) * UpdatesPerWhatFrame;
            int c2 = objHL.Count - ((int)(objHL.Count / UpdatesPerWhatFrame)) * UpdatesPerWhatFrame;
            for (int h = 0; h < c1; h++) pointData[0, h]++;
            for (int h = 0; h < c2; h++) pointData[1, h]++;
            var XX = pointData;
            pointData = new int[2, UpdatesPerWhatFrame];
            for (int g = 0; g < UpdatesPerWhatFrame; g++)
            {
                for (int x = 0; x <= g; x++)
                {
                    pointData[0, g] += XX[0, x];
                    pointData[1, g] += XX[1, x];
                }
            }

            FLAG = false;
            cnt_ = 0;
            LastLodPosition = _camera.transform.position;

            init = true;
        }
        /*
        Debug.Log 
            (
            FLAG.ToString().PadRight(5,' ') + "::" + cnt_.ToString().PadLeft(2,'0') + "/"+ 
            Vector3.Distance(_camera.position, LastLodPosition).ToString().PadRight(14,' ')+"::"+
            transform.position.ToString().PadRight(18, ' ') + ":" + 
            LastLodPosition.ToString().PadRight(18, ' '));
        */
        /*
        if (_camera.transform.position.y < AdjustedMiniimumAltitude)
        {
            _camera.transform.position = new Vector3
            (
                _camera.transform.position.x,
                AdjustedMiniimumAltitude,
                _camera.transform.position.z
             );
        }
        else*/
        if (_camera.transform.position.y > AdjustedMaximumAltitude)
        {
            _camera.transform.position = new Vector3
            (
                _camera.transform.position.x,
                AdjustedMaximumAltitude,
                _camera.transform.position.z
             );
        }



        //カメラの移動量でLODするか決める
        //カメラが移動していないなら更新しなくていいから
        if (FLAG)
        { 
            if (Vector3.Distance(_camera.position, LastLodPosition) > MovementThresholdForUpdate)
            {
                 FLAG = false;
            }
        } 
        else 
        {
            //高度を0.0f～1.0fに変換
            h_1 = (_camera.position.y-AdjustedMiniimumAltitude) / AdjustedFULL;
            //H/M/Lの値を作成
            High = HLR2 * S_H;
            Mid = MLR2 * S_M;
            Low = LLR2 * S_L;
            //UpdatesPerWhatFrameのフレームで１周するような値を作成
            //min,min2はfor文の先頭に,max,max2は終わりに指定する
            bool ee = (cnt_ - 1 < 0);
            int min  = ee ? 0 : pointData[0, cnt_ - 1];
            int min2 = ee ? 0 : pointData[1, cnt_ - 1];
            int max  = pointData[0, cnt_] - 1;
            int max2 = pointData[1, cnt_] - 1;
              
            //Debug.Log("F ::: " + FLAG + "  ///  cnt_  ::: " + cnt_ + "  /// min ::" + min+ " //  max ::" + max + " //  min2::" + min2 + "  //  max2::" + max2 + " //// LODLEN::" + objList.Count + "HLLEN:::" + objHL.Count);
            //objList[min～max]のLOD処理を行う
            //３項演算子を用いればもっとすっきりとかけたが
            //処理速度がこっちのがすこし早いためこっちにしました
            for (int v = min; v < max; v++)
            {
                float d2 =
                   (objList[v].ROOT.position.x - _camera.position.x) * (objList[v].ROOT.position.x - _camera.position.x) +
                   (objList[v].ROOT.position.y - _camera.position.y) * (objList[v].ROOT.position.y - _camera.position.y) +
                   (objList[v].ROOT.position.z - _camera.position.z) * (objList[v].ROOT.position.z - _camera.position.z);

                if (d2 < High)//ここで高さ計算
                {
                    objList[v].HIGH.gameObject.active = true;
                    objList[v].MID.gameObject.active = false;
                    objList[v].LOW.gameObject.active = false;
                }
                else if (d2 < Mid)
                {
                    objList[v].HIGH.gameObject.active = false;
                    objList[v].MID.gameObject.active = true;
                    objList[v].LOW.gameObject.active = false;
                }
                else if (d2 < Low)
                {
                    objList[v].HIGH.gameObject.active = false;
                    objList[v].MID.gameObject.active = false;
                    objList[v].LOW.gameObject.active = true;
                }
                else
                {
                    objList[v].HIGH.gameObject.active = false;
                    objList[v].MID.gameObject.active = false;
                    objList[v].LOW.gameObject.active = false;
                }
            }
              
            //objHL[min～max]のLOD処理(表示or非表示)を行う
            //３項演算子を用いればもっとすっきりとかけたが
            //処理速度がこっちのがすこし早いためこっちにしました
            for (int v = min2; v < max2; v++)
            {
                objHL[v].gameObject.active = HLR >
                   (objHL[v].position.x - _camera.position.x) * (objHL[v].position.x - _camera.position.x) +
                   (objHL[v].position.y - _camera.position.y) * (objHL[v].position.y - _camera.position.y) +
                   (objHL[v].position.z - _camera.position.z) * (objHL[v].position.z - _camera.position.z);
            }


            //cnt_を0~UpdatesPerWhatFrame-1でループする
            //LODが一周したらFLAGをfalseにしてその他いろいろする
            cnt_++;
            if (cnt_ == UpdatesPerWhatFrame)
            {
                FLAG = true;
                cnt_ = 0;
                LastLodPosition = _camera.position;
            }
             
        }



    }

    float S_H { get { return (1 - h_1 * 2) < 0 ? 0 : (1 - h_1 * 2); } }
    float S_M { get { return 1 - (h_1 * h_1); } }
    float S_L { get { return 1 + h_1* h_1 * (AdjustedFULL/ LowLevelRangeRadius); } }

#if UNITY_EDITOR
    const float line = 8000; 
    const int t = 5;
    const int max = 10;
    Vector3 cc;
    private void OnDrawGizmos()
    { 
        cc = _camera == null ? Camera.main.transform.position : _camera.position;
        
       // if (cc.y < AdjustedMiniimumAltitude)
         //   cc.y = AdjustedMiniimumAltitude;
       if (cc.y > AdjustedMaximumAltitude)
           cc.y = AdjustedMaximumAltitude;
        if (!init)
            h_1 = (cc.y - AdjustedMiniimumAltitude) / AdjustedFULL;

        if (_camera != null)
            _camera.position = cc;
        else
            Camera.main.transform.position = cc; 

        DRAWAREA(Color.white, HL_RangeRadius);
        DRAWAREA(Color.red, HighLevelRangeRadius * S_H);
        DRAWAREA(Color.green, MidLevelRangeRadius * S_M);
        DRAWAREA(Color.blue, LowLevelRangeRadius * S_L);

        Gizmos.color = Color.magenta;
        DRAWRECT(AdjustedMaximumAltitude);
        Gizmos.color = Color.cyan;
        DRAWRECT(AdjustedMiniimumAltitude);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(-line/2, AdjustedMiniimumAltitude, -line / 2), new Vector3(-line/2, AdjustedMaximumAltitude, -line / 2));
        Gizmos.DrawLine(new Vector3(-line / 2, AdjustedMiniimumAltitude, line / 2), new Vector3(-line/2, AdjustedMaximumAltitude, line/2));
        Gizmos.DrawLine(new Vector3(line/ 2, AdjustedMiniimumAltitude, -line / 2), new Vector3(line/2, AdjustedMaximumAltitude, -line / 2));
        Gizmos.DrawLine(new Vector3(line/ 2, AdjustedMiniimumAltitude, line / 2), new Vector3(line / 2, AdjustedMaximumAltitude, line / 2)); 
    }

    void DRAWAREA(Color c,float vv)
    { 
        Gizmos.color = c;
        Gizmos.DrawWireSphere(cc, vv); 
    }

    void DRAWRECT(float Y)
    {
        Vector3 pipot = new Vector3(-line/2, Y,-line/2);
        Gizmos.DrawLine(pipot + new Vector3(line, 0, line), pipot + new Vector3(line, 0, 0));
        Gizmos.DrawLine(pipot + new Vector3(line, 0, line), pipot + new Vector3(0, 0, line));
        Gizmos.DrawLine(pipot, pipot + new Vector3(line, 0, 0));
        Gizmos.DrawLine(pipot, pipot + new Vector3(0, 0, line));
        for (int v = 0; v <= t; v++) 
            Gizmos.DrawLine(pipot + (Vector3.right * line * v/ max) , pipot + (Vector3.fwd * line * v / max));
    }
#endif

}
 

/*
  |
 _|_
|- -| < 死にそう...
|_-_|    ____
 | |    |    |
 | |    |    | 
 ^ ^    |    |
        |  ・| 
      	|    |
  ==|   |    |
  ==|	|____|
================ 
*/
