using UnityEngine;
using System.Collections.Generic;
using System;

public class Inventory : MonoBehaviour
{
    public int size = 10;
    public List<InventorySlot> slots = new List<InventorySlot>();
    public event Action OnInventoryChanged;

    void Awake()
    {
        for (int i = 0; i < size; i++)
        {
            slots.Add(new InventorySlot());
        }
    }

    public void AddItem(ItemData item, int amount)
    {
        // 1. Fill existing stacks first
        foreach (var slot in slots)
        {
            if (!slot.IsEmpty && slot.item == item && item.stackable)
            {
                int spaceLeft = item.maxStack - slot.quantity;

                if (spaceLeft > 0)
                {
                    int toAdd = Mathf.Min(spaceLeft, amount);
                    slot.quantity += toAdd;
                    amount -= toAdd;

                    if (amount <= 0)
                    {
                        OnInventoryChanged?.Invoke();
                        return;
                    }
                }
            }
        }

        // 2. Fill empty slots
        foreach (var slot in slots)
        {
            if (slot.IsEmpty)
            {
                slot.item = item;
                int toAdd = item.stackable ? Mathf.Min(item.maxStack, amount) : 1;
                slot.quantity = toAdd;
                amount -= toAdd;

                if (amount <= 0)
                {
                    OnInventoryChanged?.Invoke();
                    return;
                }
            }
        }

        // 3. Inventory full
        if (amount > 0)
        {
            Debug.Log($"Inventory full! Could not add {amount} x {item.itemName}");
        }

        OnInventoryChanged?.Invoke();
    }

    public void Swap(int indexA, int indexB)
    {
        InventorySlot temp = slots[indexA];
        slots[indexA] = slots[indexB];
        slots[indexB] = temp;

        OnInventoryChanged?.Invoke();
    }
}