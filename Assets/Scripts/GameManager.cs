using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour {

	private const int MAX_ORB = 10;
	private const int RESPAWN_TIME = 1;
	private const int MAX_LEVEL = 2;

	public GameObject orbPrehab;
	public GameObject smokePrehab;
	public GameObject kusudamaPrehab;
	public GameObject canvasGame;
	public GameObject textScore;
	public GameObject imageTemple;
	public GameObject imageMokugyo;

	public AudioClip getScoreSE;
	public AudioClip levelUpSE;
	public AudioClip clearSE;

	private int score = 0;
	private int nextScore = 10;

	private int currentOrb = 0;

	private int templeLevel = 0;

	private DateTime lastDateTime;

	private int[] nextScoreTable = new int[] {10, 100, 1000};

	private AudioSource audioSource;

	// Use this for initialization
	void Start () {
		audioSource = this.gameObject.GetComponent<AudioSource> ();

		currentOrb = MAX_ORB;
		for (int i=0; i<currentOrb; i++) {
			CreateOrb ();
		}

		// initialize
		lastDateTime = DateTime.UtcNow;
		nextScore = nextScoreTable[templeLevel];
		imageTemple.GetComponent<TempleManager> ().SetTemplePicture (templeLevel);
		imageTemple.GetComponent<TempleManager> ().SetTempleScale (score, nextScore);

		RefreshScoreText ();
	}
	
	// Update is called once per frame
	void Update () {
		if (currentOrb < MAX_ORB) {
			TimeSpan timeSpan = DateTime.UtcNow - lastDateTime;

			if (timeSpan >= TimeSpan.FromSeconds (RESPAWN_TIME)) {
				while (timeSpan >= TimeSpan.FromSeconds (RESPAWN_TIME)) {
					CreateNewOrb ();
					timeSpan -= TimeSpan.FromSeconds (RESPAWN_TIME);
				}
			}
		}
	}

	public void CreateNewOrb () {
		lastDateTime = DateTime.UtcNow;
		if (currentOrb >= MAX_ORB) {
			return;
		}
		CreateOrb ();
		currentOrb++;
	}

	public void CreateOrb () {
		GameObject orb = (GameObject)Instantiate (orbPrehab);
		orb.transform.SetParent(canvasGame.transform, false);
		orb.transform.localPosition = new Vector3 (
			UnityEngine.Random.Range (-300.0f, 300.0f),
			UnityEngine.Random.Range (-140.0f, -500.0f),
			0f
		);

		int kind = UnityEngine.Random.Range (0, templeLevel + 1);
		switch (kind) {
		case 0:
				orb.GetComponent<OrbManager> ().SetKind (OrbManager.ORB_KIND.BLUE);
				break;
		case 1:
				orb.GetComponent<OrbManager> ().SetKind (OrbManager.ORB_KIND.GREEN);
				break;
		case 2:
				orb.GetComponent<OrbManager> ().SetKind (OrbManager.ORB_KIND.PURPLE);
				break;
		}
	}

	public void GetOrb (int getScore) {
		audioSource.PlayOneShot (getScoreSE);

		AnimatorStateInfo stateInfo = imageMokugyo.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0);
		if (stateInfo.fullPathHash == Animator.StringToHash("Base Layer.get@ImageMokugyo")) {
			imageMokugyo.GetComponent<Animator> ().Play (stateInfo.fullPathHash, 0, 0.0f);
		} else {
			imageMokugyo.GetComponent<Animator> ().SetTrigger ("isGetScore");
		}

		if (score < nextScore) {
			score += getScore;

			// Prevent count up over the border of level up.
			if (score > nextScore) {
				score = nextScore;
			}

			TempleLevelUp();
			RefreshScoreText ();
			imageTemple.GetComponent<TempleManager> ().SetTempleScale (score, nextScore);

			if ((score == nextScore) && (templeLevel == MAX_LEVEL)) {
				ClearEffect ();
			}
		}

		currentOrb--;
	}

	void RefreshScoreText () {
		textScore.GetComponent<Text> ().text = "徳 : " + score + " / " + nextScore;
	}

	void TempleLevelUp () {
		if (score >= nextScore) {
			if (templeLevel < MAX_LEVEL) {
				templeLevel++;
				score = 0;

				TempleLevelUpEffect ();

				nextScore = nextScoreTable[templeLevel];
				imageTemple.GetComponent<TempleManager> ().SetTemplePicture(templeLevel);
			}
		}
	}

	void TempleLevelUpEffect () {
		GameObject smoke = (GameObject)Instantiate (smokePrehab);
		smoke.transform.SetParent (canvasGame.transform, false);
		smoke.transform.SetSiblingIndex(2);
		
		audioSource.PlayOneShot (levelUpSE);

		Destroy (smoke, 0.5f);
	}

	void ClearEffect () {
		GameObject kusudama = (GameObject)Instantiate (kusudamaPrehab);
		kusudama.transform.SetParent (canvasGame.transform, false);
		
		audioSource.PlayOneShot (clearSE);
	}
}
