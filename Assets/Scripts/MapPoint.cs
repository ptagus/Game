using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPoint : MonoBehaviour
{
    public int[] links;
    public bool explored;
    public bool onExplore;
    public bool monsters;
    public bool escort;
    public int monsterchance;
    public int workershere;
    public int number;
    public int goldCountMin, goldCountMax;
    public int gold;
    public int workersonExplore;

    private void Awake()    
    {
        gold = HowMuchGold();
        monsters = Monstershere();
    }
    public int HowMuchGold()
    {
        return Random.Range(goldCountMin, goldCountMax) / 3 * 3;
    }

    public void GoldCheck(int goldmine)
    {
        gold -= workershere * goldmine;
    }

    public bool Monstershere()
    {
        return Random.Range(0, 100) < monsterchance;
    }
}

