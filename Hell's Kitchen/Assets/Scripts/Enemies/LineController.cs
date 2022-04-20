using Enemies;
using UnityEngine;
using Photon.Pun;

public class LineController : MonoBehaviour, IPunObservable
{
    [SerializeField] Texture[] texture;
    [SerializeField] private float changeRate = 0.1f;
    [SerializeField] public int targetPhotonViewID;
    
    private LineRenderer lr;
    private AlienEnemy _alienEnemy;
    private float timeCounter;
    private int listCounter = 0;

    private void Awake()
    {
        lr = gameObject.GetComponent<LineRenderer>();
        _alienEnemy = GetComponentInParent<AlienEnemy>();
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

        if (targetPhotonViewID != 0)
        {
            var pv = PhotonView.Find(targetPhotonViewID);
            if (pv != null)
            {
                lr.positionCount = 2;
                lr.SetPosition(0, _alienEnemy.transform.position + Vector3.up * 0.5f);
                lr.SetPosition(1, pv.transform.position + Vector3.up * 1.8f);
            }
        }
        else
        {
            lr.positionCount = 0;
        }
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(targetPhotonViewID);
        }
        else if (stream.IsReading)
        {
            targetPhotonViewID = (int)stream.ReceiveNext();
        }
    }
}
