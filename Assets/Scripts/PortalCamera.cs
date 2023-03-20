using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCamera : MonoBehaviour
{

    public Transform playerCamera; // Référence à la caméra du joueur
    public Transform portal; // Référence au portail actuel
    public Transform otherPortal; // Référence à l'autre portail connecté

    void LateUpdate()
    {
        // Calcul de la distance entre la caméra du joueur et l'autre portail
        Vector3 playerOffsetFromPortal = playerCamera.position - otherPortal.position;

        // Positionnement de la caméra en fonction de la position du portail actuel et de la distance entre la caméra et l'autre portail
        transform.position = portal.position + playerOffsetFromPortal;

        // Calcul de la différence d'angle entre la rotation du portail actuel et la rotation de l'autre portail
        float angularDifferenceBeteweenPortalRotations = Quaternion.Angle(portal.rotation, otherPortal.rotation);

        // Création d'un quaternion représentant la rotation nécessaire pour aligner la caméra du joueur avec le portail actuel
        Quaternion portalRotationDifference = Quaternion.AngleAxis(angularDifferenceBeteweenPortalRotations, Vector3.up);

        // Calcul de la nouvelle direction de la caméra en fonction de la rotation du portail actuel et de la direction de la caméra du joueur
        Vector3 newCameraDirection = portalRotationDifference * playerCamera.forward;

        // Rotation de la caméra en fonction de sa nouvelle direction et de la direction verticale (up) du monde
        transform.rotation = Quaternion.LookRotation(newCameraDirection, Vector3.up);
    }
}
