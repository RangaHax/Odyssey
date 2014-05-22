using UnityEngine;
using System.Collections;

public class WaveFunction : MonoBehaviour
{

		//The height of the waves
		public float amplitude = 1f;
		//The number of waves
		public float frequency = 0.1f;
		//The speed the direction of the waves rotate
		public float directionChange = 0.02f;
		//The speed that the waves travel
		public float waveSpeed = 0.25f;

		// Update is called once per frame, every frame it is rotated a small amount.
		//using transform.rotate allows for easy rotation and reference to its direction with transform.forward is automaticlly normalised.
		void FixedUpdate ()
		{
				//Rotates around the y axis
				transform.RotateAround (transform.position, Vector3.up, directionChange * Time.deltaTime);
		}	

		//Gets the height (y value) of the wave at an (x,z) position. At each frame the amplitude, time, frequency and direction are considered constant so that but these all change with between each frame.
		public float heightAt (float x, float z)//}, float offsetX, float offsetZ)
		{
				return amplitude * Mathf.Sin (Time.realtimeSinceStartup * waveSpeed + x * frequency * transform.forward.x + z * frequency * transform.forward.z);
		}
		public Vector3 normalAt (float x, float z)
		{
				//return Vector3.up;
				float argument = waveSpeed * Time.realtimeSinceStartup + x * frequency * transform.forward.x + z * frequency * transform.forward.z;
				float nx = amplitude * frequency * transform.forward.x * Mathf.Cos (argument);
				float ny = 0;//-1.0f;
				float nz = amplitude * frequency * transform.forward.z * Mathf.Cos (argument);
				return new Vector3 (nx, ny, nz).normalized * -1; // Not sure about sign. Maybe you need to multiply the result by -1.

		}
}
