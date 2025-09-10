using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D;

public class RadialMenuController : MonoBehaviour {
    [SerializeField] private SpriteAtlas _normalKeyAtlas = default; 
    [SerializeField] private SpriteAtlas _pressedKeyAtlas = default; 
    [SerializeField] private ComboPiece _letterPrefab;
    [SerializeField] private GameObject _radialContainer;
    [SerializeField] private GameObject _background;
    [SerializeField] private List<ComboPiece> _letters;
    [SerializeField] private float _radius = 250f;
    [SerializeField] private float _spinDuration = 0.3f; //seconds
    [Range(0f, 360f)]
    public float _rotationOffset = 0f;
    
    [SerializeField] private List<ComboSequence> _sequences;
    [SerializeField] private RadialMenuTypingSystem _radialMenuTypingSystem;
    
    private readonly string _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private bool _enabled;
    private PlayerInput _playerInput;
    private Coroutine _spinRoutine;
    private bool _letterSelect;
    private char _lastSelectedLetter;
    private readonly Dictionary<char, ComboPiece> _lettersMap = new Dictionary<char, ComboPiece>();
    private void Awake()
    {
        _letterSelect = true;
        
        _playerInput = new PlayerInput();
        _playerInput.Keyboard.Enable();
        _playerInput.Keyboard.A.performed += FocusLetterAnimated;
        _playerInput.Keyboard.A.performed += FocusLetterAnimated;
        _playerInput.Keyboard.B.performed += FocusLetterAnimated;
        _playerInput.Keyboard.C.performed += FocusLetterAnimated;
        _playerInput.Keyboard.D.performed += FocusLetterAnimated;
        _playerInput.Keyboard.E.performed += FocusLetterAnimated;
        _playerInput.Keyboard.F.performed += FocusLetterAnimated;
        _playerInput.Keyboard.G.performed += FocusLetterAnimated;
        _playerInput.Keyboard.H.performed += FocusLetterAnimated;
        _playerInput.Keyboard.I.performed += FocusLetterAnimated;
        _playerInput.Keyboard.J.performed += FocusLetterAnimated;
        _playerInput.Keyboard.K.performed += FocusLetterAnimated;
        _playerInput.Keyboard.L.performed += FocusLetterAnimated;
        _playerInput.Keyboard.M.performed += FocusLetterAnimated;
        _playerInput.Keyboard.N.performed += FocusLetterAnimated;
        _playerInput.Keyboard.O.performed += FocusLetterAnimated;
        _playerInput.Keyboard.P.performed += FocusLetterAnimated;
        _playerInput.Keyboard.Q.performed += FocusLetterAnimated;
        _playerInput.Keyboard.R.performed += FocusLetterAnimated;
        _playerInput.Keyboard.S.performed += FocusLetterAnimated;
        _playerInput.Keyboard.T.performed += FocusLetterAnimated;
        _playerInput.Keyboard.U.performed += FocusLetterAnimated;
        _playerInput.Keyboard.V.performed += FocusLetterAnimated;
        _playerInput.Keyboard.W.performed += FocusLetterAnimated;
        _playerInput.Keyboard.X.performed += FocusLetterAnimated;
        _playerInput.Keyboard.Y.performed += FocusLetterAnimated;
        _playerInput.Keyboard.Z.performed += FocusLetterAnimated;
        _playerInput.Keyboard.Enter.performed += EnterPressed;
        _playerInput.Keyboard.Backspace.performed += BackspacePressed;

        var i = 0;
        foreach (var letter in _letters)
        {
            _lettersMap.Add(_alphabet[i], letter);
            i++;
        }
        _radialMenuTypingSystem?.AddSequences(_sequences);
    }

    private void FocusLetterAnimated(InputAction.CallbackContext callbackContext)
    {
        if (_enterPressed) return;
        FocusLetterAnimated(callbackContext.action.name[0]);
        ShowFullWords();
    }
    
    void Start()
    {
        HideAllFullWords();
        
        for (int i = 0; i < _letters.Count; i++) {
            ComboPiece letter = _letters[i]; // You could instantiate anything here
            letter.GetComponent<ComboPiece>().SetSprite(_normalKeyAtlas.GetSprite(_alphabet[i].ToString()));
            var charPressed = $"pressed_{_alphabet[i].ToString()}";
            letter.GetComponent<ComboPiece>().SetPressedSprite(_pressedKeyAtlas.GetSprite(charPressed));
            _letters[i] = letter;
        }
    }

