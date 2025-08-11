
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
    
    [SerializeField] GameObject _enabledIndicator;
    

    private PlayerInput _playerInput;

    private Dictionary<string, InputAction> _keyActions = new();
    private Dictionary<string, Action<InputAction.CallbackContext>> _keyCallbacks = new();

    private string _currentTypedSequence = "";
    private int _currentTypingIndex = 0;
    
    protected int CurrentTypingContext;
    protected bool Active = true;

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _playerInput.Keyboard.Enable();
        BindKey("A", _playerInput.Keyboard.A);
        BindKey("B", _playerInput.Keyboard.B);
        BindKey("C", _playerInput.Keyboard.C);
        BindKey("D", _playerInput.Keyboard.D);
        BindKey("E", _playerInput.Keyboard.E);
        BindKey("F", _playerInput.Keyboard.F);
        BindKey("G", _playerInput.Keyboard.G);
        BindKey("H", _playerInput.Keyboard.H);
        BindKey("I", _playerInput.Keyboard.I);
        BindKey("J", _playerInput.Keyboard.J);
        BindKey("K", _playerInput.Keyboard.K);
        BindKey("L", _playerInput.Keyboard.L);
        BindKey("M", _playerInput.Keyboard.M);
        BindKey("N", _playerInput.Keyboard.N);
        BindKey("O", _playerInput.Keyboard.O);
        BindKey("P", _playerInput.Keyboard.P);
        BindKey("Q", _playerInput.Keyboard.Q);
        BindKey("R", _playerInput.Keyboard.R);
        BindKey("S", _playerInput.Keyboard.S);
        BindKey("T", _playerInput.Keyboard.T);
        BindKey("U", _playerInput.Keyboard.U);
        BindKey("V", _playerInput.Keyboard.V);
        BindKey("W", _playerInput.Keyboard.W);
        BindKey("X", _playerInput.Keyboard.X);
        BindKey("Y", _playerInput.Keyboard.Y);
        BindKey("Z", _playerInput.Keyboard.Z);

        _playerInput.Keyboard.Enter.performed += SwitchFailed;
        _playerInput.Keyboard.Backspace.performed += FailInstant;
    }

    private void BindKey(string keyLabel, InputAction action)
    {
        _keyActions[keyLabel] = action;
        Action<InputAction.CallbackContext> callback = ctx => OnKeyPressed(ctx, keyLabel);
        _keyCallbacks[keyLabel] = callback;
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
        foreach (var action in _keyActions.Values)
            action.Enable();
    }

    private void OnDisable()
    {
        foreach (var action in _keyActions.Values)
            action.Disable();
    }

    private void OnDestroy()
    {
        foreach (var kvp in _keyActions)
        {
            if (_keyCallbacks.TryGetValue(kvp.Key, out var callback))
                kvp.Value.performed -= callback;
        }
    }

    protected void Enable(bool active)
    {
        if (!active)
            FailAllSequences();
        Active = active;
        _enabledIndicator.SetActive(Active);
    }
        
    private void OnKeyPressed(InputAction.CallbackContext context, string key)
    {
        if (!Active) return;
        _currentTypedSequence += key;
        CheckCombo();
    }

    private void CheckCombo()
    {
        if (string.IsNullOrEmpty(_currentTypedSequence))
            return;

        // Check for full match
        for (int i = 0; i < comboSequences.Count; i++)
        {
            var comboSequence = comboSequences[i];
            
            if (_currentTypedSequence == comboSequence.Sequence 
                && comboSequence.Context == CurrentTypingContext)
            {
                ExecuteCombo(comboSequence, i);
                _currentTypedSequence = "";
                _currentTypingIndex = 0;
                return;
            }
        }

        // Check for valid prefix of any combo
        foreach (var comboSequence in comboSequences)
        {
            if (comboSequence.Sequence.StartsWith(_currentTypedSequence) 
                && comboSequence.Context == CurrentTypingContext)
            {
                _currentTypingIndex = _currentTypedSequence.Length;
                comboSequence.ForwardCombo(_currentTypingIndex);
                return;
            }
        }

        FailInstant();
    }
    
    protected void FailInstant()
    {
        _currentTypedSequence = "";
        _currentTypingIndex = 0;
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
        if (!canvasToShake[CurrentTypingContext]) return;
        
        canvasToShake[CurrentTypingContext].ShakeCanvas();
    }
    
    protected void FailInstant(InputAction.CallbackContext callbackContext)
    {
        FailInstant();
    }
    
    private void SwitchFailed(InputAction.CallbackContext callbackContext)
    {
        Enable(!Active);
    }
}