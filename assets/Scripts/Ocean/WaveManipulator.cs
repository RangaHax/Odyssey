using UnityEngine;
using System.Collections;

public class WaveManipulator : MonoBehaviour
{
		private Weather waveGenerator;
		// Use this for initialization
		void Start ()
		{
				waveGenerator = GameObject.FindGameObjectWithTag (Tags.gameController).GetComponent<Weather> ();
		}
	
		// Update is called once per frame
		void Update ()
		{
				Mesh mesh = GetComponent<MeshFilter> ().mesh;
				Vector3[] vertices = mesh.vertices;
				int i = 0;
				while (i < vertices.Length) {
						Vector3 worldPostion = waveGenerator.heightAt (transform.TransformPoint (vertices [i]));
						vertices [i] = transform.InverseTransformPoint (worldPostion);
						
						
						//vertices [i] = new Vector3 (vertices [i].x, vertices [i].y, waveGenerator.heightAt (worldPostion .x, worldPostion .z));
						//vertices [i] = new Vector3 (vertices [i].x, vertices [i].y, waveGenerator.heightAt (vertices [i].x, vertices [i].y));
						i++;
				}
				mesh.vertices = vertices;
		}
}
