using UnityEngine;
using System.Collections;

public class WaterResistance : MonoBehaviour
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

				Vector3 velocity = rigidbody.velocity;
				float scalar;
				if (Vector3.Angle (velocity, transform.forward) < 90) {
						scalar = Mathf.Sin (Vector3.Angle (velocity, transform.forward) * Mathf.Deg2Rad);
				} else {
						scalar = Mathf.Sin (Vector3.Angle (velocity, transform.forward * -1) * Mathf.Deg2Rad);
				}
				//scalar *= *180 / Mathf.PI;

				Vector3 force = (scalar) * velocity * -1;
		
				if (waterLevel > actionPoint.y) {
						rigidbody.AddForceAtPosition (force, actionPoint);
				}
		
				if (debugSphere != null) {
						if (gameController.GetComponent<DebugScript> ().debug) {
								debugSphere.transform.position = actionPoint;
								Debug.DrawRay (actionPoint, force);
						} else {
								debugSphere.transform.position = new Vector3 (0, -100, 0);
						}
				}
		
		
		
		}
}
