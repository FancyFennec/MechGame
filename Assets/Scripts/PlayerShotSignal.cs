using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Security.AccessControl;

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
        List<int> objectsToDelete = new List<int>();
        subscribers.ForEach(subscriber =>
		{
            try
            {
                subscriber.Notify();

            }
            catch (MissingReferenceException)
            {
				int index = subscribers.IndexOf(subscriber);
                objectsToDelete.Add(index);
            }
        });

        for( int i = objectsToDelete.Count -1; i > 0; i--)
		{
            subscribers.RemoveAt(objectsToDelete[i]);
        }
    }
}

public interface ISubscriber
{
    void Notify();
}