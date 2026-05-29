using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [System.Serializable]
    public class MovementSettings
    {
        [Header("Movimiento")]
        public float moveSpeed = 5f;
        [Range(0f, 1f)] public float movementSmoothness = 0.125f;
        public float jumpForce = 7f;
        public float gravity = 20f;
    }

    [System.Serializable]
    public class MouseSettings
    {
        [Header("Ratón")]
        public Vector2 sensitivity = new Vector2(200f, 200f);
        [Range(0f, 0.5f)] public float rotationSmoothTime = 0.05f;
        public float minPitch = -30f;
        public float maxPitch = 60f;
        public bool lockCursor = true;
    }

    [System.Serializable]
    public class CameraSettings
    {
        [Header("Cámara")]
        public Camera playerCamera;
        [Tooltip("Altura de la cámara respecto al suelo del jugador")]
        public float cameraHeight = 0.9f;
        [Range(40f, 110f)] public float initialFOV = 60f;
        [Range(20f, 179f)] public float minFOV = 45f;
        [Range(20f, 179f)] public float maxFOV = 90f;
    }

    public MovementSettings movement = new MovementSettings();
    public MouseSettings mouse = new MouseSettings();
    public CameraSettings cameraSettings = new CameraSettings();

    [Header("Sonido")]
    public AudioClip sonidoSalto; // Sonido al saltar

    private CharacterController controller;
    private AudioSource audioJugador; // Fuente de sonido del jugador
    private Vector3 currentVelocity = Vector3.zero;
    private Vector3 suavizadoVel = Vector3.zero; // FIX MOVIMIENTO: ref propia para el SmoothDamp (ver HandleMovement)
    private float verticalVelocity = 0f;

    private float yaw;   // rotación horizontal del cuerpo
    private float pitch; // rotación vertical de la cámara
    private float yawSmooth, pitchSmooth;
    private float yawSmoothVelocity, pitchSmoothVelocity;

    void Start()
    {

        // Bloquear y ocultar cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        controller = GetComponent<CharacterController>();
        audioJugador = gameObject.AddComponent<AudioSource>();

        if (cameraSettings.playerCamera == null)
        {
            cameraSettings.playerCamera = Camera.main;
        }

        // Colocar la cámara correctamente sobre el jugador
        if (cameraSettings.playerCamera != null)
        {
            if (cameraSettings.playerCamera.transform.parent != transform)
            {
                cameraSettings.playerCamera.transform.SetParent(transform);
            }

            cameraSettings.playerCamera.transform.localPosition = new Vector3(0f, cameraSettings.cameraHeight, 0f);
            cameraSettings.playerCamera.transform.localRotation = Quaternion.identity;
            cameraSettings.playerCamera.fieldOfView = Mathf.Clamp(cameraSettings.initialFOV, cameraSettings.minFOV, cameraSettings.maxFOV);
        }

        yaw = transform.eulerAngles.y;
        pitch = 0f;

        if (mouse.lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (UIManager.Instance != null)
                return;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // Si las instrucciones están abiertas, no mover ni girar al jugador
        if (UIManager.Instance != null && UIManager.Instance.InstruccionesAbiertas())
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        HandleMouseLook();
        HandleMovement();
    }

    void HandleMouseLook()
    {
        if (cameraSettings.playerCamera == null) return;

        float mouseX = Input.GetAxis("Mouse X") * mouse.sensitivity.x * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouse.sensitivity.y * Time.deltaTime;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, mouse.minPitch, mouse.maxPitch);

        yawSmooth = Mathf.SmoothDampAngle(yawSmooth, yaw, ref yawSmoothVelocity, mouse.rotationSmoothTime);
        pitchSmooth = Mathf.SmoothDampAngle(pitchSmooth, pitch, ref pitchSmoothVelocity, mouse.rotationSmoothTime);

        transform.localRotation = Quaternion.Euler(0f, yawSmooth, 0f);
        cameraSettings.playerCamera.transform.localRotation = Quaternion.Euler(pitchSmooth, 0f, 0f);
    }

    void HandleMovement()
    {
        // === FIX MOVIMIENTO (no salir disparado al bajar los FPS, p. ej. compartiendo pantalla en Discord) ===
        // Limitamos el deltaTime: si un frame tarda mucho (tirón), el paso de movimiento no te lanza fuera del mapa.
        float dt = Mathf.Min(Time.deltaTime, 0.05f);

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        Vector3 desiredMove = (transform.right * input.x + transform.forward * input.y) * movement.moveSpeed;

        // FIX MOVIMIENTO: el SmoothDamp ahora usa su propia variable de velocidad (suavizadoVel).
        // Antes se pasaba currentVelocity como entrada y como ref a la vez y la velocidad se disparaba.
        currentVelocity = Vector3.SmoothDamp(currentVelocity, desiredMove, ref suavizadoVel, movement.movementSmoothness);
        Vector3 horizontalVelocity = currentVelocity;

        if (controller.isGrounded)
        {
            if (verticalVelocity < 0f) verticalVelocity = -2f;
            if (Input.GetButtonDown("Jump"))
            {
                verticalVelocity = movement.jumpForce;

                if (sonidoSalto != null)
                    audioJugador.PlayOneShot(sonidoSalto);
            }
        }
        else
        {
            verticalVelocity -= movement.gravity * dt; // FIX MOVIMIENTO: dt limitado
        }

        Vector3 finalVelocity = horizontalVelocity + Vector3.up * verticalVelocity;
        controller.Move(finalVelocity * dt); // FIX MOVIMIENTO: dt limitado
    }

    public void SetFOV(float fov)
    {
        if (cameraSettings.playerCamera == null) return;
        cameraSettings.playerCamera.fieldOfView = Mathf.Clamp(fov, cameraSettings.minFOV, cameraSettings.maxFOV);
    }
}
