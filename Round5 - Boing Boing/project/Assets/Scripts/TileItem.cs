using UnityEngine;
using System.Collections;

public class TileItem : MonoBehaviour {

	bool hasItem;

	public bool HasItem()
	{
		return hasItem;
	}

	public void SetHasItem(bool value)
	{
		hasItem = value;
	}
}
