using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public string playerName;
    public GameObject prefab;
    public bool isX;
    public Text myTurnText;
    public bool isAI = false;

    public float waitTime = 2.0f;
    private float currTime = 0;
    public enum PlayerStates
    {
        None,
        MyTurn,
        OpponentsTurn
    }

    public PlayerStates state = PlayerStates.None;

    private void Awake()
    {
        myTurnText.gameObject.SetActive(false);
    }
    private void Update()
    {
        if(state == PlayerStates.MyTurn)
        {
            myTurnText.gameObject.SetActive(true);
            if (isAI)
            {
                if (currTime > waitTime)
                {
                    Square squ = null;
                    Move m = minimax(isX);
                    if(m.move != null)
                    {
                        squ = m.move;
                    }
                    else
                    {
                        Debug.Log("There is no Move to make");
                        return;
                    }

                    GameObject newGO = Instantiate(prefab, squ.gameObject.transform.position, squ.gameObject.transform.rotation);
                    newGO.transform.parent = TurnManager.Instance.container.transform;
                    if (isX)
                    {
                        squ.state = Square.SquareStates.X;
                    }
                    else
                    {
                        squ.state = Square.SquareStates.O;
                    }

                    currTime = 0;
                    TurnManager.Instance.checkGameState();
                    TurnManager.Instance.changeTurn();
                }
                currTime += Time.deltaTime;
            }
            else
            {
                if (Input.GetMouseButtonUp(0))
                {
                    Vector3 clickPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
                    RaycastHit2D hit = Physics2D.Raycast(clickPoint, Vector3.forward);
                    if (hit.collider != null)
                    {
                        Square hitSquare = hit.collider.gameObject.GetComponent<Square>();
                        if (hitSquare != null)
                        {
                            if (hitSquare.state == Square.SquareStates.None)
                            {
                                GameObject newGO = Instantiate(prefab, hitSquare.gameObject.transform.position, hitSquare.gameObject.transform.rotation);
                                newGO.transform.parent = TurnManager.Instance.container.transform;
                                if (isX)
                                {
                                    hitSquare.state = Square.SquareStates.X;
                                }
                                else
                                {
                                    hitSquare.state = Square.SquareStates.O;
                                }

                                TurnManager.Instance.checkGameState();
                                TurnManager.Instance.changeTurn();
                            }
                        }
                    }
                }
            }
        }

        else if(state == PlayerStates.OpponentsTurn)
        {
            myTurnText.gameObject.SetActive(false);
        }
        else
        {
            myTurnText.gameObject.SetActive(false);
        }
    }

    private bool isEmpty(Square squ)
    {
        return squ.state == Square.SquareStates.None;
    }

    public class Move
    {
        public Square move = null;
        public int score = 0;
    }
    private Move minimax(bool isXplayer)
    {
        TurnManager.BoardStates boardState = TurnManager.Instance.EvaluateBoard();
        if (boardState == TurnManager.BoardStates.XWin)
        {
            return new Move() { score = 10 };
        }
        else if (boardState == TurnManager.BoardStates.OWin)
        {
            return new Move() { score = -10 };
        }
        else if (boardState == TurnManager.BoardStates.Tie)
        {
            return new Move() { score = 0 };
        }

        List<Move> moves = new List<Move>();

        var list = TurnManager.Instance.squares.FindAll(isEmpty);
        for (int i = 0; i < list.Count; i++)
        {
            Move m = new Move();
            m.move = list[i];

            m.move.state = isXplayer ? Square.SquareStates.X : Square.SquareStates.O;

            Move result = minimax(!isXplayer);
            m.score = result.score;

            m.move.state = Square.SquareStates.None;
            moves.Add(m);
        }

        Move bestMove = null;
        int bestScore = 0;
        if(isXplayer) //max
        {
            bestScore = -20;
            foreach(Move mo in moves)
            {
                if(mo.score > bestScore)
                {
                    bestScore = mo.score;
                    bestMove = mo;
                }
            }
        }
        else //min
        {
            bestScore = 20;
            foreach (Move mo in moves)
            {
                if (mo.score < bestScore)
                {
                    bestScore = mo.score;
                    bestMove = mo;
                }
            }
        }
        return bestMove;
    }
}
