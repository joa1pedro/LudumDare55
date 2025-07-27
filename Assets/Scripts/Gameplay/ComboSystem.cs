
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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

    private PlayerInput playerInput;

    private List<ComboSequence> comboSequences = new();
    private Dictionary<string, InputAction> keyActions = new();
    private Dictionary<string, Action<InputAction.CallbackContext>> keyCallbacks = new();

    private string currentTypedSequence = "";
    private int currentIndex = 0;

    private void Awake()
    {
        playerInput = new PlayerInput();
        playerInput.Keyboard.Enable();
        BindKey("A", playerInput.Keyboard.A);
        BindKey("B", playerInput.Keyboard.B);
        BindKey("C", playerInput.Keyboard.C);
        BindKey("D", playerInput.Keyboard.D);
        BindKey("E", playerInput.Keyboard.E);
        BindKey("F", playerInput.Keyboard.F);
        BindKey("G", playerInput.Keyboard.G);
        BindKey("H", playerInput.Keyboard.H);
        BindKey("I", playerInput.Keyboard.I);
        BindKey("J", playerInput.Keyboard.J);
        BindKey("K", playerInput.Keyboard.K);
        BindKey("L", playerInput.Keyboard.L);
        BindKey("M", playerInput.Keyboard.M);
        BindKey("N", playerInput.Keyboard.N);
        BindKey("O", playerInput.Keyboard.O);
        BindKey("P", playerInput.Keyboard.P);
        BindKey("Q", playerInput.Keyboard.Q);
        BindKey("R", playerInput.Keyboard.R);
        BindKey("S", playerInput.Keyboard.S);
        BindKey("T", playerInput.Keyboard.T);
        BindKey("U", playerInput.Keyboard.U);
        BindKey("V", playerInput.Keyboard.V);
        BindKey("W", playerInput.Keyboard.W);
        BindKey("X", playerInput.Keyboard.X);
        BindKey("Y", playerInput.Keyboard.Y);
        BindKey("Z", playerInput.Keyboard.Z);
    }
    private void BindKey(string keyLabel, InputAction action)
    {
        keyActions[keyLabel] = action;

        Action<InputAction.CallbackContext> callback = ctx => OnKeyPressed(ctx, keyLabel);
        keyCallbacks[keyLabel] = callback;

        action.performed += callback;
    }

    private void Start()
    {
        //Initialize the Sequences
        ComboSequence[] combos = comboSequencesHolder.GetComponentsInChildren<ComboSequence>();
        comboSequences.AddRange(combos);
        
        foreach (ComboSequence sequence in comboSequences)
        {           
            sequence.Initialize(sequence.Id, normalKeyAtlas, pressedKeyAtlas);
        }
    }
    
    private void OnEnable()
    {
        foreach (var action in keyActions.Values)
            action.Enable();
    }

    private void OnDisable()
    {
        foreach (var action in keyActions.Values)
            action.Disable();
    }

    private void OnDestroy()
    {
        foreach (var kvp in keyActions)
        {
            if (keyCallbacks.TryGetValue(kvp.Key, out var callback))
            {
                kvp.Value.performed -= callback;
            }
        }
    }
    
    private void OnKeyPressed(InputAction.CallbackContext context, string key)
    {
        currentTypedSequence += key;
        CheckCombo();
    }

    private void CheckCombo()
    {
        if (string.IsNullOrEmpty(currentTypedSequence))
            return;

        // Check for full match
        for (int i = 0; i < comboSequences.Count; i++)
        {
            var combo = comboSequences[i];
            if (currentTypedSequence == combo.Id)
            {
                ExecuteCombo(combo, i);
                currentTypedSequence = "";
                currentIndex = 0;
                return;
            }
        }

        // Check for valid prefix of any combo
        bool isPartialMatch = false;
        foreach (var combo in comboSequences)
        {
            if (combo.Id.StartsWith(currentTypedSequence))
            {
                isPartialMatch = true;
                currentIndex = currentTypedSequence.Length;
                combo.ForwardCombo(currentIndex);
                break;
            }
        }

        if (!isPartialMatch)
        {
            // No combo matched and not a valid prefix — reset and fail
            currentTypedSequence = "";
            currentIndex = 0;
            FailCombos();
        }
    }
    
    private void FailCombos()
    {
        audioManager.PlaySoundFailCombo();
        foreach (ComboSequence comboSequence in comboSequences)
        {
            comboSequence.EndComboInstantly();
        }
        canvasToShake.ShakeCanvas();
    }

    private void ExecuteCombo(ComboSequence comboSequence, int comboIndex)
    {
        // Implement the effect of the combo
        comboSequence.PlaySuccessAnimation();
        StartCoroutine(comboSequence.EndComboDelayed(0.1f));

        summoningController.PerformSummon(comboIndex);
        audioManager.PlaySoundExecuteCombo();
    }
    
}