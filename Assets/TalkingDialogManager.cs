using UnityEngine;

public class TalkingDialogManager : MonoBehaviour
{
    [SerializeField] TalkingDialog _talkingDialog;
    [SerializeField] DialogDatabase _dialogDatabase;
    void Start()
    {
        _dialogDatabase.Initialize();
        foreach (var dialogEntry in _dialogDatabase.Dialogs)
        {
            DialogManagerEventBus.Subscribe(dialogEntry.Id, ExecuteDialog);
        }
    }

    void ExecuteDialog(string id, bool unsubscribe)
    {
        var dialog = _dialogDatabase.GetDialogEntry(id);
        if (dialog == null)
        {
            Debug.LogError("No dialog found for id " + id);
        }
        _talkingDialog.Setup(dialog);
        _talkingDialog.gameObject.SetActive(true);
        if (unsubscribe)
        {
            DialogManagerEventBus.Unsubscribe(id, ExecuteDialog);
        }
    }
        
}
