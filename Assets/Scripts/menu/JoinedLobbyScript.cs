using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class JoinedLobbyScript : MonoBehaviourPunCallbacks
{
    [SerializeField] Button btnStartGame;
    [SerializeField] TMP_Text MaxPlayerText;
    [SerializeField] TMP_Text CurrentPlayerText;
    [SerializeField] TMP_Text CreatedLobbyName;
    [SerializeField] TMP_Text gametimeText;
    [SerializeField] Slider gamelengthSlider;
    byte playerCount = 0;
    void Start()
    {
        CreatedLobbyName.text = PhotonNetwork.CurrentRoom.Name;
        MaxPlayerText.text = "6";
        btnStartGame.gameObject.SetActive(PhotonNetwork.IsMasterClient);
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        btnStartGame.gameObject.SetActive(PhotonNetwork.IsMasterClient);
    }

    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
            gamelengthSlider.gameObject.SetActive(true);
        playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        CurrentPlayerText.text = playerCount.ToString();
        if (playerCount < 0)
            btnStartGame.interactable = false;
        else
            btnStartGame.interactable = true;
    }
    public void UpdateGameTime(float length)
    {
        gametimeText.text = length.ToString();
        this.photonView.RPC("RPC_GameLength", RpcTarget.All, length);
    }
    [PunRPC]
    void RPC_GameLength(float length)
    {
        gametimeText.text = length.ToString();
    }

}
