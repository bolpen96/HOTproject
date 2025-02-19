using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Photon.Pun;
using Photon.Realtime;

public class MapPlacementController : MonoBehaviourPun
{
    public GameObject BtnDeck;

    bool isSpawn = false;
    private PlacementIndicator placementIndicator;
    private void Start()
    {
        
        placementIndicator = FindObjectOfType<PlacementIndicator>();
    }


    private void Update()
    {
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began && isSpawn == false)
        {
            isSpawn = true;
            
            GameManager.I.Field_map.SetActive(true);
            GameManager.I.Btn_Setfield.SetActive(true);
            GameManager.I.Btn_Resetfield.SetActive(true);
            GameManager.I.Field_map.transform.position = placementIndicator.transform.position;
            GameManager.I.Field_map.transform.rotation = placementIndicator.transform.rotation;
            GameManager.I.checkMap.SetActive(false);

    
        }
    }
    public void ResetMap()
    {
        GameManager.I.Btn_Setfield.SetActive(false);
        GameManager.I.checkMap.SetActive(true);
        GameManager.I.Field_map.SetActive(false);
        BtnDeck.SetActive(false);
        isSpawn = false;
    }

}
