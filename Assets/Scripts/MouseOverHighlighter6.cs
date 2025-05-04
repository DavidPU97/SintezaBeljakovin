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
using static UnityEngine.UI.Image;
//using UnityEngine.Windows;

public class MouseOverHighlighter6 : MonoBehaviour
{
    public Material hoverMaterial;
    public Material clickMaterial;
    public Material incorrectMaterial;

    public Material highlightColor;

    public Transform diagramSelect;
    public Transform diagramTexts;

    private string[] tags = { "Chromosome10" };
    private List<TMP_Text> highlightedTexts = new List<TMP_Text>();

    private Color yellowColor = new Color(231f / 255f, 228f / 255f, 93f / 255f);
    private Color redColor = new Color(231f / 255f, 93f / 255f, 93f / 255f);
    private Color greenColor = new Color(95f / 255f, 231f / 255f, 93f / 255f);

    public List<GameObject> highlightedObjects = new List<GameObject>(); // Currently highlighted object
    public List<GameObject> hiddenObjects = new List<GameObject>();
    private GameObject highlightedObject;

    public GameObject Stats;
    private statsUpdate StatsScript;

    private string[] area1ToNucleotid = { "A", "G", "C", "U", "A", "G", "U", "U", "U", "A", "A", "U", "A", "U", "U", "U"};
    private string[] area2ToNucleotid = { "U", "C", "A", "C", "A", "A", "U", "U", "G", "A", "C", "U", "C", "C", "G", "G"};
    private string[] area3ToNucleotid = { "G", "CUGA", "AG", "CUGA", "AG", "CU", "CU", "CU", "G", "AG", "CUGA", "CU", "CUGA", "CUGA", "CU", "A"};
    private string[] aminoAcids = { "Met/Start", "Ala", "Gln", "Ser", "Lys", "Asp", "Phe", "Phe", "Trp", "Lys", "Thr", "Phe", "Thr", "Ser", "Cys", "Stop" };
    // private string[] aminoAcids = { "Val", "Ala", "Asp", "Glu", "Gly", "Phe", "Leu", "Ser", "Tyr", "Stop", "Stop", "Cys", "Stop", "Trp", "Leu", "Pro", "Hls", "Gln", "Arg", "Ile", "Met/Start", "Thr", "Asn", "Lys", "Ser", "Arg" };
    private string[] aminoAcidsAll = { "Valine", "Alanine", "Aspartic acid", "Glutamic acid", "Glycine", "Phenylalanine", "Leucine", "Serine", "Tyrosine", "Stop", "Stop", "Cysteine", "Stop", "Tryptophan", "Leucine", "Proline", "Histidine", "Glutamine", "Arginine", "Isoleucine", "Methionine/Start", "Threonine", "Asparagine", "Lysine", "Serine", "Arginine" };
    private string[] aminoAcidsAllPath = { "GUCUGA", "GCCUGA", "GACU", "GAAG", "GGCUGA", "UUCU", "UUAG", "UCCUGA", "UACU", "UAA", "UAG", "UGCU", "UGA", "UGG", "CUCUGA", "CCCUGA", "CACU", "CAAG", "CGCUGA", "AUCUA", "AUG", "ACCUGA", "AACU", "AAAG", "AGCU", "AGAG" };
    public int correctAminoAcids = 0;
    public int incorrectAminoAcids = 0;

    public GameObject mRna;
    private Vector3 mRnaStartPos;
    private Quaternion mRnaStartRot;
    public Transform mRnaNukleotidi;

    public Renderer tRna11;
    public Renderer tRna21;
    public Renderer tRna31;
    public Renderer tRna12;
    public Renderer tRna22;
    public Renderer tRna32;
    private string[] zapis_nucleotidov = {"AUG", "GCU", "CAA", "UCG", "AAG", "GAU", "UUC", "UUU", "UGG", "AAG", "ACA", "UUC", "ACC", "UCG", "UGU", "UGA"};
    private string[] correct_area_sequence = { "Q4.AUG3", "Q1.GCCUGA3", "Q3.CAAG3", "Q2.UCCUGA3", "Q4.AAAG3", "Q1.GACU3", "Q2.UUCU3", "Q2.UUCU3", "Q2.UGG3", "Q4.AAAG3", "Q4.ACCUGA3", "Q2.UUCU3", "Q4.ACCUGA3", "Q2.UCCUGA3", "Q2.UGCU3", "Q2.UGA3" };
    public Material mRnaYellow;
    public Material mRnaGreen;
    public Material mRnaBlue;
    public Material mRnaPurple;

