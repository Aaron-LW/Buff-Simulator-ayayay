using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class CraftingUIManager : MonoBehaviour
{
    public InventarManager InventarManager;
    public CraftingManager CraftingManager;

    public Transform CraftingPanelTransform;
    public GameObject CraftingRecipe;
    public GameObject CraftingSlot;
    public GameObject Arrow;

    public void UpdateCraftingUI(List<Recipe> Recipes)
    {
        List<Transform> childrenToDestroy = new List<Transform>();

        foreach (Transform child in CraftingPanelTransform)
        {
            if (child.CompareTag("CraftingSlot"))
            {
                childrenToDestroy.Add(child);
            }
        }

        foreach (Transform child in childrenToDestroy)
        {
            Destroy(child.gameObject);
        }

        foreach (Recipe Recipe in Recipes)
        {
            GameObject InstantiatedObject = Instantiate(CraftingRecipe, CraftingPanelTransform);
            CraftingSlotScript CraftingSlotScript = InstantiatedObject.GetComponent<CraftingSlotScript>();

            CraftingSlotScript.CraftingID = Recipe.ID;
            CraftingSlotScript.CraftingManager = CraftingManager;

            foreach (KeyValuePair<int, int> Ingredient in Recipe.RequiredItems)
            {
                GameObject InstantiatedIngredient = Instantiate(CraftingSlot, InstantiatedObject.transform);

                Image[] img = InstantiatedIngredient.GetComponentsInChildren<Image>();
                if (InventarManager.InitialisedItems[Ingredient.Key].Sprite != null)
                {
                    img[1].sprite = InventarManager.InitialisedItems[Ingredient.Key].Sprite;
                }
                else
                {
                    img[1].color = Color.clear;
                }

                foreach (Transform child in InstantiatedIngredient.transform)
                {
                    if (child.tag.Equals("CraftingAmountText"))
                    {
                        child.GetComponent<TMP_Text>().text = Ingredient.Value.ToString();
                        break;
                    }
                }
            }

            Instantiate(Arrow, InstantiatedObject.transform);

            GameObject InstantiatedResult = Instantiate(CraftingSlot, InstantiatedObject.transform);

            Image[] Resultimg = InstantiatedResult.GetComponentsInChildren<Image>();
            Resultimg[1].sprite = InventarManager.InitialisedItems[Recipe.ResultID].Sprite;

            foreach (Transform child in InstantiatedResult.transform)
            {
                if (child.tag.Equals("CraftingAmountText"))
                {
                    child.GetComponent<TMP_Text>().text = Recipe.ResultAmount.ToString();
                    break;
                }
            }
        }
    }
}
