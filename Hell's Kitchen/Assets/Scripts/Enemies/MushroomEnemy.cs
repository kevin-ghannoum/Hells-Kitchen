using Common.Enums;
using UnityEngine;

public class MushroomEnemy : MonoBehaviour
{
    [SerializeField] private float Speed;
    [SerializeField] private float attackRange;
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletPos;
    private float timeCounter;
    private Vector3 direction;

    [SerializeField] private Animator anime;

    private void Start()
    {
        target = GameObject.FindWithTag(Tags.Player);
        anime = gameObject.GetComponent<Animator>();
        attackRange = 20;
    }

    private void Update()
    {
        anime.SetBool("shooting", false);
        timeCounter += Time.deltaTime;
        direction = target.transform.position - transform.position;

        if ((direction).magnitude < attackRange)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 200f);

            if (timeCounter > 2)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position + Vector3.up * 0.5f, (target.transform.position - transform.position).normalized, out hit))
                {
                    if (hit.collider.gameObject.tag == "Player")
                    {
                        Invoke("shoot", 0.5f);
                        timeCounter = 0;
                    }
                }
            }
        }
    }

    private void shoot()
    {
        anime.SetBool("shooting", true);
        GameObject Bullet = GameObject.Instantiate(bullet, bulletPos.position, Quaternion.identity);
        Bullet.GetComponent<BulletControl>().direction = direction.normalized;
    }
}
