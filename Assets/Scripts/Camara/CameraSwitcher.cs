using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Transform player; // Referencia al jugador
    public Camera mainCamera; // Cámara principal
    public Vector3 thirdPersonOffset = new Vector3(0, 3, -5); // Offset para tercera persona
    public Vector3 topDownOffset = new Vector3(0, 10, 0); // Offset para vista cenital

    private enum CameraView { FirstPerson, ThirdPerson, TopDown }
    private CameraView currentView;

    void Start()
    {
        // Buscar automáticamente el jugador si no está asignado
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }

        // Asegurar que haya una cámara asignada
        if (mainCamera == null)
            mainCamera = Camera.main;

        // Comenzar en tercera persona
        SetThirdPersonView();
    }

    void Update()
    {
        // Cambiar entre vistas con las teclas
        if (Input.GetKeyDown(KeyCode.T))
        {
            SetFirstPersonView();
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            SetThirdPersonView();
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            SetTopDownView();
        }

        // Actualizar la posición de la cámara según la vista activa
        FollowPlayer();
    }

    void FollowPlayer()
    {

        switch (currentView)
        {
            case CameraView.FirstPerson:
                mainCamera.transform.position = player.position + new Vector3(0, 0.8f, 0); // Vista en primera persona
                mainCamera.transform.rotation = player.rotation;
                break;

            case CameraView.ThirdPerson:
                mainCamera.transform.position = Vector3.Lerp(
                    mainCamera.transform.position,
                    player.position + thirdPersonOffset,
                    Time.deltaTime * 10 // Suavizado del movimiento de la cámara
                );
                mainCamera.transform.LookAt(player);
                break;

            case CameraView.TopDown:
                mainCamera.transform.position = Vector3.Lerp(
                    mainCamera.transform.position,
                    player.position + topDownOffset,
                    Time.deltaTime * 10
                );
                mainCamera.transform.rotation = Quaternion.Euler(90, 0, 0); // Orientación cenital
                break;
        }
    }

    void SetFirstPersonView()
    {
        currentView = CameraView.FirstPerson;
    }

    void SetThirdPersonView()
    {
        currentView = CameraView.ThirdPerson;
    }

    void SetTopDownView()
    {
        currentView = CameraView.TopDown;
    }
}
