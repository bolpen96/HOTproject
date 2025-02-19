using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetChar_Map03 : MonoBehaviour
{
    public Material BasicMaterial;
    public Material ChangeMaterial;

    public Renderer cubeRenderer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Blue")
        {
            other.transform.SetParent(GameManager.I.GridSet03.transform);
            GameManager.I.Btn_SetChar.SetActive(true);
            GameManager.I.SetFieldScore();
            GameManager.I.MoveChildrenIntoGrid03();
        }
    }
}
