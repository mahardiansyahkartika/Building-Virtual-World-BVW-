using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	// Use this for initialization
	
	public bool hasKey;
	public bool isMakeyMakeyActive = true;
	public float stepRatio;
	public float volumeSh;
	
	bool showingNotes;
	bool isIncoroution;
	bool isCurrentNoteNull = true;
	
	
	SoundController sc;
	BasementLightControl blc;

	GameObject currentNote;
	GameObject partyMusic;
	GameObject player;
	GameObject SoundFollowYou;
	GameObject weatherSound;
	
	private Vector3 pMusicPos = new Vector3(14.34863f, 1.388297f, 2.568963f );
	private Vector3 pMusicPosNew = new Vector3(12.70374f, -1.295909f, -0.9008411f );
	private Vector3 decreasePoint = new Vector3(12.70374f, -1.295909f, -0.9008411f );
	private Vector3 wallInside = new Vector3(4.014453f, 0.56f, 4.086f);
	private Vector3 inBasement = new Vector3(15f, 0.5f, 2.2f);
	
	enum soundName{
		whereiseveryone,
		someonedown,
		screaming
	};
	
	void Start () {
		player = GameObject.FindWithTag("Player");
		SoundFollowYou = GameObject.Find("SoundFollow");
		weatherSound = GameObject.Find("CameraRight");
		blc = GameObject.Find("BasementLightSets").GetComponent<BasementLightControl>();
		ShowNotes(false);
		initSound();
		initRig();
		stepRatio = 0f;
		//Invoke("PlayBeg", 1f);
	}
	
	// Update is called once per frame
	void Update () {
		//print(player.GetComponent<FirstPersonCharacter>().isWalking);
		//print(Vector3.Distance(player.transform.position, decreasePoint));
		//print((player.transform.position - wallInside).x);
		//print((player.transform.position - decreasePoint).z);
		
		//Adjust sound on First Floor
		if((((player.transform.position - decreasePoint).z > 5f) && ((player.transform.position - wallInside).x > 2.9f))) {
			//print("enterroom");
			partyMusic.audio.volume -= Time.deltaTime/2;
			//stepRatio = 0.5f;
			//enther room
		}else if(Vector3.Distance(player.transform.position, decreasePoint) > 9.7f) {
			partyMusic.audio.volume -= Time.deltaTime/2;
		}
		else{
			if(partyMusic.audio.volume < volumeSh) {
				partyMusic.audio.volume += Time.deltaTime * 2;
			}
		}
		
		//Making Follower Sound
		if(player.GetComponent<FirstPersonCharacter>().isWalking == false) {
			//random a walking sound
			RandFollowSound();
		}else {
			StopCoroutine("followSound");
			SoundFollowYou.audio.Stop();
		}
		//print ((inBasement- player.transform.position).y);
		//Adjust Weather Sound
		if((inBasement- player.transform.position).y > 0) {
			stepRatio = 0f;
			weatherSound.audio.volume -= Time.deltaTime/10;
			blc.playBuzzSound = true;
		}else if(weatherSound.audio.volume < 0.6){
			weatherSound.audio.volume += Time.deltaTime/10;
		}else {
			blc.playBuzzSound = false;
		}
	}
	
	void ShowNotes (bool b) {
		showingNotes = true; //used for exit reading notes
	}
	
	void PlayBeg() {
		sc.PlaySound((int)soundName.whereiseveryone);
	}
	
	void GetKey() {
		hasKey = true;
	}
	
	void initSound() {
		sc = GameObject.Find("SoundSets").GetComponent<SoundController>();
		partyMusic  = GameObject.Find("PartyMusic");
		partyMusic.transform.parent = null;
		partyMusic.transform.position = (pMusicPos);
		partyMusic.audio.volume = 0f;
		partyMusic.audio.playOnAwake = false;
		partyMusic.audio.Play();
	}
	
	public void SetCurrentNote(GameObject note) {
		currentNote = note;
		isCurrentNoteNull = false;
	}
	
	public void SetCurrentNoteNull() {
		isCurrentNoteNull = true;
	}
	
	public void RemoveCurrentNote() {
		if (!isCurrentNoteNull) {
			currentNote.SendMessage("GoBack");
		}
	}
	public void ChangeSoundPos() {
		partyMusic.transform.position = pMusicPosNew;
		partyMusic.GetComponent<AudioLowPassFilter>().enabled = false;
	}
	
	void initRig() {
		foreach (GameObject rig in GameObject.FindGameObjectsWithTag("Rig")) {
			rig.AddComponent("RigController");
		}
	}
	
	void RandFollowSound() {
		int n = Random.Range(5,10);
		if(!isIncoroution) {
			isIncoroution = true;
			StartCoroutine(followSound(n));
		}
	}
	
	IEnumerator followSound(int cnt) {
		int rollRatio = Random.Range(1,10);
		if(rollRatio < 10 * stepRatio) {
			for(int i = 0; i < cnt; i++) {
				SoundFollowYou.audio.Play ();
				yield return new WaitForSeconds(1f);
			}
		}
		yield return new WaitForSeconds(3f);
		isIncoroution = false;
	}
}