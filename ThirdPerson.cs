using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonController : MonoBehaviour
{
    [Header("Gravity")]
    public float gravity = -9.81f;
    public float groundCheckDistance = 0.2f;
    public LayerMask groundMask; 

    [Header("Movement")]
    public FixedJoystick moveJoystick;
    public Transform cam;
    public float speed = 5f;
    public float rotationSpeed = 10f;

    [Header("Body Tilt")]
    public Transform torso; 
    public float tiltAmountRun = 5f; 
    public float tiltAmountIdle = 3f; 
    public float tiltSpeed = 2f; 

    private CharacterController controller;
    private Vector3 direction;
    private bool isMoving;
    private float currentTiltX; 
    private float currentTiltZ;
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (torso == null)
            torso = transform;  
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(
        transform.position + Vector3.down * 0.1f,
        groundCheckDistance,
        groundMask);

        // Гравитация
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Фикс "дрожания" на земле
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }

        Vector2 input = new Vector2(moveJoystick.Horizontal, moveJoystick.Vertical);
        direction = new Vector3(input.x, 0f, input.y).normalized;
        isMoving = direction.magnitude >= 0.1f;


        if (isMoving)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            Vector3 moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            controller.Move(moveDir * speed * Time.deltaTime);


            Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }


        TiltBody();
    }

    void TiltBody()
    {
        if (isMoving)
        {

            currentTiltX = Mathf.Sin(Time.time * tiltSpeed) * tiltAmountRun;
            currentTiltZ = 0f; 
        }
        else
        {

            currentTiltX = 0f; 
            currentTiltZ = Mathf.Sin(Time.time * tiltSpeed) * tiltAmountIdle;
        }


        torso.localRotation = Quaternion.Euler(currentTiltX, 0, currentTiltZ);
    }
}