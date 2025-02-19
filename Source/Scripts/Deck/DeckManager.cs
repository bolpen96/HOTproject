using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DeckShuffle;
using System.Linq;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
#region 드래그에 관련된 클래스

#endregion
public class DeckManager : MonoBehaviourPun, IPunObservable
{
    GameObject LeftPanel;
    GameObject RightPanel;

    //GameObject DeckPanel;
    GameObject[] setChar;

    GameObject DeckPanel;
    Transform HandPanel;
    Transform Btnleftlabel;
    Transform Btnrightlabel;
    GameObject BtnChecklabel;
    GameObject BtnDeck;

    GameObject BtnMapReset;
    bool haspickclicked=false;
    bool letyoupick = false;
    #region Random System
    public List<GameObject> Cardlist = new List<GameObject>();
    public GameObject[] ch;
    bool isPlay = false; //카드배치페이즈에 5장만 쓸수있도록
    bool whosturn; //true : 마스터, false : 손님
    void Start()
    {
        LeftPanel = GameManager.I.LeftPanel;
        RightPanel = GameManager.I.RightPanel;
        DeckPanel = GameManager.I.DeckPanel;
        HandPanel = GameManager.I.HandPanel;
        Btnleftlabel = GameManager.I.BtnLeftLabel;
        Btnrightlabel = GameManager.I.BtnRightLabel;
        BtnChecklabel = GameManager.I.BtnCheckLabel;
        BtnMapReset = GameManager.I.BtnMapReset;

        Array.shuffle(ch);
        for(int i =0;i<ch.Length; i++)
        {
            Cardlist.Add(ch[i]);
        }
    }
    public void SettingCard()
    {
        //if (haspickclicked) return;
        GameManager.I.txt_debug.text = "ㅇㅇ";
        if(isPlay == false)
        {
            if(HandPanel.childCount == 0)
            {
                for(int i = 0; i < 5; i++)
                {
                    Instantiate(Cardlist[0]).transform.SetParent(HandPanel);
                    Cardlist.Remove(Cardlist[0]);
                }
                isPlay = true;
            }
        }
    }
    #endregion

    public void StartPlaying()
    {

        if (HandPanel.childCount == 0 && LeftPanel.transform.childCount != 0 && RightPanel.transform.childCount != 0)
        {
            foreach(Transform eachChild in RightPanel.transform)
            {
                if(RightPanel.transform.childCount != 0)
                {
                    while(RightPanel.transform.childCount != 0)
                    {
                        RightPanel.transform.GetChild(0).transform.SetParent(Btnrightlabel);
                    }
                }
                if(LeftPanel.transform.childCount != 0)
                {
                    while(LeftPanel.transform.childCount != 0)
                    {
                        LeftPanel.transform.GetChild(0).transform.SetParent(Btnleftlabel);
                    }
                }
            }
            isPlay = false;
            BtnDeck.SetActive(false);
            BtnChecklabel.SetActive(true);
        }
    }

    public void Btn_Rightpanel()
    {
        Instantiate(setChar[0], new Vector3(0, 0.15f, 0f), Quaternion.identity);
        Instantiate(setChar[0], new Vector3(0.4f, 0.15f, 0f), Quaternion.identity);

        BtnChecklabel.SetActive(false);

        if(DeckPanel != null)
        {
            Animator animator = DeckPanel.GetComponent<Animator>();

            if(animator != null)
            {
                bool isOpen = animator.GetBool("Isopen");
                animator.SetBool("Isopen", !isOpen);
            }
        }
        foreach(Transform child in Btnrightlabel.transform)
        {
            Destroy(child.gameObject);
        }
        foreach(Transform child in Btnleftlabel.transform)
        {
            Destroy(child.gameObject);
        }
        BtnMapReset.SetActive(false);
    }

    public void Btn_LeftPanel()
    {
        Instantiate(setChar[0], new Vector3(0, 0.15f, 0f), Quaternion.identity);
        Instantiate(setChar[0], new Vector3(0.4f, 0.15f, 0f), Quaternion.identity);

        BtnChecklabel.SetActive(false);

        if (DeckPanel != null)
        {
            Animator animator = DeckPanel.GetComponent<Animator>();

            if (animator != null)
            {
                bool isOpen = animator.GetBool("Isopen");
                animator.SetBool("Isopen", !isOpen);
            }
        }
        foreach (Transform child in Btnrightlabel.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in Btnleftlabel.transform)
        {
            Destroy(child.gameObject);
        }
        BtnMapReset.SetActive(false);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(haspickclicked);
        }
        else
        {
            haspickclicked = (bool)stream.ReceiveNext();
        }
    }
}
