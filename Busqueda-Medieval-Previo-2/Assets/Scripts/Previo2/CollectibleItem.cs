using UnityEngine;

// Un pergamino se puede recoger exactamente una vez.
[RequireComponent(typeof(SpriteRenderer))]
public class CollectibleItem : MonoBehaviour
{
    [SerializeField] private string itemName = "Pergamino histórico";
    [SerializeField] private int itemValue = 1;
    private bool recogido;
    private void Awake()
    {
        BoxCollider2D c = GetComponent<BoxCollider2D>();
        if (c == null) c=gameObject.AddComponent<BoxCollider2D>();
        c.isTrigger=true;
        // El pergamino se encuentra visualmente sobre el suelo.
        c.size=new Vector2(0.55f,0.65f);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (recogido || !other.CompareTag("Player")) return;
        recogido=true;
        GetComponent<Collider2D>().enabled=false;
        if (GameHUDManager.Instance!=null) GameHUDManager.Instance.AddCollectible(Mathf.Max(1,itemValue));
        Destroy(gameObject);
    }
}
