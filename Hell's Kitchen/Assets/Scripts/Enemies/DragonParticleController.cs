using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonParticleController : MonoBehaviour
{
    public GameObject parent;
    private void Start()
    {
        transform.eulerAngles = new Vector3(-90f, 0f, 0f);
    }

    private void Update() {
        transform.position = new Vector3(parent.transform.position.x, 
                                         parent.transform.position.y + 1f,
                                         parent.transform.position.z);
    }
}
