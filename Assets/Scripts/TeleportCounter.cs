using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public class TeleportCounter : MonoBehaviour
{
    public TeleportationProvider teleportationProvider;

    private int teleportCount = 0;
    private Vector3 lastPosition;
    private statsUpdate StatsScript;

    void Awake()
    {
        if (teleportationProvider == null)
            teleportationProvider = FindObjectOfType<TeleportationProvider>();
        StatsScript = GameObject.Find("MainStats").GetComponent<statsUpdate>();
    }

    void OnEnable()
    {
        if (teleportationProvider != null)
        {
            //lastPosition = teleportationProvider.xrOrigin.transform.position;
            teleportationProvider.locomotionEnded += OnTeleportEnded;
        }
    }

    void OnDisable()
    {
        if (teleportationProvider != null)
        {
            teleportationProvider.locomotionEnded -= OnTeleportEnded;
        }
    }

    private void OnTeleportEnded(LocomotionProvider provider)
    {
        StatsScript.incrementTeleportForLevel();
    }
}
