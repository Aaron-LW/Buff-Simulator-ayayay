using UnityEngine;

public class CraftingTableSpawner : MonoBehaviour
{
    public GameObject CraftingTable;
    public GameObject Player;

    private CameraMovement cameraMovement;

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 20f))
            {
                Instantiate(CraftingTable, new Vector3(hit.point.x, hit.point.y + CraftingTable.transform.localScale.y / 2, hit.point.z), Quaternion.LookRotation(transform.right));
            }
        } 
    }
}
