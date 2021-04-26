using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public int workersOnStart;
    public int warriorsOnStart;
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
    [Header("Monsters")]
    public int monster;
    [Header("Windows")]
    public GameObject buyWindow;
    public GameObject exploreWindow;
    public GameObject mineWindow;
    public GameObject sendWorkersWindow;
    public GameObject StartTurnWindow;
    public GameObject monstersAttackWindow;
    public GameObject infoMessageWindow;
    public GameObject WinLoseWindow;
    public GameObject pause;
    public Slider counter;
    public Slider counterworkers;
    public Text counterlabel;
    public Text counterworkerslabel;
    public Text turnCounter;
    public Text minecounter;
    public Text startTurnText;
    public Text escortText, noEscortText;
    public Text errorText;
    public Text goldCount;
    public Text monstersAttackText;
    public Text infoMessage;
    public Text WinLoseText;
    public Text textOnBuyWindow;
    [Header("ResoursesText")]
    public Text goldText, workerstext, warriortext;
    int nowtype;
    int tempcost = 0, tempupkeep = 0, tempfullcost = 0, tempcount = 0, tempworkers = 0, tempmappoint = 0;
    int turns = 1;
    int newpoints, allpoints = 0;
    bool firstmessage, secondmessage, thirdmessage;
    // Start is called before the first frame update
    void Start()
    {
        turnCounter.text = "Turn : " + turns;
        minecounter.text = "Mines : " + allpoints;
        BuyWorker(workersOnStart, 0);
        BuyWarrior(warriorsOnStart, 0, warriorsOnStart);
        int count = maps.Length;
        points = new int[count];
        goldText.text = gold.ToString() + "/" + goldincreas.ToString();
        workerstext.text = freeworkers.ToString() + "/" + workers.ToString();
        warriortext.text = freewarriors.ToString() + "/" + freewarriors.ToString();
        lostwarriorsontern = 0;
        lostworkersonturn = 0;
        newpoints = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowPause();
        }
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
        freewarriors -= count;
        if (warriors < 0)
        {
            warriors = 0;
            freewarriors = 0;
        }
        UpdateText();
    }

    public void LoseWorkers(int count)
    {
        lostworkersonturn += count;
        workers -= count;
        freeworkers -= count;
        if (workers < 0)
        {
            workers = 0;
            freeworkers = 0;
        }
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
        if (type == 1)
        {
            if (gold < workercost)
            {
                return;
            }
            textOnBuyWindow.text = "Workers can help you explore new mines and extract more gold. you NO need to upkeep your workers.";
            counter.maxValue = (int)gold / workercost;
            counterlabel.text = "count : 0";
        }
        if (type == 2)
        {
            if (gold < warriorcost)
            {
                return;
            }
            textOnBuyWindow.text = "Warriors can help you protect your workers and mines. You need to upkeep your warriors!";
            counter.maxValue = (int)gold / warriorcost;
            counterlabel.text = "count : 0";
        }
        buyWindow.SetActive(true);
    }

    public void Buying()
    {        
        if (nowtype == 1)
        {
            textOnBuyWindow.text = "Workers can help you explore new mines and extract more gold. you NO need to upkeep your workers.";
            tempcost = workercost;
        }
        if (nowtype == 2)
        {
            textOnBuyWindow.text = "Warriors can help you protect your workers and mines. You need to upkeep your warriors!";
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
            //Debug.LogError("OnExplore");
            return;
        }
        if (mp.NoGoldOnNextTurn() || mp.workershere == 5)
        {
            //Debug.LogError("Full");
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
                    //Debug.Log("LoseExplore");
                    m.workersonExplore = 0;
                    LoseWorkers(workerlost);
                    m.explored = false;
                    m.ExploreMine();
                }
                else
                {
                    if (m.monsters && m.escort)
                        LoseWarriors(warriorslost);
                    m.explored = true;
                    m.ExploreMine();
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
            counterworkers.maxValue = (HowMuckGoldOnPoint() / 3) - HowMuckWorkersOnPoint();
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
            //Debug.LogError("FULLhere");
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
        allpoints += newpoints;
        if (allpoints == 15)
        {
            WinLose(true);
            return;
        }
        goldincreas = 0;
        turns++;
        minecounter.text = "Mines: " + allpoints + "/15";
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
        startTurnText.text = "Results of last turn: \nGold income: " + goldincreas.ToString() + "\nWarriors lost: " + lostwarriorsontern.ToString() + "\nWorkers lost: " + lostworkersonturn.ToString() + "\nNew mine explored: " + newpoints.ToString();
        InfoUpdate();
    }

    public void NewExcortText()
    {
        escortText.text = "Your workers will be protected, but it costs money. 5 WORKERS will be busy with it. 5 WARRIORS will be busy with it. It will cost 5 GOLD.";
    }

    public void NewNoExcortText()
    {
        noEscortText.text = "You will leave workers unprotected, they can die if they meet monsters. Are you sure? 5 WORKERS will be busy with it.";
    }

    public void CleanErrorText()
    {
        errorText.gameObject.SetActive(false);
        errorText.text = "";
    }

    public void MonsterAtackWindowShow()
    {
        int monsterChance = allpoints - Random.Range(3, 10);
        if (monsterChance >= 4)
        {
            monster = allpoints + 7 - Random.Range(1, 9);
            BattleWithMonsters();
            return;
        }
        if (monsterChance >= 2)
        {
            monster = allpoints + 5 - Random.Range(1, 5);
            BattleWithMonsters();
            return;
        }
        if (monsterChance >= 0)
        {
            monster = allpoints + 3 - Random.Range(1, 2);
            BattleWithMonsters();
            return;
        }
        ShowMessageWindow();
    }

    public void BattleWithMonsters()
    {
        if (warriors > monster)
        {
            int lostinbattle = (int)(0.3f * allpoints) + 2;
            LoseWarriors((int)(0.3f * allpoints) + 2);
            monstersAttackWindow.SetActive(true);
            monstersAttackText.text = "Monsters were here! You win! \n Monster power: " + monster + "\n Warriors lost: " + lostinbattle.ToString();
            UpdateText();
            return;
        }
        if (warriors == monster)
        {
            int lostinbattle = (int)(0.5f * allpoints) + 2;
            LoseWarriors((int)(0.5f * allpoints) + 2);
            monstersAttackWindow.SetActive(true);
            monstersAttackText.text = "Monsters were here! You win! \n Monster power: " + monster + "\n Warriors lost: " + lostinbattle.ToString();
            UpdateText();
            return;
        }
        if (warriors < monster)
        {
            int lostinbattle = (int)(0.5f * allpoints) + 2;
            LoseWarriors((int)(0.5f * allpoints) + 2);
            LoseWorkers(lostworkersonturn);
            monstersAttackWindow.SetActive(true);
            monstersAttackText.text = "Monsters were here! You win! \n Monster power: " + monster + "\n Warriors lost: " + lostinbattle.ToString() + "\n Workers lost: " + lostwarriorsontern.ToString();
            UpdateText();
            return;
        }
    }

    public void ShowMessageWindow()
    {
        if (!firstmessage && allpoints >= 3)
        {
            infoMessageWindow.SetActive(true);
            infoMessage.text = "You go deeper and deeper! more and more monsters want your blood";
            firstmessage = true;
        }
        if (!secondmessage && allpoints >= 7)
        {
            infoMessageWindow.SetActive(true);
            infoMessage.text = "You go deeper and deeper! more and more monsters want your blood";
            secondmessage = true;
        }
        if (!thirdmessage && allpoints >= 11)
        {
            infoMessageWindow.SetActive(true);
            infoMessage.text = "You go deeper and deeper! more and more monsters want your blood";
            thirdmessage = true;
        }
    }

    public void WinLose(bool win)
    {
        if (win)
        {
            WinLoseText.text = "You did it! All mines under control. Congratulations!";
            WinLoseWindow.SetActive(true);
        }
        if (!win)
        {
            WinLoseText.text = "Monsters are stronger than you! Go cry or try again.";
            WinLoseWindow.SetActive(true);
        }
    }

    public void ShowPause()
    {
        pause.SetActive(true);
    }

    public void ToMainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void Retry()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
