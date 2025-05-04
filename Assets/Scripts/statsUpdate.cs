using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
//using GLTF.Schema;
//using UnityEditor.ShaderGraph;
using UnityEngine.Rendering.Universal;
using System.IO;
using System;
using System.Net.NetworkInformation;
using System.Text;
using UnityEngine.ProBuilder.MeshOperations;
//using TreeEditor;
using UnityEngine.XR.OpenXR;
using UnityEngine.XR.OpenXR.Features.Meta;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using System.Threading.Tasks;
using UnityEngine.XR.ARCore;
using UnityEngine.Networking;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit.Samples.SpatialKeyboard;


public class statsUpdate : MonoBehaviour
{
    public int currentLevel = 1;
    public AudioSource audioSource;
    public bool preventReset = false;
    public Transform XRPlayer;
    public ARCameraManager passthrough;
    public string playerName = "";


    [Header("Timers")]
    public float timeLevel1Start;
    public float timeLevel1End;
    public string timeLevel1String;
    public float timeLevel2Start;
    public float timeLevel2End;
    public string timeLevel2String;
    public float timeLevel3Start;
    public float timeLevel3End;
    public string timeLevel3String;
    public float timeLevel4Start;
    public float timeLevel4End;
    public string timeLevel4String;
    public float timeLevel5Start;
    public float timeLevel5End;
    public string timeLevel5String;
    public float timeLevel6Start;
    public float timeLevel6End;
    public string timeLevel6String;
    public string timeTotalString;

    [Header("Mistakes")]
    public int mistakesLevel1 = 0;
    public int mistakesLevel2 = 0;
    public int mistakesLevel3 = 0;
    public int mistakesLevel3_1 = 0;
    public int mistakesLevel3_2 = 0;
    public int mistakesLevel4 = 0;
    public int mistakesLevel4_1 = 0;
    public int mistakesLevel4_2 = 0;
    public int mistakesLevel4_3 = 0;
    public int correctLevel4_3 = 0;
    public int mistakesLevel5 = 0;
    public int mistakesLevel6 = 0;

    [Header("Teleports")]
    private int teleportsLevel1 = 0;
    private int teleportsLevel2 = 0;
    private int teleportsLevel3 = 0;
    private int teleportsLevel4 = 0;
    private int teleportsLevel5 = 0;
    private int teleportsLevel6 = 0;

    [Header("Level 0")]
    public GameObject level0;
    public TextMeshProUGUI uiText0;
    public TextMeshProUGUI keyboardText;
    public XRKeyboard XRKeyboard;

    [Header("Level 1")]
    public GameObject level1;
    public GameObject level1Camera;
    public GameObject level1Player;
    public TextMeshProUGUI uiText1;

    [Header("Level 2")] 
    public GameObject level2;
    public GameObject level2Camera;
    public GameObject level2Player;
    public TextMeshProUGUI uiText2;

    [Header("Level 3")]
    public GameObject level3;
    //public GameObject level3Camera;
    public GameObject level3Player;
    public TextMeshProUGUI uiText3;
    public GameObject kromosomiObject;
    public GameObject ideogramObject;
    public GameObject rightWall;
    public GameObject Cells;
    public Transform Eksocitoza;
    public Transform Tabla1;
    public Transform Tabla2;

    [Header("Level 4")]
    public GameObject level4;
    public GameObject level4Camera;
    public GameObject level4Player;
    public TextMeshProUGUI uiNavodilaTitle4;
    public TextMeshProUGUI uiNavodila4;
    public TextMeshProUGUI uiText4;
    public GameObject DNASpirale;
    public GameObject RNASpirale;
    public GameObject mRNASpirale;
    public GameObject kodogenaSpirala;
    public GameObject ThymineFront;
    public GameObject ThymineBack;
    public GameObject UracilFront;
    public GameObject UracilBack;
    public GameObject SpeedFront;
    public GameObject SpeedBack;
    public GameObject SlowFront;
    public GameObject SlowBack;
    public GameObject ControlsFront;
    public GameObject ControlsBack;
    public Material greyMaterial;
    public bool skipLevel4 = false;

    [Header("Level 5")]
    public GameObject level5;
    public TextMeshProUGUI uiNavodila5;
    public TextMeshProUGUI uiText5;
    public GameObject level5Camera;
    public GameObject level5Player;
    public GameObject mRNALevel5;
    public GameObject jedroLevel2;
    public GameObject jedroLevel5;

    [Header("Level 6")]
    public GameObject level6;
    public GameObject level6Camera;
    public GameObject level6Player;
    public AudioSource finalAudioSource;

    [Header("End")]
    public GameObject endScreen;
    public TextMeshProUGUI[] levelTimeTexts; // Assign 6 Texts for level times
    public TextMeshProUGUI totalTimeText;    // Total time display
    public TextMeshProUGUI[] levelMistakeTexts; // Assign 6 Texts for mistakes per level
    public TextMeshProUGUI totalMistakesText;   // Total mistakes display

    [Header("Scripts")]
    private MouseOverHighlighter mouseOverScript1;
    private MouseOverHighlighter2 mouseOverScript2;
    //private MouseOverHighlighter mouseOverScript3;
    private OptimizedSliceHighlighter ideogramScript3;
    private MouseOverHighlighter4 mouseOverScript4;
    private MouseOverHighlighter2 mouseOverScript5;
    private MouseOverHighlighter6 mouseOverScript6;

    // Start is called before the first frame update
    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
        if (finalAudioSource == null)
        {
            finalAudioSource = GetComponent<AudioSource>();
        }

        mouseOverScript1 = level1Camera.GetComponent<MouseOverHighlighter>();
        mouseOverScript2 = level2Camera.GetComponent<MouseOverHighlighter2>();
        //mouseOverScript3 = level3Camera.GetComponent<MouseOverHighlighter>();
        ideogramScript3 = ideogramObject.transform.Find("IdeogramPicture").GetComponent<OptimizedSliceHighlighter>();
        mouseOverScript4 = level4Camera.GetComponent<MouseOverHighlighter4>();
        mouseOverScript5 = level5Camera.GetComponent<MouseOverHighlighter2>();
        mouseOverScript6 = level6Camera.GetComponent<MouseOverHighlighter6>();

