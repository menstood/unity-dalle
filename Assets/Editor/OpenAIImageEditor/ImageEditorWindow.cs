using UnityEngine;
using UnityEditor;
using OpenAI;
using OpenAI.Images;
using UnityEngine.Assertions;

public class ImageEditor : DallEBaseEditor
{
    private Texture2D selectedImage;
    private Texture2D maskImage;
    //EDITOR WINDOW
    [MenuItem("Tools/Dall-E/Edits")]
    public static void ShowWindow()
    {
        GetWindow<ImageEditor>("Dall-E Image Edits");
    }

    async void GenerateImage()
    {
        var api = new OpenAIClient();
        var request = new ImageEditRequest(selectedImage, maskImage, prompt);
        var results = api.ImagesEndPoint.CreateImageEditAsync(request);
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
        GUILayout.Label("Select an Image:");
        selectedImage = (Texture2D)EditorGUILayout.ObjectField(selectedImage, typeof(Texture2D), false);
        GUILayout.Label("Select a Mask Image:");
        maskImage = (Texture2D)EditorGUILayout.ObjectField(maskImage, typeof(Texture2D), false);
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
                SaveTexture("EditedImage");
            }
        }
        if (isLoading)
        {
            EditorGUI.ProgressBar(GUILayoutUtility.GetRect(position.width - previewSize - 40, 20), 0.9f, "Generating...");
        }
    }
}
