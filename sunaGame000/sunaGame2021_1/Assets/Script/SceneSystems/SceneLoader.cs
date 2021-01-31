using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
 
public static class SceneLoader
{

    public static bool IsFade
    {
        get
        {
            return Fader.IsFade;
        }
    }

    public static float Fadeings
    {
        get
        {
            return Fader.Fadeings_;
        }
    }

    public static Color FadeColor
    {
        set
        {
            Fader.FadeColor = value;
        }
        get
        {
            return Fader.FadeColor;
        }
    }



    public static float FadeInTime
    {
        get
        {
            return Fader.FadeInTime;
        }
        set
        {
            if(value < 0)
                Debug.LogError("フェードイン、フェードアウトの時間は正の値でなくてはなりません");
            Fader.FadeInTime = value > 0 ? value : 2;
        }
    }




    public static float FadeOutTime
    {
        get
        {
            return Fader.FadeOutTime;
        }
        set
        {
            if (value < 0)
                Debug.LogError("フェードイン、フェードアウトの時間は正の値でなくてはなりません");
            Fader.FadeOutTime = value > 0 ? value : 2;
        }
    }





    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void StartUp()
    { 
        SceneManager.sceneLoaded += delegate (Scene n, LoadSceneMode m) 
        {
            Fader.FadeHandle = true;
            Fader.INIT();
        };
    }
    

     
    public static void LoadN(string name)
    {
        SceneManager.LoadScene(name);
    }

    public static void LoadN(int index  )
    {
        SceneManager.LoadScene(index);
    }

    public static void Load(string name)
    {
        fdr().Set(name);
    }

    public static void Load(int index  )
    {
        fdr().Set(index);
    }

    public static void Load_DX(string name)
    {
        Debug.Log("DX版は未実装です");
        fdr().Set(name);
    }

    public static void Load_DX(int index)
    {
        Debug.Log("DX版は未実装です.");
        fdr().Set(index);
    }

    static Fader fdr()
    {
        DateTime db = DateTime.Now;
        GameObject canvas = new GameObject("[Fade System]");
        canvas.AddComponent<Fader>();
        canvas.AddComponent<RectTransform>();
        Canvas cc = canvas.AddComponent<Canvas>();
        cc.renderMode = RenderMode.ScreenSpaceOverlay;
        cc.sortingOrder = 38;
        CanvasScaler cs = canvas.AddComponent<CanvasScaler>();
        cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        cs.referenceResolution = new Vector2(100, 100);
        cs.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        cs.matchWidthOrHeight = 0;
        cs.referencePixelsPerUnit = 100;

        GameObject img = new GameObject("Image");
        img.transform.SetParent(canvas.transform);
        RectTransform ir = img.AddComponent<RectTransform>();
        ir.position = new Vector3(0, 0, 0);
        ir.rotation = Quaternion.Euler(0,0,0);
        ir.anchorMin = new Vector2(0.5f, 0.5f);
        ir.anchorMax = new Vector2(0.5f, 0.5f);
        ir.pivot = new Vector2(0.5f, 0.5f);
        ir.localScale = new Vector3(1,1,1);
        ir.sizeDelta = new Vector2(10000, 10000);
        img.AddComponent<CanvasRenderer>();
        RawImage r = img.AddComponent<RawImage>();
        r.uvRect = new Rect(0, 0, 1, 1);

        DateTime da = DateTime.Now;
        return canvas.GetComponent<Fader>();
        //  return (MonoBehaviour.Instantiate(Resources.Load("fader") as GameObject) as GameObject).AddComponent<Fader>();
    }


} 


 

enum FADEMODE
{
    IN = -1,
    OUT = 1
}

public enum FADETYPE { Normal, DX }


public class Fader : MonoBehaviour
{
    public static bool FadeHandle, IsFade;
    static bool IsActive;
    FADEMODE mode = FADEMODE.OUT;
    public FADETYPE type_ = FADETYPE.Normal;
    string _name = "";
    int _index = int.MinValue;
    public RawImage targetUI;
    public static Color FadeColor = new Color(0,0,0,1);
    public static float FadeInTime = 2;
    public static float FadeOutTime = 2;
    float vals;

    public static void INIT() { Fadeings = -1; }
    static float Fadeings;
    public static float Fadeings_ { get {return Fadeings; } } 

