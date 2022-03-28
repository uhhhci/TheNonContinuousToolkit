using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PushPull/Game Event")]
public class GameEvent : ScriptableObject
{
    private List<GameEventListener> linsteners = new List<GameEventListener>();

    public void Raise()
    {
        for (int i = linsteners.Count - 1; i >= 0; i--)
        {
            linsteners[i].OnEventRaised();
        }
    }

    public void RegisterListener(GameEventListener listener)
    {
        linsteners.Add(listener);
    }

    public void UnregisterListener(GameEventListener listener)
    {
        linsteners.Remove(listener);
    }
}
