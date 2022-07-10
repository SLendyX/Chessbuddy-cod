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

    int enpassantX = -1;
    int enpassantY = -1;


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

        int piesaX = refrence.GetComponent<Chesspieces>().GetXBoard();
        int piesaY = refrence.GetComponent<Chesspieces>().GetYBoard();

        bool ok = true;


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


        controller.GetComponent<Game>().Setattackwhitenull();
        controller.GetComponent<Game>().Setattackblacknull();
        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
            {
                if (controller.GetComponent<Game>().GetPosition(i, j) != null)
                {
                    SetAttackPlate(i, j, controller.GetComponent<Game>().GetPosition(i, j).name, controller.GetComponent<Game>().GetPosition(i, j).GetComponent<Chesspieces>().GetPlayer());
                }
            }

        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
            {

                if (controller.GetComponent<Game>().GetPosition(i, j) != null)
                {
                    if (controller.GetComponent<Game>().GetCurrentPlayer() == "white")
                    {
                        if (controller.GetComponent<Game>().GetPosition(i, j).name == "white_king" && controller.GetComponent<Game>().Getattackblack(i, j) == -1)
                        {
                            controller.GetComponent<Game>().SetPositionEmpty(matriceX, matriceY);

                            refrence.GetComponent<Chesspieces>().setXBoard(piesaX);
                            refrence.GetComponent<Chesspieces>().setYBoard(piesaY);

                            refrence.GetComponent<Chesspieces>().SetCoords();

                            controller.GetComponent<Game>().SetPosition(refrence);
                            ok = false;
                        }
                    }
                    else
                        if (controller.GetComponent<Game>().GetPosition(i, j).name == "black_king" && controller.GetComponent<Game>().Getattackwhite(i, j) == -1)
                        {
                            controller.GetComponent<Game>().SetPositionEmpty(matriceX, matriceY);

                            refrence.GetComponent<Chesspieces>().setXBoard(piesaX);
                            refrence.GetComponent<Chesspieces>().setYBoard(piesaY);

                            refrence.GetComponent<Chesspieces>().SetCoords();

                        controller.GetComponent<Game>().SetPosition(refrence);
                        ok = false;
                        }
                }
            }


        /*
         * if (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y) != null)
                if (sc.GetPosition(x, y).name == "white_king" || sc.GetPosition(x, y).name == "black_king")
                {
                    x += xIncrement;
                    y += yIncrement;

                }
        */



        //for (int i = 0; i < 8; i++)
        // for (int j = 0; j < 8; j++)
        // if (controller.GetComponent<Game>().GetPosition(i, j).name == "white_king" && controller.GetComponent<Game>().Getattackwhite(i, j) == -1 || controller.GetComponent<Game>().GetPosition(i, j).name == "black_king" && controller.GetComponent<Game>().Getattackblack(i, j) == -1)
        //  controller.GetComponent<Chesspieces>().SetCheck(false);
        refrence.GetComponent<Chesspieces>().DestroyMovePlates();
        if (ok)
        {

            controller.GetComponent<Game>().NextTurn();

            //Cheking and Chekmate

            controller.GetComponent<Game>().Setattackwhitenull();
            controller.GetComponent<Game>().Setattackblacknull();

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
                    }
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


    public bool Checkmate()
    {


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
                        if (j == 0 && movecoord[i] == 0)
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

        while (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y) == null)
        {
            if (color == "white")
                controller.GetComponent<Game>().Setattackwhite(x, y);
            else
                controller.GetComponent<Game>().Setattackblack(x, y);

            x += xIncrement;
            y += yIncrement;
        }

        if (sc.PositionOnBoard(x, y))
        {
            if (color == "white")
                controller.GetComponent<Game>().Setattackwhite(x, y);
            else
                controller.GetComponent<Game>().Setattackblack(x, y);
        }
    }

    public void LMoveAttack(string color, int xBoard, int yBoard)
    {
        PointMovePlate(xBoard + 1, yBoard + 2, color);
        PointMovePlate(xBoard + 1, yBoard - 2, color);
        PointMovePlate(xBoard - 1, yBoard + 2, color);
        PointMovePlate(xBoard - 1, yBoard - 2, color);
        PointMovePlate(xBoard + 2, yBoard + 1, color);
        PointMovePlate(xBoard - 2, yBoard + 1, color);
        PointMovePlate(xBoard + 2, yBoard - 1, color);
        PointMovePlate(xBoard - 2, yBoard - 1, color);
    }

    public void SurroundMoveAttack(string color, int xBoard, int yBoard)
    {
        PointMovePlate(xBoard, yBoard + 1, color);
        PointMovePlate(xBoard, yBoard - 1, color);
        PointMovePlate(xBoard - 1, yBoard - 1, color);
        PointMovePlate(xBoard - 1, yBoard, color);
        PointMovePlate(xBoard + 1, yBoard, color);
        PointMovePlate(xBoard - 1, yBoard + 1, color);
        PointMovePlate(xBoard + 1, yBoard - 1, color);
        PointMovePlate(xBoard + 1, yBoard + 1, color);
    }

    public void PointMovePlate(int x, int y, string color)
    {
        Game sc = controller.GetComponent<Game>();
        if (sc.PositionOnBoard(x, y))
        {
            if (color == "white")
                controller.GetComponent<Game>().Setattackwhite(x, y);
            else
                controller.GetComponent<Game>().Setattackblack(x, y);
        }
    }

    public void PawnMoveAttack(int x, int y, string color)
    {
        Game sc = controller.GetComponent<Game>();

        if (sc.PositionOnBoard(x, y))
        {

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

        }
    }
}
