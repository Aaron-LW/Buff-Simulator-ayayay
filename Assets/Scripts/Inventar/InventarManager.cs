using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.Properties;

public class InventarManager : MonoBehaviour
{
    // Ich weiﬂ nicht wie ich es hinbekommen habe aber es funktioniert

    public List<Item> InitialisedItems = new List<Item>();
    public List<Inventar> Inventare = new List<Inventar>();

    public GameObject UIManager;
    [HideInInspector] public InventarUI InventarUI;

    public float InteractionDistance = 3f;
    public LayerMask ChestMask;
    public LayerMask CraftingLayer;

    private ChestScript ChestScript;
    private CraftingManager CraftingManager;
    private RaycastHit CurrentOpenChestRayCastHit;
    private RaycastHit CurrentOpenCraftingTableRayCastHit;

    [Header("InventarGameObjects")]
    public GameObject MainInventarBasePos;
    public GameObject MainInventarChestPos;
    public GameObject MainInventarObject;
    public GameObject HotbarObject;

    [Header("Transforms")]
    public Transform MainInventarTransform;
    public Transform HotbarTransform;

    [Header("Sprites")]
    public Sprite PotionSprite;
    public Sprite GlassSprite;

    [Header("Item GameObjects")]
    public GameObject SpeedPotionModell;

    [HideInInspector] public int CurrentHotbarSlot = 0;

    [Header("Andere Referenzen")]
    public GameObject PlayerObjekt;
    public GameObject CameraObject;
    public BuffManager BuffManager;

    [HideInInspector] public bool DraggingObject = false;
    [HideInInspector] public int DraggedObjectID = 999;
    [HideInInspector] public int DraggedObjectSlotIndex = 999;
    [HideInInspector] public int DraggedObjectInventoryIndex = 999;

    [HideInInspector] public bool ChestOpen = false;
    [HideInInspector] public int OpenChestIndex = 9999;

    [HideInInspector] public bool CraftingTableOpen = false;

    private void Awake()
    {
        InventarUI = UIManager.GetComponent<InventarUI>();

        Inventare.Add(new Inventar("MainInventar", 21, MainInventarTransform, MainInventarObject, new List<ItemStack>(), 0));
        Inventare.Add(new Inventar("Hotbar", 7, HotbarTransform, HotbarObject, new List<ItemStack>(), 1));

        /* 0: SpeedPotion */ InitialisedItems.Add(new Potion("Speed Potion", SpeedPotionModell, PotionSprite, 16, 0, BuffManager, 60, 1));
        /* 1: Glas */ InitialisedItems.Add(new Item("Glas", null, GlassSprite, 64));
        /* 2: Geschwindigkeitsseelen */ InitialisedItems.Add(new Item("Geschwindigkeitsseelen", null, null, 4));

        AddItem(1, 64, Inventare[0]);
        AddItem(2, 16, Inventare[0]);
        AddItem(0, 20, Inventare[0]);

        InventarUI.UpdateInventory(Inventare[0]);
        InventarUI.UpdateInventory(Inventare[1]);
    }

