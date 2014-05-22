using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour {

		public float speed;
		public GameObject camera;
	
		// Update is called once per frame
		void Update () {
			camera.transform.Rotate(0,Time.deltaTime*speed,0);
			if(Input.GetKeyDown("space")){
			    Application.LoadLevel("Scene1");
			}
		}
	}
