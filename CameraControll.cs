using UnityEngine;
using UnityEngine.EventSystems;

public class CameraControll : MonoBehaviour
{
    [Header("Collision")]
    public LayerMask collisionMask; // Выбери слои, с которыми камера должна сталкиваться (например, Default, Walls)
    public float cameraRadius = 0.3f; // Радиус камеры (чтобы не застревать в углах)
    public float minCameraDistance = 1f; // Минимальная дистанция до игрока

    [Header("Target")]
    public Transform target;  // Игрок
    public float distance = 4f;
    public float height = 2f;

    [Header("Rotation")]
    public float sensitivity = 0.2f;
    public float minVerticalAngle = -30f; // Ограничение вниз
    public float maxVerticalAngle = 60f;  // Ограничение вверх

    private float currentX = 0f;
    private float currentY = 15f; // Стартовый небольшой наклон вниз

    void Update()
    {
        // Проверяем все тачи
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);

            // Игнорируем тач, если он над UI (джойстиком)
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                continue;

            // Обрабатываем только двигающийся палец
            if (touch.phase == TouchPhase.Moved)
            {
                currentX += touch.deltaPosition.x * sensitivity;
                currentY -= touch.deltaPosition.y * sensitivity * 0.5f; // Медленнее, чем по горизонтали

                // Ограничиваем вертикальный угол
                currentY = Mathf.Clamp(currentY, minVerticalAngle, maxVerticalAngle);
            }
        }
    }

    void LateUpdate()
    {
        // Поворот камеры вокруг игрока
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
            // Если столкновение — двигаем камеру ближе к игроку
            float newDistance = hit.distance - 0.2f; // Небольшой отступ
            newDistance = Mathf.Max(newDistance, minCameraDistance); // Не ближе минимума
            offset = offset.normalized * newDistance;
        }
 
        // Позиция камеры с учетом высоты
        transform.position = target.position + offset + Vector3.up * height;
        transform.LookAt(target.position + Vector3.up * height * 0.5f);
    }
}