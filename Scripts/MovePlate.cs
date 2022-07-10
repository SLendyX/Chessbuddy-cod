using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
        
        if (attack)
        {
            
                GameObject cp = controller.GetComponent<Game>().GetPosition(matriceX, matriceY);

                if (cp != null)
                {
                    if (cp.name == "white_king") controller.GetComponent<Game>().Winner("black");
                    if (cp.name == "black_king") controller.GetComponent<Game>().Winner("white");

                }

            if (cp != null)
                Destroy(cp);
            else if (controller.GetComponent<Game>().GetPosition(matriceX, matriceY - 1) != null)
                Destroy(controller.GetComponent<Game>().GetPosition(matriceX, matriceY - 1));
           else if(controller.GetComponent<Game>().GetPosition(matriceX, matriceY + 1) != null)
                Destroy(controller.GetComponent<Game>().GetPosition(matriceX, matriceY + 1));
        }   

        controller.GetComponent<Game>().SetPositionEmpty(refrence.GetComponent<Chesspieces>().GetXBoard(),
        refrence.GetComponent<Chesspieces>().GetYBoard());

        refrence.GetComponent<Chesspieces>().setXBoard(matriceX);
        refrence.GetComponent<Chesspieces>().setYBoard(matriceY);
        refrence.GetComponent<Chesspieces>().SetCoords();

        controller.GetComponent<Game>().SetPosition(refrence);

        controller.GetComponent<Game>().NextTurn();

        refrence.GetComponent<Chesspieces>().DestroyMovePlates();



        //castle
        if (controller.GetComponent<Game>().GetPosition(matriceX, matriceY).name == "white_king" && refrence.GetComponent<Chesspieces>().Getkingmove() == 0 && controller.GetComponent<Game>().GetPosition(matriceX, matriceY).GetComponent<Chesspieces>().Getrookmove() == 0)
        {
            if (controller.GetComponent<Game>().GetPosition(matriceX+1, matriceY) != null && controller.GetComponent<Game>().GetPosition(matriceX+1, matriceY).name == "white_rook")
            {
                Destroy(controller.GetComponent<Game>().GetPosition(matriceX + 1, matriceY));

                GameObject obj = controller.GetComponent<Game>().Create("white_rook", matriceX - 1, matriceY);

                controller.GetComponent<Game>().SetPositionEmpty(matriceX + 1, matriceY);

                obj.GetComponent<Chesspieces>().setXBoard(matriceX - 1);
                obj.GetComponent<Chesspieces>().setYBoard(matriceY);
                obj.GetComponent<Chesspieces>().SetCoords();

                controller.GetComponent<Game>().SetPosition(obj);
            }else if (controller.GetComponent<Game>().GetPosition(matriceX-2, matriceY) != null && controller.GetComponent<Game>().GetPosition(matriceX-2, matriceY).name == "white_rook")
            {
                Destroy(controller.GetComponent<Game>().GetPosition(matriceX - 2, matriceY));

                GameObject obj = controller.GetComponent<Game>().Create("white_rook", matriceX + 1, matriceY);

                controller.GetComponent<Game>().SetPositionEmpty(matriceX -2, matriceY);

                obj.GetComponent<Chesspieces>().setXBoard(matriceX + 1);
                obj.GetComponent<Chesspieces>().setYBoard(matriceY);
                obj.GetComponent<Chesspieces>().SetCoords();

                controller.GetComponent<Game>().SetPosition(obj);
            }
        }else if (controller.GetComponent<Game>().GetPosition(matriceX, matriceY).name == "black_king" && refrence.GetComponent<Chesspieces>().Getkingmove() == 0 && controller.GetComponent<Game>().GetPosition(matriceX, matriceY).GetComponent<Chesspieces>().Getrookmove() == 0)
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
                else if (controller.GetComponent<Game>().GetPosition(matriceX - 2, matriceY) != null && controller.GetComponent<Game>().GetPosition(matriceX-2, matriceY).name == "black_rook")
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
            if(matriceY == 7)
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
}
