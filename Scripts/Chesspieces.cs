using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chesspieces : MonoBehaviour
{
    // Refrences
    public GameObject controller;
    public GameObject movePlate;
    public string pawnmove;

    public int whitehasmoved = 0;
    public int blackhasmoved = 0;

    public int kingmove = 0;
    public int rookmove = 0;

    public bool enpassant = false;
    public int passantturn = -1;

    public bool kingcastle = false;
    public bool queencastle = false;

    private int pawn_positionswhite;
    private int pawn_positionsblack;

    //Positions
    private int xBoard = -1;
    private int yBoard = -1;

    //variable black or white
    private string player;

    //Refrences sprites for chesspiece
    public Sprite black_king, black_queen, black_rook, black_knight, black_bishop, black_pawn;
    public Sprite white_king, white_queen, white_rook, white_knight, white_bishop, white_pawn;
    
    public void Activate()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        //takes instatiated coords and ajust the transform
        SetCoords();

        switch (this.name)
        {
            case "black_king": this.GetComponent<SpriteRenderer>().sprite = black_king; player = "black"; break;
            case "black_queen": this.GetComponent<SpriteRenderer>().sprite = black_queen; player = "black"; break;
            case "black_knight": this.GetComponent<SpriteRenderer>().sprite = black_knight; player = "black"; break;
            case "black_bishop": this.GetComponent<SpriteRenderer>().sprite = black_bishop; player = "black"; break;
            case "black_pawn": this.GetComponent<SpriteRenderer>().sprite = black_pawn; player = "black"; break;
            case "black_rook": this.GetComponent<SpriteRenderer>().sprite = black_rook; player = "black"; break;
            
            case "white_king": this.GetComponent<SpriteRenderer>().sprite = white_king; player = "white"; break;
            case "white_queen": this.GetComponent<SpriteRenderer>().sprite = white_queen; player = "white"; break;
            case "white_knight": this.GetComponent<SpriteRenderer>().sprite = white_knight; player = "white"; break;
            case "white_bishop": this.GetComponent<SpriteRenderer>().sprite = white_bishop; player = "white"; break;
            case "white_pawn": this.GetComponent<SpriteRenderer>().sprite = white_pawn; player = "white"; break;
            case "white_rook": this.GetComponent<SpriteRenderer>().sprite = white_rook; player = "white"; break;



        }
    }


    public void Setkingmove(int move)
    {
        kingmove = move;
    }

    public int Getkingmove()
    {
        return kingmove;
    }


    public void Setrookmove(int move)
    {
        rookmove = move;
    }

    public int Getrookmove()
    {
        return rookmove;
    }

    public bool Getkingcastle()
    {
        return kingcastle;
    }

    public bool Getqueencastle()
    {
        return queencastle;
    }

    public int Getpassantmove()
    {
        return passantturn;
    }

    public void Setpassantmove(int pass)
    {
        passantturn = pass;
    }

    public void Setenpassant(bool pass)
    {
        enpassant = pass;
    }

    public bool Isenpassant()
    {
        return enpassant;
    }

    public int Getpawnpostion_white()
    {
        return pawn_positionswhite;
    }

    public int Getpawnpostion_black()
    {
        return pawn_positionsblack;
    }


    public void Whitehasmoved(int moved)
    {
        whitehasmoved += moved;
    }

    public void Blackhasmoved(int moved)
    {
        blackhasmoved += moved;
    }

    public void SetCoords()
    {
        float x = xBoard;
        float y = yBoard;

        x *= 1.12f;
        y *= 1.11f;

        x += -3.9f;
        y += -3.9f;

        this.transform.position = new Vector3(x, y, -1.0f);
    }

    public int GetXBoard()
    {
        return xBoard;
    }

    public int GetYBoard()
    {
        return yBoard;
    }

    public void setXBoard(int x)
    {
        xBoard = x;
    }

    public void setYBoard(int y)
    {
        yBoard = y;
    }

    private void OnMouseUp()
    {
        if (!controller.GetComponent<Game>().IsGameOver() && controller.GetComponent<Game>().GetCurrentPlayer() == player)
        {
            DestroyMovePlates();

            InitiateMovePlate();
        }
        else
        {
            DestroyMovePlates();
        }
    }

    public void DestroyMovePlates()
    {
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
           for(int i=0; i< movePlates.Length; i++)
        {
            Destroy(movePlates[i]);

        }
    }

    public int[] movecoord = new int[] { 0, 1, -1 };

    public void InitiateMovePlate()
    {
        switch (this.name)
        {
            case "black_queen":
            case "white_queen":
                for (int i = 0; i < 3; i++)
                    for (int j = 0; j < 3; j++)
                    {
                        if (j == 0 && movecoord[i] == 0)
                            continue;
                        LineMovePlate(movecoord[i], movecoord[j]);
                    }
                break;

            case "black_knight":
            case "white_knight":
                LMovePlate();
                break;

            case "black_bishop":
            case "white_bishop":
                for (int i = 1; i < 3; i++)
                    for (int j = 1; j < 3; j++)
                        LineMovePlate(movecoord[i], movecoord[j]);
                break;
            case "black_king":
            case "white_king":
                SurroundMovePlate();
                if (kingmove == 0 && rookmove == 0)
                {
                    KingSideCastlePlate(xBoard, yBoard);
                    QueenSideCastlePlate(xBoard, yBoard);
                }
                    
                break;
            case "black_rook":
            case "white_rook":
                for (int i = 1; i < 3; i++)
                    LineMovePlate(movecoord[i], 0);
                for (int i = 1; i < 3; i++)
                    LineMovePlate(0, movecoord[i]);
                break;
            case "black_pawn":
                PawnMovePlate(xBoard, yBoard - 1);
                break;
            case "white_pawn":
                PawnMovePlate(xBoard, yBoard + 1);
                break;
        }
    }

    public void LineMovePlate(int xIncrement, int yIncrement)
    {
        Game sc = controller.GetComponent<Game>();
        int x = xBoard + xIncrement;
        int y = yBoard + yIncrement;

        while(sc.PositionOnBoard(x,y) && sc.GetPosition(x,y) == null)
        {
            MovePlateSpawn(x, y);
            x += xIncrement;
            y += yIncrement;
        }

        if(sc.PositionOnBoard(x,y) && sc.GetPosition(x,y).GetComponent<Chesspieces>().player != player)
        {
            MovePlateAttackSpawn(x, y);
        }
    }
   

    public void LMovePlate()
    {
        PointMovePlate(xBoard + 1, yBoard + 2);
        PointMovePlate(xBoard + 1, yBoard - 2);
        PointMovePlate(xBoard - 1, yBoard + 2);
        PointMovePlate(xBoard - 1, yBoard - 2);
        PointMovePlate(xBoard + 2, yBoard + 1);
        PointMovePlate(xBoard - 2, yBoard + 1);
        PointMovePlate(xBoard + 2, yBoard - 1);
        PointMovePlate(xBoard - 2, yBoard - 1);
    }

    public void SurroundMovePlate()
    {
        PointMovePlate(xBoard, yBoard + 1);
        PointMovePlate(xBoard, yBoard - 1);
        PointMovePlate(xBoard - 1, yBoard - 1);
        PointMovePlate(xBoard - 1, yBoard);
        PointMovePlate(xBoard + 1, yBoard);
        PointMovePlate(xBoard - 1, yBoard + 1);
        PointMovePlate(xBoard + 1, yBoard - 1);
        PointMovePlate(xBoard + 1, yBoard + 1);
    }

    public void PointMovePlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();
        if (sc.PositionOnBoard(x, y))
        {
            GameObject cp = sc.GetPosition(x, y);

            if(cp == null)
            {
                MovePlateSpawn(x, y);
            }else if(cp.GetComponent<Chesspieces>().player != player)
            {
                MovePlateAttackSpawn(x, y);
            }
        }
    }

    //castling
    public void KingSideCastlePlate(int x, int y)
    {
       Game sc = controller.GetComponent<Game>();
       if(sc.GetPosition(x+1, y) == null && sc.GetPosition(x + 2, y) == null && sc.GetPosition(x+3, y) != null)
       {
            if ((controller.GetComponent<Game>().GetCurrentPlayer() == "white" && sc.GetPosition(x + 3, y).name == "white_rook") || (controller.GetComponent<Game>().GetCurrentPlayer() == "black" && sc.GetPosition(x + 3, y).name == "black_rook"))
                if (sc.GetPosition(x + 3, y).GetComponent<Chesspieces>().Getrookmove() == 0)
                MovePlateSpawn(x+2, y);
       }

    }

    public void QueenSideCastlePlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();
        if (sc.GetPosition(x - 1, y) == null && sc.GetPosition(x - 2, y) == null && sc.GetPosition(x - 3, y) == null && sc.GetPosition(x-4, y) != null)
        {
            if((controller.GetComponent<Game>().GetCurrentPlayer() == "white" && sc.GetPosition(x - 4, y).name == "white_rook") || (controller.GetComponent<Game>().GetCurrentPlayer() == "black" && sc.GetPosition(x - 4, y).name == "black_rook"))
                if (sc.GetPosition(x - 4, y).GetComponent<Chesspieces>().Getrookmove() == 0)
                    MovePlateSpawn(x - 2, y);
        }
    }

    public void PawnMovePlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();
        MovePlate mp = movePlate.GetComponent<MovePlate>();
        if (controller.GetComponent<Game>().GetCurrentPlayer() == "white")
            pawn_positionswhite = y-1;
        else
            pawn_positionsblack = y+1;

        if (sc.PositionOnBoard(x, y))
        {
            if(sc.GetPosition(x,y) == null)
            {
                MovePlateSpawn(x, y);
                if(controller.GetComponent<Game>().GetCurrentPlayer() == "white" && whitehasmoved==0 && sc.GetPosition(x, y+1) == null)
                {
                    MovePlateSpawn(x, y+1);
                }else if(controller.GetComponent<Game>().GetCurrentPlayer() == "black" && blackhasmoved == 0 && sc.GetPosition(x, y-1) == null)
                {
                    MovePlateSpawn(x, y - 1);
                }    
            }

            if (sc.PositionOnBoard(x + 1, y) && sc.GetPosition(x + 1, y) != null && sc.GetPosition(x + 1, y).GetComponent<Chesspieces>().player != player) 
            {
                MovePlateAttackSpawn(x + 1, y);
            }

            if (sc.PositionOnBoard(x - 1, y) && sc.GetPosition(x - 1, y) != null && sc.GetPosition(x - 1, y).GetComponent<Chesspieces>().player != player)
            {
                MovePlateAttackSpawn(x - 1, y);
            }

            //enpasant
            if (controller.GetComponent<Game>().GetCurrentPlayer() == "white")
            {
                if (sc.PositionOnBoard(x + 1, y-1) && sc.GetPosition(x + 1, y-1) != null && sc.GetPosition(x + 1, y-1).GetComponent<Chesspieces>().player != player && sc.GetPosition(x + 1, y - 1).GetComponent<Chesspieces>().Isenpassant())
                {
                    if (sc.GetPosition(x + 1, y - 1).GetComponent<Chesspieces>().passantturn == sc.GetComponent<Game>().GetTurns())
                    {
                        MovePlateAttackSpawn(x + 1, y);
                    }
                }

                if (sc.PositionOnBoard(x - 1, y-1) && sc.GetPosition(x - 1, y-1) != null && sc.GetPosition(x - 1, y-1).GetComponent<Chesspieces>().player != player && sc.GetPosition(x - 1, y - 1).GetComponent<Chesspieces>().Isenpassant())
                {
                    if (sc.GetPosition(x - 1, y - 1).GetComponent<Chesspieces>().passantturn == sc.GetComponent<Game>().GetTurns())
                    {
                        MovePlateAttackSpawn(x - 1, y);
                    }
                }
            }else if (controller.GetComponent<Game>().GetCurrentPlayer() == "black")
            {
                if (sc.PositionOnBoard(x + 1, y + 1) && sc.GetPosition(x + 1, y + 1) != null && sc.GetPosition(x + 1, y + 1).GetComponent<Chesspieces>().player != player && sc.GetPosition(x + 1, y + 1).GetComponent<Chesspieces>().Isenpassant())
                {
                    if (sc.GetPosition(x + 1, y + 1).GetComponent<Chesspieces>().passantturn == sc.GetComponent<Game>().GetTurns())
                    {
                        MovePlateAttackSpawn(x + 1, y);
                    }
                }

                if (sc.PositionOnBoard(x - 1, y + 1) && sc.GetPosition(x - 1, y + 1) != null && sc.GetPosition(x - 1, y + 1).GetComponent<Chesspieces>().player != player && sc.GetPosition(x - 1, y + 1).GetComponent<Chesspieces>().Isenpassant())
                {
                    if (sc.GetPosition(x - 1, y + 1).GetComponent<Chesspieces>().passantturn == sc.GetComponent<Game>().GetTurns())
                    {
                        MovePlateAttackSpawn(x - 1, y);
                    }
                }
            }
        }
    }

    public void MovePlateSpawn(int matriceX, int matriceY)
    {
        float x = matriceX;
        float y = matriceY;

        x *= 1.12f;
        y *= 1.11f;

        x += -3.9f;
        y += -3.9f;

        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);

        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matriceX, matriceY);
    }

    public void MovePlateAttackSpawn(int matriceX, int matriceY)
    {
        float x = matriceX;
        float y = matriceY;

        x *= 1.12f;
        y *= 1.11f;

        x += -3.9f;
        y += -3.9f;

        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);

        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.attack = true;
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matriceX, matriceY);
    }
}
