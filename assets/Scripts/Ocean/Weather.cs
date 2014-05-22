using UnityEngine;
using System.Collections;

public class Weather : MonoBehaviour
{	

		//public WaveFunction wave1;
		//public WaveFunction wave2;


		//Gets the height (y value) of the wave at an (x,z) position. At each frame the amplitude, time, frequency and direction are considered constant so that but these all change with between each frame.
		public float heightAt (float x, float z)
		{
				float waveHeight = 0f;
				foreach (GameObject wave in GameObject.FindGameObjectsWithTag(Tags.waveFunction)) {
						waveHeight += wave.GetComponent<WaveFunction> ().heightAt (x, z);//, transform.forward.x, transform.forward.z);
				}
				return waveHeight;
				/*float waveHeight1 = wave1.heightAt (x, z, transform.forward.x, transform.forward.z);
				float waveHeight2 = wave2.heightAt (x, z, transform.forward.x, transform.forward.z);
				return waveHeight1 + waveHeight2;*/
				//return amplitude * Mathf.Sin (Time.realtimeSinceStartup * waveSpeed + x * frequency * transform.forward.x + z * frequency * transform.forward.z);
		}

		public Vector3 heightAt (Vector3 worldPosition)
		{
				return new Vector3 (worldPosition.x, heightAt (worldPosition.x, worldPosition.z), worldPosition.z);
		}

		//This needs to use the same function as the heightAt(x, z) method so that the normal at a point can be calculated e.g. the normal at a point on top of the wave would be straight up
		public Vector3 normalAt (float x, float z)
		{
				Vector3 normal = new Vector3 ();
				foreach (GameObject wave in GameObject.FindGameObjectsWithTag(Tags.waveFunction)) {
						WaveFunction function = wave.GetComponent<WaveFunction> ();
						normal += function.normalAt (x, z) * function.amplitude;//, transform.forward.x, transform.forward.z);
				}
				return normal;

				//return wave1.normalAt (x, z) * wave1.amplitude + wave2.normalAt (x, z) * wave2.amplitude;
				//return Vector3.up;
				//float argument = Time.realtimeSinceStartup + x * frequency * transform.forward.x + z * frequency * transform.forward.z;
				//float nx = amplitude * frequency * transform.forward.x * Mathf.Cos (argument);
				//float ny = 0;//-1.0f;
				//float nz = amplitude * frequency * transform.forward.z * Mathf.Cos (argument);
				//return new Vector3 (nx, ny, nz).normalized * -1; // Not sure about sign. Maybe you need to multiply the result by -1.

		}
}
