using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;

public class RegionInteraction : MonoBehaviour
{
    public GameObject mouseOverHighlighterObject;
    public GameObject highlightGameObject;
    public GameObject Q1;
    public GameObject U2;
    public GameObject C2;
    public GameObject A2;
    public GameObject G2;
    private MouseOverHighlighter6 mouseOverScript;

    //private bool isHovered = false;
    //private bool isSelected = false;    
    private bool regionColored = false;
    private bool regionActive = false;
    private bool regionClicked = false;

    void CustomStart()
    {
        //GameObject mainCamera = GameObject.Find("Camera");
        if (mouseOverHighlighterObject != null && mouseOverScript == null)
        {
            mouseOverScript = mouseOverHighlighterObject.GetComponent<MouseOverHighlighter6>();
        }
    }

    void Update()
    {
        //CustomStart();
        if (regionActive && !regionColored && mouseOverScript.coloredRegion == gameObject.name)
        {
            if (mouseOverScript.correctAminoAcids == 16 || mouseOverScript.isAnimating || mouseOverScript.isAnimating2)
            {
                return;
            }
            // find
            mouseOverScript.highlightedObjects.AddRange(findObjectsToHighlight(highlightGameObject.name));

            // rotate and show
            mouseOverScript.hoverHighlight(gameObject.name);
            regionColored = true;
            
            if (regionClicked)
            {
                regionClicked = false;
                mouseOverScript.colorHighlightedAreas(gameObject.name);
            }
        }    
    }

    private XRBaseInteractable interactable;

    void Awake()
    {
        CustomStart();
        interactable = GetComponent<XRBaseInteractable>();

        interactable.hoverEntered.AddListener(OnHoverEnter);
        interactable.hoverExited.AddListener(OnHoverExit);
        interactable.selectEntered.AddListener(OnClicked);
        interactable.selectExited.AddListener(OnSelectExit);
    }

    void OnDestroy()
    {
        interactable.hoverEntered.RemoveListener(OnHoverEnter);
        interactable.hoverExited.RemoveListener(OnHoverExit);
        interactable.selectEntered.RemoveListener(OnClicked);
        interactable.selectExited.RemoveListener(OnSelectExit);
    }

    private void OnHoverEnter(HoverEnterEventArgs args)
    {
        if (!mouseOverScript.isAnyRegionAlreadyClicked)
        {
            if (mouseOverScript.coloredRegion == null)
            {
                regionActive = true;
                regionColored = false;
                mouseOverScript.coloredRegion = gameObject.name;
            }
        }
    }

    private void OnHoverExit(HoverExitEventArgs args)
    {
        if (!mouseOverScript.isAnyRegionAlreadyClicked)
        {
            regionActive = false;
            regionColored = false;
            mouseOverScript.coloredRegion = null;
            if (mouseOverScript.correctAminoAcids == 16 || mouseOverScript.isAnimating || mouseOverScript.isAnimating2)
            {
                return;
            }
            mouseOverScript.resetAreas();
        }
    }

    private void OnClicked(SelectEnterEventArgs args)
    {
        if (!mouseOverScript.isAnyRegionAlreadyClicked)
        {
            if (mouseOverScript.correctAminoAcids == 16 || mouseOverScript.isAnimating || mouseOverScript.isAnimating2)
            {
                return;
            }
            mouseOverScript.isAnyRegionAlreadyClicked = true;
            mouseOverScript.resetAreas();
            mouseOverScript.coloredRegion = gameObject.name;
            regionActive = true;
            regionColored = false;
            regionClicked = true;
            
            //mouseOverScript.isAnyRegionColored = false;
            //regionColored = false;
        }
    }
    
    private void OnSelectExit(SelectExitEventArgs args)
    {
        mouseOverScript.isAnyRegionAlreadyClicked = false;
        //XRDetectClick();
    }

    List<GameObject> findObjectsToHighlight(string name)
    {
        List<GameObject> gameObjects = new List<GameObject>();
        int dotIndex = name.IndexOf(".");
        gameObjects.Add(highlightGameObject);
        if (dotIndex != -1)
        {
            // to pomeni da je treba highlightati veè kot enega - Q1 nima pike
            gameObjects.Add(Q1);

            string area = name.Substring(0, 1);
            int level = int.Parse(name.Substring(1, 1));

            if (level == 2) {
                return gameObjects;
            }
            
            if (area == "U")
            {
                gameObjects.Add(U2);
            }
            else if (area == "C")
            {
                gameObjects.Add(C2);
            }
            else if (area == "A")
            {
                gameObjects.Add(A2);
            }
            else if (area == "G")
            {
                gameObjects.Add(G2);
            }
        }
        return gameObjects;
    }
}
