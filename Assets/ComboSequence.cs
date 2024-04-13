using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class ComboSequence : MonoBehaviour
{
    [SerializeField] List<ComboPiece> comboPieces = new List<ComboPiece>();

    public string Id = string.Empty;

    private bool comboStarted = false;
    private int currentComboindex = -1;


    public ComboSequence(string id)
    {
        Id = id;
        for (int i = 0; i < id.Length; i++) {
            ComboPiece newComboPiece = new ComboPiece(i);
            comboPieces.Add(newComboPiece);
        }

        this.comboStarted = false;
        this.currentComboindex = -1 ;
    }

    public void Initialize(string comboName, SpriteAtlas atlas)
    {
        Id = comboName;
        int i = 0;
        foreach (ComboPiece piece in comboPieces)
        {
            piece.SetSprite(atlas.GetSprite(comboName[i].ToString()));
            piece.Index = i;
            i++;
        }
    }

    public void ForwardCombo(int id)
    {
        currentComboindex = id-1;
        if (!comboPieces[currentComboindex].Active)
        {
            comboPieces[currentComboindex].Activate(this.Id);
        }
    }

    public void EndCombo()
    {
        currentComboindex = -1;
        if (comboStarted)
        {
            comboStarted = false;
            Debug.Log("Combo Ended" + Id);
        }
        foreach(ComboPiece piece in comboPieces)
        {
            piece.MyComboEnded(this.Id);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
