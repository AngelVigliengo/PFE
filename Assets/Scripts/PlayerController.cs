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

    public Material[] arachnophobieSkybox;
    public Material[] acroophobieSkybox;
    public Material[] ophiophobieSkybox;

    public AudioSource[] aracnoAudioSources;
    public AudioSource[] acroAudioSources;
    public AudioSource[] ophioAudioSources;

    // Variable pour indiquer si le joueur est le docteur
    [SyncVar(hook = nameof(OnIsDoctorChanged))]
    public bool isDoctor = false;

    [SyncVar(hook = nameof(OnCameraRotationChanged))]
    private float cameraPitch = 0f;

    [SyncVar(hook = nameof(OnCameraRotationChanged))]
    private float cameraYaw = 0f;

    [Command]
    public void CmdChangePlayerPosition(Vector3 position)
    {
        RpcChangePlayerPosition(position);
    }

    [ClientRpc]
    public void RpcChangePlayerPosition(Vector3 position)
    {
        OnPlayerPositionChanged(position);
    }

    [Command]
    public void CmdChangeSkybox(int skyboxIndex, int levelIndex)
    {
        RpcChangeSkybox(skyboxIndex, levelIndex);
    }

    [ClientRpc]
    public void RpcChangeSkybox(int skyboxIndex, int levelIndex)
    {
        ChangeSkyboxByIndex(skyboxIndex, levelIndex);
    }

    // Fonction de synchronisation des changements d'audio vers le serveur
    [Command]
    public void CmdSyncAudioIndex(int audioIndex, int levelIndex)
    {
        // Appeler la fonction sur tous les clients pour changer l'audio
        RpcChangeAudioIndex(audioIndex, levelIndex);
    }

    // Fonction RPC pour synchroniser les changements d'audio sur tous les clients
    [ClientRpc]
    public void RpcChangeAudioIndex(int audioIndex, int levelIndex)
    {
        ChangeAudioByIndex(audioIndex, levelIndex);
    }

    void ChangeSkyboxByIndex(int skyboxIndex, int levelIndex)
    {
        Material newSkyboxMaterial = null;

        switch (levelIndex)
        {
            case 0:
                newSkyboxMaterial = arachnophobieSkybox[skyboxIndex];
                break;
            case 1:
                newSkyboxMaterial = acroophobieSkybox[skyboxIndex];
                break;
            case 2:
                newSkyboxMaterial = ophiophobieSkybox[skyboxIndex];
                break;
        }

        if (newSkyboxMaterial != null)
        {
            RenderSettings.skybox = newSkyboxMaterial;
        }
    }

    void ChangeAudioByIndex(int audioIndex, int levelIndex)
    {
        foreach (AudioSource audioSource in GetComponentsInChildren<AudioSource>())
        {
            audioSource.Stop();
            audioSource.gameObject.SetActive(false);
        }

        AudioSource selectedAudio = new AudioSource();

        switch (levelIndex)
        {
            case 0:
                selectedAudio = aracnoAudioSources[audioIndex];
                break;
            case 1:
                selectedAudio = acroAudioSources[audioIndex];
                break;
            case 2:
                selectedAudio = ophioAudioSources[audioIndex];
                break;
        }

        selectedAudio.gameObject.SetActive(true);
        selectedAudio.Play();
    }


    public override void OnStartLocalPlayer()
    {
        GameObject levelSelectionMenuObj = GameObject.FindGameObjectWithTag("LevelSelectionMenu");
        if (levelSelectionMenuObj != null)
        {
            levelSelectionMenu = levelSelectionMenuObj.GetComponent<LevelSelectionMenu>();
            levelSelectionMenu.SetLocalPlayer(this);
        }
    }

    private void OnIsDoctorChanged(bool newIsDoctor)
    {
        if (isLocalPlayer)
        {
            if (isDoctor)
            {
                Debug.Log("Vous êtes le docteur.");
            }
            else
            {
                Debug.Log("Vous êtes un patient.");
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
        transform.position = newPosition;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rigidBody = GetComponent<Rigidbody>();
        ChangeAudioByIndex(0,0);
    }

    void Update()
    {
        isMenuActive = levelSelectionMenu != null && levelSelectionMenu.isActive;

        if (isDoctor)
        {
            Cursor.visible = isMenuActive;
            Cursor.lockState = levelSelectionMenu.isActive ? CursorLockMode.None : CursorLockMode.Locked;
        }
        else
        {
            GameObject currentDoctor = GameObject.FindGameObjectWithTag("GameController");
            if (currentDoctor != null)
            {
                transform.position = currentDoctor.transform.position;
            }
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
