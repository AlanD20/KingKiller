using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using System.Linq;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    public static PlayerManager Instance;
    PhotonView PV;
    GameObject Character;
    public playerDetails pld;
    Player _player;
    ExitGames.Client.Photon.Hashtable playersTable = new ExitGames.Client.Photon.Hashtable();
    void Awake()
    {
        PV = GetComponent<PhotonView>();
        _player = PhotonNetwork.LocalPlayer;
        Instance = this;
    }

    void Start()
    {

        if (PV.IsMine)
        {
            pld = new playerDetails();
            LobbyManager.Instance.OnJoinedRoom();
            CreateCharacter();
        }
    }
    void Update()
    {
        if (PV.IsMine && !PhotonNetwork.IsConnected) PauseMenuScript.Instance.LeaveGame();
    }
    void CreateCharacter()
    {
        Transform SpawnPoint = SpawnManager.Instance.GetSpawnPoints();
        Character = PhotonNetwork.Instantiate("player", SpawnPoint.position, SpawnPoint.rotation, 0, new object[] { PV.ViewID });
    }
    public void Die()
    {
        PhotonNetwork.Destroy(Character);
        CreateCharacter();
    }

}
