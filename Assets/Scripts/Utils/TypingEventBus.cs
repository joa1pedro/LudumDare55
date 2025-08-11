using System;
using System.Collections.Generic;

public static class TypingEventBus
{
    private static readonly Dictionary<string, Action<object[]>> Events 
        = new Dictionary<string, Action<object[]>>();

    public static void Subscribe(string command, Action<object[]> callback)
    {
        if (!Events.ContainsKey(command))
            Events[command] = delegate { };

        Events[command] += callback;
    }

    public static void Unsubscribe(string command, Action<object[]> callback)
    {
        if (Events.ContainsKey(command))
            Events[command] -= callback;
    }

    public static void Publish(string command, params object[] args)
    {
        if (Events.TryGetValue(command, out var @event))
            @event?.Invoke(args);
    }
}