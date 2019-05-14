using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : Singleton<TurnManager>
{
    public enum TurnStates
    {
        None,
        Player1,
        Player2
    }
    public enum BoardStates
    {
        None,
        Tie,
        XWin,
        OWin
    }
    public Player player1;
    public Player player2;

    public TurnStates state = TurnStates.None;

    public List<Square> squares;
    public GameObject container;

    public GameObject MenuPanel;
    public GameObject GamePanel;
    public GameObject GameOverPanel;
    public GameObject PauseGamePanel;
    public Text winText;

    private void Start()
    {
        if(player1 != null && player2 != null && player1.isX != player2.isX)//not same player
        {
            state = TurnStates.None;
            player1.state = Player.PlayerStates.None;
            player2.state = Player.PlayerStates.None;
            player1.gameObject.SetActive(false);
            player2.gameObject.SetActive(false);
            MenuPanel.SetActive(true);
            GamePanel.SetActive(false);
            GameOverPanel.SetActive(false);
            PauseGamePanel.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Error with Players");
        }
    }

    public void StartSinglePlayer()
    {
        state = TurnStates.Player1;
        player1.state = Player.PlayerStates.MyTurn;
        player1.isAI = false;
        player2.state = Player.PlayerStates.OpponentsTurn;
        player2.isAI = true;
        player1.gameObject.SetActive(true);
        player2.gameObject.SetActive(true);
        MenuPanel.gameObject.SetActive(false);
        GamePanel.SetActive(true);
    }

    public void StartTwoAIPlayer()
    {
        state = TurnStates.Player1;
        player1.state = Player.PlayerStates.MyTurn;
        player1.isAI = true;
        player2.state = Player.PlayerStates.OpponentsTurn;
        player2.isAI = true;
        player1.gameObject.SetActive(true);
        player2.gameObject.SetActive(true);
        MenuPanel.gameObject.SetActive(false);
        GamePanel.SetActive(true);
    }

    public void StartTwoPlayer()
    {
        state = TurnStates.Player1;
        player1.state = Player.PlayerStates.MyTurn;
        player1.isAI = false;
        player2.state = Player.PlayerStates.OpponentsTurn;
        player2.isAI = false;
        player1.gameObject.SetActive(true);
        player2.gameObject.SetActive(true);
        MenuPanel.gameObject.SetActive(false);
        GamePanel.SetActive(true);
    }

    public void changeTurn()
    {
        if(state == TurnStates.Player1) //change to player 2
        {
            state = TurnStates.Player2;
            player1.state = Player.PlayerStates.OpponentsTurn;
            player2.state = Player.PlayerStates.MyTurn;
        }
        else if(state == TurnStates.Player2) //change to player 1
        {
            state = TurnStates.Player1;
            player1.state = Player.PlayerStates.MyTurn;
            player2.state = Player.PlayerStates.OpponentsTurn;
        }
    }

    public BoardStates EvaluateBoard()
    {
        if (squares[0].state != Square.SquareStates.None &&
            squares[0].state == squares[1].state &&
            squares[1].state == squares[2].state)
        {
            //Debug.Log("r1 win");
            return squares[0].state == Square.SquareStates.X ? BoardStates.XWin: BoardStates.OWin ;
        }

        if (squares[3].state != Square.SquareStates.None &&
            squares[3].state == squares[4].state &&
            squares[4].state == squares[5].state)
        {
            //Debug.Log("r2 win");
            return squares[3].state == Square.SquareStates.X ? BoardStates.XWin : BoardStates.OWin;
        }

        if (squares[6].state != Square.SquareStates.None &&
            squares[6].state == squares[7].state &&
            squares[7].state == squares[8].state)
        {
            //Debug.Log("r3 win");
            return squares[6].state == Square.SquareStates.X ? BoardStates.XWin : BoardStates.OWin;
        }

        //check collums
        if (squares[0].state != Square.SquareStates.None &&
            squares[0].state == squares[3].state &&
            squares[3].state == squares[6].state)
        {
            //Debug.Log("c1 win");
            return squares[0].state == Square.SquareStates.X ? BoardStates.XWin : BoardStates.OWin;
        }

        if (squares[1].state != Square.SquareStates.None &&
            squares[1].state == squares[4].state &&
            squares[4].state == squares[7].state)
        {
            //Debug.Log("c2 win");
            return squares[1].state == Square.SquareStates.X ? BoardStates.XWin : BoardStates.OWin;
        }

        if (squares[2].state != Square.SquareStates.None &&
            squares[2].state == squares[5].state &&
            squares[5].state == squares[8].state)
        {
            //Debug.Log("c3 win");
            return squares[2].state == Square.SquareStates.X ? BoardStates.XWin : BoardStates.OWin;
        }

        if (squares[0].state != Square.SquareStates.None &&
            squares[0].state == squares[4].state &&
            squares[4].state == squares[8].state)
        {
            //Debug.Log("d1 win");
            return squares[0].state == Square.SquareStates.X ? BoardStates.XWin : BoardStates.OWin;
        }

        if (squares[2].state != Square.SquareStates.None &&
            squares[2].state == squares[4].state &&
            squares[4].state == squares[6].state)
        {
            //Debug.Log("d2 win");
            return squares[2].state == Square.SquareStates.X ? BoardStates.XWin : BoardStates.OWin;
        }

        bool isEmpty(Square squ)
        {
            return squ.state == Square.SquareStates.None;
        }

        Square test = squares.Find(isEmpty);
        if(test != null)
        {
            return BoardStates.None;
        }
        else
        {
            return BoardStates.Tie;
        }
    }

    public void checkGameState()
    {
        BoardStates boardStates = EvaluateBoard();

        if (boardStates == BoardStates.XWin)
        {
            winText.text = "X Wins!";
            endGame();
            return;
        }
        else if (boardStates == BoardStates.OWin)
        {
            winText.text = "O Wins!";
            Instance.endGame();
            return;
        }
        else if (boardStates == TurnManager.BoardStates.Tie)
        {
            winText.text = "Its a Tie";
            endGame();
            return;
        }
    }

    public void ResumeGame()
    {
        PauseGamePanel.SetActive(false);
        player1.gameObject.SetActive(true);
        player2.gameObject.SetActive(true);
    }

    public void pauseGame()
    {
        PauseGamePanel.SetActive(true);
        player1.gameObject.SetActive(false);
        player2.gameObject.SetActive(false);
    }

    public void stopGame()
    {
        PauseGamePanel.SetActive(false);
        GamePanel.SetActive(false);
        restart();
    }

    public void endGame()
    {
        player1.state = Player.PlayerStates.None;
        player2.state = Player.PlayerStates.None;
        player1.gameObject.SetActive(false);
        player2.gameObject.SetActive(false);
        GameOverPanel.SetActive(true);
        GamePanel.SetActive(false);
    }

    public void restart()
    {
        GameOverPanel.SetActive(false);
        MenuPanel.SetActive(true);
        foreach(Square square in squares)
        {
            square.state = Square.SquareStates.None;
        }
        
        foreach(Transform t in container.transform)
        {
            Destroy(t.gameObject);
        }
    }
}
