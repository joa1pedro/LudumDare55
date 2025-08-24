using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TalkingDialog : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Text _talkingText;
    [SerializeField] private Text _speakerNameText;
    [SerializeField] private Image _image;
    [SerializeField] private float _typingSpeed = 0.05f;

    [SerializeField] DialogEntry _currentEntry;
    private Coroutine _typingCoroutine;
    public Action OnDialogFinished;

    public void Setup(DialogEntry entry)
    {
        _currentEntry = entry;
        _image.sprite = entry.Portrait;
        _speakerNameText.text = entry.SpeakerName;

        StartTyping();
    }

    public void StartTyping()
    {
        if (_typingCoroutine != null)
            StopCoroutine(_typingCoroutine);

        this.gameObject.SetActive(true);
        if (gameObject.activeSelf)
            _typingCoroutine = StartCoroutine(TypeText(_currentEntry.Text));
    }

    private IEnumerator TypeText(string message)
    {
        _talkingText.text = "";
        foreach (char c in message)
        {
            _talkingText.text += c;
            yield return new WaitForSeconds(_typingSpeed);
        }

        _typingCoroutine = null;
    }
    
    public void FastForward()
    {
        _typingSpeed = 0.01f;
    }
    
    public void Skip()
    {
        if (_typingCoroutine != null)
        {
            StopCoroutine(_typingCoroutine);
            _talkingText.text = _currentEntry.Text;
            _typingCoroutine = null;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_typingCoroutine != null)
            StopCoroutine(_typingCoroutine);
        
        _talkingText.text = "";
        this.gameObject.SetActive(false);
        OnDialogFinished.Invoke();
    }
}