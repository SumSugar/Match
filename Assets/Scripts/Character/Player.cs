﻿using NUnit.Framework;
using System.Collections;
using UnityEngine;

public class Player : CharacterBase
{
    public IntVariable playerMana;
    public int maxMana;

    public int CurrentMana { get => playerMana.currentValue; set => playerMana.SetValue(value); }

    private void OnEnable()
    {
        playerMana.maxValue = maxMana;
        CurrentMana = playerMana.maxValue;
    }

    /// <summary>
    /// 监听事件函数
    /// </summary>
    public void NewTurn()
    {
        CurrentMana = maxMana;
        //todo:
    }

    public void UpdateMana(int cost)
    {
        CurrentMana -= cost;
        if (CurrentMana <= 0)
        {
            CurrentMana = 0;
        }
    }

    public void NewGame()
    {
        //CurrentHP = maxHP;
        //isDead = false;
        //buffRound.currentValue = buffRound.maxValue;
        //NewTurn();
    }


}