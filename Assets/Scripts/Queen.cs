using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Chessman
{
    public override bool[,] PossibleMove()
    {
        bool[,] r = new bool[8, 8];
        Chessman c;
        int i, j;

        //오른쪽으로 이동
        i = CurrentX;
        while (true)
        {
            i++;
            if (i > 7)
                break;

            c = BoardManager.Instance.Chessmans[i, CurrentY];
            if (c == null)
                r[i, CurrentY] = true;
            else
            {
                if (c.isWhite != isWhite)
                    r[i, CurrentY] = true;
                break;
            }

        }

        //왼쪽으로 이동
        i = CurrentX;
        while (true)
        {
            i--;
            if (i < 0)
                break;

            c = BoardManager.Instance.Chessmans[i, CurrentY];
            if (c == null)
                r[i, CurrentY] = true;
            else
            {
                if (c.isWhite != isWhite)
                    r[i, CurrentY] = true;
                break;
            }

        }

        //앞으로 이동
        i = CurrentY;
        while (true)
        {
            i++;
            if (i > 7)
                break;

            c = BoardManager.Instance.Chessmans[CurrentX, i];
            if (c == null)
                r[CurrentX, i] = true;
            else
            {
                if (c.isWhite != isWhite)
                    r[CurrentX, i] = true;
                break;
            }

        }

        //뒤으로 이동
        i = CurrentY;
        while (true)
        {
            i--;
            if (i < 0)
                break;

            c = BoardManager.Instance.Chessmans[CurrentX, i];
            if (c == null)
                r[CurrentX, i] = true;
            else
            {
                if (c.isWhite != isWhite)
                    r[CurrentX, i] = true;
                break;
            }

        }

        //대각선 위로 왼쪽
        i = CurrentX;
        j = CurrentY;
        while (true)
        {
            i--;
            j++;
            if (i < 0 || j > 7)
                break;
            c = BoardManager.Instance.Chessmans[i, j];
            if (c == null)
                r[i, j] = true;
            else
            {
                if (c.isWhite != isWhite)
                    r[i, j] = true;
                break;
            }
        }

        //대각선 위로 오른쪽
        i = CurrentX;
        j = CurrentY;
        while (true)
        {
            i++;
            j++;
            if (i > 7 || j > 7)
                break;
            c = BoardManager.Instance.Chessmans[i, j];
            if (c == null)
                r[i, j] = true;
            else
            {
                if (c.isWhite != isWhite)
                    r[i, j] = true;
                break;
            }
        }

        //대각선 아래로 왼쪽
        i = CurrentX;
        j = CurrentY;
        while (true)
        {
            i--;
            j--;
            if (i < 0 || j < 0)
                break;
            c = BoardManager.Instance.Chessmans[i, j];
            if (c == null)
                r[i, j] = true;
            else
            {
                if (c.isWhite != isWhite)
                    r[i, j] = true;
                break;
            }
        }

        //대각선 아래로 오른쪽
        i = CurrentX;
        j = CurrentY;
        while (true)
        {
            i++;
            j--;
            if (i > 7 || j < 0)
                break;
            c = BoardManager.Instance.Chessmans[i, j];
            if (c == null)
                r[i, j] = true;
            else
            {
                if (c.isWhite != isWhite)
                    r[i, j] = true;
                break;
            }
        }

        return r;
    }
}