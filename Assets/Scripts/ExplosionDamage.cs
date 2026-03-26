using UnityEngine;

public class ExplosionDamage : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other) //en metod som körs när en annan collider kommer i kontakt med explosionens collider
    {
        if (other.CompareTag("Player")) //kontrollerar om den andra collidern har taggen "Player"
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>(); //hämtar PlayerHealth-komponenten från den andra collidern
            if (playerHealth != null) //kontrollerar om PlayerHealth-komponenten finns
            {
                playerHealth.LoseLife(); //kallar på metoden LoseLife() för att hantera när spelaren förlorar ett liv
            }
        }
        else if (other.CompareTag("Bomb"))
        {
            BombController bomb = other.GetComponent<BombController>(); //hämtar Bomb-komponenten från den andra collidern
            if (bomb != null) //kontrollerar om Bomb-komponenten finns
            {
                bomb.TriggerExplosion(); //kallar på metoden TriggerExplosion() för att hantera när en bomb exploderar
            }
        }
    }
}
