using UnityEngine;

[CreateAssetMenu(fileName = "DialogEntry", menuName = "Dialog System/Dialog Entry")]
public class DialogEntry : ScriptableObject
{
    public Sprite portrait;
    public string speakerName;
    [TextArea(2, 5)]
    public string text;
}