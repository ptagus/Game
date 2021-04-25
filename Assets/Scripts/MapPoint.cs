using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPoint : MonoBehaviour
{
    public int[] links;
    public bool explored;
    public bool onExplore;
    public int workershere;
    public int number;
    public int goldCountMin, goldCountMax;
    public int gold;

    private void Awake()    
    {
        gold = HowMuchGold();
    }
    public int HowMuchGold()
    {
        return Random.Range(goldCountMin, goldCountMax) / 3 * 3;
    }

    public void GoldCheck(int goldmine)
    {
        gold -= workershere * goldmine;
    }
}

