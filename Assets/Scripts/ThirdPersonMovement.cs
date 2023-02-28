using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{

    public CharacterController controller;
    public Transform cam;

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
    public float crouchSpeed = 3f;

    //cam�ra rotation
    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    //gestion de l'accroupissement
    private bool isCrouching = false;
    public float crouchHeight = 1f;
    public float standHeight = 2f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        currentSpeed = walkSpeed; //initialisation de la vitesse de d�placement
    }


    void Update()
    {

        //g�re le gravit�
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        //v�rifie si le joueur touche le sol
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        //g�re le saut
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }


        //recup�re les inputs
        float horirzontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horirzontal, 0f, vertical).normalized;


        //g�re la rotation de la cam�ra
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            // gestion de la vitesse de d�placement
    if (Input.GetKey(KeyCode.LeftShift))
            {
                currentSpeed = runSpeed;
            }
            else if (Input.GetKey(KeyCode.LeftControl))
            {
                currentSpeed = crouchSpeed;

                //gestion de l'accroupissement
                if (!isCrouching)
                {
                    controller.height = crouchHeight;
                    controller.center = new Vector3(0f, crouchHeight / 2f, 0f);
                    isCrouching = true;
                }
            }
            else
            {
                currentSpeed = walkSpeed;

                //gestion de l'accroupissement
                if (isCrouching)
                {
                    controller.height = standHeight;
                    controller.center = new Vector3(0f, standHeight / 2f, 0f);
                    isCrouching = false;
                }
            }

            controller.Move(moveDir.normalized * currentSpeed * Time.deltaTime);
        }
    }

}
