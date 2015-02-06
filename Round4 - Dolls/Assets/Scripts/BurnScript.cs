using UnityEngine;
using System.Collections;

public class BurnScript : MonoBehaviour {
    public float delayBeforeBurn = 22;
    public float BurnTime = 7.0f;
    public ParticleSystem party;
	// Use this for initialization
	void Start () {
        party.Stop();
        StartCoroutine(burnbaby());
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    IEnumerator burnbaby()
    {
        yield return new WaitForSeconds(delayBeforeBurn);
        party.Play();
        Vector3 pos = party.gameObject.transform.localPosition;
        for (float i = 0; i < BurnTime; i += Time.deltaTime)
        {
            Debug.Log("Boooo");
            this.renderer.material.SetFloat("_BurnAmount", i / BurnTime);
            //pos.x -= i / (BurnTime * 300);
            float step = (Time.deltaTime) / BurnTime;
            pos.y += (step * 0.70f);
            //party.startSize += (step * 0.05f);
            party.startLifetime += (step * 4.0f);
            party.startSpeed += (step * 0.03f);
            party.gameObject.transform.localPosition = pos;
            yield return null;
        }
        party.Stop();
        this.renderer.enabled = false;

    }
}
