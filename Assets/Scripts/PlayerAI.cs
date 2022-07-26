using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAI : MonoBehaviour
{
    public GameObject controller;

    public GameObject movePlate;

    private int[,] positions = new int[8, 8];

    private int[,] blackattack = new int[8, 8];
    private int[,] whiteattack = new int[8, 8];
    private bool[,] checks = new bool[8, 8];

    private int plymoves = 0;

    private int[] moves = new int[400];
    private int[] piececoord = new int[400];
    private int[] pieces = new int[200];
    private int k =0;

    private int infinity = 999;

    private int[] bestmovewhite = new int[2];
    private int[] bestmoveblack = new int[2];

    private int kingX;
    private int kingY;

    public int[] movecoord = new int[] { 0, 1, -1 };


    /*
    none = 0
    king = 1
    pawn = 2
    knight = 3
    bishop = 4
    rook = 5
    queen = 6
    
    white = 8
    black = 16
     */


    // Start is called before the first frame update

    // Update is called once per frame
    public void Execute()
    {
           controller = GameObject.FindGameObjectWithTag("GameController");
           Game sc = controller.GetComponent<Game>();

       
            CopyMainBoard();
            GenerateAllMoves(GetKingX(sc.GetCurrentPlayer()), GetKingY(sc.GetCurrentPlayer()));
            GenerateLegalMoves(sc.GetCurrentPlayer());

      // Debug.Log(k);
       // for (int i=0; i<k; i++)
         //  Debug.Log(pieces[i] + " " + moves[i] + " " + piececoord[i]);

        minmax(4, sc.GetCurrentPlayer(), -infinity, +infinity);

        Debug.Log(plymoves);

        if (sc.GetCurrentPlayer() == "black")
        {
            int xBoard = bestmoveblack[0] / 10;
            int yBoard = bestmoveblack[0] % 10;

            float x = xBoard;
            float y = yBoard;

            x *= 1.12f;
            y *= 1.11f;

            x += -3.9f;
            y += -3.9f;

            GameObject move = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);

            xBoard = bestmoveblack[1] / 10;
            yBoard = bestmoveblack[1] % 10;

            x = xBoard;
            y = yBoard;

            x *= 1.12f;
            y *= 1.11f;

            x += -3.9f;
            y += -3.9f;

            GameObject piece = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);
        }
        else
        {
            int xBoard = bestmovewhite[0] / 10;
            int yBoard = bestmovewhite[0] % 10;

            float x = xBoard;
            float y = yBoard;

            x *= 1.12f;
            y *= 1.11f;

            x += -3.9f;
            y += -3.9f;

            GameObject move = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);

            xBoard = bestmovewhite[1] / 10;
            yBoard = bestmovewhite[1] % 10;

            x = xBoard;
            y = yBoard;

            x *= 1.12f;
            y *= 1.11f;

            x += -3.9f;
            y += -3.9f;

            GameObject piece = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);
        }

            /* MovePlate mpScript = controller.GetComponent<MovePlate>();
             mpScript.SetReference(sc.GetPosition(0, 6));
             mpScript.SetCoords(0, 5);*/

       

         /*  if (sc.GetCurrentPlayer() == "black")
           {
               for (int i = 0; i < 8; i++)
                   for (int j = 0; j < 8; j++)
                   {
                       if (sc.GetCurrentPlayer() == "black")
                       {
                           if (controller.GetComponent<Game>().GetPosition(i, j) != null && controller.GetComponent<Game>().GetPosition(i, j).GetComponent<Chesspieces>().GetPlayer() == "black")
                           {
                               GenerateMove(i, j, controller.GetComponent<Game>().GetPosition(i, j).name);
                           }
                       }
                   }
           }
       */
    }




    public int Evaluate()
    {
        int score = 0;
        for(int i=0; i<8; i++)
            for(int j=0; j<8; j++)
                switch (positions[i,j])
                {
                    case 166:
                        score -= 90;
                        break;
                    case 86:
                        score += 90;
                        break;
                    case 163:
                    case 164:
                        score -= 30;
                        break;
                    case 83:
                    case 84:
                        score += 30;
                        break;
                    case 161:
                        score -= 900;
                        break;
                    case 81:
                        score += 900;
                        break;
                    case 165:
                        score -= 50;
                        break;
                    case 85:
                        score += 50;
                        break;
                    case 162:
                        score -= 10;
                        break;
                    case 82:
                        score += 10;
                        break;
                }
        return score;
    }

   public int minmax(int depth, string color, int alpha, int beta)
   {
        if (depth == 0)
            return Evaluate();

        GenerateAllMoves(GetKingX(color), GetKingY(color));

        GenerateLegalMoves(color);

        int[] move = new int [200], position = new int [200], piese = new int [200];
        int movenr = k;
        Debug.Log(movenr);

        for (int i = 0; i < movenr; i++) {
            move[i] = moves[i];
            position[i] = piececoord[i];
            piese[i] = pieces[i];

        }



        if (color == "white")
        {
            int counter = 0;
            int maxeval = -infinity;
            for(int i=0; i< movenr; i++)
            {
                if (piese[i] / 10 == 16)
                    continue;
                plymoves ++;
                counter++;

               // Debug.Log(piese[i] + " " + move[i] + " " + position[i]);

                int pieseatacata = 0;
                if (positions[move[i] / 10, move[i] % 10] != 0)
                    pieseatacata = positions[move[i] / 10, move[i] % 10];

                MakeMove(move[i] / 10, move[i] % 10, position[i] / 10, position[i] % 10, piese[i]);

                int eval = minmax(depth - 1, "black", alpha, beta);

                UnMakeMove(move[i] / 10, move[i] % 10, position[i] / 10, position[i] % 10, piese[i]);

                if (pieseatacata != 0)
                    positions[move[i] / 10, move[i] % 10] = pieseatacata;

                if (eval > maxeval)
                {
                    maxeval = eval;
                    bestmovewhite[0] = move[i];
                    bestmovewhite[1] = position[i];
                }
                if (alpha < eval)
                    alpha = eval;
                if (beta <= alpha)
                    break;
            }
            Debug.Log(counter);
            return maxeval;
        }
        else
        {
            int counter = 0;
            int mineval = infinity;
            for (int i = 0; i < movenr; i++)
            {
                if (piese[i] / 10 == 8)
                    continue;

                plymoves++;
                counter++;

                //Debug.Log(piese[i] + " " + move[i] + " " + position[i]);

                int pieseatacata = 0;
                if (positions[move[i] / 10, move[i] % 10] != 0)
                    pieseatacata = positions[move[i] / 10, move[i] % 10];

                MakeMove(move[i] / 10, move[i] % 10, position[i] / 10, position[i] % 10, piese[i]);

                int eval = minmax(depth - 1, "white", alpha, beta);

                UnMakeMove(move[i] / 10, move[i] % 10, position[i] / 10, position[i] % 10, piese[i]);

                if (pieseatacata != 0)
                    positions[move[i] / 10, move[i] % 10] = pieseatacata;

                if (eval < mineval)
                {
                    mineval = eval;
                    bestmoveblack[0] = move[i];
                    bestmoveblack[1] = position[i];
                }

                if (eval < beta)
                    beta = eval;
                if (beta <= alpha)
                    break;
            }
            Debug.Log(counter);
            return mineval;
        }

        return 0;
   }

    public void MakeMove(int x, int y, int xBoard, int yBoard, int piece)
    {
        positions[xBoard, yBoard] = 0;
        positions[x, y] = piece;
    }

    public void UnMakeMove(int x, int y, int xBoard, int yBoard, int piece)
    {
        positions[xBoard, yBoard] = piece;
        positions[x, y] = 0;
    }



    public void CopyMainBoard()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        Game sc = controller.GetComponent<Game>();
        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
            {
                if (sc.GetPosition(i, j) != null)
                    SetPositions(i, j, sc.GetPosition(i, j).name);
                else
                    SetPositions(i, j, "null");
            }
    }

    public void SetPositions(int x, int y, string name)
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        Game sc = controller.GetComponent<Game>();

        switch (name)
        {
            case "black_queen":
                positions[x, y] = 166;
                break;
            case "white_queen":
                positions[x, y] = 86;
                break;
            case "black_knight":
                positions[x, y] = 163;
                    break;
            case "black_bishop":
                positions[x, y] = 164;
                break;
            case "white_knight":
                positions[x, y] = 83;
                    break;
            case "white_bishop":
                positions[x, y] = 84;
                break;
            case "black_king":
                positions[x, y] = 161;
                break;
            case "white_king":
                positions[x, y] = 81;
                break;
            case "black_rook":
                positions[x, y] = 165;
                break;
            case "white_rook":
                positions[x, y] = 85;
                break;
            case "black_pawn":
                positions[x, y] = 162;
                break;
            case "white_pawn":
                positions[x, y] = 82;
                break;
            case "null":
                positions[x, y] = 0;
                break;
        }
    }

    public int GetKingX(string color)
    {
        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
            {
                if ((positions[i, j] == 161 && color == "black") || (positions[i, j] == 81 && color == "white"))
                {
                    return i;
                }
            }
        return -1;
    }

    public int GetKingY(string color)
    {
        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
            {
                if ((positions[i, j] == 161 && color == "black") || (positions[i, j] == 81 && color == "white"))
                {
                    return j;
                }
            }
        return -1;
    }

    public void GenerateAllMoves(int x, int y)
    {
        GenerateAttack();
        Checks(x, y, positions[x, y]);
    }

    public void GenerateAttack()
    {
        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
            {
                blackattack[i, j] = 0;
                whiteattack[i, j] = 0;
            }

                for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
                if (positions[i, j] != 0)
                {
                    Attack(i, j, positions[i, j]);
                }
    }

    public void GenerateLegalMoves(string color)
    {
            for (int i = 0; i < k; i++)
            {
            moves[i] = 0;
            piececoord[i] = 0;
            pieces[i] =0;
            }
        k = 0;

        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
                if (positions[i, j] != 0)
                {
                    Moves(i, j, positions[i, j], color);
                }
    }


    public void Moves(int x, int y, int piece, string color)
    {
        switch (piece)
        {
            case 166:
            case 86:
                for (int i = 0; i < 3; i++)
                    for (int j = 0; j < 3; j++)
                    {
                        if (j == i && i == 0)
                            continue;
                        LineMove(movecoord[i], movecoord[j], x, y, piece, color);
                    }
                break;

            case 163:
            case 83:
                LMove(piece, x, y, color);
                break;

            case 164:
            case 84:
                for (int i = 1; i < 3; i++)
                    for (int j = 1; j < 3; j++)
                        LineMove(movecoord[i], movecoord[j], x, y, piece, color);
                break;
            case 81:
            case 161:
                SurroundMove(piece, x, y, color);
                break;
            case 165:
            case 85:
                for (int i = 1; i < 3; i++)
                    LineMove(movecoord[i], 0, x, y, piece, color);
                for (int i = 1; i < 3; i++)
                    LineMove(0, movecoord[i], x, y, piece, color);
                break;
            case 162:
                PawnMove(x, y - 1, piece, color, x, y);
                break;
            case 82:
                PawnMove(x, y + 1, piece, color, x, y);
                break;
        }
    }


    public void Attack(int x, int y, int piece) { 
                switch (piece)
                {
                 case 166:
                 case 86:
                     for (int i = 0; i < 3; i++)
                         for (int j = 0; j < 3; j++)
                         {
                             if (j == i && i == 0)
                                 continue;
                             LineAttack(movecoord[i], movecoord[j], x, y, piece);
                         }
                     break;

                 case 163:
                 case 83:
                     LAttack(piece, x, y);
                     break;

                 case 164:
                 case 84:
                     for (int i = 1; i < 3; i++)
                         for (int j = 1; j < 3; j++)
                             LineAttack(movecoord[i], movecoord[j], x, y, piece);
                     break;
                 case 81:
                 case 161:
                     SurroundAttack(piece, x, y);
                     break;
                 case 165:
                 case 85:
                     for (int i = 1; i < 3; i++)
                         LineAttack(movecoord[i], 0, x, y, piece);
                     for (int i = 1; i < 3; i++)
                         LineAttack(0, movecoord[i], x, y, piece);
                     break;
                 case 162:
                     PawnAttack(x, y - 1, piece);
                     break;
                 case 82:
                     PawnAttack(x, y + 1, piece);
                     break;
                }
    }

    public void LineMove(int xIncrement, int yIncrement, int xBoard, int yBoard, int piece, string color)
    {
        Game sc = controller.GetComponent<Game>();
        int kingX, kingY;
        kingX = GetKingX(color);
        kingY = GetKingY(color);
        if (((blackattack[kingX, kingY] == 0 && color == "white") || (whiteattack[kingX, kingY] == 0 && color == "black")) && !checks[xBoard, yBoard])
        {
            int x = xBoard + xIncrement;
            int y = yBoard + yIncrement;

            while (PositionOnBoard(x, y) && positions[x, y] == 0)
            {
                moves[k] = x * 10 + y;
                piececoord[k] = xBoard * 10 + yBoard;
                pieces[k] = piece;
                k++;

                x += xIncrement;
                y += yIncrement;
            }

            if (PositionOnBoard(x, y) && positions[x, y]/10 != piece/10)
            {
                moves[k] = x * 10 + y;
                piececoord[k] = xBoard * 10 + yBoard;
                pieces[k] = piece;
                k++;
            }
        }
        else
        {
            int x = xBoard + xIncrement;
            int y = yBoard + yIncrement;

            while (PositionOnBoard(x, y) && positions[x, y] == 0)
            {
                if (checks[x, y])
                {
                    moves[k] = x * 10 + y;
                    piececoord[k] = xBoard * 10 + yBoard;
                    pieces[k] = piece;
                    k++;
                }
                x += xIncrement;
                y += yIncrement;
            }

            if (PositionOnBoard(x, y) && positions[x, y] / 10 != piece / 10)
            {
                if (checks[x, y])
                {
                    moves[k] = x * 10 + y;
                    piececoord[k] = xBoard * 10 + yBoard;
                    pieces[k] = piece;
                    k++;
                }
            }
        }
    }


    public bool PositionOnBoard(int x, int y)
    {
        if (x < 0 || y < 0 || x >= positions.GetLength(0) || y >= positions.GetLength(0))
            return false;
        return true;
    }

    public void LineAttack(int xIncrement, int yIncrement, int xBoard, int yBoard, int piece)
    {
        int x = xBoard + xIncrement;
        int y = yBoard + yIncrement;

        while (PositionOnBoard(x, y) && PositionOnBoard(x, y))
        { 
                if (piece / 10 == 8)
                    whiteattack[x, y] = -1;
                else
                    blackattack[x, y] = -1;
         
            x += xIncrement;
            y += yIncrement;
        }

        if (PositionOnBoard(x, y))
        {
            if (piece / 10 == 8) { 
                    whiteattack[x, y] = -1;
                if (positions[x,y] == 161)
                        LineAttack(xIncrement, yIncrement, x, y, piece);
            }
            else
            {
                blackattack[x, y] = -1;
                if (positions[x,y] == 81)
                    LineAttack(xIncrement, yIncrement, x, y, piece);
            }
        }
    }

    public void LMove(int piece, int xBoard, int yBoard, string color)
    {
        PointMove(xBoard + 1, yBoard + 2, piece, color, xBoard, yBoard);
        PointMove(xBoard + 1, yBoard - 2, piece, color, xBoard, yBoard);
        PointMove(xBoard - 1, yBoard + 2, piece, color, xBoard, yBoard);
        PointMove(xBoard - 1, yBoard - 2, piece, color, xBoard, yBoard);
        PointMove(xBoard + 2, yBoard + 1, piece, color, xBoard, yBoard);
        PointMove(xBoard - 2, yBoard + 1, piece, color, xBoard, yBoard);
        PointMove(xBoard + 2, yBoard - 1, piece, color, xBoard, yBoard);
        PointMove(xBoard - 2, yBoard - 1, piece, color, xBoard, yBoard);
    }

    public void SurroundMove(int piece, int xBoard, int yBoard, string color)
    {
        PointMove(xBoard, yBoard + 1, piece, color, xBoard, yBoard);
        PointMove(xBoard, yBoard - 1, piece, color, xBoard, yBoard);
        PointMove(xBoard - 1, yBoard - 1, piece, color, xBoard, yBoard);
        PointMove(xBoard - 1, yBoard, piece, color, xBoard, yBoard);
        PointMove(xBoard + 1, yBoard, piece, color, xBoard, yBoard);
        PointMove(xBoard - 1, yBoard + 1, piece, color, xBoard, yBoard);
        PointMove(xBoard + 1, yBoard - 1, piece, color, xBoard, yBoard);
        PointMove(xBoard + 1, yBoard + 1, piece, color, xBoard, yBoard);
    }




    public void LAttack(int piece, int xBoard, int yBoard)
    {
        PointAttack(xBoard + 1, yBoard + 2, piece);
        PointAttack(xBoard + 1, yBoard - 2, piece);
        PointAttack(xBoard - 1, yBoard + 2, piece);
        PointAttack(xBoard - 1, yBoard - 2, piece);
        PointAttack(xBoard + 2, yBoard + 1, piece);
        PointAttack(xBoard - 2, yBoard + 1, piece);
        PointAttack(xBoard + 2, yBoard - 1, piece);
        PointAttack(xBoard - 2, yBoard - 1, piece);
    }

    public void SurroundAttack(int piece, int xBoard, int yBoard)
    {
        PointAttack(xBoard, yBoard + 1, piece);
        PointAttack(xBoard, yBoard - 1, piece);
        PointAttack(xBoard - 1, yBoard - 1, piece);
        PointAttack(xBoard - 1, yBoard, piece);
        PointAttack(xBoard + 1, yBoard, piece);
        PointAttack(xBoard - 1, yBoard + 1, piece);
        PointAttack(xBoard + 1, yBoard - 1, piece);
        PointAttack(xBoard + 1, yBoard + 1, piece);
    }

    public void PointMove(int x, int y, int piece, string color, int xBoard, int yBoard)
    {
        Game sc = controller.GetComponent<Game>();
        int kingX, kingY;
        kingX = GetKingX(color);
        kingY = GetKingY(color);

        if (PositionOnBoard(x, y))
        {
            if ((piece == 81 && blackattack[xBoard, yBoard] == 0) || (piece == 161 && whiteattack[xBoard,yBoard] == 0))
            {

                if (positions[x,y] == 0)
                {
                    moves[k] = x * 10 + y;
                    piececoord[k] = xBoard * 10 + yBoard;
                    pieces[k] = piece;
                    k++;
                }
                else if (piece/10 != positions[x,y]/10)
                {
                    moves[k] = x * 10 + y;
                    piececoord[k] = xBoard * 10 + yBoard;
                    pieces[k] = piece;
                    k++;
                }
            }
            else
            {

                if (((blackattack[kingX,kingY] == 0 && color == "white") || (whiteattack[kingX, kingY] == 0 && color == "black")) && !checks[xBoard, yBoard])
                {
                    if (positions[x,y] == 0)
                    {
                        moves[k] = x * 10 + y;
                        piececoord[k] = xBoard * 10 + yBoard;
                        pieces[k] = piece;
                        k++;
                    }
                    else if (piece / 10 != positions[x, y] / 10)
                    {
                        moves[k] = x * 10 + y;
                        piececoord[k] = xBoard * 10 + yBoard;
                        pieces[k] = piece;
                        k++;
                    }
                }
                else if (checks[x, y])
                {
                    if(positions[x, y] == 0)
                    {
                        moves[k] = x * 10 + y;
                        piececoord[k] = xBoard * 10 + yBoard;
                        pieces[k] = piece;
                        k++;
                    }
                    else if (piece / 10 != positions[x, y] / 10)
                    {
                        moves[k] = x * 10 + y;
                        piececoord[k] = xBoard * 10 + yBoard;
                        pieces[k] = piece;
                        k++;
                    }
                }
            }
        }
    }

    public void PawnMove(int x, int y, int piece, string color, int xBoard, int yBoard)
    {
        int kingX, kingY;
        kingX = GetKingX(color);
        kingY = GetKingY(color);
        
        if (((blackattack[kingX, kingY] == 0 && color == "white") || (whiteattack[kingX, kingY] == 0 && color == "black")) && !checks[xBoard, yBoard])
        {
            if (PositionOnBoard(x, y))
            {
                if (positions[x, y] == 0)
                {
                    moves[k] = x * 10 + y;
                    piececoord[k] = xBoard * 10 + yBoard;
                    pieces[k] = piece;
                    k++;
                    if (piece/10 == 8 && yBoard == 1 && positions[x, y + 1] == 0)
                    {
                        moves[k] = x * 10 + y+1;
                        piececoord[k] = xBoard * 10 + yBoard;
                        pieces[k] = piece;
                        k++;
                    }
                    else if (piece/10 == 16 && yBoard == 6 && positions[x, y - 1] == 0)
                    {
                        moves[k] = x * 10 + y-1;
                        piececoord[k] = xBoard * 10 + yBoard;
                        pieces[k] = piece;
                        k++;
                    }
                }

                if (PositionOnBoard(x + 1, y) && positions[x + 1, y] != 0 && piece/10 != positions[x+1,y]/10)
                {
                    moves[k] = (x + 1) * 10 + y;
                    piececoord[k] = xBoard * 10 + yBoard;
                    pieces[k] = piece;
                    k++;
                }

                if (PositionOnBoard(x - 1, y) && positions[x - 1, y] != 0 && piece / 10 != positions[x - 1, y] / 10)
                {
                    moves[k] = (x - 1) * 10 + y;
                    piececoord[k] = xBoard * 10 + yBoard;
                    pieces[k] = piece;
                    k++;
                }

                //enpasant
               /* if (controller.GetComponent<Game>().GetCurrentPlayer() == "white")
                {
                    if (sc.PositionOnBoard(x + 1, y - 1) && sc.GetPosition(x + 1, y - 1) != null && sc.GetPosition(x + 1, y - 1).GetComponent<Chesspieces>().player != player && sc.GetPosition(x + 1, y - 1).GetComponent<Chesspieces>().Isenpassant())
                    {
                        if (sc.GetPosition(x + 1, y - 1).GetComponent<Chesspieces>().passantturn == sc.GetComponent<Game>().GetTurns())
                        {
                            MovePlateAttackSpawn(x + 1, y);
                        }
                    }

                    if (sc.PositionOnBoard(x - 1, y - 1) && sc.GetPosition(x - 1, y - 1) != null && sc.GetPosition(x - 1, y - 1).GetComponent<Chesspieces>().player != player && sc.GetPosition(x - 1, y - 1).GetComponent<Chesspieces>().Isenpassant())
                    {
                        if (sc.GetPosition(x - 1, y - 1).GetComponent<Chesspieces>().passantturn == sc.GetComponent<Game>().GetTurns())
                        {
                            MovePlateAttackSpawn(x - 1, y);
                        }
                    }
                }
                else if (controller.GetComponent<Game>().GetCurrentPlayer() == "black")
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
                }*/
            }
        }
        else
        {
            if (PositionOnBoard(x, y))
            {
                if (positions[x, y] == 0)
                {
                    if (checks[x, y])
                    {
                        moves[k] = x * 10 + y;
                        piececoord[k] = xBoard * 10 + yBoard;
                        pieces[k] = piece;
                        k++;
                    }
                    if (piece / 10 == 8 && yBoard == 1 && positions[x, y + 1] == 0 && checks[x, y + 1])
                    {
                        moves[k] = x * 10 + y+1;
                        piececoord[k] = xBoard * 10 + yBoard;
                        pieces[k] = piece;
                        k++;
                    }
                    else if (piece / 10 == 16 && yBoard == 6 && positions[x, y - 1] == 0 && checks[x, y - 1])
                    {
                        moves[k] = x * 10 + y-1;
                        piececoord[k] = xBoard * 10 + yBoard;
                        pieces[k] = piece;
                        k++;
                    }
                }

                if (PositionOnBoard(x + 1, y) && positions[x + 1, y] != 0 && piece / 10 != positions[x + 1, y] / 10 && checks[x+1, y])
                {
                    moves[k] = (x + 1) * 10 + y;
                    piececoord[k] = xBoard * 10 + yBoard;
                    pieces[k] = piece;
                    k++;
                }

                if (PositionOnBoard(x - 1, y) && positions[x - 1, y] != 0 && piece / 10 != positions[x - 1, y] / 10 && checks[x - 1, y])
                {
                    moves[k] = (x - 1) * 10 + y;
                    piececoord[k] = xBoard * 10 + yBoard;
                    pieces[k] = piece;
                    k++;
                }

                //enpasant
                /*if (controller.GetComponent<Game>().GetCurrentPlayer() == "white")
                {
                    if (sc.PositionOnBoard(x + 1, y - 1) && sc.GetPosition(x + 1, y - 1) != null && sc.GetPosition(x + 1, y - 1).GetComponent<Chesspieces>().player != player && sc.GetPosition(x + 1, y - 1).GetComponent<Chesspieces>().Isenpassant() && sc.GetCheck(x + 1, y - 1))
                    {
                        if (sc.GetPosition(x + 1, y - 1).GetComponent<Chesspieces>().passantturn == sc.GetComponent<Game>().GetTurns())
                        {
                            MovePlateAttackSpawn(x + 1, y);
                        }
                    }

                    if (sc.PositionOnBoard(x - 1, y - 1) && sc.GetPosition(x - 1, y - 1) != null && sc.GetPosition(x - 1, y - 1).GetComponent<Chesspieces>().player != player && sc.GetPosition(x - 1, y - 1).GetComponent<Chesspieces>().Isenpassant() && sc.GetCheck(x - 1, y - 1))
                    {
                        if (sc.GetPosition(x - 1, y - 1).GetComponent<Chesspieces>().passantturn == sc.GetComponent<Game>().GetTurns())
                        {
                            MovePlateAttackSpawn(x - 1, y);
                        }
                    }
                }
                else if (controller.GetComponent<Game>().GetCurrentPlayer() == "black")
                {
                    if (sc.PositionOnBoard(x + 1, y + 1) && sc.GetPosition(x + 1, y + 1) != null && sc.GetPosition(x + 1, y + 1).GetComponent<Chesspieces>().player != player && sc.GetPosition(x + 1, y + 1).GetComponent<Chesspieces>().Isenpassant() && sc.GetCheck(x + 1, y + 1))
                    {
                        if (sc.GetPosition(x + 1, y + 1).GetComponent<Chesspieces>().passantturn == sc.GetComponent<Game>().GetTurns())
                        {
                            MovePlateAttackSpawn(x + 1, y);
                        }
                    }

                    if (sc.PositionOnBoard(x - 1, y + 1) && sc.GetPosition(x - 1, y + 1) != null && sc.GetPosition(x - 1, y + 1).GetComponent<Chesspieces>().player != player && sc.GetPosition(x - 1, y + 1).GetComponent<Chesspieces>().Isenpassant() && sc.GetCheck(x - 1, y + 1))
                    {
                        if (sc.GetPosition(x - 1, y + 1).GetComponent<Chesspieces>().passantturn == sc.GetComponent<Game>().GetTurns())
                        {
                            MovePlateAttackSpawn(x - 1, y);
                        }
                    }
                }*/
            }
        }
    }



    public void PointAttack(int x, int y, int piece)
    {
        if (PositionOnBoard(x, y))
        {
            if (piece/10 == 8)
                whiteattack[x,y] = -1;
            else
                blackattack[x,y] = -1;    
        }
    }

    public void PawnAttack(int x, int y, int piece)
    {
        Game sc = controller.GetComponent<Game>();

        if (PositionOnBoard(x, y))
        {

            if (PositionOnBoard(x+1, y))
            {
                if (piece / 10 == 8)
                    whiteattack[x + 1, y] = -1;
                else
                    blackattack[x + 1, y] = -1;
            }

            if (PositionOnBoard(x-1, y))
            {
                if (piece / 10 == 8)
                    whiteattack[x - 1, y] = -1;
                else
                    blackattack[x - 1, y] = -1;
            }

            //enpasant
           /* if (piece / 10 == 8)
            {
                if (x + 1 < 8 && y -1 < 8 && positions[x + 1, y - 1] != 0 && positions[x + 1, y - 1] == 162 && y-1 = 4)
                {
                    if (sc.GetPosition(x + 1, y - 1).GetComponent<Chesspieces>().passantturn == sc.GetComponent<Game>().GetTurns())
                    {
                        controller.GetComponent<Game>().Setattackwhite(x + 1, y - 1);
                        if (sc.GetCurrentPlayer() == color)
                            controller.GetComponent<Game>().SetMoves();
                        sc.SetPawnMoves();
                    }
                }

                if (sc.PositionOnBoard(x - 1, y - 1) && sc.GetPosition(x - 1, y - 1) != null && sc.GetPosition(x - 1, y - 1).GetComponent<Chesspieces>().GetPlayer() != color && sc.GetPosition(x - 1, y - 1).GetComponent<Chesspieces>().Isenpassant())
                {
                    if (sc.GetPosition(x - 1, y - 1).GetComponent<Chesspieces>().passantturn == sc.GetComponent<Game>().GetTurns())
                    {
                        controller.GetComponent<Game>().Setattackwhite(x - 1, y - 1);
                        if (sc.GetCurrentPlayer() == color)
                            controller.GetComponent<Game>().SetMoves();
                        sc.SetPawnMoves();
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
                        if (sc.GetCurrentPlayer() == color)
                            controller.GetComponent<Game>().SetMoves();
                        sc.SetPawnMoves();
                    }
                }

                if (sc.PositionOnBoard(x - 1, y + 1) && sc.GetPosition(x - 1, y + 1) != null && sc.GetPosition(x - 1, y + 1).GetComponent<Chesspieces>().GetPlayer() != color && sc.GetPosition(x - 1, y + 1).GetComponent<Chesspieces>().Isenpassant())
                {
                    if (sc.GetPosition(x - 1, y + 1).GetComponent<Chesspieces>().passantturn == sc.GetComponent<Game>().GetTurns())
                    {
                        controller.GetComponent<Game>().Setattackblack(x - 1, y + 1);
                        if (sc.GetCurrentPlayer() == color)
                            controller.GetComponent<Game>().SetMoves();
                        sc.SetPawnMoves();
                    }
                }
            }*/
        }
    }




    public void Checks(int x, int y, int piece)
    {
        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
                checks[i, j] = false;



                LCheck(x, y, piece);

        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
            {
                if (j == i && i == 0)
                    continue;
                LineCheck(movecoord[i], movecoord[j], x, y, 0, piece);
            }

        PawnCheck(x, y, piece);
    }

    public void PawnCheck(int x, int y, int piece)
    {
        Game sc = controller.GetComponent<Game>();
        if (piece /10 == 8)
        {
            if (PositionOnBoard(x - 1, y + 1) && positions[x - 1, y + 1] != 0 && positions[x - 1, y + 1] == 162)
                checks[x - 1, y + 1] = true;
            else if (PositionOnBoard(x + 1, y + 1) && positions[x + 1, y + 1] != 0 && positions[x + 1, y + 1] == 162)
                checks[x + 1, y + 1] = true;
        }
        else
        {
            if (PositionOnBoard(x - 1, y - 1) && positions[x - 1, y - 1] != 0 && positions[x - 1, y - 1] == 82)
                checks[x - 1, y - 1] = true;
            else if (PositionOnBoard(x+1, y-1) && positions[x + 1, y - 1] != 0 && positions[x + 1, y - 1] == 82)
                checks[x + 1, y - 1] = true;
        }
    }

    public void LineCheck(int xIncrement, int yIncrement, int xBoard, int yBoard, int counter, int piece)
    {
        Game sc = controller.GetComponent<Game>();
        int x = xBoard + xIncrement;
        int y = yBoard + yIncrement;

        if (piece / 10 == 8)
        {
            if (PositionOnBoard(x, y) && positions[x, y] == 0)
            {
                LineCheck(xIncrement, yIncrement, x, y, counter, piece);
                if (PositionOnBoard(x + xIncrement, y + yIncrement))
                    checks[x, y] = checks[x + xIncrement, y + yIncrement];
            }
            else if (PositionOnBoard(x, y) && positions[x, y] != 0 && xIncrement != 0 && yIncrement != 0)
            {
                if (counter < 1 && positions[x, y] / 10 == piece / 10)
                {
                    LineCheck(xIncrement, yIncrement, x, y, counter + 1, piece);
                    if (PositionOnBoard(x + xIncrement, y + yIncrement))
                        checks[x, y] = checks[x + xIncrement, y + yIncrement];
                }
                else if (positions[x, y] == 166 || positions[x, y] == 164)
                    checks[x, y] = true;

            }
            else if (PositionOnBoard(x, y) && positions[x, y] != 0 && (xIncrement == 0 || yIncrement == 0))
            {
                if (counter < 1 && positions[x, y] / 10 == piece / 10)
                {
                    LineCheck(xIncrement, yIncrement, x, y, counter + 1, piece);
                    if (PositionOnBoard(x + xIncrement, y + yIncrement))
                        checks[x, y] = checks[x + xIncrement, y + yIncrement];
                }
                else if (positions[x, y] == 166 || positions[x, y] == 165)
                    checks[x, y] = true;
            }

        }
        else
        {
            if (PositionOnBoard(x, y) && positions[x, y] == 0)
            {
                LineCheck(xIncrement, yIncrement, x, y, counter, piece);
                if (PositionOnBoard(x + xIncrement, y + yIncrement))
                    checks[x, y] = checks[x + xIncrement, y + yIncrement];
            }
            else if (PositionOnBoard(x, y) && positions[x, y] != 0 && xIncrement != 0 && yIncrement != 0)
            {
                if (counter < 1 && positions[x, y] / 10 == piece / 10)
                {
                    LineCheck(xIncrement, yIncrement, x, y, counter + 1, piece);
                    if (PositionOnBoard(x + xIncrement, y + yIncrement))
                        checks[x, y] = checks[x + xIncrement, y + yIncrement];
                }
                else if (positions[x, y] == 86 || positions[x, y] == 84)
                    checks[x, y] = true;

            }
            else if (PositionOnBoard(x, y) && positions[x, y] != 0 && (xIncrement == 0 || yIncrement == 0))
            {
                if (counter < 1 && positions[x, y] / 10 == piece / 10)
                {
                    LineCheck(xIncrement, yIncrement, x, y, counter + 1, piece);
                    if (PositionOnBoard(x + xIncrement, y + yIncrement))
                        checks[x, y] = checks[x + xIncrement, y + yIncrement];
                }
                else if (positions[x, y] == 86 || positions[x, y] == 85)
                    checks[x, y] = true;
            }
        }
    }


    public void LCheck(int xBoard, int yBoard, int color)
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


    public void PointCheck(int x, int y, int piece)
    {
        if (PositionOnBoard(x, y) && positions[x, y] != 0)
        {
            if (piece / 10 == 8 && positions[x, y] == 163 || piece / 10 == 16 && positions[x, y] == 83)
                checks[x, y] = true;
        }
    }
}
