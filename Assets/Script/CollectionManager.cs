using UnityEngine;

public class CollectionManager : MonoBehaviour
{
    public static CollectionManager Instance;

    public Inventory inventory;

    private void Awake()
    {
        Instance = this;
    }

    public void CollectItem(ItemData item, int amount)
    {
        if (item == null)
        {
            Debug.LogWarning("CollectItem called with null ItemData!");
            return;
        }

        inventory.AddItem(item, amount);
        Debug.Log($"Collected: {item.itemName} x{amount}");
    }
}