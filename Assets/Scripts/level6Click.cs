using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;

public class level6Click : MonoBehaviour
{

    private statsUpdate StatsScript;
    public Material colorMaterial;
    public GameObject somatostatinTextBox;
    void CustomStart()
    {
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

        interactable.hoverEntered.AddListener(OnHoverEnter);
        interactable.hoverExited.AddListener(OnHoverExit);
        interactable.selectEntered.AddListener(OnClicked);
    }

    void OnDestroy()
    {
        interactable.hoverEntered.RemoveListener(OnHoverEnter);
        interactable.hoverExited.RemoveListener(OnHoverExit);
        interactable.selectEntered.RemoveListener(OnClicked);
    }

    private void OnHoverEnter(HoverEnterEventArgs args)
    {
        XRHoverEnter();
    }

    private void OnHoverExit(HoverExitEventArgs args)
    {
        XRHoverExit();
    }

    private void OnClicked(SelectEnterEventArgs args)
    {
        XRDetectClick();
    }

    //void OnMouseDown()
    void XRDetectClick()
    {
        CustomStart();
        StatsScript.updateStats(6);
        gameObject.SetActive(false);
        somatostatinTextBox.SetActive(false);
    }

    //private void OnMouseEnter()
    private void XRHoverEnter()
    {
        Renderer rendererBack = GetComponent<Renderer>();
        Material[] materialsBack = rendererBack.materials;
        if (materialsBack.Length == 1)
        {
            // Replace the last material with the new material
            Material[] newMaterialsBack = materialsBack.Concat(new Material[] { colorMaterial }).ToArray();

            // Assign the updated materials array back to the renderer
            rendererBack.materials = newMaterialsBack;
        }
    }
    
    //private void OnMouseExit()
    private void XRHoverExit()
    {
        Renderer renderer = GetComponent<Renderer>();
        Material[] materials = renderer.materials;
        if (materials.Length == 2)
        {
            //Material clickMaterial = isCorrectChoice ? GreenMaterial : (isAlmostCorrectChoice ? OrangeMaterial : RedMaterial);
            // Replace the last material with the new material
            Material[] newMaterials = materials.Take(materials.Length - 1).ToArray();

            // Assign the updated materials array back to the renderer
            renderer.materials = newMaterials;
        }
    }
}