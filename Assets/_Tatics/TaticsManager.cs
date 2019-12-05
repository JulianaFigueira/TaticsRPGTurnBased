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

    public UnityEngine.UI.Button restartBttn;

	// Use this for initialization
	void Start ()
    {
        //MapScript = GetComponentInChildren<MapManager>();
        //UnitsScript = GetComponentInChildren<UnitsManager>();

        Restart();
	}

    public void Restart()
    {
        restartBttn.gameObject.SetActive(false);
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
    }

    // Update is called once per frame
    void Update () {
		if(currentState == TaticsStates.Init)
        {
            UpdateStateMachine();
        }
    }

    public void UpdateStateMachine()
    {
        switch (currentState)
        {
            case TaticsStates.Init:
                Debug.Log("Game started!");
                bool isGamePrepared = UnitsScript.PrepareGame();
                currentState = isGamePrepared ? TaticsStates.ProcessingPlayer : TaticsStates.Exit;
                UpdateStateMachine(); 
                break;
            case TaticsStates.ProcessingPlayer:
                Debug.Log("TaticsStates ProcessingPlayer!");
                if (UnitsScript.CheckCurrentRoundFinished())
                {
                    currentState = TaticsStates.NextPlayer;
                    UpdateStateMachine();
                }
                break;
            case TaticsStates.NextPlayer:
                Debug.Log("TaticsStates NextPlayer!");
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
                Debug.Log("You lost . . .");
                restartBttn.gameObject.SetActive(true);
                DestroyTableTop();
                break;
            case TaticsStates.Win:
                restartBttn.gameObject.SetActive(true);
                Debug.Log("You WON!");
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
