using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A classe <c> MobSpawner()</c> controla o spawn de diferentes inimigos no mapa.
/// </summary>
public class MobSpawner : MonoBehaviour
{
    public enum SpawnState { SPAWNING, WAITING, COUNTING}

    [System.Serializable]
    public class Mob 
    {
        public string name;
        public Transform enemy;
        public float spawnRate;
        public int max;
    }

    public Mob[] mobs;
    public Transform[] spawnPoints;
    public int currentQuantity = 0; // Pode/deve ser colocada como private depois

    // Start is called before the first frame update
    void Start()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.Log("Erro! Nenhum SpawnPoint para este inimigo foi referenciado!");
        }

        StartCoroutine(StartMobSpawn(mobs[0]));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// A funcao <c> StartMobSpawn()</c> inicia o spawn de inimigos a partir do MobSpawner.
    /// </summary>
    IEnumerator StartMobSpawn(Mob _mob)
    {
        Debug.Log("Spawning wave: " + _mob.name);

        for (int i = 0;; i++)
        {
            yield return new WaitForSeconds(_mob.spawnRate);
            if (currentQuantity < _mob.max)
            {
                SpawnEnemy(_mob.enemy);
                currentQuantity++;
            }
        }
    }

    /// <summary>
    /// A funcao <c> SpawnEnemy()</c> spawna o inimigo, o instanciando em um dos pontos de spawn.
    /// </summary>
    void SpawnEnemy(Transform _enemy)
    {
        Debug.Log("Spawning enemy: " + _enemy.name);
        Transform _spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(_enemy, _spawnPoint.position, _spawnPoint.rotation);
    }
}