        //timeLevel1Start = Time.time;
        currentLevel = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateStats(int level, bool startNewLevel = false)
    {
        if (level == 0)
        {
            passthrough.enabled = false;
            currentLevel = 1;
            level0.SetActive(false);
            level1.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            timeLevel1Start = Time.time;
            XRPlayer.transform.localPosition = new Vector3(0f, 0f, 0f);
            XRPlayer.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
        }
        else if (level == 1)
        {
            currentLevel = 2;
            level1.SetActive(false);
            level2.SetActive(true);
            timeLevel2Start = Time.time;
            XRPlayer.transform.localPosition = new Vector3(-2.25f, 0f, -5f);
            XRPlayer.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else if (level == 2)
        {
            List<string> clickedObjects = mouseOverScript2.clickedObjects;
            int correctAnswers = 0;
            mistakesLevel2 = 0;
            foreach (string clickedObject in clickedObjects)
            {
                if (clickedObject == "Ribosom2" || clickedObject == "Nucleolus")
                {
                    correctAnswers++;
                }
                else
                {
                    mistakesLevel2++;
                }
            }

            uiText2.text = "Pravilno:  " + correctAnswers + "/2\r\nNapačno: " + mistakesLevel2 + "/7";

            if (correctAnswers == 2 && startNewLevel)
            {
                StartCoroutine(WaitForABit());
            }
        }
        else if (level == 3)
        {
            StartCoroutine(WaitForABit(startNewLevel));
        }
        else if (level == 4) {
            if (DNASpirale.activeSelf == true)
            {
                int correctAnswers = mouseOverScript4.correctNucleotides.Count(n => n.HasValue);

                uiText4.text = "Pravilno:  " + correctAnswers + "/45\r\nNapačno: " + mouseOverScript4.incorrectNucleotides + "";
                mistakesLevel4_1 = mouseOverScript4.incorrectNucleotides;

                if (correctAnswers == 45 || skipLevel4)
                {
                    StartCoroutine(spinDNASpirale());
                }
            }
            else
            {
                if (!mouseOverScript4.isRNAFinished)
                {
                    int correctAnswersRNA = mouseOverScript4.correctNucleotidesRNA.Count(n => n.HasValue);
                    uiText4.text = "Pravilno:  " + correctAnswersRNA + "/10\r\nNapačno: " + mouseOverScript4.incorrectNucleotidesRNA + "";
                    mistakesLevel4_2 = mouseOverScript4.incorrectNucleotidesRNA;

                    if ((correctAnswersRNA == 10 || skipLevel4) && !startNewLevel)
                    {
                        StartCoroutine(spinRNASpirale());
                    }
                }

                if (startNewLevel && mouseOverScript4.isRNAFinished && currentLevel == 4)
                {
                    timeLevel4End = Time.time - timeLevel4Start;
                    timeLevel4String = setTimeString(timeLevel4End);

                    mistakesLevel4 = mistakesLevel4_1 + mistakesLevel4_2 + mistakesLevel4_3;

                    audioSource.Play();
                    // animacija skozi poro
                    Animator mRNASpiraleAnimator = mRNASpirale.GetComponent<Animator>();
                    //mRNASpiraleAnimator.speed = 1;
                    mRNASpiraleAnimator.SetTrigger("Pore");
                    currentLevel = 5;
                    uiNavodila5.text = "Molekula mRNA je prešla skozi jedrno poro\r\nv citoplazmo celice.\r\nZdaj mora priti do ribosoma na zrnatem endoplazemskem retiklu.";
                    uiText2.text = "Pravilno:  0/1\r\nNapačno: 0";
                }
                else if (!startNewLevel && mouseOverScript4.isRNAFinished)
                {
                    uiText4.text = "Pravilno:  " + correctLevel4_3 + "/2\r\nNapačno: " + mistakesLevel4_3;
                }
            }
        }
        else if (level == 5)
        {
            List<string> clickedObjects = mouseOverScript5.clickedObjects;
            int correctAnswers = 0;
            mistakesLevel5 = 0;
            foreach (string clickedObject in clickedObjects)
            {
                if (clickedObject == "Ribosom2")
                {
                    correctAnswers++;
                }
                else
                {
                    mistakesLevel5++;
                }
            }

            uiText2.text = "Pravilno:  " + correctAnswers + "/1\r\nNapačno: " + mistakesLevel5;

            if (correctAnswers == 1 && startNewLevel)
            { // TODO: Preveri
                //audioSource.Play();
                StartCoroutine(WaitForABit());
                currentLevel = 6;
                level5.SetActive(false);
                level6.SetActive(true);

                XRPlayer.transform.localPosition = new Vector3(0.5f, -0.78f, -8.3f);
                XRPlayer.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);

                mouseOverScript6.tRNAAnimator1.SetTrigger("initTRNA2");
                timeLevel6Start = Time.time;
            }
        }
        else if (level == 6)
        {
            endScreen.SetActive(true);
            setEndScreenStats();
        }
    }

    private IEnumerator spinDNASpirale()
    {
        Animator DNASpiraleAnimator = DNASpirale.transform.Find("DNA chain").GetComponent<Animator>();
        preventReset = true;
        float animationDuration = 5f; // Total duration of the animation
        float maxSpeed = 300f; // Maximum speed at the end

        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;

            // Get the normalized time (0 to 1)
            float normalizedTime = elapsedTime / animationDuration;

            // Use a quadratic function to accelerate speed (e.g., y = x^2)
            float speedMultiplier = Mathf.Pow(normalizedTime, 2) * maxSpeed;

            // Apply the speed multiplier to the Animator
            DNASpiraleAnimator.speed = speedMultiplier;

            yield return null;
        }

        // Reset the speed to normal when the animation is done
        DNASpiraleAnimator.speed = 1f;

        ThymineFront.SetActive(false);
        ThymineBack.SetActive(false);
        DNASpirale.SetActive(false);
        if (SlowFront.activeSelf)
        {
            SlowFront.SetActive(false);
            SpeedFront.SetActive(true);
        }
        if (SlowBack.activeSelf)
        {
            SlowBack.SetActive(false);
            SpeedBack.SetActive(true);
        }

        RNASpirale.SetActive(true);
        UracilFront.SetActive(true);
        UracilBack.SetActive(true);
        mouseOverScript4.isRNA = true;
        uiNavodilaTitle4.text = "Transkripcija";
        uiNavodila4.text = "Prepisovanje/transkripcija/sinteza mRNA: \r\nKodogeni (modra) in mRNA (rdeča) verigi dopolni s komplementarnimi baznimi pari.";
        uiText4.text = "Pravilno:  0/10\r\nNapačno: 0";

