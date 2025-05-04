using UnityEngine;

public class TextBubbleController : MonoBehaviour
{
    public GameObject textBubbleCanvas; // Reference to the canvas

    private void Start()
    {
        // Ensure the text bubble is hidden initially
        textBubbleCanvas.SetActive(false);
    }

    public void ShowTextBubble()
    {
        textBubbleCanvas.SetActive(true);
    }

    public void HideTextBubble()
    {
        textBubbleCanvas.SetActive(false);
    }
}
