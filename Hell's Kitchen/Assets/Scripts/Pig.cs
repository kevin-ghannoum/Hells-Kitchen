using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : MonoBehaviour
{
    [SerializeField]
    private float chargeSpeed = 10;
    [SerializeField]
    private float chargeTime = 3;
    private float currentChargeTime = 0;

    private Vector3 chargeDirection;

    private GameObject[] players;
    private Animator anim;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentChargeTime >= chargeTime)
        {
            anim.SetBool("Charge", false);
            rb.velocity = Vector3.zero;
            foreach (GameObject player in players)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, (player.transform.position - transform.position).normalized, out hit))
                {
                    if (hit.collider.gameObject.tag == "Player")
                    {
                        Debug.Log("Found");
                        chargeDirection = (player.transform.position - transform.position).normalized;
                        currentChargeTime = -1;
                        anim.SetBool("Charge", true);
                        transform.rotation = Quaternion.LookRotation(chargeDirection);
                    }
                }

            }
        }
        else
        {
            currentChargeTime += Time.deltaTime;
            Debug.Log(currentChargeTime);
            if (currentChargeTime > 0)
            {
                rb.velocity = chargeDirection * chargeSpeed;
            }
        }
    }
}
