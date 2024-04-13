using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboPiece : MonoBehaviour
{
    public int Index;

    [SerializeField] Image sprite;

    public bool Active = false;

    public ComboPiece(int index) 
    {
        this.Index = index;
        this.Active = false;
    }

    public void SetSprite(Sprite image)
    {
        sprite.sprite = image;
    }

    public void Activate(string comboName)
    {
        if (!Active)
        {
            Active = true;
            sprite.color = Color.green;
        }

    }

    public void MyComboEnded(string comboName)
    {
        Active = false;
        sprite.color = Color.white;
    }
}
