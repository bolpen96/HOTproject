using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetChar_Map02 : MonoBehaviour
{
    public Material BasicMaterial;
    public Material ChangeMaterial;

    public Renderer cubeRenderer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Yellow")
        {
            other.transform.SetParent(GameManager.I.GridSet02.transform);
            GameManager.I.Btn_SetChar.SetActive(true);
            GameManager.I.SetFieldScore();
            GameManager.I.MoveChildrenIntoGrid02();
        }
    }
}