    public Animator tRNAAnimator1;
    public Animator tRNAAnimator2;
    public GameObject tRNAGameObject1;
    public GameObject tRNAGameObject2;

    public Animator AminoAcidAnimator;
    public Animator somatostatinAnimator;
    public GameObject AminoAcidGameObject;
    public Transform aminoAcidGameObjects;
    private Vector3 AKstartPos;
    private Vector3[] AKSstartPos = new Vector3[15];
    private Quaternion[] AKSstartRot = new Quaternion[15];
    public GameObject proteinGameObject;

    public GameObject releaseFactor;
    public Animator releaseFactorAnimator;

    public TextMeshProUGUI diagramText;
    public TextMeshProUGUI kodonText;
    public GameObject metioninTextObj;
    public GameObject kodonTextObj;
    public GameObject tRNATextObj;
    public TextMeshProUGUI tRNATextBoxTitle;
    public TextMeshProUGUI tRNATextBox;

    public bool isAnyRegionAlreadyClicked = false;
    public bool isAnyRegionAlreadyHovered = false;
    public bool isAnyRegionActive = false;
    public bool isAnyRegionColored = false;
    public string coloredRegion = null;


    public bool playContinuesly = false;

    void Start()
    {
        StatsScript = Stats.GetComponent<statsUpdate>();
        targetRotations = new Quaternion[targetRotationsX.Length];

        for (int i = 0; i < targetRotationsX.Length; i++)
        {
            targetRotations[i] = Quaternion.AngleAxis(targetRotationsX[i], Vector3.right);
        }

        //tRNAAnimator1.SetTrigger("initTRNA2");
        SetChildrenRenderersVisible(tRNAGameObject2.transform, false);
        SetChildrenRenderersVisible(releaseFactor.transform, false);

        mRnaStartPos = mRna.transform.localPosition;
        mRnaStartRot = mRna.transform.localRotation;
        AKstartPos = aminoAcidGameObjects.localPosition;

        
        for (int i = 1; i <= 15; i++)
        {
            Transform AKStart = aminoAcidGameObjects.transform.Find("AA" + (i));
            AKSstartPos[i-1] = AKStart.localPosition;
            AKSstartRot[i-1] = AKStart.localRotation;
        }

        //StatsScript.currentLevel = 6;
    }
    //private float timer = 0f;
    //private float updateDelay = 1f;

    private void Update()
    {
        /*if (Input.GetKeyDown("enter"))
        {
            StatsScript.restartSimulation();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            playContinuesly = !playContinuesly;
        }*/
        if (playContinuesly)
        {
            if (correctAminoAcids == 16 || isAnimating || isAnimating2)
            {
                return;
            }
            colorHighlightedAreas(correct_area_sequence[correctAminoAcids]);
        }
    }

    //void Update()
    //{
    //    /*if (Input.GetKeyDown("space") && !isAnimating && correctAminoAcids < 17)
    //    {
    //       correctAminoAcids++;
    //        StartCoroutine(SmoothTransition());
    //    }*/

    //    /*timer += Time.deltaTime;

    //    if (timer >= updateDelay)
    //    {
    //        timer = 0f;*/

    //    // Cast a ray from the camera to the mouse position
    //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //    RaycastHit hit;


    //    // Check if the ray hits an object
    //    if (Physics.Raycast(ray, out hit))
    //    {
    //        GameObject hitObject = hit.collider.gameObject;

    //        // If the object has changed or is new, update highlight
    //        //if ((((highlightedObject != hitObject) && (highlightedObject == null || highlightedObject.name != hitObject.name)) || clickedObjectsNumber != clickedObjects.Count) || forceUpdate)
    //        if (highlightedObject != hitObject || highlightedObject == null)
    //        {

    //            // Reset previous object material
    //            if (highlightedObject != null)
    //            {
    //                ResetMaterial(); // tukej nastavi material nazaj na sivega
    //                //highlightedObject.SetActive(true);
    //            }

