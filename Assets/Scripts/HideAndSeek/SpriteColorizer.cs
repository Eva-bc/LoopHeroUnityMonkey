using UnityEngine;

namespace HideAndSeek
{
    /// <summary>
    /// Generates a solid-color sprite at runtime for GameObjects that have no sprite assigned.
    /// Attach this to any GameObject with a SpriteRenderer to give it a colored rectangle.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteColorizer : MonoBehaviour
    {
        private static Texture2D _sharedWhiteTexture;

        private void Awake()
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr.sprite == null)
                sr.sprite = CreateWhiteSprite();
        }

        private static Sprite CreateWhiteSprite()
        {
            if (_sharedWhiteTexture == null)
            {
                _sharedWhiteTexture = new Texture2D(4, 4, TextureFormat.RGBA32, false)
                {
                    filterMode = FilterMode.Point,
                    wrapMode = TextureWrapMode.Clamp
                };
                Color[] pixels = new Color[16];
                for (int i = 0; i < pixels.Length; i++)
                    pixels[i] = Color.white;
                _sharedWhiteTexture.SetPixels(pixels);
                _sharedWhiteTexture.Apply();
            }

            return Sprite.Create(
                _sharedWhiteTexture,
                new Rect(0, 0, 4, 4),
                new Vector2(0.5f, 0.5f),
                4f
            );
        }
    }
}
