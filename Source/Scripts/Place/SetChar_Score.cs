using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class SetChar_Score : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Red" && GameManager.I.isWinning01 == 1 && PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            if (other.name == "Garlic(Clone)")
            {
                GameManager.I.total_Score00 += 1;
            }
            else if (other.name == "avocado(Clone)")
            {
                GameManager.I.total_Score00 += 2;
            }
            else if (other.name == "Tomato(Clone)")
            {
                GameManager.I.total_Score00 += 3;
            }
            else if (other.name == "HotDog(Clone)")
            {
                GameManager.I.total_Score00 += 4;
            }
            else if (other.name == "Rapping(Clone)")
            {
                GameManager.I.total_Score00 += 5;
            }
            
            Destroy(other.gameObject);
        }
        else if(other.tag == "Red" && GameManager.I.isWinning01 == 0 && PhotonNetwork.LocalPlayer.IsMasterClient == false)
        {
            if (other.name == "Garlic(Clone)")
            {
                GameManager.I.other_total00 += 1;
            }
            else if (other.name == "avocado(Clone)")
            {
                GameManager.I.other_total00 += 2;
            }
            else if (other.name == "Tomato(Clone)")
            {
                GameManager.I.other_total00 += 3;
            }
            else if (other.name == "HotDog(Clone)")
            {
                GameManager.I.other_total00 += 4;
            }
            else if (other.name == "Rapping(Clone)")
            {
                GameManager.I.other_total00 += 5;
            }
            Destroy(other.gameObject);
        }
        else if (other.tag == "Yellow" && GameManager.I.isWinning02 == 1 && PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            if (other.name == "Hedgehog(Clone)")
            {
                GameManager.I.total_Score00 += 1;
            }
            else if (other.name == "Penguin(Clone)")
            {
                GameManager.I.total_Score00 += 2;
            }
            else if (other.name == "Giraffe(Clone)")
            {
                GameManager.I.total_Score00 += 3;
            }
            else if (other.name == "Crocodile(Clone)")
            {
                GameManager.I.total_Score00 += 4;
            }
            else if (other.name == "Bear(Clone)")
            {
                GameManager.I.total_Score00 += 5;
            }
            Destroy(other.gameObject);
        }
        else if (other.tag == "Yellow" && GameManager.I.isWinning02 == 0 && PhotonNetwork.LocalPlayer.IsMasterClient == false)
        {
            if (other.name == "Hedgehog(Clone)")
            {
                GameManager.I.other_total00 += 1;
            }
            else if (other.name == "Penguin(Clone)")
            {
                GameManager.I.other_total00 += 2;
            }
            else if (other.name == "Giraffe(Clone)")
            {
                GameManager.I.other_total00 += 3;
            }
            else if (other.name == "Crocodile(Clone)")
            {
                GameManager.I.other_total00 += 4;
            }
            else if (other.name == "Bear(Clone)")
            {
                GameManager.I.other_total00 += 5;
            }
            
            Destroy(other.gameObject);
        }
        else if (other.tag == "Blue" && GameManager.I.isWinning03 == 1 && PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            if (other.name == "Mini(Clone)")
            {
                GameManager.I.total_Score00 += 1;
            }
            else if (other.name == "OE(Clone)")
            {
                GameManager.I.total_Score00 += 2;
            }
            else if (other.name == "Tobby(Clone)")
            {
                GameManager.I.total_Score00 += 3;
            }
            else if (other.name == "UFO(Clone)")
            {
                GameManager.I.total_Score00 += 4;
            }
            else if (other.name == "Chubby(Clone)")
            {
                GameManager.I.total_Score00 += 5;
            }
            Destroy(other.gameObject);
        }
        else if (other.tag == "Blue" && GameManager.I.isWinning03 == 0 && PhotonNetwork.LocalPlayer.IsMasterClient == false)
        {
            if (other.name == "Mini(Clone)")
            {
                GameManager.I.other_total00 += 1;
            }
            else if (other.name == "OE(Clone)")
            {
                GameManager.I.other_total00 += 2;
            }
            else if (other.name == "Tobby(Clone)")
            {
                GameManager.I.other_total00 += 3;
            }
            else if (other.name == "UFO(Clone)")
            {
                GameManager.I.other_total00 += 4;
            }
            else if (other.name == "Chubby(Clone)")
            {
                GameManager.I.other_total00 += 5;
            }
            Destroy(other.gameObject);
        }

        GameManager.I.Btn_SetChar.SetActive(true);
        GameManager.I.SetFieldScore();
    }
}
