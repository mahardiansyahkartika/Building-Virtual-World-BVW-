using UnityEngine;
using System.Collections;

public class SharkHooked : MonoBehaviour {

	public static int Hooked;
	protected Drawing drawingScript;

	// Use this for initialization
	void Start () 
	{
		Hooked = 0;

		drawingScript = GameObject.Find("Drawing").GetComponent<Drawing>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Debug.Log ("Hooked = " + Hooked.ToString ());
	}
	
	void OnTriggerStay2D(Collider2D other)
		
	{
		if(other.gameObject.tag == "SmallLine")
		{
			Hooked=1;
		}
	}
	
	void OnTriggerExit2D(Collider2D other)
		
	{
		if(other.gameObject.tag == "SmallLine" )
		{
			Hooked=0;
		}
	}
}
