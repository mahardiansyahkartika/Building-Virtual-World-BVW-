using UnityEngine;
using System.Collections;

public class EnemyGeneratorCtrl : MonoBehaviour {

	public Transform[] enemyList;
	public Transform enemyContainer;
	public float generateRange = 80;
	public float generateDelay = 10;
	public Transform characterA;

	private float generateTime = 0;
	private Transform _transform;

	// Use this for initialization
	void Start () {
		_transform = transform;
	}
	
	// Update is called once per frame
	void Update () {
		if(Vector3.Distance(_transform.position, characterA.position)<generateRange && generateTime<Time.time){
			//generate a random enemy
			float factor = Random.Range(0f,1f);
			if(factor < 0.5f){
				//generate normal eagle
				Transform newEnemy = Instantiate(enemyList[0], _transform.position, Quaternion.identity) as Transform;
				newEnemy.parent = enemyContainer;
			}else if(factor < 0.75f){
				//generate ice eagle
				Transform newEnemy = Instantiate(enemyList[1], _transform.position, Quaternion.identity) as Transform;
				newEnemy.parent = enemyContainer;
			}else{
				//generate fire eagle
				Transform newEnemy = Instantiate(enemyList[2], _transform.position, Quaternion.identity) as Transform;
				newEnemy.parent = enemyContainer;
			}
			generateTime = Time.time + generateDelay;
		}
	}

	public void OnDrawGizmos(){
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, generateRange);
	}

}
