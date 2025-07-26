using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.U2D;
using UnityEngine.UI;

public class ComboSequence : MonoBehaviour
{
    [SerializeField] List<ComboPiece> comboPieces = new List<ComboPiece>();
    [SerializeField] Star StarsEffect;

    [SerializeField] private ComboPiece comboPiecePrefab;
    [SerializeField] private Transform comboParent;

    
    public string Id = string.Empty;

    private bool comboStarted = false;
    private int currentComboindex = -1;
    
    public void Initialize(string comboName, SpriteAtlas normalKeyAtlas, SpriteAtlas pressedKeyAtlas)
    {
        Id = comboName;
        comboStarted = false;
        currentComboindex = -1;

        // Clear any existing pieces
        foreach (var piece in comboPieces)
        {
            if (piece != null)
                Destroy(piece.gameObject);
        }
        comboPieces.Clear();

        for (int i = 0; i < comboName.Length; i++)
        {
            ComboPiece newPiece = Instantiate(comboPiecePrefab, comboParent);
            var comboChar = comboName[i].ToString();
            var comboCharPressed = $"pressed_{comboChar}";
            newPiece.SetSprite(normalKeyAtlas.GetSprite(comboChar));
            newPiece.SetPressedSprite(pressedKeyAtlas.GetSprite(comboCharPressed));
            newPiece.Index = i;

            comboPieces.Add(newPiece);
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

    public void EndComboInstantly()
    {
        EndCombo();
    }
    
    public IEnumerator EndComboDelayed(float seconds)
    {
        if (seconds > 0f)
        {
            yield return new WaitForSeconds(seconds);
        }
        EndCombo();
    }

    private void EndCombo()
    {
        currentComboindex = -1;
        if (comboStarted)
        {
            comboStarted = false;
            //Debug.Log("Combo Ended" + Id);
        }
        foreach(ComboPiece piece in comboPieces)
        {
            piece.MyComboEnded(this.Id);
        }
    }

    public void PlaySuccessAnimation()
    {
        StarsEffect.PlayStarAnimation();
        this.ForwardCombo(this.Id.Length);
    }
}
