using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startLevel5 : MonoBehaviour
{
    // Start is called before the first frame update


    private statsUpdate StatsScript;
    void Start()
    {
        GameObject Stats = GameObject.Find("MainStats");
        StatsScript = Stats.GetComponent<statsUpdate>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void nextLevel()
    {
        StatsScript.startLevel5();
    }
}
