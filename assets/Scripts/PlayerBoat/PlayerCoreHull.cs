using UnityEngine;
using System.Collections;

/*
 * Odyssey Game
 * 
 * Player Core Hull
 * Version: 0.10
 * Author: Thomas Auberson
 * 
 * This script handles a small internal collider inside the player's main collider. 
 * This collider is very narrow so should only be triggered by head on collisions.
 * As such it is used to detect head on collisions as an instant death trigger.
 * 
 */


public class PlayerCoreHull : MonoBehaviour {

	private GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Core Hull collides with another game object...
	void OnTriggerEnter(Collider other) {
		// Collision with Obstacle
		if (other.gameObject.tag.Equals ("Obstacle")) {
			Debug.Log("CORE HULL HIT!");
			player.GetComponent<PlayerController>().KillPlayer();
		}
	}
}
