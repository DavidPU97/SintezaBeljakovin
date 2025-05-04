
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
//using TreeEditor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
//using UnityEngine.Windows;

public class MouseOverHighlighter4 : MonoBehaviour
{
    public Material OutlineMaterial;
    public Material OutlineMaterialThick;
    public Material clickedMaterial;
    public Material clickedMaterialIncorrect;

    public Material greyMaterial;
    public Material redMaterial;
    public Material yellowMaterial;
    public Material greenMaterial;
    public Material blueMaterial;
    public Material purpleMaterial;

    public Material adenine;
    public Material thymine;
    public Material cystosine;
    public Material guanine;

    public string selectedColor = "";
    public GameObject selectedColorPreviousFront;
    public GameObject selectedColorPreviousBack;
    public GameObject colorsFront;
    public GameObject colorsBack;
    private Material selectedMaterial;

    public GameObject helpRNA;
    public GameObject helpIntroni;
    public GameObject helpEksoni;
    public GameObject helpAdenine;
    public GameObject helpThymine;
    public GameObject helpCystosine;
    public GameObject helpGuanine;
    public GameObject helpUracil;
    public GameObject helpPore;
    public GameObject helpPore2;

    private GameObject helpBubble;

    private string[] tags = { "color", "dna", "NuclearEnvelope", "Animal" };

    public int?[] correctNucleotides = new int?[45];
    public int?[] correctNucleotidesRNA = new int?[45];
    public int incorrectNucleotides = 0;
    public int incorrectNucleotidesRNA = 0;

    public string forceUpdate;
    private GameObject highlightedObject = null; // Currently highlighted object
    private Material highlightedObjectMaterial; // Currently highlighted object
    public List<string> clickedObjects = new List<string>();
    public List<string> changedObjects = new List<string>();
    private int clickedObjectsNumber = 0;
    public List<GameObject> clickedGameObjects = new List<GameObject>();

    public GameObject Stats;
    private statsUpdate StatsScript;
    private string? currentIncorrectNucleotid;
    public bool isRNA = false;
    public bool isRNAFinished = false;
    private bool isKodogenSpiraleClicked = false;
    public bool ismRNASpiraleClicked = false;
    public Animator mRNASpiraleAnimator;
    public GameObject mRNASpirale;
    public Transform RightControllerTransform;