    //            // Set new object as highlighted and change its material
    //            highlightedObject = hitObject;
    //            //hitObject.SetActive(false);

    //            castOutline(hitObject.tag);
    //        }
    //    }
    //    else if (highlightedObject != null)
    //    {
    //        // If no object is hit, reset the currently highlighted object
    //        ResetMaterial(false);
    //    }
    //    //}
    //}

    //private void ResetMaterial(bool isClick = false)
    //{
    //    highlightedObject = null;
    //}

    //private void castOutline(string tag)
    //{
    //    //bool isOutline = highlightedObject.name.Contains("Outline");
    //    if (tags.Contains(tag))
    //    {

    //    }
    //}

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

    public void hoverHighlight(string name = "")
    {
        if (name != "")
        {
            float angleZ = findAngle(name);
            Vector3 eulerRotation = transform.eulerAngles;
            diagramSelect.rotation = Quaternion.Euler(diagramSelect.eulerAngles.x, diagramSelect.eulerAngles.y, angleZ);
        }

        foreach (GameObject obj in highlightedObjects) { 
            obj.SetActive(true);
        }

        highlightText(name);
    }

    public void resetAreas()
    {
        foreach (GameObject obj in highlightedObjects)
        {
            obj.transform.GetChild(0).GetComponent<Renderer>().material = hoverMaterial;
            obj.SetActive(false);
        }
        highlightedObjects = new List<GameObject>();

        if (highlightedTexts.Count > 0)
        {
            foreach (TMP_Text highlightedText in highlightedTexts)
            {
                highlightedText.color = Color.black;
            }
            highlightedTexts = new List<TMP_Text>();
        }
    }
    
    public float findAngle(string name)
    {
        int areaIndex = name.IndexOf("Q");

        if (areaIndex != -1)
        {
            int number = int.Parse(name.Substring(areaIndex + 1, areaIndex + 1));
            
            return (number - 1) * 90 * -1;
        }
        return 0;
    }

    // ta funkcija mora gledati ali je prov ali narobe
    public void colorHighlightedAreas(string name)
    {
        if (correctAminoAcids == 16)
        {
            return;
        }

        int dotIndex = name.IndexOf(".");
        int level = int.Parse(name.Substring(name.Length - 1, 1));
        string selectedArea1 = name.Substring(dotIndex + 1, 1);
        bool is_incorrect = selectedArea1 != area1ToNucleotid[correctAminoAcids];

        if (level == 1)
        {
            if (is_incorrect)
            {
                incorrectAminoAcids++;
                colorHighlightMaterial(incorrectMaterial, false, true, selectedArea1, "", "");

                StatsScript.mistakesLevel6++;
            }
            else
            {
                colorHighlightMaterial(clickMaterial, true, false, selectedArea1, "", "");
            }
            return;
        }

        string selectedArea2 = name.Substring(dotIndex + 2, 1);
        is_incorrect = is_incorrect || selectedArea2 != area2ToNucleotid[correctAminoAcids];
        if (level == 2)
        {
            if (is_incorrect)
            {
                incorrectAminoAcids++;
                colorHighlightMaterial(incorrectMaterial, false, true, selectedArea1, selectedArea2, "");
                
                StatsScript.mistakesLevel6++;
            }
            else
            {
                colorHighlightMaterial(clickMaterial, true, false, selectedArea1, selectedArea2, "");
            }
            return;
        }


        string selectedArea3 = name.Substring(dotIndex + 3, name.Length - 6);
        is_incorrect = is_incorrect || selectedArea3 != area3ToNucleotid[correctAminoAcids];

        if (level == 3)
        {
            if (is_incorrect)
            {
                incorrectAminoAcids++;
                colorHighlightMaterial(incorrectMaterial, false, true, selectedArea1, selectedArea2, selectedArea3);

                StatsScript.mistakesLevel6++;
            }
            else
            {
                StatsScript.audioSource.Play();
                correctAminoAcids++;
                colorHighlightMaterial(clickMaterial, true, true, selectedArea1, selectedArea2, selectedArea3);

                if (!isAnimating && !isAnimating2 && correctAminoAcids < 16)
                {
                    startTRNA();
                }
                else if (!isAnimating && !isAnimating2 && correctAminoAcids == 16)
                {
                    releaseFactorAnimator.SetTrigger("EndReleaseFactor");
                    StatsScript.timeLevel6End = Time.time - StatsScript.timeLevel6Start;
                    StatsScript.timeLevel6String = StatsScript.setTimeString(StatsScript.timeLevel6End);

                    MoveAKDown();
                }
            }
            return;
        }
    }

