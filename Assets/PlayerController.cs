using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
     private NavMeshAgent _playerAgent;
     [SerializeField] private Animator _animator;

    private static readonly int RunUp = Animator.StringToHash("RunUp");
    private static readonly int RunDown = Animator.StringToHash("RunDown");
    private static readonly int RunLeft = Animator.StringToHash("RunLeft");
    private static readonly int RunRight = Animator.StringToHash("RunRight");
    private static readonly int IdleDown = Animator.StringToHash("IdleDown");
    private static readonly int IdleLeft = Animator.StringToHash("IdleLeft");
    private static readonly int IdleRight = Animator.StringToHash("IdleRight");
    private static readonly int IdleUp = Animator.StringToHash("IdleUp");
    
    private static readonly int RunUpRight = Animator.StringToHash("RunUpRight");
    private static readonly int RunUpLeft = Animator.StringToHash("RunUpLeft");
    private static readonly int RunDownRight = Animator.StringToHash("RunDownRight");
    private static readonly int RunDownLeft = Animator.StringToHash("RunDownLeft");
    
    float _diagonalDeadzone = 0.2f;  // How close x and z must be to consider diagonal
    float _minMoveThreshold = 0.05f;  // Below this: considered idle
    private float _interactDistance = 100f;
    private float _moveInteractDistance = 100f;
    
    private PlayerInput _playerInput;
    private int _currentAnimation;
    
    void Awake()
    {
        _playerAgent = GetComponent<NavMeshAgent>();
        _playerInput = new PlayerInput();
        _playerInput.Mouse.Enable();
        
        _playerInput.Mouse.Move.performed += MoveCharacter;
        _playerInput.Mouse.Interact.performed += OnInteract;
    }

    private void OnEnable()
    {
        _playerInput?.Enable();
    }

    private void OnDisable()
    {
        _playerInput?.Disable();
    }
    
    private void Update()
    {
        Vector3 movement = _playerAgent.velocity;
        _playerAgent.updateRotation = false;

        if (movement.sqrMagnitude < _minMoveThreshold * _minMoveThreshold)
        {
            ChangeAnimation(IdleDown);
            return;
        }
        
        float absX = Mathf.Abs(movement.x);
        float absZ = Mathf.Abs(movement.z);
        float diff = Mathf.Abs(absX - absZ);
        
        if (diff < _diagonalDeadzone)
        {
            // Diagonal movement
            // TODO Make this guy walk diagonally 
            // if (movement.x > 0 && movement.z > 0)
            //     ChangeAnimation(RunUpRight);
            // else if (movement.x < 0 && movement.z > 0)
            //     ChangeAnimation(RunUpLeft);
            // else if (movement.x > 0 && movement.z < 0)
            //     ChangeAnimation(RunDownRight);
            // else if (movement.x < 0 && movement.z < 0)
            //     ChangeAnimation(RunDownLeft);
        }
        else
        {
            // Dominant axis movement
            if (absX > absZ)
            {
                if (movement.x > 0)
                    ChangeAnimation(RunRight);
                else
                    ChangeAnimation(RunLeft);
            }
            else
            {
                if (movement.z > 0)
                    ChangeAnimation(RunUp);
                else
                    ChangeAnimation(RunDown);
            }
        }
    }
    
    private void ChangeAnimation(int animationStr, float crossFade = 0.2f)
    {
        if (_currentAnimation != animationStr)
        {
            _currentAnimation = animationStr;
            _animator.CrossFade(animationStr, crossFade);
        }
    }
    
    private void MoveCharacter(InputAction.CallbackContext callbackContext)
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit, _moveInteractDistance))
        {
            _playerAgent.destination = hit.point;
        }
    }
    
    private void OnInteract(InputAction.CallbackContext callbackContext)
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

#if DEBUG_DRAW_RAY
        Debug.DrawRay(ray.origin, ray.direction * interactDistance, Color.red, 1f);
#endif
        
        if (Physics.Raycast(ray, out RaycastHit hit, _interactDistance))
        {
            if (hit.collider.TryGetComponent<IInteractable>(out var interactable))
            {
                interactable.Interact();
            }
        }
    }
}

