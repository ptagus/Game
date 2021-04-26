using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public Sprite  monstersimg, goldsimg, closeimg;

    private void Awake()    
    {
        gold = HowMuchGold();
        monsters = Monstershere();
    }
    public int HowMuchGold()
    {
        return Random.Range(goldCountMin, goldCountMax) / 3 * 3;
    }

    public bool NoGoldOnNextTurn()
    {
        if (workershere * 3 == gold)
            return true;
        return false;
    }

    public void GoldCheck(int goldmine)
    {
        gold -= workershere * goldmine;
        if (gold == 0)
        {
            GetComponent<Image>().sprite = closeimg;
        }
    }

    public bool Monstershere()
    {
        return Random.Range(0, 100) < monsterchance;
    }

    public void ExploreMine()
    {
        if (!escort && monsters)
        {
            GetComponent<Image>().sprite = monstersimg;
            return;
        }
        if (explored)
        {
            GetComponent<Image>().sprite = goldsimg;
        }
        if (gold == 0)
        {
            GetComponent<Image>().sprite = closeimg;
        }
    }
}

