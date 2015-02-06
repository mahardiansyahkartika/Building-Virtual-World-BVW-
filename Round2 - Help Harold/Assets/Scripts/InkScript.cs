using UnityEngine;
using System.Collections;

public class InkScript : MonoBehaviour 
{
	public GUITexture Crayon;  //This has beeen innitialized in the inspector to the Crayon GUI object
	public float HeightScale=50.0f;
	public float k;
	//public static float InkUsePerUnitLength=0.004f;
	//public static float InkRemaining=1.0f;

	// Use this for initialization
	void Start () 
	{
		HeightScale = 50.0f;
		k = 1.0f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		UpdateInk (k);
	}

	void UpdateInk(float InkRemaining)
	{
		float temp= -360 * InkRemaining;
		Crayon.pixelInset = new Rect (0, 0, temp, HeightScale);
	}
}
