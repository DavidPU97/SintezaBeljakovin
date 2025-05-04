using UnityEngine;
using UnityEngine.InputSystem;

public class OptimizedSliceHighlighter : MonoBehaviour
{
    public Renderer planeRenderer;       // Renderer of the main plane
    public GameObject hoverOverlay;      // Prefab or GameObject for hover highlight
    public GameObject clickedOverlay;    // Prefab or GameObject for clicked highlight
    public GameObject clickedOverlayCorrect;    // Prefab or GameObject for clicked highlight
    public int numberOfSlices = 39;      // Number of horizontal slices
    public Color hoverColor = Color.yellow;  // Color for the hover overlay
    public Color clickedColor = Color.red;   // Color for the clicked overlay
    public GameObject hoverWidthObject;
    public GameObject ideogramTextBubble;

    private int currentHoveredSlice = -1;  // Currently hovered slice
    private int selectedSlice = -1;        // Currently clicked slice
    private float sliceWidth;              // Width of each slice in world units
    private float planeHeight;             // Height of the plane
    private float hoverSlice = 0.03f;             // Height of the plane

    private float startPlaneXCamera = 4.87f;
    private float endPlaneXCamera = -4.968f;
    //private float startPlaneX = 3.85f;
    //private float endPlaneX = -4.265f;
    private float startPlaneX = 0f;
    private float endPlaneX = -8.3f;
    public Transform RightControllerTransform;
    public InputActionReference triggerAction;

    private statsUpdate StatsScript;

    void Start()
    {
        StatsScript = GameObject.Find("MainStats").GetComponent<statsUpdate>();
        // Calculate slice width and plane height based on the plane's size
        //Bounds planeBounds = planeRenderer.bounds;
        Renderer hoverRenderer = hoverWidthObject.GetComponent<Renderer>();
        Bounds planeBounds = hoverRenderer.bounds;
        sliceWidth = planeBounds.size.x;// / numberOfSlices;
        planeHeight = planeBounds.size.y;

        // Disable overlays initially
        hoverOverlay.SetActive(false);
        clickedOverlay.SetActive(false);
        clickedOverlayCorrect.SetActive(false);
    }

    void Update()
    {
        // Perform a raycast to detect the mouse position on the plane
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Ray ray = new Ray(RightControllerTransform.position, RightControllerTransform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject == planeRenderer.gameObject)
            {
                // Convert the hit point to a slice index
                Vector3 localHitPoint = planeRenderer.transform.InverseTransformPoint(hit.point);
                float actualHitPointX = ConvertCoordinate(localHitPoint.x, startPlaneXCamera, endPlaneXCamera, startPlaneX, endPlaneX);

                //int hoveredSlice = Mathf.FloorToInt((actualHitPointX + 0.5f * planeRenderer.bounds.size.x) / sliceWidth);
                int hoveredSlice = Mathf.FloorToInt((actualHitPointX + planeRenderer.bounds.size.x) / sliceWidth);
                // Highlight the hovered slice
                if (hoveredSlice != currentHoveredSlice && hoveredSlice >= 0 && hoveredSlice <= numberOfSlices)
                {
                    currentHoveredSlice = hoveredSlice;
                    PositionOverlay(hoverOverlay, hoveredSlice, hoverColor, actualHitPointX);
                    hoverOverlay.SetActive(true);
                }

                // Handle click to lock highlight
                //if (Input.GetMouseButtonDown(0))
                if (triggerAction.action.WasPressedThisFrame())
                {
                    selectedSlice = hoveredSlice;
                    hoverSlice = 0.04f;
                    if (hoveredSlice == 2)
                    {
                        PositionOverlay(clickedOverlayCorrect, selectedSlice, clickedColor, actualHitPointX);
                        clickedOverlayCorrect.SetActive(true);
                        clickedOverlay.SetActive(false);

                        if (!ideogramTextBubble.activeSelf)
                        {
                            StatsScript.audioSource.Play();
                            ideogramTextBubble.SetActive(true);

                            StatsScript.timeLevel3End = Time.time - StatsScript.timeLevel3Start;
                            StatsScript.timeLevel3String = StatsScript.setTimeString(StatsScript.timeLevel3End);

                            StatsScript.mistakesLevel3 = StatsScript.mistakesLevel3_1 + StatsScript.mistakesLevel3_2;
                        }
                        else
                        {
                            StatsScript.updateStats(3, true);
                        }
                    }
                    else { 
                        PositionOverlay(clickedOverlay, selectedSlice, clickedColor, actualHitPointX);
                        clickedOverlay.SetActive(true);
                        clickedOverlayCorrect.SetActive(false);

                        if (!ideogramTextBubble.activeSelf)
                        {
                            StatsScript.mistakesLevel3_2++;
                        }
                    }
                    hoverSlice = 0.03f;
                }
            }
        }
        else
        {
            // Disable hover overlay when the mouse leaves the plane
            if (currentHoveredSlice != -1)
            {
                currentHoveredSlice = -1;
                hoverOverlay.SetActive(false);
            }
        }
    }

    void PositionOverlay(GameObject overlay, int sliceIndex, Color color, float xpos)
    {
        // Calculate the center position of the slice in world space
        Bounds planeBounds = planeRenderer.bounds;
        Vector3 sliceCenter = planeRenderer.transform.position;
        sliceCenter.x += (sliceIndex - numberOfSlices) * sliceWidth;
        float newX = ConvertCoordinate(sliceCenter.x, startPlaneXCamera, endPlaneXCamera, startPlaneX, endPlaneX);

        // Position the overlay slightly above the plane
        overlay.transform.position = new Vector3(sliceCenter.x, sliceCenter.y, sliceCenter.z + hoverSlice);

        // Scale the overlay to match the slice dimensions
        //overlay.transform.localScale = new Vector3(sliceWidth, planeHeight, overlay.transform.localScale.z); 

        // Set the overlay's color
        Renderer overlayRenderer = overlay.GetComponent<Renderer>();
        if (overlayRenderer != null)
        {
            overlayRenderer.material.color = color;
        }
    }

    float ConvertCoordinate(float oldX, float oldMin, float oldMax, float newMin, float newMax)
    {
        // Linear transformation formula
        return ((oldX - oldMin) / (oldMax - oldMin)) * (newMax - newMin) + newMin;
    }

    public void resetIdeogram()
    {
        hoverOverlay.SetActive(false);
        clickedOverlay.SetActive(false);
        clickedOverlayCorrect.SetActive(false);
        ideogramTextBubble.SetActive(false);
    }
}
