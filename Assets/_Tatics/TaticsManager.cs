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
        Lose
    }

    private GameState gameState;
    private TaticsStates currentState;
    public MapManager MapScript;
    public UnitsManager UnitsScript;

	// Use this for initialization
	void Start ()
    {
        //MapScript = GetComponentInChildren<MapManager>();
        //UnitsScript = GetComponentInChildren<UnitsManager>();

        Restart();
	}

    private void Restart()
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
        UpdateStateMachine();
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void UpdateStateMachine()
    {
        switch (currentState)
        {
            case TaticsStates.Init:
                bool isGamePrepared = UnitsScript.PrepareGame();
                currentState = isGamePrepared ? TaticsStates.ProcessingPlayer : TaticsStates.Exit;
                UpdateStateMachine(); 
                break;
            case TaticsStates.ProcessingPlayer:
                bool isFinishedRound = UnitsScript.CheckCurrentRoundFinished();
                if (isFinishedRound)
                {
                    currentState = TaticsStates.NextPlayer;
                    UpdateStateMachine();
                }
                break;
            case TaticsStates.NextPlayer:
                gameState = UnitsScript.CheckGameState();
                switch (gameState)
                {
                    case GameState.Play:
                        UnitsScript.ChangeCurrentPlayer();
                        currentState = TaticsStates.ProcessingPlayer;
                        break;
                    case GameState.Win:
                        UnitsScript.StopGame();
                        currentState = TaticsStates.Win;
                        UpdateStateMachine();
                        break;
                    case GameState.Lose:
                        UnitsScript.StopGame();
                        currentState = TaticsStates.Lose;
                        UpdateStateMachine();
                        break;
                }
                break;
            case TaticsStates.Lose:
                //TODO: show message. restart?
                DestroyTableTop();
                break;
            case TaticsStates.Win:
                //TODO: show message. restart?
                DestroyTableTop();
                break;
            case TaticsStates.Exit:
                DestroyTableTop();
                break;
        }
    }

    private void DestroyTableTop()
    {
        UnitsScript.Delete();
        MapScript.Delete();
    }
}
