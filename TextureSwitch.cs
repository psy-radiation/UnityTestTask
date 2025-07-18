using UnityEngine;

public class TextureSwitch : MonoBehaviour
{
    [Header("Textures")]
    public Texture[] textures;  // Массив текстур для переключения
    public float switchInterval = 2f;  // Интервал смены (в секундах)

    private Renderer objectRenderer;
    private int currentTextureIndex = 0;
    private float timer = 0f;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if (textures.Length == 0)
            Debug.LogError("No textures assigned!");
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= switchInterval)
        {
            timer = 0f;  // Сброс таймера
            SwitchTexture();
        }
    }

    void SwitchTexture()
    {
        if (textures.Length == 0) return;

        currentTextureIndex = (currentTextureIndex + 1) % textures.Length;  // Циклическое переключение
        objectRenderer.material.mainTexture = textures[currentTextureIndex];
    }
}