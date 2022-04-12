using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomEnemy : MonoBehaviour
{
    [SerializeField] private float Speed;
    [SerializeField] private float attackRange;
    [SerializeField] private GameObject target;
    private GameObject[] targetList;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletPos;
    private float timeCounter;

    [SerializeField] private Animator anime;

    private void Start()
    {
        targetList = GameObject.FindGameObjectsWithTag("Player");
        anime = gameObject.GetComponent<Animator>();
        attackRange = 10;
    }

    private void Update()
    {
        anime.SetBool("shooting", false);
        target = findCloset(targetList);
        timeCounter += Time.deltaTime;
        Vector3 direction = target.transform.position - transform.position;

        if ((direction).magnitude < attackRange)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 200f);

            if (timeCounter > 2)
            {
                anime.SetBool("shooting", true);
                GameObject Bullet = GameObject.Instantiate(bullet, bulletPos.position, Quaternion.identity);
                Bullet.GetComponent<BulletControl>().direction = direction;
                timeCounter = 0;
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
