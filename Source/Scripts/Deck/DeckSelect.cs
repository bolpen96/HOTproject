using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public class DeckSelect : MonoBehaviourPun
{
    PhotonView PV;
    public Button Btn_done;
    public GameObject Checkable;
    public GameObject panelLeft;
    public GameObject panelRight;
    bool whosturn; // true : 마스터클라이언트가 고름, false : 손님이 고름
    public bool whichselected; //true 왼쪽, false 오른쪽
    public void Start()
    {
        whosturn = GameManageTest.I.whosturn;
        if (whosturn && !PhotonNetwork.LocalPlayer.IsMasterClient)
        {
                panelLeft.GetComponent<Button>().interactable = false;
                panelRight.GetComponent<Button>().interactable = false;
        }
        else
        {
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                panelLeft.GetComponent<Button>().interactable = false;
                panelRight.GetComponent<Button>().interactable = false;
            }
        }
    }
    public void SelectPanel1()
    {
        whichselected = true;
        GameManager.I.txt_debug.text = "left";
    }
    public void SelectPanel2()
    {
        whichselected = false;
        GameManager.I.txt_debug.text = "right";
    }
}
