using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering.Universal;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;

public class ClickableObject : MonoBehaviour
{
    private MouseOverHighlighter mouseOverScript;
    private statsUpdate StatsScript;
    public GameObject chalkboardObject; // Assign the inactive object in the Inspector
    public GameObject textBubbleObject; // Assign the inactive object in the Inspector
    public GameObject mouseOverGameObject;
    
    
    void CustomStart()
    {
        // This code will run when the object is clicked
        //GameObject mainCamera = GameObject.Find("Camera");
        if (mouseOverScript == null && mouseOverGameObject != null)
        {
            mouseOverScript = mouseOverGameObject.GetComponent<MouseOverHighlighter>();
        }

        if (StatsScript == null)
        {
            GameObject Stats = GameObject.Find("MainStats");
            StatsScript = Stats.GetComponent<statsUpdate>();
        }
    }


    // Update is called once per frame
    void Update()
    {
        
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


    //void OnMouseDown()
    public void XRDetectClick()
    {
        CustomStart();
        // This code will run when the object is clicked
        if (mouseOverScript.clickedObjects == null || !mouseOverScript.clickedObjects.Contains(gameObject.tag))
        {
            mouseOverScript.clickedObjects.Add(gameObject.tag);
        }

        resetChalkboard();
        if (gameObject.tag == "Animal")
        {
            if (!textBubbleObject.activeSelf)
            {
                chalkboardObject.SetActive(true);
                textBubbleObject.SetActive(true);
                StatsScript.audioSource.Play();
                StatsScript.timeLevel1End = Time.time - StatsScript.timeLevel1Start;
                StatsScript.timeLevel1String = StatsScript.setTimeString(StatsScript.timeLevel1End);
            }
            else
            {
                StatsScript.updateStats(1);
            }
        }
        else
        {
            if (!mouseOverScript.clickedObjects.Contains("Animal")){
                StatsScript.mistakesLevel1 = mouseOverScript.clickedObjects.Count;
            }
            textBubbleObject.SetActive(true);
            chalkboardObject.SetActive(true); // chalkboard
        }
    }

    void resetChalkboard()
    {
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("chalkboard");
        foreach (GameObject obj in taggedObjects)
        {
            // Toggle active state
            if (obj.name != "TextBubbleAnimal")
            {
                obj.SetActive(false);
            }
        }
    }
}