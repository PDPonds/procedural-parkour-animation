using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MainObserver : MonoBehaviour
{
    List<IObserver> _observers = new List<IObserver>();

    public void AddObserver(IObserver observer)
    {
        _observers.Add(observer);
    }

    public void RemoveObserver(IObserver observer)
    {
        _observers.Remove(observer);
    }

    protected void ActiveAllObserver(PlayerAction action)
    {
        _observers.ForEach((_observers) => { _observers.FuncToDo(action); });
    }

}

public interface IObserver
{
    public void FuncToDo(PlayerAction action);
}

public enum PlayerAction
{
    Walk, Aim, Jump, Shoot, ClimbUp, ClimbUpHeight, JumpForward, Attack
}