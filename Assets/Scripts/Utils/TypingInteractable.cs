using UnityEngine;

public class TypingInteractable : MonoBehaviour, ITypingInteractable
{ 
    [SerializeField] private string _sequence;
    private bool _playerInRange = false;
    
    void OnEnable() {
        TypingEventBus.Subscribe(_sequence, Interact);
    }

    void OnDisable() {
        TypingEventBus.Unsubscribe(_sequence, Interact);
    }
    
    public void Interact(object[] args)
    {
        if (!_playerInRange) return;
        Debug.Log("Typing Interacted");
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInRange = false; 
        }
    }
}
