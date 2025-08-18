using System;
using System.Collections.Generic;
using UnityEngine;

public static class DialogManagerEventBus
{
    private static readonly Dictionary<string, Action<string, bool>> Events = new();

    public static void Subscribe(string command, Action<string, bool> callback)
    {
        if (!Events.ContainsKey(command))
        {
            Events[command] = delegate { };
        }
        else
        {
            Debug.LogWarning("Trying to re-add dialog key for " + command);
        }

        Events[command] += callback;
    }

    public static void Unsubscribe(string command, Action<string, bool> callback)
    {
        if (Events.ContainsKey(command))
            Events[command] -= callback;
    }

    public static void Publish(string command, string id, bool unsub)
    {
        if (Events.TryGetValue(command, out var @event))
            @event?.Invoke(id, unsub);
    }
}