    void Update() {
        if (!_enabled) return;
        
        float step = 360f / _alphabet.Length;

        for (int i = 0; i < _letters.Count; i++) {
            float angle = 90f - i * step - _rotationOffset;
            float rad = angle * Mathf.Deg2Rad;

            Vector2 pos = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * _radius;
            _letters[i].GetComponent<RectTransform>().anchoredPosition = pos;
            _letters[i].GetComponent<RectTransform>().rotation = Quaternion.identity;
        }
    }
    
    private bool GetRotationTarget(char letter, out float targetOffset)
    {
        int index = _alphabet.IndexOf(char.ToUpper(letter));
        if (index == -1)
        {
            targetOffset = 0;
            return true;
        }

        float step = 360f / _alphabet.Length;
        targetOffset = -index * step;
        return false;
    }    

    private void FocusLetterAnimated(char letter) {
        if (!_enabled)
        {
            return;
        }
        _lastSelectedLetter = char.ToUpper(letter);
        if (GetRotationTarget(letter, out var targetOffset)) return;

        if (_spinRoutine != null) {
            StopCoroutine(_spinRoutine);
        }
        DeactivateLetters();
        _spinRoutine = StartCoroutine(SpinToOffset(targetOffset, letter));
    }
    
    private IEnumerator SpinToOffset(float targetOffset, char letter)
    {
        if (!_enabled)
        {
            yield return null;
        }
        
        float startOffset = _rotationOffset;
        targetOffset = Mathf.Repeat(targetOffset, 360f);

        float delta = Mathf.DeltaAngle(startOffset, targetOffset);

        float elapsed = 0f;
        while (elapsed < _spinDuration) {
            elapsed += Time.deltaTime;
            float t = elapsed / _spinDuration;
            _rotationOffset = Mathf.LerpAngle(startOffset, startOffset + delta, t);
            yield return null;
        }

        _rotationOffset = targetOffset;
        _spinRoutine = null;
        ActivateLetter(char.ToUpper(letter));
    }
    
    private void FocusLetterInstant(char letter)
    {
        if (GetRotationTarget(letter, out var targetOffset)) return;
        DeactivateLetters();
        _rotationOffset = Mathf.Repeat(targetOffset, 360f);
        ActivateLetter(char.ToUpper(letter));
    }

    private void DeactivateLetters()
    {
        foreach (var piece in _letters)
        {
            piece.Deactivate();
        }
    }
    
    private void ActivateLetter(char letter)
    {
        _lettersMap[letter].Activate();
    }
    
    public void SetActive(bool enable)
    {       
        if (!_enabled && enable)
        {
            FocusLetterInstant('a');
            HideAllFullWords();
        }
        _enabled = enable;
    }
    
    private void HideAllFullWords()
    {
        foreach (var sequence in _sequences)
        {
            sequence.gameObject.SetActive(false);
        }
    }

    private void ShowFullWords()
    {
        HideAllFullWords();
        var data = GetDataFromLastSelectedLetter();
        for (var i = 0; i < data.Count; i++)
        {
            _sequences[i].gameObject.SetActive(true);
            _sequences[i].Initialize(data[i], _normalKeyAtlas, _pressedKeyAtlas);
        }
    }
    
    List<String> AData = new List<string>(4){"ADAMANTIO", "APOCALYPSE", "AMIGO", "AMAZONIA"};
    List<String> BData = new List<string>(4){"BEBECITO", "BANDANA"};
    List<String> CData = new List<string>(4){"CACTUS", "CURCUMA", "CAI"};
    private List<string> GetDataFromLastSelectedLetter()
    {
        switch (_lastSelectedLetter)
        {
            case ('A'):
                return AData;
            case ('B'):
                return BData;
            case ('C'):
                return CData;
            default:
                return new List<string>();
        }
    }

    private bool _enterPressed;
    private bool _backspacePressed = true;
    private void EnterPressed(InputAction.CallbackContext callbackContext)
    {
        _enterPressed = true;
        _backspacePressed = false;
        _radialMenuTypingSystem.SetActive(true);
        _enabled = false;
    }
    
    private void BackspacePressed(InputAction.CallbackContext callbackContext)
    {
        _backspacePressed = true;
        _enterPressed = false;
        _radialMenuTypingSystem.SetActive(false);
        _enabled = true;
    }
}