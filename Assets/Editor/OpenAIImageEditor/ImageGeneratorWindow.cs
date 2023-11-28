using UnityEngine;
using UnityEditor;
using OpenAI;
using OpenAI.Models;
using OpenAI.Images;
using UnityEngine.Assertions;


public class ImageGenerator : DallEBaseEditor
{

    [MenuItem("Tools/Dall-E/Generations", priority = 1)]
    public static void ShowWindow()
    {
        GetWindow<ImageGenerator>("Dall-E Image Generations");
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

    protected override void OnGUI()
    {
        base.OnGUI();
        if (GUILayout.Button("Generate Image"))
        {
            GenerateImage();
        }
        if (previewImage != null)
        {
            GUILayout.Label("Preview Image");
            Rect previewRect = GUILayoutUtility.GetRect(previewSize, previewSize);
            previewRect = SetPreviewSize(previewSize, previewRect);
            EditorGUI.DrawPreviewTexture(previewRect, previewImage);
            if (GUILayout.Button("Save Image"))
            {
                SaveTexture("GeneratedImage");
            }
        }
        if (isLoading)
        {
            EditorGUI.ProgressBar(GUILayoutUtility.GetRect(position.width - previewSize - 40, 20), 0.9f, "Generating...");
        }
    }
}
