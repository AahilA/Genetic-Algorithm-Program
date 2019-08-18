using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour {
	Material backgroundMat;


	// Use this for initialization
	void Start () {

		backgroundMat = gameObject.GetComponent<Renderer> ().material;
		StartCoroutine("CycleColors");

	}
	
	IEnumerator CycleColors(){
		Vector3 previousCol = new Vector3(backgroundMat.color.r,backgroundMat.color.g,backgroundMat.color.b);
		Vector3 currCol = previousCol;

		float colTime = 20f;

		while (true) {

			Vector3 newCol = new Vector3 (UnityEngine.Random.Range (0.0f, 1.0f), UnityEngine.Random.Range (0.0f, 1.0f), UnityEngine.Random.Range (0.0f, 1.0f));

				
			Vector3 deltaCol = (newCol - previousCol) / colTime;

			while ((newCol - currCol).magnitude > 0.1f) {

				currCol = currCol + deltaCol * Time.deltaTime;

				backgroundMat.color = new Color (currCol.x, currCol.y, currCol.z);
				yield return null;
					


			}
			previousCol = newCol;
		}
	}
}
