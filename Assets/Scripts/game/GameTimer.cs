using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class GameTimer : MonoBehaviourPun
{
    public static GameTimer Instance;
    public static bool GameStarted = false;
    [SerializeField] TMP_Text UI_TimerText;
    [SerializeField] GameObject WinnerScreen;
    [SerializeField] Transform leaderboardContent;
    [SerializeField] GameObject LeaderboardItemPrefab;
    [SerializeField] Button leavegame;
    public static int StartTime, CurrentGameTime, EndTime;
    Coroutine TimerCoroutine;
    PhotonView pv;
    void Awake()
    {
        Instance = this;
        pv = GetComponentInParent<PhotonView>();
    }
    void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += NCER_RefreshTimer;
    }
    void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NCER_RefreshTimer;
    }
    void Update()
    {

    }
    void RefreshTimerUI()
    {
        string minutes = (CurrentGameTime / 60).ToString("00");
        string seconds = (CurrentGameTime % 60).ToString("00");
        UI_TimerText.text = $"{minutes}:{seconds}";
    }
    public void SetTimer(int minutes)
    {
        EndTime = minutes * 60;
        CurrentGameTime = EndTime;
        RefreshTimerUI();
        if (PhotonNetwork.IsMasterClient) TimerCoroutine = StartCoroutine(Timer());
    }
    IEnumerator Timer()
    {
        RefreshTimer_SendEvent();
        yield return new WaitForSeconds(1f);
        CurrentGameTime -= 1;
        if (CurrentGameTime <= 0)
        {
            TimerCoroutine = null;
            pv.RPC("RPC_EndGame", RpcTarget.AllViaServer);
        }
        else
        {
            RefreshTimerUI();
            TimerCoroutine = StartCoroutine(Timer());
        }
    }
    [PunRPC]
    void RPC_EndGame()
    {
        if (TimerCoroutine != null) StopCoroutine(TimerCoroutine);
        CurrentGameTime = 0;
        RefreshTimerUI();
        GameUI.GamePaused = true;
        Cursor.lockState = CursorLockMode.None;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in players) Destroy(p.gameObject);
        WinnerScreenCalculation();
        WinnerScreen.SetActive(true);
        if (PhotonNetwork.IsMasterClient)
        {
            leavegame.gameObject.SetActive(true);
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.DestroyAll();
        }
    }
    public void RefreshTimer_SendEvent()
    {
        object dt = CurrentGameTime;
        PhotonNetwork.RaiseEvent(LobbyManager.REFRESH_TIMER_CODE_EVENT, dt, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
    }
    public void RefreshTimer_ReceiveEvent(int TimeInSecond)
    {
        CurrentGameTime = TimeInSecond;
        RefreshTimerUI();
    }

    void NCER_RefreshTimer(EventData obj)
    {
        if (obj.Code == LobbyManager.REFRESH_TIMER_CODE_EVENT)
        {
            int TimeFromMaster = (int)obj.CustomData;
            RefreshTimer_ReceiveEvent(TimeFromMaster);
        }
    }
    void WinnerScreenCalculation()
    {
        List<playerDetails> ppa = LobbyManager.allPlayers;
        ppa[0].username += " -> King Killer <-";
        foreach (playerDetails pl in ppa)
        {
            Instantiate(LeaderboardItemPrefab, leaderboardContent).GetComponent<playerDetailsItem>().updateLeaderboard(pl);
        }
    }
    public void CallLeaveGame()
    {
        this.photonView.RPC("RPC_LeaveTheGame", RpcTarget.AllViaServer);
    }
    [PunRPC]
    void RPC_LeaveTheGame()
    {
        LobbyManager.allPlayers.Clear();
        GameUI.GamePaused = false;
        GameStarted = false;
        PhotonNetwork.LeaveRoom(true);
        PhotonNetwork.Disconnect();
    }
}
