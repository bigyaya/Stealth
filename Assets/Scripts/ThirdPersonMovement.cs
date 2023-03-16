using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{

    public CharacterController controller;
    public Transform cam;
    public Animator animator;


    //gravit�
    Vector3 velocity;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public float gravity = -9.81f;
    public LayerMask groundMask; //"LayerMask" acc�de au composant Layer
    private bool isGrounded;

    //saut
    public float jumpHeight = 3f;

    //vitesse de d�placement
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    private float currentSpeed;

    //cam�ra rotation
    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        currentSpeed = walkSpeed; //initialisation de la vitesse de d�placement
    }


    void Update()
    {

        //g�re la gravit�
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        //v�rifie si le joueur touche le sol
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsGrounded", true);
        }

        //g�re le saut
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            // D�finit le param�tre "Jump" dans l'Animator
            animator.SetBool("IsJumping", true);
            animator.SetBool("IsGrounded", false);
        }

        //g�re le sneak
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



    //recup�re les inputs
    float horirzontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        //float cameraHorizontal = Input.GetAxisRaw("JoystickRightHorizontal");
        //float cameraVertical = Input.GetAxisRaw("JoystickRightVertical");

        // Fait tourner la cam�ra en fonction des axes du joystick droit
        //transform.Rotate(Vector3.up, cameraHorizontal * Time.deltaTime * turnSmoothVelocity);
        //transform.Rotate(Vector3.right, cameraVertical * Time.deltaTime * turnSmoothVelocity);


        Vector3 direction = new Vector3(horirzontal, 0f, vertical).normalized;

        // Arr�te l'animation "sneak" si le joueur ne bouge pas
        if (direction.magnitude < 0.1f && animator.GetBool("IsSneaking"))
        {
            animator.SetBool("IsSneaking", false);
            animator.SetBool("IsCrouching", true);

        }

        // D�finit la vitesse de d�placement dans l'Animator
        float speed = direction.magnitude * currentSpeed;
        animator.SetFloat("MoveSpeed", speed);

        // D�finit les param�tres SpeedX et SpeedY dans l'Animator
        animator.SetFloat("SpeedX", Mathf.Abs(horirzontal));
        animator.SetFloat("SpeedY", Mathf.Abs(vertical));

        //animator.SetBool("IsRunning", speed > walkSpeed);



        //g�re la rotation de la cam�ra
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward * direction.magnitude;

            // Ajoute la vitesse de course si la touche LeftShift est appuy�e ou si le joystick gauche est pouss� � fond
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
