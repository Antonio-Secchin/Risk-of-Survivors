using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

/// <summary>
/// A classe <c> FlyingEnemy</c> controla os sprites de todos os inimigos voadores, ajustando-os para a direcao onde esta o alvo
/// </summary>
public class FlyingEnemy : MonoBehaviour
{
    public AIPath aiPath;

    // Update is called once per frame
    void Update()
    {
        //Muda a sprite para a direcao onde esta o alvo
        if(aiPath.desiredVelocity.x > 0) 
        {
            transform.localScale = new Vector3(1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if(aiPath.desiredVelocity.x < 0) 
        {
            transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        
    }
}
