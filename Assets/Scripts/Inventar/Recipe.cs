using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Recipe
{
    public int ID;
    public int ResultID;
    public int ResultAmount;

    public Dictionary<int, int> RequiredItems = new Dictionary<int, int>();

    public Recipe(int id, Dictionary<int, int> requireditems, int resultID, int resultAmount)
    {
        ID = id;
        RequiredItems = requireditems;
        ResultID = resultID;
        ResultAmount = resultAmount;
    }
}
