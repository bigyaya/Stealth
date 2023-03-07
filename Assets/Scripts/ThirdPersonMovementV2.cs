using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovementV2 : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public Animator animator;

    //gravité
    Vector3 velocity;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public float gravity = -9.81f;
    public LayerMask groundMask; //"LayerMask" accède au composant Layer
    private bool isGrounded;




    //saut
    public float jumpHeight = 3f;

    //vitesse de déplacement
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    private float currentSpeed;

    //caméra rotation
    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        currentSpeed = walkSpeed; //initialisation de la vitesse de déplacement
    }


    void Update()
    {

        //gère le gravité
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        //vérifie si le joueur touche le sol
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsGrounded", true);

        }

        //gère le saut
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            // Définit le paramètre "Jump" dans l'Animator
            animator.SetBool("IsJumping", true);
            animator.SetBool("IsGrounded", false);


        }



        //recupère les inputs
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        

        // Définit la vitesse de déplacement dans l'Animator
        float speed = direction.magnitude * currentSpeed;
        animator.SetFloat("MoveSpeed", speed);

        // Définit les paramètres SpeedX et SpeedY dans l'Animator
        animator.SetFloat("SpeedX", Mathf.Abs(horizontal));
        animator.SetFloat("SpeedY", Mathf.Abs(vertical));



        //gère la rotation de la caméra
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);


            // Ajoute la vitesse de course si la touche de course est appuyée
            if (Input.GetKey(KeyCode.LeftShift))
            {
                currentSpeed = runSpeed;
            }
            else
            {
                currentSpeed = walkSpeed;
            }

            // Calcule la direction de déplacement
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward * direction.magnitude;

            // Ajoute le mouvement latéral
            if (horizontal != 0f || vertical != 0f)
            {
                float lateralAngle = cam.eulerAngles.y + (horizontal > 0 ? 90f : -90f);
                Vector3 lateralDir = Quaternion.Euler(0f, lateralAngle, 0f) * Vector3.forward;
                moveDir += lateralDir * Mathf.Abs(horizontal) * 0.5f;
                moveDir += cam.forward * Mathf.Abs(vertical) * 0.5f;
            }

   
            
            // Définit les paramètres d'animation en fonction de la direction de déplacement
            animator.SetFloat("SpeedX", moveDir.x);
            animator.SetFloat("SpeedY", moveDir.z);

            // Applique le mouvement
            controller.Move(moveDir.normalized * currentSpeed * Time.deltaTime);
        }
        else
        {
            currentSpeed = 0f;
        }

        // Fait toujours face à la direction de la caméra
        transform.rotation = Quaternion.Euler(0f, cam.eulerAngles.y, 0f);

    }

}




