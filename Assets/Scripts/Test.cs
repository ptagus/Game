using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    [Header("Map")]
    public GameObject[] maps;
    public MapPoint[] mappoints;
    int[] points;
    int mappos;
    [Header("Gold")]
    public int gold;
    public int goldmining;
    public int upkeep;
    [Header("Goldfactors")]
    public int goldminingfactor;
    public int upkeepfactor;
    public int goldincreas;
    public int escortfactor;
    [Header("People")]
    public int workers;
    public int warriors;
    int freeworkers;
    [Header("Costs")]
    public int workercost;
    public int warriorcost;
    [Header("Windows")]
    public GameObject buyWindow;
    public GameObject exploreWindow;
    public GameObject mineWindow;
    public GameObject sendWorkersWindow;
    public Slider counter;
    public Slider counterworkers;
    public Text counterlabel;
    public Text counterworkerslabel;
    [Header("ResoursesText")]
    public Text goldText, workerstext, warriortext;
    int nowtype;
    int tempcost = 0, tempupkeep = 0, tempfullcost = 0, tempcount = 0, tempworkers = 0, tempmappoint = 0;
    // Start is called before the first frame update
    void Start()
    {
        freeworkers = workers;
        int count = maps.Length;
        points = new int[count];
        goldText.text = gold.ToString();
        workerstext.text = workers.ToString();
        warriortext.text = warriors.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuyWorker(int count, int cost)
    {
        workers += count;
        freeworkers += count;
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
        UpdateText();
    }

    public void LoseWorkers(int count)
    {
        workers -= count;
        UpdateText();
    }

    public void GoldIncrease(int sum)
    {
        goldincreas += sum;
        UpdateText();
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
        DenyBuy();
        UpdateText();
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

    public void UpdateText()
    {
        goldText.text = gold.ToString();
        workerstext.text = workers.ToString();
        warriortext.text = warriors.ToString();
    }

    public void ShowExploreWindow(MapPoint mp)
    {
        if (mp.onExplore)
        {
            Debug.LogError("OnExplore");
            return;
        }
        if (mp.explored)
        {
            NowMapPoint(mp.number);
            mineWindow.SetActive(true);
            counterworkers.maxValue = 5 - HowMuckWorkersOnPoint();
            return;
        }
        NowMapPoint(mp.number);
        exploreWindow.SetActive(true);
    }

    public void CloseExploreWindow()
    {
        exploreWindow.SetActive(false);
    }

    public void CloseMineWindow()
    {
        mineWindow.SetActive(false);
    }

    public void Explore()
    {
        OpenPoint(mappoints[tempmappoint]);
    }
    public void ExploreEscort()
    {
        if (gold > escortfactor)
        {
            gold -= escortfactor;
            OpenPoint(mappoints[tempmappoint]);
        }
        else
        {
            Debug.Log("Not Enogth money");
            return;
        }
    }

    public void OpenPoint(MapPoint mp)
    {
        for (int i = 0; i < mp.links.Length; i++)
        {
            points[mappos] = mp.links[i];
            mappos++;
        }
        mp.onExplore = true;
        CloseExploreWindow();
    }

    public void EndTurn()
    {
        foreach (MapPoint m in mappoints)
        {           
            if (m.workershere > 0)
            {
                m.GoldCheck(goldminingfactor);
                gold+=goldmining;
                goldmining = 0;
                UpdateText();
                m.workershere = 0;
                freeworkers = workers;
            }
            if (m.onExplore)
                m.explored = true;
            m.onExplore = false;
        }
        for (int i = 0; i < points.Length; i++)
        {
            maps[points[i]].SetActive(true);
        }
    }

    void NowMapPoint(int number)
    {
        tempmappoint = number;
    }

    public void SendWorkers()
    {  
        if (HowMuckGoldOnPoint() / 3 < 5)
        {
            counterworkers.maxValue = HowMuckGoldOnPoint() / 3;
            tempworkers = (int)counterworkers.value;
            counterworkerslabel.text = "Count: " + tempworkers.ToString();
            return;
        }
        if (freeworkers > 5 - HowMuckWorkersOnPoint())
        {
            counterworkers.maxValue = 5 - HowMuckWorkersOnPoint();
        }
        else
            counterworkers.maxValue = freeworkers;
        tempworkers = (int)counterworkers.value;
        counterworkerslabel.text = "Count: " + tempworkers.ToString();
    }

    public void ApplySend()
    {
        mappoints[tempmappoint].workershere += tempworkers;
        freeworkers -= tempworkers;
        goldmining += tempworkers * goldminingfactor;
        counterworkers.value = 0;
        tempworkers = 0;
    }

    public void DenySend()
    {
        counterworkers.value = 0;
        tempworkers = 0;
    }

    int HowMuckWorkersOnPoint() 
    {
        return mappoints[tempmappoint].workershere;
    }

    int HowMuckGoldOnPoint()
    {
        Debug.LogError(mappoints[tempmappoint].gold);
        return mappoints[tempmappoint].gold;
    }
    public void OpenSendWorkersWindow()
    {
        if (HowMuckWorkersOnPoint() == 5)
        {
            Debug.LogError("FULLhere");
            return;
        }
        sendWorkersWindow.SetActive(true);
    }

    public void CheckGoldInPoint()
    {

    }
}
