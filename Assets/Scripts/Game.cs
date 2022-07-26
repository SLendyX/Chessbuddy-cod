using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public GameObject chesspiece;

    //pozitii si culori
    private GameObject[,] positions = new GameObject[8, 8];
    private GameObject[] playerBlack = new GameObject[16];
    private GameObject[] playerWhite = new GameObject[16];

    private int[,] blackattackplate = new int[8,8];
    private int[,] whiteattackplate = new int[8,8];

    private int[,] blackmoveplate = new int[8, 8];
    private int[,] whitemoveplate = new int[8, 8];

    private bool[,] kingmoveplate = new bool[8, 8];


    private int piecemoves;

    private int pawnmoves;
    private int lastpawnmove;


    //score paramaters
    private int score;


    private int turns = 0;

    private string curentPlayer = "white";

    private bool gameOver = false;

    // Start is called before the first frame update
    void Start()
    {
            playerWhite = new GameObject[]
            {
            Create("white_rook", 0, 0), Create("white_knight", 1, 0), Create("white_bishop", 2, 0),
            Create("white_queen", 3, 0), Create("white_king", 4, 0), Create("white_bishop", 5, 0),
            Create("white_knight", 6, 0), Create("white_rook", 7, 0), Create("white_pawn", 0, 1),
            Create("white_pawn", 1, 1), Create("white_pawn", 2, 1), Create("white_pawn", 3, 1),
            Create("white_pawn", 4, 1), Create("white_pawn", 5, 1), Create("white_pawn", 6, 1),
            Create("white_pawn", 7, 1)
            };
            playerBlack = new GameObject[]
            {
            Create("black_rook", 0, 7), Create("black_knight", 1, 7), Create("black_bishop", 2, 7),
            Create("black_queen", 3, 7), Create("black_king", 4, 7), Create("black_bishop", 5, 7),
            Create("black_knight", 6, 7), Create("black_rook", 7, 7), Create("black_pawn", 0, 6),
            Create("black_pawn", 1, 6), Create("black_pawn", 2, 6), Create("black_pawn", 3, 6),
            Create("black_pawn", 4, 6), Create("black_pawn", 5, 6), Create("black_pawn", 6, 6),
            Create("black_pawn", 7, 6)
            };


        //Set piece position on the board
        for (int i=0; i< playerBlack.Length; i++)
        {
            SetPosition(playerBlack[i]);
            SetPosition(playerWhite[i]);
        }
    }

    public int GetScore()
    {
        return score;
    }

    public void SetScoreNull()
    {
        score = 0;
    }

    public void SetScore(string name)
    {
        switch (name)
        {
            case "black_queen":
                score -= 90;
                break;
            case "white_queen":
                score += 90;
                break;
            case "black_knight":
            case "black_bishop":
                score -= 30;
                break;
            case "white_knight":
            case "white_bishop":
                score += 30;
                break;
            case "black_king":
                score -= 900;
                break;
            case "white_king":
                score += 900;
                break;
            case "black_rook":
                score -= 50;
                break;
            case "white_rook":
                score += 50;
                break;
            case "black_pawn":
                score -= 10;
                break;
            case "white_pawn":
                score += 10;
                break;
        }

    }

    public void SetTurnPawn(int currentturn)
    {
        lastpawnmove = currentturn;
    }

    public int GetTurnPawn()
    {
        return lastpawnmove;
    }

    public void SetPawnMovesNull()
    {
        pawnmoves = 0;
    }

    public void SetPawnMoves()
    {
        pawnmoves++;
    }

    public int GetPawnMoves()
    {
        return pawnmoves;
    }

    public void SetMovesNull()
    {
        piecemoves = 0;
    }

    public void SetMoves() {
        piecemoves++;
    }

    public int GetMoves()
    {
        return piecemoves;
    }


        public void SetCheck(int x, int y, bool value)
        {
        kingmoveplate[x, y] = value;
        }

    public bool GetCheck(int x, int y)
    {
        return kingmoveplate[x, y];
    }

    public void CheckSquaresNull()
    {
        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
                kingmoveplate[i, j] = false;

    }

    public void Setattackwhite(int x, int y)
    {
        whiteattackplate[x, y]--;
    }

    public void Setattackblack(int x, int y)
    {
        blackattackplate[x, y]--;
    }

    public void Setattackwhiteking(int x, int y)
    {
        whiteattackplate[x, y] = -64;
    }

    public void Setattackblackking(int x, int y)
    {
        blackattackplate[x, y] = -64;
    }


    public int Getattackwhite(int x, int y)
    {
        return whiteattackplate[x, y];
    }

    public int Getattackblack(int x, int y)
    {
        return blackattackplate[x, y];
    }

    public void Setattackwhitenull()
    {
        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
                whiteattackplate[i, j] = 0;
    }

    public void Setattackblacknull()
    {
        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
                blackattackplate[i, j] = 0;
    }


    public void Setmovewhite(int x, int y)
    {
        whitemoveplate[x, y]++;
    }

    public void Setmoveblack(int x, int y)
    {
        blackmoveplate[x, y]++;
    }

    public int Getmovewhite(int x, int y)
    {
        return whitemoveplate[x, y];
    }

    public int Getmoveblack(int x, int y)
    {
        return blackmoveplate[x, y];
    }

    public void Setmovewhitenull()
    {
        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
                whitemoveplate[i, j] = 0;
    }

    public void Setmoveblacknull()
    {
        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
                blackmoveplate[i, j] = 0;
    }


    public GameObject Create(string name, int x, int y)
    {
        GameObject obj = Instantiate(chesspiece, new Vector3(0, 0, -1), Quaternion.identity);
        Chesspieces cm = obj.GetComponent<Chesspieces>();
        cm.name = name;
        cm.setXBoard(x);
        cm.setYBoard(y);
        cm.Activate();
        return obj;
    }
  
    public void SetPosition(GameObject obj)
    {
        Chesspieces cm = obj.GetComponent<Chesspieces>();

        positions[cm.GetXBoard(), cm.GetYBoard()] = obj;
    }

    public void SetPositionEmpty(int x, int y)
    {
        positions[x, y] = null;
    }

    public GameObject GetPosition(int x, int y)
    {
        return positions[x, y];
    }

    public bool PositionOnBoard(int x, int y)
    {
        if (x < 0 || y < 0 || x >= positions.GetLength(0) || y >= positions.GetLength(0))
            return false;
        return true;
    }
   
    public string GetCurrentPlayer()
    {
        return curentPlayer;
    }

    public bool IsGameOver()
    {
        return gameOver;
    }

    public void NextTurn()
    {
        if(curentPlayer == "white")
        {
            curentPlayer = "black";
        }
        else
        {
            curentPlayer = "white";
        }
        turns++;
    }

    public int GetTurns()
    {
        return turns;
    }


    public void Update()
    {
        if(gameOver == true && Input.GetMouseButtonDown(0))
        {
            gameOver = false;

            SceneManager.LoadScene("SampleScene");
        }
    }

    public void Winner(string playerWinner)
    {
        gameOver = true;
        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().enabled = true;
        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().text = playerWinner + " is the winner";

        GameObject.FindGameObjectWithTag("RestartText").GetComponent<Text>().enabled = true;
    }

    public void Stalemate()
    {
        gameOver = true;
        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().enabled = true;
        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().text = "Stalemate";

        GameObject.FindGameObjectWithTag("RestartText").GetComponent<Text>().enabled = true;
    }
}
