using UnityEngine;
using System.Collections;

public class HDebug : MonoBehaviour {

	Transform p1;
	Transform p2;
	Transform p3;
	Transform p4;
	
	GUIText p1FreezeTimeText;
	GUIText p1KillCountText;
	GUIText p1LastHitFromText;

	GUIText p2FreezeTimeText;
	GUIText p2KillCountText;
	GUIText p2LastHitFromText;

	GUIText p3KillCountText;
	GUIText p3LastHitFromText;

	GUIText p4KillCountText;
	GUIText p4LastHitFromText;

	GUIText gameTimeText;

	PlayerAttack p1Attack;
	PlayerAttack p2Attack;
	PlayerAttack p3Attack;
	PlayerAttack p4Attack;

	GameController gameController;

	int activePlayerCount;

	void Awake()
	{
		gameController = GameObject.Find("GameController").GetComponent<GameController>();
		activePlayerCount = gameController.activePlayersCount;

		p1FreezeTimeText = transform.Find("FreezeTime").GetComponent<GUIText>();
		p1KillCountText = transform.Find("P1KillCount").GetComponent<GUIText>();
		p1LastHitFromText = transform.Find("P1LastHitFrom").GetComponent<GUIText>();

		p2FreezeTimeText = transform.Find("FreezeTimeP2").GetComponent<GUIText>();
		p2KillCountText = transform.Find("P2KillCount").GetComponent<GUIText>();
		p2LastHitFromText = transform.Find("P2LastHitFrom").GetComponent<GUIText>();

		p3KillCountText = transform.Find("P3KillCount").GetComponent<GUIText>();
		p3LastHitFromText = transform.Find("P3LastHitFrom").GetComponent<GUIText>();

		p4KillCountText = transform.Find("P4KillCount").GetComponent<GUIText>();
		p4LastHitFromText = transform.Find("P4LastHitFrom").GetComponent<GUIText>();

		gameTimeText = transform.Find("GameTime").GetComponent<GUIText>();

		p1 = GameObject.Find("P1").transform;
		p1Attack = p1.GetComponent<PlayerAttack>();
		p2 = GameObject.Find("P2").transform;
		p2Attack = p2.GetComponent<PlayerAttack>();
		if(activePlayerCount >= 3)
		{
			p3 = GameObject.Find ("P3").transform;
			p3Attack = p3.GetComponent<PlayerAttack>();
		}

		if(activePlayerCount >= 4)
		{
			p4 = GameObject.Find("P4").transform;
			p4Attack = p4.GetComponent<PlayerAttack>();
		}
	}

	void Update()
	{
		gameTimeText.text = "Remaining Game Time : " + gameController.GetRemainingGameTime();

		//p1FreezeTimeText.text = "P1 FreezeTime : " + p1Attack.GetFreezeTime();
		p1KillCountText.text = "P1 Kill Count : " + p1Attack.GetKillCount();
		p1LastHitFromText.text = "P1 Last Hit From : " + (p1Attack.GetLastHitFrom() != null ? p1Attack.GetLastHitFrom().name : "");

		//p2FreezeTimeText.text = "P2 FreezeTime : " + p2Attack.GetFreezeTime();
		p2KillCountText.text = "P2 Kill Count : " + p2Attack.GetKillCount();
		p2LastHitFromText.text = "P2 Last Hit From : " + (p2Attack.GetLastHitFrom() != null ? p2Attack.GetLastHitFrom().name : "");

		if(activePlayerCount >= 3)
		{
			p3KillCountText.text = "P3 Kill Count : " + p3Attack.GetKillCount();
			p3LastHitFromText.text = "P3 Last Hit From : " + (p3Attack.GetLastHitFrom() != null ? p3Attack.GetLastHitFrom().name : "");
		}

		if(activePlayerCount >= 4)
		{
			p4KillCountText.text = "P4 Kill Count : " + p4Attack.GetKillCount();
			p4LastHitFromText.text = "P4 Last Hit From : " + (p4Attack.GetLastHitFrom() != null ? p4Attack.GetLastHitFrom().name : "");
		}
	}

}
