using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SummonButton : MonoBehaviour
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

        buttonText.text = text;
        SetSelected(selected);

        // Let Unity handle input events
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            onClicked?.Invoke(this);
            ToggleSelection();
        });
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