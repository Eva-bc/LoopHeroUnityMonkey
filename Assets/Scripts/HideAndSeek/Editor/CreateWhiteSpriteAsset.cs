using UnityEngine;
using UnityEditor;
using System.IO;

namespace HideAndSeek.Editor
{
    /// <summary>
    /// One-shot editor utility to generate a white 4x4 PNG sprite used by the HideAndSeek scene.
    /// Run via: Tools > HideAndSeek > Create White Sprite
    /// </summary>
    public static class CreateWhiteSpriteAsset
    {
        private const string OutputPath = "Assets/Assets/Texture/WhiteSquare.png";

        [MenuItem("Tools/HideAndSeek/Create White Sprite")]
        public static void Create()
        {
            Texture2D tex = new Texture2D(4, 4, TextureFormat.RGBA32, false);
            Color[] pixels = new Color[16];
            for (int i = 0; i < pixels.Length; i++)
                pixels[i] = Color.white;
            tex.SetPixels(pixels);
            tex.Apply();

            byte[] pngData = tex.EncodeToPNG();
            Object.DestroyImmediate(tex);

            File.WriteAllBytes(
                Path.Combine(Application.dataPath, "../" + OutputPath),
                pngData
            );
            AssetDatabase.Refresh();

            // Set texture import type to Sprite
            TextureImporter importer = AssetImporter.GetAtPath(OutputPath) as TextureImporter;
            if (importer != null)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Single;
                importer.filterMode = FilterMode.Point;
                importer.SaveAndReimport();
            }

            Debug.Log($"[HideAndSeek] White sprite created at {OutputPath}");
        }
    }
}
