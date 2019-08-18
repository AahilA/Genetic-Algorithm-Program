using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Genome {
	public List<int> bitz;
	public double fitness;

	public Genome() {
		Initialize ();
	}

	public Genome(int numBits) {
		Initialize ();

		for (int i = 0; i < numBits; i++) {
			System.Random rnd = new System.Random ();

			bitz.Add (rnd.Next (0, 1));
		}
	}

	private void Initialize() {
		fitness = 0;
		bitz = new List<int> ();
	}
}