using UnityEngine;
using System.Collections;

public class WaveRiding : MonoBehaviour
{
		public float temp;
		//public float waterLevel;
		public float floatHeight;
		public Vector3 buoyancyCentreOffset;
		public float bounceDamp;

		public GameObject debugSphere;

		public Weather waveGenerator;
		public GameObject gameController;

		//private int numberOfForces;
		// Use this for initialization
		void Start ()
		{
				waveGenerator = GameObject.FindGameObjectWithTag (Tags.gameController).GetComponent<Weather> ();
				//numberOfForces = GetComponents<Buoyancy> ().Length;
				gameController = GameObject.FindGameObjectWithTag (Tags.gameController);

		}
	
		// Update is called once per frame
		void FixedUpdate ()
		{
				Vector3 actionPoint = transform.position + transform.TransformDirection (buoyancyCentreOffset);
		
				float waterLevel = waveGenerator.heightAt (actionPoint .x, actionPoint .z);
		
				float forceFactor = 1f - ((actionPoint.y - waterLevel) / floatHeight);


				Vector3 uplift = waveGenerator.normalAt (actionPoint.x, actionPoint.z) * (forceFactor - rigidbody.velocity.y * bounceDamp);
				if (forceFactor > 0f) {
						rigidbody.AddForceAtPosition (uplift * temp, actionPoint);
				}
				if (debugSphere != null) {
						if (gameController.GetComponent<DebugScript> ().debug) {
								debugSphere.transform.position = actionPoint;
								Debug.DrawRay (actionPoint, uplift * temp);
						} else {
								debugSphere.transform.position = new Vector3 (0, -100, 0);
						}
				}
		}

		void Update ()
		{

		}
}
