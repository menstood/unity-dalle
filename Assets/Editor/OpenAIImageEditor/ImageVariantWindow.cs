using UnityEngine;
using UnityEditor;
using OpenAI;
using OpenAI.Images;
using UnityEngine.Assertions;

public class ImageVariant : DallEBaseEditor
{
    private Texture2D selectedImage;

    [MenuItem("Tools/Dall-E/Variations")]
    public static void ShowWindow()
    {
        GetWindow<ImageVariant>("DallE Image Variations");
    }

    async void GenerateImage()
    {
        var api = new OpenAIClient();
        var imagePath = AssetDatabase.GetAssetPath(selectedImage);
        var request = new ImageVariationRequest(imagePath);
        var results = api.ImagesEndPoint.CreateImageVariationAsync(request);
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
        GUILayout.Label("Select an Image:");
        selectedImage = (Texture2D)EditorGUILayout.ObjectField(selectedImage, typeof(Texture2D), false);
        if (selectedImage != null)
        {
            GUILayout.Label("Selected Image:");
            GUILayout.Label(selectedImage, GUILayout.Width(100), GUILayout.Height(100));
            if (GUILayout.Button("Generate Image"))
            {
                GenerateImage();
            }
        }

        if (previewImage != null)
        {
            GUILayout.Label("Preview Image");
            Rect previewRect = GUILayoutUtility.GetRect(previewSize, previewSize);
            previewRect = SetPreviewSize(previewSize, previewRect);
            EditorGUI.DrawPreviewTexture(previewRect, previewImage);
            if (GUILayout.Button("Save Image"))
            {
                SaveTexture("VariantImage");
            }
        }
        if (isLoading)
        {
            EditorGUI.ProgressBar(GUILayoutUtility.GetRect(position.width - previewSize - 40, 20), 0.9f, "Generating...");
        }
    }
}
