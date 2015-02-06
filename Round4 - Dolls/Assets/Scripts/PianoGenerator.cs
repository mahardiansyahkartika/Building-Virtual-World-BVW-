using UnityEngine;
using System.Collections;

public class PianoGenerator : MonoBehaviour {
	
	public Object key_C_F, key_Black, key_D_G_A, key_E_H;
	
	private Object[] blockNormalKeyObjectList, blockLastKeyObjectList;
	
	//private float scale = 1.792165f;
	private float scale = 2.95f;
	private float secondaryScale = 0.7f;
	private float space = 0.016f; // space between white keys
	private int indexCurrentWhiteKey = 0;
	private int indexCurrentKey = 0;
	
	GameObject allPianoKeys;
	
	// Use this for initialization
	void Start () {
		GameObject pianoGenerator = GameObject.Find ("PianoGenerator");
		
		allPianoKeys = new GameObject ();
		allPianoKeys.name = "AllPianoKeys";
		allPianoKeys.transform.position = pianoGenerator.transform.position;
		allPianoKeys.transform.rotation = pianoGenerator.transform.rotation;
		allPianoKeys.transform.localScale = new Vector3 (scale * secondaryScale, scale * secondaryScale, scale);
		
		// from right to left
		blockNormalKeyObjectList = new Object[] {
			key_E_H,
			key_Black,
			key_D_G_A,
			key_Black,
			key_D_G_A,
			key_Black,
			key_C_F,
			key_E_H,
			key_Black,
			key_D_G_A,
			key_Black,
			key_C_F
		};
		blockLastKeyObjectList = new Object[] {
			key_E_H,
			key_Black,
			key_C_F
		};
		
		GenerateAllKeys();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	protected bool IsBlack(int index) {
		if (index == 1 || index == 3 || index == 5 || index == 8 || index == 10)
			return true;
		return false;
	}
	
	protected void GenerateAllKeys() {
		// first keys
		GenerateFirstKey ();
		// rest keys
		for (int i = 0; i < 4; i++) {
			GenerateOneBlockKey();		
		}
	}
	
	protected void GenerateOneBlockKey() {
		for (int i = 0; i < 12; i++) {
			GameObject key = GenerateOneKey (blockNormalKeyObjectList[i]);
			key.transform.localPosition = new Vector3(key.transform.localPosition.x, key.transform.localPosition.y, key.transform.localPosition.z + space * indexCurrentWhiteKey);
			
			if (IsBlack(i)) {
				key.transform.localPosition = new Vector3(key.transform.localPosition.x, key.transform.localPosition.y, key.transform.localPosition.z - space / 2f);
			} else {
				indexCurrentWhiteKey++;
			}
		}
	}
	
	protected void GenerateFirstKey() {
		GameObject key = GenerateOneKey (key_C_F);
		key.transform.localPosition = new Vector3(key.transform.localPosition.x, key.transform.localPosition.y, key.transform.localPosition.z + space * indexCurrentWhiteKey);
		indexCurrentWhiteKey++;
	}
	
	protected GameObject GenerateOneKey(Object objectSource) {		
		indexCurrentKey++;
		
		GameObject key = GameObject.Instantiate (objectSource) as GameObject;
		key.name = "piano_key_" + indexCurrentKey;
		key.transform.parent = allPianoKeys.transform;
		key.transform.localPosition = Vector3.zero;
		key.transform.localRotation = Quaternion.Euler (Vector3.zero);
		key.transform.localScale = Vector3.one;
		key.audio.clip = Resources.Load("Sound/Piano/piano (" + (50 - indexCurrentKey) + ")") as AudioClip;
		
		return key;
	}
}