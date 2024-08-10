using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldCheck : MonoBehaviour
{
    public TMP_InputField inputField;
    public Button submitButton;

    private void Start()
    {
        submitButton.interactable = false;
        inputField.onValueChanged.AddListener(delegate { OnInputFieldChanged(); });
    }

    public void OnInputFieldChanged()
    {
        if (!string.IsNullOrWhiteSpace(inputField.text))
        {
            submitButton.interactable = true;
        }
        else
        {
            submitButton.interactable = false;
        }
    }
}
