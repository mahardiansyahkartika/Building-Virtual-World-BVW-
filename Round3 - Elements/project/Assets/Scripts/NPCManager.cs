using UnityEngine;
using System.Collections;

public class NPCManager : MonoBehaviour {

	public GameObject beast;
	public GameObject flyingMonster;

	public float waveWait = 4.0f;
	public float startWait = 2.0f;
	public float spawnWait = 2.5f;


	//these variables are used to get the wave information
	int totalGroupsInWave;
	int totalNPCsPerGroup;
	int npcHealthForWave;
	int npcSpeedForWave;

	protected float score = 0f;

	//these variables are used to keep track of the current wave
	int currentGroup;
	int currentNPCNumber;

	public int currentWave = 0; //starts from 0 and counts up
	public int totalNumberOfWaves = 3;

	private bool spawn = true;

	//NGHS = number of groups, no of NPCs per group, health of each NPC, speed
	private string[] wavePatternString = new string[] {"3-3-6-7", "3-4-8-9", "4-4-8-9"};
	//private string[] wavePatternString = new string[] {"1-1-1-1", "1-1-1-1", "1-1-1-1"};
	//private string[] wavePatternString = new string[] {"1-1-1-1"};

	// Use this for initialization
	void Start () {
		totalNumberOfWaves = wavePatternString.Length;
		print ("total no of waves : " + totalNumberOfWaves);
		InitializeNewWave ();
	}

	void Update() {
		/*
		Debug.Log ("asdfd f: " + score);

		if (score >= 200) {
			print ("You win the game!");
			GameObject.FindGameObjectWithTag ("WinLoseController").SendMessage("Win");
			return;
		}
		*/
	}

	//This function is used to grab the next string from the wave pattern array and initialize all the variables for spawning
	private void InitializeNewWave()
	{
		print ("INITIALIZED");

		if(currentWave >= (totalNumberOfWaves - 1))
		{
			print ("You win the game!");
			GameObject.FindGameObjectWithTag ("WinLoseController").SendMessage("Win");
			return;
		}

		currentGroup = 0;
		currentNPCNumber = 0;

		string[] splitString = wavePatternString[currentWave].Split("-"[0]);

		totalGroupsInWave = int.Parse (splitString[0]);
		totalNPCsPerGroup = int.Parse (splitString[1]);
		npcHealthForWave = int.Parse (splitString[2]);
		npcSpeedForWave = int.Parse (splitString[3]);

		spawn = true;
		StartCoroutine (SpawnWaves ());


		//print ("number of groups = " + totalGroupsInWave);
		//print ("number of NPC per group = " + totalNPCsPerGroup);
		//print ("health of npc = " + npcHealthForWave);
		//print ("speed of npcs = " + npcSpeedForWave);

		//StartCoroutine (SpawnNPC());
	}

	void InitializeNewGroup()
	{
		if (currentGroup > totalGroupsInWave)
		{
			print ("Wave done : " + currentWave);
			currentWave++;
			InitializeNewWave ();
		}
		else
		{
			spawn = true;
			StartCoroutine(SpawnWaves());
		}
	}

	IEnumerator SpawnWaves ()
	{				
		yield return new WaitForSeconds (startWait);

		while (spawn)
		{
			for (int i = 0; i < totalNPCsPerGroup; i++)
			{
				//Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
				//Quaternion spawnRotation = Quaternion.identity;
				Instantiate (beast);
				Instantiate(flyingMonster);
				yield return new WaitForSeconds (spawnWait);
			}

			print ("GROUP DONE");
			yield return new WaitForSeconds (waveWait);
			spawn = false;
		}

		//spawn = false;
		currentGroup++;
		InitializeNewGroup ();
	}


	public void AddScore(float score) {
		this.score += score;
	}
}
