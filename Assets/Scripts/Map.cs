using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public GameObject[] maps;
    int[] points;
    int mappos;
    void Start()
    {
        int count = maps.Length;
        points = new int[count];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenPoint(MapPoint mp)
    {
        for (int i = 0; i< mp.links.Length; i++)
        {
            points[mappos] = mp.links[i];
            mappos++;
        }
    }

    public void EndTurn()
    {
        for (int i = 0; i < points.Length; i++)
        {
            maps[points[i]].SetActive(true);
        }       
    }
}
