using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventHitZoneTrigger : MonoBehaviour
{
    [SerializeField] private GameEvent _eventLeft;
    [SerializeField] private GameEvent _eventRight;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "HitZoneTriggerLeft")
        {
            _eventLeft.Raise();
        }
        if (other.tag == "HitZoneTriggerRight")
        {
            _eventRight.Raise();
        }
    }
}
