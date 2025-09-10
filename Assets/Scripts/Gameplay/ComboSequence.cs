using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class ComboSequence : MonoBehaviour
{
    [SerializeField] protected List<ComboPiece> comboPieces = new List<ComboPiece>();
    [SerializeField] protected ComboPiece comboPiecePrefab;
    [SerializeField] protected Transform comboParent;
    
    // Logic
    [SerializeField] public int Context;
    [SerializeField] public string Sequence = string.Empty;
    [SerializeField] public string CallbackKey = string.Empty;

    private int CurrentComboindex = -1;
    
    public void Initialize(string comboName, SpriteAtlas normalKeyAtlas, SpriteAtlas pressedKeyAtlas)
    {
        Sequence = comboName;
        CurrentComboindex = -1;

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
        CurrentComboindex = id-1;
        if (!comboPieces[CurrentComboindex].Active)
        {
            comboPieces[CurrentComboindex].Activate();
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
        CurrentComboindex = -1;
        foreach(ComboPiece piece in comboPieces)
        {
            piece.Deactivate();
        }
    }

    public virtual void PlaySuccessAnimation()
    {
        ForwardCombo(Sequence.Length);
    }
}
