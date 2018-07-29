using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	private const int MAX_ORB = 10;

	public GameObject orbPrehab;
	public GameObject canvasGame;

	// Use this for initialization
	void Start () {
		for (int i=0; i<MAX_ORB; i++) {
			CreateOrb ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void CreateOrb () {
		GameObject orb = (GameObject)Instantiate (orbPrehab);
		orb.transform.SetParent(canvasGame.transform, false);
		orb.transform.localPosition = new Vector3 (
			UnityEngine.Random.Range (-300.0f, 300.0f),
			UnityEngine.Random.Range (-140.0f, -500.0f),
			0f
		);
	}
}
