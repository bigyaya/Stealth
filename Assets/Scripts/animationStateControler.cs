using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationStateControler : MonoBehaviour
{
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //walk
        if (Input.GetKey("z"))
        {
            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);

        }

        //Jogging
        if (Input.GetKey("left shift"))
        {
            animator.SetBool("IsJogging", true);
        }
        else
        {
            animator.SetBool("IsJogging", false);

        }
    }
}
