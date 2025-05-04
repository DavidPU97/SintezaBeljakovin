using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit.Samples.SpatialKeyboard;

public class SpatialKeyboardInputReader : MonoBehaviour
{
    private statsUpdate StatsScript;
    public TextMeshProUGUI textLevel0;
    public TMP_InputField keyboardInputField;
    public TextMeshProUGUI clearText;
    public XRKeyboard keyboard;

    public TextMeshProUGUI confirmInputText;
    public GameObject emptyInputPanel;
    public GameObject confirmInputPanel;
    public GameObject confirmationCanvas;
    
    private string userNameInput = null;

    private void Awake()
    {

        if (StatsScript == null)
        {
            StatsScript = GameObject.Find("MainStats").GetComponent<statsUpdate>();
        }
        StartCoroutine(RenameClear());
    }

    public void HandleInputComplete(string submittedText)
    {
        userNameInput = submittedText.Trim();
        confirmationCanvas.SetActive(true);

        if (string.IsNullOrWhiteSpace(userNameInput))
        {
            emptyInputPanel.SetActive(true);
        }
        else
        {
            confirmInputText.text = "Vneseno: \"" + userNameInput + "\". Nadaljujem?";
            confirmInputPanel.SetActive(true);
        }
    }

    public void submitInput()
    {
        StatsScript.playerName = userNameInput;
        userNameInput = null;
        keyboard.Clear();
        confirmInputText.text = "Vneseno: \"Ime 01\". Nadaljujem?";
        closeMenu();
        StatsScript.updateStats(0);
    }

    IEnumerator RenameClear()
    {
        yield return new WaitForSeconds(0.1f); // Wait for keyboard to build keys

        clearText.text = "Poèisti";
    }

    public void closeMenu()
    {
        EventSystem.current.SetSelectedGameObject(null);
        emptyInputPanel.SetActive(false);
        confirmInputPanel.SetActive(false);
        confirmationCanvas.SetActive(false);
    }
}
