using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Key
{
    public static SelectGamePad gamePad;
    public static int select;

    public static int SetGamePad 
    {
        get { return select; }
        set
        {
            select = value;
            if (select < 0) select = 0;
            else if (select > 1) select = 1; //add
            switch (select)
            {
                case 0: gamePad = SelectGamePad.PS4; break;
                case 1: gamePad = SelectGamePad.XBOX360; break;
                    //add
            }
        }
    }

    //================================================   [KEYBOARD]　　　　　[PS4]　　　　　　　　　XBOX360
    public static readonly GamePad_TB A  = new GamePad_TB(KeyCode.Z, KeyCode.JoystickButton1,  KeyCode.JoystickButton0); 
    public static readonly GamePad_TB B  = new GamePad_TB(KeyCode.X, KeyCode.JoystickButton2,  KeyCode.JoystickButton1); 
    public static readonly GamePad_TB X  = new GamePad_TB(KeyCode.C, KeyCode.JoystickButton0,  KeyCode.JoystickButton2); 
    public static readonly GamePad_TB Y  = new GamePad_TB(KeyCode.V, KeyCode.JoystickButton3,  KeyCode.JoystickButton3); 
    public static readonly GamePad_TB FL = new GamePad_TB(KeyCode.B, KeyCode.JoystickButton8,  KeyCode.JoystickButton6); 
    //public static readonly GamePad_TB FC = new GamePad_TB(KeyCode.N, KeyCode.JoystickButton14, KeyCode.JoystickButton15); //X
    public static readonly GamePad_TB FR = new GamePad_TB(KeyCode.M, KeyCode.JoystickButton9, KeyCode.JoystickButton7);

    //=================================================================================
    public static readonly GamePad_JS JoyStickL = new GamePad_JS("Horizontal", "Vertical", "X axis",   "Y axis",   "X axis",   "Y axis", KeyCode.A, KeyCode.D, KeyCode.S, KeyCode.W);
    public static readonly GamePad_JS JoyStickR = new GamePad_JS("JS_RH",      "JS_RV",    "3rd axis", "6th axis", "4th axis", "5th axis", KeyCode.F, KeyCode.H, KeyCode.G, KeyCode.T);

    //================================================================
    public static readonly GamePad_Trigger _L  = new GamePad_Trigger();
    public static readonly GamePad_Trigger _LT = new GamePad_Trigger();
    public static readonly GamePad_Trigger _R  = new GamePad_Trigger();
    public static readonly GamePad_Trigger _RT = new GamePad_Trigger();
}


public enum SelectGamePad
{
    PS4,
    XBOX360
    //add
}


public class GamePad_Trigger
{
    //add

}


public class GamePad_JS
{
    public static bool InvertX, InvertY;
    string[,] tags = new string[3, 2]; //add
    KeyCode[] H, V;
    public static float sensivirity = 0.4f;
    public GamePad_JS
    (
        string KeyBoard_X,
        string KeyBoard_Y,
        string PS4_X,
        string PS4_Y,
        string XBOX360_X,
        string XBOX360_Y,
        KeyCode keyPush_0a,
        KeyCode keyPush_0b,
        KeyCode keyPush_1a,
        KeyCode keyPush_1b
    )
    {
        H = new KeyCode[] { keyPush_0a, keyPush_0b };
        V = new KeyCode[] { keyPush_1a, keyPush_1b };
        tags[0, 0] = KeyBoard_X;
        tags[0, 1] = KeyBoard_Y;
        tags[1, 0] = PS4_X;
        tags[1, 1] = PS4_Y;
        tags[2, 0] = XBOX360_X;
        tags[2, 1] = XBOX360_Y;
        //add
    }

    Vector2 Inverts { get { return new Vector2(InvertX ? -1 : 1, InvertY ? 1 : -1); } }

    public Vector2 Get
    {
        get
        {
            switch (Key.gamePad)
            {
                case SelectGamePad.PS4: return sensivirity * Inverts * new Vector2(GetF(tags[0,0], tags[1, 0]), GetF(tags[0, 1], tags[1, 1])) ;
                case SelectGamePad.XBOX360: return sensivirity * Inverts * new Vector2(GetF(tags[0, 0], tags[2, 0]), GetF(tags[0, 1], tags[2, 1]));
                    //add
            }
            return new Vector2(0, 0);
        }
    }

    float GetF(string x0, string x1)
    {
        float p0 = Input.GetAxis(x0);
        float p1 = Input.GetAxis(x1);
        return p0 * p0 > p1 * p1 ? p0 : p1;
    }

    public bool Push
    {
        get
        {

            bool kb = (Input.GetKey(H[0]) == Input.GetKey(H[1])) || (Input.GetKey(V[0]) == Input.GetKey(V[1]));
            if (kb) return kb;

            switch (Key.gamePad)
            {
                case SelectGamePad.PS4: return false;
                case SelectGamePad.XBOX360: return false;
                    //add
            }

            Debug.Log("ジョイスティックのプッシュ検知はまだ未実装です:常にfalseを返します");
            return false;
        }
    }

    public static implicit operator Vector2(GamePad_JS v) {  return v.Get; }

}

public class GamePad_TB
{
    KeyCode ps4, xbox360, keyboard;//add

    public GamePad_TB(KeyCode keyboard, KeyCode ps4, KeyCode xbox360)
    {
        this.ps4 = ps4;
        this.xbox360 = xbox360;
        this.keyboard = keyboard;
        //add
    }

    public bool Press
    {
        get
        {
            switch (Key.gamePad)
            {
                case SelectGamePad.PS4    : return Input.GetKey(keyboard) || Input.GetKey(ps4);
                case SelectGamePad.XBOX360: return Input.GetKey(keyboard) || Input.GetKey(xbox360);
                    //add
            }
            return false;
        }
    }

    public bool Down
    {
        get
        {
            switch (Key.gamePad)
            {
                case SelectGamePad.PS4: return Input.GetKeyDown(keyboard) || Input.GetKeyDown(ps4);
                case SelectGamePad.XBOX360: return Input.GetKeyDown(keyboard) || Input.GetKeyDown(xbox360);
                //add
            }
            return false;
        }
    }

    public bool Up
    {
        get
        {
            switch (Key.gamePad)
            {
                case SelectGamePad.PS4: return Input.GetKeyUp(keyboard) || Input.GetKeyUp(ps4);
                case SelectGamePad.XBOX360: return Input.GetKeyUp(keyboard) || Input.GetKeyUp(xbox360);
                    //add
            }
            return false;
        }
    }
}

/*
 * push / Trigger/ Manual
 新規コントローラ追加時には//addに記載を足す。
     */