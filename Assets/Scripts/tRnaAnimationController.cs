using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class tRnaAnimationController : MonoBehaviour
{
    MouseOverHighlighter6 level6Script;
    public Renderer tRNAAKRenderer;
    public Renderer AK1Renderer;
    public TextMeshProUGUI tRNATextBoxTitle;
    public TextMeshProUGUI tRNATextBox;
    public GameObject tRNATextObj;
    private statsUpdate StatsScript;
    public GameObject mouseOverHighlighterObject;

    // Start is called before the first frame update
    void Start()
    {
        //GameObject mainCamera = GameObject.Find("Camera");
        level6Script = mouseOverHighlighterObject.GetComponent<MouseOverHighlighter6>();

        GameObject Stats = GameObject.Find("MainStats");
        StatsScript = Stats.GetComponent<statsUpdate>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void hideTRNA()
    {
        level6Script.colorTRNA();
        SetChildrenRenderersVisible(gameObject.transform, false);
    }
    
    public void showTRNA()
    {
        SetChildrenRenderersVisible(gameObject.transform, true);
    }

    public void hideAKTRNA()
    {
        level6Script.TRNAAnimationEnd();
    }

    public void startAKAnimation()
    {
        level6Script.moveAminoAcid();
        tRNAAKRenderer.enabled = false;
        AK1Renderer.enabled = true;
    }

    public void showAK()
    {
        level6Script.resetAAAnimation();
        AK1Renderer.enabled = false;
    }

    public void showProtein()
    {
        level6Script.showSomatostatin();
        setTRNATextBox();
    }

    public void sendTRNA()
    {
        level6Script.sendTRNA();
    }

    public void setTRNATextBox()
    {
        tRNATextBoxTitle.text = "Somatostatin";
        tRNATextBox.text = "je hormon, ki zavira izloèanje drugih hormonov, kot so rastni hormon, inzulin in glukagon, ter uravnava prebavne procese. Po sintezi se z membranskim mešièkom prenese izven celice, kjer ga kri raznese po telesu.";
        tRNATextObj.transform.localPosition = new Vector3(-0.009f, 0.886f, -1.625f);
        tRNATextObj.SetActive(true);
        StatsScript.finalAudioSource.Play();
    }

    void SetChildrenRenderersVisible(Transform parent, bool visible)
    {
        foreach (Renderer renderer in parent.GetComponentsInChildren<Renderer>())
        {
            renderer.enabled = visible; // Hide or Show all child renderers
        }
    }

    void afterNewTRNASendOldTRNA()
    {
        level6Script.moveOldTRNA();
    }

    void startmRNA()
    {
        level6Script.startmRNA();
    }

    public void hideReleaseFactor()
    {
        SetChildrenRenderersVisible(gameObject.transform, false);
    }
}
