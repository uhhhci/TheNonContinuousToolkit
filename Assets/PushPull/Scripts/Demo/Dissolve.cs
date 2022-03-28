using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    private float _targetDissolve = -0.05f;
    Material[] _materials;
    private List<Shader> _dissolvingShaders;
    public float _dissolveDuration = 2.75f;
    private float _dissolveStartTime;


    private void Start()
    {
        //this skips the first animation
        _targetDissolve = 0f;
        _dissolveStartTime = -100f;

        _materials = GetComponent<Renderer>().materials;

        foreach (var mat in _materials)
        {
            mat.SetFloat("_Dissolve", 0);
        }

        Invoke("Disappear", 0.25f);
    }

    private void FixedUpdate()
    {
        float dissolveValue = 0f;
        if ((Time.time - _dissolveStartTime) < _dissolveDuration)
        {
            dissolveValue = ((Time.time - _dissolveStartTime) / _dissolveDuration);
            dissolveValue = Mathf.Clamp(dissolveValue, 0, 1);

            if (_targetDissolve == -0.05f)
            {
                dissolveValue = 1 - dissolveValue;
            }
        }
        else
        {
            dissolveValue = _targetDissolve;
        }
        foreach (var mat in _materials)
        {
            mat.SetFloat("_Dissolve", dissolveValue);
        }
    }

    public void Appear()
    {
        foreach (var mat in _materials)
        {
            mat.SetFloat("_Dissolve", 1);
        }
        _dissolveStartTime = Time.time;
        _targetDissolve = -0.05f;

    }

    public void Disappear()
    {
        foreach (var mat in _materials)
        {
            mat.SetFloat("_Dissolve", 0);
        }
        _dissolveStartTime = Time.time;
        _targetDissolve = 1;
    }
}