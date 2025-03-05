using UnityEngine;
using System.Collections.Generic;

public class ChestScript : MonoBehaviour
{
    [SerializeField] public GameObject InventarManagerObject;
    private InventarManager InventarManager;

    [HideInInspector] public int InventarIndex;

    public Transform InventarTransform;
    public GameObject InventarObject;

    public List<ItemStack> AddItemQueue = new List<ItemStack>();

    private void Start()
    {
        InventarManager = InventarManagerObject.GetComponent<InventarManager>();

        InventarIndex = InventarManager.Inventare.Count;
        InventarManager.Inventare.Add(new Inventar("ChestInventar", 15, InventarTransform, InventarObject, new List<ItemStack>(), InventarIndex));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && InventarManager.ChestOpen)
        {
            CloseChest();
        }

        if (AddItemQueue.Count > 0)
        {
            InventarManager.AddItem(AddItemQueue[0].ID, AddItemQueue[0].Amount, InventarManager.Inventare[InventarIndex]);
            AddItemQueue.RemoveAt(0);
        }
    }

    public void CloseChest()
    {
        InventarManager.MainInventarObject.transform.position = InventarManager.MainInventarBasePos.transform.position;
        InventarManager.ChestOpen = false;
        InventarObject.SetActive(false);
        InventarManager.MainInventarObject.SetActive(false);
        InventarManager.InventarUI.InventarOpen = false;
        InventarManager.OpenChestIndex = 9999;
        InventarManager.InventarUI.CameraMovement.UnlockCursor();
    }

    public void OpenChest()
    {
        InventarManager.MainInventarObject.transform.position = InventarManager.MainInventarChestPos.transform.position;
        InventarManager.MainInventarObject.SetActive(true);
        InventarManager.InventarUI.InventarOpen = true;
        InventarManager.ChestOpen = true;
        InventarObject.SetActive(true);
        InventarManager.InventarUI.CameraMovement.LockCursor();

        InventarManager.OpenChestIndex = InventarIndex;
        InventarManager.InventarUI.UpdateInventory(InventarManager.Inventare[InventarIndex]);
    }
}
