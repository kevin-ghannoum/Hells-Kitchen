using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    public Vector3 direction;

    private void Update() {
        transform.position += direction * speed * Time.deltaTime;
    }
}
