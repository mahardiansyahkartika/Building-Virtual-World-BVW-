using UnityEngine;
using System.Collections;

public class PhoneController : MonoBehaviour
{
	public Texture[] textureList;

	private Vector3 initLocalPosition, hidePosition;
	
	// animation attribute
	private bool isAnimating = false;
	private float elapsedTime = 0f;
	private float totalTimeAnimation = 1f;
	private Vector3 firstPosition, targetPosition;

	// sound attribute
	private float elapsedTimeSFX = 0f;
	private bool isWaitingSFX = false;
	private float durationSFX = 0f;

	private int index = 1;
	private float delay = 1f;

	// Use this for initialization
	void Start ()
	{
		initLocalPosition = transform.localPosition;

		// hide from camera
		hidePosition = new Vector3 (transform.localPosition.x, transform.localPosition.y - 0.4f, transform.localPosition.z);
		transform.localPosition = hidePosition;
	}

	// Update is called once per frame
	void Update ()
	{
		if (isAnimating) {
			elapsedTime += Time.deltaTime;

			transform.localPosition = Vector3.Slerp(firstPosition, targetPosition, elapsedTime / totalTimeAnimation);

			if (elapsedTime >= totalTimeAnimation) {
				if (targetPosition == initLocalPosition) {
					isWaitingSFX = true;

					// play VO
					AudioClip lindseyVoice = Resources.Load("Sound/First Floor/party is downstairs") as AudioClip;
					switch(index) {
					case 1:
						lindseyVoice = Resources.Load("Sound/First Floor/party is downstairs") as AudioClip;
						break;
					case 2 :
						lindseyVoice = Resources.Load("Sound/First Floor/hurry up lindsay") as AudioClip;
						break;
					}

					durationSFX = lindseyVoice.length;
					GameObject.Find("LindseyVoice").SendMessage("PlaySound", lindseyVoice);
				} else if (targetPosition == hidePosition) {
					// enable makey makey
					GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ().isMakeyMakeyActive = true;
				}

				// init
				isAnimating = false;
				elapsedTime = 0f;
			}
		}

		if (isWaitingSFX) {
			elapsedTimeSFX += Time.deltaTime;
			if (elapsedTimeSFX >= durationSFX) {
				// init
				isWaitingSFX = false;
				elapsedTimeSFX = 0f;

				Hide();
			}
		}
	}
	
	void Show(int index) {
		this.index = index;

		if (!isAnimating) {
			isAnimating = true;

			firstPosition = transform.localPosition;
			targetPosition = initLocalPosition;

			// sound
			audio.Play();

			// disable makey makey
			GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ().isMakeyMakeyActive = false;

			// change texture
			gameObject.transform.GetChild(0).transform.GetChild(0).renderer.material.mainTexture = textureList[index-1];
		}
	}

	void ShowWithDelay(int index) {
		StartCoroutine("ShowDelay", index);
	}

	IEnumerator ShowDelay(int index) {
		yield return new WaitForSeconds (delay);
		
		Show (index);
	}

	void Hide() {
		if (!isAnimating) {
			isAnimating = true;

			firstPosition = transform.localPosition;
			targetPosition = hidePosition;
		}
	}
}
