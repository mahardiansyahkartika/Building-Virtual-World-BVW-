using UnityEngine;
using System.Collections;

public class Scoreboard : MonoBehaviour {

	PlayerAttack[] playerAttacks;
	CharacterBaseController[] characterBaseControllers;
	GUIText[] killCountTexts;
	GUIText[] deathCountTexts;
	GameObject[] wingIcons;
	GameObject[] fistIcons;

	bool initialized;

	int activePlayerCount;
	GameController gameController;

	void Awake()
	{
		gameController = GameObject.Find("GameController").GetComponent<GameController>();
		activePlayerCount = gameController.activePlayersCount;
		playerAttacks = new PlayerAttack[activePlayerCount];
		killCountTexts = new GUIText[activePlayerCount];
		deathCountTexts = new GUIText[activePlayerCount];
		wingIcons = new GameObject[activePlayerCount];
		fistIcons = new GameObject[activePlayerCount];
		characterBaseControllers = new CharacterBaseController[activePlayerCount];
	}

	void Init()
	{
		Transform scoreboard = GameObject.Find("Scoreboard").transform;
		for(int i=0; i<activePlayerCount; i++)
		{
			playerAttacks[i] = gameController.GetPlayer(i + 1).GetComponent<PlayerAttack>();
			characterBaseControllers[i] = gameController.GetPlayer(i + 1).GetComponent<CharacterBaseController>();
			Transform t = scoreboard.Find("P" + (i + 1));
			killCountTexts[i] = t.Find("Kills").GetComponent<GUIText>();
			deathCountTexts[i] = t.Find("Deaths").GetComponent<GUIText>();
			wingIcons[i] = t.Find("Wing").gameObject;
			fistIcons[i] = t.Find("Boxing").gameObject;
			wingIcons[i].SetActive(false);
			fistIcons[i].SetActive(false);
		}

		initialized = true;
	}

	void Update()
	{
		if(!initialized)
		{
			Init();
		}

		for(int i=0; i<activePlayerCount; i++)
		{
			PlayerAttack pa = playerAttacks[i];
			CharacterBaseController cb = characterBaseControllers[i];
			killCountTexts[i].text = "Kills:" + pa.GetKillCount();
			deathCountTexts[i].text = "Deaths:" + pa.GetDeathCount();

			if(pa.IsMultipleFistExist() && !fistIcons[i].activeSelf)
			{
				fistIcons[i].SetActive(true);
			}
			else if(!pa.IsMultipleFistExist() && fistIcons[i].activeSelf)
			{
				fistIcons[i].SetActive(false);
			}
	
			if(cb.IsFlying() && !wingIcons[i].activeSelf)
			{
				wingIcons[i].SetActive(true);
			}
			else if(!cb.IsFlying() && wingIcons[i].activeSelf)
			{
				wingIcons[i].SetActive(false);
			}
		}
	}
}
