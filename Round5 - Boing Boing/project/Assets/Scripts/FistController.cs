using UnityEngine;
using System.Collections;

public class FistController : MonoBehaviour {
	public enum FistDirection {FRONT, RIGHT, BACK, LEFT};
	public FistDirection fistDirection;
	public bool isAdditionalFist = false;

	const float MAX_FREEZE_TIME = 0.5f;

	private bool isAttacking = false;
	private bool isMoving = false;
	private bool flagCheckMissed = false;

	float totalTimeMoving = 0f;
	float elapsedTimeMoving = 0f;
	float attackDistance;
	float sqrAttackDistance;
	Vector3 fistTargetPos;
	Vector3 fistOriPos;

	GameObject characterGameObject;

	TileController tileController;


	// Use this for initialization
	void Start () {
		tileController = GameObject.Find("GameController").GetComponent<TileController>();
		attackDistance = 2.4f;
		sqrAttackDistance = attackDistance * attackDistance;
		fistOriPos = transform.localPosition;

		if (isAdditionalFist) {
			characterGameObject = transform.parent.parent.gameObject;
		} else {				
			characterGameObject = transform.parent.gameObject;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (isMoving) {
			elapsedTimeMoving += Time.deltaTime;

			if (flagCheckMissed && elapsedTimeMoving >= totalTimeMoving / 2f) {
				flagCheckMissed = false;
				isAttacking = false;

				// miss animation
				characterGameObject.GetComponent<PlayerAttack>().MissAnimation(transform.position);
			}

			if (elapsedTimeMoving >= totalTimeMoving) {
				isMoving = false;
				elapsedTimeMoving = 0f;
			}

			AnimateFist();
		}
	}

	void OnTriggerStay(Collider other)
	{
		if (isAttacking) {
			if(other.tag == "Player" || other.tag == "Boundary")
			{
				// hit target
				isAttacking = false;
				flagCheckMissed = false;

				// hit animation
				characterGameObject.GetComponent<PlayerAttack>().HitAnimation(transform.position);

				totalTimeMoving = elapsedTimeMoving * 2f;
				if (totalTimeMoving == 0f) totalTimeMoving = 0.0001f; // avoid divided by zero
				fistTargetPos = transform.localPosition;

				if(other.tag == "Player")
					other.gameObject.GetComponent<PlayerAttack>().KnockedDown((other.gameObject.transform.position - characterGameObject.transform.position).normalized, characterGameObject);
				else if(other.tag == "Boundary")
				{
					tileController.BreakBoundaryTile(other.gameObject);
				}
			}
		}
	}

	void Attack() {
		if (!isMoving) {
			isMoving = true;
			isAttacking = true;
			elapsedTimeMoving = 0f;
			totalTimeMoving = MAX_FREEZE_TIME;
			flagCheckMissed = true;

			SetupFist();
		}
	}
	
	void AnimateFist()
	{
		if(elapsedTimeMoving <= totalTimeMoving * 0.5f)
		{
			transform.localPosition = Vector3.Lerp(fistOriPos, fistTargetPos, 2 * (elapsedTimeMoving / totalTimeMoving));
		}
		else
		{
			transform.localPosition = Vector3.Lerp(fistOriPos, fistTargetPos, 2 * (1 - (elapsedTimeMoving / totalTimeMoving)));
		}
	}

	void SetupFist()
	{
		Vector3 from, to;

		switch (fistDirection) {
		case FistDirection.FRONT:
			from = transform.position;
			to = characterGameObject.transform.position + characterGameObject.transform.forward * attackDistance;
			fistTargetPos = characterGameObject.transform.InverseTransformPoint(to);
			fistTargetPos = new Vector3(0f, 0f, Mathf.Max(fistTargetPos.z, fistOriPos.z));
			break;
		case FistDirection.RIGHT:
			from = transform.position;
			to = characterGameObject.transform.position + ((characterGameObject.transform.right * attackDistance) / 2f);
			fistTargetPos = characterGameObject.transform.InverseTransformPoint(to);
			fistTargetPos = new Vector3(Mathf.Max(fistTargetPos.x, fistOriPos.x), 0f, 0f);
			break;
		case FistDirection.BACK:
			from = transform.position;
			to = characterGameObject.transform.position + ((-characterGameObject.transform.forward * attackDistance) / 2f);
			fistTargetPos = characterGameObject.transform.InverseTransformPoint(to);
			fistTargetPos = new Vector3(Mathf.Min(fistTargetPos.z, fistOriPos.z), 0f, 0f);
			break;
		case FistDirection.LEFT:
			from = transform.position;
			to = characterGameObject.transform.position + ((-characterGameObject.transform.right * attackDistance) / 2f);
			fistTargetPos = characterGameObject.transform.InverseTransformPoint(to);
			fistTargetPos = new Vector3(Mathf.Min(fistTargetPos.x, fistOriPos.x), 0f, 0f);
			break;
		}

		Vector3 dir = (to - from).normalized;
		dir.y = 0;
		transform.localPosition = fistOriPos;
	}
}