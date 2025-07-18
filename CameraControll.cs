using UnityEngine;
using UnityEngine.EventSystems;

public class CameraControll : MonoBehaviour
{
    [Header("Collision")]
    public LayerMask collisionMask; // ������ ����, � �������� ������ ������ ������������ (��������, Default, Walls)
    public float cameraRadius = 0.3f; // ������ ������ (����� �� ���������� � �����)
    public float minCameraDistance = 1f; // ����������� ��������� �� ������

    [Header("Target")]
    public Transform target;  // �����
    public float distance = 4f;
    public float height = 2f;

    [Header("Rotation")]
    public float sensitivity = 0.2f;
    public float minVerticalAngle = -30f; // ����������� ����
    public float maxVerticalAngle = 60f;  // ����������� �����

    private float currentX = 0f;
    private float currentY = 15f; // ��������� ��������� ������ ����

    void Update()
    {
        // ��������� ��� ����
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);

            // ���������� ���, ���� �� ��� UI (����������)
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                continue;

            // ������������ ������ ����������� �����
            if (touch.phase == TouchPhase.Moved)
            {
                currentX += touch.deltaPosition.x * sensitivity;
                currentY -= touch.deltaPosition.y * sensitivity * 0.5f; // ���������, ��� �� �����������

                // ������������ ������������ ����
                currentY = Mathf.Clamp(currentY, minVerticalAngle, maxVerticalAngle);
            }
        }
    }

    void LateUpdate()
    {
        // ������� ������ ������ ������
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 offset = rotation * new Vector3(0, 0, -distance);

        RaycastHit hit;
        Vector3 targetPos = target.position + Vector3.up * height;
        if (Physics.SphereCast(
            targetPos,
            cameraRadius,
            offset.normalized,
            out hit,
            distance,
            collisionMask))
        {
            // ���� ������������ � ������� ������ ����� � ������
            float newDistance = hit.distance - 0.2f; // ��������� ������
            newDistance = Mathf.Max(newDistance, minCameraDistance); // �� ����� ��������
            offset = offset.normalized * newDistance;
        }
 
        // ������� ������ � ������ ������
        transform.position = target.position + offset + Vector3.up * height;
        transform.LookAt(target.position + Vector3.up * height * 0.5f);
    }
}