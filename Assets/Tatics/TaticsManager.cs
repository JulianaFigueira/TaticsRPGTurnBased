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
    private int currentPlayer;
    private MapManager MapScript;
    private UnitsManager UnitsScript;

	// Use this for initialization
	void Start ()
    {
        InitGame();
        InitStateMachine();
	}

    private void InitGame()
    {
        MapScript = GetComponentInChildren<MapManager>();
        MapScript.Generate();

        UnitsScript = GetComponentInChildren<UnitsManager>();
        UnitsScript.Generate();
    }

    private void InitStateMachine()
    {
        gameState = GameState.Play;
        currentState = TaticsStates.Init;
        currentPlayer = 0;
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
                currentState = isGamePrepared ? TaticsStates.ProcessingPlayer : TaticsStates.Init;
                break;
            case TaticsStates.ProcessingPlayer:
                bool isFinishedRound = UnitsScript.CheckCurrentRound();
                if (isFinishedRound)
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
