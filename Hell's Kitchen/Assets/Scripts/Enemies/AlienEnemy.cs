using Common.Enums;
using UnityEngine;
using Common.Interfaces;

public class AlienEnemy : MonoBehaviour
{
    [SerializeField] private float attackRange;
    [SerializeField] private GameObject target;
    private float timeCounter;
    private float damageTimeCounter;
    private float damageRate = 0.1f;
    private Vector3 direction;
    [SerializeField] private Animator anime;
    private LineRenderer lr;
    [SerializeField] private float attackDamage = 1;

    private void Start()
    {
        attackRange = 12;
        target = GameObject.FindWithTag(Tags.Player);
        anime = gameObject.GetComponent<Animator>();
        lr = gameObject.GetComponent<LineRenderer>();
    }

    private void Update()
    {
        timeCounter += Time.deltaTime;
        damageTimeCounter += Time.deltaTime;
        direction = target.transform.position - transform.position;

        if (direction.magnitude < attackRange)
        {
            transform.LookAt(target.transform.position);
            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up * 0.5f, transform.forward, out hit))
            {
                if (hit.collider.gameObject.tag == "Player")
                {
                    lr.positionCount = 2;
                    Vector3 startPos = new Vector3(transform.position.x, transform.position.y * 2, transform.position.z);

                    lr.SetPosition(0, transform.position + Vector3.up * 0.5f);
                    lr.SetPosition(1, target.transform.position + Vector3.up * 1.8f);

                    if(damageTimeCounter > damageRate)
                    {
                        target.gameObject.GetComponent<IKillable>().TakeDamage(attackDamage);
                    }
                }
            }
        }
        else
        {
            lr.positionCount = 0;
        }
    }
}
