using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventarUI : MonoBehaviour
{
    //[HideInInspector] public GameObject InventarManagerObject;
    public InventarManager InventarManager;

    public GameObject Slot;

    public CameraMovement CameraMovement;

    [HideInInspector] public bool InventarOpen = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (InventarManager.MainInventarObject.gameObject.activeSelf == true)
            {
                CameraMovement.UnlockCursor();
                InventarOpen = false;
            }
            if (InventarManager.MainInventarObject.gameObject.activeSelf == false && !InventarManager.ChestOpen && !InventarManager.CraftingTableOpen)
            {
                CameraMovement.LockCursor();
                InventarOpen = true;
            }

            UpdateInventory(InventarManager.Inventare[0]);
            UpdateInventory(InventarManager.Inventare[1]);
            InventarManager.MainInventarObject.SetActive(!InventarManager.MainInventarObject.gameObject.activeSelf);
        }
    }

    public void UpdateInventory(Inventar Inventar)
    {
        bool InventarActive =  Inventar.InventarObject.activeSelf;
        List<int> KeysToDestroy = new List<int>();

        Inventar.InventarObject.SetActive(true);

        if (InventarActive == false)
        {
            Inventar.InventarObject.SetActive(false);
        }

        List<Transform> childrenToDestroy = new List<Transform>();

        foreach (Transform child in Inventar.Transform)
        {
            if (child.CompareTag("InventarSlots"))
            {
                childrenToDestroy.Add(child);
            }
        }

        foreach (Transform child in childrenToDestroy)
        {
            Destroy(child.gameObject);
        }

        int counter = 0;

        foreach (ItemStack Stack in Inventar.Items)
        {
            if (Stack.Amount > 0)
            {
                GameObject InstantiatedObject = Instantiate(Slot, Inventar.Transform);

                InstantiatedObject.gameObject.transform.localScale = new Vector3(InstantiatedObject.gameObject.transform.localScale.x, InstantiatedObject.gameObject.transform.localScale.y, InstantiatedObject.gameObject.transform.localScale.z);
                SlotScript slotScript = InstantiatedObject.GetComponentInChildren<SlotScript>();

                slotScript.SlotIndex = counter;
                slotScript.InventarIndex = Inventar.ID;
                slotScript.Exisitert = true;

                Image[] img = InstantiatedObject.GetComponentsInChildren<Image>();

                if (InventarManager.InitialisedItems[Stack.ID].Sprite != null)
                {
                    img[1].sprite = InventarManager.InitialisedItems[Stack.ID].Sprite;
                }
                else
                {
                    img[1].color = Color.clear;
                }

                TMP_Text TextKomponente = InstantiatedObject.GetComponentInChildren<TMP_Text>();
                Outline outline = InstantiatedObject.GetComponent<Outline>();


                if (Stack.Amount > 1)
                {
                    TextKomponente.text = Stack.Amount.ToString();
                }
                else
                {
                    TextKomponente.text = "";
                }

                if (InventarManager.CurrentHotbarSlot == counter && InventarOpen == false && Inventar.ID == 1)
                {
                    outline.enabled = true;
                }
                else
                {
                    outline.enabled = false;
                }
            }
            else
            {
                KeysToDestroy.Add(counter);
            }

            counter++;
        }

        foreach (int i in KeysToDestroy)
        {
            Inventar.Items.RemoveAt(i);
        }

        for (int i = Inventar.Slots - Inventar.Items.Count; i > 0; i--)
        {
            GameObject InstantiatedObject = Instantiate(Slot, Inventar.Transform);

            Outline outline = InstantiatedObject.GetComponent<Outline>();
            if (InventarManager.CurrentHotbarSlot == counter && InventarOpen == false && Inventar == InventarManager.Inventare[1])
            {
                outline.enabled = true;
            }
            else
            {
                outline.enabled = false;
            }

            counter++;

            foreach (Transform child in InstantiatedObject.transform)
            {
                child.gameObject.SetActive(false);
            }
        }
    }
}
