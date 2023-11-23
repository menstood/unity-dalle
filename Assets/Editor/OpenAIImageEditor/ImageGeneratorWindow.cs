using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using OpenAI;
using OpenAI.Models;
using OpenAI.Images;
using UnityEngine.Assertions;

public class ImageGenerator : EditorWindow
{
    //OPEN AI
    private string previewImagePath;
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
    private void SetPreviewImage(string path)
    {
        previewImage = new Texture2D(1, 1);
        previewImage.LoadImage(System.IO.File.ReadAllBytes(path));
    }

    async void GenerateImage()
    {
        var api = new OpenAIClient();
        var request = new ImageGenerationRequest(prompt, Model.DallE_2);
        var results = api.ImagesEndPoint.GenerateImageAsync(request);
        isLoading = true;
        await results;
        foreach (var (path, texture) in results.Result)
        {
            previewImagePath = path;
            SetPreviewImage(path);
            Assert.IsNotNull(texture);
            isLoading = false;
        }

    }

    private void SaveTexture()
    {
        string path = "Assets/GeneratedImage";
        if (!AssetDatabase.IsValidFolder(path))
        {
            AssetDatabase.CreateFolder("Assets", "GeneratedImage");
        }
        string fileName = System.IO.Path.GetFileName(previewImagePath);
        string destFile = System.IO.Path.Combine(path, fileName);
        System.IO.File.Copy(previewImagePath, destFile, true);
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
            EditorGUI.ProgressBar(GUILayoutUtility.GetRect(position.width - previewSize - 40, 20), 0.9f, "Loading...");
        }
    }
}