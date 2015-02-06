using UnityEngine;
using System.Collections;

public class SingleTerrain {

	public int tileX, tileZ;
	public GameObject gameObject;

	// Use this for initialization
	public SingleTerrain (GameObject gameObject, int tileX, int tileZ) {
		this.gameObject = gameObject;
		this.tileX = tileX;
		this.tileZ = tileZ;
	}
}
