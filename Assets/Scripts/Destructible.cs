using UnityEngine;

public class Destructible : MonoBehaviour
{
    public float destroyTime = 0.5f; //tiden det tar för det destruktiva objektet att förstöras efter att det träffats av en explosion

    public float itemSpawnChance = 0.1f; //chansen att en powerup ska spawna när det destruktiva objektet förstörs
    public GameObject[] powerupPrefabs; //en array som innehåller prefabs för de olika powerups som kan spawna

    private void Start()
    {
        Destroy(gameObject, destroyTime); //förstör det destruktiva objektet efter destroyTime sekunder
    }

    private void OnDestroy() //en metod som körs när det destruktiva objektet förstörs
    {
        if (powerupPrefabs.Length > 0 && Random.value < itemSpawnChance) 
        {
            int randomIndex = Random.Range(0, powerupPrefabs.Length);
            Instantiate(powerupPrefabs[randomIndex], transform.position, Quaternion.identity); //spawna en slumpmässig powerup från arrayen på det destruktiva objektets position
        }
    }
}
