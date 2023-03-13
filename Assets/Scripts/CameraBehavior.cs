using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    [SerializeField] private Transform rightLimit;    // Un point limite à droite pour la caméra
    [SerializeField] private Transform leftLimit;     // Un point limite à gauche pour la caméra
    [SerializeField] private float rotateSpeed;       // La vitesse de rotation de la caméra
    [SerializeField] private Light spotlight;         // La source de lumière de la caméra

    [SerializeField] public AudioSource alarmSound;    // Un son d'alarme à jouer lorsque le joueur est proche
    [SerializeField] public float alarmDistance = 10f; // La distance à laquelle l'alarme doit être déclenchée

    private bool rightToLeft = true;                   // Un booléen pour savoir si la caméra doit aller de la droite à la gauche
    private Transform target;                          // Le point limite actuel que la caméra doit atteindre
    private Transform playerDirection = null;          // La direction dans laquelle le joueur est repéré

    // Start est appelée avant la première frame
    void Start()
    {
        spotlight = GetComponentInChildren<Light>();  // Récupère la source de lumière de l'enfant de la caméra
        target = rightLimit;                          // Initialise la cible à droite
    }

    // Update est appelée à chaque frame
    void Update()
    {
        // Si la direction du joueur est connue
        if (playerDirection != null)
        {
            float distance = Vector3.Distance(transform.position, playerDirection.position);    // Calcul la distance entre la caméra et le joueur
            if (distance > alarmDistance)     // Si la distance est supérieure à la distance d'alarme
            {
                playerDirection = null;             // Le joueur n'est plus repéré
                spotlight.color = Color.white;      // La source de lumière est blanche
            }
            else // Sinon, si le joueur est à portée
            {
                transform.LookAt(playerDirection.position);      // La caméra regarde dans la direction du joueur
                if (!alarmSound.isPlaying)                       // Si le son d'alarme n'est pas en train d'être joué
                {
                    alarmSound.PlayOneShot(alarmSound.clip);      // Joue le son d'alarme une seule fois
                }
                spotlight.color = Color.red;      // La source de lumière est rouge
            }
        }
        else // Si la direction du joueur n'est pas connue
        {
            alarmSound.Stop();      // Arrête le son d'alarme
            PingpongCam();          // Fait bouger la caméra de gauche à droite
        }
    }

    // Est appelé lorsqu'un objet entre en collision avec le collider de la caméra
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))      // Si l'objet est le joueur
        {
            playerDirection = other.transform;            // Stocke la direction du joueur
            spotlight.color = Color.red;                  // La source de lumière est rouge
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





    // Fait bouger la caméra de gauche à droite
    private void PingpongCam()
    {
        Vector3 lookPosition = Vector3.RotateTowards(transform.forward, target.localPosition, rotateSpeed * Time.deltaTime, 0f); // Calcule la position à laquelle la caméra doit regarder
        Quaternion lookRotation = Quaternion.LookRotation(lookPosition);      // Calcule la rotation de la caméra pour regarder la nouvelle position

        transform.rotation = lookRotation;       // Applique la rotation à la caméra

        Quaternion targetRotation = Quaternion.LookRotation(target.localPosition);      // Calcule la rotation de la cible

        // Si la rotation de la caméra est proche de la rotation de la cible
        if (Quaternion.Angle(transform.rotation, targetRotation) < 2)
        {
            rightToLeft = !rightToLeft;       // Inverse la direction de la caméra

            if (rightToLeft)      // Si la caméra va vers la droite
            {
                target = rightLimit;       // Définit la cible à droite
            }
            else      // Sinon, si la caméra va vers la gauche
            {
                target = leftLimit;      // Définit la cible à gauche
            }
        }
    }
}

//Le script permet donc à la caméra de se déplacer de gauche à droite et de suivre le joueur lorsqu'il est à portée, tout en jouant un son d'alarme et en illuminant la scène en rouge.
