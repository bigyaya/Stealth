using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCamera : MonoBehaviour
{

    public Transform playerCamera; // R�f�rence � la cam�ra du joueur
    public Transform portal; // R�f�rence au portail actuel
    public Transform otherPortal; // R�f�rence � l'autre portail connect�

    void LateUpdate()
    {
        // Calcul de la distance entre la cam�ra du joueur et l'autre portail
        Vector3 playerOffsetFromPortal = playerCamera.position - otherPortal.position;

        // Positionnement de la cam�ra en fonction de la position du portail actuel et de la distance entre la cam�ra et l'autre portail
        transform.position = portal.position + playerOffsetFromPortal;

        // Calcul de la diff�rence d'angle entre la rotation du portail actuel et la rotation de l'autre portail
        float angularDifferenceBeteweenPortalRotations = Quaternion.Angle(portal.rotation, otherPortal.rotation);

        // Cr�ation d'un quaternion repr�sentant la rotation n�cessaire pour aligner la cam�ra du joueur avec le portail actuel
        Quaternion portalRotationDifference = Quaternion.AngleAxis(angularDifferenceBeteweenPortalRotations, Vector3.up);

        // Calcul de la nouvelle direction de la cam�ra en fonction de la rotation du portail actuel et de la direction de la cam�ra du joueur
        Vector3 newCameraDirection = portalRotationDifference * playerCamera.forward;

        // Rotation de la cam�ra en fonction de sa nouvelle direction et de la direction verticale (up) du monde
        transform.rotation = Quaternion.LookRotation(newCameraDirection, Vector3.up);
    }
}
