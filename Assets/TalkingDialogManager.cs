using System;
using UnityEngine;

public class TalkingDialogManager : MonoBehaviour
{
    [SerializeField] TalkingDialog _talkingDialog;
    [SerializeField] DialogDatabase _dialogDatabase;

    public bool IsAnyDialogActive = false;
    void Start()
    {
        _talkingDialog.OnDialogFinished += FinishDialog;
        _dialogDatabase.Initialize();
        foreach (var dialogEntry in _dialogDatabase.Dialogs)
        {
            DialogManagerEventBus.Subscribe(dialogEntry.Id, ExecuteDialog);
        }
    }

    private void OnDisable()
    {
        _talkingDialog.OnDialogFinished -= FinishDialog;
    }

    void ExecuteDialog(string id, bool unsubscribe)
    {
        if (IsAnyDialogActive)
        {
            Debug.Log("Already active dialog");
            return;
        }
        
        var dialog = _dialogDatabase.GetDialogEntry(id);
        if (dialog == null)
        {
            Debug.LogError("No dialog found for id " + id);
        }
        
        _talkingDialog.Setup(dialog);
        _talkingDialog.gameObject.SetActive(true);
        IsAnyDialogActive = true;
        if (unsubscribe)
        {
            DialogManagerEventBus.Unsubscribe(id, ExecuteDialog);
        }
    }
    
    private void FinishDialog()
    {
        IsAnyDialogActive = false;
    }
}
