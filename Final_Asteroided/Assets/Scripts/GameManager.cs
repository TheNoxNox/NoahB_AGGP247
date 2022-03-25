using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Main;

    public List<GamePiece> gamePieces = new List<GamePiece>();

    public Vector3 screenCenter;

    public GameObject spaceShipPrefab;

    public GameObject asteroidTestPref;

    public GameObject gravWellTest;

    private void Awake()
    {
        if (Main)
        {
            Destroy(this.gameObject);
        }

        Main = this;
        screenCenter = new Vector3(Screen.width / 2, Screen.height / 2);

        SpawnPlayerShip();

        Instantiate(asteroidTestPref).GetComponent<GamePiece>().location = screenCenter + new Vector3(-100, -300);

        Instantiate(gravWellTest).GetComponent<GravitySource>().location = screenCenter;
    }

    public void DestroyGamePiece(GamePiece piece)
    {
        gamePieces.Remove(piece);

        if(piece.GetType() == typeof(Spaceship))
        {
            Destroy(piece.gameObject);
            SpawnPlayerShip();
        }
        else
        {
            Destroy(piece.gameObject);
        }

        
    }

    public void SpawnPlayerShip(bool isPlayerOne = true)
    {
        Instantiate(spaceShipPrefab).GetComponent<GamePiece>().location = screenCenter + new Vector3(500, 400);
    }
}
