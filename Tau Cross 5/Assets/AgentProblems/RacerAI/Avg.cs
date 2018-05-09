using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avg : MonoBehaviour {

  public GameObject AvgTester;

    void Start()
    {CalAvg();}

  public void CalAvg() // Reciver for the CONTROLLER
    {
      // Debug.Log ("Results");
      AvgTester.GetComponent<Calculate>().ShowAll();
      AvgTester.GetComponent<Calculate>().TotalNum = 0;
        AvgTester.GetComponent<Calculate>().TotalAddedNum = 0;
        AvgTester.GetComponent<Calculate>().highestNum = 0;
        AvgTester.GetComponent<Calculate>().lowestNum = 0;


    }
}
