using UnityEngine;
using System.Collections;

public class InteractiveObjectController : MonoBehaviour
{
	public enum AnimationType
	{
		NULL,
		SHOWUP,
		GOBACK,
		FULLSCREEN
	};
	
	private AnimationType animationType = AnimationType.NULL;
	private Vector3 initPosition, firstPosition, targetPosition;
	private Quaternion initRotation, firstRotation, targetRotation;
	private Vector3 initScale, firstScale, targetScale, fullscreenScale;
	private GameObject oculusCameraRight, oculusCameraLeft;
	private Shader initShader, targetShader;
	private GameObject gameController;
	
	// animation attribute
	private bool isAnimating = false;
	private float animationTime = 0.5f;
	private float elapsedTimeAnimation = 0f;
	
	// sound attribute
	private float elapsedTimeSFX = 0f;
	private bool isWaitingSFX = false;
	private float durationSFX = 0f;
	
	public AudioClip SFX;
	public float maxScaling = 1f;
	public bool isParent = false;

	private AudioClip paperPickupSFX;
	private Transform objectTransform;

	// Use this for initialization
	void Start ()
	{
		if (isParent)
			objectTransform = transform.parent.transform;
		else
			objectTransform = transform;

		initShader = Shader.Find ("Custom/Rim");
		targetShader = Shader.Find ("Unlit/Transparent");

		// material
		for (int i = 0; i < renderer.materials.Length; i++) {
			renderer.materials[i].shader = initShader;
		}

		gameObject.AddComponent<AudioSource> ();
		audio.playOnAwake = false;
		audio.clip = Resources.Load ("Sound/First Floor/paperpickup") as AudioClip;
		paperPickupSFX = audio.clip;
		
		initRotation = objectTransform.rotation;
		initPosition = objectTransform.position;
		initScale = objectTransform.localScale;
		
		float multiplier = maxScaling / ((initScale.x > initScale.y ? initScale.x : initScale.y));
		fullscreenScale = new Vector3 (initScale.x * multiplier, initScale.y * multiplier, initScale.z * multiplier);
		
		oculusCameraRight = GameObject.Find ("CameraRight");
		oculusCameraLeft = GameObject.Find ("CameraLeft");
		gameController = GameObject.FindGameObjectWithTag ("GameController");
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (isAnimating) {
			elapsedTimeAnimation += Time.deltaTime;
			
			// animation
			float progressTime = elapsedTimeAnimation / animationTime;
			this.objectTransform.localPosition = Vector3.Slerp (firstPosition, targetPosition, progressTime);
			this.objectTransform.localRotation = Quaternion.Slerp (firstRotation, targetRotation, progressTime);
			this.objectTransform.localScale = Vector3.Slerp (firstScale, targetScale, progressTime);
			
			if (elapsedTimeAnimation >= animationTime) {
				// set as currentNote
				if (animationType == AnimationType.SHOWUP) {
					gameController.SendMessage ("SetCurrentNote", this.gameObject);
				}
				
				// play SFX
				if (animationType == AnimationType.SHOWUP)
					PlaySFX ();
				
				// init
				elapsedTimeAnimation = 0f;				
				isAnimating = false;				
				if (animationType == AnimationType.SHOWUP) {
					animationType = AnimationType.FULLSCREEN;
				} else if (animationType == AnimationType.GOBACK) {
					animationType = AnimationType.NULL;
				}
			}
		}
		
		if (isWaitingSFX) {
			elapsedTimeSFX += Time.deltaTime;
			if (elapsedTimeSFX >= durationSFX) {
				// init
				isWaitingSFX = false;
				elapsedTimeSFX = 0f;
				audio.clip = paperPickupSFX;
				SFX = null; // just once they can listen the sfx
				// enable makey makey
				GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ().isMakeyMakeyActive = true;
			}
		}
	}
	
	protected void PlaySFX ()
	{
		if (SFX != null) {
			durationSFX = SFX.length;
			isWaitingSFX = true;
			
			audio.clip = SFX;
			audio.Play ();
			
			// disable makey makey
			GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ().isMakeyMakeyActive = false;
		}
	}
	
	protected void ShowUp ()
	{
		if (!isAnimating && animationType == AnimationType.NULL) {
			isAnimating = true;
			animationType = AnimationType.SHOWUP;
			
			// change parent
			objectTransform.parent = oculusCameraRight.transform;
			
			// change shader
			for (int i = 0; i < renderer.materials.Length; i++) {
				renderer.materials[i].shader = targetShader;
			}

			// set first
			firstPosition = objectTransform.localPosition;
			firstRotation = objectTransform.localRotation;
			firstScale = objectTransform.localScale;
			
			// set target
			targetRotation = Quaternion.Euler (Vector3.zero);
			targetPosition = new Vector3 (-0.032f, 0f, 0.2f);
			targetScale = fullscreenScale;
			
			// sound
			audio.Play ();
		}
	}
	
	protected void GoBack ()
	{
		if (!isAnimating && animationType == AnimationType.FULLSCREEN) {
			isAnimating = true;
			animationType = AnimationType.GOBACK;
			
			// change parent
			objectTransform.parent = null;
			
			// change shader
			for (int i = 0; i < renderer.materials.Length; i++) {
				renderer.materials[i].shader = initShader;
			}

			// set first
			firstPosition = objectTransform.position;
			firstRotation = objectTransform.rotation;
			firstScale = objectTransform.localScale;
			
			// set target
			targetRotation = initRotation;
			targetPosition = initPosition;
			targetScale = initScale;
			
			gameController.SendMessage ("SetCurrentNoteNull");
			
			// sound
			audio.Play ();
		}
	}
	
	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Hand") {
			ShowUp ();
		}
	}
}
