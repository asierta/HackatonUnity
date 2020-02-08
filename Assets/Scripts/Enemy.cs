using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public void OnDamage(int value)
    {
        //print(this.name + " Damaged");
    }
}
