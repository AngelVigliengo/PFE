using UnityEngine;
using UnityEngine.Networking;

public class Spectator : NetworkBehaviour
{
    public Transform playerCamera; // Référence à la caméra du joueur

    void Update()
    {
        // Assurez-vous que le joueur et la caméra du spectateur sont bien initialisés
        if (playerCamera != null && isServer)
        {
            // Suivre la position du joueur sur le serveur
            RpcUpdateCameraPosition(playerCamera.position);
        }
    }

    [ClientRpc]
    private void RpcUpdateCameraPosition(Vector3 position)
    {
        // Mettre à jour la position de la caméra du spectateur sur les clients
        transform.position = position;
    }
}
