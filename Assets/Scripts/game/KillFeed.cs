using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class KillFeed : MonoBehaviour
{
    public static KillFeed Instance;
    public Transform KillFeedContent;
    [SerializeField] GameObject KillFeedItemPrefab;
    void Awake()
    {
        Instance = this;
    }
    public void AddKillFeedDetail(string[] feed)
    {
        Debug.Log("add kill feed function");
        Instantiate(KillFeedItemPrefab, KillFeedContent).GetComponent<PlayerFeedItem>().ShowKillFeed(feed);
    }

}
