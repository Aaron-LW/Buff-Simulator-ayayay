using UnityEngine;
using System.Collections.Generic;

public class MainWorldGen : MonoBehaviour
{
    public bool DebugMode = false;

    public GameObject Building;
    public GameObject Ground;
    public GameObject Chest;

    public InventarManager InventarManager;

    public float BuildingPuffer = 10f;
    public float ChestPuffer = 0.5f;

    [Range(0, 30)] public float ChestRange;
    [Range(0, 5)] public int ChestAmount;

    private float WorldSize;
    public float Chance = 1f;

    private List<GameObject> InstantiatedBuildings = new List<GameObject>();

    public GameObject InventarManagerObjekt;
    public Transform ChestInventar;
    public GameObject InventarObjekt;

    private void Awake()
    {
        Collider GroundCollider = Ground.GetComponent<Collider>();
        WorldSize = GroundCollider.bounds.extents.x - Building.transform.localScale.x / 2;
    }

    private void Start()
    {
        GenerateBuildings();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.T))
        {
            GenerateBuildings();
        }
    }

    void GenerateBuildings()
    {
        if (InstantiatedBuildings.Count > 0)
        {
            foreach (GameObject building in InstantiatedBuildings)
            {
                Destroy(building.gameObject);
            }

            InstantiatedBuildings.Clear();
        }

        for (float x = -(WorldSize); x < WorldSize - Building.transform.localScale.x / 2;)
        {
            for (float z = -(WorldSize); z < WorldSize - Building.transform.localScale.x / 2; z += Building.transform.localScale.z + BuildingPuffer)
            {
                float zufall = Random.Range(1f, 100f);
                zufall = Mathf.Round(zufall);

                if (zufall <= Chance)
                {
                    GameObject InstantiatedBuilding = Instantiate(Building, new Vector3(x, 0 + Building.transform.localScale.y / 2, z), Quaternion.identity);
                    InstantiatedBuildings.Add(InstantiatedBuilding);

                    if (Random.Range(0, 2) == 1)
                    {
                        GenerateChests(x, z, Random.Range(1, ChestAmount));
                    }

                    z += Building.transform.localScale.z + BuildingPuffer;

                    if (Random.Range(1, 3) == 1)
                    {
                        z += BuildingPuffer;

                        if (Random.Range(0, 2) == 1)
                        {
                            GenerateChests(x, z, Random.Range(1, ChestAmount));
                        }

                        InstantiatedBuilding = Instantiate(Building, new Vector3(x, 0 + Building.transform.localScale.y / 2, z), Quaternion.identity);

                        if (DebugMode)
                        {
                            Renderer Renderer = InstantiatedBuilding.GetComponent<Renderer>();
                            Renderer.material.color = Color.green;
                        }

                        InstantiatedBuildings.Add(InstantiatedBuilding);

                        if (Random.Range(1, 3) == 1)
                        {
                            if (Random.Range(1, 2) == 1)
                            {
                                InstantiatedBuilding = Instantiate(Building, new Vector3(x + Building.transform.localScale.x + BuildingPuffer, 0 + Building.transform.localScale.y / 2, z), Quaternion.identity);
                                
                                if (Random.Range(0, 2) == 1)
                                {
                                    GenerateChests(x + Building.transform.localScale.x + BuildingPuffer, z, Random.Range(1, ChestAmount));
                                }
                            }
                            else
                            {
                                InstantiatedBuilding = Instantiate(Building, new Vector3(x - Building.transform.localScale.x + BuildingPuffer, 0 + Building.transform.localScale.y / 2, z), Quaternion.identity);

                                if (Random.Range(0, 2) == 1)
                                {
                                    GenerateChests(x - Building.transform.localScale.x + BuildingPuffer, z, Random.Range(1, ChestAmount));
                                }
                            }

                            if (DebugMode)
                            {
                                Renderer Renderer2 = InstantiatedBuilding.GetComponent<Renderer>();
                                Renderer2.material.color = Color.red;
                            }

                            InstantiatedBuildings.Add(InstantiatedBuilding);
                        }
                    }
                }
            }
            x += Building.transform.localScale.x + BuildingPuffer;
        }

        void GenerateChests(float x, float z, int Amount)
        {
            if (Mathf.Round(Random.Range(0, 2)) >= 1)
            {
                if (Mathf.Round(Random.Range(0, 2)) == 1)
                {
                    for (int i = 0; i < Amount; i++)
                    {
                        GameObject InstantiatedChestXPositive = Instantiate(Chest, new Vector3(x + Building.transform.localScale.x / 2 + Chest.transform.localScale.x / 2 + ChestPuffer, 0 + Chest.transform.localScale.y / 2, z + Random.Range(-ChestRange, ChestRange)), Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up));
                        InstantiatedBuildings.Add(InstantiatedChestXPositive);

                        ChestScript chestScript = InstantiatedChestXPositive.GetComponent<ChestScript>();

                        chestScript.InventarObject = InventarObjekt;
                        chestScript.InventarTransform = ChestInventar;
                        chestScript.InventarManagerObject = InventarManagerObjekt;

                        GenerateChestLoot(chestScript);
                    }
                    return;
                }
                else
                {
                    for (int i = 0; i < Amount; i++)
                    {
                        GameObject InstantiatedChestXNegative = Instantiate(Chest, new Vector3(x + -(Building.transform.localScale.x / 2) + Chest.transform.localScale.x / 2 + -ChestPuffer, 0 + Chest.transform.localScale.y / 2, z + Random.Range(-ChestRange, ChestRange)), Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up));
                        InstantiatedBuildings.Add(InstantiatedChestXNegative);

                        ChestScript chestScript = InstantiatedChestXNegative.GetComponent<ChestScript>();

                        chestScript.InventarObject = InventarObjekt;
                        chestScript.InventarTransform = ChestInventar;
                        chestScript.InventarManagerObject = InventarManagerObjekt;

                        GenerateChestLoot(chestScript);
                    }
                    return;
                }
            }
            else
            {
                if (Mathf.Round(Random.Range(0, 2)) == 1)
                {
                    for (int i = 0; i < Amount; i++)
                    {
                        GameObject InstantiatedChestZPositive = Instantiate(Chest, new Vector3(x + Random.Range(-ChestRange, ChestRange), 0 + Chest.transform.localScale.y / 2, z + Building.transform.localScale.z / 2 + Chest.transform.localScale.z / 2 + ChestPuffer), Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up));
                        InstantiatedBuildings.Add(InstantiatedChestZPositive);

                        ChestScript chestScript = InstantiatedChestZPositive.GetComponent<ChestScript>();

                        chestScript.InventarObject = InventarObjekt;
                        chestScript.InventarTransform = ChestInventar;
                        chestScript.InventarManagerObject = InventarManagerObjekt;

                        GenerateChestLoot(chestScript);
                    }
                    return;
                }
                else
                {
                    for (int i = 0; i < Amount; i++)
                    {
                        GameObject InstantiatedChestZNegative = Instantiate(Chest, new Vector3(x + Random.Range(-ChestRange, ChestRange), 0 + Chest.transform.localScale.y / 2, z + -(Building.transform.localScale.z / 2) + Chest.transform.localScale.z / 2 + -ChestPuffer), Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up));
                        InstantiatedBuildings.Add(InstantiatedChestZNegative);

                        ChestScript chestScript = InstantiatedChestZNegative.GetComponent<ChestScript>();

                        chestScript.InventarObject = InventarObjekt;
                        chestScript.InventarTransform = ChestInventar;
                        chestScript.InventarManagerObject = InventarManagerObjekt;

                        GenerateChestLoot(chestScript);
                    }
                    return;
                }
            }
        }
    }

    void GenerateChestLoot(ChestScript ChestScript)
    {
        int AmountItems = Random.Range(1, 10);

        for (int i = 0;  i < AmountItems; i++)
        {
            int ID = Mathf.RoundToInt(Random.Range(1, 4));
            int Amount = Mathf.RoundToInt(Random.Range(1, 30));

            ChestScript.AddItemQueue.Add(new ItemStack(ID, Amount));
        }
    }
}
