using UnityEngine;
using TMPro;

public class ItemHandManager : MonoBehaviour
{
    public InventarManager InventarManager;
    public InventarUI InventarUI;

    private ItemStack CurrentItemStack;
    private Item CurrentItem;
    private System.Type CurrentItemType;
    private GameObject CurrentItemModell = null;
    private GameObject LastItemModell = null;

    public GameObject Camera;
    public GameObject ItemPlace;

    void Update()
    {
        ShowItemsInHand();

        if (Input.GetMouseButtonDown(0))
        {
            if (CurrentItem is Potion potion && !InventarUI.InventarOpen)
            {
                potion.UsePotion();
                CurrentItemStack.Amount--;
                InventarUI.UpdateInventory(InventarManager.Inventare[1]);
            }
        }
    }

    private void ShowItemsInHand()
    {
        if (InventarManager.Inventare[1].Items.Count > 0 && InventarManager.CurrentHotbarSlot < InventarManager.Inventare[1].Items.Count)
        {
            CurrentItemStack = InventarManager.Inventare[1].Items[InventarManager.CurrentHotbarSlot];

            // Falls `InitialisedItems` eine Liste ist:
            if (CurrentItemStack.ID >= 0 && CurrentItemStack.ID < InventarManager.InitialisedItems.Count)
            {
                CurrentItem = InventarManager.InitialisedItems[CurrentItemStack.ID];
            }
            else
            {
                CurrentItem = null;
            }

            CurrentItemType = CurrentItem?.GetType();
            CurrentItemModell = CurrentItem?.Modell;
        }
        else
        {
            CurrentItem = null;
            CurrentItemType = null;
            CurrentItemModell = null;
        }

        if (CurrentItemModell != null)
        {
            CurrentItemModell.SetActive(true);
        }

        if (LastItemModell != CurrentItemModell && LastItemModell != null)
        {
            LastItemModell.SetActive(false);
        }

        LastItemModell = CurrentItemModell;
    }


}
