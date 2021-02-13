using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(TimeData))]
public class Editor_TimeData : Editor
{
    SerializedProperty _m, _h;
    public void OnEnable()
    {
        _h = serializedObject.FindProperty("hour");
        _m = serializedObject.FindProperty("min");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        using (new EditorGUILayout.HorizontalScope())
        {
            EditorGUILayout.PropertyField(_h);
            EditorGUILayout.PropertyField(_m);
        }
        serializedObject.ApplyModifiedProperties();
    }
}
#endif

[System.Serializable]
public class TimeData : MonoBehaviour
{
    public static float TimePoint => (_CLOCK.hour * 60 + _CLOCK.min) / 1440f;

    public static TimeData clock
    {
        get
        {
            return _CLOCK;
        }
        set
        {
            value._Update();
            _CLOCK = value;
        }
    }

    static TimeData _CLOCK;

    [SerializeField]
    public int hour;

    [SerializeField]
    public int min;

#if UNITY_EDITOR
    private void OnValidate()
    {
        _Update();
        clock = this;
    }
#endif


    static void MakeTimer()
    {
        if (_CLOCK == null)
        {
            var tt = GameObject.Find("[CLOCK]");
            if (tt == null)
                _CLOCK = new GameObject("[CLOCK]").AddComponent<TimeData>();
            else if (tt.GetComponent<TimeData>() == null)
                _CLOCK = tt.AddComponent<TimeData>();
        }
    }

    void Start() { clock = this; }
    void Update() { _Update(); }
    void FixedUpdate() { _Update(); }

    void _Update()
    {
        if (min >= 60) hour += min / 60;
        else if (min < 0) hour -= min / 60 + 1;

        min = (60 + min) % 60;

        if (hour >= 24) hour = (hour + 24) % 24;
        else if (hour < 0) hour = 24 + hour;
    }

}


