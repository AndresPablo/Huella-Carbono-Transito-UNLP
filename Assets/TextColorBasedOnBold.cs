using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextColorBasedOnBold : MonoBehaviour
{
    private TextMeshProUGUI textComponent;

    void Start()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        UpdateTextColor();
    }

    void Update()
    {
        UpdateTextColor();
    }

    private void UpdateTextColor()
    {
        if (textComponent.fontStyle == FontStyles.Bold)
        {
            textComponent.color = Color.black; // Color negro para texto en bold
        }
        else
        {
            textComponent.color = Color.gray; // Color gris para texto no bold
        }
    }
}