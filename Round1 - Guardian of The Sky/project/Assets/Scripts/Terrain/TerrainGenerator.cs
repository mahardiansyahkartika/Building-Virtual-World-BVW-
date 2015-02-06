using UnityEngine;
using System.Collections;

public class TerrainGenerator : MonoBehaviour {
	// player
	public Transform player;
	// prefab object of singleTerrain
	public Object terrainObject;
	// total tile in the scene will be (buffer * 2 + 1) ^ 2
	protected int buffer = 10;
	// control scale (by default singleTerrain is 10 units each unit length = 1 unity unit), by default singleTerrainSize = 10f;
	protected float singleTerrainSize = 600f;
	// total singleTerrain in a single row (buffer * 2 + 1)
	protected int terrainCountRow; 
	// current position
	protected int posX, posZ;
	// list all terrain
	protected SingleTerrain[,] terrainList;

	// Use this for initialization
	void Start () {
		terrainCountRow = buffer * 2 + 1;
		posX = Mathf.RoundToInt (player.position.x / singleTerrainSize);
		posZ = Mathf.RoundToInt(player.position.z / singleTerrainSize);

		GenerateAll ();
	}

	// create single terrain with given index
	protected SingleTerrain CreateSingleTerrain(int x, int z) {
		GameObject singleTerrainGameObject = (GameObject)Instantiate(terrainObject, new Vector3(x * singleTerrainSize, 0, z * singleTerrainSize), Quaternion.identity);
		singleTerrainGameObject.transform.localScale = new Vector3(singleTerrainSize * 0.1f, 1, singleTerrainSize * 0.1f);
		singleTerrainGameObject.transform.parent = transform;

		return new SingleTerrain (singleTerrainGameObject, x, z);
	}
	
	protected void GenerateAll() {
		// kill them all
		if (terrainList != null) {
			foreach (SingleTerrain terrain in terrainList) {
				Destroy(terrain.gameObject);
			}
		}
		
		// initiate new list
		terrainList = new SingleTerrain[terrainCountRow, terrainCountRow];
		
		// create new one
		for (int x  = 0; x < terrainCountRow; x++) {
			for (int z = 0; z < terrainCountRow; z++){
				terrainList[x, z] = CreateSingleTerrain(x + posX - buffer, z - posZ - buffer);
			}
		}
	}

	protected void UpdateTerrain(int deltaX, int deltaZ) {
		SingleTerrain[] newRowTerrainList = new SingleTerrain[terrainCountRow];
		SingleTerrain[,] newTerrainList =  new SingleTerrain[terrainCountRow, terrainCountRow]; 

		// remove the useless terrain
		if (deltaX != 0) {
			for (int i = 0; i < terrainCountRow; i++) {
				Destroy(terrainList[buffer - buffer * deltaX, i].gameObject);
				terrainList[buffer - buffer * deltaX, i] = null;
				newRowTerrainList[i] = CreateSingleTerrain(posX + buffer * deltaX + deltaX, posZ - buffer + i);
			}
		}
		if (deltaZ != 0) {
			for (int i = 0; i < terrainCountRow; i++) {
				Destroy(terrainList[i, buffer - buffer * deltaZ].gameObject);
				terrainList[i, buffer - buffer * deltaZ] = null;
				newRowTerrainList[i] = CreateSingleTerrain(posX - buffer + i, posZ + buffer * deltaZ + deltaZ);
			}
		}

		// copy list
		System.Array.Copy(terrainList, newTerrainList, terrainCountRow * terrainCountRow);

		// move index
		for (int i = 0; i < terrainCountRow; i++) {
			for (int j = 0; j < terrainCountRow; j++) {
				SingleTerrain _terrain = terrainList[i, j];
				if (_terrain != null) 
					newTerrainList[-posX - deltaX + buffer + _terrain.tileX, -posZ - deltaZ + buffer + _terrain.tileZ] = _terrain;
			}
		}

		// add new terrain
		for (int i = 0; i < newRowTerrainList.Length; i++) {
			SingleTerrain _terrain = newRowTerrainList[i];
			newTerrainList[-posX - deltaX + buffer + _terrain.tileX, -posZ - deltaZ + buffer + _terrain.tileZ] = _terrain;
		}
		
		terrainList = newTerrainList;
	}
	
	// Update is called once per frame
	void Update () {
		// currentPosition
		int newPosX = Mathf.RoundToInt (player.position.x / singleTerrainSize);
		int newPosZ = Mathf.RoundToInt (player.position.z / singleTerrainSize);

		// check
		int deltaX = (newPosX - posX);
		int deltaZ = (newPosZ - posZ);

		if (deltaX != 0) {
			UpdateTerrain(deltaX, 0);
			posX = newPosX;
		}

		if (deltaZ != 0) {
			UpdateTerrain(0, deltaZ);			
			posZ = newPosZ;
		}
	}
}