    private void Update()
    {
        if (Input.mouseScrollDelta.y < 0f && CurrentHotbarSlot < Inventare[1].Slots - 1 && !InventarUI.InventarOpen) { CurrentHotbarSlot++; InventarUI.UpdateInventory(Inventare[1]); }
        if (Input.mouseScrollDelta.y > 0f && CurrentHotbarSlot > 0 && !InventarUI.InventarOpen) { CurrentHotbarSlot--; InventarUI.UpdateInventory(Inventare[1]); }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Interaction();
        }
    }

    public void AddItem(int ID, int Amount, Inventar Inventar, int SlotIndex = 9999)
    {
        bool ItemFound = false;
        int LeftoverAmount = 0;

        if (SlotIndex == 9999)
        {
            SlotIndex = Inventar.Items.Count - 1;
        }
        else
        {
            Inventar.Items.Insert(SlotIndex, new ItemStack(ID, Amount));
            return;
        }

        foreach (var item in Inventar.Items)
        {
            if (item.ID == ID)
            {
                if (item.Amount + Amount <= InitialisedItems[ID].MaxStackSize)
                {
                    item.Amount += Amount;
                    ItemFound = true;
                    break;
                }
                else
                {
                    LeftoverAmount = item.Amount + Amount - InitialisedItems[ID].MaxStackSize;
                    item.Amount += Amount - LeftoverAmount;
                }
            }
        }

        if (!ItemFound)
        {
            if (Inventar.Items.Count < Inventar.Slots)
            {
                if (LeftoverAmount == 0)
                {
                    Inventar.Items.Add(new ItemStack(ID, Amount));
                }
                else
                {
                    Inventar.Items.Add(new ItemStack(ID, LeftoverAmount));
                }
            }
            else if (LeftoverAmount > 0)
            {
            }
        }
    }

    public void RemoveItem(int ID, int Amount, Inventar Inventar, bool reverse = true)
    {
        List<ItemStack> RemoveList = Inventar.Items;
        List<int> indexToRemove = new List<int>();

        if (reverse)
        {
            RemoveList.Reverse();
        }

        int SubtractAmount = Amount;
        int counter = 0;

        foreach (var Item in RemoveList)
        {
            if (Item.ID == ID)
            {
                if (Amount - Item.Amount > 0)
                {
                    SubtractAmount -= Item.Amount;
                    indexToRemove.Add(counter);
                }
                else
                {
                    Item.Amount -= SubtractAmount;
                    break;
                }
            }
            counter++;
        }

        foreach (int i in indexToRemove)
        {
            RemoveList.RemoveAt(i);
        }

        if (reverse)
        {
            RemoveList.Reverse();
        }

        Inventar.Items = RemoveList;
        InventarUI.UpdateInventory(Inventar);
    }

    public void SwitchItem(Inventar ZielInventar, Inventar OriginalInventar, ItemStack Item)
    {

        if (ZielInventar.Items.Count != ZielInventar.Slots)
        {
            AddItem(Item.ID, Item.Amount, ZielInventar);
            RemoveItem(Item.ID, Item.Amount, OriginalInventar, true);

            InventarUI.UpdateInventory(OriginalInventar);
            InventarUI.UpdateInventory(ZielInventar);
        }
        return;
    }

    public void Interaction()
    {
        if (ChestOpen)
        {
            ChestScript.CloseChest();
            StopCoroutine(CheckDistanceToInteracted(CurrentOpenChestRayCastHit.transform, ChestScript));

            return;
        }

        if (CraftingTableOpen)
        {
            CraftingManager.AcessCraftingTable();
            StopCoroutine(CheckDistanceToInteracted(CraftingManager.transform, null, CraftingManager));

            return;
        }

        if (Physics.Raycast(CameraObject.transform.position, CameraObject.transform.forward, out CurrentOpenChestRayCastHit, InteractionDistance, ChestMask))
        {
            ChestScript = CurrentOpenChestRayCastHit.collider.gameObject.GetComponent<ChestScript>();

            if (ChestScript != null && !ChestOpen)
            {
                ChestScript.OpenChest();
                StartCoroutine(CheckDistanceToInteracted(CurrentOpenChestRayCastHit.transform, ChestScript));
                return;
            }
            return;
        }

        if (Physics.Raycast(CameraObject.transform.position, CameraObject.transform.forward, out CurrentOpenCraftingTableRayCastHit, InteractionDistance, CraftingLayer))
        {
            CraftingManager = CurrentOpenCraftingTableRayCastHit.transform.GetComponent<CraftingManager>();

            if (CraftingManager != null)
            {
                CraftingManager.AcessCraftingTable();
                StartCoroutine(CheckDistanceToInteracted(CraftingManager.transform, null, CraftingManager));

                return;
            }
            return;
        }
    }

    private IEnumerator CheckDistanceToInteracted(Transform InteractedTransform, ChestScript chestScript = null, CraftingManager CraftingManager = null)
    {
        while (true)
        {
            float distance = Vector3.Distance(CameraObject.transform.position, InteractedTransform.position);

            if (distance > 7f)
            {
                if (ChestScript != null)
                {
                    ChestScript.CloseChest();
                }
                else if (CraftingManager != null)
                {
                    if (CraftingTableOpen)
                    {
                        CraftingManager.AcessCraftingTable();
                    }
                }

                yield break;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
}