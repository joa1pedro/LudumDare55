
using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D;

public class TypingSystem : MonoBehaviour
{
    [Header("Atlases")]
    [SerializeField] SpriteAtlas _normalKeyAtlas = default; 
    [SerializeField] SpriteAtlas _pressedKeyAtlas = default;

    [Header("Canvas Shake References")]
    [SerializeField]
    protected List<CanvasShaker> _canvasToShake;
    
    [Header("Sequences")]
    [SerializeField] protected List<ComboSequence> _comboSequences = new();
    
    [SerializeField] GameObject _enabledIndicator;
    
    [Header("Script Options")]
    [SerializeField] bool _useEnterForSubmit;
    [Tooltip("Turn this on/off for starting the system On/Off")]
    [SerializeField] protected bool Active = false;

    private PlayerInput _playerInput;

    private Dictionary<string, InputAction> _keyActions = new();
    private Dictionary<string, Action<InputAction.CallbackContext>> _keyCallbacks = new();

    private string _currentTypedSequence = "";
    private int _currentTypingIndex = 0;
    
    public int CurrentTypingContext;
    private bool _cantEnable;

    private void Awake()
    {
        _enabledIndicator?.SetActive(Active);
        
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
        _playerInput.Keyboard.Backspace.performed += FailInstant;
        _playerInput.Keyboard.Escape.performed += EscapePressed;
        
        if (_useEnterForSubmit)
            _playerInput.Keyboard.Enter.performed += EnableOrSubmit;
        else
            _playerInput.Keyboard.Enter.performed += SwitchEnabled;
    }

    private void Start()
    {
        foreach (ComboSequence sequence in _comboSequences)
        {           
            sequence.Initialize(sequence.Sequence, _normalKeyAtlas, _pressedKeyAtlas);
        }
    
        // Initialize the context
        CurrentTypingContext = 0;
        Initialize();
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
    
    private void BindKey(string keyLabel, InputAction action)
    {
        _keyActions[keyLabel] = action;
        Action<InputAction.CallbackContext> callback = ctx => OnKeyPressed(ctx, keyLabel);
        _keyCallbacks[keyLabel] = callback;
        action.performed += callback;
    }

    /// <summary>
    /// Checks for a full typed sequence on each key callback and submits if matched
    /// </summary>
    private void CheckCombo()
    {
        if (string.IsNullOrEmpty(_currentTypedSequence))
            return;

        if (CheckForFullMatch()) 
            return;

        if (CheckForwardForSingleKey()) 
            return;

        FailInstant();
    }

    /// <summary>
    /// Checks the current typed sequence and submits it if a match
    /// </summary>
    /// <returns>Returns true if any combo has been matched and executed</returns>
    private bool CheckForFullMatch()
    {
        // Check for full match
        for (int i = 0; i < _comboSequences.Count; i++)
        {
            var comboSequence = _comboSequences[i];
            
            if (_currentTypedSequence == comboSequence.Sequence 
                && comboSequence.Context == CurrentTypingContext)
            {
                ExecuteCombo(comboSequence, i);
                _currentTypedSequence = "";
                _currentTypingIndex = 0;
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Checks for the last typed key and forwards it in all sequences if match
    /// </summary>
    /// <returns>Returns true if any sequence has been forwarded</returns>
    private bool CheckForwardForSingleKey()
    {
        // Check for valid prefix of any combo
        foreach (var comboSequence in _comboSequences)
        {
            if (comboSequence.Sequence.StartsWith(_currentTypedSequence) 
                && comboSequence.Context == CurrentTypingContext)
            {
                _currentTypingIndex = _currentTypedSequence.Length;
                comboSequence.ForwardCombo(_currentTypingIndex);
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Fails instantly all Sequences and refreshes to empty the current one 
    /// </summary>
    private void FailInstant()
    {
        _currentTypedSequence = "";
        _currentTypingIndex = 0;
        FailAllSequences();
    }

    /// <summary>
    /// Initialization method for Children of TypingSystem. Runs after Sequence Initialization
    /// </summary>
    protected virtual void Initialize()
    {
    }

    /// <summary>
    /// Enabled/Disables the whole System
    /// </summary>
    /// <param name="active"></param>
    /// <param name="cantEnable"></param>
    public void SetActive(bool active, bool cantEnable = false)
    {
        if (!active)
            FailAllSequences();
        Active = active;
        _enabledIndicator?.SetActive(Active);
        _cantEnable = cantEnable;
    }

    /// <summary>
    /// Publishes the current sequence to the Event Bus
    /// </summary>
    /// <param name="comboSequence"></param>
    /// <param name="comboIndex"></param>
    protected virtual void ExecuteCombo(ComboSequence comboSequence, int comboIndex)
    {
        TypingEventBus.Publish(comboSequence.Sequence, "Anything");
    }

    protected virtual void FailAllSequences()
    {
        if (!_canvasToShake[CurrentTypingContext]) return;
        
        _canvasToShake[CurrentTypingContext].ShakeCanvas();
    }
    
    #region Key Callbacks
    
    /// <summary>
    /// Redirection method for assigning into InputSystem for a Fail Key Button
    /// </summary>
    /// <param name="callbackContext"></param>
    private void EscapePressed(InputAction.CallbackContext callbackContext)
    {
        
    }
    
    /// <summary>
    /// Redirection method for assigning into InputSystem for a Fail Key Button
    /// </summary>
    /// <param name="callbackContext"></param>
    private void FailInstant(InputAction.CallbackContext callbackContext)
    {
        FailInstant();
    }
    
    /// <summary>
    /// Redirection method for assigning into InputSystem for an Enable/Disable Key Button
    /// </summary>
    /// <param name="callbackContext"></param>
    private void SwitchEnabled(InputAction.CallbackContext callbackContext)
    {
        if (_cantEnable) return;
        SetActive(!Active);
    }
    
    /// <summary>
    /// Redirection method for assigning into InputSystem for a Enable/Submit Key Button (Normally Enter)
    /// </summary>
    /// <param name="callbackContext"></param>
    private void EnableOrSubmit(InputAction.CallbackContext callbackContext)
    {
        if (_currentTypedSequence == "")
        {
            SetActive(!Active);
        }
        else
        {
            if(!CheckForFullMatch())
                FailInstant();
        }
    }
    
    /// <summary>
    /// Redirection method for assigning into InputSystem for any key that can contribute to a Sequence
    /// </summary>
    /// <param name="callbackContext"></param>
    /// <param name="key"></param>
    private void OnKeyPressed(InputAction.CallbackContext callbackContext, string key)
    {
        if (!Active) return;
        _currentTypedSequence += key;

        if (_useEnterForSubmit)
        {
            if (!CheckForwardForSingleKey())
            {
                FailInstant();
            }
            return;
        }

        CheckCombo();
    }

    #endregion


    public void AddSequences(List<ComboSequence> sequences)
    {
        _comboSequences.AddRange(sequences);
    }
}