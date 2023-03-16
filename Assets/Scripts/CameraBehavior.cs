using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    [SerializeField] private Transform rightLimit;    // Un point limite � droite pour la cam�ra
    [SerializeField] private Transform leftLimit;     // Un point limite � gauche pour la cam�ra
    [SerializeField] private float rotateSpeed;       // La vitesse de rotation de la cam�ra
    [SerializeField] private Light spotlight;         // La source de lumi�re de la cam�ra

    [SerializeField] public AudioSource alarmSound;    // Un son d'alarme � jouer lorsque le joueur est proche
    [SerializeField] public float alarmDistance = 10f; // La distance � laquelle l'alarme doit �tre d�clench�e

    private bool rightToLeft = true;                   // Un bool�en pour savoir si la cam�ra doit aller de la droite � la gauche
    private Transform target;                          // Le point limite actuel que la cam�ra doit atteindre
    private Transform playerDirection = null;          // La direction dans laquelle le joueur est rep�r�

    // Start est appel�e avant la premi�re frame
    void Start()
    {
        spotlight = GetComponentInChildren<Light>();  // R�cup�re la source de lumi�re de l'enfant de la cam�ra
        target = rightLimit;                          // Initialise la cible � droite
    }

    // Update est appel�e � chaque frame
    void Update()
    {
        // Si la direction du joueur est connue
        if (playerDirection != null)
        {
            float distance = Vector3.Distance(transform.position, playerDirection.position);    // Calcul la distance entre la cam�ra et le joueur
            if (distance > alarmDistance)     // Si la distance est sup�rieure � la distance d'alarme
            {
                playerDirection = null;             // Le joueur n'est plus rep�r�
                spotlight.color = Color.white;      // La source de lumi�re est blanche
            }
            else // Sinon, si le joueur est � port�e
            {
                transform.LookAt(playerDirection.position);      // La cam�ra regarde dans la direction du joueur
                if (!alarmSound.isPlaying)                       // Si le son d'alarme n'est pas en train d'�tre jou�
                {
                    alarmSound.PlayOneShot(alarmSound.clip);      // Joue le son d'alarme une seule fois
                }
                spotlight.color = Color.red;      // La source de lumi�re est rouge
            }
        }
        else // Si la direction du joueur n'est pas connue
        {
            alarmSound.Stop();      // Arr�te le son d'alarme
            PingpongCam();          // Fait bouger la cam�ra de gauche � droite
        }
    }

    // Est appel� lorsqu'un objet entre en collision avec le collider de la cam�ra
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))      // Si l'objet est le joueur
        {
            playerDirection = other.transform;            // Stocke la direction du joueur
            spotlight.color = Color.red;                  // La source de lumi�re est rouge
            alarmSound.PlayOneShot(alarmSound.clip);      // Joue le son d'alarme une seule fois
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        spotlight.color = Color.white;
    //    }
    //}





    // Fait bouger la cam�ra de gauche � droite
    private void PingpongCam()
    {
        Vector3 lookPosition = Vector3.RotateTowards(transform.forward, target.localPosition, rotateSpeed * Time.deltaTime, 0f); // Calcule la position � laquelle la cam�ra doit regarder
        Quaternion lookRotation = Quaternion.LookRotation(lookPosition);      // Calcule la rotation de la cam�ra pour regarder la nouvelle position

        transform.rotation = lookRotation;       // Applique la rotation � la cam�ra

        Quaternion targetRotation = Quaternion.LookRotation(target.localPosition);      // Calcule la rotation de la cible

        // Si la rotation de la cam�ra est proche de la rotation de la cible
        if (Quaternion.Angle(transform.rotation, targetRotation) < 2)
        {
            rightToLeft = !rightToLeft;       // Inverse la direction de la cam�ra

            if (rightToLeft)      // Si la cam�ra va vers la droite
            {
                target = rightLimit;       // D�finit la cible � droite
            }
            else      // Sinon, si la cam�ra va vers la gauche
            {
                target = leftLimit;      // D�finit la cible � gauche
            }
        }
    }
}

//Le script permet donc � la cam�ra de se d�placer de gauche � droite et de suivre le joueur lorsqu'il est � port�e, tout en jouant un son d'alarme et en illuminant la sc�ne en rouge.
