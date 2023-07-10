using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //bool EnemyIsAlive()
    //{
    //    searchCountdown -= Time.deltaTime;
    //    if (searchCountdown <= 0f)
    //    {
    //        searchCountdown = 1f;
    //        /* Atenção: este é um método bastante custoso. 
    //           Havendo muitos GameObjects em jogo, pode trazer problemas de performance. 
    //           Nossa proposta é pequena e não devemos ter problemas, mas fica o aviso. */
    //        if (GameObject.FindGameObjectsWithTag("Enemy") == null)
    //        {
    //            return false;
    //        }     
    //    }
    //    return true;
    //}

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

        //state = SpawnState.WAITING;

        //yield break;
    }

    void SpawnEnemy(Transform _enemy)
    {
        Debug.Log("Spawning enemy: " + _enemy.name);
        Transform _spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(_enemy, _spawnPoint.position, _spawnPoint.rotation);
    }
}