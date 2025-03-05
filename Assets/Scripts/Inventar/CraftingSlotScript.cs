using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CraftingSlotScript : MonoBehaviour, IPointerClickHandler
{
    public int CraftingID;
    public CraftingManager CraftingManager;

    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        CraftingManager.Craft(CraftingID);
    }
}
