using System;
using System.Reflection.Emit;
using System.Collections;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class target : MonoBehaviourPunCallbacks
{
    public static target Instance;
    AudioSource AS;
    public float maxHealth = 100, currentHealth = 0f;
    PhotonView SelfPV;
    public healthbar hp;
    PlayerManager plManager;
    Animator ani;
    ExitGames.Client.Photon.Hashtable tbl;
    public static bool DeathAnimation = false;
    void Awake()
    {
        Instance = this;
        SelfPV = GetComponent<PhotonView>();
        ani = GetComponentInParent<Animator>();
        AS = GetComponentInParent<AudioSource>();
        plManager = PhotonView.Find((int)SelfPV.InstantiationData[0]).GetComponent<PlayerManager>();
    }
    void Update()
    {
        if (transform.position.y <= -10f) StartCoroutine(Die());
    }
    void Start()
    {
        if (!SelfPV.IsMine) return;
        currentHealth = maxHealth;
        hp.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(float amount, string uid)
    {
        SelfPV.RPC("RPC_TakeDamage", RpcTarget.All, amount, uid);
    }
    [PunRPC]
    void RPC_TakeDamage(float amount, string uid)
    {
        if (!SelfPV.IsMine) return;
        if (currentHealth >= 0f)
        {
            currentHealth -= amount;
            hp.SetHealth(currentHealth);
            if (currentHealth <= 0f && !DeathAnimation)
            {
                DeathAnimation = true;
                GameUI.GamePaused = true;
                ani.SetBool("dying", true);
                ani.SetLayerWeight(2, 1f);
                StartCoroutine(Die(uid));
                DeathAnimation = false;
                // Invoke("Die", 5f);
            }
        }
    }
    // Update is called once per frame
    // void Die()
    // {
    //     // Destroy(gameObject);
    //     plManager.Die();
    // }
    IEnumerator Die(string KillerUID = "")
    {
        SelfPV.RPC("RPC_DeathSound", RpcTarget.Others);
        RPC_DeathSound();
        yield return new WaitForEndOfFrame();
        RaiseEventOptions leaderboardEventRaise = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        if (!GameTimer.GameStarted)
        {
            int StartingTimeStamp = PhotonNetwork.ServerTimestamp;
            object dt = StartingTimeStamp;
            PhotonNetwork.RaiseEvent(LobbyManager.START_GAME_CODE_EVENT, dt, leaderboardEventRaise, SendOptions.SendReliable);
            LobbyManager.Instance.StartGame(StartingTimeStamp);
        }
        string PlayerKiller = "";
        string PlayerDied = "";
        LobbyManager.allPlayers.ForEach(pl =>
        {
            if (pl.PlayerID == KillerUID)
            {
                object indexKiller = LobbyManager.allPlayers.IndexOf(pl);
                pl.addKill();
                PlayerKiller = pl.username;
                PhotonNetwork.RaiseEvent(LobbyManager.KILL_CODE_EVENT, indexKiller, leaderboardEventRaise, SendOptions.SendReliable);
            }
            if (plManager.pld.PlayerID == pl.PlayerID)
            {
                object indexDead = LobbyManager.allPlayers.IndexOf(pl);
                plManager.pld.addDeath();
                PlayerDied = plManager.pld.username;
                PhotonNetwork.RaiseEvent(LobbyManager.DEATH_CODE_EVENT, indexDead, leaderboardEventRaise, SendOptions.SendReliable);
            }
        });
        object KillFeedNames = new object[] { PlayerKiller, PlayerDied };
        PhotonNetwork.RaiseEvent(LobbyManager.KILL_FEED_CODE_EVENT, KillFeedNames, leaderboardEventRaise, SendOptions.SendReliable);
        string[] feed = { PlayerKiller, PlayerDied };
        KillFeed.Instance.AddKillFeedDetail(feed);
        LobbyManager.Instance.UpdatePlayerScore();
        yield return new WaitForSeconds(5f);
        plManager.Die();
        GameUI.GamePaused = false;
    }
    [PunRPC]
    void RPC_DeathSound()
    {
        AS.Play();
    }

}
