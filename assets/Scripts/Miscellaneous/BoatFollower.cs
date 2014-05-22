using UnityEngine;
using System.Collections;

public class BoatFollower : MonoBehaviour
{
		private GameObject boat;
		// Use this for initialization
		void Start ()
		{
				boat = GameObject.FindGameObjectWithTag ("Player");
		}
	
		// Update is called once per frame
		void Update ()
		{
				transform.position = boat.transform.position;
		}
}
