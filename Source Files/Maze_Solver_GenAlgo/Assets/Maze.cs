using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour {

	public int[,] map;
	public GameObject wall;
	public GameObject start;
	public GameObject end;
	public GameObject path;
	public Vector2 startPosition;
	public Vector2 endPosition;
	public GenAlgo geneticAlgorithm;
	public List<int> fittestDirections;
	public List<GameObject> pathTiles;
	public GameObject text;
	Vector2 realPosition = new Vector2 (8f,-14f);

	void Start () {
		map= new int[,]{
		    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
			{1,0,0,0,0,0,0,1,0,0,0,0,0,0,1},
			{3,0,0,0,0,0,0,0,1,0,0,0,0,0,1},
			{1,0,0,1,0,0,0,0,0,1,0,0,0,0,1},
			{1,0,0,1,0,1,0,0,0,1,1,1,0,0,1},
			{1,0,0,0,1,0,0,0,0,1,1,1,0,0,1},
			{1,0,1,0,0,0,0,0,0,1,1,1,0,0,1},
			{1,0,0,1,0,0,0,1,0,0,0,0,0,0,1},
			{1,0,0,0,1,0,0,0,0,0,0,0,1,0,2},
			{1,0,0,0,0,1,0,0,0,0,0,0,0,0,1},
			{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}
		};
		CreateMap ();
		startPosition = new Vector2 (14f,8f);
		endPosition = new Vector2 (0f,2f);

		geneticAlgorithm = new GenAlgo ();
		geneticAlgorithm.maze = this;
		geneticAlgorithm.Run ();

	}

	public void CreateMap(){

		for (int x = 0; x < map.GetLength (0); x++) {
			for (int y = 0; y < map.GetLength (1); y++) {
				GameObject tile = TileByNumber (map[x,y]);
				if (tile != null) {
					GameObject pTile = Instantiate (tile);
					pTile.transform.position = new Vector3(x, 0, -y);

				}

			}

		}
			

	}
	public GameObject TileByNumber(int tile) {
		if (tile == 1) return wall;
		if (tile == 2) return start;
		if (tile == 3) return end;
		return null;
	}
	
	public double TestRoute(List<int> directions) {
		Vector2 position = startPosition;

		for (int directionIndex = 0; directionIndex < directions.Count; directionIndex++) {
			int nextDirection = directions [directionIndex];
			position = Move (position, nextDirection);
		}

		Vector2 deltaPosition = new Vector2(
			Math.Abs(position.x - endPosition.x),
			Math.Abs(position.y - endPosition.y));
		double result = 1 / (double)(deltaPosition.x + deltaPosition.y + 1);
			
		return result;
	}



	public Vector2 Move(Vector2 position, int direction) {
		switch (direction) {
		case 0: 
			if (position.y - 1 < 0 || map [(int)(position.y - 1), (int)position.x] == 1) {
				break;
			} 
			else {
				position.y -= 1;
				realPosition.y -= 1;
			}
			break;
		case 1: 
			if (position.y + 1 >= map.GetLength (0) || map [(int)(position.y + 1), (int)position.x] == 1) {
				break;
			} 
			else {
				position.y += 1;
				realPosition.y += 1;
			}
			break;
		case 2:
			if (position.x + 1 >= map.GetLength (1) || map [(int)position.y, (int)(position.x + 1)] == 1) {
				break;
			} 
			else {
				position.x += 1;
				realPosition.x += 1;
			}
			break;
		case 3: 
			if (position.x - 1 < 0 || map [(int)position.y, (int)(position.x - 1)] == 1) {
				break;
			} 
			else {
				position.x -= 1;
				realPosition.x -= 1;
			}
			break;
		}
		return position;
	}

	public void ClearPathTiles() {
		foreach (GameObject pathTile in pathTiles) {
			Destroy(pathTile);
		}
		pathTiles.Clear();
	}

	public void RenderFittestChromosomePath() {
		ClearPathTiles ();
		Genome fittestGenome = geneticAlgorithm.genomes[geneticAlgorithm.fittestGenome];
		List<int> fittestDirections = geneticAlgorithm.Decode (fittestGenome.bitz);
		Vector2 position = startPosition;



		foreach (int direction in fittestDirections) {
			position = Move (position, direction);
			GameObject pathTile = Instantiate (path);
			pathTile.transform.position = new Vector3(position.y, 0, -position.x);
			pathTiles.Add (pathTile);
		
		}
	}

	void Update () {
		if (geneticAlgorithm.busy)
			geneticAlgorithm.EvaluateFitness();
		RenderFittestChromosomePath ();
		TextMesh textMesh = text.GetComponent<TextMesh> ();
		Vector3 lastPosition = pathTiles.Last ().transform.position;
		textMesh.text = "Generation: " + geneticAlgorithm.generation;
	}
}
