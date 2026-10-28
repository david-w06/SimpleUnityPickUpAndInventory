using UnityEngine;

public class Collectible : MonoBehaviour
{
    // Assign an ItemData ScriptableObject in the Inspector
    public ItemData itemData;
    public int amount = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CollectionManager.Instance.CollectItem(itemData, amount);
            Destroy(gameObject);
        }
    }
}
