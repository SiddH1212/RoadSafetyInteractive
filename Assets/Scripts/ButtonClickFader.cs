using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonClickFader : MonoBehaviour
{
    [Tooltip("The color to apply when the button has already been clicked.")]
    public Color clickedColor = new Color(1f, 1f, 1f, 0.4f); // translucent/faded
    [Tooltip("Optional: reference to the Image if not automatically found.")]
    public Image targetImage;

    private Button button;
    private Color originalColor;
    private bool hasBeenClicked = false;

    void Awake()
    {
        button = GetComponent<Button>();

        // If not assigned manually, find the first Image in the children
        if (targetImage == null)
        {
            targetImage = GetComponentInChildren<Image>();
        }

        if (targetImage != null)
        {
            originalColor = targetImage.color;
        }
        else
        {
            Debug.LogWarning($"{name}: No Image found on child for ButtonClickFader.");
        }

        button.onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        if (hasBeenClicked)
            return;

        hasBeenClicked = true;
        SetClickedAppearance();
    }

    private void SetClickedAppearance()
    {
        if (targetImage != null)
            targetImage.color = clickedColor;

        // Optional: disable interactivity too
        // button.interactable = false;
    }

    public void ResetButton()
    {
        hasBeenClicked = false;
        if (targetImage != null)
            targetImage.color = originalColor;

        // Optional: re-enable if you disabled it
        // button.interactable = true;
    }
}
