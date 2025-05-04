using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MenuToggle : MonoBehaviour
{
    public GameObject menuCanvas;
    public InputActionProperty toggleMenuAction; // Bind to XRI LeftHand/Menu
    private statsUpdate StatsScript;

    public GameObject mouseOverHighlighterObject6;
    private MouseOverHighlighter6 mouseOverScript;

    public GameObject skipSectionButton;

    private void Awake()
    {

        if (StatsScript == null)
        {
            StatsScript = GameObject.Find("MainStats").GetComponent<statsUpdate>();
        }

        mouseOverScript = mouseOverHighlighterObject6.GetComponent<MouseOverHighlighter6>();
    }

    private void OnEnable()
    {
        toggleMenuAction.action.Enable();
        toggleMenuAction.action.performed += OnMenuButtonPressed;
    }

    private void OnDisable()
    {
        toggleMenuAction.action.performed -= OnMenuButtonPressed;
        toggleMenuAction.action.Disable();
    }

    private void OnMenuButtonPressed(InputAction.CallbackContext context)
    {
        if (menuCanvas != null && StatsScript.playerName.Contains("admin"))
        {
            menuCanvas.SetActive(!menuCanvas.activeSelf);

            skipSectionButton.SetActive(StatsScript.currentLevel == 4 || StatsScript.currentLevel == 6);
        }
    }

    public void skipSection()
    {
        closeMenu();

        if (StatsScript.currentLevel == 4)
        {
            StatsScript.skipLevel4 = !StatsScript.skipLevel4;
        }
        else if (StatsScript.currentLevel == 6)
        {
            mouseOverScript.playContinuesly = !mouseOverScript.playContinuesly;
        }
    }

    public void resetLevel()
    {
        closeMenu();
        StatsScript.restartSimulation(StatsScript.currentLevel);
    }

    public void resetSimulation()
    {
        closeMenu();
        StatsScript.restartSimulation();
    }

    public void closeMenu()
    {
        EventSystem.current.SetSelectedGameObject(null);
        menuCanvas.SetActive(false); 
    }
}
