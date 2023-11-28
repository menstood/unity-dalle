using UnityEngine;
using UnityEditor;

public class DallEBaseEditor : EditorWindow
{
    protected string prompt;
    protected Texture2D previewImage;
    protected string windowTitle = "Generative Image";
    protected bool isLoading;
    protected float previewSize = 100;

    protected void SetPreviewImage(Texture2D texture)
    {
        previewImage = texture;
    }

    protected void SaveTexture(string prefix)
    {
        string path = "Assets/GeneratedImage/" + prefix;
        if (!AssetDatabase.IsValidFolder(path))
        {
            AssetDatabase.CreateFolder("Assets/GeneratedImage", prefix);
        }
        string fileName = previewImage.name + ".png";
        //Texture into file
        byte[] bytes = previewImage.EncodeToPNG();
        System.IO.File.WriteAllBytes(System.IO.Path.Combine(path, fileName), bytes);
        AssetDatabase.Refresh();
    }

    protected void SetWindowTitle(string title)
    {
        windowTitle = title;
    }

    protected Rect SetPreviewSize(float size, Rect previewRect)
    {
        float xOffset = (previewRect.width - previewSize) / 2f;
        float yOffset = (previewRect.height - previewSize) / 2f;
        previewRect.x += xOffset;
        previewRect.y += yOffset;
        previewRect.width = previewSize;
        previewRect.height = previewSize;
        return previewRect;
    }

    protected virtual void OnGUI()
    {
        GUILayout.Label("Prompt");
        prompt = GUILayout.TextField(prompt);
    }


}
