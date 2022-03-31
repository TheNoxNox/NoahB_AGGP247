using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Main;

    public List<GamePiece> gamePieces = new List<GamePiece>();

    public List<Spaceship> players = new List<Spaceship>();

    public List<Color> playerColors = new List<Color>();

    public List<Vector3> playerSpawns = new List<Vector3>();

    public Vector3 screenCenter;

    public GameObject spaceShipPrefab;

    public GameObject asteroidTestPref;

    public List<GameObject> asteroidPrefabs = new List<GameObject>();

    public GameObject gravWellTest;

    public int numOfPlayers = 2;

    private void Awake()
    {
        if (Main)
        {
            Destroy(this.gameObject);
        }

        Main = this;
        screenCenter = new Vector3(Screen.width / 2, Screen.height / 2);

        for(int i = 0; i < numOfPlayers; i++)
        {
            SpawnPlayerShip(i);
        }   

        Instantiate(asteroidTestPref).GetComponent<GamePiece>().location = screenCenter + new Vector3(-100, -300);

        //Instantiate(gravWellTest).GetComponent<GravitySource>().location = screenCenter;
    }

    private void Update()
    {
        PlayerInput();
    }

    public void DestroyGamePiece(GamePiece piece)
    {
        
        

        if(piece.GetType() == typeof(Spaceship))
        {
            piece.location = playerSpawns[(piece as Spaceship).playerNum];
            piece.dead = false;
            piece.velocity = Vector3.zero;
            piece.rotation = 90;
        }
        else
        {
            Destroy(piece.gameObject);
            gamePieces.Remove(piece);
        }

        
    }

    public void SpawnAsteroid()
    {

    }

    public void SpawnPlayerShip(int playerNum)
    {
        Spaceship ship = Instantiate(spaceShipPrefab).GetComponent<Spaceship>();
        ship.location = playerSpawns[playerNum];
        ship.pieceColor = playerColors[playerNum];
        ship.playerNum = playerNum;
    }

    public void PlayerInput()
    {
        if (Input.GetKey(KeyCode.A))
        {
            players[0].inputs.Add(KeyCode.A);
        }
        if (Input.GetKey(KeyCode.D))
        {
            players[0].inputs.Add(KeyCode.D);
        }
        if (Input.GetKey(KeyCode.W))
        {
            players[0].inputs.Add(KeyCode.W);
        }
        if (Input.GetKey(KeyCode.S))
        {
            players[0].inputs.Add(KeyCode.S);
        }


        if (Input.GetKey(KeyCode.LeftArrow))
        {
            players[1].inputs.Add(KeyCode.A);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            players[1].inputs.Add(KeyCode.D);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            players[1].inputs.Add(KeyCode.W);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            players[1].inputs.Add(KeyCode.S);
        }
    }
}
