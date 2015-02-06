using UnityEngine;
using System.Collections;

public class Bob : MonoBehaviour 
{
	private Vector2 Root;
	public float Omega;
	public float A=1.0f;
	private float Theta;
	Vector2 Displacement;
	// Use this for initialization
	void Start () 
	{
		Root = transform.position;
		Theta = 0.0f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		Theta += Omega * Time.deltaTime;

		Displacement = new Vector2 (0, A * Mathf.Sin (Omega * Theta));
		gameObject.rigidbody2D.MovePosition (Root + Displacement);


		if (gameObject.tag == "Wheel") 
		{
			gameObject.rigidbody2D.MoveRotation(-Theta*Mathf.Rad2Deg);
		}
	}
	
}
