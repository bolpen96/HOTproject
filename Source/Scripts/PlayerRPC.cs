using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


public class PlayerRPC : MonoBehaviourPunCallbacks
{
    public PhotonView PV;
    public bool done = false;
    public bool ready = false;
    public void OnClick_BtnDone1() //맵설치후done
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient == false)
        {
            PV.RPC("CheckDone", RpcTarget.MasterClient, true);
            GameManager.I.BtnMapReset.gameObject.SetActive(false);
            GameManager.I.Btn_Setfield.gameObject.SetActive(false); 
        }
        if (done)
            PV.RPC("DoneClick1", RpcTarget.AllBuffered);
    }
    [PunRPC]
    void CheckDone(bool ddone)
    {
        done = ddone;
    }
    [PunRPC]
    void DoneClick1()
    {
        GameManager.I.OnClick_Setfield();
    }
    public void OnClick_BtnPick() //pick버튼
    {
        PV.RPC("PickClick", RpcTarget.All);
    }
    [PunRPC]
    void PickClick()
    {

        GameManager.I.SettingCard();

    }
    public void OnClick_BtnDone2() //카드분배done
    {
        PV.RPC("DoneClick2", RpcTarget.All);
    }
    [PunRPC]
    void DoneClick2()
    {
        GameManager.I.StartPlaying();
    }
    public void OnClick_Left() //왼쪽선택
    {
        PV.RPC("LeftClick", RpcTarget.All);

    }
    [PunRPC]
    void LeftClick()
    {
        GameManager.I.Btn_LeftPanel();

    }
    public void OnClick_Right() //오른쪽
    {
        PV.RPC("RightClick", RpcTarget.All);
    }
    [PunRPC]
    void RightClick()
    {
        GameManager.I.Btn_Rightpanel();
    }

    public void OnClick_ReadyCharSet()
    {
        PV.RPC("ReadyCharSet", RpcTarget.All, true);
    }
    [PunRPC]
    void ReadyCharSet(bool isReady)
    {
        
        if (ready == false)
        {
            GameManager.I.WaitPanel.SetActive(false);
            ready = isReady;
        }
        else if (ready == true)
        {
            GameManager.I.whosturn = !(GameManager.I.whosturn);

            GameManager.I.TurnSync(GameManager.I.whosturn);

            GameManager.I.SetField();
            if (GameManager.I.Cardlist.Count == 0)
            {
                PV.RPC("End", RpcTarget.All);
                
            ready = false;
                return;
            }
            ready = false;
            if ((PhotonNetwork.LocalPlayer.IsMasterClient == false && GameManager.I.whosturn == false) ||
                (PhotonNetwork.LocalPlayer.IsMasterClient == true && GameManager.I.whosturn == true))
            {
                GameManager.I.txt_debug.text = GameManager.I.whosturn.ToString();
                GameManager.I.DeckPanelMovement();
            }

            return;
        }
        
    }
    [PunRPC]
    public void End()
    {
        GameManager.I.score_total += GameManager.I.total_Score00;
        GameManager.I.Otherscore_total += GameManager.I.other_total00;

        if (GameManager.I.isWinning01 == 1) { GameManager.I.score_total += 5; }
        if (GameManager.I.isWinning02 == 1) { GameManager.I.score_total += 7; }
        if (GameManager.I.isWinning03 == 1) { GameManager.I.score_total += 9; }
        if (GameManager.I.isWinning04 == 1) { GameManager.I.score_total += 3; }


        if (GameManager.I.isWinning01 == 0) { GameManager.I.Otherscore_total += 5; }
        if (GameManager.I.isWinning02 == 0) { GameManager.I.Otherscore_total += 7; }
        if (GameManager.I.isWinning03 == 0) { GameManager.I.Otherscore_total += 9; }
        if (GameManager.I.isWinning04 == 0) { GameManager.I.Otherscore_total += 3; }

        if (GameManager.I.score_total > GameManager.I.Otherscore_total)
        {
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                GameManager.I.WinPanel.SetActive(true);
            }
            else
            {
                GameManager.I.LosePanel.SetActive(true);
            }
        }
        else if (GameManager.I.score_total < GameManager.I.Otherscore_total)
        {
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                GameManager.I.LosePanel.SetActive(true);
            }
            else
            {
                GameManager.I.WinPanel.SetActive(true);
            }
        }
        else
        {
            GameManager.I.LosePanel.SetActive(true);
        }
    }
    
    public void Btn_End()
    {
        PV.RPC("EndClick", RpcTarget.All);
    }
    [PunRPC]
    void EndClick()
    {
        PhotonNetwork.LoadLevel(0);
        PhotonNetwork.LeaveRoom();
    }
}
