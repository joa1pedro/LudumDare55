using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkingDialog : MonoBehaviour
{
    [SerializeField] private Text talkingText;
    [SerializeField] private Text speakerNameText;
    [SerializeField] private Image image;
    [SerializeField] private float typingSpeed = 0.05f;

    [SerializeField] DialogEntry currentEntry;
    private Coroutine typingCoroutine;

    private void Start()
    {
        Initialize(currentEntry);
    }

    void Initialize(DialogEntry entry)
    {
        currentEntry = entry;
        image.sprite = entry.portrait;
        // talkingText.text = entry.text;
        speakerNameText.text = entry.speakerName;

        StartTyping();
    }

    public void StartTyping()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(currentEntry.text));
    }

    private IEnumerator TypeText(string message)
    {
        talkingText.text = "";
        foreach (char c in message)
        {
            talkingText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        typingCoroutine = null;
    }
    
    public void FastForward()
    {
        typingSpeed = 0.01f;
    }
    
    public void Skip()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            talkingText.text = currentEntry.text;
            typingCoroutine = null;
        }
    }
    
}