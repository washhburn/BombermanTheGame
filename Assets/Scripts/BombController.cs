using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BombController : MonoBehaviour
{
    [Header("Bomb")]
    public KeyCode bombKey = KeyCode.Space;
    public GameObject bombPrefab;
    public float bombTimer = 3f;
    public int bombCount = 1;
    private int bombsLeft;

    [Header("Explosion")]
    public Explosion explosionPrefab;
    public LayerMask explosionLayerMask;
    public float explosionTime = 1f;
    public int explosionRadius = 1;

    [Header("Destructible")]
    public Tilemap destructibleTiles;
    public Destructible destructiblePrefab;


    private void OnEnable() //en metod som körs när scriptet aktiveras
    {
        bombsLeft = bombCount; //sätter bombsLeft till det angivna bombCount när scriptet aktiveras
    }

    private void Update() //en metod som körs varje frame och hanterar input för att placera bomber
    {
        if (bombsLeft > 0 && Input.GetKeyDown(bombKey)) //kontrollerar om bombKey tryckts ner & att det finns bomber kvar
        {
            StartCoroutine(PlaceBomb());
        }
    }

    private IEnumerator PlaceBomb() //en korutin som hanterar placeringen av bomben, väntan på explosionen och explosionens effekter
    {
        //placera bomben på den närmaste heltalspositionen för att undvika att den fastnar i väggar eller andra objekt
        Vector2 position = transform.position; //hämtar den aktuella positionen för spelaren
        position.x = Mathf.Round(position.x); //avrundar x-koordinaten till närmaste heltal
        position.y = Mathf.Round(position.y) -0.05f; //avrundar y-koordinaten till närmaste heltal

        GameObject bomb = Instantiate(bombPrefab, position, Quaternion.identity);  //skapar en instans av bombPrefab på den aktuella positionen
        bombsLeft--; //minskar antalet bomber kvar med 1

        yield return new WaitForSeconds(bombTimer); //väntar i bombTimer sekunder innan explosionen sker

        position = bomb.transform.position; //hämtar den aktuella positionen för bomben
        position.x = Mathf.Round(position.x); //avrundar x-koordinaten till närmaste heltal
        position.y = Mathf.Round(position.y); //avrundar y-koordinaten till närmaste heltal

        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity); //skapar en instans av explosionPrefab på den avrundade positionen
        explosion.SetActiveRenderer(explosion.start); //sätter explosionens aktiva renderer till start
        explosion.DestroyAfter(explosionTime); //förstör explosionen efter explosionTime sekunder

        Explode(position, Vector2.up, explosionRadius); //explodera i uppåtgående riktning
        Explode(position, Vector2.down, explosionRadius); //explodera i nedåtgående riktning
        Explode(position, Vector2.left, explosionRadius); //explodera i vänster riktning
        Explode(position, Vector2.right, explosionRadius); //explodera i höger riktning

        Destroy(bomb);
        bombsLeft++; //ökar antalet bomber kvar med 1 när bomben förstörs
    }

    public void Explode(Vector2 position, Vector2 direction, int length) //en rekursiv metod som hanterar explosionens spridning i en given riktning och längd
    {
        if (length <= 0)
        {
            return;
        }

        position+= direction; //flyttar positionen i den angivna riktningen

        if (Physics2D.OverlapBox(position, Vector2.one / 2f, 0f, explosionLayerMask)) //kontrollerar om det finns en kolliderare i explosionLayerMask på den nya positionen
        {
            ClearDestructible(position); //om det finns en kolliderare i explosionLayerMask på den nya positionen, förstör det förstörbara objektet på den positionen
            return; //om det finns en kolliderare i explosionLayerMask på den nya positionen, stoppa explosionen i den riktningen
        }


        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity); //skapar en instans av explosionPrefab på den nya positionen
        explosion.SetActiveRenderer(length > 1 ? explosion.middle : explosion.end); //sätter explosionens aktiva renderer till middle om längden är större än 1, annars sätter den till end
        explosion.SetDirection(direction); //sätter explosionens riktning så att den kan anpassa sin animation och kollisionshantering
        explosion.DestroyAfter(explosionTime); //förstör explosionen efter explosionTime sekunder

        Explode(position, direction, length - 1); //rekursivt anropa Explode för att fortsätta explodera i samma riktning tills längden är 0 eller mindre
    }

    private void ClearDestructible(Vector2 position) //en metod som hanterar förstöringen av ett destruktivt objekt på en given position
    {
        Vector3Int cell = destructibleTiles.WorldToCell(position); //konverterar den världsliga positionen till en cellposition i Tilemap
        TileBase tile = destructibleTiles.GetTile(cell); //hämtar den tile som finns på den cellpositionen

        if (tile != null) //kontrollerar om det finns en tile på den cellpositionen
        {  
            Instantiate(destructiblePrefab, position, Quaternion.identity); //skapar en instans av destructiblePrefab på den positionen
            destructibleTiles.SetTile(cell, null); //tar bort tile från Tilemap på den positionen
        }
    }

    private void OnTriggerExit2D(Collider2D other) //en metod som hanterar när ett objekt lämnar bombens trigger-kolliderare
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bomb")) //kontrollerar om det objekt som lämnar triggern är en bomb
        {
            other.isTrigger = false; //sätter bombens collider till att inte vara en trigger längre så att den kan kollidera med spelaren och andra objekt
        }
    }

    public void TriggerExplosion()
    {
        StopAllCoroutines();

        Vector2 position = transform.position; //hämtar den aktuella positionen för spelaren
        position.x = Mathf.Round(position.x); //avrundar x-koordinaten till närmaste heltal
        position.y = Mathf.Round(position.y) -0.05f; //avrundar y-koordinaten till närmaste heltal

        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity); //skapar en instans av explosionPrefab på den aktuella positionen
        explosion.SetActiveRenderer(explosion.start); //sätter explosionens aktiva renderer till start
        explosion.DestroyAfter(explosionTime); //förstör explosionen efter explosionTime sekunder

        Explode(position, Vector2.up, explosionRadius); //explodera i uppåtgående riktning
        Explode(position, Vector2.down, explosionRadius); //explodera i nedåtgående riktning
        Explode(position, Vector2.left, explosionRadius); //explodera i vänster riktning
        Explode(position, Vector2.right, explosionRadius); //explodera i höger riktning

        bombsLeft++; //ökar antalet bomber kvar med 1 när bomben förstörs
        Destroy(gameObject); //förstör bomben som utlöste explosionen
    }

    public void AddBomb() //en metod som hanterar när spelaren plockar upp en powerup som ger extra bomber
    {
        bombCount++; //ökar det totala antalet bomber som spelaren kan placera med 1
        bombsLeft++; //ökar antalet bomber kvar med 1 så att spelaren kan använda den nya bomben direkt
    }
}
