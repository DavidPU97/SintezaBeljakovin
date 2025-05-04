using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;

public class level4Click : MonoBehaviour
{
    private MouseOverHighlighter4 mouseOverScript;
    private statsUpdate StatsScript;
    public GameObject mouseOver4Component;
    void CustomStart()
    {
        if (mouseOverScript == null && mouseOver4Component != null)
        {
            mouseOverScript = mouseOver4Component.GetComponent<MouseOverHighlighter4>();
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

    //void OnMouseDown()
    void XRDetectClick()
    {
        CustomStart();
        // This code will run when the object is clicked
        if (gameObject.tag == "color")
        {
            if (mouseOverScript.selectedColor != "")
            {
                mouseOverScript.selectedColorPreviousFront = mouseOverScript.colorsFront.transform.Find(mouseOverScript.selectedColor + "Front").gameObject;
                mouseOverScript.selectedColorPreviousBack = mouseOverScript.colorsBack.transform.Find(mouseOverScript.selectedColor + "Back").gameObject;
            }
            mouseOverScript.selectedColor = gameObject.name.Replace("Front", "").Replace("Back", "");
            mouseOverScript.forceUpdate = gameObject.tag;
        }
        else if (gameObject.tag == "Animal") // pore
        {
            mouseOverScript.forceUpdate = gameObject.tag;

            if (mouseOverScript.isRNAFinished && mouseOverScript.ismRNASpiraleClicked)
            {
                StatsScript.updateStats(4, true); // to mora biti pod pogojem da si najprej kliknil mRNA 
            }
        }
        else
        {
            mouseOverScript.forceUpdate = gameObject.tag;
        }
    }
}