    public void Set(string s)
    {
        SetRAW();
        _name = s;
    }


    public void Set(int a)
    {
        SetRAW();
        _index = a;
    }



    void SetRAW()
    {
        IsFade = true;
        Fadeings = 1;
        if (IsActive)
            Debug.LogError("同時に2つのフェード処理はできません");
        IsActive = true;
        DontDestroyOnLoad(gameObject);
        FadeHandle = false; 
        targetUI = transform.Find("Image").GetComponent<RawImage>();
        targetUI.color = new Color(FadeColor.r, FadeColor.g, FadeColor.b, 0);
    }

    

     
    void Update()
    { 
        Fadeings -= Time.deltaTime / (mode == FADEMODE.OUT ? SceneLoader.FadeOutTime : SceneLoader.FadeInTime);
        if (Fadeings < -1) Fadeings = -1;
        print("fadeings:::" + Fadeings_);
        vals += Time.deltaTime;

        if (type_ == FADETYPE.DX)
        {
            type_ = FADETYPE.Normal;
            if (mode == FADEMODE.IN)
            {
                //変化
            }

            else
            {
                //変化
            }
        }

        else if (type_ == FADETYPE.Normal)
            targetUI.color = new Color(FadeColor.r, FadeColor.g, FadeColor.b, 1 - Mathf.Abs(Fadeings_));
 

        if (mode == FADEMODE.IN)
        {
            if (Fadeings_ <= -1f)
            {
                IsActive = false;
                Destroy(gameObject);
                IsFade = false;
                _name = "";
                _index = -1;
                //Fadeings = -1;
            }
        } 
        else
        {
            if (FadeHandle)
                mode = FADEMODE.IN;

            if (Fadeings_ <= 0)
            {
                //Fadeings = 0;
                if (_name != string.Empty)
                    SceneLoader.LoadN(_name);
                else if (_index >= 0)
                    SceneLoader.LoadN(_index);
            }
        }


    } 


}