        if (mouseOverScript4.selectedColor == "Thymine")
        {
            mouseOverScript4.selectedColor = "Uracil";
            mouseOverScript4.fillColor(mouseOverScript4.selectedColor);
        }

        Animator RNASpiraleAnimator = RNASpirale.transform.Find("RNA chain").GetComponent<Animator>();
        
        elapsedTime = 0f;
        animationDuration = 3f;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;

            // Get the normalized time (0 to 1)
            float normalizedTime = elapsedTime / animationDuration;

            // Use a quadratic function to accelerate speed (e.g., y = x^2)
            float speedMultiplier = Mathf.Pow(1 - normalizedTime, 2) * maxSpeed;

            // Apply the speed multiplier to the Animator
            RNASpiraleAnimator.speed = speedMultiplier;

            yield return null;
        }

        // Reset the speed to normal when the animation is done
        RNASpiraleAnimator.speed = 1f;
        preventReset = false;
    }

    private IEnumerator spinRNASpirale()
    {
        preventReset = true;
        Animator RNASpiraleAnimator = RNASpirale.transform.Find("RNA chain").GetComponent<Animator>();
        float animationDuration = 5f; // Total duration of the animation
        float maxSpeed = 300f; // Maximum speed at the end

        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;

            // Get the normalized time (0 to 1)
            float normalizedTime = elapsedTime / animationDuration;

            // Use a quadratic function to accelerate speed (e.g., y = x^2)
            float speedMultiplier = Mathf.Pow(normalizedTime, 2) * maxSpeed;

            // Apply the speed multiplier to the Animator
            RNASpiraleAnimator.speed = speedMultiplier;

            yield return null;
        }

        // Reset the speed to normal when the animation is done
        RNASpiraleAnimator.speed = 1f;

        ControlsFront.SetActive(false);
        ControlsBack.SetActive(false);
        RNASpirale.SetActive(false);

        mRNASpirale.SetActive(true);
        kodogenaSpirala.SetActive(true);


        mouseOverScript4.isRNAFinished = true;
        uiNavodilaTitle4.text = "Transport mRNA";
        uiNavodila4.text = "Veriga mRNA je pravilno kodirana. Zdaj se mora mRNA prenesti iz jedra na ribosom zrnatega endoplazemskega retikla. Izberi mRNA verigo (rdeča) in jo prenesi ven iz jedra skozi jedrno poro.";
        uiText4.text = "Pravilno:  0/2\r\nNapačno: 0";


        Animator mRNASpiraleAnimator = mRNASpirale.GetComponent<Animator>();
        Animator kodogenaSpiralaAnimator = kodogenaSpirala.GetComponent<Animator>();

        elapsedTime = 0f;
        animationDuration = 3f;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;

            // Get the normalized time (0 to 1)
            float normalizedTime = elapsedTime / animationDuration;

            // Use a quadratic function to accelerate speed (e.g., y = x^2)
            float speedMultiplier = Mathf.Pow(1 - normalizedTime, 2) * maxSpeed;

            // Apply the speed multiplier to the Animator
            mRNASpiraleAnimator.speed = speedMultiplier;
            kodogenaSpiralaAnimator.speed = speedMultiplier;

            yield return null;
        }

        // Reset the speed to normal when the animation is done
        mRNASpiraleAnimator.speed = 1f;
        kodogenaSpiralaAnimator.speed = 1f;
        preventReset = false;
    }

    private IEnumerator WaitForABit(bool startNewLevel = false, float seconds = 0.1f)
    {
        int currentLevelNOW = currentLevel;
        yield return new WaitForSeconds(seconds);

        if (currentLevelNOW == 2)
        {
            startLevel3();
        }
        else if (currentLevelNOW == 3)
        {
            if (!startNewLevel)
            {
                //audioSource.Play();
                kromosomiObject.SetActive(false);
                ideogramObject.SetActive(true);
                Eksocitoza.Find("Title").GetComponent<TextMeshPro>().text = "Ideogram";
                Eksocitoza.Find("Text").GetComponent<TextMeshPro>().text = "- je shematski prikaz kromosomov, kjer so kromosomi narisani v standardizirani obliki glede na velikost, položaj centromere in vzorec barvanja.\r\n- Uporablja se za prepoznavanje kromosomskih nepravilnosti in za lažje kartiranje genov.";
                Tabla2.transform.Find("Tabla 2 title").GetComponent<TextMeshPro>().text = "Iskanje zapisa gena za somatostatin";
                Tabla2.transform.Find("Tabla 2 tekst").GetComponent<TextMeshPro>().text = "Izberi kratek odsek ideograma tretjega kromosoma na poziciji 3q27.3, kjer se nahaja gen za somatostatin.";
            }
            else
            {
                //audioSource.Play();
                currentLevel = 4;
                level3.SetActive(false);
                level4.SetActive(true);
                timeLevel4Start = Time.time;
                XRPlayer.transform.localPosition = new Vector3(-7f, 1.74f, -3f);
                XRPlayer.transform.localRotation = Quaternion.Euler(0f, 15f, 0f);
            }
        }
        else if (currentLevelNOW == 6)
        {
            level6.SetActive(startNewLevel);
        }
    }

    private void startLevel3()
    {
        // change level
        currentLevel = 3;
        level2.SetActive(false);
        level3.SetActive(true);

        timeLevel3Start = Time.time;

        // init script
        mouseOverScript1.resetLevel3();
        //level1Player.transform.localPosition = new Vector3(3.958851f, 0.4614812f, -0.670835f);
        //level1Player.transform.localRotation = Quaternion.Euler(0f, 92.339f, 0f);
        XRPlayer.transform.localPosition = new Vector3(0f, 0f, 0f);
        XRPlayer.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);

        // custom
        kromosomiObject.SetActive(true);
        //ideogramObject;
        rightWall.SetActive(false);
        Cells.SetActive(false);
        Eksocitoza.Find("Title").GetComponent<TextMeshPro>().text = "Kromosom";
        Eksocitoza.Find("Text").GetComponent<TextMeshPro>().text = "- Shranjuje genetsko informacijo v obliki DNA, ki vsebuje navodila za sintezo beljakovin.\r\n- Organizira in zaščiti gene, da so stabilni med celičnimi delitvami.\r\n- Omogoča regulacijo izražanja genov, saj določa, kateri geni se bodo aktivirali za sintezo beljakovin.\r\n- Omogoča natančen prenos genetske informacije na mRNA med procesom transkripcije.";
        Tabla2.transform.Find("Tabla 2 title").GetComponent<TextMeshPro>().text = "Iskanje kromosoma za somatostatin";
        Tabla2.transform.Find("Tabla 2 tekst").GetComponent<TextMeshPro>().text = "V celičnem jedru se nahajajo kromosomi, ki vsebujejo genski zapis DNK. Izberi kromosom na katerem se nahaja genski zapis za somatostatin.";
    }

    public void startLevel5()
    {
        //audioSource.Play();
        level4.SetActive(false);
        level5.SetActive(true);
        timeLevel5Start = Time.time;


        mRNALevel5.SetActive(true);
        jedroLevel5.SetActive(true);
        jedroLevel2.SetActive(false);
        mouseOverScript5.resetLevel5();
        //level2Player.SetActive(false);

        //level5Player.SetActive(true);

        XRPlayer.transform.localPosition = new Vector3(-2.2f, 0f, -22f);
        XRPlayer.transform.localRotation = Quaternion.Euler(0f, 210f, 0f);
    }

    public string setTimeString(float time)
    {
        int hours = Mathf.FloorToInt(time / 3600);
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        if (hours == 0)
        {
            return string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else
        {
            return string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
        }
    }

    public void setEndScreenStats()
    {
        levelTimeTexts[0].text = "Čas: "+timeLevel1String;
        levelTimeTexts[1].text = "Čas: "+timeLevel2String;
        levelTimeTexts[2].text = "Čas: "+timeLevel3String;
        levelTimeTexts[3].text = "Čas: "+timeLevel4String;
        levelTimeTexts[4].text = "Čas: "+timeLevel5String;
        levelTimeTexts[5].text = "Čas: "+timeLevel6String;


        int totalSeconds = ConvertTimeToSeconds(timeLevel1String) + ConvertTimeToSeconds(timeLevel2String) + ConvertTimeToSeconds(timeLevel3String) + ConvertTimeToSeconds(timeLevel4String) +
            ConvertTimeToSeconds(timeLevel5String) + ConvertTimeToSeconds(timeLevel6String);

        timeTotalString = setTimeString(totalSeconds);

        totalTimeText.text = "Skupni čas: " + timeTotalString;

        levelMistakeTexts[0].text = "Napake: " + mistakesLevel1;
        levelMistakeTexts[1].text = "Napake: " + mistakesLevel2;
        levelMistakeTexts[2].text = "Napake: " + mistakesLevel3;
        levelMistakeTexts[3].text = "Napake: " + mistakesLevel4;
        levelMistakeTexts[4].text = "Napake: " + mistakesLevel5;
        levelMistakeTexts[5].text = "Napake: " + mistakesLevel6;

        int totalMistakes = mistakesLevel1 + mistakesLevel2 + mistakesLevel3 + mistakesLevel4 + mistakesLevel5 + mistakesLevel6;
        totalMistakesText.text = "Vse napake: " + totalMistakes;

        string filePath = saveStats();
        //SendEmailSMTP(filePath);
        StartCoroutine(SendEmail(filePath));
    }

    string saveStats()
    {
        string folderPath = Application.persistentDataPath;
        string baseFileName = "GameStats";
        string fileExtension = ".txt";

        // Generate a unique filename
        string filePath = GetUniqueFilePath(folderPath, baseFileName, fileExtension);

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            // Write date and time
            writer.WriteLine($"Čas simulacije: {DateTime.Now}");
            writer.WriteLine($"Uporabnik: {playerName}");
            writer.WriteLine($"Naprava: {SystemInfo.deviceUniqueIdentifier}");
            writer.WriteLine("----------------------------------");

            // Write level-wise mistakes

            writer.WriteLine("Level 1 - Laboratorij - Izbira celice ---------");
            writer.WriteLine($"Level 1: {mistakesLevel1} napak");
            writer.WriteLine($"Level 1 čas: {timeLevel1String} ");
            writer.WriteLine($"Level 1 teleport-i: {teleportsLevel1} ");

            writer.WriteLine("Level 2 - Živalska celica izbira organelov ---------");
            writer.WriteLine($"Level 2: {mistakesLevel2} napak");
            writer.WriteLine($"Level 2 čas: {timeLevel2String} ");
            writer.WriteLine($"Level 2 teleport-i: {teleportsLevel2} ");

            writer.WriteLine("Level 3 - Laboratorij - Izbira kromosoma in odseka kromosoma ---------");
            writer.WriteLine($"Level 3-1: {mistakesLevel3_1} napak");
            writer.WriteLine($"Level 3-2: {mistakesLevel3_2} napak");
            writer.WriteLine($"Level 3: {mistakesLevel3} napak");
            writer.WriteLine($"Level 3 čas: {timeLevel3String} ");
            writer.WriteLine($"Level 3 teleport-i: {teleportsLevel3} ");

            writer.WriteLine("Level 4 - Jedro - Transkripcija mRNA ---------");
            writer.WriteLine("Level 4 - Jedro - DNA ---------");
            writer.WriteLine($"Level 4-1: {mistakesLevel4_1} napak");
            writer.WriteLine("Level 4 - Jedro - Transkripcija RNA ---------");
            writer.WriteLine($"Level 4-2: {mistakesLevel4_2} napak");
            writer.WriteLine("Level 4 - Jedro - Transport RNA ---------");
            writer.WriteLine($"Level 4-3: {mistakesLevel4_3} napak");
            writer.WriteLine($"Level 4: {mistakesLevel4} napak");
            writer.WriteLine($"Level 4 čas: {timeLevel4String} ");
            writer.WriteLine($"Level 4 teleport-i: {teleportsLevel4} ");

            writer.WriteLine("Level 5 - Živalska celica izbira Ribosoma ---------");
            writer.WriteLine($"Level 5: {mistakesLevel5} napak");
            writer.WriteLine($"Level 5 čas {timeLevel5String} ");
            writer.WriteLine($"Level 5 teleport-i: {teleportsLevel5} ");

            writer.WriteLine("Level 6 - Ribosom izbira aminokislin ---------");
            writer.WriteLine($"Level 6: {mistakesLevel6} napak");
            writer.WriteLine($"Level 6 čas: {timeLevel6String} ");
            writer.WriteLine($"Level 6 teleport-i: {teleportsLevel6} ");

            writer.WriteLine("----------------------------------");


            // Write total stats
            int totalMistakes = mistakesLevel1 + mistakesLevel2 + mistakesLevel3 + mistakesLevel4 + mistakesLevel5 + mistakesLevel6;
            int totalTeleports = teleportsLevel1 + teleportsLevel2 + teleportsLevel3 + teleportsLevel4 + teleportsLevel5 + teleportsLevel6;
            writer.WriteLine($"Skupni čas: {timeTotalString}");
            writer.WriteLine($"Vse napake: {totalMistakes}");
            writer.WriteLine($"Vsi teleport-i: {totalTeleports}");


            writer.WriteLine("----------------------------------");
        }

        Debug.Log("Game stats saved to: " + filePath);

        return filePath;
    }

    private string GetUniqueFilePath(string folderPath, string baseName, string extension)
    {
        int counter = 1;
        string filePath = Path.Combine(folderPath, baseName + extension);

        // Check if the file exists and increment a number at the end
        while (File.Exists(filePath))
        {
            filePath = Path.Combine(folderPath, $"{baseName}_{counter}{extension}");
            counter++;
        }

        return filePath;
    }

    private static int ConvertTimeToSeconds(string time)
    {
        string[] parts = time.Split(':'); // Split time string

        int hours = 0, minutes, seconds;

        if (parts.Length == 3) // If format is HH:MM:SS
        {
            hours = int.Parse(parts[0]);
            minutes = int.Parse(parts[1]);
            seconds = int.Parse(parts[2]);
        }
        else if (parts.Length == 2) // If format is MM:SS
        {
            minutes = int.Parse(parts[0]);
            seconds = int.Parse(parts[1]);
        }
        else
        {
            return 0;
            //throw new FormatException("Invalid time format: " + time);
        }

        return (hours * 3600) + (minutes * 60) + seconds;
    }

    private string logStats()
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine($"Čas simulacije: {DateTime.Now}");
        sb.AppendLine($"Uporabnik: {playerName}");
        sb.AppendLine($"Naprava: {SystemInfo.deviceUniqueIdentifier}");
        sb.AppendLine("----------------------------------");

        // Write level-wise mistakes

        sb.AppendLine("Level 1 - Laboratorij - Izbira celice ---------");
        sb.AppendLine($"Level 1: {mistakesLevel1} napak");
        sb.AppendLine($"Level 1 čas: {timeLevel1String} ");
        sb.AppendLine($"Level 1 teleport-i: {teleportsLevel1} ");

        sb.AppendLine("Level 2 - Živalska celica izbira organelov ---------");
        sb.AppendLine($"Level 2: {mistakesLevel2} napak");
        sb.AppendLine($"Level 2 čas: {timeLevel2String} ");
        sb.AppendLine($"Level 2 teleport-i: {teleportsLevel2} ");

        sb.AppendLine("Level 3 - Laboratorij - Izbira kromosoma in odseka kromosoma ---------");
        sb.AppendLine($"Level 3-1: {mistakesLevel3_1} napak");
        sb.AppendLine($"Level 3-2: {mistakesLevel3_2} napak");
        sb.AppendLine($"Level 3: {mistakesLevel3} napak");
        sb.AppendLine($"Level 3 čas: {timeLevel3String} ");
        sb.AppendLine($"Level 3 teleport-i: {teleportsLevel3} ");

        sb.AppendLine("Level 4 - Jedro - Transkripcija mRNA ---------");
        sb.AppendLine("Level 4 - Jedro - DNA ---------");
        sb.AppendLine($"Level 4-1: {mistakesLevel4_1} napak");
        sb.AppendLine("Level 4 - Jedro - Transkripcija RNA ---------");
        sb.AppendLine($"Level 4-2: {mistakesLevel4_2} napak");
        sb.AppendLine("Level 4 - Jedro - Transport RNA ---------");
        sb.AppendLine($"Level 4-3: {mistakesLevel4_3} napak");
        sb.AppendLine($"Level 4: {mistakesLevel4} napak");
        sb.AppendLine($"Level 4 čas: {timeLevel4String} ");
        sb.AppendLine($"Level 4 teleport-i: {teleportsLevel4} ");

        sb.AppendLine("Level 5 - Živalska celica izbira Ribosoma ---------");
        sb.AppendLine($"Level 5: {mistakesLevel5} napak");
        sb.AppendLine($"Level 5 čas {timeLevel5String} ");
        sb.AppendLine($"Level 5 teleport-i: {teleportsLevel5} ");

        sb.AppendLine("Level 6 - Ribosom izbira aminokislin ---------");
        sb.AppendLine($"Level 6: {mistakesLevel6} napak");
        sb.AppendLine($"Level 6 čas: {timeLevel6String} ");
        sb.AppendLine($"Level 6 teleport-i: {teleportsLevel6} ");

        sb.AppendLine("----------------------------------");


        // Write total stats
        int totalMistakes = mistakesLevel1 + mistakesLevel2 + mistakesLevel3 + mistakesLevel4 + mistakesLevel5 + mistakesLevel6;
        int totalTeleports = teleportsLevel1 + teleportsLevel2 + teleportsLevel3 + teleportsLevel4 + teleportsLevel5 + teleportsLevel6;
        sb.AppendLine($"Skupni čas: {timeTotalString}");
        sb.AppendLine($"Vse napake: {totalMistakes}");
        sb.AppendLine($"Vsi teleport-i: {totalTeleports}");

        sb.AppendLine("----------------------------------");

        Debug.Log(sb.ToString());
        
        return sb.ToString();
    }

    public void restartSimulation(int? level_to_restart = null)
    {
        if (preventReset)
        {
            Debug.Log("PreventReset");
            return;
        }

        if (level_to_restart != null)
        {
            Debug.Log("restart level: " + level_to_restart);
            if (level_to_restart == 6)
            {
                restartLevel6();
            }
            else if (level_to_restart == 5)
            {
                restartLevel5();
            }
            else if (level_to_restart == 4)
            {
                restartLevel4();
            }
            else if (level_to_restart == 3)
            {
                restartLevel3();
            }
            else if (level_to_restart == 2)
            {
                restartLevel2();
            }
            else if (level_to_restart == 1)
            {
                restartLevel1();
            }
            else if (level_to_restart == 0)
            {
                restartLevel0();
            }
            return;
        }

        Debug.Log("reset all. current level: " + currentLevel);

        if (currentLevel == 6)
        {
            resetLevel6();
            resetLevel5();
            resetLevel4();
            resetLevel3();
            resetLevel2();
        }
        if (currentLevel == 5)
        {
            resetLevel5();
            resetLevel4();
            resetLevel3();
            resetLevel2();
        }
        if (currentLevel == 4)
        {
            resetLevel4();
            resetLevel3();
            resetLevel2();
        }
        if (currentLevel == 3)
        {
            resetLevel3();
            resetLevel2();
        }
        if (currentLevel == 2)
        {
            resetLevel2();
        }

        resetLevel1();
        //actuallyMovePlayers();
        resetLevel0();
        XRPlayer.transform.localPosition = new Vector3(0f, 0f, 0f);
        XRPlayer.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
        level0.SetActive(true);

        currentLevel = 0;
    }

    private void resetLevel0()
    {
        level0.SetActive(true);
        passthrough.enabled = true;
        XRKeyboard.Clear();
        playerName = "";
        XRPlayer.transform.localPosition = new Vector3(0f, 0f, 0f);
        XRPlayer.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
        level0.SetActive(false);
    }

    private void resetLevel1()
    {
        level1.SetActive(true);
        mouseOverScript1.resetLevel1();

        mistakesLevel1 = 0;
        teleportsLevel1 = 0;

        kromosomiObject.SetActive(false);
        ideogramObject.SetActive(false);
        rightWall.SetActive(true);
        Cells.SetActive(true);

        Eksocitoza.Find("Title").GetComponent<TextMeshPro>().text = "Eksocitoza";
        Eksocitoza.Find("Text").GetComponent<TextMeshPro>().text = " - je aktivni celični transport, pri\r\nkaterem se snovi iz celice sproščajo v zunajcelični prostor.\r\n - Celica lahko na ta način izloči nerabne snovi, hormone, encime…, ki jih predhodno zapakira v membranske mešičke, ki se izlijejo z membrano, vsebina pa se izloči iz celice.";
        Tabla2.transform.Find("Tabla 2 title").GetComponent<TextMeshPro>().text = "Izbira celice";
        Tabla2.transform.Find("Tabla 2 tekst").GetComponent<TextMeshPro>().text = "Izberi celico, v kateri se lahko sintetizira somatosatin";

        //level1Player.transform.localPosition = new Vector3(3.958851f, 0.4614812f, -0.670835f);
        //actuallyMovePlayer(level1Player.transform, new Vector3(3.958851f, 0.4614812f, -0.670835f));
        XRPlayer.transform.localPosition = new Vector3(0f, 0f, 0f);
        XRPlayer.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
        //level1Player.transform.localRotation = Quaternion.Euler(0f, 92.339f, 0f);

        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("chalkboard");
        foreach (GameObject obj in taggedObjects)
        {
            obj.SetActive(false);
        }
        level1.SetActive(false);
    }

    private void resetLevel2() {
        level2.SetActive(true);
        mouseOverScript2.resetLevel2();

        mistakesLevel2 = 0;
        teleportsLevel2 = 0;

        mRNALevel5.SetActive(false);
        jedroLevel5.SetActive(false);
        jedroLevel2.SetActive(true);
        //level2Player.SetActive(true);
        //level2Player.transform.localPosition = new Vector3(6.78f, -1.13f, -3.77f);
        //actuallyMovePlayer(level2Player.transform, new Vector3(6.78f, -1.13f, -3.77f));
        //level2Player.transform.localRotation = Quaternion.Euler(0f, 144.622f, 0f);

        //level5Player.SetActive(false);
        //level5Player.transform.localPosition = new Vector3(12.48385f, 1.47f, -30.82501f);
        //actuallyMovePlayer(level5Player.transform, new Vector3(12.48385f, 1.47f, -30.82501f));
        //level5Player.transform.localRotation = Quaternion.Euler(0f, 202.5f, 0f);

        XRPlayer.transform.localPosition = new Vector3(-2.25f, 0f, -5f);
        XRPlayer.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);

        uiNavodila5.text = "Nahajaš se v žlezni celici hipotalamusa.\r\nPotrebuješ recept za izgradnjo beljakovine in organel, ki lahko somatostatin izloči iz celice.\r\nKateri organeli bodo sodelujejo pri sintezi somatostatina?";
        uiText2.text = "Pravilno:  0/2\r\nNapačno: 0/7";

        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("chalkboard");
        foreach (GameObject obj in taggedObjects)
        {
            // Toggle active state
            obj.SetActive(false);
        }

        level2.SetActive(false);
    }

    private void resetLevel3() {
        level3.SetActive(true);
        kromosomiObject.SetActive(true);
        ideogramObject.SetActive(true);
        mouseOverScript1.resetLevel3();
        ideogramScript3.resetIdeogram();

        mistakesLevel3 = 0;
        mistakesLevel3_1 = 0;
        mistakesLevel3_2 = 0;
        teleportsLevel3 = 0;

        ideogramObject.SetActive(false);
        rightWall.SetActive(false);
        Cells.SetActive(false);
        Eksocitoza.Find("Title").GetComponent<TextMeshPro>().text = "Kromosom";
        Eksocitoza.Find("Text").GetComponent<TextMeshPro>().text = "- Shranjuje genetsko informacijo v obliki DNA, ki vsebuje navodila za sintezo beljakovin.\r\n- Organizira in zaščiti gene, da so stabilni med celičnimi delitvami.\r\n- Omogoča regulacijo izražanja genov, saj določa, kateri geni se bodo aktivirali za sintezo beljakovin.\r\n- Omogoča natančen prenos genetske informacije na mRNA med procesom transkripcije.";
        Tabla2.transform.Find("Tabla 2 title").GetComponent<TextMeshPro>().text = "Iskanje kromosoma za somatostatin";
        Tabla2.transform.Find("Tabla 2 tekst").GetComponent<TextMeshPro>().text = "V celičnem jedru se nahajajo kromosomi, ki vsebujejo genski zapis DNK. Izberi kromosom na katerem se nahaja genski zapis za somatostatin.";

        //level3Player.transform.localPosition = new Vector3(3.958851f, 0.4614812f, -0.670835f);
        //actuallyMovePlayer(level3Player.transform, new Vector3(3.958851f, 0.4614812f, -0.670835f));
        //level3Player.transform.localRotation = Quaternion.Euler(0f, 92.339f, 0f);
        XRPlayer.transform.localPosition = new Vector3(0f, 0f, 0f);
        XRPlayer.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);

        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("chalkboard");
        foreach (GameObject obj in taggedObjects)
        {
            obj.SetActive(false);
        }
        level3.SetActive(false);
    }

    private void resetLevel4() {
        level4.SetActive(true);
        DNASpirale.SetActive(true);
        RNASpirale.SetActive(true);
        mRNASpirale.SetActive(true);
        kodogenaSpirala.SetActive(true);

        int[] rnaNukleotidiPositions = { 2, 6, 7, 19, 20, 27, 28, 29, 30, 31 };

        for (int i = 1; i <= 45; i++)
        {
            GameObject emptyNukleotid = DNASpirale.transform.Find("DNA chain/Nukleotidi/DNA." + i.ToString("D2") + "B").gameObject;
            emptyNukleotid.GetComponent<Renderer>().material = greyMaterial;
            emptyNukleotid.GetComponent<Collider>().enabled = true;
            DNASpirale.transform.Find("DNA chain/Nukleotidi/DNA." + i.ToString("D2") + "A").GetComponent<Collider>().enabled = true;
        }

        for (int i = 0; i < 10; i++)
        {
            int rna_reset_number = rnaNukleotidiPositions[i];
            GameObject emptyNukleotid = RNASpirale.transform.Find("RNA chain/Nukleotidi/DNA." + rna_reset_number.ToString("D2") + "B").gameObject;
            emptyNukleotid.GetComponent<Renderer>().material = greyMaterial;
            emptyNukleotid.GetComponent<Collider>().enabled = true;
            RNASpirale.transform.Find("RNA chain/Nukleotidi/DNA." + rna_reset_number.ToString("D2") + "A").GetComponent<Collider>().enabled = true;
        }

        mouseOverScript4.resetLevel4();
        Animator DNASpiraleAnimator = DNASpirale.transform.Find("DNA chain").GetComponent<Animator>();
        Animator RNASpiraleAnimator = RNASpirale.transform.Find("RNA chain").GetComponent<Animator>();
        Animator kodogenaSpiralaAnimator = kodogenaSpirala.GetComponent<Animator>();
        DNASpiraleAnimator.speed = 1f;
        RNASpiraleAnimator.speed = 1f;
        DNASpiraleAnimator.Rebind();
        DNASpiraleAnimator.Update(0f);
        RNASpiraleAnimator.Rebind();
        RNASpiraleAnimator.Update(0f);
        kodogenaSpiralaAnimator.Rebind();
        kodogenaSpiralaAnimator.Update(0f);

        mistakesLevel4 = 0;
        mistakesLevel4_1 = 0;
        mistakesLevel4_2 = 0;
        mistakesLevel4_3 = 0;
        correctLevel4_3 = 0;
        teleportsLevel4 = 0;

        ControlsFront.SetActive(true);
        ControlsBack.SetActive(true);
        UracilFront.SetActive(false);
        UracilBack.SetActive(false);
        ThymineFront.SetActive(true);
        ThymineBack.SetActive(true);
        if (SlowFront.activeSelf)
        {
            SlowFront.SetActive(false);
            SpeedFront.SetActive(true);
        }
        if (SlowBack.activeSelf)
        {
            SlowBack.SetActive(false);
            SpeedBack.SetActive(true);
        }

        skipLevel4 = false;
        uiNavodilaTitle4.text = "DNK somatostatina";
        uiNavodila4.text = "Nahajaš se v tretjem kromosomu na lokaciji 3q27.3. Ker je zapis zgrajen iz intronov (rdeče obarvana veriga) in eksonov (modro obarvana veriga), so zapisana samo kodirajoča zaporedja. Dopolni DNK in pobarvaj manjkakoče baze.";
        uiText4.text = "Pravilno:  0/45\r\nŠt. napak: 0";

        //level4Player.transform.localPosition = new Vector3(-3.001f, -0.3414855f, -1.119f);
        //actuallyMovePlayer(level4Player.transform, new Vector3(-3.001f, -0.3414855f, -1.119f));
        //level4Player.transform.localRotation = Quaternion.Euler(0f, 42.711f, 0f);
        XRPlayer.transform.localPosition = new Vector3(-7f, 1.74f, -3f);
        XRPlayer.transform.localRotation = Quaternion.Euler(0f, 15f, 0f);

        RNASpirale.SetActive(false);
        mRNASpirale.SetActive(false);
        kodogenaSpirala.SetActive(false);

        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("chalkboard");
        foreach (GameObject obj in taggedObjects)
        {
            // Toggle active state
            obj.SetActive(false);
        }
        level4.SetActive(false);
    }

    private void resetLevel5() {
        level5.SetActive(true);
        mouseOverScript5.resetLevel5();

        mistakesLevel5 = 0;
        teleportsLevel5 = 0;

        mRNALevel5.SetActive(true);
        jedroLevel5.SetActive(true);
        jedroLevel2.SetActive(false);
        //level5Player.SetActive(true);
        //level5Player.transform.localPosition = new Vector3(12.48385f, 1.47f, -30.82501f);
        //actuallyMovePlayer(level5Player.transform, new Vector3(12.48385f, 1.47f, -30.82501f));
        //level5Player.transform.localRotation = Quaternion.Euler(0f, 202.5f, 0f);

        //level2Player.SetActive(false);
        //level2Player.transform.localPosition = new Vector3(6.78f, -1.13f, -3.77f);
        //actuallyMovePlayer(level2Player.transform, new Vector3(6.78f, -1.13f, -3.77f));
        //level2Player.transform.localRotation = Quaternion.Euler(0f, 144.622f, 0f);
        XRPlayer.transform.localPosition = new Vector3(-2.2f, 0f, -22f);
        XRPlayer.transform.localRotation = Quaternion.Euler(0f, 210f, 0f);

        mRNALevel5.transform.localPosition = new Vector3(0.152f, 0.2306f, 0.103f);

        uiNavodila5.text = "Molekula mRNA je prešla skozi jedrno poro\r\nv citoplazmo celice.\r\nZdaj mora priti do ribosoma na zrnatem endoplazemskem retiklu.";
        uiText2.text = "Pravilno:  0/1\r\nNapačno: 0";
        level5.SetActive(false);
    }

    private void resetLevel6(bool is_restart = false) {
        level6.SetActive(true);
        endScreen.SetActive(false);

        //level6Player.transform.localPosition = new Vector3(0.182f, -2.999f, -7.331f);
        //actuallyMovePlayer(level6Player.transform, new Vector3(0.182f, -2.999f, -7.331f));
        //level6Player.transform.localRotation = Quaternion.Euler(0f, 214.74f, 0f);
        mouseOverScript6.resetLevel6();

        mistakesLevel6 = 0;
        teleportsLevel6 = 0;

        XRPlayer.transform.localPosition = new Vector3(0.5f, -0.78f, -8.3f);
        XRPlayer.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);

        //level6.SetActive(false);
        //StartCoroutine(WaitForABit(is_restart)); // tale povzroci vec listenerjev
        level6.SetActive(false);
    }

    public void actuallyMovePlayers()
    {
        for (int i=0;i<10;i++)
        {
            //StartCoroutine(WaitAndRetry(i, playerTransform, playerPosition));

            // level 6
            StartCoroutine(WaitAndRetry(i, level6Player.transform, new Vector3(0.182f, -2.999f, -7.331f)));
            //level 5
            // StartCoroutine(WaitAndRetry(i, level5Player.transform, new Vector3(12.48385f, 1.47f, -30.82501f)));
            // StartCoroutine(WaitAndRetry(i, level2Player.transform, new Vector3(6.78f, -1.13f, -3.77f)));
            // level 4
            StartCoroutine(WaitAndRetry(i, level4Player.transform, new Vector3(-3.001f, -0.3414855f, -1.119f)));
            // level 3
            // StartCoroutine(WaitAndRetry(i, level3Player.transform, new Vector3(3.958851f, 0.4614812f, -0.670835f)));
            // level 2
            StartCoroutine(WaitAndRetry(i, level2Player.transform, new Vector3(6.78f, -1.13f, -3.77f)));
            StartCoroutine(WaitAndRetry(i, level5Player.transform, new Vector3(12.48385f, 1.47f, -30.82501f)));
            // level 1
            StartCoroutine(WaitAndRetry(i, level1Player.transform, new Vector3(3.958851f, 0.4614812f, -0.670835f)));
        }
    }


    private IEnumerator WaitAndRetry(int i, Transform playerTransform, Vector3 playerPosition)
    {
        yield return new WaitForSeconds(0.01f * i);
        playerTransform.localPosition = playerPosition;
    }

    public void restartLevel0()
    {
        resetLevel0();
        level0.SetActive(true);
    }
    public void restartLevel1()
    {
        resetLevel1();
        level1.SetActive(true);
        timeLevel1Start = Time.time;
    }
    public void restartLevel2()
    {
        resetLevel2();
        level2.SetActive(true);
        timeLevel2Start = Time.time;
    }
    public void restartLevel3()
    {
        resetLevel3();
        level3.SetActive(true);
        timeLevel3Start = Time.time;
    }
    public void restartLevel4()
    {
        resetLevel4();
        level4.SetActive(true);
        timeLevel4Start = Time.time;
    }
    public void restartLevel5()
    {
        resetLevel5();
        level5.SetActive(true);
        timeLevel5Start = Time.time;
    }
    public void restartLevel6()
    {
        resetLevel6();
        level6.SetActive(true);
        timeLevel6Start = Time.time;
        mouseOverScript6.tRNAAnimator1.SetTrigger("initTRNA2");
    }

    public void incrementTeleportForLevel()
    {
        if (currentLevel == 1)
        {
            teleportsLevel1++;
        }
        else if (currentLevel == 2) {
            teleportsLevel2++;
        }
        else if (currentLevel == 3) {
            teleportsLevel3++;
        }
        else if (currentLevel == 4) {
            teleportsLevel4++;
        }
        else if (currentLevel == 5) {
            teleportsLevel5++;
        }
        else if (currentLevel == 6) {
            teleportsLevel6++;
        }
    }

    private IEnumerator SendEmail(string fileLocation)
    {
        byte[] fileData = File.ReadAllBytes(fileLocation);

        List<IMultipartFormSection> formData = new List<IMultipartFormSection>
        {
            new MultipartFormDataSection("name", name),
            new MultipartFormDataSection("message", "Simulacija končana. Podatki: \n\r" + logStats()),
            new MultipartFormFileSection("attachment", fileData, "PlayerReport.txt", "text/plain")
        };

        UnityWebRequest request = UnityWebRequest.Post("https://unityemailservice.onrender.com/sendemail", formData);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Email sent with attachment!");
        }
        else
        {
            Debug.LogError("Failed: " + request.error);
        }
    }

    public void uiButtonClick()
    {
        EventSystem.current.SetSelectedGameObject(null);
        restartSimulation(null);
    }

    /*private void SendEmailSMTP(string fileLocation)
    {
        string smtpHost = "smtp.gmail.com";
        int smtpPort = 587;
        string fromEmail = "david.pu1997@gmail.com\r\n";
        string fromPassword = "tkewsggjttsixasd";
        string toEmail = "david.pu97@gmail.com";
        string subject = "Sinteza beljakovin simulacija";
        string bodyText = "Simulacija končana. Podatki: \n\r" + logStats();


        //string filePath = Application.persistentDataPath + "/report.txt";
        string filePath = fileLocation;

        // Create a dummy .txt file if it doesn’t exist
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "This is a test file created by Unity.");
        }

        MailMessage mail = new MailMessage();
        mail.From = new MailAddress(fromEmail);
        mail.To.Add(toEmail);
        mail.Subject = subject;
        mail.Body = bodyText;

        Attachment attachment = new Attachment(filePath, MediaTypeNames.Text.Plain);
        mail.Attachments.Add(attachment);

        SmtpClient smtpServer = new SmtpClient(smtpHost);
        smtpServer.Port = smtpPort;
        smtpServer.Credentials = new NetworkCredential(fromEmail, fromPassword);
        smtpServer.EnableSsl = true;

        try
        {
            smtpServer.Send(mail);
        }
        catch (System.Exception ex)
        {
        }
    }*/
}
