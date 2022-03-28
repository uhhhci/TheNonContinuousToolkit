using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalSpaceImitator : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _rotationOffset;

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = _target.localPosition;
        transform.localRotation = _target.localRotation;
        transform.localRotation *= Quaternion.Euler(_rotationOffset);
    }
}
