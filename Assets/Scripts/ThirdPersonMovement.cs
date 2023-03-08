using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
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

        //gère la gravité
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

        //gère le sneak
        if (Input.GetButtonDown("Sneak"))
        {
            if (animator.GetBool("IsSneaking"))
            {
                animator.SetBool("IsSneaking", false);
            }
            else
            {
                animator.SetBool("IsSneaking", true);
            }
        }




        //recupère les inputs
        float horirzontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        float cameraHorizontal = Input.GetAxisRaw("JoystickRightHorizontal");
        float cameraVertical = Input.GetAxisRaw("JoystickRightVertical");

        // Fait tourner la caméra en fonction des axes du joystick droit
        //transform.Rotate(Vector3.up, cameraHorizontal * Time.deltaTime * turnSmoothVelocity);
        //transform.Rotate(Vector3.right, cameraVertical * Time.deltaTime * turnSmoothVelocity);


        Vector3 direction = new Vector3(horirzontal, 0f, vertical).normalized;

        // Arrête l'animation "sneak" si le joueur ne bouge pas
        if (direction.magnitude < 0.1f && animator.GetBool("IsSneaking"))
        {
            animator.SetBool("IsSneaking", false);

        }

        // Définit la vitesse de déplacement dans l'Animator
        float speed = direction.magnitude * currentSpeed;
        animator.SetFloat("MoveSpeed", speed);

        // Définit les paramètres SpeedX et SpeedY dans l'Animator
        animator.SetFloat("SpeedX", Mathf.Abs(horirzontal));
        animator.SetFloat("SpeedY", Mathf.Abs(vertical));

        //animator.SetBool("IsRunning", speed > walkSpeed);



        //gère la rotation de la caméra
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward * direction.magnitude;

            // Ajoute la vitesse de course si la touche LeftShift est appuyée ou si le joystick gauche est poussé à fond
            if (Input.GetKey(KeyCode.LeftShift) /*|| Input.GetAxisRaw("Run") > 0f*/)
            {
                currentSpeed = runSpeed;
            }
            else
            {
                currentSpeed = walkSpeed;
            }

            controller.Move(moveDir.normalized * currentSpeed * Time.deltaTime);
        }




    }


}
