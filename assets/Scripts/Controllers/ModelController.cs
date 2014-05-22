using UnityEngine;
using System.Collections;

/*
 * Odyssey Game
 * 
 * Model Controller
 * Version: 0.10
 * Author: Thomas Auberson
 * 
 * This script handles animation related effects.
 * 
 * Currently this includes setting the player to sinking 
 */

public class ModelController : MonoBehaviour {

	// Death
	public float sinkSpeed = -0.1f;	
	private bool sinking;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (sinking) {			
			transform.Translate (0, sinkSpeed, 0);
			return;
		}
	}

	public void SetSinking(){
		sinking = true;
	}
}
