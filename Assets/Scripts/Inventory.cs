using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public GameObject inventoryPanel; 
    public GameObject slotPrefab; 
    public int slotCount; 
    public GameObject[] itemPrefabs;

    void Start()
    {
        for (int i = 0; i < slotCount; i++)
        {
            Slot slot = Instantiate(slotPrefab, inventoryPanel.transform) .GetComponent<Slot>();
            if (i < itemPrefabs.Length)
            {
                GameObject item = Instantiate(itemPrefabs[i], slot.transform); 
                RectTransform rt = item.GetComponent<RectTransform>();
                if (rt != null)
                {
                    rt.anchoredPosition = Vector2.zero;
                } 
                slot.currentItem = item;
            }
        }
    }

    public bool AddItem(GameObject itemPrefab)
    {
        foreach (Transform t in inventoryPanel.transform)
        {
            Slot slot = t.GetComponent<Slot>();
            if (slot != null && slot.currentItem == null)
            {
                GameObject newItem = Instantiate(itemPrefab, slot.transform); 
                newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero; 
                slot.currentItem = newItem; 
                return true;
            }
        } Debug.Log("Inventory is full!"); return false;
    }
}