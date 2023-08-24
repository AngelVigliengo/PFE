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

    // Référence au préfab du patient (assurez-vous de le définir dans l'inspecteur Unity)
    public GameObject patientPrefab;

    // Méthode pour obtenir la référence au PlayerController local
    public void SetLocalPlayer(PlayerController player)
    {
        localPlayer = player;
    }

    // Fonction de téléportation du patient vers une position donnée
    private void TeleportPatientToPosition(Vector3 position)
    {
        // Assurez-vous que le patientPrefab est défini dans l'inspecteur Unity
        if (patientPrefab != null)
        {
            // Recherchez le patient existant
            GameObject currentPatient = GameObject.FindGameObjectWithTag("Player");
            if (currentPatient != null)
            {
                // Changez simplement la position du patient existant
                currentPatient.transform.position = position;
                Debug.Log("TP");
                Debug.Log(currentPatient.transform.position);
            }
            else
            {
                Debug.Log("Pas de joueur trouvé");
            }
        }
        else
        {
            Debug.LogError("Le préfab du patient n'est pas défini dans le script LevelSelectionMenu.");
        }

        GameObject currentDoctor = GameObject.FindGameObjectWithTag("GameController");
        if (currentDoctor != null)
        {
            currentDoctor.transform.position = position;
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {

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

    public void AcrophopieLevelSelection()
    {
        Debug.Log(acrophobieButton.value);
        switch (acrophobieButton.value)
        {
            case 0:
                TeleportPatientToPosition(new Vector3(25f, 30f, 25f));
            Debug.Log("Spawn");
            break;
            case 1:
                TeleportPatientToPosition(new Vector3(4.5f, 0.7f, 4f));
            break;
            case 2:
                TeleportPatientToPosition(new Vector3(13.5f, 0.9f, 3f));
            break;
        }
    }

    public void AracnophobieLevelSelection()
    {
        switch (arachnophobieButton.value)
        {
            case 0:
                TeleportPatientToPosition(new Vector3(25f, 30f, 25f));
                Debug.Log("Spawn");
                break;
            case 1:
                TeleportPatientToPosition(new Vector3(4.5f, 0.7f, 4f));
                break;
            case 2:
                TeleportPatientToPosition(new Vector3(13.5f, 0.9f, 3f));
                break;
        }
    }
}
