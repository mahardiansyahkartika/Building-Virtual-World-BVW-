using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public struct Point
{
	public int x;
	public int y;
	
	public Point(int x, int y)
	{
		this.x = x;
		this.y = y;
	}
}

public class GameController : MonoBehaviour {

	public static bool[] activePlayers = new bool[]{false,false,false,false};

	public int activePlayersCount = 4;

	Transform p1;
	Transform p2;
	Transform p3;
	Transform p4;
	Transform[] players;
	TileController tileController;

	bool initialized;
	float spawnYPos;
	int spawnPosOffset = 3;
	Point[] spawnPoints;
	int[] spawnTileIndex;
	float OFFSET_SPAWN_Y_POS = 3f;
	float remainingGameTime;
	const float MAX_GAME_TIME = 120f; // in seconds
	
	WinCameraController winCameraController;
	ItemGenerator itemGenerator;
	SoundController soundController;
	SceneFader sceneFader;

	bool gameStart;
	GameObject StartCountdown;
	GameObject finishCountdown;
	GameObject[] finishCountdownObjects;

	int[] disappearingTimeCue = new int[] {30, 20, 10, 5}; // in game seconds
	int disappearingIndex;

	float oriFixedDeltaTime;

	void Awake()
	{
		//just to make sure the timescale is one
		Time.timeScale = 1f;
		oriFixedDeltaTime = Time.fixedDeltaTime;

		remainingGameTime = MAX_GAME_TIME;
		sceneFader = GameObject.Find("SceneFader").GetComponent<SceneFader>();
		p1 = GameObject.Find("P1").transform;
		p2 = GameObject.Find("P2").transform;
		p3 = GameObject.Find("P3").transform;
		p4 = GameObject.Find("P4").transform;
		tileController = GetComponent<TileController>();
		soundController = GetComponent<SoundController>();
		players = new Transform[4];
		players[0] = p1;
		players[1] = p2;
		players[2] = p3;
		players[3] = p4;

		for(int i=activePlayersCount; i < 4; i++)
		{
			players[i].gameObject.SetActive(false);
		}

		winCameraController = Camera.main.GetComponent<WinCameraController>();
		itemGenerator = GetComponent<ItemGenerator>();
		StartCountdown = GameObject.Find("StartCountdown");
		finishCountdown = GameObject.Find("FinishCountdown");
		finishCountdownObjects = new GameObject[6];

		for(int i=0; i<StartCountdown.transform.childCount; i++)
		{
			StartCountdown.transform.GetChild(i).gameObject.SetActive(false);	
        }

		for(int i=0; i<finishCountdown.transform.childCount; i++)
		{
			GameObject child = finishCountdown.transform.GetChild(i).gameObject;
			child.SetActive(false);
			finishCountdownObjects[Int32.Parse(child.name)] = child;
		}

		StartCoroutine(ShowStartCountdown(3));
	}

	void Start() {
		Screen.showCursor = false;
	}

	void Init()
	{
		spawnYPos = p1.transform.position.y + OFFSET_SPAWN_Y_POS;
		int boardWidth = tileController.boardWidth;
		int boardHeight = tileController.boardHeight;
		spawnPoints = new Point[4];
		spawnPoints[0] = new Point(spawnPosOffset, spawnPosOffset);
		spawnPoints[1] = new Point(boardWidth - 1 - spawnPosOffset, spawnPosOffset);
		spawnPoints[2] = new Point(boardWidth - 1 - spawnPosOffset, boardHeight - 1 - spawnPosOffset);
		spawnPoints[3] = new Point(spawnPosOffset, boardHeight - 1 - spawnPosOffset);

		spawnTileIndex = new int[4];
		int count_index = 0;
		foreach(var point in spawnPoints)
		{
			spawnTileIndex[count_index++] = tileController.GetTileObject(point.x, point.y).GetComponent<TileMovement>().index;
		}

		for(int i=1; i<=4; i++)
		{
			SpawnPlayer(i);
		}

		// freeze the players first
		for(int i=0; i<activePlayersCount; i++)
		{
			players[i].GetComponent<CharacterBaseController>().SetFreezeMovement(true);
		}
		
		//freeze the item generator
		itemGenerator.SetEnabled(false);
		
		//freeze the remaining game time timer
        gameStart = false;

		sceneFader.FadeInScene();

		initialized = true;
	}

	void StartGame()
	{
		// unfreeze the players first
		for(int i=0; i<activePlayersCount; i++)
		{
			players[i].GetComponent<CharacterBaseController>().SetFreezeMovement(false);
		}

		
		//unfreeze the item generator
		itemGenerator.SetEnabled(true);
		
		//unfreeze the remaining game time timer
		gameStart = true;
	}

	public void SpawnPlayer(int playerIndex)
	{
		Vector3 pos;
		GameObject player = players[playerIndex - 1].gameObject;
		int boardWidth = tileController.boardWidth;
		int boardHeight = tileController.boardHeight;
		float tileHeight = tileController.GetTileHeight();

		switch(playerIndex)
		{
		case 1 :
			pos = tileController.GetWorldPos(spawnPosOffset, spawnPosOffset) + new Vector3(0, spawnYPos + tileHeight / 2, 0);
			break;
		case 2:
			pos = tileController.GetWorldPos(boardWidth - 1 - spawnPosOffset, spawnPosOffset) + new Vector3(0, spawnYPos + tileHeight / 2, 0);
			break;
		case 3:
			pos = tileController.GetWorldPos(boardWidth - 1 - spawnPosOffset, boardHeight - 1 - spawnPosOffset) + new Vector3(0, spawnYPos + tileHeight / 2, 0);
			break;
		case 4:
		default:
			pos = tileController.GetWorldPos(spawnPosOffset, boardHeight - 1 - spawnPosOffset) + new Vector3(0, spawnYPos + tileHeight / 2, 0);
			break;
		}

		player.transform.position = pos;
		player.transform.rotation = Quaternion.identity;
		player.GetComponent<CharacterBaseController>().Reset();
	}

