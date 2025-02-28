using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class LobbyPanel : MonoBehaviourPunCallbacks, ILobbyCallbacks
{
    public AudioSource audioSource;
    public AudioClip sound;

    [Header("Main Panel")]
    public GameObject MainPanel;
    public InputField NameInput;

    [Header("RoomList Panel")]
    public GameObject RoomListPanel;
    public Button CreateRoomButton;
    public GameObject RoomListContent;
    public GameObject RoomListEntryPrefab;

    [SerializeField]
    private Transform contents;

    [Header("Room Panel")]
    public GameObject RoomPanel;
    public Transform PlayerPanel;
    public Button StartButton;
    public Button ReadyButton;
    public Image ReadyImage;
    public List<Toggle> toggles;
    public RoomInfo RoomInfo { get; private set; }
    public GameObject Player1prefab;
    public GameObject Player2prefab;
    public string SimpleName;
    bool IsReady = false;


    private Dictionary<string, RoomInfo> cachedRoomList;
    private Dictionary<string, GameObject> roomListEntries;
    private Dictionary<int, GameObject> playerListEntries;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        cachedRoomList = new Dictionary<string, RoomInfo>();
        roomListEntries = new Dictionary<string, GameObject>();
        NameInput.text = "Player" + Random.Range(1000, 10000);
        GameManageTest.I.player1name = NameInput.text;
    }


    #region Buttons
    public void OnClick_LoginButton()
    {
        MakeSound();
        string inputPlayername = NameInput.text;
        if (!inputPlayername.Equals(""))
        {
            SimpleName = "Player" + Random.Range(1000, 10000);
            PhotonNetwork.LocalPlayer.NickName = SimpleName; 
            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.ConnectUsingSettings();
            }
            else PhotonNetwork.JoinLobby();
        }
        else return;
    }
    public void OnClick_BacktoMainButton()
    {
        MakeSound();
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.Disconnect();
            this.SetActivePanel(MainPanel.name);
        }
        else return;
    }
    [PunRPC]
    public void OnClick_BacktoLobbyButton()
    {//방장이 나가면 다 나가지는 기능 예정
        MakeSound();
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();

            cachedRoomList.Clear();
            ClearRoomListView();
            this.SetActivePanel(RoomListPanel.name);
        }
        else return;
    }
    public void OnClick_CreateRoomButton()
    {
        MakeSound();
        string roomName;
        roomName = "즐거운 매너겜 필수";
        RoomOptions options = new RoomOptions { MaxPlayers = 2 };
        options.CustomRoomProperties = new Hashtable() {{ "MasterScore", 0 }, { "MasterScore1", 0 }, { "MasterScore2", 0 }, { "MasterScore3", 0 }, { "MasterScore4", 0 } ,
            { "GuestScore", 0 },  { "GuestScore1", 0 }, { "GuestScore2", 0 }, { "GuestScore3", 0 } , { "GuestScore4", 0 }};
        PhotonNetwork.CreateRoom(roomName, options, null);
    }
    public void OnClick_StartGameButton()
    {
        MakeSound();
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false; //방이 꽉찼을때로 이동
        print(PhotonNetwork.LocalPlayer.NickName);
        print(PhotonNetwork.PlayerList[1].NickName);

        PhotonNetwork.LoadLevel(1);
    }
    public void OnClick_ReadyButton()
    {
        MakeSound();
        print("Readybutton Clicked");
        IsReady = !IsReady;
        ReadyImage.gameObject.SetActive(IsReady);
        SetPlayerReady(PhotonNetwork.LocalPlayer, IsReady);
    }

    #endregion

    #region PunCallBacks
    public override void OnDisconnected(DisconnectCause cause)
    {
        print("Disconnected " + cause);
    }
    public override void OnConnectedToMaster()
    {
        print("Connected to Master");
        if (!PhotonNetwork.InLobby)
            PhotonNetwork.JoinLobby();
        this.SetActivePanel(RoomListPanel.name);
    }
    public override void OnJoinedLobby()
    {
        print("Joined lobby");
        
        this.SetActivePanel(RoomListPanel.name);
    }
    public override void OnJoinedRoom()
    {
        print("Joined room");

        cachedRoomList.Clear();
        contents.DestroyChildren();

        this.SetActivePanel(RoomPanel.name);

        if (playerListEntries == null)
        {
            playerListEntries = new Dictionary<int, GameObject>();
        }

        if (PhotonNetwork.IsMasterClient) //토글 활성화 , 레디버튼비활성화 , 왼쪽에 프리팹, 해시테이블 true로 추가
        {
            foreach (Toggle toggle in toggles)
            {
                toggle.interactable = true;
            }

            Hashtable PlayerCP = new Hashtable { { "READY", true } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(PlayerCP);
            ReadyButton.gameObject.SetActive(false);
            GameObject p1 = Instantiate(Player1prefab, RoomPanel.transform.GetChild(0));
            p1.transform.localPosition = new Vector2(-119, 0);
            p1.transform.GetComponentInChildren<Text>().text = SimpleName;
        }
        else // 왼쪽 오른쪽에 프리팹, 
        {
            foreach (Player p in PhotonNetwork.PlayerList)
            {
                if (p.IsMasterClient)
                {
                    GameObject p1 = Instantiate(Player1prefab, RoomPanel.transform.GetChild(0));
                    p1.transform.localPosition = new Vector2(-119, 0);
                    GameManageTest.I.player1name = p.NickName;
                    p1.transform.GetComponentInChildren<Text>().text = p.NickName;
                }
                else
                {
                    GameObject p2 = Instantiate(Player2prefab, RoomPanel.transform.GetChild(0));
                    p2.transform.localPosition = new Vector2(250, 0);
                    GameManageTest.I.player2name = p.NickName;
                    p2.transform.GetComponentInChildren<Text>().text = p.NickName;
                }
            }
            Hashtable PlayerCP = new Hashtable { { "READY", false } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(PlayerCP);
            print("조인룸 시점 " + PlayerCP["READY"]);
        }

    }
    public override void OnLeftLobby()
    {
        print("Leave lobby");

    }
    public override void OnLeftRoom()
    {
        print("Leave room");

        cachedRoomList.Clear();
        ClearRoomListView();

        contents.DestroyChildren();
        PlayerPanel.DestroyChildren();
        ReadyButton.gameObject.SetActive(true);
        IsReady = false;
        SetPlayerReady(PhotonNetwork.LocalPlayer, IsReady);
        foreach (Toggle toggle in toggles)
        {
            toggle.interactable = false;
        }
    }
    public override void OnCreatedRoom()
    {
        print("Created room");
        cachedRoomList.Clear();
        ClearRoomListView();
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        GameObject p2 = Instantiate(Player2prefab, RoomPanel.transform.GetChild(0));
        p2.transform.localPosition = new Vector2(250, 0);
        p2.transform.GetComponentInChildren<Text>().text = newPlayer.NickName;
        playerListEntries.Add(newPlayer.ActorNumber, p2);
        GameManageTest.I.player2name = newPlayer.NickName;

        StartButton.GetComponent<Button>().interactable = CheckPlayersReady();
        print("someone entered");
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient == false )
        {
            print("마스터아니니깐~");
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            print("마스터는 할게 많다");
            ReadyImage.gameObject.SetActive(false);
            StartButton.interactable = CheckPlayersReady();
            Destroy(playerListEntries[otherPlayer.ActorNumber].gameObject);
            playerListEntries.Remove(otherPlayer.ActorNumber);
        }
    }


    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        print("Player properties updated");

        print("셋플레이어 " + targetPlayer + "번 " + (bool)targetPlayer.CustomProperties["READY"]);

        if (PhotonNetwork.IsMasterClient && PhotonNetwork.PlayerList.Length > 1)
        {
            print("스타트버튼 체크" + CheckPlayersReady());
            ReadyImage.gameObject.SetActive(CheckPlayersReady());
            StartButton.GetComponent<Button>().interactable = CheckPlayersReady();
        }
    }
    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        print("Room properties updated");
        Hashtable PC = propertiesThatChanged;
        print("Deadline is " + PC["DEADLINE"]);
        switch (PC["DEADLINE"])
        {
            case 30: { toggles[0].isOn = true; break; }
            case 60: { toggles[1].isOn = true; break; }
            case 90: { toggles[2].isOn = true; break; }
            case 120: { toggles[3].isOn = true; break; }
            default: break;
        }

    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        ClearRoomListView();

        UpdateCachedRoomList(roomList);
        UpdateRoomListView();
    }

    #endregion

    private void SetActivePanel(string activePanel)
    {
        MainPanel.SetActive(activePanel.Equals(MainPanel.name));
        RoomListPanel.SetActive(activePanel.Equals(RoomListPanel.name));    // UI should call OnRoomListButtonClicked() to activate this
        RoomPanel.SetActive(activePanel.Equals(RoomPanel.name));
    }
    private void ClearRoomListView()
    {
        foreach (GameObject entry in roomListEntries.Values)
        {
            Destroy(entry.gameObject);
        }
        roomListEntries.Clear();
    }
    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            // Remove room from cached room list if it got closed, became invisible or was marked as removed
            if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
            {
                if (cachedRoomList.ContainsKey(info.Name))
                {
                    cachedRoomList.Remove(info.Name);
                }

                continue;
            }

            // Update cached room info
            if (cachedRoomList.ContainsKey(info.Name))
            {
                cachedRoomList[info.Name] = info;
            }
            // Add new room info to cache
            else
            {
                cachedRoomList.Add(info.Name, info);
            }
        }
    }

    private void UpdateRoomListView()
    {
        foreach (RoomInfo info in cachedRoomList.Values)
        {
            GameObject entry = Instantiate(RoomListEntryPrefab);
            entry.transform.SetParent(RoomListContent.transform);
            entry.transform.localScale = Vector3.one;
            entry.GetComponent<RoomList>().Initialize(info.Name);

            roomListEntries.Add(info.Name, entry);
        }
    }
    private void SetPlayerReady(Player player, bool ready)
    {

        //CP
        player.SetCustomProperties(new Hashtable() { { "READY", ready } });

    }
    private bool CheckPlayersReady()
    {
        int n = 0;
        if (PhotonNetwork.PlayerList.Length <= 1) return false;
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            Hashtable tempHa = PhotonNetwork.PlayerList[i].CustomProperties;
            //print("체크플레이어" + i + "/ "+ PhotonNetwork.PlayerList.Length +  " "+tempHa["READY"]);
            if (!(bool)tempHa["READY"])
            {
                n = 1;
            }
        }
        if (n == 1)
        {
            n = 0;
            return false;
        }
        return true;
    }

    public void MakeSound()
    {
        audioSource.clip = sound;
        audioSource.Play();
    }
}