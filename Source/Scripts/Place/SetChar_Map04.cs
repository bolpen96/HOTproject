using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetChar_Map04 : MonoBehaviour
{
    public Material BasicMaterial;
    public Material ChangeMaterial;

    public Renderer cubeRenderer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Red" || other.gameObject.tag == "Yellow" || other.gameObject.tag == "Blue")
        {
            other.transform.SetParent(GameManager.I.GridSet04.transform);
            GameManager.I.Btn_SetChar.SetActive(true);
            GameManager.I.SetFieldScore();
            GameManager.I.MoveChildrenIntoGrid04();
        }
    }
}
