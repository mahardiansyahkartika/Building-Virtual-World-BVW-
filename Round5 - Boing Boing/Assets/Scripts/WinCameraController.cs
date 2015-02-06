using UnityEngine;
using System.Collections;

public class WinCameraController : MonoBehaviour {
	public bool win = false;
	public bool startfollow = false;
	public GameObject winPlayer;
	public Vector3 offset;
	public Camera c;
	public GameObject[] fireworks;
	public GameObject restart;
	public GameObject winner;

	SoundController sc;

	Vector3 pFacing;
	Vector3 pPos;
	Vector3 cFacing;
	Vector3 foffset;
	float increase = 1f;
	// Use this for initialization
	void Start () {
		sc = GameObject.Find ("GameController").GetComponent<SoundController> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(win) {
			PlayAnimation(winPlayer);
			startfollow = true;
			StartCoroutine(PlayParticle());
			foreach (Transform tf in GameObject.Find("SoundSets").transform) {
				tf.GetComponent<AudioSource>().volume = 0;
			}
			sc.PlaySound("Victory Sound", 0.9f, false);
			sc.PlaySound("yeah");
			sc.PlaySound("fireworks", 0.8f, true);
			sc.mute = true;
			winner.SetActive(true);
		}

		if(Input.GetKeyDown(KeyCode.R)) {
			Application.LoadLevel(Application.loadedLevel);
		}
		if(Input.GetKeyDown(KeyCode.Q)) {
			Application.LoadLevel(0);
		}
		if(Input.GetKeyDown(KeyCode.Y)) {
			Application.LoadLevel(Application.loadedLevel + 1);
		}
	}

	void LateUpdate() {
		//Quaternion q = Quaternion.AngleAxis(1 * 3f, Vector3.up);
		if(startfollow) {
			offset = Quaternion.AngleAxis ((increase) * 1.5f, Vector3.up) * offset;
			c.transform.position = pPos + offset;
			c.transform.LookAt (pPos);
		}
	}

	void PlayAnimation(GameObject p) {
		win = false;
		winPlayer = p;
		//pFacing = p.transform.forward;
		pPos = p.transform.position;
		//cFacing = -pFacing;
		c.transform.position = pPos + offset;
		//c.transform.forward = cFacing;
		c.transform.LookAt (pPos);
	}

	void PlayParticle_core() {
		int num = Random.Range (0, fireworks.Length);
		foffset = new Vector3 (Random.Range(-2f, 2f), Random.Range(0f, 2f),Random.Range(-2f, 2f));
		GameObject ps = Instantiate (fireworks[num], pPos + foffset, Quaternion.identity) as GameObject;
		Destroy (ps, 1f);
	}

	IEnumerator PlayParticle() {
		while (true) {
						PlayParticle_core ();
						PlayParticle_core ();
						yield return new WaitForSeconds (1.5f);
				}
	}

	public void Winneris(GameObject go) {
		winPlayer = go;
		win = true;
		GetCrown(go);
		foreach(GameObject p in GameObject.FindGameObjectsWithTag("Player")) {
			if(p.name != go.name) {
				p.SetActive(false);
			}
		}
		StartCoroutine(ShowRestart());
	}

	void GetCrown(GameObject go) {
		foreach(Transform tf in go.transform) {
			if(tf.gameObject.name == "crown") {
				tf.gameObject.SetActive(true);
			}
		}
	}

	IEnumerator ShowRestart() {
		yield return new WaitForSeconds(4f);
		iTween.MoveTo(restart, new Vector3(0.47f, 0.2f, 0f),2);
	}

}
