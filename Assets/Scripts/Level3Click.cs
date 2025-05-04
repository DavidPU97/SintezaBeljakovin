using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;

public class Level3Click : MonoBehaviour
{
    public GameObject mainCameraLevel3;
    private MouseOverHighlighter mouseOverScript;
    private statsUpdate StatsScript;
    void CustomStart()
    {
        if (mainCameraLevel3 != null && mouseOverScript == null)
        {
            mouseOverScript = mainCameraLevel3.GetComponent<MouseOverHighlighter>();
        }

        if (StatsScript == null)
        {
            GameObject Stats = GameObject.Find("MainStats");
            StatsScript = Stats.GetComponent<statsUpdate>();
        }
    }

    private XRBaseInteractable interactable;

    void Awake()
    {
        interactable = GetComponent<XRBaseInteractable>();
        interactable.selectEntered.AddListener(OnClicked);
    }

    private void OnClicked(SelectEnterEventArgs args)
    {
        XRDetectClick();
    }

    private void OnDestroy()
    {
        interactable.selectEntered.RemoveListener(OnClicked);
    }

    public GameObject targetObject; // Assign the inactive object in the Inspector

    //void OnMouseDown()
    void XRDetectClick()
    {
        CustomStart();
        if (mouseOverScript.clickedObjects == null || !mouseOverScript.clickedObjects.Contains(gameObject.tag))
        {
            mouseOverScript.clickedObjects.Add(gameObject.tag);
        }

        if (targetObject != null) { 
            targetObject.SetActive(true);
        }
        //StatsScript.updateStats();
    }
}