using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform _target;
    public bool _lockRotation;

    // Update is called once per frame
    void Update()
    {
        transform.position = _target.position;
        if (_lockRotation)
        {
            transform.rotation = Quaternion.identity;
        }
        else
        {
            transform.rotation = _target.rotation;
        }
    }
}