    void Start()
    {
        selectedColor = "Thymine";
        fillColor(selectedColor);
        StatsScript = Stats.GetComponent<statsUpdate>();
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
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Ray ray = new Ray(RightControllerTransform.position, RightControllerTransform.forward);
        RaycastHit hit;

        // Check if the ray hits an object
        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObject = hit.collider.gameObject;

            // If the object has changed or is new, update highlight
            //if ((((highlightedObject != hitObject) && (highlightedObject == null || highlightedObject.name != hitObject.name)) || clickedObjectsNumber != clickedObjects.Count) || forceUpdate)
            if (highlightedObject != hitObject || highlightedObject == null || forceUpdate != null)
            {
                bool DNAClick = forceUpdate == "dna";
                bool colorClick = forceUpdate == "color";
                bool spiralClick = forceUpdate == "NuclearEnvelope";
                bool holeClick = forceUpdate == "Animal";
                forceUpdate = null;
                if (colorClick)
                {

                    if (selectedColorPreviousFront != null) {
                        Renderer renderer = selectedColorPreviousFront.GetComponent<Renderer>();
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
                    if (selectedColorPreviousBack != null) {
                        Renderer renderer = selectedColorPreviousBack.GetComponent<Renderer>();
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
                clickedObjectsNumber = clickedObjects.Count;
                // Reset previous object material
                if (highlightedObject != null)
                {
                    ResetMaterial(DNAClick || colorClick || spiralClick || holeClick); // tukej nastavi material nazaj na sivega
                    //highlightedObject.SetActive(true);
                }

                // Set new object as highlighted and change its material
                highlightedObject = hitObject;
                //hitObject.SetActive(false);

                castOutline(hitObject.tag, DNAClick, spiralClick, holeClick);
            }
        }
        else if (highlightedObject != null)
        {
            // If no object is hit, reset the currently highlighted object
            ResetMaterial(false);
        }
        //}
    }

    private void ResetMaterial(bool isClick = false)
    {
        if (highlightedObject == null) {
            return;
        }

        if (highlightedObject.tag == "dna" && highlightedObjectMaterial != null)
        {
            string selectedNucleotid = highlightedObject.name;
            int dotIndex = selectedNucleotid.IndexOf('.');
            
            if (dotIndex != -1 && selectedMaterial != null)
            {
                string afterDot = selectedNucleotid.Substring(dotIndex + 1); // Get the part after the dot
                string number = afterDot.Substring(0, afterDot.Length - 1);
                int numberInt = int.Parse(number);
                
                if ((!isRNA && !Array.Exists(correctNucleotides, e => e == numberInt)) || (isRNA && !Array.Exists(correctNucleotidesRNA, e => e == numberInt)))
                {
                    bool isNucleotidEmpty = afterDot[afterDot.Length - 1] == 'B'; // Get the last character (A or B)

                    GameObject emptyNukleotid = highlightedObject;
                    if (!isNucleotidEmpty)
                    {
                        emptyNukleotid = GameObject.Find("DNA." + number + "B");
                    }
                    emptyNukleotid.GetComponent<Renderer>().material = highlightedObjectMaterial;
                }
            }
        }
        else if (highlightedObject.tag == "color" && selectedColor != highlightedObject.name.Replace("Front", "").Replace("Back", ""))
        {
            string currentColor = highlightedObject.name.Replace("Front", "").Replace("Back", "");
            GameObject selectedColorFront = colorsFront.transform.Find(currentColor + "Front").gameObject; //GameObject.Find(currentColor + "Front");
            GameObject selectedColorBack = colorsBack.transform.Find(currentColor + "Back").gameObject; // GameObject.Find(currentColor + "Back");

            if (selectedColorFront != null)
            {
                Renderer rendererFront = selectedColorFront.GetComponent<Renderer>();
                Material[] materialsFront = rendererFront.materials;
                if (materialsFront.Length == 2)
                {
                    //Material clickMaterial = isCorrectChoice ? GreenMaterial : (isAlmostCorrectChoice ? OrangeMaterial : RedMaterial);
                    // Replace the last material with the new material
                    Material[] newMaterialsFront = materialsFront.Take(materialsFront.Length - 1).ToArray();

                    // Assign the updated materials array back to the renderer
                    rendererFront.materials = newMaterialsFront;
                }
            }

            if (selectedColorBack != null)
            {
                Renderer rendererBack = selectedColorBack.GetComponent<Renderer>();
                Material[] materialsBack = rendererBack.materials;
                if (materialsBack.Length == 2)
                {
                    //Material clickMaterial = isCorrectChoice ? GreenMaterial : (isAlmostCorrectChoice ? OrangeMaterial : RedMaterial);
                    // Replace the last material with the new material
                    Material[] newMaterialsBack = materialsBack.Take(materialsBack.Length - 1).ToArray();

                    // Assign the updated materials array back to the renderer
                    rendererBack.materials = newMaterialsBack;
                }
            }
        }
        else if ((highlightedObject.tag == "NuclearEnvelope" || highlightedObject.tag == "Animal") 
            && !(highlightedObject.tag == "NuclearEnvelope" && isRNAFinished && (((highlightedObject.name.Contains("Red") || highlightedObject.name.Contains("RNA.")) && ismRNASpiraleClicked) || (highlightedObject.name.Contains("Kodogen") && isKodogenSpiraleClicked))))
        {
            GameObject tempGameObject = highlightedObject;
            if (highlightedObject.name.Contains("RNA."))
            {
                highlightedObject = mRNASpirale;
            }
            Renderer renderer = highlightedObject.GetComponent<Renderer>();
            Material[] materials = renderer.materials;
            if (materials.Length == 2)
            {
                Material[] newMaterials = materials.Take(materials.Length - 1).ToArray();
                renderer.materials = newMaterials;
            }
            highlightedObject = tempGameObject;
        }

        highlightedObjectMaterial = null;
        highlightedObject = null;
        if (helpBubble != null && isClick)
        {
            helpBubble.SetActive(false);
            helpBubble = null;
        }
    }

    private void castOutline(string tag, bool DNAClick = false, bool spiralClick = false, bool holeClick = false)
    {
        //bool isOutline = highlightedObject.name.Contains("Outline");
        if (tags.Contains(tag))
        {
            bool isTagged = clickedObjects.Contains(highlightedObject.name);

            if (tag == "dna")
            {
                string selectedNucleotid = highlightedObject.name;
                int dotIndex = selectedNucleotid.IndexOf('.');
                if (dotIndex != -1 && selectedMaterial != null)
                {
                    string afterDot = selectedNucleotid.Substring(dotIndex + 1); // Get the part after the dot
                    string number = afterDot.Substring(0, afterDot.Length - 1); // Extract the number
                    int numberInt = int.Parse(number);
                    bool isNucleotidEmpty = afterDot[afterDot.Length - 1] == 'B'; // Get the last character (A or B)
                    if ((!isRNA && Array.Exists(correctNucleotides, e => e == numberInt)) || (isRNA && Array.Exists(correctNucleotidesRNA, e => e == numberInt)))
                    {
                        return;
                    }
                    if (DNAClick && !StatsScript.preventReset)
                    {
                        bool isThymine = false;
                        Material materialToMatch = GameObject.Find("DNA." + number + "A").GetComponent<Renderer>().material;
                        if (AreMaterialsEqual(materialToMatch, greenMaterial))
                        {
                            if (selectedColor == "Thymine" && !isRNA)
                            {
                                colorDNA(isNucleotidEmpty, number, selectedMaterial, true, true);
                            }
                            else if (selectedColor == "Uracil" && isRNA)
                            {
                                colorDNA(isNucleotidEmpty, number, selectedMaterial, true, true);
                            }
                            else
                            {
                                colorDNA(isNucleotidEmpty, number, greyMaterial, true, false);
                                helpBubble = helpAdenine;
                                helpBubble.SetActive(true);
                            }
                        }
                        else if ((isThymine = AreMaterialsEqual(materialToMatch, redMaterial)) || AreMaterialsEqual(materialToMatch, purpleMaterial))
                        {
                            if (selectedColor == "Adenine")
                            {
                                colorDNA(isNucleotidEmpty, number, selectedMaterial, true, true);
                            }
                            else
                            {
                                colorDNA(isNucleotidEmpty, number, greyMaterial, true, false);
                                helpBubble = isThymine ? helpThymine : helpUracil;
                                helpBubble.SetActive(true);
                            }
                        }
                        else if (AreMaterialsEqual(materialToMatch, blueMaterial))
                        {
                            if (selectedColor == "Guanine")
                            {
                                colorDNA(isNucleotidEmpty, number, selectedMaterial, true, true);
                            }
                            else
                            {
                                colorDNA(isNucleotidEmpty, number, greyMaterial, true, false);
                                helpBubble = helpCystosine;
                                helpBubble.SetActive(true);
                            }
                        }
                        else if (AreMaterialsEqual(materialToMatch, yellowMaterial))
                        {
                            if (selectedColor == "Cystosine")
                            {
                                colorDNA(isNucleotidEmpty, number, selectedMaterial, true, true);
                            }
                            else
                            {
                                colorDNA(isNucleotidEmpty, number, greyMaterial, true, false);
                                helpBubble = helpGuanine;
                                helpBubble.SetActive(true);
                            }
                        }                        
                    }
                    else
                    {
                        // find and change color
                        colorDNA(isNucleotidEmpty, number, selectedMaterial, false, false);
                    }

                }
            }
            else if (tag == "color" /*&& selectedColor != ""*/)
            {
                fillColor(highlightedObject.name.Replace("Front", "").Replace("Back", ""));
            }
            else if (tag == "NuclearEnvelope")
            {
                GameObject tempGameObject = highlightedObject;
                if (highlightedObject.name.Contains("RNA."))
                {
                    highlightedObject = mRNASpirale;
                }

                Renderer rendererSpirale = highlightedObject.GetComponent<Renderer>();
                Material[] materialsSpirale = rendererSpirale.materials;
                if (materialsSpirale.Length == 1)
                {
                    // Replace the last material with the new material
                    Material[] newMaterialsFront = materialsSpirale.Concat(new Material[] { OutlineMaterial }).ToArray();

                    // Assign the updated materials array back to the renderer
                    rendererSpirale.materials = newMaterialsFront;
                }

                if (spiralClick && !StatsScript.preventReset)
                {
                    if (highlightedObject.name.Contains("Red") || highlightedObject.name.Contains("RNA."))
                    {
                        helpBubble = !isRNA ? helpIntroni : helpRNA;
                        if (!ismRNASpiraleClicked && isRNAFinished)
                        {
                            StatsScript.audioSource.Play();
                            StatsScript.correctLevel4_3++;
                            setClickedMaterial(highlightedObject, clickedMaterial);
                            ismRNASpiraleClicked = true;
                            mRNASpiraleAnimator.SetTrigger("LiftRNA");
                            helpBubble = null;

                            StartCoroutine(spinRNA());
                            StatsScript.updateStats(4);
                        }
                        else if (ismRNASpiraleClicked && isRNAFinished)
                        {
                            helpBubble = null;
                        }
                    }
                    else
                    {
                        helpBubble = helpEksoni;
                        if (!isKodogenSpiraleClicked && isRNAFinished && !ismRNASpiraleClicked)
                        {
                            setClickedMaterial(highlightedObject, clickedMaterialIncorrect);
                            isKodogenSpiraleClicked = true;
                            StatsScript.mistakesLevel4_3++;
                            StatsScript.updateStats(4);
                        }
                    }
                    
                    if (helpBubble != null)
                    {
                        helpBubble.SetActive(true);
                    }
                }
                highlightedObject = tempGameObject;
            }
            else if (tag == "Animal")
            {
                Renderer rendererHole = highlightedObject.GetComponent<Renderer>();
                Material[] materialsHole = rendererHole.materials;
                if (materialsHole.Length == 1)
                {
                    // Replace the last material with the new material
                    Material[] newMaterialsFront = materialsHole.Concat(new Material[] { OutlineMaterialThick }).ToArray();

                    // Assign the updated materials array back to the renderer
                    rendererHole.materials = newMaterialsFront;
                }

                if (holeClick)
                {
                    helpBubble = !isRNAFinished ? helpPore : (!ismRNASpiraleClicked ? helpPore2 : null);

                    if (helpBubble != null)
                    {
                        helpBubble.SetActive(true);
                    }
                    else if (StatsScript.correctLevel4_3 == 1)
                    {
                        StatsScript.correctLevel4_3++;
                        StatsScript.updateStats(4);
                    }
                }
            }
        }
    }

    bool AreMaterialsEqual(Material mat1, Material mat2)
    {
        if (mat1 == null || mat2 == null) return false;

        // Compare shaders
        if (mat1.shader != mat2.shader) return false;

        // Compare colors
        if (mat1.color != mat2.color) return false;

        // Compare textures
        if (mat1.mainTexture != mat2.mainTexture) return false;

        return true;
    }

    void colorDNA(bool isNucleotidEmpty, string number, Material setMaterial, bool clicked, bool isCorrect)
    {
        GameObject emptyNukleotid = highlightedObject;
        if (!isNucleotidEmpty)
        {
            emptyNukleotid = GameObject.Find("DNA." + number + "B");
        }
        if (clicked)
        {
            highlightedObjectMaterial = setMaterial;
        }
        else
        {
            highlightedObjectMaterial = greyMaterial;//emptyNukleotid.GetComponent<Renderer>().material;
        }
        emptyNukleotid.GetComponent<Renderer>().material = setMaterial;
        if (isCorrect)
        {
            StatsScript.audioSource.Play();
            int numberInt = int.Parse(number);
            emptyNukleotid.GetComponent<Collider>().enabled = false;
            GameObject.Find("DNA." + number + "A").GetComponent<Collider>().enabled = false;
            if (!isRNA)
            {
                correctNucleotides[numberInt - 1] = numberInt;
            }
            else
            {
                correctNucleotidesRNA[numberInt - 1] = numberInt;
            }
        }
        else if (clicked && currentIncorrectNucleotid != number)
        {
            currentIncorrectNucleotid = number;
            if (!isRNA)
            {
                incorrectNucleotides += 1;
            }
            else
            {
                incorrectNucleotidesRNA += 1;
            }
        }

        if (clicked)
        {
            StatsScript.updateStats(4);
        }
    }

    public void fillColor(string currentColor)
    {
        if (selectedColor == "Adenine")
        {
            selectedMaterial = greenMaterial;
        }
        else if (selectedColor == "Cystosine")
        {
            selectedMaterial = blueMaterial;
        }
        else if (selectedColor == "Guanine")
        {
            selectedMaterial = yellowMaterial;
        }
        else if (selectedColor == "Thymine")
        {
            selectedMaterial = redMaterial;
        }
        else if (selectedColor == "Uracil")
        {
            selectedMaterial = purpleMaterial;
        }

        Material newMaterial = selectedColor == currentColor ? clickedMaterial : OutlineMaterial;
        GameObject selectedColorFront = colorsFront.transform.Find(currentColor + "Front").gameObject; //GameObject.Find(currentColor + "Front");
        GameObject selectedColorBack = colorsBack.transform.Find(currentColor + "Back").gameObject; // GameObject.Find(currentColor + "Back");

        if (selectedColorFront != null)
        {
            setClickedMaterial(selectedColorFront, newMaterial);
        }

        if (selectedColorBack != null)
        {
            setClickedMaterial(selectedColorBack, newMaterial);
        }
    }

    void setClickedMaterial(GameObject objectToColor, Material colorMaterial)
    {
        Renderer rendererBack = objectToColor.GetComponent<Renderer>();
        Material[] materialsBack = rendererBack.materials;
        if (materialsBack.Length == 1)
        {
            // Replace the last material with the new material
            Material[] newMaterialsBack = materialsBack.Concat(new Material[] { colorMaterial }).ToArray();

            // Assign the updated materials array back to the renderer
            rendererBack.materials = newMaterialsBack;
        }
        else if (materialsBack.Length == 2)
        {
            // Replace the last material with the new material
            materialsBack[materialsBack.Length - 1] = colorMaterial;

            // Assign the updated materials array back to the renderer
            rendererBack.materials = materialsBack;
        }
    }

    private IEnumerator spinRNA()
    {
        float elapsedTime = 0f;
        float animationDuration = 3f;
        float maxSpeed = 100f;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;

            // Get the normalized time (0 to 1)
            float normalizedTime = elapsedTime / animationDuration;

            // Use a quadratic function to accelerate speed (e.g., y = x^2)
            float speedMultiplier = Mathf.Pow(normalizedTime, 2) * maxSpeed;

            // Apply the speed multiplier to the Animator
            //mRNASpiraleAnimator.speed = speedMultiplier;
            mRNASpiraleAnimator.SetFloat("SpeedRotate", speedMultiplier);

            yield return null;
        }
    }

    public void resetLevel4()
    {
        ResetMaterial();

        // remove outline from color balls
        string[] colorArray = { "Adenine", "Thymine", "Uracil", "Cystosine", "Guanine", "Speedup", "Speeddown" };
        for (int i = 0; i < colorArray.Length; i++) { 
            string currentColor = colorArray[i];
            GameObject selectedColorFront = colorsFront.transform.Find(currentColor + "Front").gameObject;
            GameObject selectedColorBack = colorsBack.transform.Find(currentColor + "Back").gameObject;

            if (selectedColorFront != null)
            {
                Renderer rendererFront = selectedColorFront.GetComponent<Renderer>();
                Material[] materialsFront = rendererFront.materials;
                if (materialsFront.Length == 2)
                {
                    Material[] newMaterialsFront = materialsFront.Take(materialsFront.Length - 1).ToArray();
                    rendererFront.materials = newMaterialsFront;
                }
            }

            if (selectedColorBack != null)
            {
                Renderer rendererBack = selectedColorBack.GetComponent<Renderer>();
                Material[] materialsBack = rendererBack.materials;
                if (materialsBack.Length == 2)
                {
                    Material[] newMaterialsBack = materialsBack.Take(materialsBack.Length - 1).ToArray();
                    rendererBack.materials = newMaterialsBack;
                }
            }
        }

        selectedColor = "Thymine";
        fillColor(selectedColor);

        selectedColorPreviousFront = null;
        selectedColorPreviousBack = null;
        if (helpBubble != null)
        {
            helpBubble.SetActive(false);
            helpBubble = null;
        }

        correctNucleotides = new int?[45];
        correctNucleotidesRNA = new int?[45];
        incorrectNucleotides = 0;
        incorrectNucleotidesRNA = 0;
        forceUpdate = null;
        highlightedObjectMaterial = null;

        highlightedObject = null;
        clickedObjects = new List<string>();
        changedObjects = new List<string>();
        clickedObjectsNumber = 0;
        clickedGameObjects = new List<GameObject>();

        mRNASpiraleAnimator.SetFloat("SpeedRotate", 1);
        mRNASpiraleAnimator.SetFloat("SpeedPore", 1);
        mRNASpiraleAnimator.SetFloat("SpeedLift", 1);
        mRNASpiraleAnimator.ResetTrigger("LiftRNA");
        mRNASpiraleAnimator.ResetTrigger("Pore");

        mRNASpiraleAnimator.Rebind();
        mRNASpiraleAnimator.Update(0f);

        if (ismRNASpiraleClicked)
        {
            Renderer mRNASpiraleRenderer = StatsScript.mRNASpirale.transform.Find("Red Spirale").GetComponent<Renderer>();
            Material[] mRNASpiraleMaterials = mRNASpiraleRenderer.materials;
            if (mRNASpiraleMaterials.Length == 2)
            {
                Material[] newMaterials = mRNASpiraleMaterials.Take(mRNASpiraleMaterials.Length - 1).ToArray();
                mRNASpiraleRenderer.materials = newMaterials;
            }
        }
        StatsScript.mRNASpirale.transform.localPosition = new Vector3(2.37f, -0.232f, 3.178f);
        if (isKodogenSpiraleClicked)
        {
            Renderer kodogenaSpiralaRenderer = StatsScript.kodogenaSpirala.transform.Find("Kodogen Spirale").GetComponent<Renderer>();
            Material[] kodogenaSpiralaMaterials = kodogenaSpiralaRenderer.materials;
            if (kodogenaSpiralaMaterials.Length == 2)
            {
                Material[] newMaterials = kodogenaSpiralaMaterials.Take(kodogenaSpiralaMaterials.Length - 1).ToArray();
                kodogenaSpiralaRenderer.materials = newMaterials;
            }
        }

        currentIncorrectNucleotid = null;
        isRNA = false;
        isRNAFinished = false;
        isKodogenSpiraleClicked = false;
        ismRNASpiraleClicked = false;
    }
}
