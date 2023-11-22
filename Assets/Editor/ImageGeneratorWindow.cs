using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Drawing;

public class ImageGenerator : EditorWindow
{
  private Texture2D previewImage;
  [MenuItem("Tools/Image Generator")]
    public static void ShowWindow()
    {
        GetWindow<ImageGenerator>("Image Generator");
    }
    private void SetPreviewImage(string path)
    {
        previewImage = new Texture2D(1, 1);
        previewImage.LoadImage(System.IO.File.ReadAllBytes(path));
    }
    private void OnGUI()
    {
        GUILayout.Label("Image Generator", EditorStyles.boldLabel);
        GUILayout.Label("API Key");
        GUILayout.TextField("API Key");
        GUILayout.Label("Prompt");
        GUILayout.TextField("Prompt");
        if (GUILayout.Button("Generate Image"))
        {
            Debug.Log("Generate Image");
        }
        // create preview image
        // create button to save image
     if (previewImage != null)
    { 
        GUILayout.Label("Preview Image");
        Rect previewRect = GUILayoutUtility.GetRect(100, 100); // Define the size of the preview
        EditorGUI.DrawPreviewTexture(previewRect, previewImage);
        GUILayout.Button("Save Image");
    }
    }
}
