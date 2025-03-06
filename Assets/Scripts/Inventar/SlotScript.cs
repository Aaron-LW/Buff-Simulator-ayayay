using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotScript : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public int SlotIndex = 999;
    public int InventarIndex = 999;
    public bool Exisitert = false;

    Outline outline;

    public bool MouseOver = false;

    public InventarManager InventarManagerScript;

    void Start()
    {
        GameObject InventarManager = GameObject.Find("InventarManager");
        InventarManagerScript = InventarManager.GetComponent<InventarManager>();

        outline = GetComponent<Outline>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (InventarIndex != 999)
        {
            if (InventarManagerScript.ChestOpen)
            {
                if (InventarIndex != InventarManagerScript.OpenChestIndex)
                {
                    InventarManagerScript.SwitchItem(InventarManagerScript.Inventare[InventarManagerScript.OpenChestIndex], InventarManagerScript.Inventare[InventarIndex], InventarManagerScript.Inventare[InventarIndex].Items[SlotIndex]);
                    return;
                }
                else
                {
                    InventarManagerScript.SwitchItem(InventarManagerScript.Inventare[0], InventarManagerScript.Inventare[InventarManagerScript.OpenChestIndex], InventarManagerScript.Inventare[InventarManagerScript.OpenChestIndex].Items[SlotIndex]);
                    return;
                }
            }
            else
            {
                if (InventarIndex == 0)
                {
                    InventarManagerScript.SwitchItem(InventarManagerScript.Inventare[1], InventarManagerScript.Inventare[InventarIndex], InventarManagerScript.Inventare[InventarIndex].Items[SlotIndex]);
                    return;
                }

                if (InventarIndex == 1)
                {
                    InventarManagerScript.SwitchItem(InventarManagerScript.Inventare[0], InventarManagerScript.Inventare[InventarIndex], InventarManagerScript.Inventare[InventarIndex].Items[SlotIndex]);
                    return;
                }
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        outline.enabled = true;
        MouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        outline.enabled = false;
        MouseOver = false;
    }
}
