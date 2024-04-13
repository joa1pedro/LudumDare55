using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboPiece : MonoBehaviour
{
    public int Index;

    [SerializeField] Image currentSprite;
    private Sprite normalSprite;
    private Sprite pressedSprite;

    public bool Active = false;

    public ComboPiece(int index) 
    {
        this.Index = index;
        this.Active = false;
    }

    public void SetSprite(Sprite sprite)
    {
        normalSprite = sprite;
        currentSprite.sprite = normalSprite;
    }

    public void SetPressedSprite(Sprite sprite)
    {
        pressedSprite = sprite;
    }

    public void Activate(string comboName)
    {
        if (!Active)
        {
            Active = true;
            currentSprite.sprite = pressedSprite;
        }

    }

    public void MyComboEnded(string comboName)
    {
        Active = false;
        currentSprite.sprite = normalSprite;
    }
}
