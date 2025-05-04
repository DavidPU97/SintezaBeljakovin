using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.XR.Oculus;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;

public class Level2Click : MonoBehaviour
{
    // Start is called before the first frame update
    private MouseOverHighlighter2 mouseOverScriptLevel2;
    private MouseOverHighlighter2 mouseOverScriptLevel5;
    private statsUpdate StatsScript;
    public GameObject mainCameraLevel2;
    public GameObject mainCameraLevel5;
    void CustomStart()
    {
        if (mainCameraLevel2 != null && mouseOverScriptLevel2 == null)
        {
            mouseOverScriptLevel2 = mainCameraLevel2.GetComponent<MouseOverHighlighter2>();
        }
        if (mainCameraLevel5 != null && mouseOverScriptLevel5 == null)
        {
            mouseOverScriptLevel5 = mainCameraLevel5.GetComponent<MouseOverHighlighter2>();
        }

        if (StatsScript == null)
        {
            GameObject Stats = GameObject.Find("MainStats");
            StatsScript = Stats.GetComponent<statsUpdate>(); 
        }
    }

    public GameObject targetObject; // Assign the inactive object in the Inspector
    public TextMeshProUGUI[] targetObjects; // Assign the inactive object in the Inspector

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
        //StatsScript = GameObject.Find("MainStats").GetComponent<statsUpdate>();
        CustomStart();

        bool nextLevel = false;
        
        string originalTextRibosom = "Pravilen odgovor! Njihova vloga je sinteza beljakovin iz mRNA, pri èemer so odgovorni za dejansko združevanje aminokislin v pravilno zaporedje, da nastane beljakovina, ki se nato transportira v druge dele celice ali izloèa iz celice.\r\nZa nadaljevanje ";
        string originalTextNucleus = "Pravilen odgovor! Jedro shranjuje genetski material (DNA) ter uravnava celiène procese, vkljuèno s sintezo beljakovin in celièno delitvijo.\r\nZa nadaljevanje ";
        if (targetObjects != null && StatsScript.currentLevel == 2) {
            string textbox_text = "";
            bool jedro_clicked = mouseOverScriptLevel2.clickedObjects.Contains("Nucleolus");
            bool ribosom_clicked = mouseOverScriptLevel2.clickedObjects.Contains("Ribosom2");

            if (gameObject.tag == "Nucleolus" || gameObject.tag == "Ribosom2")
            {
                string text_to_add = "";
                if (gameObject.tag == "Nucleolus")
                {
                    textbox_text = originalTextNucleus;
                    if (jedro_clicked && ribosom_clicked)
                    {
                        nextLevel = true;
                        //text_to_add = "ponovno izberi celièno jedro.";
                    }
                    else if (ribosom_clicked && !jedro_clicked)
                    {
                        text_to_add = "ponovno izberi celièno jedro.";
                        StatsScript.audioSource.Play();

                        if (StatsScript.timeLevel2End == 0f)
                        {
                            StatsScript.timeLevel2End = Time.time - StatsScript.timeLevel2Start;
                            StatsScript.timeLevel2String = StatsScript.setTimeString(StatsScript.timeLevel2End);
                        }
                    }
                    else if (!ribosom_clicked)
                    {
                        text_to_add = "izberi še en organel, ki sodeluje pri sintezi beljakovin.";
                        StatsScript.audioSource.Play();
                    }
                }
                else if (gameObject.tag == "Ribosom2")
                {
                    textbox_text = originalTextRibosom;
                    if (!jedro_clicked)
                    {
                        text_to_add = "izberi še en organel, ki sodeluje pri sintezi beljakovin.";
                        if (!ribosom_clicked)
                        {
                            StatsScript.audioSource.Play();
                        }
                    }
                    else
                    {
                        text_to_add = "ponovno izberi celièno jedro.";
                        if (!ribosom_clicked)
                        {
                            StatsScript.audioSource.Play();
                        }

                        if (StatsScript.timeLevel2End == 0f)
                        {
                            StatsScript.timeLevel2End = Time.time - StatsScript.timeLevel2Start;
                            StatsScript.timeLevel2String = StatsScript.setTimeString(StatsScript.timeLevel2End);
                        }
                    }
                }

                foreach (TextMeshProUGUI textbox in targetObjects)
                {
                    textbox.text = textbox_text + text_to_add;
                }
            }
        }
        else if (StatsScript.currentLevel == 5 && gameObject.tag == "Ribosom2")
        {
            foreach (TextMeshProUGUI textbox in targetObjects)
            {
                textbox.text = originalTextRibosom + "ponovno izberi ribosom na zrnatem endoplazemskem retiklu.";
            }

            //bool ribosom_clicked = mouseOverScriptLevel5.clickedObjects.Contains("Ribosom2");
            bool ribosom_clicked = mouseOverScriptLevel2.clickedObjects.Contains("Ribosom2");
            if (ribosom_clicked) {
                nextLevel = true;
            }
            else
            {
                StatsScript.audioSource.Play();

                StatsScript.timeLevel5End = Time.time - StatsScript.timeLevel5Start;
                StatsScript.timeLevel5String = StatsScript.setTimeString(StatsScript.timeLevel5End);
            }
        }

        //if (StatsScript.currentLevel == 2)
        //{
        if (mouseOverScriptLevel2.clickedObjects == null || !mouseOverScriptLevel2.clickedObjects.Contains(gameObject.tag))
        {
            if (!(StatsScript.currentLevel == 5 && gameObject.tag == "Nucleolus"))
            {
                mouseOverScriptLevel2.clickedObjects.Add(gameObject.tag);
            }
        }
        //}
        /*else if (StatsScript.currentLevel == 5)
        {
            if (mouseOverScriptLevel5.clickedObjects == null || !mouseOverScriptLevel5.clickedObjects.Contains(gameObject.tag))
            {
                mouseOverScriptLevel5.clickedObjects.Add(gameObject.tag);
            }
        }*/

        // Add any other code here to interact with the object
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("chalkboard");
        foreach (GameObject obj in taggedObjects)
        {
            // Toggle active state
            obj.SetActive(false);
        }

        if (targetObject != null && !nextLevel)
        {
            if (!(StatsScript.currentLevel == 5 && gameObject.tag == "Nucleolus"))
            {
                targetObject.SetActive(true);
            }
        }
        if (!(StatsScript.currentLevel == 5 && gameObject.tag == "Nucleolus"))
        {
            StatsScript.updateStats(StatsScript.currentLevel, nextLevel);
        }
    }
}