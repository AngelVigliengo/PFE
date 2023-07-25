using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
    private Rigidbody rigidBody;

    public Camera playerCamera;
    public float mouseSensitivity = 3f;
    public float maxLookAngle = 90f;
    public float moveSpeed = 10f;
    public float maxVelocityChange = 7f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    // Variable pour indiquer si le joueur est le docteur
    [SyncVar(hook = nameof(OnIsDoctorChanged))]
    public bool isDoctor = false;

    [SyncVar(hook = nameof(OnCameraRotationChanged))]
    private float cameraPitch = 0f;

    [SyncVar(hook = nameof(OnCameraRotationChanged))]
    private float cameraYaw = 0f;

    private void OnCameraRotationChanged(float newRotation)
    {
        // Mise à jour de la rotation de la caméra sur tous les clients
        if (!isLocalPlayer)
        {
            pitch = cameraPitch;
            yaw = cameraYaw;
            transform.localEulerAngles = new Vector3(0, yaw, 0);
            playerCamera.transform.localEulerAngles = new Vector3(pitch, 0, 0);
        }
    }

    // Fonction appelée lorsque la variable isDoctor est changée
    private void OnIsDoctorChanged(bool newIsDoctor)
    {
        // Implémentez ici le comportement en fonction du rôle du joueur (docteur ou patient)
        if (isLocalPlayer)
        {
            if (isDoctor)
            {
                Debug.Log("Vous êtes le docteur.");
                // Implémentez ici le comportement spécifique au docteur
            }
            else
            {
                Debug.Log("Vous êtes un patient.");
                // Implémentez ici le comportement spécifique au patient
            }
        }
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        yaw = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch = pitch - Input.GetAxis("Mouse Y") * mouseSensitivity;

        pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);

        transform.localEulerAngles = new Vector3(0, yaw, 0);
        playerCamera.transform.localEulerAngles = new Vector3(pitch, 0, 0);

        // Ajoutez cette vérification pour mettre à jour les rotations uniquement sur le joueur local
        if (isLocalPlayer)
        {
            // Envoyer les rotations de la caméra au serveur
            CmdSyncCameraRotations(pitch, yaw);
        }
    }

    // Fonction de synchronisation des rotations de la caméra vers le serveur
    [Command]
    void CmdSyncCameraRotations(float pitch, float yaw)
    {
        cameraPitch = pitch;
        cameraYaw = yaw;
    }

    private void FixedUpdate()
    {
        Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        targetVelocity = transform.TransformDirection(targetVelocity) * moveSpeed;

        Vector3 velocityChange = targetVelocity - rigidBody.velocity;
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0;
        rigidBody.AddForce(velocityChange, ForceMode.VelocityChange);

        Vector3 gravity = new Vector3(0, -9.81f, 0);
        float gravityMultiplayer = 10f;
        gravity *= gravityMultiplayer;

        if (rigidBody.velocity.y < 0)
        {
            rigidBody.AddForce(gravity, ForceMode.Force);
        }
    }
}
