using UnityEngine;
using UnityEditor;
using OpenAI;
using OpenAI.Models;
using OpenAI.Images;
using UnityEngine.Assertions;


public class ImageGenerator : EditorWindow
{
    //OPEN AI
    private string prompt;

    //EDITOR WINDOW
    private Texture2D previewImage;
    private bool isLoading;
    private float previewSize = 100;
    [MenuItem("Tools/Generative Image Generator")]
    public static void ShowWindow()
    {
        GetWindow<ImageGenerator>("Generative Image Generator");
    }
    private void SetPreviewImage(Texture2D texture)
    {
        previewImage = texture;
    }

    async void GenerateImage()
    {
        var api = new OpenAIClient();
        var request = new ImageGenerationRequest(prompt, Model.DallE_2);
        var results = api.ImagesEndPoint.GenerateImageAsync(request);
        isLoading = true;
        await results;
        foreach (var (_, texture) in results.Result)
        {
            SetPreviewImage(texture);
            Assert.IsNotNull(texture);
            isLoading = false;
        }

    }

    private void SaveTexture()
    {
        string path = "Assets/GeneratedImage/Created";
        if (!AssetDatabase.IsValidFolder(path))
        {
            AssetDatabase.CreateFolder("Assets/GeneratedImage", "Created");
        }

        string fileName = previewImage.name + ".png";
        //Texture into file
        byte[] bytes = previewImage.EncodeToPNG();
        System.IO.File.WriteAllBytes(System.IO.Path.Combine(path, fileName), bytes);
        AssetDatabase.Refresh();
    }
    
    private void OnGUI()
    {
        GUILayout.Label("Image Generator", EditorStyles.boldLabel);
        GUILayout.Label("Prompt");
        prompt = GUILayout.TextField(prompt);
        if (GUILayout.Button("Generate Image"))
        {
            Debug.Log("Generate Image");
            GenerateImage();
        }
        if (previewImage != null)
        {
            GUILayout.Label("Preview Image");
            Rect previewRect = GUILayoutUtility.GetRect(previewSize, previewSize);
            float xOffset = (previewRect.width - previewSize) / 2f;
            float yOffset = (previewRect.height - previewSize) / 2f;
            previewRect.x += xOffset;
            previewRect.y += yOffset;
            previewRect.width = previewSize;
            previewRect.height = previewSize;
            EditorGUI.DrawPreviewTexture(previewRect, previewImage);
            if (GUILayout.Button("Save Image"))
            {
                SaveTexture();
            }
        }
        if (isLoading)
        {
            EditorGUI.ProgressBar(GUILayoutUtility.GetRect(position.width - previewSize - 40, 20), 0.9f, "Generating...");
        }
    }
}
