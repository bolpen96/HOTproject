using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class LeftPanel : MonoBehaviour, IDropHandler,IPointerEnterHandler,IPointerExitHandler
{
    public Dragable.Slot typeOfItem = Dragable.Slot.Free;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;
        Dragable d = eventData.pointerDrag.GetComponent<Dragable>();

        if(d != null)
        {
            if(typeOfItem == d.typeOfItem || typeOfItem == Dragable.Slot.Free)
            {
                d.placeholderParent = this.transform;
                
            }
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;
        Dragable d = eventData.pointerDrag.GetComponent<Dragable>();

        if(d != null && d.placeholderParent == this.transform)
        {
            if(typeOfItem == d.typeOfItem || typeOfItem == Dragable.Slot.Free)
            {
                d.placeholderParent = d.parentToReturnTo;
            }
        }
    }
    public void OnDrop(PointerEventData eventData)
    {
        Dragable d = eventData.pointerDrag.GetComponent<Dragable>();

        if(d != null)
        {
            if(typeOfItem == d.typeOfItem || typeOfItem== Dragable.Slot.Free)
            {
                d.parentToReturnTo = this.transform;
            }
        }
    }
}
