using UnityEngine;

[CreateAssetMenu(fileName = "DialogEntry", menuName = "Dialog System/Dialog Entry")]
public class DialogEntry : ScriptableObject
{
    public string Id;
    public Sprite Portrait;
    public string SpeakerName;
    [TextArea(2, 5)]
    public string Text;
}