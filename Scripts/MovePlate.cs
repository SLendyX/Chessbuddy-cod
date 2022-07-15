using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;

public class MovePlate : MonoBehaviour
{
    public GameObject controller;

    GameObject refrence = null;


    int matriceX;
    int matriceY;

    int kingX = 0, kingY = 0;


    public Sprite white_pawn;

    public bool attack = false;

    public void Start()
    {
        if (attack)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }
    }

    public void OnMouseUp()
    {

        controller = GameObject.FindGameObjectWithTag("GameController");

        controller.GetComponent<Game>().CheckSquaresNull();

        if (attack)
            {

                GameObject cp = controller.GetComponent<Game>().GetPosition(matriceX, matriceY);

                if (cp != null)
                {
                    if (cp.name == "white_king") controller.GetComponent<Game>().Winner("black");
                    if (cp.name == "black_king") controller.GetComponent<Game>().Winner("white");

                }

            //enpassant
                if (cp != null)
                    Destroy(cp);
                else if (controller.GetComponent<Game>().GetPosition(matriceX, matriceY - 1) != null)
                    Destroy(controller.GetComponent<Game>().GetPosition(matriceX, matriceY - 1));
                else if (controller.GetComponent<Game>().GetPosition(matriceX, matriceY + 1) != null)
                    Destroy(controller.GetComponent<Game>().GetPosition(matriceX, matriceY + 1));
            }

        controller.GetComponent<Game>().SetPositionEmpty(refrence.GetComponent<Chesspieces>().GetXBoard(),
    refrence.GetComponent<Chesspieces>().GetYBoard());

        refrence.GetComponent<Chesspieces>().setXBoard(matriceX);
        refrence.GetComponent<Chesspieces>().setYBoard(matriceY);

        refrence.GetComponent<Chesspieces>().SetCoords();
        controller.GetComponent<Game>().SetPosition(refrence);

        refrence.GetComponent<Chesspieces>().DestroyMovePlates();


            controller.GetComponent<Game>().NextTurn();


        //Checking and checkmate


            controller.GetComponent<Game>().Setattackwhitenull();
            controller.GetComponent<Game>().Setattackblacknull();

            controller.GetComponent<Game>().Setmovewhitenull();
            controller.GetComponent<Game>().Setmoveblacknull();



        /*for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
                if (controller.GetComponent<Game>().GetPosition(i, j) != null)
                    controller.GetComponent<Game>().GetPosition(i, j).GetComponent<Chesspieces>().SetPinned(false);
        */

        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
            {
                if (controller.GetComponent<Game>().GetPosition(i, j) != null)
                {
                    SetAttackPlate(i, j, controller.GetComponent<Game>().GetPosition(i, j).name, controller.GetComponent<Game>().GetPosition(i, j).GetComponent<Chesspieces>().GetPlayer());

                    // if (controller.GetComponent<Game>().GetPosition(i, j).name == "white_king" || controller.GetComponent<Game>().GetPosition(i, j).name == "black_king")
                    //CheckPins(i, j, controller.GetComponent<Game>().GetPosition(i, j).GetComponent<Chesspieces>().GetPlayer());


                    if (controller.GetComponent<Game>().GetCurrentPlayer() == "white")
                    {
                        if (controller.GetComponent<Game>().GetPosition(i, j).name == "white_king")
                        {
                            SetKingCoords(i, j);
                        }
                    }else if(controller.GetComponent<Game>().GetPosition(i, j).name == "black_king")
                    {
                        SetKingCoords(i, j);
                    }
                } 

                   
            }
        CheckSquares(kingX, kingY);




        if (Checkmate(kingX, kingY))
        {
              if (controller.GetComponent<Game>().GetCurrentPlayer() == "white")
              {
                 controller.GetComponent<Game>().Winner("black");
              }
              else
                 controller.GetComponent<Game>().Winner("white");
        }


        //King in check


        //for (int i = 0; i < 8; i++)
        //  for (int j = 0; j < 8; j++)
        //  if (controller.GetComponent<Game>().GetPosition(i, j).name == "white_king" && controller.GetComponent<Game>().Getattackwhite(i,j) == -1 || controller.GetComponent<Game>().GetPosition(i, j).name == "black_king" && controller.GetComponent<Game>().Getattackblack(i,j) == -1)
        //controller.GetComponent<Chesspieces>().SetCheck(true);


        //castle
        if (controller.GetComponent<Game>().GetPosition(matriceX, matriceY).name == "white_king" && refrence.GetComponent<Chesspieces>().Getkingmove() == 0 && controller.GetComponent<Game>().GetPosition(matriceX, matriceY).GetComponent<Chesspieces>().Getrookmove() == 0)
            {
                if (controller.GetComponent<Game>().GetPosition(matriceX + 1, matriceY) != null && controller.GetComponent<Game>().GetPosition(matriceX + 1, matriceY).name == "white_rook")
                {
                    Destroy(controller.GetComponent<Game>().GetPosition(matriceX + 1, matriceY));

                    GameObject obj = controller.GetComponent<Game>().Create("white_rook", matriceX - 1, matriceY);

                    controller.GetComponent<Game>().SetPositionEmpty(matriceX + 1, matriceY);

                    obj.GetComponent<Chesspieces>().setXBoard(matriceX - 1);
                    obj.GetComponent<Chesspieces>().setYBoard(matriceY);
                    obj.GetComponent<Chesspieces>().SetCoords();

                    controller.GetComponent<Game>().SetPosition(obj);
                }
                else if (controller.GetComponent<Game>().GetPosition(matriceX - 2, matriceY) != null && controller.GetComponent<Game>().GetPosition(matriceX - 2, matriceY).name == "white_rook")
                {
                    Destroy(controller.GetComponent<Game>().GetPosition(matriceX - 2, matriceY));

                    GameObject obj = controller.GetComponent<Game>().Create("white_rook", matriceX + 1, matriceY);

                    controller.GetComponent<Game>().SetPositionEmpty(matriceX - 2, matriceY);

                    obj.GetComponent<Chesspieces>().setXBoard(matriceX + 1);
                    obj.GetComponent<Chesspieces>().setYBoard(matriceY);
                    obj.GetComponent<Chesspieces>().SetCoords();

                    controller.GetComponent<Game>().SetPosition(obj);
                }
        }
            else if (controller.GetComponent<Game>().GetPosition(matriceX, matriceY).name == "black_king" && refrence.GetComponent<Chesspieces>().Getkingmove() == 0 && controller.GetComponent<Game>().GetPosition(matriceX, matriceY).GetComponent<Chesspieces>().Getrookmove() == 0)
            {
                if (controller.GetComponent<Game>().GetPosition(matriceX + 1, matriceY) != null && controller.GetComponent<Game>().GetPosition(matriceX + 1, matriceY).name == "black_rook")
                {
                    Destroy(controller.GetComponent<Game>().GetPosition(matriceX + 1, matriceY));

                    GameObject obj = controller.GetComponent<Game>().Create("black_rook", matriceX - 1, matriceY);

                    controller.GetComponent<Game>().SetPositionEmpty(matriceX + 1, matriceY);

                    obj.GetComponent<Chesspieces>().setXBoard(matriceX - 1);
                    obj.GetComponent<Chesspieces>().setYBoard(matriceY);
                    obj.GetComponent<Chesspieces>().SetCoords();

                    controller.GetComponent<Game>().SetPosition(obj);
                }
                else if (controller.GetComponent<Game>().GetPosition(matriceX - 2, matriceY) != null && controller.GetComponent<Game>().GetPosition(matriceX - 2, matriceY).name == "black_rook")
                {
                    Destroy(controller.GetComponent<Game>().GetPosition(matriceX - 2, matriceY));

                    GameObject obj = controller.GetComponent<Game>().Create("black_rook", matriceX + 1, matriceY);

                    controller.GetComponent<Game>().SetPositionEmpty(matriceX - 2, matriceY);

                    obj.GetComponent<Chesspieces>().setXBoard(matriceX + 1);
                    obj.GetComponent<Chesspieces>().setYBoard(matriceY);
                    obj.GetComponent<Chesspieces>().SetCoords();

                    controller.GetComponent<Game>().SetPosition(obj);
                }
            }

            //king moves
            if (controller.GetComponent<Game>().GetPosition(matriceX, matriceY).name == "white_king" || controller.GetComponent<Game>().GetPosition(matriceX, matriceY).name == "black_king")
            {
                refrence.GetComponent<Chesspieces>().Setkingmove(1);
            }

            if (controller.GetComponent<Game>().GetPosition(matriceX, matriceY).name == "white_rook" || controller.GetComponent<Game>().GetPosition(matriceX, matriceY).name == "black_rook")
            {
                refrence.GetComponent<Chesspieces>().Setrookmove(1);
            }

            //pawn movement
            if (controller.GetComponent<Game>().GetPosition(matriceX, matriceY).name == "white_pawn")
            {
                refrence.GetComponent<Chesspieces>().Whitehasmoved(2);
                if (Math.Abs(refrence.GetComponent<Chesspieces>().Getpawnpostion_white() - matriceY) == 2)
                {
                    refrence.GetComponent<Chesspieces>().Setenpassant(true);
                    refrence.GetComponent<Chesspieces>().Setpassantmove(controller.GetComponent<Game>().GetTurns());
                }
                if (matriceY == 7)
                {
                    Destroy(controller.GetComponent<Game>().GetPosition(matriceX, matriceY));
                    controller.GetComponent<Game>().SetPositionEmpty(refrence.GetComponent<Chesspieces>().GetXBoard(),
                    refrence.GetComponent<Chesspieces>().GetYBoard());

                    refrence.GetComponent<Chesspieces>().setXBoard(matriceX);
                    refrence.GetComponent<Chesspieces>().setYBoard(matriceY);
                    refrence.GetComponent<Chesspieces>().SetCoords();

                    GameObject obj = controller.GetComponent<Game>().Create("white_queen", matriceX, matriceY);

                    controller.GetComponent<Game>().SetPosition(obj);
                }
            }
            else if (controller.GetComponent<Game>().GetPosition(matriceX, matriceY).name == "black_pawn")
            {
                refrence.GetComponent<Chesspieces>().Blackhasmoved(2);
                if (Math.Abs(refrence.GetComponent<Chesspieces>().Getpawnpostion_black() - matriceY) == 2)
                {
                    refrence.GetComponent<Chesspieces>().Setenpassant(true);
                    refrence.GetComponent<Chesspieces>().Setpassantmove(controller.GetComponent<Game>().GetTurns());
                }
                if (matriceY == 0)
                {
                    Destroy(controller.GetComponent<Game>().GetPosition(matriceX, matriceY));
                    controller.GetComponent<Game>().SetPositionEmpty(refrence.GetComponent<Chesspieces>().GetXBoard(),
                    refrence.GetComponent<Chesspieces>().GetYBoard());

                    refrence.GetComponent<Chesspieces>().setXBoard(matriceX);
                    refrence.GetComponent<Chesspieces>().setYBoard(matriceY);
                    refrence.GetComponent<Chesspieces>().SetCoords();

                    GameObject obj = controller.GetComponent<Game>().Create("black_queen", matriceX, matriceY);

                    controller.GetComponent<Game>().SetPosition(obj);
                }
            }
    }

    public void SetKingCoords(int x, int y)
    {
        kingX = x;
        kingY = y;
    }

    public int GetKingX()
    {
        return kingX;
    }

    public int GetKingY()
    {
        return kingY;
    }


    public void SetCoords(int x, int y)
    {
        matriceX = x;
        matriceY = y;
    }

    public void SetReference(GameObject obj)
    {
        refrence = obj;
    }

    public GameObject GetReference()
    {
        return refrence;
    }

    public int[] movecoord = new int[] { 0, 1, -1 };


   // public void Kingattackspace

    public void CheckSquares(int x, int y)
    {
        string color = controller.GetComponent<Game>().GetCurrentPlayer();
        controller = GameObject.FindGameObjectWithTag("GameController");

            LCheck(color, x, y);

            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    if (j == i && i == 0)
                        continue;
                    LineCheck(movecoord[i], movecoord[j], color, x, y, 0);
                }

        PawnCheck(color, x, y);
    }

    public void PawnCheck(string color, int x, int y)
    {
        Game sc = controller.GetComponent<Game>();
        if (color == "white")
        {
            if (sc.PositionOnBoard(x - 1, y + 1) && sc.GetPosition(x - 1, y + 1) != null && sc.GetPosition(x - 1, y + 1).name == "black_pawn")
                sc.SetCheck(x - 1, y + 1, true);
            else if (sc.PositionOnBoard(x + 1, y + 1) && sc.GetPosition(x + 1, y + 1) != null && sc.GetPosition(x + 1, y + 1).name == "black_pawn")
                sc.SetCheck(x + 1, y + 1, true);
        }else
        {
            if (sc.PositionOnBoard(x - 1, y - 1) && sc.GetPosition(x - 1, y - 1) != null && sc.GetPosition(x - 1, y - 1).name == "white_pawn")
                sc.SetCheck(x - 1, y - 1, true);
            else if (sc.PositionOnBoard(x + 1, y -1) && sc.GetPosition(x + 1, y - 1) != null && sc.GetPosition(x + 1, y - 1).name == "white_pawn")
                sc.SetCheck(x + 1, y - 1, true);
        }
    }

    public void LineCheck(int xIncrement, int yIncrement, string color, int xBoard, int yBoard, int counter)
    {
        Game sc = controller.GetComponent<Game>();
        int x = xBoard + xIncrement;
        int y = yBoard + yIncrement;

        if (color == "white")
        {
            if (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y) == null)
            {
                LineCheck(xIncrement, yIncrement, color, x, y, counter);
                if(sc.PositionOnBoard(x + xIncrement, y + yIncrement))
                    sc.SetCheck(x, y, sc.GetCheck(x + xIncrement, y + yIncrement));
            }
            else if (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y) != null && xIncrement != 0 && yIncrement != 0)
            {
                if (counter < 1 && sc.GetPosition(x, y).GetComponent<Chesspieces>().GetPlayer() == color)
                {
                    LineCheck(xIncrement, yIncrement, color, x, y, counter + 1);
                    if (sc.PositionOnBoard(x + xIncrement, y + yIncrement))
                        sc.SetCheck(x, y, sc.GetCheck(x + xIncrement, y + yIncrement));
                }
                else if (sc.GetPosition(x, y).name == "black_queen" || sc.GetPosition(x, y).name == "black_bishop")
                    sc.SetCheck(x, y, true);

            }
            else if (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y) != null && (xIncrement == 0 || yIncrement == 0))
            {
                if (counter < 1 && sc.GetPosition(x, y).GetComponent<Chesspieces>().GetPlayer() == color)
                {
                    LineCheck(xIncrement, yIncrement, color, x, y, counter + 1);
                    if (sc.PositionOnBoard(x + xIncrement, y + yIncrement))
                        sc.SetCheck(x, y, sc.GetCheck(x + xIncrement, y + yIncrement));
                }
                else if (sc.GetPosition(x, y).name == "black_rook" || sc.GetPosition(x, y).name == "black_queen")
                    sc.SetCheck(x, y, true);
            }
                
        }
        else
        {
            if (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y) == null)
            {
                LineCheck(xIncrement, yIncrement, color, x, y, counter);
                if (sc.PositionOnBoard(x + xIncrement, y + yIncrement))
                    sc.SetCheck(x, y, sc.GetCheck(x + xIncrement, y + yIncrement));
            }
            else if (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y) != null && xIncrement != 0 && yIncrement != 0) {
                if (counter < 1 && sc.GetPosition(x, y).GetComponent<Chesspieces>().GetPlayer() == color)
                {
                    LineCheck(xIncrement, yIncrement, color, x, y, counter + 1);
                    if (sc.PositionOnBoard(x + xIncrement, y + yIncrement))
                        sc.SetCheck(x, y, sc.GetCheck(x + xIncrement, y + yIncrement));
                }else if (sc.GetPosition(x, y).name == "white_queen" || sc.GetPosition(x, y).name == "white_bishop")
                    sc.SetCheck(x, y, true);
                
            } else if (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y) != null && (xIncrement == 0 || yIncrement == 0))
                if (counter < 1 && sc.GetPosition(x, y).GetComponent<Chesspieces>().GetPlayer() == color)
                {
                    LineCheck(xIncrement, yIncrement, color, x, y, counter + 1);
                    if (sc.PositionOnBoard(x + xIncrement, y + yIncrement))
                        sc.SetCheck(x, y, sc.GetCheck(x + xIncrement, y + yIncrement));
                }else if (sc.GetPosition(x, y).name == "white_rook" || sc.GetPosition(x, y).name == "white_queen")
                    sc.SetCheck(x, y, true);
        }
    }


    public void LCheck(string color, int xBoard, int yBoard)
    {

        PointCheck(xBoard + 1, yBoard + 2, color);
        PointCheck(xBoard + 1, yBoard - 2, color);
        PointCheck(xBoard - 1, yBoard + 2, color);
        PointCheck(xBoard - 1, yBoard - 2, color);
        PointCheck(xBoard + 2, yBoard + 1, color);
        PointCheck(xBoard - 2, yBoard + 1, color);
        PointCheck(xBoard + 2, yBoard - 1, color);
        PointCheck(xBoard - 2, yBoard - 1, color);
    }


    public void PointCheck(int x, int y, string color)
    {
        Game sc = controller.GetComponent<Game>();
        if (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y) != null)
        {
            if (color == "white" && sc.GetPosition(x, y).name == "black_knight" || color == "black" && sc.GetPosition(x, y).name == "white_knight")
                sc.SetCheck(x, y, true);
        }
    }

    public bool Checkmate(int x, int y)
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        if (controller.GetComponent<Game>().GetCurrentPlayer() == "white")
        {
            if (controller.GetComponent<Game>().Getattackblack(x, y) ==0)
                return false;
            else
            {
                if (SurroundCheck(x, y))
                    return BlockCheck(x, y);
            }
        }else
        {
            if (controller.GetComponent<Game>().Getattackwhite(x, y) ==0)
                return false;
            else
            {
                if (SurroundCheck(x, y))
                    return BlockCheck(x, y);

            }
        }
        return false;

    }

    public bool BlockCheck(int x, int y)
    {
        int checknumber;
        controller = GameObject.FindGameObjectWithTag("GameController");
        Game sc = controller.GetComponent<Game>();

        if (sc.GetCurrentPlayer() == "white")
            checknumber = sc.Getattackblack(x, y) * (-1);
        else
            checknumber = sc.Getattackwhite(x, y) * (-1);

        for(int i=0; i<8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (checknumber == 0)
                    break;

                if(sc.GetCurrentPlayer() == "white")
                {
                    if (sc.PositionOnBoard(i, j) && sc.GetPosition(i, j) == null && sc.GetCheck(i,j) && sc.Getmovewhite(i,j) > 0)
                        checknumber--;
                    else if(sc.PositionOnBoard(i, j) && sc.GetPosition(i, j) != null && sc.Getattackwhite(i, j) < 0  && sc.Getattackwhite(i, j) > -64 && sc.GetPosition(i, j).GetComponent<Chesspieces>().GetPlayer() != sc.GetCurrentPlayer() && sc.GetCheck(i, j))
                        checknumber--;
                }else
                {
                    if (sc.PositionOnBoard(i, j) && sc.GetPosition(i, j) == null && sc.GetCheck(i, j) && sc.Getmoveblack(i, j) > 0)
                        checknumber--;
                    else if (sc.PositionOnBoard(x, y) && sc.GetPosition(i, j) != null && sc.Getattackblack(i, j) < 0 && sc.Getattackblack(i, j) > -64 && sc.GetPosition(i, j).GetComponent<Chesspieces>().GetPlayer() != sc.GetCurrentPlayer() && sc.GetCheck(i, j))
                        checknumber--;
                }
            }
            if (checknumber == 0)
                break;
        }

        if (checknumber == 0)
            return false;
        else
            return true;
    }

    public bool SurroundCheck(int xBoard, int yBoard)
    {

        if (!PointMove(xBoard, yBoard + 1))
            return false;
        else if (!PointMove(xBoard, yBoard - 1))
            return false;
        else if (!PointMove(xBoard - 1, yBoard - 1))
            return false;
        else if (!PointMove(xBoard - 1, yBoard))
            return false;
        else if (!PointMove(xBoard + 1, yBoard))
            return false;
        else if (!PointMove(xBoard - 1, yBoard + 1))
            return false;
        else if (!PointMove(xBoard + 1, yBoard - 1))
            return false;
        else if (!PointMove(xBoard + 1, yBoard + 1))
            return false;
        else
            return true;
        
    }

    public bool PointMove(int x, int y)
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        Game sc = controller.GetComponent<Game>();

        if (sc.GetCurrentPlayer() == "white")
        {
            if (!sc.PositionOnBoard(x, y) || sc.Getattackblack(x, y) != 0 || (sc.GetPosition(x, y) != null && sc.GetPosition(x, y).GetComponent<Chesspieces>().GetPlayer() == sc.GetCurrentPlayer()))
                return true;
            else
                return false;
        }
        else
        {
            if (!sc.PositionOnBoard(x, y) || sc.Getattackwhite(x, y) != 0 || (sc.GetPosition(x, y) != null && sc.GetPosition(x, y).GetComponent<Chesspieces>().GetPlayer() == sc.GetCurrentPlayer()))
                return true;
            else
                return false;
        }
    }


    public void SetAttackPlate(int x, int y, string name, string color)
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        switch (name)
        {
            case "black_queen":
            case "white_queen":
                for (int i = 0; i < 3; i++)
                    for (int j = 0; j < 3; j++)
                    {
                        if (j == i && i == 0)
                            continue;
                        LineMoveAttack(movecoord[i], movecoord[j], color, x, y);
                    }
                break;

            case "black_knight":
            case "white_knight":
                LMoveAttack(color, x, y);
                break;

            case "black_bishop":
            case "white_bishop":
                for (int i = 1; i < 3; i++)
                    for (int j = 1; j < 3; j++)
                        LineMoveAttack(movecoord[i], movecoord[j], color, x, y);
                break;
            case "black_king":
            case "white_king":
                SurroundMoveAttack(color, x, y);
                break;
            case "black_rook":
            case "white_rook":
                for (int i = 1; i < 3; i++)
                    LineMoveAttack(movecoord[i], 0, color, x, y);
                for (int i = 1; i < 3; i++)
                    LineMoveAttack(0, movecoord[i], color, x, y);
                break;
            case "black_pawn":
                PawnMoveAttack(x, y - 1, color);
                break;
            case "white_pawn":
                PawnMoveAttack(x, y + 1, color);
                break;
        }
    }
