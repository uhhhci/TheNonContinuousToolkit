using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sliceable : MonoBehaviour
{
    private SlicingManager _slicingManager;

    // Start is called before the first frame update
    void Start()
    {
        _slicingManager = SlicingManager._instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Sharp")
        {
            _slicingManager.Slice(this.gameObject, other.transform);
        }
    }

    public void Slice(Transform slicingPlane)
    {
        _slicingManager.Slice(this.gameObject, slicingPlane, Vector3.zero, new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));
    }
}
