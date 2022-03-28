using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public VelocityEstimator _velocityEstimator;
    public float _threshold = 25f;

    private void Update()
    {
        //print(_velocityEstimator.GetVelocityEstimate().sqrMagnitude);
        if (_velocityEstimator.GetVelocityEstimate().sqrMagnitude > _threshold)
        {
            gameObject.tag = "Sharp";
        }
        else
        {
            gameObject.tag = "Untagged";
        }
    }
}
