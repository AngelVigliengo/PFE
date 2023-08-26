using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


public class LevelSelectionMenu : NetworkBehaviour
{
    public GameObject Panel;

    public bool isActive = false;
    private PlayerController localPlayer;

    public Dropdown arachnophobieButton;
    public Dropdown acrophobieButton;
    public Dropdown ophiophobieButton;
    public GameObject patientPrefab;

    // Méthode pour obtenir la référence au PlayerController local
    public void SetLocalPlayer(PlayerController player)
    {
        localPlayer = player;
    }

    // Fonction de téléportation du patient vers une position donnée
    private void TeleportPatientToPosition(Vector3 position)
    {
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

    void Start()
    {
        Panel.SetActive(isActive);
    }

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
                localPlayer.RpcChangeAudioIndex(0, 0);
                localPlayer.CmdChangeSkybox(0, 0);
                break;
            case 1:
                TeleportPatientToPosition(new Vector3(25f, 30f, 25f));
                localPlayer.RpcChangeAudioIndex(1, 0);
                localPlayer.CmdChangeSkybox(1, 0);
                break;
            case 2:
                TeleportPatientToPosition(new Vector3(25f, 30f, 25f));
                localPlayer.RpcChangeAudioIndex(2, 0);
                localPlayer.CmdChangeSkybox(1, 0);
                break;
            case 3:
                TeleportPatientToPosition(new Vector3(5f, 1f, 3f));
                localPlayer.RpcChangeAudioIndex(0, 0);
                localPlayer.CmdChangeSkybox(0, 0);
                break;
            case 4:
                TeleportPatientToPosition(new Vector3(22.5f, 1f, 3f));
                localPlayer.RpcChangeAudioIndex(0, 0);
                localPlayer.CmdChangeSkybox(0, 0);
                break;
            case 5:
                TeleportPatientToPosition(new Vector3(31f, 1f, 3f));
                localPlayer.RpcChangeAudioIndex(0, 0);
                localPlayer.CmdChangeSkybox(0, 0);
                break;
            case 6:
                TeleportPatientToPosition(new Vector3(13.5f, 09f, 3f));
                localPlayer.RpcChangeAudioIndex(0, 0);
                localPlayer.CmdChangeSkybox(0, 0);
                break;
            case 7:
                TeleportPatientToPosition(new Vector3(13.5f, 09f, 3f));
                localPlayer.RpcChangeAudioIndex(2, 0);
                localPlayer.CmdChangeSkybox(0, 0);
                break;
        }
    }

    public void AcrophobieLevelSelection()
    {
        int selectedIndex = acrophobieButton.value;

        switch (selectedIndex)
        {
            case 0:
                TeleportPatientToPosition(new Vector3(25f, 30f, 25f));
                localPlayer.RpcChangeAudioIndex(0, 1);
                localPlayer.CmdChangeSkybox(0, 1);
                break;
            case 1:
                TeleportPatientToPosition(new Vector3(25f, -22f, 25f));
                localPlayer.RpcChangeAudioIndex(1, 1);
                localPlayer.CmdChangeSkybox(0, 1);
                break;
            case 2:
                TeleportPatientToPosition(new Vector3(25f, -22f, 25f));
                localPlayer.RpcChangeAudioIndex(2, 1);
                localPlayer.CmdChangeSkybox(0, 1);
                break;
            case 3:
                TeleportPatientToPosition(new Vector3(25f, -22f, 25f));
                localPlayer.RpcChangeAudioIndex(0, 1);
                localPlayer.CmdChangeSkybox(2, 1);
                break;
            case 4:
                TeleportPatientToPosition(new Vector3(40f, 1f, 23f));
                localPlayer.RpcChangeAudioIndex(0, 1);
                localPlayer.CmdChangeSkybox(0, 1);
                break;
            case 5:
                TeleportPatientToPosition(new Vector3(40f, 1f, 3f));
                localPlayer.RpcChangeAudioIndex(0, 1);
                localPlayer.CmdChangeSkybox(0, 1);
                break;
            case 6:
                TeleportPatientToPosition(new Vector3(25f, -22f, 25f));
                localPlayer.RpcChangeAudioIndex(0, 1);
                localPlayer.CmdChangeSkybox(1, 1);
                break;
            case 7:
                TeleportPatientToPosition(new Vector3(25f, -22f, 25f));
                localPlayer.RpcChangeAudioIndex(2, 1);
                localPlayer.CmdChangeSkybox(1, 1);
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
                localPlayer.RpcChangeAudioIndex(0,2);
                localPlayer.CmdChangeSkybox(0,2);
                break;
            case 1:
                TeleportPatientToPosition(new Vector3(25f, 30f, 25f));
                localPlayer.RpcChangeAudioIndex(1, 2);
                localPlayer.CmdChangeSkybox(1, 2);
                break;
            case 2:
                TeleportPatientToPosition(new Vector3(25f, 30f, 25f));
                localPlayer.RpcChangeAudioIndex(2, 2);
                localPlayer.CmdChangeSkybox(1, 2);
                break;
            case 3:
                TeleportPatientToPosition(new Vector3(5f, 1f, 23f));
                localPlayer.RpcChangeAudioIndex(0, 2);
                localPlayer.CmdChangeSkybox(0, 2);
                break;
            case 4:
                TeleportPatientToPosition(new Vector3(22.5f, 1f, 23f));
                localPlayer.RpcChangeAudioIndex(0, 2);
                localPlayer.CmdChangeSkybox(0, 2);
                break;
            case 5:
                TeleportPatientToPosition(new Vector3(31f, 1f, 23f));
                localPlayer.RpcChangeAudioIndex(0, 2);
                localPlayer.CmdChangeSkybox(0, 2);
                break;
            case 6:
                TeleportPatientToPosition(new Vector3(13.5f, 09f, 23f));
                localPlayer.RpcChangeAudioIndex(0, 2);
                localPlayer.CmdChangeSkybox(0, 2);
                break;
            case 7:
                TeleportPatientToPosition(new Vector3(13.5f, 09f, 23f));
                localPlayer.RpcChangeAudioIndex(2, 2);
                localPlayer.CmdChangeSkybox(0, 2);
                break;
        }
    }
}
