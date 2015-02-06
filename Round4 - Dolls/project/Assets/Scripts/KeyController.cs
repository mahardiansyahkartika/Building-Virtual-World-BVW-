using UnityEngine;
using System.Collections;

public class KeyController : MonoBehaviour {

	bool played = false;

	GameObject[] dollList;

	// Use this for initialization
	void Start () {
		dollList = new GameObject[4];
		for (int i = 0; i < dollList.Length; i++) {
			dollList[i] = GameObject.Find("Doll"+(i+1));
		}

		ShowFirstDoll ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnCollisionEnter(Collision c) {
		if(c.gameObject.tag == "Hand") {
			GameObject.Find("GameController").SendMessage("GetKey");
			GameObject.Find("SoundSets").SendMessage("PlaySound",4);

			GameObject.FindGameObjectWithTag ("Phone").SendMessage ("ShowWithDelay", 2);
			ShowLastDoll();
			Destroy(gameObject);
		}
	}

	void ShowFirstDoll() {
		for (int i = 0; i < dollList.Length; i++) {
			if (i < 2) {
				dollList[i].transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = true;
			} else {
				dollList[i].transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = false;
			}	
		}
	}

	void ShowLastDoll() {
		for (int i = 0; i < dollList.Length; i++) {
			if (i < 2) {
				dollList[i].transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = false;
			} else {
				dollList[i].transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = true;
			}
		}
	}
}