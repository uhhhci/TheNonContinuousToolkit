using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventTrigger : MonoBehaviour
{
    [SerializeField] private GameEvent _event;
    private bool _triggered = false;
    public float delay = 0.05f;

    private void Update()
    {
        if (transform.position.y > 100 && !_triggered)
        {
            Invoke("RaiseEvent", delay);
            _triggered = true;
        }
        if (transform.position.y < 10)
        {
            _triggered = false;
        }
    }

    private void RaiseEvent()
    {
        _event.Raise();
    }
}
