using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Just makes a serializable key value storage
[Serializable]
public class EnemyManagerType
{
    public string name;
    public int count;
}

// Manages the enemies in a scene and makes sure theres not too many
public class EnemyManager : MonoBehaviour
{
    public int currentEnemies = 0;
    public int legitimateCurrentEnemies = 0;
    public bool spawnEnabled = false;
    public GameManager gm;

    public Dictionary<string, int> activeEnemies = new();
    public EnemyManagerType[] maxEnemies;


    private void Start() {
        // Find game manager in order to increment the amount of enemies
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Check if the spawner meets the conditions in order to spawn the enemy
    public bool CheckSpawn(string name)
    {
        if (!spawnEnabled) return false;
        
        EnemyManagerType enemyMax = Array.Find(maxEnemies, enemyType => enemyType.name == name);

        if (!activeEnemies.ContainsKey(name))
        {
            activeEnemies.Add(name, 0);
        }

        int activeCount = activeEnemies[name];
        return activeCount + 1 < enemyMax.count;
    }

    // Increment the enemy count in the game manager
    public void IncrementEnemyCount(string name)
    {
        legitimateCurrentEnemies++;
        currentEnemies++;

        activeEnemies[name]++;
    }

    // Decrease the enemy count in the game manager
    public void DecrementEnemyCount(string name)
    {

        activeEnemies[name]--;

        legitimateCurrentEnemies--;

        if (legitimateCurrentEnemies <= 0)
        {
            legitimateCurrentEnemies = 0;
            currentEnemies = 0;
        }
    }

    // IEnumerable version of the delay decrement enemy count
    public IEnumerator _DelayDecrementEnemyCount(string name, float delay)
    {
        yield return new WaitForSeconds(delay);

        DecrementEnemyCount(name);
    }

    // Same exact function as above but allows a default value for delay and also removes the necessity of calling it by using StartCoroutine
    public void DelayDecrementEnemyCount(string name, float delay = 1f)
    {
        StartCoroutine(_DelayDecrementEnemyCount(name, delay));
    }
}
