using UnityEngine;
using System.Collections;

public class Glow : MonoBehaviour 
{
	public SpriteRenderer SR;
	public float Omega;
	public float A;
	public Color Tint;
	public float Theta;
	private float Base;
	public float C;

	// Use this for initialization
	void Start () 
	{
		Theta = 0f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		Base = 255 - A;
		Theta += Omega*Time.deltaTime;
		C = Mathf.FloorToInt(Base + A * Mathf.Sin (Theta)) /255f;
		Tint = new Color  (C, C, C);
		SR.color = Tint;
	}
}
