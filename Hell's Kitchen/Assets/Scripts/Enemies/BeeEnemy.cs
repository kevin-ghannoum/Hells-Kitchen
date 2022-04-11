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
    private GameObject Hive;
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
        Vector3 hiveDirection = Hive.transform.position - transform.position;
        hiveDirection = new Vector3(hiveDirection.x, 0, hiveDirection.z);
        Vector3 targetDirection = target.transform.position - transform.position;
        targetDirection = new Vector3(targetDirection.x, 0, targetDirection.z);

        if (targetDirection.magnitude < 8)
        {
            transform.LookAt(target.transform);
            transform.position += targetDirection.normalized * Speed * Time.deltaTime;

        }
        else
        {
            if (hiveDirection.magnitude > 5)
            {
                transform.LookAt(Hive.transform);
                transform.position += hiveDirection.normalized * Speed * Time.deltaTime;
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
