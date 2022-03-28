using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoActivator : MonoBehaviour
{
    public GameObject _linkedObject;
    public bool _inverse;

    private void OnEnable()
    {
        if (_linkedObject)
            _linkedObject.SetActive(_inverse ? false : true);
    }

    private void OnDisable()
    {
        if (_linkedObject)
            _linkedObject.SetActive(_inverse ? true : false);
    }
}
