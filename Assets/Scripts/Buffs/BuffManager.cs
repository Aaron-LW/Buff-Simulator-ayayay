using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class BuffManager : MonoBehaviour
{
    [Header("Buffeffektstärken")]
    public List<int> EffektStärken = new List<int>();

    [Header("Maximale Stärken")]
    public List<int> MaxStärken = new List<int>();

    [Header("Sprites")]
    public List<Sprite> Sprites = new List<Sprite>();

    List<Buff> ActiveBuffs = new List<Buff>();
    List<int> RemoveList = new List<int>();

    [Space(15)]
    public GameObject Player;
    public TMP_Text Debugtext;
    public GameObject BuffSlot;
    public Transform SlotParent;

    private PlayerMovement PlayerMovement;

    private void Awake()
    {
        PlayerMovement = Player.GetComponent<PlayerMovement>();
    }

    private void Update()
    {
    }

    public void ApplyBuff(int BuffID, int Duration, int Level)
    {
        foreach (Buff Buff in ActiveBuffs)
        {
            if (Buff.ID == BuffID)
            {
                int LevelsToAdd = CalculateLevel(Buff.ID, Buff.Level, Level);
                
                Buff.Level += LevelsToAdd;
                Buff.Duration = Buff.StartDuration;

                GameObject[] Slots = GameObject.FindGameObjectsWithTag("BuffSlot");

                foreach (GameObject Slot in Slots)
                {
                    BuffUI Skript = Slot.GetComponent<BuffUI>();

                    if (Skript.ID == BuffID)
                    {
                        Skript.Duration = Buff.StartDuration;
                        Skript.Strength += LevelsToAdd;
                        Skript.UpdateText();
                        break;
                    }
                }

                ApplyBuffEffect(BuffID, LevelsToAdd);
                return;
            }
        }

        ActiveBuffs.Add(new Buff(BuffID, Duration, Level));
        ApplyBuffEffect(BuffID, Level);

        GameObject InstantiatedSlot = Instantiate(BuffSlot, SlotParent);
        BuffUI SlotScript = InstantiatedSlot.GetComponent<BuffUI>();

        SlotScript.Duration = Duration;
        SlotScript.Strength = Level;
        SlotScript.ID = BuffID;
        if (Sprites[BuffID] != null) SlotScript.Sprite = Sprites[BuffID];
        else
        {
            SlotScript.Sprite = null;
            SlotScript.Image.color = Color.clear;
        }
        SlotScript.BuffManager = GetComponent<BuffManager>();
    }

    public void RemoveBuff(int BuffID)
    {
        foreach (Buff Buff in ActiveBuffs)
        {
            if (Buff.ID == BuffID)
            {
                ActiveBuffs.Remove(Buff);

                ApplyBuffEffect(Buff.ID, Buff.Level, true);
                break;
            }
        }
    }

    void ApplyBuffEffect(int ID, int Level, bool Remove = false)
    {
        if (!Remove)
        {
            switch (ID)
            {
                case 0:
                    PlayerMovement.Speed += EffektStärken[ID] * Level;
                    break;
                case 1:
                    PlayerMovement.Speed += EffektStärken[ID] * Level;
                    break;
            }

        }
        else
        {
            switch (ID)
            {
                case 0:
                    PlayerMovement.Speed -= EffektStärken[ID] * Level;
                    break;
                case 1:
                    PlayerMovement.Speed -= EffektStärken[ID] * Level;
                    break;
            }
        }
    }

    int CalculateLevel(int BuffID, int CurrentLevel, int LevelsToAdd)
    {
        if (CurrentLevel + LevelsToAdd < MaxStärken[BuffID])
        {
            return LevelsToAdd;
        }
        else
        {
            return MaxStärken[BuffID] - CurrentLevel;
        }
    }
}