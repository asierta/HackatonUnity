using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public float health = 100;

    public void OnDamage(int value)
    {
        if (health - value <= 0)
        {
            //gameController.SendMessage("GameObjectDestroyed", gameObject.name);
            Destroy(gameObject, 2);
        }
        health -= value;
        print(this.name + " damaged " + value + " points");
    }
}