/*
 using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
 
public static class SceneLoader
{

    public static bool IsFade
    {
        get
        {
            return Fader.IsFade;
        }
    }

    public static float Fadeings
    {
        get
        {
            return Fader.Fadeings_;
        }
    }

    public static Color FadeColor
    {
        set
        {
            Fader.FadeColor = value;
        }
        get
        {
            return Fader.FadeColor;
        }
    }



    public static float FadeInTime
    {
        get
        {
            return Fader.FadeInTime;
        }
        set
        {
            if(value < 0)
                Debug.LogError("フェードイン、フェードアウトの時間は正の値でなくてはなりません");
            Fader.FadeInTime = value > 0 ? value : 2;
        }
    }




    public static float FadeOutTime
    {
        get
        {
            return Fader.FadeOutTime;
        }
        set
        {
            if (value < 0)
                Debug.LogError("フェードイン、フェードアウトの時間は正の値でなくてはなりません");
            Fader.FadeOutTime = value > 0 ? value : 2;
        }
    }





    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void StartUp()
    { 
        SceneManager.sceneLoaded += delegate (Scene n, LoadSceneMode m) 
        {
            Fader.FadeHandle = true;
            Fader.INIT();
        };
    }
    

     
    public static void LoadN(string name)
    {
        SceneManager.LoadScene(name);
    }

    public static void LoadN(int index  )
    {
        SceneManager.LoadScene(index);
    }

    public static void Load(string name)
    {
        fdr().Set(name);
    }

    public static void Load(int index  )
    {
        fdr().Set(index);
    }

    public static void Load_DX(string name)
    {
        Debug.Log("DX版は未実装です");
        fdr().Set(name);
    }

    public static void Load_DX(int index)
    {
        Debug.Log("DX版は未実装です.");
        fdr().Set(index);
    }

    static Fader fdr()
    {
        DateTime db = DateTime.Now;
        GameObject canvas = new GameObject("[Fade System]");
        canvas.AddComponent<Fader>();
        canvas.AddComponent<RectTransform>();
        Canvas cc = canvas.AddComponent<Canvas>();
        cc.renderMode = RenderMode.ScreenSpaceOverlay;
        cc.sortingOrder = 38;
        CanvasScaler cs = canvas.AddComponent<CanvasScaler>();
        cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        cs.referenceResolution = new Vector2(100, 100);
        cs.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        cs.matchWidthOrHeight = 0;
        cs.referencePixelsPerUnit = 100;

        GameObject img = new GameObject("Image");
        img.transform.SetParent(canvas.transform);
        RectTransform ir = img.AddComponent<RectTransform>();
        ir.position = new Vector3(0, 0, 0);
        ir.rotation = Quaternion.Euler(0,0,0);
        ir.anchorMin = new Vector2(0.5f, 0.5f);
        ir.anchorMax = new Vector2(0.5f, 0.5f);
        ir.pivot = new Vector2(0.5f, 0.5f);
        ir.localScale = new Vector3(1,1,1);
        ir.sizeDelta = new Vector2(10000, 10000);
        img.AddComponent<CanvasRenderer>();
        RawImage r = img.AddComponent<RawImage>();
        r.uvRect = new Rect(0, 0, 1, 1);

        DateTime da = DateTime.Now;
        return canvas.GetComponent<Fader>();
        //  return (MonoBehaviour.Instantiate(Resources.Load("fader") as GameObject) as GameObject).AddComponent<Fader>();
    }


} 


 

enum FADEMODE
{
    IN = -1,
    OUT = 1
}

public enum FADETYPE { Normal, DX }


public class Fader : MonoBehaviour
{
    public static bool FadeHandle, IsFade;
    static bool IsActive;
    FADEMODE mode = FADEMODE.OUT;
    public FADETYPE type_ = FADETYPE.Normal;
    string _name = "";
    int _index = int.MinValue;
    public RawImage targetUI;
    public static Color FadeColor = new Color(0,0,0,1);
    public static float FadeInTime = 2;
    public static float FadeOutTime = 2;
    float vals;

    public static void INIT() { Fadeings = 1; }

    static float Fadeings;
    public static float Fadeings_ { get {return Fadeings; } } 

    public void Set(string s)
    {
        SetRAW();
        _name = s;
    }


    public void Set(int a)
    {
        SetRAW();
        _index = a;
    }



    void SetRAW()
    {
        IsFade = true;
        Fadeings = 1;
        if (IsActive)
            Debug.LogError("同時に2つのフェード処理はできません");
        IsActive = true;
        DontDestroyOnLoad(gameObject);
        FadeHandle = false; 
        targetUI = transform.Find("Image").GetComponent<RawImage>();
        targetUI.color = new Color(FadeColor.r, FadeColor.g, FadeColor.b, 0);
    }

    

     
    void Update()
    { 
        Fadeings -= Time.deltaTime * 1 / (mode == FADEMODE.OUT ? SceneLoader.FadeOutTime : SceneLoader.FadeInTime);
        if (Fadeings < -1) Fadeings = -1;
        print("fadeings:::" + Fadeings_);
        vals += Time.deltaTime;

        if (type_ == FADETYPE.DX)
        {
            type_ = FADETYPE.Normal;
            if (mode == FADEMODE.IN)
            {
                //変化
            }

            else
            {
                //変化
            }
        }

        if (type_ == FADETYPE.Normal)
        {

            if (mode == FADEMODE.IN)
            {
                targetUI.color = new Color(FadeColor.r, FadeColor.g, FadeColor.b, 1 - (float)Math.Sin(((vals - FadeOutTime) / FadeInTime) * 90 * (Math.PI / 180)));
            }

            else
            {
                targetUI.color = new Color(FadeColor.r, FadeColor.g, FadeColor.b, (float)Math.Sin((vals / FadeOutTime) * 90 * (Math.PI / 180)));
            }
        }


        if (mode == FADEMODE.IN)
        {
            if (vals >= FadeInTime + FadeOutTime)
            {
                IsActive = false;
                Destroy(gameObject);
                IsFade = false;
                _name = "";
                _index = -1;
                Fadeings = -1;
            }
        } 
        else
        { 
            if (FadeHandle)
                mode = FADEMODE.IN;

            if (vals >= FadeOutTime)
            {

                Fadeings = 0;
                if (_name != string.Empty)
                    SceneLoader.LoadN(_name);
                else if (_index >= 0)
                    SceneLoader.LoadN(_index);
            }
        }


    } 


}


     */
