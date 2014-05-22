using UnityEngine;
using System.Collections;

/*
 * Odyssey Game
 * 
 * World Tile
 * Version: 0.1
 * Author: Thomas Auberson
 * 
 * This script controls the collider for world tiles that detect when player enters.
 * The collider then reports detection of player to the tile controller.
 */

public class WorldTile : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	// When Tile is entered
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag.Equals ("Player")) {
			GameObject.FindGameObjectWithTag ("GameController").GetComponent<TileController> ().TileEntryAt (transform.position.x, transform.position.z);
		}
	}
}
