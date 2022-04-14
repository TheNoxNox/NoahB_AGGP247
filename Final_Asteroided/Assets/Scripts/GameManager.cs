using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    #region under the hood stuff
    public static GameManager Main;

    public List<GamePiece> gamePieces = new List<GamePiece>();

    public List<Spaceship> players = new List<Spaceship>();

    public List<Color> playerColors = new List<Color>();

    public List<Vector3> playerSpawns = new List<Vector3>();

    public Vector3 screenCenter;

    public GameObject bulletPrefab;

    public GameObject spaceShipPrefab;

    public GameObject asteroidTestPref;

    public List<GameObject> asteroidPrefabs = new List<GameObject>();

    List<StarTwinkle> stars = new List<StarTwinkle>();

    public GameObject gravWellTest;

    public int numOfPlayers = 2;

    public bool allowInput = false;

    #endregion

    #region scoring stuff

    public int roundNumber = 1;

    public TMP_Text redScoreText;
    public TMP_Text blueScoreText;
    public TMP_Text roundCounter;
    public TMP_Text roundStartText;

    #endregion

    private void Awake()
    {
        if (Main)
        {
            Destroy(this.gameObject);
        }

        Main = this;
        screenCenter = new Vector3(Screen.width / 2, Screen.height / 2);

        for(int i = 0; i < 100; i++)
        {
            stars.Add(new StarTwinkle());
        }

        for(int i = 0; i < numOfPlayers; i++)
        {
            SpawnPlayerShip(i);
        }   

        //Instantiate(asteroidTestPref).GetComponent<GamePiece>().location = screenCenter + new Vector3(-100, -300);

        Instantiate(gravWellTest).GetComponent<GravitySource>().location = screenCenter;

        StartRound();
    }

    private void Update()
    {
        if (allowInput) { PlayerInput(); }
        foreach(StarTwinkle star in stars)
        {
            star.Twinkle();
        }
        blueScoreText.text = "SCORE\n" + players[1].score;
        redScoreText.text = "SCORE\n" + players[0].score;
        roundCounter.text = "Round " + roundNumber;
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
            gamePieces.Remove(piece);
            Destroy(piece.gameObject);          
        }

        
    }

    public void PlayerScored(Spaceship player)
    {
        player.score += 1;
        EndRound();
    }

    public void PlayerWipeout(Spaceship player)
    {
        player.score -= 1;
        EndRound();
    }

    public void SpawnAsteroid()
    {

    }

    public void Cleanup()
    {
        List<GamePiece> piecesToRemove = new List<GamePiece>();
        foreach(GamePiece piece in gamePieces)
        {
            if(piece.GetType() != typeof(Spaceship) && piece.GetType() != typeof(GravitySource))
            {
                piecesToRemove.Add(piece);
            }
        }
        foreach(GamePiece piece in piecesToRemove)
        {
            DestroyGamePiece(piece);
        }
    }

    public void SpawnPlayerShip(int playerNum)
    {
        Spaceship ship = Instantiate(spaceShipPrefab).GetComponent<Spaceship>();
        ship.location = playerSpawns[playerNum];
        ship.pieceColor = playerColors[playerNum];
        ship.playerNum = playerNum;
    }

    public void EndRound()
    {
        allowInput = false;
        roundNumber++;
        Cleanup();
        foreach(Spaceship player in players)
        {
            DestroyGamePiece(player);
        }

        StartRound();
    }

    public void StartRound()
    {
        StartCoroutine(RoundStartTimer());
    }

    public IEnumerator RoundStartTimer()
    {
        roundStartText.text = "ROUND " + roundNumber + "\nSTART";
        roundStartText.enabled = true;
        yield return new WaitForSeconds(1.2f);
        allowInput = true;
        roundStartText.enabled = false;
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
        if (Input.GetKeyDown(KeyCode.Q))
        {
            players[0].inputs.Add(KeyCode.Q);
        }


        if (Input.GetKey(KeyCode.J))
        {
            players[1].inputs.Add(KeyCode.A);
        }
        if (Input.GetKey(KeyCode.L))
        {
            players[1].inputs.Add(KeyCode.D);
        }
        if (Input.GetKey(KeyCode.I))
        {
            players[1].inputs.Add(KeyCode.W);
        }
        if (Input.GetKey(KeyCode.K))
        {
            players[1].inputs.Add(KeyCode.S);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            players[1].inputs.Add(KeyCode.Q);
        }
    }
}
