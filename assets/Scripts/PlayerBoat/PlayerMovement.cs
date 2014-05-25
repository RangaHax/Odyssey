using UnityEngine;
using System.Collections;

/*
 * Odyssey Game
 * 
 * Player Movement
 * Version: 0.13
 * Author: Thomas Auberson
 * 
 * This script handles the movement controls of the player avatar
 * 
 * Currently player movement is at a fixed forward velocity and turning is controlled by horizontal input axis
 */

public class PlayerMovement : MonoBehaviour
{
	
	//public float turnAcceleration = 0.1; // Rate at which turning accelerates
	public float turnRate = 1.0f; // Rate at which character turns
	public float defaultMoveSpeed = 1.0f; // Default speed at which character moves
	public float defaultAcceleration = 0.001f; // Default rate at which the character gradually accelerates throughout game
	
	public float currentThrust = 0.01f;
		
	// When player is dead
	private bool sinking = false;
	
	private GameObject gameController;
	
	
	
	
	// Use this for initialization
	void Start ()
	{
		gameController = GameObject.FindGameObjectWithTag (Tags.gameController);
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (sinking) {
			return;
		}
		
		float hInput = Input.GetAxis ("Horizontal"); 
		//float vInput = Input.GetAxis ("Vertical");
		
		// Roatation
		float turnAmount = hInput * turnRate;
		rigidbody.AddForceAtPosition (-transform.right * turnAmount, transform.position + transform.forward * -3);
		//transform.Rotate (0, turnAmount, 0);
		
		// Movement
		// Currently movement is at a fixed speed
		currentThrust += defaultAcceleration;
		//transform.Translate (0, 0, currentSpeed);
		rigidbody.AddForce (transform.forward * currentThrust);
		
		if (gameController.GetComponent<DebugScript> ().debug) {
			Debug.DrawRay (transform.position, transform.forward * currentThrust);
		}
	}
	
	// When Player collides with another game object...
	void OnTriggerEnter (Collider other)
	{		
		// Collision with Obstacle
		if (other.gameObject.tag.Equals ("Obstacle")) {
			Debug.Log ("PLAYER HIT");
			gameObject.GetComponent<PlayerController> ().InflictDamage ((int)currentThrust);
		}
	}
	
	public float GetCurrentSpeed ()
	{
		return currentThrust;
	}
	
	public string GetCurrentSpeedDisplay ()
	{
		return currentThrust.ToString ("F1");
	}
	
	public void SetSinking ()
	{
		sinking = true;
	}
}
