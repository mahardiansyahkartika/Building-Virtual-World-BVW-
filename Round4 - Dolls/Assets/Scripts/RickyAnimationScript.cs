using UnityEngine;
using System.Collections;

public class RickyAnimationScript : MonoBehaviour {

	public Texture2D[] rickies;
	public Texture2D original;
	int num;
	bool startwalk = false;
	// Use this for initialization
	void Start () {
		num = rickies.Length;
		//original = (Texture2D)this.renderer.material.mainTexture;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void startWalking()
	{
		startwalk = true;
		StartCoroutine (rickywalking ());
	}

	public void stopWalking()
	{
		startwalk = false;
		this.renderer.material.mainTexture = original;
	}

	IEnumerator rickywalking()
	{
		int counter = 0;
		while (true) 
		{
			if(!startwalk)
			{
				this.renderer.material.mainTexture = original;
				yield break;
			}
			yield return new WaitForSeconds(0.333f);
			this.renderer.material.mainTexture = rickies[(counter%num)];
			counter++;
			
		}
	}
}
