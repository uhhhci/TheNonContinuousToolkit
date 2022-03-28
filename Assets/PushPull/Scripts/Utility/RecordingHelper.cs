using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordingHelper : MonoBehaviour
{
    public GameObject _target;

    public void FlashObject()
    {
        ActivateTarget();
        Invoke("DeactivateTarget", 0.05f);
    }

    private void ActivateTarget()
    {
        _target.transform.position = new Vector3(0, 1000, 0);
    }

    private void DeactivateTarget()
    {
        _target.transform.position = new Vector3(0, 0, 0);
    }
}
