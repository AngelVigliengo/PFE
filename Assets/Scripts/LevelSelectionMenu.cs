using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


public class LevelSelectionMenu : NetworkBehaviour
{
    public GameObject Panel;

    public bool isActive = false;
    private PlayerController localPlayer;

    public Material normalSkybox;
    public Material arachnophobieSkybox;

    public AudioSource normalAudio;
    public AudioSource arachnophobie1Audio;
    public AudioSource arachnophobie6Audio;

    public Dropdown arachnophobieButton;
    public Dropdown acrophobieButton;
    public Dropdown ophiophobieButton;
    public GameObject patientPrefab;


    private void ChangeSkyboxAndAudio(Material skybox, AudioSource audio)
    {
        RenderSettings.skybox = skybox;

        if (audio.isPlaying)
        {
            audio.Stop();
        }
        audio.Play();
    }

    public void SetNormalLevel()
    {
        ChangeSkyboxAndAudio(normalSkybox, normalAudio);
    }

    public void SetArachnophobieLevel1()
    {
        ChangeSkyboxAndAudio(arachnophobieSkybox, arachnophobie1Audio);
    }

    public void SetArachnophobieLevel6()
    {
        ChangeSkyboxAndAudio(arachnophobieSkybox, arachnophobie6Audio);
    }

    private void ResetAudioAndSkybox()
    {
        if (normalAudio.isPlaying)
        {
            normalAudio.Stop();
        }

        if (arachnophobie1Audio.isPlaying)
        {
            arachnophobie1Audio.Stop();
        }

        if(arachnophobie6Audio.isPlaying)
        {
            arachnophobie6Audio.Stop();
        }

        // Ajoute d'autres AudioSources ici si nécessaire
    }


    // Méthode pour obtenir la référence au PlayerController local
    public void SetLocalPlayer(PlayerController player)
    {
        localPlayer = player;
    }

