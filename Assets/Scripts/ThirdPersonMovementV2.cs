using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovementV2 : MonoBehaviour
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
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        //g�re la rotation de la cam�ra
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            //ajoute la vitesse de course si la touche de course est appuy�e
            if (Input.GetKey(KeyCode.LeftShift))
            {
                currentSpeed = runSpeed;
            }
            else
            {
                currentSpeed = walkSpeed;
            }

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward * direction.magnitude;

            //g�re le mouvement lat�ral
            Vector3 moveDire = Vector3.zero;
            if (horizontal != 0)
            {
                float lateralAngle = cam.eulerAngles.y + (horizontal > 0 ? 90f : -90f);
                moveDir += Quaternion.Euler(0f, lateralAngle, 0f) * Vector3.forward * Mathf.Abs(horizontal);
            }
            if (vertical != 0)
            {
                moveDir += Quaternion.Euler(0f, cam.eulerAngles.y, 0f) * Vector3.forward * (vertical > 0 ? 1 : -1);
            }

            controller.Move(moveDir.normalized * currentSpeed * Time.deltaTime);

        }
        else
        {
            currentSpeed = 0f;
        }

        //fait toujours face � la direction de la cam�ra
        transform.rotation = Quaternion.Euler(0f, cam.eulerAngles.y, 0f);
    }

}
