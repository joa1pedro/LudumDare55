using UnityEngine;

public class ClickInteractable : MonoBehaviour, IClickInteractable
{ 
    [SerializeField] private string _id;
    
    public void Interact()
    {
        Debug.Log("Typing Interacted");
        TypingEventBus.Publish(_id);
    }
}