    void colorHighlightMaterial(Material materialToColorWith, bool isCorrect, bool colorAK = false, string area1 = "", string area2 = "", string area3 = "")
    {
        foreach (GameObject obj in highlightedObjects)
        {
            obj.transform.GetChild(0).GetComponent<Renderer>().material = materialToColorWith;
        }

        if (colorAK)
        {
            foreach (TMP_Text highlightedText in highlightedTexts)
            {
                highlightedText.color = !isCorrect ? redColor : greenColor;            
            }
        }

        string diagram_text_builder = area1;
        if (area2 != "")
        {
            diagram_text_builder += "-" + area2;
        }
        if (area3 != "")
        {
            int index = Array.IndexOf(aminoAcidsAllPath, area1+area2+area3);
            area3 = string.Join("/", area3.ToCharArray());
            diagram_text_builder += "-" + area3;

            if (index != -1)
            {
                diagram_text_builder += " = " + aminoAcidsAll[index];
            }
        }

        diagramText.text = diagram_text_builder;
        diagramText.color = !isCorrect ? redColor : greenColor;
    }

    void highlightText(string name)
    {
        int level = int.Parse(name.Substring(name.Length - 1, 1));
        if (level != 3)
        {
            return;
        }

        int dotIndex = name.IndexOf(".");
        string selectedArea1 = name.Substring(dotIndex + 1, 1);
        string selectedArea2 = name.Substring(dotIndex + 2, 1);
        string selectedArea3 = name.Substring(dotIndex + 3, name.Length - 6);
        foreach (Transform child in diagramTexts)
        {
            if (child.gameObject.name == selectedArea1 + selectedArea2 + selectedArea3)
            {
                TMP_Text highlightedText = child.GetComponent<TMP_Text>();
                highlightedText.color = yellowColor;
                highlightedTexts.Add(highlightedText);
            }
        }
    }

    public bool isAnimating = false;
    public bool isAnimating2 = false;
    public float transitionDuration = 2f;

    public float[] targetRotationsX = new float[16];
    private Quaternion[] targetRotations;
    public float[] targetPositionsZ = new float[16];

