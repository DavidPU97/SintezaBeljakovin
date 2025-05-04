using UnityEngine;
using System.IO;

public class TextureConverter : MonoBehaviour
{
    public Texture texture; // Drag your image here in the Inspector
    private Texture2D texture2D;

    void Start()
    {
        // Convert the Texture to Texture2D
        texture2D = ConvertToTexture2D(texture);

        if (texture2D != null)
        {
            Debug.Log("Texture successfully converted to Texture2D!");
            SaveTextureToFile(texture2D, "ConvertedTexture");
        }
        else
        {
            Debug.LogError("Failed to convert the texture to Texture2D.");
        }
    }

    void SaveTextureToFile(Texture2D texture, string fileName)
    {
        byte[] bytes = texture.EncodeToPNG();  // You can also use EncodeToJPG()
        File.WriteAllBytes(Application.dataPath + "/" + fileName + ".png", bytes);
        Debug.Log("Texture saved as: " + Application.dataPath + "/" + fileName + ".png");
    }

    Texture2D ConvertToTexture2D(Texture originalTexture)
    {
        // Check if the texture is already a Texture2D
        if (originalTexture is Texture2D)
        {
            return (Texture2D)originalTexture;
        }

        // Create a new Texture2D with the same dimensions and format
        Texture2D newTexture2D = new Texture2D(originalTexture.width, originalTexture.height, TextureFormat.RGBA32, false);

        // Copy the pixel data from the original texture to the new Texture2D
        RenderTexture renderTexture = RenderTexture.GetTemporary(
            originalTexture.width,
            originalTexture.height,
            0,
            RenderTextureFormat.Default,
            RenderTextureReadWrite.Linear);

        Graphics.Blit(originalTexture, renderTexture);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTexture;

        newTexture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        newTexture2D.Apply();

        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTexture);

        return newTexture2D;
    }
}
