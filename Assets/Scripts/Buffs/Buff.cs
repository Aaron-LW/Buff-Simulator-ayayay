using UnityEngine;

public class Buff
{
    public int ID;
    public int StartDuration;
    public int Level;
    public int Duration;

    public Buff(int id, int startduration = 60, int level = 1)
    {
        ID = id;
        StartDuration = startduration;
        Level = level;
        Duration = startduration;
    }
}
