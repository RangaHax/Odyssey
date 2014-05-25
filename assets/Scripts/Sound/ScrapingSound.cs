using UnityEngine;
using System.Collections;

public class ScrapingSound : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void PlaySound(){
		audio.Play ();
	}

	void StopSound(){
				audio.Stop ();
		}

}
