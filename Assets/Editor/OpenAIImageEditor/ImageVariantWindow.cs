using UnityEngine;
using UnityEditor;
using OpenAI;
using OpenAI.Models;
using OpenAI.Images;
using UnityEngine.Assertions;

public class ImageVariant : EditorWindow
{
    //OPEN AI
    private string previewImagePath;
    private Texture2D selectedImage;
    //EDITOR WINDOW
    private Texture2D previewImage;
    private bool isLoading;
    private float previewSize = 100;
    [MenuItem("Tools/Generative Image Variant")]
    public static void ShowWindow()
    {
        GetWindow<ImageVariant>("Generative Image Variant");
    }
    private void SetPreviewImage(string path)
    {
        previewImage = new Texture2D(1, 1);
        previewImage.LoadImage(System.IO.File.ReadAllBytes(path));
    }

    async void GenerateImage()
    {
        var api = new OpenAIClient();
        var imagePath = AssetDatabase.GetAssetPath(selectedImage);
        var request = new ImageVariationRequest(imagePath);
        var results = api.ImagesEndPoint.CreateImageVariationAsync(request);
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
        string path = "Assets/GeneratedImage/Variants";
        if (!AssetDatabase.IsValidFolder(path))
        {
            AssetDatabase.CreateFolder("Assets/GeneratedImage", "Variants");
        }
        string fileName = System.IO.Path.GetFileName(previewImagePath);
        string destFile = System.IO.Path.Combine(path, fileName);
        System.IO.File.Copy(previewImagePath, destFile, true);
        AssetDatabase.Refresh();
    }
    private void OnGUI()
    {
        GUILayout.Label("Image Editor", EditorStyles.boldLabel);
        GUILayout.Label("Select an Image:");
        selectedImage = (Texture2D)EditorGUILayout.ObjectField(selectedImage, typeof(Texture2D), false);
        if (selectedImage != null)
        {
            GUILayout.Label("Selected Image:");
            GUILayout.Label(selectedImage, GUILayout.Width(100), GUILayout.Height(100));
            if (GUILayout.Button("Generate Image"))
            {
                Debug.Log("Generate Image");
                GenerateImage();
            }
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
