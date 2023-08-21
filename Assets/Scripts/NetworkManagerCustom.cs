using UnityEngine;
using UnityEngine.Networking;

public class NetworkManagerCustom : NetworkManager
{
    public GameObject doctorPrefab;
    public GameObject patientPrefab;

    // Variable pour indiquer si le joueur est le docteur
    public bool isDoctor;

    public GameObject levelSelectionMenuObject;
    private LevelSelectionMenu levelSelectionMenu;

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        GameObject playerPrefab = isDoctor ? doctorPrefab : patientPrefab;

        GameObject player = Instantiate(playerPrefab);

        // Rechercher le GameObject contenant le script LevelSelectionMenu
        if (levelSelectionMenuObject != null)
        {
            levelSelectionMenu = levelSelectionMenuObject.GetComponent<LevelSelectionMenu>();
        }

        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            if (isDoctor)
            {
                playerController.isDoctor = true;
                // Attribuer la référence au LevelSelectionMenu au joueur (docteur)
                playerController.levelSelectionMenu = levelSelectionMenu;
            }
            else
            {
                // Si le joueur est un patient, mettez la caméra du patient dans le script Spectator du joueur (docteur)
                Spectator spectator = player.GetComponent<Spectator>();
                if (spectator != null)
                {
                    doctorPrefab.GetComponent<PlayerController>().playerCamera = patientPrefab.GetComponentInChildren<Camera>();
                }
            }
        }

        // Ajouter le joueur à la connexion réseau
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);

        // Inverser la variable pour que le prochain joueur soit un patient
        isDoctor = !isDoctor;
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);

        // Si le joueur qui héberge la partie se déconnecte, réinitialiser la variable pour qu'il soit le docteur par défaut lorsqu'il se reconnecte
        if (numPlayers == 0)
        {
            isDoctor = true;
        }
    }
}
