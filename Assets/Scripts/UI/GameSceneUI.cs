using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameSceneUI : MonoBehaviour
{
    public Text Time;
    public Text Lv;
    public Button Close;
    public Button Tip;
    public Button Back;

    public delegate bool TimeOver();
    public event TimeOver TimeOverEvent;

    public delegate void CubeTurnBack();
    public event CubeTurnBack OnEventBack;

    private int TimeCount;

    public static GameSceneUI Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        Close.onClick.AddListener(OnCloseClicked);
        Tip.onClick.AddListener(OnTipClicked);
        Back.onClick.AddListener(OnTurnBackClicked);
    }



    private void OnCloseClicked()
    {

    }

    private void OnTurnBackClicked()
    {
        OnEventBack?.Invoke();
    }

    private void OnTipClicked()
    {

    }

}
