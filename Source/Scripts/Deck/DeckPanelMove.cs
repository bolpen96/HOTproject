using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckPanelMove : MonoBehaviour
{
    public GameObject DeckPanel;
    public void DeckPanelMovement()
    {
        Animator animator = DeckPanel.GetComponent<Animator>();

        if(animator != null)
        {
            bool isOpen = animator.GetBool("Isopen");
            animator.SetBool("Isopen", !isOpen);
        }
    }
}
