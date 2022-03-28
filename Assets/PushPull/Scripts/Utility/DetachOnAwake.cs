using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetachOnAwake : MonoBehaviour
{
    private void Awake()
    {
        transform.SetParent(null);
    }
}
