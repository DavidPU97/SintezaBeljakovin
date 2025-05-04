using UnityEngine;

public class SliceSelector : MonoBehaviour
{
    public Renderer planeRenderer;   // The Renderer displaying the plane
    public int numberOfSlices = 10;  // Number of horizontal slices
    public Color hoverBorderColor = Color.yellow;  // Color of the border for hover
    public Color clickedBorderColor = Color.red;   // Color of the border for selection
    public int borderWidth = 5;  // Thickness of the border in pixels

    private Texture2D originalTexture;  // The original texture
    private Texture2D editableTexture;  // Modifiable texture for highlighting
    private int sliceWidth;  // Width of each slice in pixels
    private int currentHoveredSlice = -1;  // Currently hovered slice
    private int selectedSlice = -1;  // The slice that has been clicked/selected

    void Start()
    {
        // Ensure the texture is readable and create a copy for modification
        originalTexture = (Texture2D)planeRenderer.material.mainTexture;
        editableTexture = new Texture2D(originalTexture.width, originalTexture.height, originalTexture.format, false);
        editableTexture.SetPixels(originalTexture.GetPixels());
        editableTexture.Apply();

        // Assign the editable texture to the plane
        planeRenderer.material.mainTexture = editableTexture;

        // Calculate the width of each slice in pixels
        sliceWidth = originalTexture.width / numberOfSlices;
    }

    void Update()
    {
        // Perform a raycast to detect the mouse position on the plane
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject == planeRenderer.gameObject)
            {
                // Convert the hit point to UV coordinates
                Vector2 pixelUV = hit.textureCoord;
                pixelUV.x *= originalTexture.width;

                // Calculate the currently hovered slice
                int hoveredSlice = Mathf.FloorToInt(pixelUV.x / sliceWidth);

                // Update hover highlight if the hovered slice changes
                if (hoveredSlice != currentHoveredSlice)
                {
                    currentHoveredSlice = hoveredSlice;
                    UpdateHighlights();
                }

                // Handle click to lock the highlight
                if (Input.GetMouseButtonDown(0))
                {
                    selectedSlice = hoveredSlice;
                    Debug.Log($"Slice {selectedSlice} clicked and highlighted.");
                    UpdateHighlights();
                }
            }
        }
        else
        {
            // Reset hover highlight when the mouse leaves the plane
            if (currentHoveredSlice != -1)
            {
                currentHoveredSlice = -1;
                UpdateHighlights();
            }
        }
    }

    void UpdateHighlights()
    {
        // Reset the texture to the original image
        ResetTexture();

        // Reapply highlights for the clicked slice
        if (selectedSlice != -1)
        {
            HighlightSlice(selectedSlice, clickedBorderColor);
        }

        // Apply hover highlight if hovering over a different slice
        if (currentHoveredSlice != -1 && currentHoveredSlice != selectedSlice)
        {
            HighlightSlice(currentHoveredSlice, hoverBorderColor);
        }

        // Apply changes to the texture
        editableTexture.Apply();
    }

    void HighlightSlice(int sliceIndex, Color borderColor)
    {
        // Calculate the pixel range for the slice
        int startX = sliceIndex * sliceWidth;
        int endX = Mathf.Min(startX + sliceWidth, editableTexture.width);

        // Add a border along the edges of the slice
        for (int x = startX; x < endX; x++)
        {
            for (int y = 0; y < borderWidth; y++) // Bottom border
            {
                editableTexture.SetPixel(x, y, borderColor);
            }

            for (int y = editableTexture.height - borderWidth; y < editableTexture.height; y++) // Top border
            {
                editableTexture.SetPixel(x, y, borderColor);
            }
        }

        for (int y = 0; y < editableTexture.height; y++)
        {
            for (int x = startX; x < startX + borderWidth; x++) // Left border
            {
                editableTexture.SetPixel(x, y, borderColor);
            }

            for (int x = endX - borderWidth; x < endX; x++) // Right border
            {
                editableTexture.SetPixel(x, y, borderColor);
            }
        }
    }

    void ResetTexture()
    {
        // Reset the texture to its original state
        editableTexture.SetPixels(originalTexture.GetPixels());
    }
}
