using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlotUI : MonoBehaviour
{
    public Image icon;
    public TMP_Text quantityText;
    public int index;
    public InventoryUI inventoryUI;

    public void Set(InventorySlot slot)
    {
        if (slot.IsEmpty)
        {
            icon.enabled = false;
            quantityText.text = "";
        }
        else
        {
            icon.enabled = true;
            icon.sprite = slot.item.icon;
            quantityText.text = slot.quantity > 1 ? slot.quantity.ToString() : "";
        }
    }
}