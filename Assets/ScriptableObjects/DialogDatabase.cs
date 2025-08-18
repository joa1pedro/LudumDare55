using System.Collections.Generic;
using UnityEngine;

public class DialogDatabase : ScriptableObject
{
    [Tooltip("List of enemies with unique IDs and prefabs")]
    public DialogEntry[] Dialogs;

    private Dictionary<string, DialogEntry> _lookup;

    public void Initialize()
    {
        _lookup = new Dictionary<string, DialogEntry>();
        foreach (var entry in Dialogs)
            _lookup[entry.Id] = entry;
    }

    public DialogEntry GetDialogEntry(string id)
    {
        if (_lookup == null)
            Initialize();

        if (_lookup.TryGetValue(id, out var entry))
            return entry;

        Debug.LogWarning($"Dialog ID '{id}' not found in database.");
        return null;
    }
}