/*
    public void CheckPins(int x,int y, string color)
    {
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
            {
                if (j == 0 && movecoord[i] == 0)
                    continue;
                LineCheckpins(movecoord[i], movecoord[j], x, y, color);
            }
    }

    public void LineCheckpins(int xIncrement, int yIncrement, int xBoard, int yBoard, string color)
    {
        Game sc = controller.GetComponent<Game>();
        int x = xBoard + xIncrement;
        int y = yBoard + yIncrement;

        while (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y) == null)
        {
            x += xIncrement;
            y += yIncrement;
        }
        if (color == "white")
            if (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y).GetComponent<Chesspieces>().GetPlayer() == color && sc.Getattackblack(x, y) == -1)
            {
                sc.GetPosition(x, y).GetComponent<Chesspieces>().SetPinned(true);
            }
        else if (color == "black")
           if (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y).GetComponent<Chesspieces>().GetPlayer() == color && sc.Getattackwhite(x, y) == -1) 
           {
                sc.GetPosition(x, y).GetComponent<Chesspieces>().SetPinned(true);
           }
    }*/


    public void LineMoveAttack(int xIncrement, int yIncrement, string color, int xBoard, int yBoard)
    {
        Game sc = controller.GetComponent<Game>();
        int x = xBoard + xIncrement;
        int y = yBoard + yIncrement;

        while (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y) == null )
        {
            if (color == "white")
            {
                controller.GetComponent<Game>().Setattackwhite(x, y);
                controller.GetComponent<Game>().Setmovewhite(x, y);
            }
            else
            {
                controller.GetComponent<Game>().Setattackblack(x, y);
                controller.GetComponent<Game>().Setmoveblack(x, y);
            }

            x += xIncrement;
            y += yIncrement;
        }

        if (sc.PositionOnBoard(x, y))
        {
            if (color == "white")
            {
                controller.GetComponent<Game>().Setattackwhite(x, y);
                controller.GetComponent<Game>().Setmovewhite(x, y);
                if (sc.GetPosition(x, y).name == "black_king")
                    LineMoveAttack(xIncrement, yIncrement, color, x, y);
            }
            else
            {
                controller.GetComponent<Game>().Setattackblack(x, y);
                controller.GetComponent<Game>().Setmoveblack(x, y);
                if (sc.GetPosition(x, y).name == "white_king")
                    LineMoveAttack(xIncrement, yIncrement, color, x, y);
            }
        }
    }

    public void LMoveAttack(string color, int xBoard, int yBoard)
    {
        PointMovePlate(xBoard + 1, yBoard + 2, color, true);
        PointMovePlate(xBoard + 1, yBoard - 2, color, true);
        PointMovePlate(xBoard - 1, yBoard + 2, color, true);
        PointMovePlate(xBoard - 1, yBoard - 2, color, true);
        PointMovePlate(xBoard + 2, yBoard + 1, color, true);
        PointMovePlate(xBoard - 2, yBoard + 1, color, true);
        PointMovePlate(xBoard + 2, yBoard - 1, color, true);
        PointMovePlate(xBoard - 2, yBoard - 1, color, true);
    }

    public void SurroundMoveAttack(string color, int xBoard, int yBoard)
    {
        PointMovePlate(xBoard, yBoard + 1, color, false);
        PointMovePlate(xBoard, yBoard - 1, color, false);
        PointMovePlate(xBoard - 1, yBoard - 1, color, false);
        PointMovePlate(xBoard - 1, yBoard, color, false);
        PointMovePlate(xBoard + 1, yBoard, color, false);
        PointMovePlate(xBoard - 1, yBoard + 1, color, false);
        PointMovePlate(xBoard + 1, yBoard - 1, color, false);
        PointMovePlate(xBoard + 1, yBoard + 1, color, false);
    }

    public void PointMovePlate(int x, int y, string color, bool ok)
    {
        Game sc = controller.GetComponent<Game>();
        if (sc.PositionOnBoard(x, y))
        {
            if (color == "white")
            {
                if (ok)
                {
                    controller.GetComponent<Game>().Setattackwhite(x, y);
                    controller.GetComponent<Game>().Setmovewhite(x, y);
                }
                else
                    controller.GetComponent<Game>().Setattackwhiteking(x, y);
            }
            else
            {
                if (ok)
                {
                    controller.GetComponent<Game>().Setattackblack(x, y);
                    controller.GetComponent<Game>().Setmoveblack(x, y);
                }
                else
                    controller.GetComponent<Game>().Setattackwhiteking(x, y);
            }
        }
    }

    public void PawnMoveAttack(int x, int y, string color)
    {
        Game sc = controller.GetComponent<Game>();

        if (sc.PositionOnBoard(x, y))
        {

            if (sc.GetPosition(x, y) == null)
            {
                if (color == "white")
                    controller.GetComponent<Game>().Setmovewhite(x, y);
                else
                    controller.GetComponent<Game>().Setmoveblack(x, y);

                if (color == "white" && sc.GetPosition(x ,y - 1).GetComponent<Chesspieces>().whitehasmoved == 0 && sc.GetPosition(x, y + 1) == null)
                {
                    controller.GetComponent<Game>().Setmovewhite(x, y + 1);
                }
                else if (color == "black" && sc.GetPosition(x, y + 1).GetComponent<Chesspieces>().blackhasmoved == 0 && sc.GetPosition(x, y - 1) == null)
                {
                    controller.GetComponent<Game>().Setmoveblack(x, y - 1);
                }
            }



            if (sc.PositionOnBoard(x + 1, y))
            {
                if (color == "white")
                    controller.GetComponent<Game>().Setattackwhite(x+1, y);
                else
                    controller.GetComponent<Game>().Setattackblack(x+1, y);
            }

            if (sc.PositionOnBoard(x - 1, y))
            {
                if (color == "white")
                    controller.GetComponent<Game>().Setattackwhite(x-1, y);
                else
                    controller.GetComponent<Game>().Setattackblack(x-1, y);
            }

            //enpasant
            if (color == "white")
            {
                if (sc.PositionOnBoard(x + 1, y - 1) && sc.GetPosition(x + 1, y - 1) != null && sc.GetPosition(x + 1, y - 1).GetComponent<Chesspieces>().GetPlayer() != color && sc.GetPosition(x + 1, y - 1).GetComponent<Chesspieces>().Isenpassant())
                {
                    if (sc.GetPosition(x + 1, y - 1).GetComponent<Chesspieces>().passantturn == sc.GetComponent<Game>().GetTurns())
                    {
                        controller.GetComponent<Game>().Setattackwhite(x + 1, y - 1);
                    }
                }

                if (sc.PositionOnBoard(x - 1, y - 1) && sc.GetPosition(x - 1, y - 1) != null && sc.GetPosition(x - 1, y - 1).GetComponent<Chesspieces>().GetPlayer() != color && sc.GetPosition(x - 1, y - 1).GetComponent<Chesspieces>().Isenpassant())
                {
                    if (sc.GetPosition(x - 1, y - 1).GetComponent<Chesspieces>().passantturn == sc.GetComponent<Game>().GetTurns())
                    {
                        controller.GetComponent<Game>().Setattackwhite(x - 1, y - 1);
                    }
                }
            }
            else if (color == "black")
            {
                if (sc.PositionOnBoard(x + 1, y + 1) && sc.GetPosition(x + 1, y + 1) != null && sc.GetPosition(x + 1, y + 1).GetComponent<Chesspieces>().GetPlayer() != color && sc.GetPosition(x + 1, y + 1).GetComponent<Chesspieces>().Isenpassant())
                {
                    if (sc.GetPosition(x + 1, y + 1).GetComponent<Chesspieces>().passantturn == sc.GetComponent<Game>().GetTurns())
                    {
                        controller.GetComponent<Game>().Setattackblack(x + 1, y + 1);
                    }
                }
               
                if (sc.PositionOnBoard(x - 1, y + 1) && sc.GetPosition(x - 1, y + 1) != null && sc.GetPosition(x - 1, y + 1).GetComponent<Chesspieces>().GetPlayer() != color && sc.GetPosition(x - 1, y + 1).GetComponent<Chesspieces>().Isenpassant())
                {
                    if (sc.GetPosition(x - 1, y + 1).GetComponent<Chesspieces>().passantturn == sc.GetComponent<Game>().GetTurns())
                    {
                        controller.GetComponent<Game>().Setattackblack(x - 1, y + 1);
                    }
                }
            }
        }
    }








}

