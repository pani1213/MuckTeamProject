using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    public int health    = 100;
    public int Maxhealth = 100;
    void Start()
    {
        health = Maxhealth;
    }
 
}
