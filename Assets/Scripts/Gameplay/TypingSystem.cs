
using System;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D;

public class TypingSystem : MonoBehaviour
{
    [Header("Atlases")]
    [SerializeField] SpriteAtlas normalKeyAtlas = default; 
    [SerializeField] SpriteAtlas pressedKeyAtlas = default;

    [Header("Canvas Shake References")]
    [SerializeField]
    protected List<CanvasShaker> canvasToShake;
    
    [Header("Sequences")]
    [SerializeField] protected List<ComboSequence> comboSequences = new();

    private PlayerInput playerInput;

    private Dictionary<string, InputAction> keyActions = new();
    private Dictionary<string, Action<InputAction.CallbackContext>> keyCallbacks = new();

    private string currentTypedSequence = "";
    private int currentTypingIndex = 0;
    protected int CurrentTypingContext;
    
    protected bool Active = true;

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

        playerInput.Keyboard.Backspace.performed += FailInstant;
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
        ValidateSequences(comboSequences);
        foreach (ComboSequence sequence in comboSequences)
        {           
            sequence.Initialize(sequence.Sequence, normalKeyAtlas, pressedKeyAtlas);
        }
    
        // Initialize the context
        CurrentTypingContext = 0;
        Initialize();
    }


    private void ValidateSequences(List<ComboSequence> sequences)
    {
        //TODO validation
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
                kvp.Value.performed -= callback;
        }
    }

    protected void Enable(bool active)
    {
        if (!active)
            FailAllSequences();
        Active = active;
    }
        
    private void OnKeyPressed(InputAction.CallbackContext context, string key)
    {
        if (!Active) return;
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
            var comboSequence = comboSequences[i];
            
            if (currentTypedSequence == comboSequence.Sequence 
                && comboSequence.Context == CurrentTypingContext)
            {
                ExecuteCombo(comboSequence, i);
                currentTypedSequence = "";
                currentTypingIndex = 0;
                return;
            }
        }

        // Check for valid prefix of any combo
        foreach (var comboSequence in comboSequences)
        {
            if (comboSequence.Sequence.StartsWith(currentTypedSequence) 
                && comboSequence.Context == CurrentTypingContext)
            {
                currentTypingIndex = currentTypedSequence.Length;
                comboSequence.ForwardCombo(currentTypingIndex);
                break;
            }
        }

        FailInstant();
    }
    
    protected void FailInstant()
    {
        currentTypedSequence = "";
        currentTypingIndex = 0;
        FailAllSequences();
    }
    

    protected virtual void Initialize()
    {
    }

    protected virtual void ExecuteCombo(ComboSequence comboSequence, int comboIndex)
    {
    }

    protected virtual void FailAllSequences()
    {
        // Shake the context canvas
        canvasToShake[CurrentTypingContext].ShakeCanvas();
    }
    
    protected void FailInstant(InputAction.CallbackContext callbackContext)
    {
        FailInstant();
    }
}