using UnityEngine;
using Photon.Pun;

public class DestroyNotMine : MonoBehaviour
{
    PhotonView UIPV;
    void Awake()
    {
        UIPV = GetComponent<PhotonView>();
        if (!UIPV.IsMine)
            Destroy(this.gameObject);
    }
}
