using UnityEngine;
using System.Collections;

public class BoundaryCollider : MonoBehaviour {

	public TileMovement.ShakeType shakeType;

	TileMovement tileMovement;

	void Awake()
	{
		tileMovement = GetComponent<TileMovement>();
	}

	void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.tag == "Player")
		{
			tileMovement.ShakeTile(shakeType);
		}
	}

}
