using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorCamera : MonoBehaviour
{
    private Transform _vrCam;
    public Transform _mirrorCam;

    // Start is called before the first frame update
    void Start()
    {
        _vrCam = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        _mirrorCam.position = _vrCam.position;
        Vector3 pos = _mirrorCam.localPosition;
        pos.z *= -1;
        _mirrorCam.localPosition = pos;
        _mirrorCam.rotation = Quaternion.LookRotation(Vector3.Reflect(_vrCam.forward, transform.forward), Vector3.up);
    }
}
