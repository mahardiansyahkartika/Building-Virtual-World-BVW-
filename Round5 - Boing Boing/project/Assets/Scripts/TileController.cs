using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileController : MonoBehaviour {

	public int boardWidth; // how many tiles for the board's width (x axis)
	public int boardHeight; // how many tiles for the board's height (z axis)
	public Transform tile;
	public Transform[] tileTypes;
	public BoxCollider boundary;
	public PhysicMaterial boundaryPhysicMaterial;
	public GameObject boundaryExplosion;

	GameObject tilesLayer;
	GameObject[,] tilesObj;
	List<GameObject> activeTiles;

	float tileWidth;
	float tileDepth;
	float tileHeight;

	GameObject placeHolderTile; // tile used as reference for the others. located on the bottom left of the board
	GameController gameController;

	float time;

	void Awake()
	{
		tilesLayer = GameObject.Find("Tiles");
		activeTiles = new List<GameObject>();
		gameController = GameObject.Find("GameController").GetComponent<GameController>();

		InitializeTiles();
		//CreateBoundaryCollider();

		InitializeBoundaryTiles();
	}

	void InitializeTiles()
	{
		//tileWidth = tile.renderer.bounds.size.x;
		//tileDepth = tile.renderer.bounds.size.z;
		//tileHeight = tile.renderer.bounds.size.y;
		tileWidth = 1;
		tileDepth = 1;
		tileHeight = 1;

//		var test = GameObject.Find("GameObject").renderer.bounds.size;
//
//		print (test.x + " " + test.y + " " + test.z);
//		print (tileWidth)

		tilesObj = new GameObject[boardHeight, boardWidth];

		//deactive place holder tile not in tiles layer
		placeHolderTile = GameObject.Find("Tile");
		placeHolderTile.SetActive(false);

		for(int i=0; i<Mathf.Ceil(boardHeight * 0.5f); i++)
		{
			for(int j=0; j<Mathf.Ceil (boardWidth * 0.5f); j++)
			{
				int type;
				//int type = Random.Range (0, tileTypes.Length);

                if(j >= i && j <= boardWidth - 1 - i)
                {
                    type = i % tileTypes.Length;
                }
                else
                {
                    type = j % tileTypes.Length;
                }
                
				CreateTile(j, i, type);
				if(j != boardWidth - 1 - j)
					CreateTile(boardWidth - 1 - j, i, type);
				if(i != boardHeight - 1 - i)
					CreateTile(j, boardHeight - 1 - i, type);
				if(i != boardHeight - 1 - i && j != boardWidth - 1 - j)
					CreateTile(boardWidth - 1 - j, boardHeight - 1 - i, type);
			}
		}
	}

	void InitializeBoundaryTiles()
	{
		float width = tileWidth;
		float height = tileHeight;
		float left = GetWorldPos(0, 0).x - tileWidth * 0.5f - width * 0.5f;
		float right = GetWorldPos(boardWidth - 1, 0).x + tileWidth * 0.5f + width * 0.5f;
		float front = GetWorldPos(0, boardHeight - 1).z + tileHeight * 0.5f + height * 0.5f;
		float back = GetWorldPos(0, 0).z - tileHeight * 0.5f - height * 0.5f;
		float y = placeHolderTile.transform.position.y + tileHeight - 0.1f; // the last number is offset
		//int type = (int)(boardHeight * 0.5f) % tileTypes.Length;
		int type = 4;
		int count = 0;

		GameObject parent = new GameObject();
		parent.name = "Bounds";

		//left
		for(int i=0; i<boardHeight; i++)
		{
			CreateBoundaryTile(left, y, i * height, type, parent, count++, TileMovement.ShakeType.Left);
		}

		//right
		for(int i=0; i<boardHeight; i++)
		{
			CreateBoundaryTile(right, y, i * height, type, parent, count++, TileMovement.ShakeType.Right);
		}

		//front 
		for(int i=-1; i<boardWidth + 1; i++)
		{
			CreateBoundaryTile(i * width, y, front, type, parent, count++, TileMovement.ShakeType.Forward);
		}

		//back
		for(int i=-1; i<boardWidth + 1; i++)
		{
			CreateBoundaryTile(i * width, y, back, type, parent, count++, TileMovement.ShakeType.Backward);
		}
	}

	void CreateTile(int x, int y, int type)
	{
		Transform t = Instantiate(tileTypes[type], new Vector3(x*tileWidth, placeHolderTile.transform.position.y, y*tileDepth), Quaternion.identity) as Transform;
		t.name = tile.name + (y*boardHeight + x);
		t.parent = tilesLayer.transform;
		tilesObj[y, x] = t.gameObject;
		activeTiles.Add(t.gameObject);
		t.GetComponent<TileMovement> ().index = activeTiles.Count - 1;
	}

	void CreateBoundaryTile(float world_x, float world_y, float world_z, int type, GameObject parent, int number, TileMovement.ShakeType shakeType)
	{
		Transform t = Instantiate(tileTypes[type], new Vector3(world_x, world_y, world_z), Quaternion.identity) as Transform;
		t.name = "Boundary" + number;
		t.parent = parent.transform;
		t.tag = "Boundary";
		t.GetComponent<BoxCollider>().material = boundaryPhysicMaterial;
		BoundaryCollider bc = t.gameObject.AddComponent<BoundaryCollider>();
		bc.shakeType = shakeType;
	}

	public void BreakBoundaryTile(GameObject boundaryTile)
	{
		GameObject boundaryExplosionInst = Instantiate (boundaryExplosion, boundaryTile.transform.position, Quaternion.identity) as GameObject;
		Destroy (boundaryExplosionInst, 3f);
		Destroy(boundaryTile);
	}

	void CreateBoundaryCollider()
	{
		BoxCollider left = Instantiate(boundary) as BoxCollider;
		BoxCollider right = Instantiate(boundary) as BoxCollider;
		BoxCollider top = Instantiate(boundary) as BoxCollider;
		BoxCollider down = Instantiate(boundary) as BoxCollider;
		left.name = "left";
		right.name = "right";
		top.name = "top";
		down.name = "down";

		float midZ = (tilesObj[0, 0].transform.position.z + tilesObj[boardHeight - 1, 0].transform.position.z) / 2;
		float midX = (tilesObj[0, 0].transform.position.x + tilesObj[0, boardWidth - 1].transform.position.x) / 2;

		left.gameObject.transform.localScale = new Vector3(1, tileHeight + 3, boardHeight);
		right.gameObject.transform.localScale = new Vector3(1, tileHeight + 3, boardHeight);
		top.gameObject.transform.localScale = new Vector3(boardWidth, tileHeight + 3, 1);
		down.gameObject.transform.localScale = new Vector3(boardWidth, tileHeight + 3, 1);

		left.gameObject.transform.position = tilesObj[0, 0].transform.position + new Vector3(-tileWidth / 2 - boundary.size.x / 2, 0, midZ);
		right.gameObject.transform.position = tilesObj[0, boardWidth - 1].transform.position + new Vector3(tileWidth / 2 + boundary.size.x / 2, 0, midZ);
		top.gameObject.transform.position = new Vector3(midX, 0, tileDepth / 2 + boundary.size.z / 2) + tilesObj[boardHeight - 1, 0].transform.position;
		down.gameObject.transform.position = new Vector3(midX, 0, - tileDepth / 2 - boundary.size.z / 2) + tilesObj[0, 0].transform.position;

		GameObject parent = new GameObject();
		parent.name = "Bounds";
		left.transform.parent = parent.transform;
		right.transform.parent = parent.transform;
		top.transform.parent = parent.transform;
		down.transform.parent = parent.transform;
	}

	void SetTileHeight(TileMovement tile, float targetHeight)
	{
		tile.SetTileHeight(targetHeight);
	}

	public void MakeTilesDisappear()
	{
		int count = Random.Range(10, 15);
		List<GameObject> activeTiles = GetActiveTiles();

		for(int i=0;i<count;i++)
		{
			if(i >= activeTiles.Count)
				break;
			int random = Random.Range(i, activeTiles.Count-1);

			//swap it to beginning
			GameObject temp = activeTiles[random];
			activeTiles[random] = activeTiles[i];
			activeTiles[i] = temp;

			bool forbidden = false;
			Vector2 tilePos = GetTilePos(activeTiles[i].transform.position);
			TileMovement tileMovement = activeTiles[i].GetComponent<TileMovement>();
			foreach(var index in gameController.GetSpawnTileIndex())
			{
				if(index == tileMovement.index)
				{
					forbidden = true;
					break;
				}
			}

			if(!forbidden && !tileMovement.IsAboutToFall())
				tileMovement.ShakeTile(TileMovement.ShakeType.AboutToFall);
		}
	}

	public void ExplodeTiles(Vector2 tilePos)
	{
		int minX = (int)Mathf.Max(0, tilePos.x - 1);
		int maxX = (int)Mathf.Min(boardWidth - 1, tilePos.x + 1);
		int minY = (int)Mathf.Max(0, tilePos.y - 1);
		int maxY = (int)Mathf.Min(boardHeight - 1, tilePos.y + 1);
		Point[] spawnPoints = gameController.GetSpawnPoints();

		for(int i=minY; i<=maxY; i++)
		{
			for(int j=minX; j<=maxX; j++)
			{
				bool forbidden = false;
				for(int k=0; k<spawnPoints.Length; k++)
				{
					if(spawnPoints[k].x == j && spawnPoints[k].y == i)
					{
						forbidden = true;
						break;
					}
				}

				if(!forbidden)
				{
					tilesObj[i,j].gameObject.SetActive(false);
					activeTiles.Remove(tilesObj[i,j]);
				}
			}
		}
	}

	public List<GameObject> GetActiveTiles()
	{
		return activeTiles;
	}

	public void RemoveActiveTiles(GameObject tile)
	{
		activeTiles.Remove(tile);
	}

	//return tile pos given a world position
	//if it is outside the tile, then this function will return wrong value
	public Vector2 GetTilePos(Vector3 worldPos)
	{
		int x = (int)((worldPos.x - tilesObj[0, 0].transform.position.x) / tileWidth);
		int y = (int)((worldPos.z - tilesObj[0, 0].transform.position.z) / tileDepth);

		return new Vector2(x, y);
	}

	public Vector3 GetWorldPos(int tileX, int tileY)
	{
		return tilesObj[tileY, tileX].transform.position;
	}

	void Update()
	{
		time += Time.deltaTime;

		//cheat
		if(Input.GetKeyDown(KeyCode.Space))
		{
			MakeTilesDisappear();
		}

		if(Input.GetKeyDown(KeyCode.B))
		{
			for(int i=0; i<boardHeight; i++)
			{
				for(int j=0; j<boardWidth; j++)
				{
					tilesObj[i, j].GetComponent<TileMovement>().SetTileHeightToNormal();
				}
			}
		}
	}

	public float GetTileHeight()
	{
		return tileHeight;
	}

	public GameObject GetTileObject(int x, int y)
	{
		return tilesObj [y, x].gameObject;
	}
}
