using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    public GameEvent Event;
    public UnityEvent Response;
    public float _delay = 0f;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised()
    {
        if (_delay != 0f)
        {
            Invoke("InvokeResponse", _delay);
        }
        else
        {
            Response.Invoke();
        }
    }

    private void InvokeResponse()
    {
        Response.Invoke();
    }
}
