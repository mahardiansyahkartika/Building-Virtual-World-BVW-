using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemGenerator : MonoBehaviour {

	public GameObject[] itemPrefab;
	public Vector3 offset;
	public float interve;
	public int amount_max;
	public bool enabled;

	TileController tileController;

	public enum ItemType {
		Fist,
		Wings,
		Bombs,
		NumberOfTypes
	};

	void Start() {
		tileController = GameObject.Find("GameController").GetComponent<TileController>();
		int length = GameObject.FindGameObjectsWithTag ("Tile").Length;
		StartCoroutine (Drop());
	}
	
	void DropItem() {
		int amount = Random.Range (3,amount_max);

		if(!enabled)
		{
			return;
		}

		List<GameObject> activeTiles = tileController.GetActiveTiles();

		for(int i =0 ; i < amount; i++) {
			int it = Random.Range(0, (int)ItemType.NumberOfTypes); // item type
			int tilenum = Random.Range(0, activeTiles.Count);
			TileItem tileItem = activeTiles[tilenum].GetComponent<TileItem>();
			if(tileItem.HasItem())
			{
				return;
			}

			GameObject item = Instantiate(itemPrefab[it], activeTiles[tilenum].transform.position + offset, Quaternion.identity) as GameObject;
			tileItem.SetHasItem(true);
			ItemController itemController = item.AddComponent("ItemController") as ItemController;
			itemController.SetTileItem(tileItem);
			item.SendMessage("SetType", it);  // item type can send int directly
			iTween.MoveTo(item, activeTiles[tilenum].transform.position + new Vector3(0,1,0), 2);
		}
	}

	IEnumerator Drop() {
		while(true) {
			float interve_imp = Random.Range(5f, 5+interve);
			DropItem();
			yield return new WaitForSeconds(1f);
			//Clean();
			yield return new WaitForSeconds(interve_imp + 5f); //item exist time
		}
	}

	public void SetEnabled(bool value)
	{
		this.enabled = value;
	}
}
