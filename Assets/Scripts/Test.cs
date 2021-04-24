using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    [Header("Gold")]
    public int gold;
    public int goldmining;
    public int upkeep;
    [Header("Goldfactors")]
    public int goldminingfactor;
    public int upkeepfactor;
    public int goldincreas;
    [Header("People")]
    public int workers;
    public int warriors;
    [Header("Costs")]
    public int workercost;
    public int warriorcost;
    [Header("Windows")]
    public GameObject buyWindow;
    public Slider counter;
    public Text counterlabel;
    int nowtype;
    int tempcost = 0, tempupkeep = 0, tempfullcost = 0, tempcount = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuyWorker(int count, int cost)
    {
        workers += count;
        gold -= cost;
    }

    public void BuyWarrior(int count, int cost, int upkeeps)
    {
        workers += count;
        gold -= cost;
        upkeep += upkeeps;
        GoldIncrease(-upkeeps);
    }

    public void MineGold(int count)
    {
        goldmining += count * goldminingfactor;
        int newgoldmine = count * goldminingfactor;
        GoldIncrease(newgoldmine);
    }

    public void LoseWarriors(int count)
    {
        warriors -= count;
        upkeep -= count * upkeepfactor;
    }

    public void GoldIncrease(int sum)
    {
        goldincreas += sum;
    }

    public void BuyWindow(int type)
    {
        nowtype = type;
        buyWindow.SetActive(true);
        if (type == 1)
        {
            counter.maxValue = (int)gold / workercost;
            counterlabel.text = "count : 0";
        }
        if (type == 2)
        {
            counter.maxValue = (int)gold / warriorcost;
            counterlabel.text = "count : 0";
        }
    }

    public void Buying()
    {        
        if (nowtype == 1)
        {
            tempcost = workercost;
        }
        if (nowtype == 2)
        {
            tempcost = warriorcost;
        }
        tempcount = (int)counter.value;
        tempfullcost = tempcount * tempcost;
        if (nowtype == 2)
        {
            tempupkeep = tempcount * upkeepfactor;
        }
        counterlabel.text = "Count: " + tempcount.ToString() + "\nGold: " + tempfullcost.ToString() +"\nUpkeep: " + tempupkeep;
    }

    public void ApplyBuy()
    {
        if (nowtype == 1)
        {
            BuyWorker(tempcount, tempfullcost);
        }
        if (nowtype == 2)
        {
            BuyWarrior(tempcount, tempfullcost, tempupkeep);
        }
        counter.value = 0;
        buyWindow.SetActive(false);
    }

    public void DenyBuy()
    {
        tempcost = 0;
        tempupkeep = 0; 
        tempfullcost = 0; 
        tempcount = 0;
        counter.value = 0;
        buyWindow.SetActive(false);
    }
}
