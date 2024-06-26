
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class ComboSystem : MonoBehaviour
{
    [Header("Atlases")]
    [SerializeField] SpriteAtlas normalKeyAtlas = default; 
    [SerializeField] SpriteAtlas pressedKeyAtlas = default;

    [Header("Canvas Shake Reference")]
    [SerializeField] CanvasShaker canvasToShake;

    [Header("SummonerReference")]
    [SerializeField] Summoner summoner;

    [Header("Summons Controller Reference")]
    [SerializeField] SummoningController summoningController;

    [Header("Audio Manager Reference")]
    [SerializeField] AudioManager audioManager;

    [Header("Combo Related Stuff")]
    [SerializeField] GameObject comboSequencesHolder;


    private List<ComboSequence> comboSequences = new List<ComboSequence> ();

    // Privates
    private string currentTypedSequence = "";
    private float lastInputTime = 0f;
    private float comboTimeout = 5f; // Time player has to hit the next key in sequence
    private int currentIndex = 0;

    private void Start()
    {
        int i = 0;

        //Initialize the Sequences
        ComboSequence[] combos = comboSequencesHolder.GetComponentsInChildren<ComboSequence>();
        comboSequences.AddRange(combos);

        foreach (ComboSequence sequence in comboSequences)
        {           
            sequence.Initialize(sequence.Id, normalKeyAtlas, pressedKeyAtlas);
            i++;
        }
    }

    void Update()
    {
        if (summoner.GameEnded) return;
        if (Time.time - lastInputTime > comboTimeout)
        {
            currentTypedSequence = ""; // Reset the sequence if too much time has passed
        }

        // Detect each key of the combo
        if (Input.GetKeyDown(KeyCode.A)) AddKeyToSequence("A");
        if (Input.GetKeyDown(KeyCode.B)) AddKeyToSequence("B");
        if (Input.GetKeyDown(KeyCode.C)) AddKeyToSequence("C");
        if (Input.GetKeyDown(KeyCode.D)) AddKeyToSequence("D");
        if (Input.GetKeyDown(KeyCode.E)) AddKeyToSequence("E");
        if (Input.GetKeyDown(KeyCode.F)) AddKeyToSequence("F");
        if (Input.GetKeyDown(KeyCode.G)) AddKeyToSequence("G");
        if (Input.GetKeyDown(KeyCode.H)) AddKeyToSequence("H");
        if (Input.GetKeyDown(KeyCode.I)) AddKeyToSequence("I");
        if (Input.GetKeyDown(KeyCode.J)) AddKeyToSequence("J");
        if (Input.GetKeyDown(KeyCode.K)) AddKeyToSequence("K");
        if (Input.GetKeyDown(KeyCode.L)) AddKeyToSequence("L");
        if (Input.GetKeyDown(KeyCode.M)) AddKeyToSequence("M");
        if (Input.GetKeyDown(KeyCode.N)) AddKeyToSequence("N");
        if (Input.GetKeyDown(KeyCode.O)) AddKeyToSequence("O");
        if (Input.GetKeyDown(KeyCode.P)) AddKeyToSequence("P");
        if (Input.GetKeyDown(KeyCode.Q)) AddKeyToSequence("Q");
        if (Input.GetKeyDown(KeyCode.R)) AddKeyToSequence("R");
        if (Input.GetKeyDown(KeyCode.S)) AddKeyToSequence("S");
        if (Input.GetKeyDown(KeyCode.T)) AddKeyToSequence("T");
        if (Input.GetKeyDown(KeyCode.U)) AddKeyToSequence("U");
        if (Input.GetKeyDown(KeyCode.V)) AddKeyToSequence("V");
        if (Input.GetKeyDown(KeyCode.W)) AddKeyToSequence("W");
        if (Input.GetKeyDown(KeyCode.X)) AddKeyToSequence("X");
        if (Input.GetKeyDown(KeyCode.Y)) AddKeyToSequence("Y");
        if (Input.GetKeyDown(KeyCode.Z)) AddKeyToSequence("Z");

        if (Input.GetKeyDown(KeyCode.Alpha0)) AddKeyToSequence("0");
        if (Input.GetKeyDown(KeyCode.Alpha1)) AddKeyToSequence("1");
        if (Input.GetKeyDown(KeyCode.Alpha2)) AddKeyToSequence("2");
        if (Input.GetKeyDown(KeyCode.Alpha3)) AddKeyToSequence("3");
        if (Input.GetKeyDown(KeyCode.Alpha4)) AddKeyToSequence("4");
        if (Input.GetKeyDown(KeyCode.Alpha5)) AddKeyToSequence("5");
        if (Input.GetKeyDown(KeyCode.Alpha6)) AddKeyToSequence("6");
        if (Input.GetKeyDown(KeyCode.Alpha7)) AddKeyToSequence("7");
        if (Input.GetKeyDown(KeyCode.Alpha8)) AddKeyToSequence("8");
        if (Input.GetKeyDown(KeyCode.Alpha9)) AddKeyToSequence("9");

        if (Input.GetKeyDown(KeyCode.Space)) AddKeyToSequence("Space");

        if (Input.GetKeyDown(KeyCode.LeftArrow)) AddKeyToSequence("LeftArrow");
        if (Input.GetKeyDown(KeyCode.RightArrow)) AddKeyToSequence("RightArrow");
        if (Input.GetKeyDown(KeyCode.UpArrow)) AddKeyToSequence("UpArrow");
        if (Input.GetKeyDown(KeyCode.DownArrow)) AddKeyToSequence("DownArrow");

        if (Input.GetKeyDown(KeyCode.F1)) AddKeyToSequence("F1");
        if (Input.GetKeyDown(KeyCode.F2)) AddKeyToSequence("F2");
        if (Input.GetKeyDown(KeyCode.F3)) AddKeyToSequence("F3");
        if (Input.GetKeyDown(KeyCode.F4)) AddKeyToSequence("F4");
        if (Input.GetKeyDown(KeyCode.F5)) AddKeyToSequence("F5");
        if (Input.GetKeyDown(KeyCode.F6)) AddKeyToSequence("F6");
        if (Input.GetKeyDown(KeyCode.F7)) AddKeyToSequence("F7");
        if (Input.GetKeyDown(KeyCode.F8)) AddKeyToSequence("F8");
        if (Input.GetKeyDown(KeyCode.F9)) AddKeyToSequence("F9");
        if (Input.GetKeyDown(KeyCode.F10)) AddKeyToSequence("F10");
        if (Input.GetKeyDown(KeyCode.F11)) AddKeyToSequence("F11");
        if (Input.GetKeyDown(KeyCode.F12)) AddKeyToSequence("F12");

        CheckCombo();
    }

    private void AddKeyToSequence(string key)
    {
        currentTypedSequence += key;
        lastInputTime = Time.time;
    }

    private void CheckCombo()
    {
        //Early exit if any key hasn't been hit yet
        if (string.IsNullOrEmpty(currentTypedSequence)) return;
        int comboIndex = 0;
        foreach (ComboSequence comboSequence in comboSequences)
        {
            if (currentTypedSequence == comboSequence.Id)
            {
                // Combo matched, reset sequence
                currentTypedSequence = "";
                currentIndex = 0;
                ExecuteCombo(comboSequence, comboIndex);
                return;
            }
            comboIndex++;
        }
        comboIndex = 0;

        // If no combo matched, check if the current sequence is part of any combo
        foreach (ComboSequence comboSequence in comboSequences)
        {
            if (!string.IsNullOrEmpty(currentTypedSequence) && comboSequence.Id.StartsWith(currentTypedSequence))
            {
                // Current sequence is part of a combo, increment index
                currentIndex = currentTypedSequence.Length;
                comboSequence.ForwardCombo(currentIndex);
                return;
            }
        }

        // If no combo or partial combo matched, reset sequence
        currentTypedSequence = "";
        currentIndex = 0;

        FailCombos();
    }

    private void FailCombos()
    {
        audioManager.PlaySoundFailCombo();
        foreach (ComboSequence comboSequence in comboSequences)
        {
            FailCombo(comboSequence);
        }
    }

    private void ExecuteCombo(ComboSequence comboSequence, int comboIndex)
    {
        // Implement the effect of the combo
        comboSequence.PlaySuccessAnimation();
        comboSequence.EndCombo();

        summoningController.PerformSummon(comboIndex);
        audioManager.PlaySoundExecuteCombo();
    }

    public void FailCombo(ComboSequence comboSequence)
    {
        comboSequence.EndCombo();
        //TODO play audio
        canvasToShake.ShakeCanvas();
    }
}