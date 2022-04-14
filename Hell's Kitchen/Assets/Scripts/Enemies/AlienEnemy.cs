using Common.Enums;
using UnityEngine;

public class AlienEnemy : MonoBehaviour
{
    [SerializeField] private float Speed;
    [SerializeField] private float attackRange;
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletPos;
    private float timeCounter;
    private Vector3 direction;
    [SerializeField] private Animator anime;
    private LineRenderer lr;

    private void Start()
    {
        attackRange = 20;
        target = GameObject.FindWithTag(Tags.Player);
        anime = gameObject.GetComponent<Animator>();
        lr = gameObject.GetComponent<LineRenderer>();
    }

    private void Update()
    {
        timeCounter += Time.deltaTime;
        direction = target.transform.position - transform.position;

        if ((direction).magnitude < attackRange)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 100f);
            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up * 0.5f, transform.forward, out hit))
            {
                if (hit.collider.gameObject.tag == "Player")
                {
                    Vector3 startPos = new Vector3(transform.position.x, transform.position.y * 2, transform.position.z);

                    lr.SetPosition(0, transform.position + Vector3.up * 0.5f);
                    lr.SetPosition(1, target.transform.position + Vector3.up * 1.8f);
                }
            }
        }
    }
}
