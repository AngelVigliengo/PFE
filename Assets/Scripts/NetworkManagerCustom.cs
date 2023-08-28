using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using System.Collections.Generic; 

public class NetworkManagerCustom : NetworkManager
{
    public GameObject doctorPrefab;
    public GameObject patientPrefab;

    // Variable pour indiquer si le joueur est le docteur
    public bool isDoctor;

    public GameObject levelSelectionMenuObject;
    private LevelSelectionMenu levelSelectionMenu;

    private NetworkMatch networkMatch;

    private void Start()
    {
        networkMatch = gameObject.AddComponent<NetworkMatch>();
        StartMatchmaking();
    }

    private void StartMatchmaking()
    {
        networkMatch.ListMatches(0, 10, "default", true, 0, 0, OnMatchList);
    }

    private void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches)
    {
        if (success && matches != null && matches.Count > 0)
        {
            // Rejoindre le match "default" existant
            var match = matches[0];
            networkMatch.JoinMatch(match.networkId, "", "", "", 0, 0, OnMatchJoined);
        }
        else
        {
            // Créer un nouveau match "default" s'il n'en existe pas
            networkMatch.CreateMatch("default", 4, true, "", "", "", 0, 0, OnMatchCreate);
        }
    }

    private void OnMatchJoined(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        if (success)
        {
            StartClient(matchInfo);
        }
        else
        {
            Debug.Log("Impossible de rejoindre le match.");
        }
    }

    private void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        if (success)
        {
            StartHost(matchInfo);
        }
        else
        {
            Debug.Log("Impossible de créer le match.");
        }
    }

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
