using UnityEngine;
using System.Collections.Generic;

public class CraftingManager : MonoBehaviour
{
    public InventarManager InventarManager;
    public InventarUI InventarUI;
    public CraftingUIManager CraftingUIManager;

    public GameObject CraftingPanel;
    
    public List<Recipe> Recipes = new List<Recipe>();

    private void Start()
    {
        Recipes.Add(new Recipe(0, new Dictionary<int, int> { { 1, 4 }, { 2, 1 } }, 0, 1));
        Recipes.Add(new Recipe(1, new Dictionary<int, int> { { 1, 8 }, { 2, 2 } }, 0, 2));
        Recipes.Add(new Recipe(1, new Dictionary<int, int> { { 1, 8 }, { 2, 2 } }, 0, 2));
        Recipes.Add(new Recipe(2, new Dictionary<int, int> { { 2, 4 }, { 0, 1 } }, 1, 20));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && InventarManager.CraftingTableOpen)
        {
            AcessCraftingTable();
        }
    }

    public void Craft(int RecipeID)
    {
        Dictionary<int, int> FoundItems = new Dictionary<int, int>();
        List<int> Inventorys = new List<int>();
        
        int RecipeIndex = 0;

        for (int i = 0; i < Recipes.Count; i++)
        {
            if (Recipes[i].ID == RecipeID)
            {
                RecipeIndex = i;
                break;
            }
        }

        foreach (int i in Recipes[RecipeIndex].RequiredItems.Keys)
        {
            FoundItems.Add(i, 0);
        }

        for (int i = 0; i < 2; i++)
        {
            foreach (ItemStack ItemStack in InventarManager.Inventare[i].Items)
            {
                foreach (int ID in Recipes[RecipeIndex].RequiredItems.Keys)
                {
                    if (ItemStack.ID == ID)
                    {
                        FoundItems[ID] += ItemStack.Amount;

                        if (!Inventorys.Contains(InventarManager.Inventare[i].ID))
                        {
                            Inventorys.Add(InventarManager.Inventare[i].ID);
                        }
                    }
                }
            }
        }

        int FittingItems = 0;

        foreach (KeyValuePair<int, int> ID in FoundItems)
        {
            if (ID.Value >= Recipes[RecipeIndex].RequiredItems[ID.Key])
            {
                FittingItems++;
            }
        }
        
        if (FittingItems == Recipes[RecipeIndex].RequiredItems.Count)
        {
            foreach (int i in Inventorys)
            {
                foreach (KeyValuePair<int, int> ID in FoundItems)
                {
                    InventarManager.RemoveItem(ID.Key, Recipes[RecipeIndex].RequiredItems[ID.Key], InventarManager.Inventare[i]);
                }
            }

            InventarManager.AddItem(Recipes[RecipeIndex].ResultID, Recipes[RecipeIndex].ResultAmount, InventarManager.Inventare[0]);
            InventarUI.UpdateInventory(InventarManager.Inventare[0]);
        }
        else
        {

        }
    }

    public void AcessCraftingTable()
    {
        if (InventarManager.CraftingTableOpen)
        {
            InventarManager.MainInventarObject.SetActive(false);
            InventarUI.InventarOpen = false;

            InventarManager.MainInventarObject.transform.position = InventarManager.MainInventarBasePos.transform.position;
            InventarManager.InventarUI.CameraMovement.UnlockCursor();
            CraftingPanel.SetActive(false);

            InventarManager.CraftingTableOpen = false;
        }
        else
        {
            InventarManager.CraftingTableOpen = true;

            InventarManager.MainInventarObject.SetActive(true);
            CraftingPanel.SetActive(true);
            InventarUI.InventarOpen = true;

            InventarManager.InventarUI.CameraMovement.LockCursor();
            InventarManager.MainInventarObject.transform.position = InventarManager.MainInventarChestPos.transform.position;

            CraftingUIManager.UpdateCraftingUI(Recipes);
        }
    }
}
