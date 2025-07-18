using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MainGameScript : MonoBehaviour
{
    public GameObject botPrefab;
    public Transform[] spawnPoints;

    [Header("Texture Settings")]
    public Texture[] textures;                  
    public string[] textureNames;              
    public Renderer targetRenderer;            
    public TextMesh leftText;                    
    public TextMesh rightText;                    

    [Header("Game Settings")]
    public float roundTime = 30f;      
    public GameObject losePanel;
    public GameObject winPanel;
    public string playerTag = "Player";      

    [SerializeField] private NavMeshSurface surface;

    private int currentTextureIndex;           
    public int correctZone;                   
    private bool playerInZone;
    private bool playerInLeftZone;
    private bool playerInRightZone;   
    private string currentCorrectName;
    int botCount = 9;
    private Coroutine gameLoopCoroutine;       
    private List<BotAI> waitingBots = new List<BotAI>();
    private int lastTextureIndex = -1;

    void Start()
    {
        // Проверки на корректность настроек
        if (textures.Length != textureNames.Length)
        {
            Debug.LogError("Количество текстур и названий должно совпадать!");
            return;
        }
        BakeNavMesh();
        SpawnBots();
        if (losePanel != null)
            losePanel.SetActive(false);
        
        StartGameLoop();
    }
    
    public void BakeNavMesh()
    {
        if (surface != null)
        {
            surface.BuildNavMesh(); // Бейк поверхности
            Debug.Log("NavMesh baked at runtime!");
        }
        else
        {
            Debug.LogError("NavMeshSurface not assigned!");
        }
    }

    public void RegisterBotWaiting(BotAI bot)
    {
        if (!waitingBots.Contains(bot))
            waitingBots.Add(bot);
    }

    private void OnTextureChanged()
    {
        foreach (var bot in waitingBots)
        {
           bot.CheckDecision();
        }

        waitingBots.Clear();

    }

    public void reduce() {
        botCount--;
        if(botCount == 0) StartCoroutine(DelayedWin()); 
    }


    IEnumerator DelayedWin()
    {
        yield return new WaitForSeconds(1.5f); // Задержка перед победой
        if (winPanel != null)
        {
            winPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }


    void SpawnBots()
    {
        for (int i = 0; i < 9; i++)
        {
            Instantiate(botPrefab, spawnPoints[i].position, Quaternion.identity);
        }
    }

    void StartGameLoop()
    {
        if (gameLoopCoroutine != null)
            StopCoroutine(gameLoopCoroutine);

        gameLoopCoroutine = StartCoroutine(GameLoop());
    }

    IEnumerator GameLoop()
    {
        while (true)
        {
            do
            {
                currentTextureIndex = Random.Range(0, textures.Length);
            } while (currentTextureIndex == lastTextureIndex && textures.Length > 1); // Повторяем, если текстура совпала с предыдущей

            lastTextureIndex = currentTextureIndex; // Запоминаем текущую текстуру

            targetRenderer.material.mainTexture = textures[currentTextureIndex];
            currentCorrectName = textureNames[currentTextureIndex];

            correctZone = Random.Range(1, 3);

            if (correctZone == 1)
            {
                leftText.text = currentCorrectName;
                rightText.text = GetRandomWrongName();
            }
            else
            {
                rightText.text = currentCorrectName;
                leftText.text = GetRandomWrongName();
            }

            yield return new WaitForSeconds(roundTime);

            CheckPlayerChoice();
            OnTextureChanged();
        }
    }

    string GetRandomWrongName()
    {
        string wrongName;
        do
        {
            wrongName = textureNames[Random.Range(0, textureNames.Length)];
        } while (wrongName == currentCorrectName); // Гарантируем, что название будет другим

        return wrongName;
    }

    void CheckPlayerChoice()
    {

        if (!playerInZone ||
            (correctZone == 1 && !playerInLeftZone) ||
            (correctZone == 2 && !playerInRightZone))
        {
            ShowLosePanel();
        }
    }


    public void SetPlayerInLeftZone(bool state)
    {
        playerInZone = state;
        playerInLeftZone = state;
    }

    public void SetPlayerInRightZone(bool state)
    {
        playerInZone = state;
        playerInRightZone = state;
    }

    void ShowLosePanel()
    {
        if (losePanel != null)
        {
            losePanel.SetActive(true);
            Time.timeScale = 0f; 
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}