    // Fonction de téléportation du patient vers une position donnée
    private void TeleportPatientToPosition(Vector3 position)
    {
        /*// Recherchez le patient existant
        GameObject currentPatient = GameObject.FindGameObjectWithTag("Player");
        if (currentPatient != null)
        {            

            currentPatient.transform.position = position;

            PlayerController patientController = currentPatient.GetComponent<PlayerController>();
            patientController.capsuleCollider.enabled = false;
            if (patientController != null)
            {
                patientController.CmdChangePlayerPosition(position);
            }

            patientController.capsuleCollider.enabled =  true;
        }*/

        GameObject currentDoctor = GameObject.FindGameObjectWithTag("GameController");
        if (currentDoctor != null)
        {
            currentDoctor.transform.position = position;
            PlayerController doctorController = currentDoctor.GetComponent<PlayerController>();
            if (doctorController != null)
            {
                doctorController.CmdChangePlayerPosition(position);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //ResetAudioAndSkybox();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isActive = !isActive;
            Panel.SetActive(isActive);
        }
    }

    public void CloseButon()
    {
        if (isActive)
        {
            isActive = !isActive;
            Panel.SetActive(isActive);
        }
    }

    public void AracnophobieLevelSelection()
    {
        int selectedIndex = arachnophobieButton.value;

        switch (selectedIndex)
        {
            case 0:
                TeleportPatientToPosition(new Vector3(25f, 30f, 25f));
                localPlayer.CmdChangeSkybox(0, 0);
                localPlayer.RpcChangeAudioIndex(0,0);
                break;
            case 1:
                TeleportPatientToPosition(new Vector3(25f, 30f, 25f));
                localPlayer.CmdChangeSkybox(1, 0);
                localPlayer.RpcChangeAudioIndex(1,0);
                break;
            case 2:
                TeleportPatientToPosition(new Vector3(25f, 30f, 25f));
                localPlayer.CmdChangeSkybox(1, 0);
                localPlayer.RpcChangeAudioIndex(2,0);
                break;
            case 3:
                TeleportPatientToPosition(new Vector3(5f, 1f, 3f));
                localPlayer.CmdChangeSkybox(0, 0);
                localPlayer.RpcChangeAudioIndex(0,0);
                break;
            case 4:
                TeleportPatientToPosition(new Vector3(24f, 1f, 3f));
                localPlayer.CmdChangeSkybox(0, 0);
                localPlayer.RpcChangeAudioIndex(0,0);
                break;
            case 5:
                TeleportPatientToPosition(new Vector3(13.5f, 09f, 3f));
                localPlayer.CmdChangeSkybox(0, 0);
                localPlayer.RpcChangeAudioIndex(0,0);
                break;
            case 6:
                TeleportPatientToPosition(new Vector3(13.5f, 09f, 3f));
                localPlayer.CmdChangeSkybox(0, 0);
                localPlayer.RpcChangeAudioIndex(2,0);
                break;
        }
    }

    public void AcrophobieLevelSelection()
    {
        int selectedIndex = acrophobieButton.value;

        switch (acrophobieButton.value)
        {
            case 0:
                TeleportPatientToPosition(new Vector3(25f, 30f, 25f));
                localPlayer.CmdChangeSkybox(0, 1);
                localPlayer.RpcChangeAudioIndex(0,0);
                break;
            case 1:
                TeleportPatientToPosition(new Vector3(0f, 1f, 300f));
                localPlayer.CmdChangeSkybox(1, 1);
                localPlayer.RpcChangeAudioIndex(1,1);
                break;
            case 2:
                TeleportPatientToPosition(new Vector3(0f, 1f, 320f));
                localPlayer.CmdChangeSkybox(1, 1);
                localPlayer.RpcChangeAudioIndex(2,1);
                break;
            case 3:
                TeleportPatientToPosition(new Vector3(20f, 1f, 300f));
                localPlayer.CmdChangeSkybox(2, 1);
                localPlayer.RpcChangeAudioIndex(0,0);
                break;
            case 4:
                TeleportPatientToPosition(new Vector3(20f, 1f, 320f));
                localPlayer.CmdChangeSkybox(1, 1);
                localPlayer.RpcChangeAudioIndex(0,0);
                break;
            case 5:
                TeleportPatientToPosition(new Vector3(13.5f, 09f, 3f));
                localPlayer.CmdChangeSkybox(1, 1);
                localPlayer.RpcChangeAudioIndex(2,1);
                break;
        }
    }

    public void OphiophobieLevelSelection()
    {
        int selectedIndex = ophiophobieButton.value;

        switch (selectedIndex)
        {
            case 0:
                TeleportPatientToPosition(new Vector3(25f, 30f, 25f));
                localPlayer.CmdChangeSkybox(0, 2);
                localPlayer.RpcChangeAudioIndex(0, 2);
                break;
            case 1:
                TeleportPatientToPosition(new Vector3(25f, 30f, 25f));
                localPlayer.CmdChangeSkybox(0, 2);
                localPlayer.RpcChangeAudioIndex(1, 2);
                break;
            case 2:
                TeleportPatientToPosition(new Vector3(5f, 1f, 3f));
                localPlayer.CmdChangeSkybox(0, 2);
                localPlayer.RpcChangeAudioIndex(2, 2);
                break;
            case 3:
                TeleportPatientToPosition(new Vector3(5f, 1f, 3f));
                localPlayer.CmdChangeSkybox(0, 2);
                localPlayer.RpcChangeAudioIndex(0, 2);
                break;
            case 4:
                TeleportPatientToPosition(new Vector3(5f, 1f, 3f));
                localPlayer.CmdChangeSkybox(0, 2);
                localPlayer.RpcChangeAudioIndex(0, 2);
                break;
            case 5:
                TeleportPatientToPosition(new Vector3(13.5f, 09f, 3f));
                localPlayer.CmdChangeSkybox(0, 2);
                localPlayer.RpcChangeAudioIndex(2, 2);
                break;
        }
    }
}
