using UnityEngine;

public class TypingInteractable : MonoBehaviour, IInteractable
{ 
    [SerializeField] private string id;
    
    public void Interact()
    {
        TypingEventBus.Publish(id);
    }
}
