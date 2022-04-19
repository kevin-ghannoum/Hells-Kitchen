using Common.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    float speed = 15f;
    float destroyTime = 2.5f;
    float time = 0f;
    [SerializeField] private float damage = 25f;
    public Vector3 rotation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        time += Time.deltaTime;
        if(time >= destroyTime){
            Destroy(this.gameObject);
        }
        transform.position += rotation * Time.deltaTime * speed;
        transform.rotation = Quaternion.LookRotation(rotation);
    }

    List<GameObject> hitList = new List<GameObject> ();
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("collided with" + other.name);
        if (!hitList.Contains(other.gameObject) && other.gameObject.TryGetComponent(out IKillable killable) && other.tag != "Player" && other.tag != "SousChef") {
            hitList.Add(other.gameObject);
            killable.TakeDamage(damage);
        }
    }
}
