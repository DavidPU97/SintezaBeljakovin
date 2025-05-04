using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering.Universal;

public class MouseOverHighlighter2 : MonoBehaviour
{
    public GameObject ShadedAnimal; // Assign your highlight material in the Inspector
    public GameObject ShadedPlant; // Assign your highlight material in the Inspector
    public GameObject ShadedBacteria1; // Assign your highlight material in the Inspector
    public GameObject ShadedBacteria2; // Assign your highlight material in the Inspector
    public GameObject ShadedBacteria3; // Assign your highlight material in the Inspector
    public GameObject ShadedFungus; // Assign your highlight material in the Inspector
    public GameObject ShadedMitohondrij; // Assign your highlight material in the Inspector
    public GameObject Ribosomi; // Assign your highlight material in the Inspector
    public GameObject ZrnatiRibosom; // Assign your highlight material in the Inspector
    public GameObject NuclearEnvelope; // Assign your highlight material in the Inspector
    public GameObject Nucleolus; // Assign your highlight material in the Inspector

    private GameObject highlightedObject; // Currently highlighted object
    private List<GameObject> highlightedObjects = new List<GameObject>(); // Currently highlighted object
    public Material OutlineMaterial;
    public Material RedMaterial;
    public Material GreenMaterial;
    public Material OrangeMaterial;

    //private GameObject hiddenObject; // Currently hidden object

    private string[] tags = { "Animal", "Plant", "Bacteria", "Fungus", "Mitohondrij", "Ribosom1", "Ribosom2", "NuclearEnvelope", "Nucleolus" };

    public List<string> clickedObjects = new List<string>();
    public int clickedObjectsNumber = 0;
    public List<GameObject> clickedGameObjects = new List<GameObject>();
                                                                          //private GameObject hiddenObject; // Currently hidden object
    public UniversalRendererData OutlineRendererOriginal; // Assign your highlight material in the Inspector
    private UniversalRendererData OutlineRendererRuntime; // Assign your highlight material in the Inspector
    private statsUpdate StatsScript;

    public Animator mRNAAnimatorLevel5;
    public Transform RightControllerTransform;
    private float lastHighlightTime = -1f; // Default to -1 (never used)

    void Start()
    {
        OutlineRendererRuntime = Instantiate(OutlineRendererOriginal);
        changeColorInRenderer(false);
        StatsScript = GameObject.Find("MainStats").GetComponent<statsUpdate>();
        lastHighlightTime = Time.time - 1f;
    }

    //private float timer = 0f;
    //private float updateDelay = 1f;

    void Update()
    {
        /*if (Input.GetKeyDown("space"))
        {
            StatsScript.restartSimulation();
        }*/

        /*timer += Time.deltaTime;

        if (timer >= updateDelay)
        {
            timer = 0f;*/

        // Cast a ray from the camera to the mouse position
        int interactionMask = ~(1 << LayerMask.NameToLayer("TransparentFloor"));
        Ray ray = new Ray(RightControllerTransform.position, RightControllerTransform.forward);
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Check if the ray hits an object
            if (Physics.Raycast(ray, out hit, 300f, interactionMask))
            {
                GameObject hitObject = hit.collider.gameObject;

                // If the object has changed or is new, update highlight
                if (((highlightedObject != hitObject) && (highlightedObject == null || highlightedObject.tag != hitObject.tag) && Time.time - lastHighlightTime > 0.5f) || clickedObjectsNumber != clickedObjects.Count)
                {
                    lastHighlightTime = Time.time;
                    clickedObjectsNumber = clickedObjects.Count;
                    // Reset previous object material
                    if (highlightedObject != null)
                    {
                        ResetMaterial();
                        //highlightedObject.SetActive(true);
                    }

                    // Set new object as highlighted and change its material
                    highlightedObject = hitObject;
                    //hitObject.SetActive(false);

                    castOutline(hitObject.tag);
                }
            }
            else if (highlightedObject != null)
            {
                // If no object is hit, reset the currently highlighted object
                ResetMaterial();
            }
        //}
    }

    private void ResetMaterial(bool forceReset = false)
    {
        if (highlightedObject != null || forceReset)
        {
            ShadedAnimal.SetActive(false);
            ShadedPlant.SetActive(false);
            ShadedFungus.SetActive(false);
            ShadedBacteria1.SetActive(false);
            ShadedBacteria2.SetActive(false);
            ShadedBacteria3.SetActive(false);
            ShadedMitohondrij.SetActive(false);

            ZrnatiRibosom.SetActive(false);
            Ribosomi.SetActive(false);

            NuclearEnvelope.SetActive(false);
            Nucleolus.SetActive(false);

            highlightedObject = null;
        }

        /*if (hiddenObject != null)
        {
            hiddenObject.SetActive(true);
            hiddenObject = null;
        }*/
    }

