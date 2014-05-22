using UnityEngine;
using System.Collections;

public class Buoyancy : MonoBehaviour
{
		//public float waterLevel;
		public float floatHeight;
		public Vector3 buoyancyCentreOffset;
		public float bounceDamp;
		//Debug sphere to see the position that the forces are applied
		public GameObject debugSphere;
	
		private Weather waveGenerator;
		private int numberOfForces;
		private GameObject gameController;
		
		// Use this for initialization
		void Start ()
		{
				waveGenerator = GameObject.FindGameObjectWithTag (Tags.gameController).GetComponent<Weather> ();
				numberOfForces = GetComponents<Buoyancy> ().Length;
				gameController = GameObject.FindGameObjectWithTag (Tags.gameController);
		}
	
		// Update is called once per frame
		void FixedUpdate ()
		{
				Vector3 actionPoint = transform.position + transform.TransformDirection (buoyancyCentreOffset);

				float waterLevel = waveGenerator.heightAt (actionPoint .x, actionPoint .z);
				
				float forceFactor = 1f - ((actionPoint.y - waterLevel) / floatHeight);


				Vector3 uplift = -Physics.gravity * (forceFactor - rigidbody.velocity.y * bounceDamp);
				if (forceFactor > 0f) {
						rigidbody.AddForceAtPosition (uplift / (numberOfForces + 1), actionPoint);
				}

				if (debugSphere != null) {
						if (gameController.GetComponent<DebugScript> ().debug) {
								debugSphere.transform.position = actionPoint;
								Debug.DrawRay (actionPoint, uplift / (numberOfForces + 1));
						} else {
								debugSphere.transform.position = new Vector3 (0, -100, 0);
						}
				}


				
		}
}