	void Update()
	{
		if(!initialized)
		{
			Init();
		}

		if(gameStart && remainingGameTime > 0)
		{
			remainingGameTime -= Time.deltaTime;
			if(remainingGameTime >= 0f && remainingGameTime <= 6f)
			{
				int number = (int) Mathf.Floor(remainingGameTime);
				if(!finishCountdownObjects[number].activeSelf)
				{
					if(number < 5)
					{
						finishCountdownObjects[number + 1].SetActive(false);
					}
					if(number == 0)
					{
						soundController.PlaySound("whistle");
					}
					finishCountdownObjects[number].SetActive(true);
				}
			}

			if(disappearingIndex < disappearingTimeCue.Length && Mathf.FloorToInt(remainingGameTime) == disappearingTimeCue[disappearingIndex])
			{
				disappearingIndex++;
				tileController.MakeTilesDisappear();
			}

			if(remainingGameTime <= 0f)
			{
				StartCoroutine(FinishGame());
			}
		}

		if(Input.GetKeyDown(KeyCode.P))
		{
			StartCoroutine(FinishGame());
		}
		if(Input.GetKeyDown(KeyCode.O))
		{
			print(tileController.GetTilePos(tileController.GetTileObject(11, 5).transform.position));
		}
	}

	IEnumerator FinishGame()
	{
		remainingGameTime = 0;

		Time.timeScale = 0.5f;
		Time.fixedDeltaTime = oriFixedDeltaTime * Time.timeScale;

		yield return new WaitForSeconds(1);

		Time.timeScale = 0.3f;
		Time.fixedDeltaTime = oriFixedDeltaTime *  Time.timeScale;

		yield return new WaitForSeconds(0.3f);

		Time.timeScale = 0.1f;
		Time.fixedDeltaTime = oriFixedDeltaTime *  Time.timeScale;

		yield return new WaitForSeconds(0.1f);

		Time.timeScale = 1f;
		Time.fixedDeltaTime = oriFixedDeltaTime *  Time.timeScale;

		//copy the players array
		Transform[] arr = new Transform[4];

		for(int i=0; i<4; i++)
		{
			arr[i] = players[i];
		}

		//search for player with highest kill, if they have the same kill count then compare the least death count, if it is still the same, compare who has the first kill

		Array.Sort(arr, CompareWinner);

		for(int i=0; i<activePlayersCount; i++)
		{
			PlayerAttack pa = arr[i].GetComponent<PlayerAttack>();
			arr[i].GetComponent<CharacterBaseController>().SetFreezeMovement(true);

			print (i + " place : " + arr[i].name + " " + pa.GetKillCount() + " " + pa.GetDeathCount() + " " + pa.IsFirstKill());
		}

		//disable time's up text
		finishCountdown.transform.Find("0").gameObject.SetActive(false);

		//freeze the item generator
		itemGenerator.SetEnabled(false);

		sceneFader.FadeOutScene(1f, ()=> {
			sceneFader.FadeInScene();
			Vector3 tilePos = tileController.GetWorldPos(tileController.boardWidth / 2, tileController.boardHeight / 2);
			arr[0].gameObject.transform.position = new Vector3(tilePos.x, spawnYPos - OFFSET_SPAWN_Y_POS, tilePos.z);//place the player in the middle of the arena
			winCameraController.Winneris(arr[0].gameObject);
		});

		yield return null;
	}

	public float GetRemainingGameTime()
	{
		return remainingGameTime;
	}

	public float GetMaximumGameTime()
	{
		return MAX_GAME_TIME;
	}

	// 1 - based
    public GameObject GetPlayer(int index)
    {
        if(index <= players.Length)
        {
            return players[index-1].gameObject;
        }
        else
        {
            return null;
        }
    }

	public static int CompareWinner(Transform t1, Transform t2)
	{
		PlayerAttack pa1 = t1.GetComponent<PlayerAttack>();
		PlayerAttack pa2 = t2.GetComponent<PlayerAttack>();

		if(pa1.GetKillCount() != pa2.GetKillCount())
		{
			return (pa1.GetKillCount() > pa2.GetKillCount()) ? -1 : 1; // the most kill count
		}
		else if(pa1.GetDeathCount() != pa2.GetDeathCount())
		{
			return (pa1.GetDeathCount() < pa2.GetDeathCount()) ? -1 : 1; // the lease kill count
		}
		else if(pa1.IsFirstKill() != pa2.IsFirstKill())
		{
			return pa1.IsFirstKill() ? -1 : 1;
		}
		else 
		{
			return 0;
		}
	}

	public float GetSpawnYPos()
	{
		return spawnYPos;
	}

	public Point[] GetSpawnPoints()
	{
		return spawnPoints;
	}

	public int[] GetSpawnTileIndex()
	{
		return spawnTileIndex;
	}

	IEnumerator ShowStartCountdown(int value)
	{
		yield return new WaitForSeconds(2f);

		while(value >= 0)
		{
			for(int i=0; i<StartCountdown.transform.childCount; i++)
			{
				StartCountdown.transform.GetChild(i).gameObject.SetActive(false);	
			}

			if(value > 0)
			{
				soundController.PlaySound("b");
			}
			else
			{
				soundController.PlaySound("go");
			}
			StartCountdown.transform.Find("" + value).gameObject.SetActive(true);
			value--;
			yield return new WaitForSeconds(1f);
		}

		StartGame();

		yield return new WaitForSeconds(1f);
		StartCountdown.transform.Find("0").gameObject.SetActive(false);
	}
}