    private void castOutline(string tag)
    {
        //bool isOutline = highlightedObject.name.Contains("Outline");
        if (tags.Contains(tag) )
        {
            bool isCorrectChoice = false;
            bool isAlmostCorrectChoice = false;

            highlightedObjects.Clear();
            /*hiddenObject = highlightedObject;
            if (!hiddenObject.name.Contains("Original"))
            {
                hiddenObject.SetActive(false);
            }*/

            if (tag == "Animal")
            {
                highlightedObjects.Add(ShadedAnimal);
                isAlmostCorrectChoice = true;
            }
            else if (tag == "Plant")
            {
                highlightedObjects.Add(ShadedPlant);
            }
            else if (tag == "Fungus")
            {
                highlightedObjects.Add(ShadedFungus);

            }
            else if (tag == "Bacteria")
            {
                highlightedObjects.Add(ShadedBacteria1);
                highlightedObjects.Add(ShadedBacteria2);
                highlightedObjects.Add(ShadedBacteria3);
            }
            else if (tag == "Mitohondrij")
            {
                highlightedObjects.Add(ShadedMitohondrij);
            }
            else if (tag == "Ribosom1")
            {
                isAlmostCorrectChoice = true;
                highlightedObjects.Add(Ribosomi);
            }
            else if (tag == "Ribosom2")
            {
                isCorrectChoice = true;
                highlightedObjects.Add(ZrnatiRibosom);
            }
            else if (tag == "NuclearEnvelope")
            {
                isAlmostCorrectChoice = StatsScript.currentLevel == 2;
                highlightedObjects.Add(NuclearEnvelope);
            }
            else if (tag == "Nucleolus")
            {
                isCorrectChoice = StatsScript.currentLevel == 2;
                if (!isCorrectChoice)
                {
                    return; // tega ne smemo izbirati v level 5
                }
                highlightedObjects.Add(Nucleolus);
            }
            
            foreach (GameObject obj in highlightedObjects)
            {
                obj.SetActive(true);
                bool isTagged = clickedObjects.Contains(obj.tag);

                Renderer renderer = obj.GetComponent<Renderer>();
                Material[] materials = renderer.materials;
                if (materials.Length > 0)
                {
                    Material clickMaterial = isCorrectChoice ? GreenMaterial : (isAlmostCorrectChoice ? OrangeMaterial : RedMaterial);
                    // Replace the last material with the new material
                    materials[materials.Length - 1] = isTagged ? clickMaterial : OutlineMaterial;

                    // Assign the updated materials array back to the renderer
                    renderer.materials = materials;

                }
            }

            highlightedObject = highlightedObjects[0];
        }
        /*else if (hiddenObject != null) {
            hiddenObject.SetActive(true);
            hiddenObject = null;
        }*/
    }

    public void changeColorInRenderer(bool wasTagged)
    {
        if (OutlineRendererRuntime == null)
        {
            OutlineRendererRuntime = Instantiate(OutlineRendererOriginal);
        }
        // Iterate through the list of ScriptableRendererFeatures in the rendererData
        foreach (var feature in OutlineRendererRuntime.rendererFeatures)
        {
            // Check if the feature is a RenderObjects type
            if (feature is RenderObjects renderObjectsFeature)
            {
                // Assuming we want to modify a specific RenderObjects feature by name
                if (renderObjectsFeature.name == "RenderObjectsOutline")
                {
                    // Set the material override in the RenderObjects feature
                    renderObjectsFeature.SetActive(!wasTagged);

                }
                else if (renderObjectsFeature.name == "RenderObjectsClick")
                {
                    // Set the material override in the RenderObjects feature
                    renderObjectsFeature.SetActive(wasTagged);

                }
            }
        }
    }

    private void highlightMultiple(GameObject parentObject)
    {
        parentObject.SetActive(true);
        Transform children = parentObject.transform;
        foreach (Transform child in children)
        {
            highlightedObjects.Add(child.gameObject);
        }
    }

    public void resetLevel2()
    {
        ResetMaterial(true);
        clickedObjects = new List<string>();
        clickedObjectsNumber = 0;
        clickedGameObjects = new List<GameObject>();
    }

    public void resetLevel5()
    {
        ResetMaterial(true);
        clickedObjects = new List<string>();
        clickedObjectsNumber = 0;
        clickedGameObjects = new List<GameObject>();
        mRNAAnimatorLevel5.Rebind();
        mRNAAnimatorLevel5.Update(0f);
    }
}
