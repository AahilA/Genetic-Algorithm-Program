using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeeMee : MonoBehaviour {


	public float fitnessScore = 1.0f;
	public Color color = new Color(1f,1f,1f);
	public Material material;




	// Use this for initialization
	void Awake () {
		material = gameObject.GetComponent<Renderer> ().material;
	}

	public void Setcolor(Color color)
	{
		this.color = color;
		material.color = color;


	}

}
