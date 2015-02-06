using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {
	
	private GameObject BGMusic;
	private GameObject waterTower;
	private GameObject fireTower;
	private GameObject electricTower;
	private GameObject poisonTower;
	private GameObject explosion;
	private GameObject warning;

	private int playCount = 0;
	private int[] soundSeq;

	private string musicPath = "Sounds/BGMusic/";
	private string towerPath = "Sounds/Tower/";
	private string explosionPath = "Sounds/Explosion/";
	private string warningPath = "Sounds/Warning/";

	//Music
	private string[] music = new string[] {"GameMusic", "CreditsMusic"};

	//Tower sounds
	private string[] towerSounds = new string[] {"ElectricTower", "FireTower", "PoisonTower", "WaterTower"};

	//Explosion sounds
	private string[] explosionSounds = new string[] {"LargeExplosion1", "LargeExplosion2", "MediumExplosion", "SmallExplosion1", "SmallExplosion2"};

	private string[] warningSounds = new string[] {"Beep"};
	
	void Start () 
	{
		BGMusic = transform.Find ("BGMusic").gameObject;
		waterTower = transform.Find ("WaterTower").gameObject;
		fireTower = transform.Find ("FireTower").gameObject;
		electricTower = transform.Find ("ElectricTower").gameObject;
		poisonTower = transform.Find ("PoisonTower").gameObject;
		explosion = transform.Find ("Explosion").gameObject;
		warning = transform.Find ("Warning").gameObject;
	}

	public void PlayMusic(int num)
	{
		AudioClip newClip = (AudioClip)Resources.Load(string.Concat(musicPath,music[num]), typeof(AudioClip));
		BGMusic.GetComponent<AudioSource> ().clip = newClip;
		BGMusic.GetComponent<AudioSource> ().Play ();
	}

	public void PlayWaterTowerSound()
	{
		//int num = Random.Range (0, 12);
		AudioClip newClip = (AudioClip)Resources.Load(string.Concat(towerPath,towerSounds[3]), typeof(AudioClip));
		waterTower.GetComponent<AudioSource> ().clip = newClip;
		waterTower.GetComponent<AudioSource> ().Play ();
		//Invoke("PlayZombieVox", newClip.length + Random.Range (2.0f,7.5f));
	}

	public void PlayFireTowerSound()
	{
		//int num = Random.Range (0, 12);
		AudioClip newClip = (AudioClip)Resources.Load(string.Concat(towerPath,towerSounds[1]), typeof(AudioClip));
		fireTower.GetComponent<AudioSource> ().clip = newClip;
		fireTower.GetComponent<AudioSource> ().Play ();
		//Invoke("PlayZombieVox", newClip.length + Random.Range (2.0f,7.5f));
	}

	public void PlayElectricTowerSound()
	{
		//int num = Random.Range (0, 12);
		AudioClip newClip = (AudioClip)Resources.Load(string.Concat(towerPath,towerSounds[0]), typeof(AudioClip));
		electricTower.GetComponent<AudioSource> ().clip = newClip;
		electricTower.GetComponent<AudioSource> ().Play ();
		//Invoke("PlayZombieVox", newClip.length + Random.Range (2.0f,7.5f));
	}

	public void PlayPoisonTowerSound()
	{
		//int num = Random.Range (0, 12);
		AudioClip newClip = (AudioClip)Resources.Load(string.Concat(towerPath,towerSounds[2]), typeof(AudioClip));
		poisonTower.GetComponent<AudioSource> ().clip = newClip;
		poisonTower.GetComponent<AudioSource> ().Play ();
		//Invoke("PlayZombieVox", newClip.length + Random.Range (2.0f,7.5f));
	}

	public void PlayExplosionSound()
	{
		int num = Random.Range (0, 5);
		AudioClip newClip = (AudioClip)Resources.Load(string.Concat(explosionPath,explosionSounds[num]), typeof(AudioClip));
		explosion.GetComponent<AudioSource> ().clip = newClip;
		explosion.GetComponent<AudioSource> ().Play ();
	}

	public void PlayWarningSound()
	{
		//int num = Random.Range (0, 12);
		AudioClip newClip = (AudioClip)Resources.Load(string.Concat(warningPath,warningSounds[0]), typeof(AudioClip));
		warning.GetComponent<AudioSource> ().clip = newClip;
		warning.GetComponent<AudioSource> ().Play ();
		//Invoke("PlayZombieVox", newClip.length + Random.Range (2.0f,7.5f));
	}

	/*private void testingSequence()
	{
		int[] testArray = {8,1,9};
		PlaySystemVoxSequence (testArray);
	}*/

	/*IEnumerator ScheduleThunder() 
	{
		yield return new WaitForSeconds(Random.Range (5.0f, 10.0f));
		Debug.Log("After Waiting Seconds");
		PlayThunder ();
	}*/

	/*public void PlaySystemVoxSequence(int[] seq) //eg. 1,3,5
	{
		soundSeq = seq;

		if(playCount < seq.Length) //haven't finished all clips yet
		{
			AudioClip newClip = (AudioClip)Resources.Load(string.Concat(systemVoxPath,systemLines[seq[playCount]]), typeof(AudioClip));
			voxSystem.GetComponent<AudioSource> ().clip = newClip;
			voxSystem.GetComponent<AudioSource> ().Play ();
			playCount++;
			Invoke("CheckIfSequenceDone", newClip.length);
		}
		else //all clips done, reset playcount to 0
		{
			playCount = 0;
		}
	}*/

	/*private void CheckIfSequenceDone()
	{
		PlaySystemVoxSequence (soundSeq);
	}*/

	/*public void PlayFoleySound(int num)
	{
		AudioClip newClip = (AudioClip)Resources.Load(string.Concat(foleyPath,foleySounds[num]), typeof(AudioClip));
		foley.GetComponent<AudioSource> ().clip = newClip;
		foley.GetComponent<AudioSource> ().Play ();
	}*/
}
