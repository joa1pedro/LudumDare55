using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SummonButton : MonoBehaviour, IPointerClickHandler, ISubmitHandler
{
    [SerializeField] private Text buttonText;
    [SerializeField] private Image buttonImage;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color selectedColor = Color.green;
    
    private Button button;
    private bool isSelected;
    private UnityAction<SummonButton> onClicked;

    public bool Selected => isSelected;
    public string SummonName => buttonText.text;
    
    public void Initialize(string text, bool selected, UnityAction<SummonButton> callback)
    {
        buttonImage = GetComponent<Image>(); 
        button = GetComponent<Button>();
        onClicked = callback;

        SetSelected(selected);
        buttonText.text = text;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onClicked?.Invoke(this);
        ToggleSelection();
    }

    public void OnSubmit(BaseEventData eventData)
    {
        onClicked?.Invoke(this);
        ToggleSelection();
    }
    
    public void SetSelected(bool selected)
    {
        isSelected = selected;
        buttonImage.color = isSelected ? selectedColor : normalColor;
    }

    private void ToggleSelection()
    {
        SetSelected(isSelected);
    }
}