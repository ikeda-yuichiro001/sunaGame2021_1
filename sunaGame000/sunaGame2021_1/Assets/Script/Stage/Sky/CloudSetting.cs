using UnityEngine;

public class CloudSetting : MonoBehaviour
{
    public Gradient emitter;
    public Material _material;
    void Start()
    {
        GetComponent<ParticleSystem>().Simulate(700);
        GetComponent<ParticleSystem>().Play(); 
    }
    void Update()
    { 
        var v = TimeData.TimePoint;
        var c = _material.GetColor("_Color");
        _material.SetColor("_Color", new Color(c.r, c.g, c.b, emitter.Evaluate(v).grayscale));
    }


#if UNITY_EDITOR
     
    void OnDrawGizmos() => Update();

#endif
}
