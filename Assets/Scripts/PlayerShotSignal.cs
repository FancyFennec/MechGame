using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerShotSignal
{
    private List<ISubscriber> subscribers = new List<ISubscriber>();


    public PlayerShotSignal()
    {
    }

    public void Subscribe(ISubscriber subscriber)
    {
        subscribers.Add(subscriber);
    }

    public void Emmit()
    {
        subscribers.ForEach(subscriber => subscriber.Notify());
    }
    
}

public interface ISubscriber
{
    void Notify();
}