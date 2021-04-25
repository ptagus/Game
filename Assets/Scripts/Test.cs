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
    public int warriorslost;
    public int workerlost;
    public int warriorsEscrot = 5;
    public int workersExplore = 5;
    int lostwarriorsontern;
    int lostworkersonturn;
    int freeworkers;
    int freewarriors;
    [Header("Costs")]
    public int workercost;
    public int warriorcost;
    [Header("Windows")]
    public GameObject buyWindow;
    public GameObject exploreWindow;
    public GameObject mineWindow;
    public GameObject sendWorkersWindow;
    public GameObject StartTurnWindow;
    public Slider counter;
    public Slider counterworkers;
    public Text counterlabel;
    public Text counterworkerslabel;
    public Text turnCounter;
    public Text startTurnText;
    public Text escortText, noEscortText;
    public Text errorText;
    public Text goldCount;
    [Header("ResoursesText")]
    public Text goldText, workerstext, warriortext;
    int nowtype;
    int tempcost = 0, tempupkeep = 0, tempfullcost = 0, tempcount = 0, tempworkers = 0, tempmappoint = 0;
    int turns = 1;
    int newpoints;
    // Start is called before the first frame update
    void Start()
    {
        turnCounter.text = "Turn : " + turns;
        freeworkers = workers;
        freewarriors = warriors;
        int count = maps.Length;
        points = new int[count];
        goldText.text = gold.ToString() + "/" + goldincreas.ToString();
        workerstext.text = freeworkers.ToString() + "/" + workers.ToString();
        warriortext.text = freewarriors.ToString() + "/" + freewarriors.ToString();
        lostwarriorsontern = 0;
        lostworkersonturn = 0;
        newpoints = 0;
    }

    public void BuyWorker(int count, int cost)
    {
        workers += count;
        freeworkers += count;
        gold -= cost;
    }

    public void BuyWarrior(int count, int cost, int upkeeps)
    {
        warriors += count;
        freewarriors += count;
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
        lostwarriorsontern += count;
        warriors -= count;
        upkeep -= count * upkeepfactor;
        UpdateText();
    }

    public void LoseWorkers(int count)
    {
        lostworkersonturn += count;
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
        if (goldincreas > 0)
            goldText.text = gold.ToString() + "/ +" + goldincreas.ToString();
        else
            goldText.text = gold.ToString() + "/ " + goldincreas.ToString();
        workerstext.text = freeworkers.ToString() + "/" + workers.ToString();
        warriortext.text = freewarriors.ToString() + "/" + warriors.ToString();
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
            goldCount.text = "Gold in Mine: " + mp.gold.ToString();
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
        if (freeworkers >= workersExplore)
        {
            freeworkers -= workersExplore;
            UpdateText();
            OpenPoint(mappoints[tempmappoint], false);
            return;
        }
        errorText.gameObject.SetActive(true);
        errorText.text = "Not much workers";
    }
    public void ExploreEscort()
    {
        if (freeworkers < workersExplore)
        {

            errorText.gameObject.SetActive(true);
            errorText.text = "Not much workers";
            return;
        }
        if (gold < escortfactor)
        {

            errorText.gameObject.SetActive(true);
            errorText.text = "Not much money";
            return;
        }
        if (freewarriors < warriorsEscrot)
        {

            errorText.gameObject.SetActive(true);
            errorText.text = "Not much warriors";
            return;
        }
        gold -= escortfactor;
        freeworkers -= workersExplore;
        freewarriors -= warriorsEscrot;
        UpdateText();
        OpenPoint(mappoints[tempmappoint], true);
    }

    public void OpenPoint(MapPoint mp, bool escort)
    {
        CleanErrorText();
        mp.workersonExplore = workersExplore;
        mp.escort = escort;
        mp.onExplore = true;
        if (mp.monsters && !mp.escort)
        {
            CloseExploreWindow();
            return;
        }
        for (int i = 0; i < mp.links.Length; i++)
        {
            points[mappos] = mp.links[i];
            mappos++;
        }
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
            }
            if (m.onExplore)
            {
                if (m.monsters && !m.escort)
                {
                    Debug.Log("LoseExplore");
                    m.workersonExplore = 0;
                    LoseWorkers(workerlost);
                    m.explored = false;
                }
                else
                {
                    if (m.monsters && m.escort)
                        LoseWarriors(warriorslost);
                    m.explored = true;
                    newpoints++;
                    for (int i = 0; i < points.Length; i++)
                    {
                        Debug.Log(i);
                        maps[points[i]].SetActive(true);
                    }
                }
            }
            m.onExplore = false;
        }
        ShowStartTurnWindow();
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
        GoldIncrease(tempworkers * goldminingfactor);
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

    public void InfoUpdate()
    {
        gold -= warriors;
        while (gold < 0)
        {
            warriors--;
            gold++;
        }
        goldincreas = 0;
        turns++;
        turnCounter.text = "Turn : " + turns;
        mappos = 0;
        freeworkers = workers;
        freewarriors = warriors;
        lostwarriorsontern = 0;
        lostworkersonturn = 0;
        newpoints = 0;
        GoldIncrease(-warriors);
        UpdateText();
    }
    
    public void ShowStartTurnWindow()
    {
        StartTurnWindow.SetActive(true);
        startTurnText.text = "Gold: " + goldincreas.ToString() + "\nLostWarriors: " + lostwarriorsontern.ToString() + "\nLostWorkers: " + lostworkersonturn.ToString() + "\nNewMines: " + newpoints.ToString();
        InfoUpdate();
    }

    public void NewExcortText()
    {
        escortText.text = "text" + "\nNeed Workers: " + workersExplore.ToString() + "\nNeed Warriors: " + warriorsEscrot.ToString() + "\nNeed Gold: " + escortfactor.ToString();
    }

    public void NewNoExcortText()
    {
        noEscortText.text = "text" + "\nNeed Workers: " + workersExplore.ToString();
    }

    public void CleanErrorText()
    {
        errorText.gameObject.SetActive(false);
        errorText.text = "";
    }
}
