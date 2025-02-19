using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DeckShuffle;
using System.Linq;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using JsonFx.Json;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameManager : MonoBehaviourPunCallbacks
{
    #region awake setting
    public static GameManager I = null;

    private void Awake()
    {
        if (I == null)
        {
            I = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        Screen.SetResolution(Screen.width, Screen.width * 9 / 16, true);
    }
    #endregion

    public PhotonView PV;
    public GameObject WaitPanel;
    public GameObject WinPanel;
    public GameObject LosePanel;
    public Renderer Crown1;
    public Renderer Crown2;
    public Renderer Crown3;
    public Renderer Crown4;
    public Material Mat_crown;
    public Material Mat_defaultCrown;
    public int isWinning01;
    public int isWinning02;
    public int isWinning03;
    public int isWinning04;

    public int total_Score00 = 0;
    public int total_Score01 = 0;
    public int total_Score02 = 0;
    public int total_Score03 = 0;
    public int total_Score04 = 0;
    public int score_total = 0;
    public Text score_0;
    public Text score_1;
    public Text score_2;
    public Text score_3;
    public Text score_4;

    public int other_total00 = 0;
    public int other_total01 = 0;
    public int other_total02 = 0;
    public int other_total03 = 0;
    public int other_total04 = 0;
    public int Otherscore_total = 0;
    public Text Otherscore_0;
    public Text Otherscore_1;
    public Text Otherscore_2;
    public Text Otherscore_3;
    public Text Otherscore_4;


    public bool isHolding;
    public Text txt_debug;

    public GameObject checkMap;
    public GameObject Field_map;
    public GameObject Btn_Setfield;
    public GameObject Btn_Resetfield;
    public Transform CharSetObject;

    public GameObject GridSet01;
    public GameObject GridSet02;
    public GameObject GridSet03;
    public GameObject GridSet04;

    public GameObject Btn_Deck;
    public GameObject Btn_SetChar;

    public GameObject DeckManagerprefab;
    public GameObject LeftPanel;
    public GameObject RightPanel;
    public GameObject DeckPanel;
    public Transform HandPanel;
    public Transform BtnLeftLabel;
    public Transform BtnRightLabel;
    public GameObject BtnCheckLabel;
    public GameObject BtnMapReset;
    public GameObject BtnDeck;
    public GameObject DontTouchPanel;

    public GameObject[] setChar;

    public GameObject TimerPrefab;
    public Transform TimerPanel;
    public string player1name;
    public string player2name;
    public int timelimit;
    public float lefttime;
    public Button Btn_Pick;

    public Text txt_warning;

    public List<GameObject> Cardlist = new List<GameObject>();
    public Dictionary<string, string> LeftCards = new Dictionary<string, string>();
    public Dictionary<string, string> RightCards = new Dictionary<string, string>();

    public Dictionary<string, string> LLC = new Dictionary<string, string>();
    public Dictionary<string, string> RLC = new Dictionary<string, string>();
    public GameObject[] ch;
    bool isPlay = false; //카드배치페이즈에 5장만 쓸수있도록
    public bool whosturn = true; //true : 마스터, false : 손님
    

    public void DeckPanelMovement()
    {
        Btn_Pick.interactable = true;
        Animator animator = DeckPanel.GetComponent<Animator>();

        bool isOpen = animator.GetBool("Isopen");
        animator.SetBool("Isopen", !isOpen);
        
    }
    #region grid
    public int gridSize;
    public float gapX;
    public float gapZ;
    public void MoveChildrenIntoGrid01()
    {
        for (int i = 0; i < GridSet01.transform.childCount; i++)
        {
            float x = (i - (Mathf.Abs(i / gridSize) * gridSize)) * gapX;
            float z = Mathf.Abs(i / gridSize) * gapZ;
            GridSet01.transform.GetChild(i).localPosition = new Vector3(x, 3, z);

        }
    }
    public void MoveChildrenIntoGrid02()
    {
        for (int i = 0; i < GridSet02.transform.childCount; i++)
        {
            float x = (i - (Mathf.Abs(i / gridSize) * gridSize)) * gapX;
            float z = Mathf.Abs(i / gridSize) * gapZ;
            GridSet02.transform.GetChild(i).localPosition = new Vector3(x, 3, z);

        }
    }
    public void MoveChildrenIntoGrid03()
    {
        for (int i = 0; i < GridSet03.transform.childCount; i++)
        {
            float x = (i - (Mathf.Abs(i / gridSize) * gridSize)) * gapX;
            float z = Mathf.Abs(i / gridSize) * gapZ;
            GridSet03.transform.GetChild(i).localPosition = new Vector3(x, 3, z);

        }
    }
    public void MoveChildrenIntoGrid04()
    {
        for (int i = 0; i < GridSet04.transform.childCount; i++)
        {
            float x = (i - (Mathf.Abs(i / gridSize) * gridSize)) * gapX;
            float z = Mathf.Abs(i / gridSize) * gapZ;
            GridSet04.transform.GetChild(i).localPosition = new Vector3(x, 3, z);

        }
    }
    public void SetCharGrid()
    {
        for (int i = 0; i < CharSetObject.transform.childCount; i++)
        {
            float x = (i - (Mathf.Abs(i / gridSize) * gridSize)) * gapX;
            float z = Mathf.Abs(i / gridSize) + gapZ;
            CharSetObject.transform.GetChild(i).localPosition = new Vector3(x, 3, z);
        }
    }
    #endregion
    public void SetField()
    {
        Btn_SetChar.SetActive(false);
        for (int i = 0; i < GridSet01.transform.childCount; i++)
        {
            GridSet01.transform.GetChild(i).Rotate(GridSet01.transform.GetChild(i).rotation.x, 180, GridSet01.transform.GetChild(i).rotation.z);
        }
        for (int i = 0; i < GridSet02.transform.childCount; i++)
        {
            GridSet02.transform.GetChild(i).Rotate(GridSet02.transform.GetChild(i).rotation.x, 180, GridSet01.transform.GetChild(i).rotation.z);
        }
        for (int i = 0; i < GridSet03.transform.childCount; i++)
        {
            GridSet03.transform.GetChild(i).Rotate(GridSet03.transform.GetChild(i).rotation.x, 180, GridSet01.transform.GetChild(i).rotation.z);
        }
        for (int i = 0; i < GridSet04.transform.childCount; i++)
        {
            GridSet04.transform.GetChild(i).Rotate(GridSet04.transform.GetChild(i).rotation.x, 180, GridSet01.transform.GetChild(i).rotation.z);
        }
    }
    public void SetFieldScore()
    {
        total_Score01 = 0;
        total_Score02 = 0;
        total_Score03 = 0;
        total_Score04 = 0;

        foreach (Transform child in GridSet01.transform)
        {
            if (child.name == "Garlic(Clone)")
            {
                total_Score01 += 1;
            }
            else if (child.name == "avocado(Clone)")
            {
                total_Score01 += 2;
            }
            else if (child.name == "Tomato(Clone)")
            {
                total_Score01 += 3;
            }
            else if (child.name == "HotDog(Clone)")
            {
                total_Score01 += 4;
            }
            else if (child.name == "Rapping(Clone)")
            {
                total_Score01 += 5;
            }
        }
        foreach (Transform child in GridSet02.transform)
        {
            if (child.name == "Hedgehog(Clone)")
            {
                total_Score02 += 1;
            }
            else if (child.name == "Penguin(Clone)")
            {
                total_Score02 += 2;
            }
            else if (child.name == "Giraffe(Clone)")
            {
                total_Score02 += 3;
            }
            else if (child.name == "Crocodile(Clone)")
            {
                total_Score02 += 4;
            }
            else if (child.name == "Bear(Clone)")
            {
                total_Score02 += 5;
            }
            
        }
        foreach (Transform child in GridSet03.transform)
        {
            if (child.name == "Mini(Clone)")
            {
                total_Score03 += 1;
            }
            else if (child.name == "OE(Clone)")
            {
                total_Score03 += 2;
            }
            else if (child.name == "Tobby(Clone)")
            {
                total_Score03 += 3;
            }
            else if (child.name == "UFO(Clone)")
            {
                total_Score03 += 4;
            }
            else if (child.name == "Chubby(Clone)")
            {
                total_Score03 += 5;
            }
        }
        foreach (Transform child in GridSet04.transform)
        {
            if (child.name == "Hedgehog(Clone)")
            {
                total_Score04 += 1;
            }
            else if (child.name == "Penguin(Clone)")
            {
                total_Score04 += 2;
            }
            else if (child.name == "Giraffe(Clone)")
            {
                total_Score04 += 3;
            }
            else if (child.name == "Crocodile(Clone)")
            {
                total_Score04 += 4;
            }
            else if (child.name == "Bear(Clone)")
            {
                total_Score04 += 5;
            }
            else if (child.name == "Garlic(Clone)")
            {
                total_Score04 += 1;
            }
            else if (child.name == "avocado(Clone)")
            {
                total_Score04 += 2;
            }
            else if (child.name == "Tomato(Clone)")
            {
                total_Score04 += 3;
            }
            else if (child.name == "HotDog(Clone)")
            {
                total_Score04 += 4;
            }
            else if (child.name == "Rapping(Clone)")
            {
                total_Score04 += 5;
            }
            else if (child.name == "Mini(Clone)")
            {
                total_Score04 += 1;
            }
            else if (child.name == "OE(Clone)")
            {
                total_Score04 += 2;
            }
            else if (child.name == "Tobby(Clone)")
            {
                total_Score04 += 3;
            }
            else if (child.name == "UFO(Clone)")
            {
                total_Score04 += 4;
            }
            else if (child.name == "Chubby(Clone)")
            {
                total_Score04 += 5;
            }
        }
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            Hashtable cp = PhotonNetwork.CurrentRoom.CustomProperties;
            int mValue = (int)(cp["MasterScore1"]);

            Hashtable setValue = new Hashtable();
            setValue.Add("MasterScore1", total_Score01);

            Hashtable expectedValue = new Hashtable();
            expectedValue.Add("MasterScore1", mValue);

            PhotonNetwork.CurrentRoom.SetCustomProperties(setValue, expectedValue);

            int mValue2 = (int)(cp["MasterScore2"]);

            Hashtable setValue2 = new Hashtable();
            setValue2.Add("MasterScore2", total_Score02);

            Hashtable expectedValue2 = new Hashtable();
            expectedValue2.Add("MasterScore2", mValue2);

            PhotonNetwork.CurrentRoom.SetCustomProperties(setValue2, expectedValue2);

            int mValue3 = (int)(cp["MasterScore3"]);

            Hashtable setValue3 = new Hashtable();
            setValue3.Add("MasterScore3", total_Score03);

            Hashtable expectedValue3 = new Hashtable();
            expectedValue3.Add("MasterScore3", mValue3);

            PhotonNetwork.CurrentRoom.SetCustomProperties(setValue3, expectedValue3);

            int mValue4 = (int)(cp["MasterScore4"]);

            Hashtable setValue4 = new Hashtable();
            setValue4.Add("MasterScore4", total_Score04);

            Hashtable expectedValue4 = new Hashtable();
            expectedValue4.Add("MasterScore4", mValue4);

            PhotonNetwork.CurrentRoom.SetCustomProperties(setValue4, expectedValue4);

            int mValue0 = (int)(cp["MasterScore"]);

            Hashtable setValue0 = new Hashtable();
            setValue0.Add("MasterScore", total_Score00);

            Hashtable expectedValue0 = new Hashtable();
            expectedValue0.Add("MasterScore", mValue0);

            PhotonNetwork.CurrentRoom.SetCustomProperties(setValue0, expectedValue0);
        }
        else
        {
            Hashtable cp = PhotonNetwork.CurrentRoom.CustomProperties;
            int oValue = (int)(cp["GuestScore1"]);

            Hashtable setValue = new Hashtable();
            setValue.Add("GuestScore1", total_Score01);

            Hashtable expectedValue = new Hashtable();
            expectedValue.Add("GuestScore1", oValue);

            PhotonNetwork.CurrentRoom.SetCustomProperties(setValue, expectedValue);

            int oValue2 = (int)(cp["GuestScore2"]);
            Hashtable setValue2 = new Hashtable();
            setValue2.Add("GuestScore2", total_Score02);

            Hashtable expectedValue2 = new Hashtable();
            expectedValue2.Add("GuestScore2", oValue2);

            PhotonNetwork.CurrentRoom.SetCustomProperties(setValue2, expectedValue2);

            int oValue3 = (int)(cp["GuestScore3"]);
            Hashtable setValue3 = new Hashtable();
            setValue3.Add("GuestScore3", total_Score03);

            Hashtable expectedValue3 = new Hashtable();
            expectedValue3.Add("GuestScore3", oValue3);

            PhotonNetwork.CurrentRoom.SetCustomProperties(setValue3, expectedValue3);

            int oValue4 = (int)(cp["GuestScore4"]);
            Hashtable setValue4 = new Hashtable();
            setValue4.Add("GuestScore4", total_Score04);

            Hashtable expectedValue4 = new Hashtable();
            expectedValue4.Add("GuestScore4", oValue4);

            PhotonNetwork.CurrentRoom.SetCustomProperties(setValue4, expectedValue4);

            int oValue0 = (int)(cp["GuestScore"]);

            Hashtable setValue0 = new Hashtable();
            setValue0.Add("GuestScore", other_total00);

            Hashtable expectedValue0 = new Hashtable();
            expectedValue0.Add("GuestScore", oValue0);

            PhotonNetwork.CurrentRoom.SetCustomProperties(setValue0, expectedValue0);
        }
    } 
    public void OnClick_Setfield()
    {

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            BtnDeck.SetActive(true);
        }

        Btn_Setfield.SetActive(false);
        BtnMapReset.SetActive(false);
    }

    #region Random System from DeckManager
    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        /*foreach (var item in propertiesThatChanged)
        {
            txt_debug.text += item.Key;
        }*/
        score_0.text = PhotonNetwork.CurrentRoom.CustomProperties["MasterScore"].ToString();
        Otherscore_0.text = PhotonNetwork.CurrentRoom.CustomProperties["GuestScore"].ToString();
        score_1.text = PhotonNetwork.CurrentRoom.CustomProperties["MasterScore1"].ToString();
        Otherscore_1.text = PhotonNetwork.CurrentRoom.CustomProperties["GuestScore1"].ToString();
        score_2.text = PhotonNetwork.CurrentRoom.CustomProperties["MasterScore2"].ToString();
        Otherscore_2.text = PhotonNetwork.CurrentRoom.CustomProperties["GuestScore2"].ToString();
        score_3.text = PhotonNetwork.CurrentRoom.CustomProperties["MasterScore3"].ToString();
        Otherscore_3.text = PhotonNetwork.CurrentRoom.CustomProperties["GuestScore3"].ToString();
        score_4.text = PhotonNetwork.CurrentRoom.CustomProperties["MasterScore4"].ToString();
        Otherscore_4.text = PhotonNetwork.CurrentRoom.CustomProperties["GuestScore4"].ToString();
        

        #region map01
        if ((int)(PhotonNetwork.CurrentRoom.CustomProperties["MasterScore1"]) > (int)(PhotonNetwork.CurrentRoom.CustomProperties["GuestScore1"]))
        {
            if (PhotonNetwork.LocalPlayer.IsMasterClient == true)
            {
                Crown1.material = Mat_crown;
                isWinning01 = 1;
                CrownMove1(true);
            }
            else
            {
                Crown1.material = Mat_defaultCrown;
                isWinning01 = 0;
                CrownMove1(false);
            }
        }
        else if ((int)(PhotonNetwork.CurrentRoom.CustomProperties["MasterScore1"]) < (int)(PhotonNetwork.CurrentRoom.CustomProperties["GuestScore1"]))
        {
            if (PhotonNetwork.LocalPlayer.IsMasterClient == false)
            {
                Crown1.material = Mat_crown;
                isWinning01 = 0;
                CrownMove1(true);
            }
            else
            {
                Crown1.material = Mat_defaultCrown;
                isWinning01 = 1;
                CrownMove1(false);
            }
        }
        else if ((int)(PhotonNetwork.CurrentRoom.CustomProperties["MasterScore1"]) == (int)(PhotonNetwork.CurrentRoom.CustomProperties["GuestScore1"]))
        {
            Crown1.material = Mat_defaultCrown;
            isWinning01 = -1;
            CrownMove1(false);
        }

        if ((int)(PhotonNetwork.CurrentRoom.CustomProperties["MasterScore2"]) > (int)(PhotonNetwork.CurrentRoom.CustomProperties["GuestScore2"]))
        {
            if (PhotonNetwork.LocalPlayer.IsMasterClient == true)
            {
                Crown2.material = Mat_crown;
                isWinning02 = 1;
                CrownMove2(true);
            }
            else
            {
                Crown2.material = Mat_defaultCrown;
                isWinning02 = 0;
                CrownMove2(false);
            }
        }
        else if ((int)(PhotonNetwork.CurrentRoom.CustomProperties["MasterScore2"]) < (int)(PhotonNetwork.CurrentRoom.CustomProperties["GuestScore2"]))
        {
            if (PhotonNetwork.LocalPlayer.IsMasterClient == false)
            {
                Crown2.material = Mat_crown;
                isWinning02 = 0;
                CrownMove2(true);
            }
            else
            {
                Crown2.material = Mat_defaultCrown;
                isWinning02 = 1;
                CrownMove2(false);
            }
        }
        else if ((int)(PhotonNetwork.CurrentRoom.CustomProperties["MasterScore2"]) == (int)(PhotonNetwork.CurrentRoom.CustomProperties["GuestScore2"]))
        {
            Crown2.material = Mat_defaultCrown;
            isWinning02 = -1;
            CrownMove2(false);
        }
        
        if ((int)(PhotonNetwork.CurrentRoom.CustomProperties["MasterScore3"]) > (int)(PhotonNetwork.CurrentRoom.CustomProperties["GuestScore3"]))
        {
            if (PhotonNetwork.LocalPlayer.IsMasterClient == true)
            {
                Crown3.material = Mat_crown;
                isWinning03 = 1;
                CrownMove3(true);
            }
            else
            {
                Crown3.material = Mat_defaultCrown;
                isWinning03 = 0;
                CrownMove3(false);
            }
        }
        else if ((int)(PhotonNetwork.CurrentRoom.CustomProperties["MasterScore3"]) < (int)(PhotonNetwork.CurrentRoom.CustomProperties["GuestScore3"]))
        {
            if (PhotonNetwork.LocalPlayer.IsMasterClient == false)
            {
                Crown3.material = Mat_crown;
                isWinning03 = 0;
                CrownMove3(true);
            }
            else
            {
                Crown3.material = Mat_defaultCrown;
                isWinning03 = 1;
                CrownMove3(false);
            }
        }
        else if ((int)(PhotonNetwork.CurrentRoom.CustomProperties["MasterScore3"]) == (int)(PhotonNetwork.CurrentRoom.CustomProperties["GuestScore3"]))
        {
            Crown3.material = Mat_defaultCrown;
            isWinning03 = -1;
            CrownMove3(false);
        }

        if ((int)(PhotonNetwork.CurrentRoom.CustomProperties["MasterScore4"]) > (int)(PhotonNetwork.CurrentRoom.CustomProperties["GuestScore4"]))
        {
            if (PhotonNetwork.LocalPlayer.IsMasterClient == true)
            {
                Crown4.material = Mat_crown;
                isWinning04 = 1;
                CrownMove4(true);
            }
            else
            {
                Crown4.material = Mat_defaultCrown;
                isWinning04 = 0;
                CrownMove4(false);
            }
        }
        else if ((int)(PhotonNetwork.CurrentRoom.CustomProperties["MasterScore4"]) < (int)(PhotonNetwork.CurrentRoom.CustomProperties["GuestScore4"]))
        {
            if (PhotonNetwork.LocalPlayer.IsMasterClient == false)
            {
                Crown4.material = Mat_crown;
                isWinning04 = 0;
                CrownMove4(true);
            }
            else
            {
                Crown4.material = Mat_defaultCrown;
                isWinning04 = 1;
                CrownMove4(false);
            }
        }
        else if ((int)(PhotonNetwork.CurrentRoom.CustomProperties["MasterScore4"]) == (int)(PhotonNetwork.CurrentRoom.CustomProperties["GuestScore4"]))
        {
            Crown4.material = Mat_defaultCrown;
            isWinning04 = -1;
            CrownMove4(false);
        }
        
        
        #endregion 
    }
    void Start()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            Array.shuffle(ch);
            for (int i = 0; i < ch.Length; i++)
            {
                Cardlist.Add(ch[i]);
                PV.RPC("ShuffleSync", RpcTarget.Others, Cardlist[i].name);
            }
        }
        Crown1 = Crown1.GetComponent<Renderer>();
        Crown2 = Crown2.GetComponent<Renderer>();
        Crown3 = Crown3.GetComponent<Renderer>();
        Crown4 = Crown4.GetComponent<Renderer>();
    }
    [PunRPC]
    void ShuffleSync(string b)
    {
        for (int i = 0; i < ch.Length; i++)
        {
            if (ch[i].name == b)
            {
                Cardlist.Add(ch[i]);
            }
        }
    }
    public void SettingCard()
    {
        Btn_Pick.interactable = false;
        if ((PhotonNetwork.LocalPlayer.IsMasterClient == true && whosturn == true)
            || (PhotonNetwork.LocalPlayer.IsMasterClient == false && whosturn == false))
        {
            if (isPlay == false)
            {
                if (HandPanel.childCount == 0)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        Instantiate(Cardlist[0]).transform.SetParent(HandPanel);
                        PV.RPC("RemoveCard", RpcTarget.All);
                    }
                    isPlay = true;
                }
            }
            if (HandPanel.childCount != 0)
            {
                //txt_warning.text = "손에 들고 있는 카드를 전부 배치시켜주세요 ^0^";
                //StartCoroutine(FadeTextWarning());
            }
        }
        //txt_debug.text = "later " + Cardlist.Count.ToString() +"   ";
    }
    [PunRPC]
    void RemoveCard()
    {
        Cardlist.Remove(Cardlist[0]);
    }
    #endregion
    #region From DeckManager2
    public void StartPlaying()
    {
        if ((PhotonNetwork.LocalPlayer.IsMasterClient == true && whosturn == true)
       || (PhotonNetwork.LocalPlayer.IsMasterClient == false && whosturn == false))
        {
            if (HandPanel.childCount == 0 && LeftPanel.transform.childCount != 0 && RightPanel.transform.childCount != 0 && isPlay == true)
            {
                foreach (Transform child in LeftPanel.transform)
                {
                    LeftCards.Add(child.transform.name.Replace("(Clone)", ""), child.transform.name.Replace("(Clone)", ""));
                }
                foreach (Transform child in RightPanel.transform)
                {
                    RightCards.Add(child.transform.name.Replace("(Clone)", ""), child.transform.name.Replace("(Clone)", ""));
                }
                string LeftLableCard = JsonWriter.Serialize(LeftCards);
                string RightLableCard = JsonWriter.Serialize(RightCards);
                DeckPanelMovement();
                isPlay = false;
                BtnDeck.SetActive(false);
                
                PV.RPC("CardJson", RpcTarget.AllBuffered, LeftLableCard, RightLableCard);

                foreach (Transform child in LeftPanel.transform)
                {
                    Destroy(child.gameObject);
                }
                foreach (Transform child in RightPanel.transform)
                {
                    Destroy(child.gameObject);
                }

                Btn_Pick.interactable = true;

            }
            else if (RightPanel.transform.childCount == 5)
            {
                txt_warning.text = "왼쪽에도 카드를 배치시켜주세요 ^0^";
                StartCoroutine(FadeTextWarning());
            }
            else if (LeftPanel.transform.childCount == 5)
            {
                txt_warning.text = "오른쪽에도 카드를 배치시켜주세요 ^0^";
                StartCoroutine(FadeTextWarning());
            }
            else if (HandPanel.childCount != 0)
            {
                txt_warning.text = "손에 있는 카드를 좌우에 배치시켜주세요 ^0^";
                StartCoroutine(FadeTextWarning());
            }
        }

        
    }
    [PunRPC]
    void CardJson(string left, string right)
    {
        if ((PhotonNetwork.LocalPlayer.IsMasterClient == false && whosturn == true)
          || (PhotonNetwork.LocalPlayer.IsMasterClient == true && whosturn == false))
        {
            BtnCheckLabel.SetActive(true);
        }
        LLC = JsonReader.Deserialize<Dictionary<string, string>>(left);
        RLC = JsonReader.Deserialize<Dictionary<string, string>>(right);

        foreach (var item in LLC)
        {
            foreach (var i in ch)
            {
                if (i.name.Contains(item.Value))
                {
                    Instantiate(i).transform.SetParent(BtnLeftLabel);
                }
            }
        }
        foreach (var item in RLC)
        {
            foreach (var i in ch)
            {
                if (i.name.Contains(item.Value))
                {
                    Instantiate(i).transform.SetParent(BtnRightLabel);
                }
            }
        }
        LeftCards.Clear();
        RightCards.Clear();
        LLC.Clear();
        RLC.Clear();
    }
    public void Btn_Rightpanel()
    {
        if ((PhotonNetwork.LocalPlayer.IsMasterClient == true && whosturn == false)
               || (PhotonNetwork.LocalPlayer.IsMasterClient == false && whosturn == true))
        {
            SpawnRightPanel();
        }
        else
        {
            WaitPanel.SetActive(true);
            SpawnLeftPanel();
        }


        foreach (Transform child in BtnRightLabel.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in BtnLeftLabel.transform)
        {
            Destroy(child.gameObject);
        }
    }
    public void Btn_LeftPanel()
    {
        if ((PhotonNetwork.LocalPlayer.IsMasterClient == true && whosturn == false)
                  || (PhotonNetwork.LocalPlayer.IsMasterClient == false && whosturn == true))
        {
            SpawnLeftPanel();
        }
        else
        {
            WaitPanel.SetActive(true);
            SpawnRightPanel();
        }

        foreach (Transform child in BtnRightLabel.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in BtnLeftLabel.transform)
        {
            Destroy(child.gameObject);
        }
    }
    public void SpawnRightPanel()
    {
        foreach (Transform child in BtnRightLabel.transform)
        {
            #region children
            if (child.name == "Card 0(Clone)")
            {
                Instantiate(setChar[0]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 1(Clone)")
            {
                Instantiate(setChar[0]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 2(Clone)")
            {
                Instantiate(setChar[0]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 3(Clone)")
            {
                Instantiate(setChar[0]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 4(Clone)")
            {
                Instantiate(setChar[0]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 5(Clone)")
            {
                Instantiate(setChar[1]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 6(Clone)")
            {
                Instantiate(setChar[1]).transform.SetParent(CharSetObject);
            }
            if (child.name == "Card 7(Clone)")
            {
                Instantiate(setChar[1]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 8(Clone)")
            {
                Instantiate(setChar[1]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 9(Clone)")
            {
                Instantiate(setChar[2]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 10(Clone)")
            {
                Instantiate(setChar[2]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 11(Clone)")
            {
                Instantiate(setChar[2]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 12(Clone)")
            {
                Instantiate(setChar[2]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 13(Clone)")
            {
                Instantiate(setChar[3]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 14(Clone)")
            {
                Instantiate(setChar[3]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 15(Clone)")
            {
                Instantiate(setChar[4]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 16(Clone)")
            {
                Instantiate(setChar[4]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 17(Clone)")
            {
                Instantiate(setChar[5]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 18(Clone)")
            {
                Instantiate(setChar[5]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 19(Clone)")
            {
                Instantiate(setChar[5]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 20(Clone)")
            {
                Instantiate(setChar[5]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 21(Clone)")
            {
                Instantiate(setChar[6]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 22(Clone)")
            {
                Instantiate(setChar[6]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 23(Clone)")
            {
                Instantiate(setChar[6]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 24(Clone)")
            {
                Instantiate(setChar[6]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 25(Clone)")
            {
                Instantiate(setChar[7]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 26(Clone)")
            {
                Instantiate(setChar[7]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 27(Clone)")
            {
                Instantiate(setChar[8]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 28(Clone)")
            {
                Instantiate(setChar[8]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 29(Clone)")
            {
                Instantiate(setChar[9]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 30(Clone)")
            {
                Instantiate(setChar[10]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 31(Clone)")
            {
                Instantiate(setChar[10]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 32(Clone)")
            {
                Instantiate(setChar[10]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 33(Clone)")
            {
                Instantiate(setChar[10]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 34(Clone)")
            {
                Instantiate(setChar[11]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 35(Clone)")
            {
                Instantiate(setChar[11]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 36(Clone)")
            {
                Instantiate(setChar[12]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 37(Clone)")
            {
                Instantiate(setChar[12]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 38(Clone)")
            {
                Instantiate(setChar[13]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 39(Clone)")
            {
                Instantiate(setChar[14]).transform.SetParent(CharSetObject);
            }

            #endregion
        }
        BtnCheckLabel.SetActive(false);
        SetCharGrid();
        BtnMapReset.SetActive(false);
    }
    public void SpawnLeftPanel()
    {
        foreach (Transform child in BtnLeftLabel.transform)
        {
            #region children
            if (child.name == "Card 0(Clone)")
            {
                Instantiate(setChar[0]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 1(Clone)")
            {
                Instantiate(setChar[0]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 2(Clone)")
            {
                Instantiate(setChar[0]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 3(Clone)")
            {
                Instantiate(setChar[0]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 4(Clone)")
            {
                Instantiate(setChar[0]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 5(Clone)")
            {
                Instantiate(setChar[1]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 6(Clone)")
            {
                Instantiate(setChar[1]).transform.SetParent(CharSetObject);
            }
            if (child.name == "Card 7(Clone)")
            {
                Instantiate(setChar[1]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 8(Clone)")
            {
                Instantiate(setChar[1]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 9(Clone)")
            {
                Instantiate(setChar[2]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 10(Clone)")
            {
                Instantiate(setChar[2]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 11(Clone)")
            {
                Instantiate(setChar[2]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 12(Clone)")
            {
                Instantiate(setChar[2]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 13(Clone)")
            {
                Instantiate(setChar[3]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 14(Clone)")
            {
                Instantiate(setChar[3]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 15(Clone)")
            {
                Instantiate(setChar[4]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 16(Clone)")
            {
                Instantiate(setChar[4]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 17(Clone)")
            {
                Instantiate(setChar[5]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 18(Clone)")
            {
                Instantiate(setChar[5]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 19(Clone)")
            {
                Instantiate(setChar[5]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 20(Clone)")
            {
                Instantiate(setChar[5]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 21(Clone)")
            {
                Instantiate(setChar[6]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 22(Clone)")
            {
                Instantiate(setChar[6]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 23(Clone)")
            {
                Instantiate(setChar[6]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 24(Clone)")
            {
                Instantiate(setChar[6]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 25(Clone)")
            {
                Instantiate(setChar[7]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 26(Clone)")
            {
                Instantiate(setChar[7]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 27(Clone)")
            {
                Instantiate(setChar[8]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 28(Clone)")
            {
                Instantiate(setChar[8]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 29(Clone)")
            {
                Instantiate(setChar[9]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 30(Clone)")
            {
                Instantiate(setChar[10]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 31(Clone)")
            {
                Instantiate(setChar[10]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 32(Clone)")
            {
                Instantiate(setChar[10]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 33(Clone)")
            {
                Instantiate(setChar[10]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 34(Clone)")
            {
                Instantiate(setChar[11]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 35(Clone)")
            {
                Instantiate(setChar[11]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 36(Clone)")
            {
                Instantiate(setChar[12]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 37(Clone)")
            {
                Instantiate(setChar[12]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 38(Clone)")
            {
                Instantiate(setChar[13]).transform.SetParent(CharSetObject);
            }
            else if (child.name == "Card 39(Clone)")
            {
                Instantiate(setChar[14]).transform.SetParent(CharSetObject);
            }
            
            
            #endregion
        }
        BtnCheckLabel.SetActive(false);
        SetCharGrid();
        BtnMapReset.SetActive(false);

    }
    #endregion
    public void SpawnTimer()
    {
        Instantiate(TimerPrefab).transform.SetParent(TimerPanel);
    }
    public void DestroyTimer()
    {
        for (int i = 0; i < TimerPanel.childCount; i++)
        {
            Destroy(TimerPanel.GetChild(0).gameObject);
        }

        TimerPanel.DetachChildren();
    }
    IEnumerator FadeTextWarning()
    {
        txt_warning.color = new Color(txt_warning.color.r, txt_warning.color.g, txt_warning.color.b, 1);
        while (txt_warning.color.a > 0f)
        {
            txt_warning.color = new Color(txt_warning.color.r, txt_warning.color.g, txt_warning.color.b, txt_warning.color.a - (Time.deltaTime * 0.5f));
            yield return null;
        }
    }

    public void TurnSync(bool turn)
    {
        PV.RPC("TurnControl", RpcTarget.AllBuffered, turn);
    }

    [PunRPC]
    void TurnControl(bool turn)
    {
        whosturn = turn;
    }
    #region crown move
    public void CrownMove1(bool Winning)
    {
        Animator animator = Crown1.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("isWinning", Winning);
        }
    }
    public void CrownMove2(bool Winning)
    {
        Animator animator = Crown2.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("isWinning", Winning);
        }
    }
    public void CrownMove3(bool Winning)
    {
        Animator animator = Crown3.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("isWinning", Winning);
        }
    }
    public void CrownMove4(bool Winning)
    {
        Animator animator = Crown4.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("isWinning", Winning);
        }
    }
    #endregion
}

