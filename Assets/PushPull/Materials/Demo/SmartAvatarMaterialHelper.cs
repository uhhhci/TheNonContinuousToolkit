using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteAlways]
public class SmartAvatarMaterialHelper : MonoBehaviour
{
    public Transform _origin;
    private Renderer _ren;
    private SkinnedMeshRenderer _skinnedRen;
    public Vector3 _offset = new Vector3(0, -0.5f, 0);

    // Start is called before the first frame update
    void Start()
    {
        _ren = GetComponent<Renderer>();
        _skinnedRen = GetComponent<SkinnedMeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_origin)
        {
            if (_ren) _ren.sharedMaterial.SetVector("_originPosition", _origin.position + _offset);
            if (_skinnedRen) _skinnedRen.material.SetVector("_originPosition", _origin.position + _offset);
        }
        else
        {
            if (_ren) _ren.sharedMaterial.SetVector("_originPosition", Vector3.zero);
            if (_skinnedRen) _skinnedRen.material.SetVector("_originPosition", Vector3.zero);
        }
    }
}
