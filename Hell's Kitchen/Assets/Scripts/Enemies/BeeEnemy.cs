using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeEnemy : MonoBehaviour
{
    private float Speed = 5;
    [SerializeField] private float attackRange;
    private GameObject target;
    private GameObject[] targetList;
    private float timeCounter;
    private float wanderTimeCounter;
    private GameObject Hive;
    private Vector3 wanderPos;
    private float wanderRadius = 7f;
    private bool wanderCheck = true;
    [SerializeField] private Animator anime;

    private void Start()
    {
        targetList = GameObject.FindGameObjectsWithTag("Player");
        Hive = GameObject.Find("Hive");
        anime = gameObject.GetComponent<Animator>();
        anime.SetBool("walking", true);
        attackRange = 7;
    }

    private void Update()
    {
        target = findCloset(targetList);
        timeCounter += Time.deltaTime;
        wanderTimeCounter += Time.deltaTime;
        Vector3 hiveDirection = Hive.transform.position - transform.position;
        hiveDirection.y = 0;
        Vector3 targetDirection = target.transform.position - transform.position;
        targetDirection.y = 0;

        if (targetDirection.magnitude < 8)
        {
            transform.LookAt(target.transform);
            transform.position += targetDirection.normalized * Speed * Time.deltaTime;

        }
        else
        {
            if (hiveDirection.magnitude > 8)
            {
                transform.LookAt(Hive.transform);
                transform.position += hiveDirection.normalized * Speed * Time.deltaTime;
            }
            else
            {
                //wander
                if (wanderCheck)
                {
                    wanderPos = new Vector3(Random.Range(Hive.transform.position.x - wanderRadius, Hive.transform.position.x + wanderRadius),
                                            0,
                                            Random.Range(Hive.transform.position.z - wanderRadius, Hive.transform.position.z + wanderRadius));

                    wanderCheck = false;
                }

                Vector3 wanderDirection = wanderPos - transform.position;
                wanderDirection = new Vector3(wanderDirection.x, 0, wanderDirection.z);
                Debug.Log(Vector3.Distance(wanderPos, transform.position));

                if (Vector3.Distance(wanderPos, new Vector3(transform.position.x, 0, transform.position.z)) < 2f && wanderTimeCounter > Random.Range(2, 4))
                {
                    wanderCheck = true;
                    wanderTimeCounter = 0;
                }
                else if(Vector3.Distance(wanderPos, new Vector3(transform.position.x, 0, transform.position.z)) < 2f)
                {

                }
                else
                {
                    transform.rotation = Quaternion.LookRotation(wanderDirection);
                    transform.position += wanderDirection.normalized * Speed * Time.deltaTime;
                }
            }
        }
    }

    private GameObject findCloset(GameObject[] targetList)
    {
        GameObject currentTarget = targetList[0];
        float distance = (targetList[0].transform.position - transform.position).magnitude;

        for (int i = 1; i < targetList.Length; i++)
        {
            if (distance > (targetList[i].transform.position - transform.position).magnitude)
            {
                currentTarget = targetList[i];
            }
        }

        return currentTarget;
    }
}
