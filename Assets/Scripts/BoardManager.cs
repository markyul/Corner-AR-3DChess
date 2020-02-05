using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Chat;

public class BoardManager : MonoBehaviourPun
{
    public static BoardManager Instance { set; get; }
    public Camera HostCamera;
    public Camera ClientCamera;
    PhotonView pv;

    private bool[,] allowedMoves { set; get; }

    public Chessman[,] Chessmans { set; get; }
    private Chessman selectedChessman;
    
    private const float TILE_SIZE = 1.0f;
    private const float TILE_OFFSET = 0.5f;

    private int selectionX = -1;
    private int selectionY = -1;

    public List<GameObject> chessmanPrefabs;
    private List<GameObject> activeChessman;

    private Quaternion orientation = Quaternion.Euler(0, 180, 0);

    public bool isWhiteTurn = true;
    private void Start()
    {
        Instance = this;
        SpawnAllChessmans();
        pv = GetComponent<PhotonView>();
  
        if (PhotonNetwork.IsMasterClient)
        {
            HostCamera.enabled = true;
            ClientCamera.enabled = false;
            Debug.Log("위에꺼");
            
        }
        else
        {
            HostCamera.enabled = false;
            ClientCamera.enabled = true;
            Debug.Log("밑에꺼");

        }

    }

    private void Update()
    {
        UpdateSelection();
        DrawChessboard();

        if (Input.GetMouseButtonDown(0))
        {
            if(selectionX >= 0 && selectionY >= 0)
            {
                if(selectedChessman == null)
                {
                    //체스말 선택
                    SelectChessman(selectionX, selectionY);
                    pv.RPC("SelectChessman", RpcTarget.AllViaServer, selectionX, selectionY);
                }
                else
                {
                    //체스말 이동
                    MoveChessman(selectionX, selectionY);
                    pv.RPC("MoveChessman", RpcTarget.AllViaServer, selectionX, selectionY);
                }
            }
        }
    }

