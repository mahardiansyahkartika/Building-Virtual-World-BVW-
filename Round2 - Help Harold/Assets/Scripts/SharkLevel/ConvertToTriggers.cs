using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConvertToTriggers : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		GameObject[] DrawnLineList = GameObject.FindGameObjectsWithTag("DrawnObject");

		for (int i = 0; i < DrawnLineList.Length; i++)
		{
			//if (DrawnLineList[i].transform.childCount > 0) 
			//{
				foreach (Transform child in DrawnLineList[i].transform) 
				{
					child.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
				}
				//DrawnLineList[i].tag = "TriggeredDrawnObject";
			//}
		}

	}
	
}
