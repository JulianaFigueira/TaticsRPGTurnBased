using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaticsManager : MonoBehaviour {

    public enum TaticsStates
    {
        Init,
        ProcessingPlayer,
        PlayerMove,
        PlayerAttack,
        NextPlayer,
        Win,
        Lose,
        Exit
    }

    public enum GameState
    {
        Play,
        Win,
        Lose,
        Pause
    }

    private GameState gameState;
    private TaticsStates currentState;
    private int currentPlayer;
    public CreateMapScript MapScript;
    public CreateUnitsScript UnitsScript;


	// Use this for initialization
	void Start ()
    {
        InitGame();
        InitStateMachine();
	}

    private void InitGame()
    {
        MapScript.Generate();
        UnitsScript.Generate();
    }

    private void InitStateMachine()
    {
        gameState = GameState.Play;
        currentState = TaticsStates.Init;
        currentPlayer = 0;
    }

    // Update is called once per frame
    void Update () {
		switch (currentState)
        {
            case TaticsStates.Init:
                InitStateMachine();
                currentState = TaticsStates.ProcessingPlayer;
                break;
            case TaticsStates.ProcessingPlayer:
                bool finishedRound = UnitsScript.CheckCurrentPlay();
                if (finishedRound)
                {
                    currentState = TaticsStates.NextPlayer;
                }
                break;
            case TaticsStates.NextPlayer:
                gameState = UnitsScript.CheckGameState();

                switch (gameState)
                {
                    case GameState.Play:
                        UnitsScript.ChangeCurrentPlayer();
                        break;
                    case GameState.Win:
                        UnitsScript.StopGame();
                        currentState = TaticsStates.Win;
                        break;
                    case GameState.Lose:
                        UnitsScript.StopGame();
                        currentState = TaticsStates.Lose;
                        break;
                    case GameState.Pause:
                        //TODO
                        break;
                }
                break;
            case TaticsStates.Lose:
                currentState = TaticsStates.Exit;
                break;
            case TaticsStates.Win:
                currentState = TaticsStates.Exit;
                break;
            case TaticsStates.Exit:
                UnitsScript.Delete();
                MapScript.Delete();
                break;
        }
	}
0}
