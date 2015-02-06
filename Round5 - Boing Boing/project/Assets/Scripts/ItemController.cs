using UnityEngine;
using System.Collections;

public class ItemController : ItemGenerator {
	ItemType itc;
	float existTime = 5f;

	public TileItem tileItem; // tile in which this item exists

	void Start () {
		collider.isTrigger = true;
		StartCoroutine (BlinkToDestroy());
	}

	void Update () {
		transform.Rotate (transform.up);
	}

	void SetType(ItemType it) {
		itc = it;
	}

	public void SetTileItem(TileItem value)
	{
		tileItem = value;
	}

	void OnTriggerEnter(Collider c) {
		if(c.tag == "Player" && !c.collider.isTrigger) {
			GameObject.Find ("GameController").GetComponent<SoundController>().PlaySound("itempickup",0.8f, false);
			Destroy(gameObject);
			switch(itc) 
			{
			case ItemType.Wings:
				ActivateWings(c);
				break;
			case ItemType.Fist:
				Fist(c);
				break;
			case ItemType.Bombs:
				if(!c.GetComponent<CharacterBaseController>().hasBomb) {
					AttachBomb(c);
				}
				break;
			}
		}
	}

	void ActivateWings(Collider c){
		//print ("activate wings on " + c.name);
		c.gameObject.GetComponent<CharacterBaseController>().ActivateFlying();
		Destroy (gameObject, 0f);
	}

	void AttachBomb(Collider c){
		if(!c.gameObject.GetComponent<CharacterBaseController>().hasBomb) {
			c.gameObject.GetComponent<CharacterBaseController>().AttachBomb(c.gameObject);
			Destroy (gameObject, 0f);
		}
	}

	void Fist(Collider c){
		c.gameObject.GetComponent<PlayerAttack>().ShowMultipleFist();
	}

	IEnumerator BlinkToDestroy() {
		yield return new WaitForSeconds(existTime);
		StartCoroutine(Blink ());

	}

	IEnumerator Blink() {
		for(int i = 0; i < 5; i++) {
			//gameObject.GetComponentsInChildren<MeshRenderer>().enabled = false;
			foreach(MeshRenderer mr in gameObject.GetComponentsInChildren<MeshRenderer>()) {
				mr.enabled = false;
			}
			yield return new WaitForSeconds(0.1f);
			//gameObject.GetComponentsInChildren<MeshRenderer>().enabled = true;
			foreach(MeshRenderer mr in gameObject.GetComponentsInChildren<MeshRenderer>()) {
				mr.enabled = true;
			}
			yield return new WaitForSeconds(0.1f);
		}

		tileItem.SetHasItem(false);
		Destroy (gameObject);
	}
}
