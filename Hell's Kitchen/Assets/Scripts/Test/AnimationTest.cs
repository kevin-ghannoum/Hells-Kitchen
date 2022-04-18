using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTest : MonoBehaviour
{
    private float speed = 3f;
    private Animator animator;
    private bool flag = true;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("isRunning", false);
        animator.SetBool("isWalking", false);
        if(flag){
            animator.SetTrigger("CastSpell");
            flag = false;
        }

        // speed = 3f;
        // GetComponent<Rigidbody>().velocity = Vector3.zero;
        // animator.SetBool("isRunning", false);
        // animator.SetBool("isWalking", false);
        // animator.ResetTrigger("Jump");
        // animator.ResetTrigger("Punch");
        // animator.ResetTrigger("PickUp");

        // if(Input.GetKey(KeyCode.W)){
        //     animator.SetBool("isWalking", true);
        
        //     if(Input.GetKey(KeyCode.LeftShift)){
        //         speed = 2*speed;
        //         animator.SetBool("isRunning", true);
        //     }
        
        //     transform.position += new Vector3(0, 0, speed * Time.deltaTime);
        //     transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
        // }
        // else if(Input.GetKey(KeyCode.S)){
        //     animator.SetBool("isWalking", true);
        
        //     if(Input.GetKey(KeyCode.LeftShift)){
        //         speed = 2*speed;
        //         animator.SetBool("isRunning", true);
        //     }
        
        //     transform.position += new Vector3(0, 0, -speed * Time.deltaTime);
        //     transform.rotation = Quaternion.LookRotation(Vector3.back, Vector3.up);
        // }
        // else if(Input.GetKey(KeyCode.A)){
        //     animator.SetBool("isWalking", true);
        
        //     if(Input.GetKey(KeyCode.LeftShift)){
        //         speed = 2*speed;
        //         animator.SetBool("isRunning", true);
        //     }
        
        //     transform.position += new Vector3(-speed * Time.deltaTime, 0, 0);
        //     transform.rotation = Quaternion.LookRotation(Vector3.left, Vector3.up);
        // }
        // else if(Input.GetKey(KeyCode.D)){
        //     animator.SetBool("isWalking", true);
        
        //     if(Input.GetKey(KeyCode.LeftShift)){
        //         speed = 2*speed;
        //         animator.SetBool("isRunning", true);
        //     }
        
        //     transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
        //     transform.rotation = Quaternion.LookRotation(Vector3.right, Vector3.up);
        // }
        
        // if(Input.GetKeyDown(KeyCode.Space)){
        //     animator.SetTrigger("Jump");
        // }
        
        // if(Input.GetMouseButtonDown(0)){
        //     animator.SetTrigger("Attack");
        // }
        
        // if(Input.GetMouseButtonDown(1)){
        //     animator.SetTrigger("PickUp");
        // }

        // if(Input.GetMouseButtonDown(2)){
        //     animator.SetTrigger("CastSpell");
        // }

    }
}
