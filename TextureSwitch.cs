using UnityEngine;

public class TextureSwitch : MonoBehaviour
{
    [Header("Textures")]
    public Texture[] textures;  // ������ ������� ��� ������������
    public float switchInterval = 2f;  // �������� ����� (� ��������)

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
            timer = 0f;  // ����� �������
            SwitchTexture();
        }
    }

    void SwitchTexture()
    {
        if (textures.Length == 0) return;

        currentTextureIndex = (currentTextureIndex + 1) % textures.Length;  // ����������� ������������
        objectRenderer.material.mainTexture = textures[currentTextureIndex];
    }
}