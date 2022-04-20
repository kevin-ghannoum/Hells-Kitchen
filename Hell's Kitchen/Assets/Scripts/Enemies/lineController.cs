using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class lineController : MonoBehaviour
{
    private LineRenderer lr;
    [SerializeField] Texture[] texture;
    [SerializeField] private PhotonView photonView;

    private float timeCounter;
    [SerializeField] private float changeRate = 0.1f;
    private int listCounter = 0;

    private void Awake()
    {
        lr = gameObject.GetComponent<LineRenderer>();
    }

    private void Update()
    {
        timeCounter += Time.deltaTime;

        if (listCounter == texture.Length)
            listCounter = 0;

        if (timeCounter > changeRate)
        {
            lr.material.SetTexture("_MainTex", texture[listCounter]);

            listCounter += 1;
            timeCounter = 0;
        }
    }
}
