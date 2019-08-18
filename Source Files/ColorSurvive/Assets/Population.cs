using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Population : MonoBehaviour {

	public int pop_sze = 1000;
	public GameObject environment;
	protected List<MeeMee> pop = new List<MeeMee>();






	// Use this for initialization
	void Start () {
		
		Bounds boundaries = environment.GetComponent<Renderer> ().bounds;

		for (int i = 0; i < pop_sze; i++) {
			MeeMee meemee = CreateMeemee (boundaries);
			pop.Add (meemee);


		}



		StartCoroutine(EvalulationLoop());


	}

	public MeeMee CreateMeemee(Bounds bounds){

		Vector3 rndPos = new Vector3 (UnityEngine.Random.Range(-0.5f,0.5f)*bounds.size.x,UnityEngine.Random.Range(-0.5f,0.5f)*bounds.size.y,UnityEngine.Random.Range(-0.5f,0.5f)*bounds.size.z );

		Vector3 WrldPos = environment.transform.position + rndPos;

		GameObject temp = GameObject.CreatePrimitive (PrimitiveType.Capsule);
		MeeMee meemee = temp.AddComponent<MeeMee>();

		float height = temp.GetComponent<MeshFilter> ().mesh.bounds.size.y;

		WrldPos.y += height / 2;


		temp.transform.position = WrldPos;

		RndCol(temp);

		return meemee;

	}
	public void RndCol(GameObject meemee){


		meemee.GetComponent<MeeMee> ().Setcolor (new Color(UnityEngine.Random.Range (0.0f, 1.0f), UnityEngine.Random.Range (0.0f, 1.0f), UnityEngine.Random.Range (0.0f, 1.0f)));
	}

	float EvalulateFitness(Color environment, Color meemee){

		float Fitness = (new Vector3 (environment.r, environment.g, environment.b) - 
			new Vector3 (meemee.r, meemee.g, meemee.b)).magnitude;
		return Fitness;
	}

	void EvalulatePopulation(){
		
		//fitness
		for (int i = 0; i < pop.Count; i++) {

			float fitness = EvalulateFitness (environment.GetComponent<MeshRenderer>().material.color, 
				pop[i].GetComponent<MeshRenderer>().material.color);

			pop[i].fitnessScore = fitness;

		}
		// sort fitness 
		pop.Sort(

			delegate ( MeeMee meemee1, MeeMee meemee2) {
				
				if (meemee1.fitnessScore > meemee2.fitnessScore)
					return 1;
				else if (meemee1.fitnessScore == meemee2.fitnessScore)
					return 0;
				else
					return -1;

			}); 
		// kill meemee
		int halfwayMark = (int)pop.Count / 2;

		if (halfwayMark % 2 != 0)
			halfwayMark++;

		for (int i = halfwayMark; i < pop.Count; i++) {

			Destroy (pop [i].gameObject);
			pop [i] = null;

		}
		pop.RemoveRange (halfwayMark, pop.Count - halfwayMark); 

		// breed meemee
		Breed ();


				
		}

	void Breed(){
		List<MeeMee> tempList = new List<MeeMee>();

			for (int i = 1; i < pop.Count ; i = i + 2) {

				int breederIndex1 = i - 1;
				int breederIndex2 = i;

			float split = UnityEngine.Random.Range (0.0f, 1.0f);



				Bounds bounds = environment.GetComponent<Renderer> ().bounds;

				MeeMee childMe1 = CreateMeemee (bounds);
				MeeMee childMe2 = CreateMeemee (bounds);

			tempList.Add (childMe1);
			tempList.Add (childMe2);

				if (split <= 0.16f) { // 100/6 
				        

				Color tempC = new Color (pop [breederIndex1].color.r,
					              pop [breederIndex1].color.g,
					              pop [breederIndex2].color.b);
				childMe1.Setcolor (Mut(tempC));


			
				    tempC = new Color (pop [breederIndex1].color.r,
					pop [breederIndex2].color.g,
					pop [breederIndex1].color.b);

				childMe2.Setcolor (Mut(tempC));



				} else if (split <= 0.32f) {

				Color tempC = new Color (pop [breederIndex1].color.r,
					pop [breederIndex2].color.g,
					pop [breederIndex1].color.b);
				childMe1.Setcolor (Mut(tempC));
				tempC = new Color (pop [breederIndex2].color.r,
					pop [breederIndex1].color.g,
					pop [breederIndex2].color.b);

				childMe2.Setcolor (Mut(tempC));
				} else if (split <= 0.48f) {

				Color tempC = new Color (pop [breederIndex2].color.r,
					              pop [breederIndex1].color.g,
					              pop [breederIndex1].color.b);
				childMe1.Setcolor (Mut(tempC));
				tempC = new Color (pop [breederIndex2].color.r,
					pop [breederIndex1].color.g,
					pop [breederIndex1].color.b);
				childMe2.Setcolor (Mut(tempC));
				} else if (split <= 0.64f) {
					
				Color tempC = new Color (
					              pop [breederIndex2].color.r,
					              pop [breederIndex1].color.g,
					              pop [breederIndex1].color.b);
				childMe1.Setcolor (Mut(tempC));
				tempC = new Color (pop [breederIndex1].color.r,
					pop [breederIndex1].color.g,
					pop [breederIndex2].color.b);
				childMe2.Setcolor (Mut(tempC));
				} else if (split <= 0.8f) {
					
				Color tempC = new Color (pop [breederIndex2].color.r,
					              pop [breederIndex2].color.g,
					              pop [breederIndex1].color.b);
				childMe1.Setcolor (Mut(tempC));
				tempC = new Color (pop [breederIndex1].color.r,
					pop [breederIndex1].color.g,
					pop [breederIndex2].color.b);
				childMe2.Setcolor (Mut(tempC));
				} else {
					
				Color tempC = new Color (pop [breederIndex2].color.r,
					              pop [breederIndex1].color.g,
					              pop [breederIndex2].color.b);
				childMe1.Setcolor (Mut(tempC));
				tempC = new Color (pop [breederIndex1].color.r,
					pop [breederIndex2].color.g,
					pop [breederIndex1].color.b);

				childMe2.Setcolor (Mut(tempC));
				}



			}
			pop.AddRange (tempList);



	}
		
		
		
	public Color setColour(Color meemee){

		Vector3 setcol = new Vector3 (meemee.r, meemee.g, meemee.b);
		return new Color (setcol.x,setcol.y,setcol.z);


	}
	public Color Mut(Color meemee)
	{

		float rate = 0.1f;

		Vector3 mutcol = new Vector3 (meemee.r, meemee.g, meemee.b);

		for (int i = 0; i < 3; i++) {

			if (UnityEngine.Random.Range (0.0f, 1.0f) <= rate) {

				mutcol [i] = UnityEngine.Random.Range (0.0f, 1.0f);
			}
		}


		return new Color (mutcol.x,mutcol.y,mutcol.z);
	}



	IEnumerator EvalulationLoop(){

		while (true) 
		{
			
			yield return new WaitForSeconds(1.0f);
			EvalulatePopulation ();



		}

	}


}
