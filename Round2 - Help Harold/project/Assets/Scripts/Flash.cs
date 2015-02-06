using UnityEngine;
using System.Collections;

public class Flash : MonoBehaviour {

	public Transform PurpleCar;
	public float CycleTime;
	bool Blink;
	bool ON;

	// Use this for initialization
	void Start () 
	{
		Blink = true;
		ON = true;
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Blink==true)
		{
			StartCoroutine(Alternate());
		}
	}

	IEnumerator Alternate()
	{
		Blink = false;
		if(ON)
		{
			PurpleCar.gameObject.SetActive(false);
			ON=false;
		}
		else
		{
			PurpleCar.gameObject.SetActive (true);
			ON=true;
		}

		yield return new WaitForSeconds (CycleTime);
		Blink=true;
	}
}
