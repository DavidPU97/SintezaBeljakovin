using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
//using TreeEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering.Universal;

public class MouseOverHighlighter : MonoBehaviour
{
    public GameObject ShadedAnimal; // Assign your highlight material in the Inspector
    public GameObject ShadedPlant; // Assign your highlight material in the Inspector
    public GameObject ShadedBacteria1; // Assign your highlight material in the Inspector
    public GameObject ShadedBacteria2; // Assign your highlight material in the Inspector
    public GameObject ShadedBacteria3; // Assign your highlight material in the Inspector
    public GameObject ShadedBacteria4; // Assign your highlight material in the Inspector
    public GameObject ShadedFungus; // Assign your highlight material in the Inspector

    private GameObject highlightedObject; // Currently highlighted object
    public GameObject animalTextBox;
    public Material OutlineMaterial;
    public Material OutlineSlightlyThickerMaterial;

    private string[] tags = { "Animal", "Plant", "Bacteria", "Fungus" };

    public List<string> clickedObjects = new List<string>();
    private int clickedObjectsNumber = 0;
    public List<GameObject> clickedGameObjects = new List<GameObject>();
    private List<GameObject> highlightedObjects = new List<GameObject>(); // Currently highlighted object
                                                                          //private GameObject hiddenObject; // Currently hidden object
    public UniversalRendererData OutlineRendererOriginal; // Assign your highlight material in the Inspector
    private UniversalRendererData OutlineRendererRuntime; // Assign your highlight material in the Inspector

    // Level 3
    public GameObject Chromosome1; // Assign your highlight material in the Inspector
    public GameObject Chromosome2; // Assign your highlight material in the Inspector
    public GameObject Chromosome3; // Assign your highlight material in the Inspector
    public GameObject Chromosome4; // Assign your highlight material in the Inspector
    public GameObject Chromosome5; // Assign your highlight material in the Inspector
    public GameObject Chromosome6; // Assign your highlight material in the Inspector
    public GameObject Chromosome7; // Assign your highlight material in the Inspector
    public GameObject Chromosome8; // Assign your highlight material in the Inspector
    public GameObject Chromosome9; // Assign your highlight material in the Inspector
    public GameObject Chromosome10; // Assign your highlight material in the Inspector
    public GameObject Chromosome11; // Assign your highlight material in the Inspector
    public GameObject Chromosome12; // Assign your highlight material in the Inspector
    public GameObject Chromosome13; // Assign your highlight material in the Inspector
    public GameObject Chromosome14; // Assign your highlight material in the Inspector
    public GameObject Chromosome15; // Assign your highlight material in the Inspector
    public GameObject Chromosome16; // Assign your highlight material in the Inspector
    public GameObject Chromosome17; // Assign your highlight material in the Inspector
    public GameObject Chromosome18; // Assign your highlight material in the Inspector
    public GameObject Chromosome19; // Assign your highlight material in the Inspector
    public GameObject Chromosome20; // Assign your highlight material in the Inspector
    public GameObject Chromosome21; // Assign your highlight material in the Inspector
    public GameObject Chromosome22; // Assign your highlight material in the Inspector
    public GameObject Chromosome23; // Assign your highlight material in the Inspector



    public Material RedMaterial;
    public Material GreenMaterial;
    public Material OrangeMaterial;

    //private GameObject hiddenObject; // Currently hidden object

    public GameObject kromosom3TextBubble;
    public List<string> changedObjects = new List<string>();

    private Dictionary<string, GameObject> objectDictionary = new Dictionary<string, GameObject>();
    private statsUpdate StatsScript;

    public Transform RightControllerTransform;

    void Start()
    {
        OutlineRendererRuntime = Instantiate(OutlineRendererOriginal);
        changeColorInRenderer(false);

        // level 3
        StatsScript = GameObject.Find("MainStats").GetComponent<statsUpdate>();

        objectDictionary["Chromosome1"] = Chromosome1;
        objectDictionary["Chromosome2"] = Chromosome2;
        objectDictionary["Chromosome3"] = Chromosome3;
        objectDictionary["Chromosome4"] = Chromosome4;
        objectDictionary["Chromosome5"] = Chromosome5;
        objectDictionary["Chromosome6"] = Chromosome6;
        objectDictionary["Chromosome7"] = Chromosome7;
        objectDictionary["Chromosome8"] = Chromosome8;
        objectDictionary["Chromosome9"] = Chromosome9;
        objectDictionary["Chromosome10"] = Chromosome10;
        objectDictionary["Chromosome11"] = Chromosome11;
        objectDictionary["Chromosome12"] = Chromosome12;
        objectDictionary["Chromosome13"] = Chromosome13;
        objectDictionary["Chromosome14"] = Chromosome14;
        objectDictionary["Chromosome15"] = Chromosome15;
        objectDictionary["Chromosome16"] = Chromosome16;
        objectDictionary["Chromosome17"] = Chromosome17;
        objectDictionary["Chromosome18"] = Chromosome18;
        objectDictionary["Chromosome19"] = Chromosome19;
        objectDictionary["Chromosome20"] = Chromosome20;
        objectDictionary["Chromosome21"] = Chromosome21;
        objectDictionary["Chromosome22"] = Chromosome22;
        objectDictionary["Chromosome23"] = Chromosome23;
    }

