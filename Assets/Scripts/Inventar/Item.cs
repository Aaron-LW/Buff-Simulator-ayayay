using UnityEngine;
using System.Collections.Generic;

public class Item
{
    public string Name;
    public GameObject Modell;
    public int MaxStackSize;
    public Sprite Sprite;
    public int UseID;
    public GameObject PlayerObjekt;

    private PlayerMovement PlayerMovementScript;

    public Item(string name, GameObject modell, Sprite sprite, int maxstacksize, int useid = 0, GameObject playerobject = null)
    {
        Name = name;
        Modell = modell;
        MaxStackSize = maxstacksize;
        Sprite = sprite;
        UseID = useid;
        PlayerObjekt = playerobject;

        if (playerobject != null)
        {
            PlayerMovementScript = playerobject.GetComponent<PlayerMovement>();
        }
    }

    public void UseItem()
    {
        switch (this.UseID)
        {
            case 1:
                if (PlayerMovementScript != null)
                {
                    PlayerMovementScript.Speed += 1f;
                }
                break;
        }
    }
}

public class ItemStack
{
    public int ID;
    public int Amount;

    public ItemStack(int id, int amount)
    {
        ID = id;
        Amount = amount;
    }
}

public class Inventar
{
    public string Name;
    public int Slots;
    public Transform Transform;
    public GameObject InventarObject;
    public List<ItemStack> Items;
    public int ID;

    public Inventar(string name, int slots, Transform transform, GameObject inventarObject, List<ItemStack> items, int id)
    {
        Name = name;
        Slots = slots;
        Transform = transform;
        InventarObject = inventarObject;
        Items = items;
        ID = id;
    }
}

public class Waffe : Item
{
    public float Damage;
    public int Ammo;
    public float FireRate;
    public int MaxAmmoInMag;
    public float RecoilMultiplier;
    public float Range;
    public float Knockback;
    public int AmmoInInv;
    public ParticleSystem Particle;
    public GameObject ParticlePosition;

    public Waffe(string name, GameObject modell, Sprite sprite, int maxstacksize, float damage, float firerate, int maxammoinmag, int ammoininv, float recoilmultiplier, float range, float knockback, ParticleSystem particle, GameObject particletransform) : base(name, modell, sprite, maxstacksize)
    {
        Damage = damage;
        Ammo = maxammoinmag;
        FireRate = firerate;
        MaxAmmoInMag = maxammoinmag;
        RecoilMultiplier = recoilmultiplier;
        Range = range;
        Knockback = knockback;
        AmmoInInv = ammoininv;
        Particle = particle;
        ParticlePosition = particletransform;
    }

    public void Reload()
    {
        int AddAmmo = this.MaxAmmoInMag - this.Ammo;

        if (this.AmmoInInv - AddAmmo < 0)
        {
            AddAmmo = this.AmmoInInv;
        }

        this.Ammo += AddAmmo;
        this.AmmoInInv -= AddAmmo;
    }
}

public class Tool : Item
{
    public float MiningSpeed;
    public int MiningDamage;
    public float MiningDistance;

    public Tool(string name, GameObject modell, Sprite sprite, int maxstacksize, float miningspeed, int miningdamage, float miningdistance) : base(name, modell, sprite, maxstacksize)
    {
        MiningSpeed = miningspeed;
        MiningDamage = miningdamage;
        MiningDistance = miningdistance;
    }
}

public class Potion : Item
{
    public int PotionID;
    public int Duration;
    public int Level;
    public BuffManager BuffManager;

    public Potion(string name, GameObject modell, Sprite sprite, int maxstacksize, int potionid, BuffManager buffManager, int duration, int level) : base(name, modell, sprite, maxstacksize)
    {
        PotionID = potionid;
        BuffManager = buffManager;
        Duration = duration;
        Level = level;
    }

    public void UsePotion()
    {
        BuffManager.ApplyBuff(this.PotionID, this.Duration, this.Level);
    }
}

