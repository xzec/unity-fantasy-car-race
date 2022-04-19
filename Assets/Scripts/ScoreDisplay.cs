﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    private Text _scoreText;
    private GameSession _gameSession;

    // Start is called before the first frame update
    private void Start()
    {
        _scoreText = GetComponent<Text>();
        _gameSession = FindObjectOfType<GameSession>();
    }

    // Update is called once per frame
    private void Update()
    {
        _scoreText.text = _gameSession.GetScore().ToString();
    }
}