    IEnumerator SmoothTransition()
    {
        isAnimating2 = true;
        // start tRNA animation        
        tRNAAnimator1.SetInteger("numberOfAKs", correctAminoAcids);
        tRNAAnimator2.SetInteger("numberOfAKs", correctAminoAcids);
        tRNAAnimator1.ResetTrigger("initTRNA2");

        // remove highlight from previous
        for (int i = 0; i < 3; i++)
        {

            Renderer nucleotideToUnhighlight = mRnaNukleotidi.Find("RNA." + ((correctAminoAcids * 3) - i)).GetComponent<Renderer>();
            Material[] materialsBack = nucleotideToUnhighlight.materials;
            if (materialsBack.Length == 2)
            {
                Material[] newMaterials = materialsBack.Take(materialsBack.Length - 1).ToArray();
                // Assign the updated materials array back to the renderer
                nucleotideToUnhighlight.materials = newMaterials;
            }
        }

        kodonText.text = "";

        float elapsedTime = 0f;

        // Store current state
        Quaternion startRotation = mRna.transform.localRotation;
        Quaternion endRotation = startRotation * targetRotations[correctAminoAcids];

        Vector3 startPosition = mRna.transform.localPosition;
        Vector3 endPosition = new Vector3(mRna.transform.localPosition.x, mRna.transform.localPosition.y, targetPositionsZ[correctAminoAcids]);

        while (elapsedTime < transitionDuration)
        {
            float t = elapsedTime / transitionDuration;
            mRna.transform.localRotation = Quaternion.Slerp(startRotation, endRotation, t); // Smooth rotation
            mRna.transform.localPosition = Vector3.Lerp(startPosition, endPosition, t); // Smooth movement

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final values are set
        mRna.transform.localRotation = endRotation;
        mRna.transform.localPosition = endPosition;

        resetAreas();

        // set highlight to current
        for (int i = 1; i <= 3; i++)
        {
            Renderer nucleotideToHighlight = mRnaNukleotidi.Find("RNA." + ((correctAminoAcids * 3) + i)).GetComponent<Renderer>();
            Material[] materialsBack = nucleotideToHighlight.materials;
            Material[] newMaterialsBack = materialsBack.Concat(new Material[] { highlightColor }).ToArray();

            // Assign the updated materials array back to the renderer
            nucleotideToHighlight.materials = newMaterialsBack;
        }

        kodonText.text = zapis_nucleotidov[correctAminoAcids][0] + "   " + zapis_nucleotidov[correctAminoAcids][1] + "   " + zapis_nucleotidov[correctAminoAcids][2];
        isAnimating2 = false;
    }

    Material findMaterialNucleotid(char color)
    {
        if (color == 'U')
        {
            return mRnaGreen;
        }
        else if (color == 'A')
        {
            return mRnaPurple;
        }
        else if (color == 'G')
        {
            return mRnaBlue;
        }
        else if (color == 'C')
        {
            return mRnaYellow;
        }
        else
        {
            return null;
        }
    }

    public void TRNAAnimationEnd()
    {
        tRNAAnimator1.ResetTrigger("resetTRNA");
        tRNAAnimator2.ResetTrigger("resetTRNA");
    }
    
    public void SomatostatinAnimationEnd()
    {
        // remove highlight from previous
        for (int i = 0; i < 3; i++)
        {
            Renderer nucleotideToUnhighlight = mRnaNukleotidi.Find("RNA." + (48 - i)).GetComponent<Renderer>();
            Material[] materialsBack = nucleotideToUnhighlight.materials;
            if (materialsBack.Length == 2)
            {
                Material[] newMaterials = materialsBack.Take(materialsBack.Length - 1).ToArray();
                // Assign the updated materials array back to the renderer
                nucleotideToUnhighlight.materials = newMaterials;
            }
        }
    }

    public void moveAminoAcid() {
        if (correctAminoAcids < 15)
        {
            aminoAcidGameObjects.transform.Find("AA"+(correctAminoAcids)).gameObject.SetActive(true);
            AminoAcidAnimator.SetTrigger("AminoAcidTrigger");
        }
    }

    public void resetAAAnimation()
    {
        if (correctAminoAcids < 15)
        {
            aminoAcidGameObjects.transform.Find("AA" + (correctAminoAcids+1)).gameObject.SetActive(true);
            AminoAcidAnimator.ResetTrigger("AminoAcidTrigger");
        }
    }

    public void showSomatostatin()
    {
        for (int i = 1; i <= 15; i++)
        {
            Transform AK_to_reset = aminoAcidGameObjects.Find("AA" + (i));
            AK_to_reset.gameObject.SetActive(false);
        }
        //AminoAcidGameObject.SetActive(false);
        proteinGameObject.SetActive(true);
        //somatostatinAnimator.SetTrigger("startSomatostatin");
    }

    public void colorTRNA()
    {
        if (correctAminoAcids + 1 < 16)
        {
            // change tRNA colors
            char color1 = zapis_nucleotidov[correctAminoAcids+1][0];
            char color2 = zapis_nucleotidov[correctAminoAcids+1][1];
            char color3 = zapis_nucleotidov[correctAminoAcids+1][2];

            bool isTRNA2New = correctAminoAcids % 2 != 0;
            if (isTRNA2New)
            {
                tRna11.material = findMaterialNucleotid(color1);
                tRna21.material = findMaterialNucleotid(color2);
                tRna31.material = findMaterialNucleotid(color3);
            }
            else
            {
                tRna12.material = findMaterialNucleotid(color1);
                tRna22.material = findMaterialNucleotid(color2);
                tRna32.material = findMaterialNucleotid(color3);
            }
        }

        tRNAAnimator1.ResetTrigger("sendTRNA");
        tRNAAnimator2.ResetTrigger("sendTRNA");

        isAnimating = false;
    }

    public float moveDistanceY = -0.863f; // Move down by 2 units
    public float moveDistanceZ = 0.8f; // Move down by 2 units
    public float moveDuration = 3f; // Move duration

    void MoveAKDown()
    {
        StartCoroutine(MoveDownThenPlayAnimation());
    }

    IEnumerator MoveDownThenPlayAnimation()
    {
        isAnimating = true;
        //AKstartPos = aminoAcidGameObjects.localPosition;
        Vector3 targetPos = AKstartPos + new Vector3(0, moveDistanceY, moveDistanceZ);
        float elapsedTime = 0f;

        // Smoothly move the object down
        while (elapsedTime < moveDuration)
        {
            aminoAcidGameObjects.localPosition = Vector3.Lerp(AKstartPos, targetPos, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        aminoAcidGameObjects.localPosition = targetPos; // Ensure it reaches the exact position

        // Start the animation after moving down
        AminoAcidAnimator.SetTrigger("AKProteinTrigger");
        metioninTextObj.SetActive(true);
        kodonTextObj.SetActive(false);
        tRNATextObj.SetActive(false);
        SomatostatinAnimationEnd();
        isAnimating = false;
    }

    public void moveOldTRNA()
    {        
        bool isTRNA2New = correctAminoAcids % 2 != 0;
        if (isTRNA2New)
        {
            //SetChildrenRenderersVisible(tRNAGameObject2.transform, false);
        }
        else
        {
            //SetChildrenRenderersVisible(tRNAGameObject2.transform, false);
        }
    }

    void SetChildrenRenderersVisible(Transform parent, bool visible)
    {
        foreach (Renderer renderer in parent.GetComponentsInChildren<Renderer>())
        {
            renderer.enabled = visible; // Hide or Show all child renderers
        }
    }

    public void sendTRNA()
    {
        bool isTRNA2New = correctAminoAcids % 2 != 0;
        if (isTRNA2New)
        {
            tRNAAnimator1.SetTrigger("sendTRNA");
        }
        else
        {
            tRNAAnimator2.SetTrigger("sendTRNA");
        }
    }

    public void startTRNA()
    {
        isAnimating = true;


        if (correctAminoAcids == 15)
        {
            startReleaseFactor();
        }
        else
        {
            bool isTRNA2New = correctAminoAcids % 2 != 0;

            if (isTRNA2New)
            {
                tRNAAnimator2.SetTrigger("resetTRNA");
            }
            else
            {
                tRNAAnimator1.SetTrigger("resetTRNA");
            }
        }
    }

    public void startReleaseFactor()
    {
        //SetChildrenRenderersVisible(releaseFactor.transform, true);
        releaseFactorAnimator.SetTrigger("ReleaseReleaseFactor");
    }

    public void startmRNA()
    {
        StartCoroutine(SmoothTransition());
    }

    public void resetLevel6()
    {
        resetAreas();

        highlightedObjects = new List<GameObject>();
        hiddenObjects = new List<GameObject>();
        highlightedObject = null;
        
        correctAminoAcids = 0;
        incorrectAminoAcids = 0;

        tRna11.material = findMaterialNucleotid('U');
        tRna21.material = findMaterialNucleotid('A');
        tRna31.material = findMaterialNucleotid('C');
        tRna12.material = findMaterialNucleotid('C');
        tRna22.material = findMaterialNucleotid('G');
        tRna32.material = findMaterialNucleotid('A');

        AminoAcidGameObject.SetActive(true);
        proteinGameObject.SetActive(true);
        kodonTextObj.SetActive(true);
        tRNATextObj.SetActive(true);

        diagramText.text = "Tabela genskega koda";
        diagramText.color = Color.white;
        isAnimating = false;
        isAnimating2 = false;
        playContinuesly = false;

        AminoAcidGameObject.GetComponent<Animator>().enabled = false;
        //AKstartPos = new Vector3(-1.026f, -1.737f, -15.457f);
        AminoAcidGameObject.transform.localPosition = new Vector3(-1.026f, -1.737f, -15.457f);
        for (int i = 1; i <= 15; i++)
        {
            Transform AK_to_reset = aminoAcidGameObjects.Find("AA" + (i));
            AK_to_reset.localPosition = AKSstartPos[i - 1];
            AK_to_reset.localRotation = AKSstartRot[i - 1];

            AK_to_reset.gameObject.SetActive(i == 1);
            if (i == 1)
            {
                AK_to_reset.Find("AK model copy").GetComponent<Renderer>().enabled = true;
            }
        }

        // remove highlight from all (any)
        for (int i = 1; i <= 48; i++)
        {
            Renderer nucleotideToUnhighlight = mRnaNukleotidi.Find("RNA." + i).GetComponent<Renderer>();
            Material[] materialsBack = nucleotideToUnhighlight.materials;
            if (materialsBack.Length == 2)
            {
                Material[] newMaterials = materialsBack.Take(materialsBack.Length - 1).ToArray();
                // Assign the updated materials array back to the renderer
                nucleotideToUnhighlight.materials = newMaterials;
            }
        }

        // highlight first three
        for (int i = 1; i <= 3; i++)
        {
            Renderer nucleotideToHighlight = mRnaNukleotidi.Find("RNA." + i).GetComponent<Renderer>();
            Material[] materialsBack = nucleotideToHighlight.materials;
            Material[] newMaterialsBack = materialsBack.Concat(new Material[] { highlightColor }).ToArray();

            // Assign the updated materials array back to the renderer
            nucleotideToHighlight.materials = newMaterialsBack;
        }

        tRNAAnimator1.Rebind();
        tRNAAnimator1.Update(0f);
        tRNAAnimator2.Rebind();
        tRNAAnimator2.Update(0f);

        tRNAGameObject1.transform.localPosition = new Vector3(-0.007f, 0.644f, -1.685f);
        tRNAGameObject2.transform.localPosition = new Vector3(-0.007f, 2.135f, 6.098f);

        AminoAcidGameObject.GetComponent<Animator>().enabled = true;

        //AminoAcidAnimator.enabled = true;
        AminoAcidAnimator.Rebind();
        AminoAcidAnimator.Update(0f);
        somatostatinAnimator.Rebind();
        releaseFactorAnimator.Rebind();
        somatostatinAnimator.Update(0f);
        releaseFactorAnimator.Update(0f);

        tRNAAnimator1.ResetTrigger("resetTRNA");
        tRNAAnimator2.ResetTrigger("resetTRNA");
        tRNAAnimator1.ResetTrigger("sendTRNA");
        tRNAAnimator2.ResetTrigger("sendTRNA");

        releaseFactorAnimator.ResetTrigger("ReleaseReleaseFactor");
        releaseFactorAnimator.ResetTrigger("EndReleaseFactor");
        AminoAcidAnimator.ResetTrigger("AminoAcidTrigger");
        AminoAcidAnimator.ResetTrigger("AKProteinTrigger");
        //somatostatinAnimator.ResetTrigger("startSomatostatin");

        SetChildrenRenderersVisible(tRNAGameObject1.transform, true);
        SetChildrenRenderersVisible(tRNAGameObject2.transform, false);
        SetChildrenRenderersVisible(releaseFactor.transform, false);
        tRNAGameObject1.transform.Find("AminoAcid").GetComponent<Renderer>().enabled = false;

        mRna.transform.localPosition = mRnaStartPos;
        mRna.transform.localRotation = mRnaStartRot;

        tRNAAnimator1.SetInteger("numberOfAKs", 0);
        tRNAAnimator2.SetInteger("numberOfAKs", 0);
        //tRNAAnimator1.SetTrigger("initTRNA2");

        Renderer proteinGameObject_renderer = proteinGameObject.GetComponent<Renderer>();
        Material[] proteinGameObject_materials = proteinGameObject_renderer.materials;
        if (proteinGameObject_materials.Length == 2)
        {
            // Replace the last material with the new material
            Material[] proteinGameObject_newMaterials = proteinGameObject_materials.Take(proteinGameObject_materials.Length - 1).ToArray();

            // Assign the updated materials array back to the renderer
            proteinGameObject_renderer.materials = proteinGameObject_newMaterials;
        }

        proteinGameObject.SetActive(false);
        metioninTextObj.SetActive(false);

        kodonText.text = "A   U   G";
        tRNATextBoxTitle.text = "tRNA";
        tRNATextBox.text = "je molekula RNA, ki prenaša aminokisline do ribosoma in sodeluje pri sintezi beljakovin z ustreznim prepoznavanjem kodonov na mRNA.";
        tRNATextObj.transform.localPosition = new Vector3(-0.179f, 1.047f, -1.014696f);
    }
}