using UnityEngine;
using System.Collections;

public class CharacterACtrl : MonoBehaviour {

	enum State {IDLE, FLAP};
	private State stateAnimation = State.FLAP;

	public Animator animator;

	private Vector2 flappingTimeRange = new Vector2 (3f, 5f);
	private Vector2 idleTimeRange = new Vector2 (1f, 3f);

	private float elapsedTime;
	private float flappingTime;
	private float idleTime;

	// Use this for initialization
	void Start () {
		elapsedTime = 0f;
		flappingTime = Random.Range (flappingTimeRange.x, flappingTimeRange.y);
		idleTime = Random.Range (idleTimeRange.x, idleTimeRange.y);
	}
	
	// Update is called once per frame
	void Update () {
		elapsedTime += Time.deltaTime;

		if (stateAnimation == State.FLAP) {
			if (elapsedTime >= flappingTime) {
				stateAnimation = State.IDLE;
				idleTime = Random.Range (idleTimeRange.x, idleTimeRange.y);
				elapsedTime = 0f;
				animator.SetBool ("isFlap", false);
			}
		} else if (stateAnimation == State.IDLE) {
			if (elapsedTime >= idleTime) {
				stateAnimation = State.FLAP;
				flappingTime = Random.Range (flappingTimeRange.x, flappingTimeRange.y);
				elapsedTime = 0f;
				animator.SetBool ("isFlap", true);
			}
		}
	}
}