    //체스말 선택
    [PunRPC]
    private void SelectChessman(int x, int y)
    {
        if(Chessmans[x, y] == null)
        {
            return;
        }

        if(Chessmans[x, y].isWhite != isWhiteTurn)
        {
            return;
        }

        bool hasAtLeastOneMove = false;
        allowedMoves = Chessmans[x, y].PossibleMove();
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (allowedMoves[i, j])
                    hasAtLeastOneMove = true;
            }
        }
        if (!hasAtLeastOneMove)
            return;

        allowedMoves = Chessmans[x, y].PossibleMove();
        selectedChessman = Chessmans[x, y];
        BoardHighlights.Instance.HighlightAllowedMoves(allowedMoves);
    }

    [PunRPC]
    //체스말 이동
    private void MoveChessman(int x, int y)
    {
        if (allowedMoves[x, y])
        {
            Chessman c = Chessmans[x, y];
            //상대말 잡기
            if(c != null && c.isWhite != isWhiteTurn)
            {
                //킹을 잡을 경우
                if(c.GetType() == typeof(King))
                {
                    //Game Over
                    EndGame();
                    return;
                }

                activeChessman.Remove(c.gameObject);
                Destroy(c.gameObject);
            }

            Chessmans[selectedChessman.CurrentX, selectedChessman.CurrentY] = null;
            selectedChessman.transform.position = GetTileCenter(x, y);
            selectedChessman.SetPosition(x, y);
            Chessmans[x, y] = selectedChessman;
            isWhiteTurn = !isWhiteTurn;   //턴 변경
        }
        BoardHighlights.Instance.HideHighlights();
        selectedChessman = null;
    }

    //판 위에 마우스를 올렸을떄 마우스 좌표
    private void UpdateSelection()
    {
        if (!Camera.main)
            return;
        if (PhotonNetwork.IsMasterClient && !isWhiteTurn) return;
        if (!PhotonNetwork.IsMasterClient && isWhiteTurn) return;
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f, LayerMask.GetMask("ChessPlane")))
        {
            selectionX = (int)hit.point.x;
            selectionY = (int)hit.point.z;
        }
        else
        {
            selectionX = -1;
            selectionY = -1;
        }
    }

    //체스말 위치지정
    private void SpawnChessman(int index, int x, int y)
    {
        GameObject go;
        if (index < 6)
        {
            go = Instantiate(chessmanPrefabs[index], GetTileCenter(x, y), Quaternion.identity) as GameObject;
        }
        else
        {
            go = Instantiate(chessmanPrefabs[index], GetTileCenter(x, y), orientation) as GameObject;
        }
        go.transform.SetParent(transform);
        go.AddComponent<PhotonView>();
        Chessmans[x, y] = go.GetComponent<Chessman>();
        Chessmans[x, y].SetPosition(x, y);
        activeChessman.Add(go);
    }

    //체스말 각각의 좌표지정
    private void SpawnAllChessmans()
    {
        activeChessman = new List<GameObject>();
        Chessmans = new Chessman[8, 8];

        //----White team----
        SpawnChessman(0, 4, 0);   //King
        SpawnChessman(1, 3, 0);   //Quenn
        SpawnChessman(2, 0, 0);   //Rook_left
        SpawnChessman(2, 7, 0);   //Rook_right
        SpawnChessman(3, 2, 0);   //Bishop_left
        SpawnChessman(3, 5, 0);   //Bishop_right
        SpawnChessman(4, 1, 0);   //Knight_left
        SpawnChessman(4, 6, 0);   //Knight_right
        //Pawn
        for (int i = 0; i < 8; i++)
        {
            SpawnChessman(5, i, 1);
        }
        //------------------
        //----Black team----
        SpawnChessman(6, 4, 7);   //King
        SpawnChessman(7, 3, 7);   //Quenn
        SpawnChessman(8, 0, 7);   //Rook_left
        SpawnChessman(8, 7, 7);   //Rook_right
        SpawnChessman(9, 2, 7);   //Bishop_left
        SpawnChessman(9, 5, 7);   //Bishop_right
        SpawnChessman(10, 1, 7);   //Knight_left
        SpawnChessman(10, 6, 7);   //Knight_right
        //Pawn
        for (int i = 0; i < 8; i++)
        {
            SpawnChessman(11, i, 6);
        }
    }
    //체스말 좌표 및 네모위에 위치
    private Vector3 GetTileCenter(int x, int y)
    {
        Vector3 origin = Vector3.zero;
        origin.x += (TILE_SIZE * x) + TILE_OFFSET;
        origin.z += (TILE_SIZE * y) + TILE_OFFSET;
        return origin;
    }
    //체스판
    private void DrawChessboard()
    {
        Vector3 widthLine = Vector3.right * 8;
        Vector3 heigthLine = Vector3.forward * 8;

        for(int i = 0; i <= 8; i++)
        {
            Vector3 start = Vector3.forward * i;
            Debug.DrawLine(start, start + widthLine);
            for (int j = 0; j <= 8; j++)
            {
                start = Vector3.right * j;
                Debug.DrawLine(start, start + heigthLine);
            }
        }

        // 마우스 위치에 따른 표시
        if(selectionX >= 0 && selectionY >= 0)
        {
            Debug.DrawLine(
                Vector3.forward * selectionY + Vector3.right * selectionX,
                Vector3.forward * (selectionY + 1) + Vector3.right * (selectionX + 1));

            Debug.DrawLine(
                Vector3.forward * selectionY + Vector3.right * (selectionX + 1),
                Vector3.forward * (selectionY + 1) + Vector3.right * selectionX);
        }
    }
    //게임 종료
    private void EndGame()
    {
        if (isWhiteTurn)
            Debug.Log("White team wins");
        else
            Debug.Log("White team wins");

        foreach (GameObject go in activeChessman)
            Destroy(go);

        isWhiteTurn = true;
        BoardHighlights.Instance.HideHighlights();
        SpawnAllChessmans();
    }
}
