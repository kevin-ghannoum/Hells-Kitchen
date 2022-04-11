using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 3f;
    void Update()
    {
        
        if(Input.GetKey(KeyCode.W)){

            transform.position += new Vector3(0, 0, speed * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
        }
        else if(Input.GetKey(KeyCode.S)){

            transform.position += new Vector3(0, 0, -speed * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(Vector3.back, Vector3.up);
        }
        else if(Input.GetKey(KeyCode.A)){
            transform.position += new Vector3(-speed * Time.deltaTime, 0, 0);
            transform.rotation = Quaternion.LookRotation(Vector3.left, Vector3.up);
        }
        else if(Input.GetKey(KeyCode.D)){
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
            transform.rotation = Quaternion.LookRotation(Vector3.right, Vector3.up);
        }
    }
}
