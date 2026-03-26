using UnityEngine;

public class Powerups : MonoBehaviour
{
    public enum ItemType //en enum som definierar olika typer av powerups
    {
        ExtraBomb,
        SpeedBoost,
        BlastRadius,
    }

    public ItemType item; //en variabel som håller reda på vilken typ av powerup det är

    private void OnItemPickup(GameObject player)
    {
        switch(item)
        {
            case ItemType.ExtraBomb:
                player.GetComponent<BombController>().AddBomb(); //kallar på AddBomb() i BombController för att öka antalet bomber som spelaren kan placera
                break;
            case ItemType.SpeedBoost:
                player.GetComponent<MovementController>().movementSpeed++; //kallar på movementSpeed i MovementController och ökar den med 1 för att ge spelaren en hastighetsökning
                break;
            case ItemType.BlastRadius:
                player.GetComponent<BombController>().explosionRadius++; //kallar på explosionRadius i BombController och ökar den med 1 för att ge bomben en större explosionsradie
                break;
        }
        Destroy(this.gameObject); //förstör powerupen efter att den har plockats upp
    }

    private void OnTriggerEnter2D(Collider2D other) //en metod som körs när en annan collider kommer i kontakt med powerupens collider
    {
        if (other.CompareTag("Player")) //kontrollerar om den andra collidern har taggen "Player"
        {
            OnItemPickup(other.gameObject); //kallar på metoden OnItemPickup och skickar den andra collidern som argument
        }
    }
}
