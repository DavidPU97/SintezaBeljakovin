using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;

public class AnimationSpeedControl : MonoBehaviour
{
    public Animator animator; // Assign the Animator component in the Inspector
    public Animator animatorRNA; // Assign the Animator component in the Inspector
    public float deltaSpeed = 1f;
    public GameObject speedUpObject;
    public GameObject slowDownObject;
    private statsUpdate StatsScript;

    void Start()
    {
        // Set animation speed (1.0 is normal speed, 0.5 is half speed, 2.0 is double speed)
        //animator.speed = 1f; // Play animations at double speed
        StatsScript = GameObject.Find("MainStats").GetComponent<statsUpdate>();
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

    void XRDetectClick()
    {
        bool isFront = slowDownObject.name.Contains("Front");
        if (!slowDownObject.activeSelf) {
            slowDownObject.SetActive(true);
            speedUpObject.SetActive(false);

            if (isFront)
            {
                StatsScript.SlowBack.SetActive(true);
                StatsScript.SpeedBack.SetActive(false);
            }
            else
            {
                StatsScript.SlowFront.SetActive(true);
                StatsScript.SpeedFront.SetActive(false);
            }
        }
        else if(!speedUpObject.activeSelf) {
            speedUpObject.SetActive(true);
            slowDownObject.SetActive(false);

            if (isFront)
            {
                StatsScript.SpeedBack.SetActive(true);
                StatsScript.SlowBack.SetActive(false);
            }
            else
            {
                StatsScript.SpeedFront.SetActive(true);
                StatsScript.SlowFront.SetActive(false);
            }
        }
        else
        {
            speedUpObject.SetActive(true);
            slowDownObject.SetActive(false);

            if (isFront)
            {
                StatsScript.SlowBack.SetActive(true);
                StatsScript.SpeedBack.SetActive(false);
            }
            else
            {
                StatsScript.SlowFront.SetActive(true);
                StatsScript.SpeedFront.SetActive(false);
            }
        }
        animator.speed = deltaSpeed;
        animatorRNA.speed = deltaSpeed;
    }
}
