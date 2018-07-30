﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using DG.Tweening;

public class OrbManager : MonoBehaviour {

	private GameObject gameManager;

	public Sprite[] orbPictures = new Sprite[3];

	public enum ORB_KIND {
		BLUE,
		GREEN,
		PURPLE,
	}

	private ORB_KIND orbKind;

	// Use this for initialization
	void Start () {
		gameManager = GameObject.Find("GameManager");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void AddOrbPoint () {
		switch (orbKind) {
		case ORB_KIND.BLUE:
			gameManager.GetComponent<GameManager> ().GetOrb (1);
			break;
		case ORB_KIND.GREEN:
			gameManager.GetComponent<GameManager> ().GetOrb (5);
			break;
		case ORB_KIND.PURPLE:
			gameManager.GetComponent<GameManager> ().GetOrb (30);
			break;
		}
		Destroy (this.gameObject);
	}

	public void SetKind (ORB_KIND kind) {
		orbKind = kind;

		switch (orbKind) {
		case ORB_KIND.BLUE:
			GetComponent<Image> ().sprite = orbPictures[0];
			break;
		case ORB_KIND.GREEN:
			GetComponent<Image> ().sprite = orbPictures[1];
			break;
		case ORB_KIND.PURPLE:
			GetComponent<Image> ().sprite = orbPictures[2];
			break;
		}
	}
}
