using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragNDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Inventory inventory;
    public Transform inventoryPanel;
    public Slot mySlot;
    public Slot destinationSlot;
    private Image myImage;

    private void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        inventoryPanel = transform.parent.parent;
        myImage = this.GetComponent<Image>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        mySlot = transform.parent.GetComponent<Slot>();
        transform.SetParent(inventoryPanel);
        transform.position = eventData.position;
        myImage.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (destinationSlot != null)
        {
            if (destinationSlot.slotInfo.id != mySlot.slotInfo.id)
            {
                inventory.SwapSlots(mySlot.slotInfo.id, destinationSlot.slotInfo.id, this.transform, destinationSlot.itemImage.transform);
                destinationSlot.itemImage.transform.localPosition = Vector3.zero;
            }
            else
            {
                inventory.SwapSlots(mySlot.slotInfo.id, mySlot.slotInfo.id, this.transform, this.transform);
            }
        }
        else
        {
            inventory.SwapSlots(mySlot.slotInfo.id, mySlot.slotInfo.id, this.transform, this.transform);
            inventory.RemoveItem(mySlot.slotInfo.itemId, mySlot.slotInfo);
        }
        myImage.raycastTarget = true;
        destinationSlot = null;
    }
}
