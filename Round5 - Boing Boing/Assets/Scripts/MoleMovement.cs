using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoleMovement : MonoBehaviour {

	public float speed = 1f;

	bool isMoving;
	Vector3 offsetPos; // from tile
	Vector2 targetTilePos;
	List<Vector3> path;
	float stoppingDistance = 0.1f;
	float sqrStoppingDistance;

	TileController tileController;

	void Awake()
	{
		sqrStoppingDistance = stoppingDistance * stoppingDistance;
		path = new List<Vector3>();
		tileController = GameObject.Find("GameController").GetComponent<TileController>();
	}

	void Start()
	{
		//intentionally in Start()
		offsetPos = transform.position - tileController.GetWorldPos(0, 0);

		//put the mole in the middle at the very first start
		GameObject.Find("Mole").transform.position = tileController.GetWorldPos(tileController.boardWidth / 2, tileController.boardHeight / 2) + offsetPos;
	}

	public void SetTargetMoveTilePos(int x, int y)
	{
		targetTilePos = new Vector2(x, y);
		isMoving = true;
		List<Vector2> tilePath = CreatePath(targetTilePos);
		path.Clear();

		//convert to world pos
		for(int i=0; i<tilePath.Count; i++)
		{
			path.Add(tileController.GetWorldPos((int) tilePath[i].x, (int) tilePath[i].y) + offsetPos);
		}

//		foreach(var worldPos in path)
//		{
//			print (worldPos);
//		}
	}

	void Update()
	{
		//print (transform.position);
		if(isMoving)
		{
			if(path.Count > 0)
			{
				//print(path.Count + " " + isMoving + " " + ((path[0] - transform.position).normalized * speed));
				//print (path.Count + " " + path[0] + " " + transform.position);
				transform.position += (path[0] - transform.position).normalized * speed * Time.deltaTime;
				
				if((transform.position - path[0]).sqrMagnitude < sqrStoppingDistance)
				{
					path.RemoveAt(0);
				}
			}
			else
			{
				transform.position = tileController.GetWorldPos((int) targetTilePos.x, (int) targetTilePos.y) + offsetPos;
				isMoving = false;
			}
		}
	}

	List<Vector2> CreatePath(Vector2 targetTilePos)
	{
		Vector2 currentPos = tileController.GetTilePos(transform.position);
		Queue<Vector2> queue = new Queue<Vector2>();
		bool[, ] visited = new bool[tileController.boardHeight, tileController.boardWidth];
		Vector2[, ] cameFrom = new Vector2[tileController.boardHeight, tileController.boardWidth];
		Vector2[] dir = new Vector2[] {new Vector2(0, -1), new Vector2(1, 0),  new Vector2(0, 1), new Vector2(-1, 0)};

		queue.Enqueue(currentPos);
		cameFrom[(int) currentPos.y, (int) currentPos.x] = new Vector2(-1, -1);

		while(queue.Count > 0)
		{
			currentPos = queue.Dequeue();

			if(targetTilePos.x == currentPos.x && targetTilePos.y == currentPos.y)
			{
				return ReconstructPath(ref cameFrom, currentPos);
			}

			visited[(int) currentPos.y, (int) currentPos.x] = true;

			for(int i=0; i<4; i++)
			{
				Vector2 nextPos = currentPos + dir[i];
				if(nextPos.x < 0 || nextPos.y < 0 || nextPos.x >= tileController.boardWidth || nextPos.y >= tileController.boardHeight
				   || visited[(int) nextPos.y, (int) nextPos.x])
				{
					continue;
				}

				cameFrom[(int) nextPos.y, (int) nextPos.x] = currentPos;
				queue.Enqueue(nextPos);
			}
		}

		return null;
	}

	List<Vector2> ReconstructPath(ref Vector2[, ] cameFrom, Vector2 currentPos)
	{
		List<Vector2> path = new List<Vector2>();
		Vector2 invalid = new Vector2(-1, -1);

		while(currentPos != invalid)
		{
			path.Add(currentPos);
			currentPos = cameFrom[(int) currentPos.y, (int) currentPos.x];
		}

		path.Reverse();
		return path;
	}
}
