using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Chessman
{
    public override bool[,] PossibleMove()
    {
        bool[,] r = new bool[8, 8];
        Chessman c;

        if (isWhite)    //White team
        {
            // 좌측 위
            if (CurrentX != 0 && CurrentY != 7)
            {
                c = BoardManager.Instance.Chessmans[CurrentX - 1, CurrentY + 1];
                if (c == null || !c.isWhite)
                    r[CurrentX - 1, CurrentY + 1] = true;
            }
            // 우측 위
            if (CurrentX != 7 && CurrentY != 7)
            {
                c = BoardManager.Instance.Chessmans[CurrentX + 1, CurrentY + 1];
                if (c == null || !c.isWhite)
                    r[CurrentX + 1, CurrentY + 1] = true;
            }
            // 좌측 아래
            if (CurrentX != 0 && CurrentY != 0)
            {
                c = BoardManager.Instance.Chessmans[CurrentX - 1, CurrentY - 1];
                if (c == null || !c.isWhite)
                    r[CurrentX - 1, CurrentY - 1] = true;
            }
            // 우측 아래
            if (CurrentX != 7 && CurrentY != 0)
            {
                c = BoardManager.Instance.Chessmans[CurrentX + 1, CurrentY - 1];
                if (c == null || !c.isWhite)
                    r[CurrentX + 1, CurrentY - 1] = true;
            }
            // 상
            if (CurrentY != 7)
            {
                c = BoardManager.Instance.Chessmans[CurrentX, CurrentY + 1];
                if (c == null || !c.isWhite)
                    r[CurrentX, CurrentY + 1] = true;
            }
            // 하
            if (CurrentY != 0)
            {
                c = BoardManager.Instance.Chessmans[CurrentX, CurrentY - 1];
                if (c == null || !c.isWhite)
                    r[CurrentX, CurrentY - 1] = true;
            }
            // 좌
            if (CurrentX != 0)
            {
                c = BoardManager.Instance.Chessmans[CurrentX - 1, CurrentY];
                if (c == null || !c.isWhite)
                    r[CurrentX - 1, CurrentY] = true;
            }
            // 우
            if (CurrentY != 7)
            {
                c = BoardManager.Instance.Chessmans[CurrentX + 1, CurrentY];
                if (c == null || !c.isWhite)
                    r[CurrentX + 1, CurrentY] = true;
            }
        }
        else    //Black team
        {
            // 좌측 위
            if (CurrentX != 0 && CurrentY != 7)
            {
                c = BoardManager.Instance.Chessmans[CurrentX - 1, CurrentY + 1];
                if (c == null || c.isWhite)
                    r[CurrentX - 1, CurrentY + 1] = true;
            }
            // 우측 위
            if (CurrentX != 7 && CurrentY != 7)
            {
                c = BoardManager.Instance.Chessmans[CurrentX + 1, CurrentY + 1];
                if (c == null || c.isWhite)
                    r[CurrentX + 1, CurrentY + 1] = true;
            }
            // 좌측 아래
            if (CurrentX != 0 && CurrentY != 0)
            {
                c = BoardManager.Instance.Chessmans[CurrentX - 1, CurrentY - 1];
                if (c == null || c.isWhite)
                    r[CurrentX - 1, CurrentY - 1] = true;
            }
            // 우측 아래
            if (CurrentX != 7 && CurrentY != 0)
            {
                c = BoardManager.Instance.Chessmans[CurrentX + 1, CurrentY - 1];
                if (c == null || c.isWhite)
                    r[CurrentX + 1, CurrentY - 1] = true;
            }
            // 상
            if (CurrentY != 7)
            {
                c = BoardManager.Instance.Chessmans[CurrentX, CurrentY + 1];
                if (c == null || c.isWhite)
                    r[CurrentX, CurrentY + 1] = true;
            }
            // 하
            if (CurrentY != 0)
            {
                c = BoardManager.Instance.Chessmans[CurrentX, CurrentY - 1];
                if (c == null || c.isWhite)
                    r[CurrentX, CurrentY - 1] = true;
            }
            // 좌
            if (CurrentX != 0)
            {
                c = BoardManager.Instance.Chessmans[CurrentX - 1, CurrentY];
                if (c == null || c.isWhite)
                    r[CurrentX - 1, CurrentY] = true;
            }
            // 우
            if (CurrentY != 7)
            {
                c = BoardManager.Instance.Chessmans[CurrentX + 1, CurrentY];
                if (c == null || c.isWhite)
                    r[CurrentX + 1, CurrentY] = true;
            }
        }
        return r;
    }
}
