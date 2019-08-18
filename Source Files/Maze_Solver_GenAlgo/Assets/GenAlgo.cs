using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class GenAlgo{

	public List<Genome> genomes;
	public List<Genome> lastGenerationGenomes;



	public int populationSize = 150;
	public double crossoverRate = 0.7f;
	public double mutationRate = 0.001f;
	public int chromosomeLength = 70;
	public int geneLength = 2;
	public int fittestGenome;
	public double bestFitnessScore;
	public double totalFitnessScore;
	public int generation;
	public Maze maze;
	public Maze mazeview;
	public bool busy;


	public GenAlgo() {
		busy = false;
		genomes = new List<Genome> ();
		lastGenerationGenomes = new List<Genome> ();
	}

	public void Run() {
		
		CreateStartPopulation ();
		busy = true;
	}

	public void CreateStartPopulation() {
		genomes.Clear ();

		for (int i = 0; i < populationSize; i++) {
			Genome baby = new Genome (chromosomeLength);
			genomes.Add (baby);
		}
	}

	public void UpdateFitness(){

		fittestGenome = 0;
		bestFitnessScore = 0;
		totalFitnessScore = 0;

		for (int i = 0; i < populationSize; i++) {
			List<int> directions = Decode (genomes [i].bitz);

			genomes [i].fitness = maze.TestRoute (directions);

			totalFitnessScore += genomes [i].fitness;

			if (genomes [i].fitness > bestFitnessScore) {
				bestFitnessScore = genomes [i].fitness;
				fittestGenome = i;

			}
				if (genomes [i].fitness == 1) {
					busy = false; // stop the run

					return;

			}
		}


	}

	public List<int> Decode(List<int> bitz) {
		List<int> directions = new List<int> ();

		for (int geneIndex = 0; geneIndex < bitz.Count; geneIndex += geneLength) {
			List<int> gene = new List<int> ();

			for (int bitIndex = 0; bitIndex < geneLength; bitIndex++) {
				gene.Add (bitz [geneIndex + bitIndex]);
			}

			directions.Add (BinaryToInt (gene));
		}
		return directions;
	}

	public int BinaryToInt(List<int> gene){

		int number = 0;
		int pOf2 = 1;

		for (int i = gene.Count; i > 0; i--) {
			number += gene [i - 1] * pOf2;
			pOf2 *= 2;
		}
		return number;

	}

	public Genome RouletteWheelSelection() {
		double r = UnityEngine.Random.value * totalFitnessScore;
		double partialsum = 0;
		int sGenome = 0;

		for (int i = 0; i < populationSize; i++) {
			partialsum += genomes [i].fitness;

			if (partialsum > r) {
				sGenome = i;
				break;
			}
		}
		return genomes[sGenome];
	}

	public void EvaluateFitness() {
		//Check Fitness
		if (!busy) return;
		UpdateFitness();

		//LastGen to display
		if (!busy) {
			lastGenerationGenomes.Clear();
			lastGenerationGenomes.AddRange (genomes);
			return;
		}

	

		

		int numberOfNewBabies = 0;

		List<Genome> babies = new List<Genome> ();
		while (numberOfNewBabies < populationSize) {
			// select 2 parents
			Genome mom = RouletteWheelSelection ();
			Genome dad = RouletteWheelSelection ();
			Genome baby1 = new Genome();
			Genome baby2 = new Genome();
			crossover (mom.bitz, dad.bitz, baby1.bitz, baby2.bitz);
			Mutate (baby1.bitz);
			Mutate (baby2.bitz);
			babies.Add (baby1);
			babies.Add (baby2);

			numberOfNewBabies += 2;
		}


		lastGenerationGenomes.Clear();
		lastGenerationGenomes.AddRange (genomes);
		// overwrite population with all the babies
		genomes = babies;

		// increment the generation counter
		generation++;
	}



	public void crossover(List<int> mom, List<int> dad, List<int> baby1, List<int> baby2) {
		

		System.Random rnd = new System.Random ();

		int crossoverPoint = rnd.Next (0, chromosomeLength - 1);

		for (int i = 0; i < crossoverPoint; i++) {
			baby1.Add (mom [i]);
			baby2.Add (dad [i]);
		}

		for (int i = crossoverPoint; i < mom.Count; i++) {
			baby1.Add (dad [i]);
			baby2.Add (mom [i]);
		}
	}
	public void Mutate(List<int> bits) {
		for (int i = 0; i < bits.Count; i++) {
			
			if (UnityEngine.Random.value < mutationRate) {
				// flip the bit
				bits [i] = (bits [i] == 0 )? 1 : 0;
			}

			
		}
	}
}
