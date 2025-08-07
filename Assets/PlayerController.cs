using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private NavMeshAgent agent;
    private PlayerInput playerInput;
    private int currentAnimation;

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
    
    float diagonalDeadzone = 0.2f;  // How close x and z must be to consider diagonal
    float minMoveThreshold = 0.05f;  // Below this: considered idle
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        playerInput = new PlayerInput();
        playerInput.Enable();
        AssignInputs();
    }

    private void OnEnable()
    {
        playerInput?.Enable();
    }

    private void OnDisable()
    {
        playerInput?.Disable();
    }
    
    private void AssignInputs()
    {
        playerInput.Mouse.Move.performed += MoveCharacter;
    }
    
    private void Update()
    {
        Vector3 movement = agent.velocity;
        agent.updateRotation = false;

        if (movement.sqrMagnitude < minMoveThreshold * minMoveThreshold)
        {
            ChangeAnimation(IdleDown);
            return;
        }
        
        float absX = Mathf.Abs(movement.x);
        float absZ = Mathf.Abs(movement.z);
        float diff = Mathf.Abs(absX - absZ);
        
        if (diff < diagonalDeadzone)
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
        if (currentAnimation != animationStr)
        {
            currentAnimation = animationStr;
            animator.CrossFade(animationStr, crossFade);
        }
    }
    
    private void MoveCharacter(InputAction.CallbackContext callbackContext)
    {
        RaycastHit hit;

        Vector2 mousePos = Mouse.current.position.ReadValue(); // New Input System
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out hit, 100))
        {
            agent.destination = hit.point;
        }
    }

}
