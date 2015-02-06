using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class CameraController : MonoBehaviour {

	private GameObject playerCar;
	private Vector3 initPosPlayer;

	public AudioClip[] countDownAudio;
	public AudioClip finishAudio;

	// Use this for initialization
	void Start () {
		Screen.showCursor = false;

		playerCar = GameObject.FindGameObjectWithTag ("Player");

		initPosPlayer = playerCar.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 deltaPosCar = playerCar.transform.position - initPosPlayer;
		initPosPlayer = playerCar.transform.position;

		transform.Translate(new Vector3(deltaPosCar.x, 0, deltaPosCar.z),Space.World);
	}

	void PlayCountDown1() {
		audio.PlayOneShot (countDownAudio[0]);
	}

	void PlayCountDown2() {
		audio.PlayOneShot (countDownAudio[1]);
	}

	void PlayCountDown3() {
		audio.PlayOneShot (countDownAudio[2]);
	}

	void PlayFinish() {
		audio.PlayOneShot (finishAudio);
	}
}
