using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationStateControler : MonoBehaviour
{
    Animator animator;
    //ThirdPersonMovementV2 controller;

    //float SpeedX = 0.0f;
    //float SpeedY = 0.0f;
    //public float acceleration = 2.0f;
    //public float deceleration = 2.0f;
    //public bool isJumping = false;






    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {



        ////récupère les inputs
        //bool forwardPressed = Input.GetKey("z");
        //bool leftPressed = Input.GetKey("q");
        //bool rightPressed = Input.GetKey("d");
        //bool backwardPressed = Input.GetKey("s");
        //bool runPressed = Input.GetKey("left shift");

        ////si le joueur appuie sur "z", augmente la vélocité sur la direction z.
        //if (forwardPressed)
        //{
        //    SpeedY += Time.deltaTime * acceleration;
        //    animator.SetFloat("SpeedY", SpeedY);

        //}
        //if (leftPressed)
        //{
        //    SpeedX -= Time.deltaTime * acceleration;
        //}
        //if (rightPressed)
        //{
        //    SpeedX += Time.deltaTime * acceleration;
        //}
        //if (backwardPressed)
        //{
        //    SpeedY -= Time.deltaTime * acceleration;
        //}

        //animator.SetFloat("SpeedY", SpeedY);
        //animator.SetFloat("SpeedX", SpeedX);

    }

    
    
        
    
}
