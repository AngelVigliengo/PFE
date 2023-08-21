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
    public LevelSelectionMenu levelSelectionMenu;

    private float yaw = 0.0f;
    private float pitch = 0.0f;
    private bool isMenuActive = false;

    // Variable pour indiquer si le joueur est le docteur
    [SyncVar(hook = nameof(OnIsDoctorChanged))]
    public bool isDoctor = false;

    [SyncVar(hook = nameof(OnCameraRotationChanged))]
    private float cameraPitch = 0f;

    [SyncVar(hook = nameof(OnCameraRotationChanged))]
    private float cameraYaw = 0f;

    [SyncVar(hook = nameof(OnPlayerPositionChanged))]
    public Vector3 playerPosition = Vector3.zero;

    public override void OnStartLocalPlayer()
    {
        // Appeler la méthode SetLocalPlayer du LevelSelectionMenu pour définir la référence au PlayerController local
        GameObject levelSelectionMenuObj = GameObject.FindGameObjectWithTag("LevelSelectionMenu");
        if (levelSelectionMenuObj != null)
        {
            levelSelectionMenu = levelSelectionMenuObj.GetComponent<LevelSelectionMenu>();
            levelSelectionMenu.SetLocalPlayer(this);
        }
    }

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

    private void OnPlayerPositionChanged(Vector3 newPosition)
    {
        // Mise à jour de la position du joueur sur les clients
        if (!isLocalPlayer)
        {
            transform.position = newPosition;
        }
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        isMenuActive = levelSelectionMenu != null && levelSelectionMenu.isActive;

        if (isDoctor)
        {
            Cursor.visible = isMenuActive;
            Cursor.lockState = levelSelectionMenu.isActive ? CursorLockMode.None : CursorLockMode.Locked;
        }
        
        // Mettre à jour la rotation de la caméra uniquement si le menu n'est pas actif
        if (!isMenuActive)
        {
            yaw = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivity;
            pitch = pitch - Input.GetAxis("Mouse Y") * mouseSensitivity;

            pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);

            transform.localEulerAngles = new Vector3(0, yaw, 0);
            playerCamera.transform.localEulerAngles = new Vector3(pitch, 0, 0);

            if (isLocalPlayer)
            {
                // Envoyer les rotations de la caméra au serveur
                CmdSyncCameraRotations(pitch, yaw);

                // Envoyer la position du joueur au serveur
                CmdSyncPlayerPosition(transform.position);
            }
        }
        
    }

    // Fonction de synchronisation des rotations de la caméra vers le serveur
    [Command]
    void CmdSyncCameraRotations(float pitch, float yaw)
    {
        cameraPitch = pitch;
        cameraYaw = yaw;
    }

    // Fonction de synchronisation de la position du joueur vers le serveur
    [Command]
    void CmdSyncPlayerPosition(Vector3 position)
    {
        playerPosition = position;
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
        float gravityMultiplier = 10f;
        gravity *= gravityMultiplier;

        if (rigidBody.velocity.y < 0)
        {
            rigidBody.AddForce(gravity, ForceMode.Force);
        }
    }

    // Fonction pour mettre à jour la caméra du docteur avec celle du patient
    public void UpdateDoctorCamera(Vector3 patientPosition)
    {
        // Calculer la direction du patient par rapport au docteur
        Vector3 direction = patientPosition - transform.position;
        direction.y = 0f;

        // Calculer la rotation à appliquer à la caméra du docteur pour regarder vers le patient
        Quaternion newRotation = Quaternion.LookRotation(direction);

        // Mettre à jour la rotation de la caméra du docteur
        playerCamera.transform.rotation = newRotation;
    }
}