    void Update()
    {
        Ray ray = new Ray(RightControllerTransform.position, RightControllerTransform.forward);

        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        /*if (Input.GetKeyDown("space") )
        {
            StatsScript.restartSimulation();
        }*/

        if (StatsScript.currentLevel == 1)
        {
            // Check if the ray hits an object
            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitObject = hit.collider.gameObject;

                // If the object has changed or is new, update highlight
                if (((highlightedObject != hitObject) && (highlightedObject == null || highlightedObject.tag != hitObject.tag)) || clickedObjectsNumber != clickedObjects.Count)
                {
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
        }
        else if (StatsScript.currentLevel == 3)
        {
            // level 3
            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitObject = hit.collider.gameObject;

                // If the object has changed or is new, update highlight
                if (((highlightedObject != hitObject) && (highlightedObject == null || highlightedObject.tag != hitObject.tag)) || clickedObjectsNumber != clickedObjects.Count)
                {
                    clickedObjectsNumber = clickedObjects.Count;
                    // Reset previous object material
                    if (highlightedObject != null)
                    {
                        ResetMaterial_level3();
                        //highlightedObject.SetActive(true);
                    }

                    // Set new object as highlighted and change its material
                    highlightedObject = hitObject;
                    //hitObject.SetActive(false);

                    castOutline_level3(hitObject.tag);
                }
            }
            else if (highlightedObject != null)
            {
                // If no object is hit, reset the currently highlighted object
                ResetMaterial_level3();
            }
        }
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
            ShadedBacteria4.SetActive(false);


            highlightedObject = null;
        }
        /*
        if (hiddenObject != null)
        {
            hiddenObject.SetActive(true);
            hiddenObject = null;
        }        */
    }
    
    private void castOutline(string tag)
    {
        if (tags.Contains(tag))
        {
            bool isCorrectChoice = false;
            highlightedObjects.Clear();
            /*hiddenObject = highlightedObject;
            if (!hiddenObject.name.Contains("Original"))
            {
                //hiddenObject.SetActive(false);
            }*/

            if (tag == "Animal")
            {
               highlightedObjects.Add(ShadedAnimal);
               isCorrectChoice = true;
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
                highlightedObjects.Add(ShadedBacteria4);
            }

            foreach (GameObject obj in highlightedObjects) { 
                obj.SetActive(true);
                bool isTagged = clickedObjects.Contains(obj.tag);
                changeColorInRenderer(isTagged);
                
                Renderer renderer = obj.GetComponent<Renderer>();
                Material[] materials = renderer.materials;
                if (materials.Length > 0)
                {
                    Material clickMaterial = isCorrectChoice ? GreenMaterial : RedMaterial;
                    // Replace the last material with the new material
                    materials[materials.Length - 1] = isTagged ? clickMaterial : ((tag == "Animal" || tag == "Fungus" || tag == "Bacteria") ? OutlineSlightlyThickerMaterial : OutlineMaterial);

                    // Assign the updated materials array back to the renderer
                    renderer.materials = materials;

                }
            }
        }
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

    public void changeCanvasTextColor(int chromosomeNumber, bool isTagged, bool reset = false)
    {
        GameObject canvas = GameObject.Find("Chromosome Label " + chromosomeNumber);
        if (canvas == null)
        {
            return;
        }
        Transform imageShape = canvas.transform.Find("Imageshape");
        if (imageShape == null)
        {
            return;
        }
        Transform image = imageShape.Find("Image");
        if (image == null)
        {
            return;
        }
        Transform textTransform = image.Find("Ttile");
        if (textTransform == null)
        {
            return;
        }

        TextMeshProUGUI textComponent = textTransform.GetComponent<TextMeshProUGUI>();
        if (textComponent != null)
        {
            if (isTagged || (kromosom3TextBubble.activeSelf && chromosomeNumber == 3))
            {
                if (chromosomeNumber == 3)
                {
                    textComponent.color = Color.green;
                }
                else
                {
                    textComponent.color = Color.red;
                }
            }
            else if (reset)
            {
                //if (chromosomeNumber != 3)
                //{
                    textComponent.color = Color.white;
                //}
            }
            else
            {
                textComponent.color = Color.yellow;
            }
        }
    }

    private void castOutline_level3(string tag)
    {
        //bool isOutline = highlightedObject.name.Contains("Outline");
        if (tags.Contains(tag))
        {
            bool isCorrectChoice = false;

            highlightedObjects.Clear();
            
            if (tag.Contains("Chromosome"))
            {
                if (objectDictionary.TryGetValue(tag, out GameObject foundObject))
                {
                    highlightedObjects.Add(foundObject);
                }
                if (tag == "Chromosome3")
                {
                    isCorrectChoice = true;
                }
            }

            foreach (GameObject obj in highlightedObjects)
            {
                obj.SetActive(true);
                bool isTagged = clickedObjects.Contains(obj.tag);

                if (isTagged && (!changedObjects.Contains(obj.tag) || tag == "Chromosome3"))
                {
                    Renderer renderer = obj.GetComponent<Renderer>();
                    Material[] materials = renderer.materials;
                    if (materials.Length > 0)
                    {
                        Material clickMaterial = isCorrectChoice ? GreenMaterial : RedMaterial;
                        // Replace the last material with the new material
                        materials[materials.Length - 1] = isTagged ? clickMaterial : OutlineMaterial;

                        // Assign the updated materials array back to the renderer
                        renderer.materials = materials;
                    }

                    if (isTagged && !isCorrectChoice)
                    {
                        StatsScript.mistakesLevel3_1++;
                    }

                    changedObjects.Add(obj.tag);
                }
                /* Change text color */
                if (/*isTagged &&*/ obj.name.StartsWith("Chromosome"))
                {
                    int chromosomeNumber = int.Parse(obj.name.Substring("Chromosome".Length));
                    changeCanvasTextColor(chromosomeNumber, isTagged);
                    if (isCorrectChoice && isTagged)
                    {
                        if (!kromosom3TextBubble.activeSelf)
                        {
                            StatsScript.audioSource.Play();
                            kromosom3TextBubble.SetActive(true);
                            clickedObjects.Remove(tag);
                        }
                        else
                        {
                            StatsScript.updateStats(3);
                        }
                    }
                }
            }

            highlightedObject = highlightedObjects[0];
        }
    }

    private void ResetMaterial_level3(bool forceReset = false)
    {
        if (highlightedObject != null || forceReset)
        {
            if (!forceReset && !clickedObjects.Contains(highlightedObject.tag) && highlightedObject.name.StartsWith("Chromosome"))
            {
                int chromosomeNumber = int.Parse(highlightedObject.name.Substring("Chromosome".Length));
                changeCanvasTextColor(chromosomeNumber, false, true);
            }


            Chromosome1.SetActive(false);
            Chromosome2.SetActive(false);
            Chromosome3.SetActive(false);
            Chromosome4.SetActive(false);
            Chromosome5.SetActive(false);
            Chromosome6.SetActive(false);
            Chromosome7.SetActive(false);
            Chromosome8.SetActive(false);
            Chromosome9.SetActive(false);
            Chromosome10.SetActive(false);
            Chromosome11.SetActive(false);
            Chromosome12.SetActive(false);
            Chromosome13.SetActive(false);
            Chromosome14.SetActive(false);
            Chromosome15.SetActive(false);
            Chromosome16.SetActive(false);
            Chromosome17.SetActive(false);
            Chromosome18.SetActive(false);
            Chromosome19.SetActive(false);
            Chromosome20.SetActive(false);
            Chromosome21.SetActive(false);
            Chromosome22.SetActive(false);
            Chromosome23.SetActive(false);

            highlightedObject = null;
        }
    }

    public void resetLevel1()
    {
        ResetMaterial(true);
        clickedObjects = new List<string>();
        clickedObjectsNumber = 0;
        clickedGameObjects = new List<GameObject>();
        highlightedObjects = new List<GameObject>();
        tags = new string[] { "Animal", "Plant", "Bacteria", "Fungus" };
    }

    public void resetLevel3()
    {
        ResetMaterial_level3(true);
        highlightedObject = null;
        highlightedObjects = new List<GameObject>();

        tags = new string[] { "Chromosome1", "Chromosome2", "Chromosome3", "Chromosome4", "Chromosome5", "Chromosome6", "Chromosome7", "Chromosome8", "Chromosome9",
        "Chromosome10", "Chromosome11", "Chromosome12", "Chromosome13", "Chromosome14", "Chromosome15", "Chromosome16", "Chromosome17",
        "Chromosome18", "Chromosome19", "Chromosome20", "Chromosome21", "Chromosome22", "Chromosome23" };
        clickedObjects = new List<string>();
        clickedObjectsNumber = 0;
        clickedGameObjects = new List<GameObject>();
        kromosom3TextBubble.SetActive(false);

        for (int i=1; i<24; i++)
        {
            changeCanvasTextColor(i, false, true);
        }
    }
}