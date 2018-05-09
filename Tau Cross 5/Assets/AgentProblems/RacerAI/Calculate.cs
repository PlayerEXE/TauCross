using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calculate : MonoBehaviour {

	public float TotalNum = 0;
    public float TotalAddedNum = 0;
	public float highestNum = 0;
	public float lowestNum = 0;

    public void ShowAll()
    {
        Debug.Log("Results: |NAI| " + TotalNum + " |High| " + highestNum + " |Low| " + lowestNum + " |Total| " + TotalAddedNum + " |Avg| " + (TotalAddedNum / TotalNum));
    }
}
