using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingTiledGame : MonoBehaviour {

	Vector3 screenPositionToAnimate;

	GameState gamestate;
	Piece PieceToAnimate;
	int toAnimateI, toAnimateJ;
	public Piece[,] Matrix = new Piece[Constant.MAXCOL,Constant.MAXROW];
	public GameObject[] go;
	public float AnimSpeed = 10.0f;


	private Vector3 GetScreenCoordinatesFromViewport(int i, int j)
	{
		Vector3 point = Camera.main.ViewportToWorldPoint (new Vector3(0.25f *j, 1 -0.25f *i,0));
		point.z = 0;
		return point;
	}


	void Start () {
		gamestate = GameState.Start;
		int index = Random.Range (0, Constant.MAXSIZE);
		go [index].SetActive (false);
		for (int i = 0; i < Constant.MAXCOL; i++)
			for (int j = 0; j < Constant.MAXROW; j++) {
				if(go[i*Constant.MAXCOL +j].activeInHierarchy)
				{
					Vector3 point = GetScreenCoordinatesFromViewport (i,j);
					go [i * Constant.MAXCOL + j].transform.position = point;

					Matrix [i, j] = new Piece();
					Matrix [i, j].GameObject = go [i * Constant.MAXCOL + j];
					Matrix [i, j].CurrentI = i;
					Matrix [i, j].CurrentJ = j;


					if (Matrix [i, j].GameObject.GetComponent<BoxCollider2D> () == null)
						Matrix [i, j].GameObject.AddComponent<BoxCollider2D> ();
				}
				else
				{
					Matrix [i, j] = null;
				}
		}

	}
	
	// Update is called once per frame
	void Update () {
		switch (gamestate) {
		case GameState.Start:
			if (Input.GetMouseButtonUp (0)) {
				Shuffle ();
				gamestate = GameState.Playing;
			}
			break;
		case GameState.Playing:
			CheckPieceInput ();
			break;
		case GameState.Animating:
			AnimateMovement (PieceToAnimate, Time.deltaTime);
			CheckIfAnimationEnded ();
			break;
		case GameState.End:
			if (Input.GetMouseButtonUp (0)) {
				Application.LoadLevel (Application.loadedLevel);
			}
			break;
		}
		
	}

	void OnGUI()
	{
		switch (gamestate) {
			case GameState.Start:
				GUI.Label (new Rect(0,0,100,100), "Tap to start!");
				break;
			case GameState.Playing:
				GUI.Label (new Rect(0,0,100,100), "Playing...");
				break;
			case GameState.End:
				GUI.Label (new Rect(0,0,100,100), "Congrats, Tap to start!");
				break;
		}
	}


	void Shuffle()
	{
		for (int i = 0; i < Constant.MAXCOL; i++)
			for (int j = 0; j < Constant.MAXROW; j++) 
			{
				if (Matrix [i, j] == null)
					continue;
				int random_i = Random.Range (0, Constant.MAXCOL);
				int random_j = Random.Range (0, Constant.MAXROW);
				Swap (i, j, random_i, random_j);
			}
			
	}

	void Swap(int i, int j, int random_i, int random_j)
	{
		Piece temp = Matrix[i,j];
		Matrix [i, j] = Matrix [random_i, random_j];
		Matrix [random_i, random_j] = temp;

		if (Matrix [i, j] != null) {
			Matrix [i, j].GameObject.transform.position = GetScreenCoordinatesFromViewport (i, j);
			Matrix [random_i, random_j].GameObject.transform.position = GetScreenCoordinatesFromViewport (random_i, random_j);

		}

		if (Matrix [i, j] != null) {
			Matrix [i, j].CurrentI = i;
			Matrix [i, j].CurrentJ = j;


		}
		Matrix [random_i, random_j].CurrentI = random_j;
		Matrix [random_i, random_j].CurrentJ = random_j;
	}

	void CheckPieceInput()
	{
	}
	void CheckIfAnimationEnded()
	{
		
	}
	void CheckForVictory()
	{
		
	}
	void AnimateMovement(Piece toMove, float time)
	{
		toMove.GameObject.transform.position = Vector2.MoveTowards (toMove.GameObject.transform.position, screenPositionToAnimate, time * AnimSpeed);
		
	}

}
