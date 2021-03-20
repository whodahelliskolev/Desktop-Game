using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadManager : MonoBehaviour
{
    public int EnemiesAlive;
    
    public void DecreaseSquadCount()
    {
        EnemiesAlive--;
    }

}
