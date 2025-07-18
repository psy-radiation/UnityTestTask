using UnityEngine;
using UnityEngine.AI;

public class BotAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform[] zones;
    private int chosenZoneIndex;
    private bool isWaitingForResult;
    private MainGameScript gameController;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        gameController = FindFirstObjectByType<MainGameScript>();
        agent.stoppingDistance = 2f; // ����� �� "������" � �����
        zones = new Transform[] {
            GameObject.FindWithTag("leftBox").transform,
            GameObject.FindWithTag("rightBox").transform
        };

        MakeDecision();
    }

    void MakeDecision()
    {
        chosenZoneIndex = Random.Range(0, 2); // 0 = left, 1 = right
        agent.SetDestination(zones[chosenZoneIndex].position);
        isWaitingForResult = false;
    }

    void Update()
    {
        // ���� ��� ����� �� ���� � ��� �� ������� ���������
        if (!isWaitingForResult &&
            !agent.pathPending &&
            agent.remainingDistance < 3f)
        {
            isWaitingForResult = true;
            gameController.RegisterBotWaiting(this); // �������� GameController
        }
    }

    // ���������� GameController ��� ����� ��������
    public void CheckDecision()
    {
        bool isCorrect = ((chosenZoneIndex + 1) == gameController.correctZone); // +1 �.�. � ��� correctZone: 1=left, 2=right
        if (!isCorrect) { gameController.reduce(); Destroy(gameObject); }
        else MakeDecision(); // �������� ����� ����
    }
}