using UnityEngine;
using System.Collections;

public class MoleController : MonoBehaviour {

	TileController tileController;
	MoleMovement moleMovement;

	void Awake()
	{
		tileController = GameObject.Find("GameController").GetComponent<TileController>();
		moleMovement = GameObject.Find("Mole").GetComponent<MoleMovement>();
	}

	public void MoleRunAround()
	{
		MoveToTilePos(Random.Range(0, tileController.boardWidth), Random.Range(0, tileController.boardHeight));
	}

	void MoveToTilePos(int x, int y)
	{
		moleMovement.SetTargetMoveTilePos(x, y);
	}

	void Update()
	{
		//cheat
		if(Input.GetKeyDown(KeyCode.M))
		{
			MoveToTilePos(Random.Range(0, tileController.boardWidth), Random.Range(0, tileController.boardHeight));
		}
	}
}
