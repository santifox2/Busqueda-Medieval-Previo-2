using UnityEngine;

// Solo prepara recursos que no conviene duplicar manualmente en cada escena.
// Las escenas de menú y juego se conservan como archivos nativos de Unity.
public class MontajePrevio2 : MonoBehaviour
{
    private void Awake()
    {
        GameObject jugador=GameObject.Find("Player");
        if(jugador==null){Debug.LogError("Previo 2: falta el Player");return;}
        jugador.tag="Player";
        if(GetComponent<GameHUDManager>()==null)gameObject.AddComponent<GameHUDManager>();
        foreach(Transform item in BuscarPergaminos())
            if(item.GetComponent<CollectibleItem>()==null)item.gameObject.AddComponent<CollectibleItem>();
        CrearNPC("NPC_Guia", "Previo2/Sabio",new Vector3(-9,-.95f,0),true);
        CrearNPC("Objeto_Interactivo_Libro", "Previo2/LibroHistoria",new Vector3(3,-.95f,0),false,
            "EL OFICIO DEL COPISTA",
            "En los monasterios medievales, los copistas reproducían textos a mano. El pergamino se elaboraba a partir de piel animal preparada para escribir y conservar documentos.");
        CrearNPC("Objeto_Interactivo_Cartel", "Previo2/CartelHistoria",new Vector3(21,-.95f,0),false,
            "LA IMPORTANCIA DE LOS CASTILLOS",
            "Durante la Edad Media, los castillos servían como residencias fortificadas y puntos de defensa. Muchos protegían poblaciones, caminos y territorios cercanos.");
        CrearLimite("MuroIzquierdo",-12f);
        CrearLimite("MuroDerecho",39f);
    }
    private System.Collections.Generic.IEnumerable<Transform> BuscarPergaminos()
    {
        for(int i=1;i<=5;i++)
        { GameObject o=GameObject.Find("Pergamino_"+i); if(o!=null)yield return o.transform; }
    }
    private void CrearNPC(string name,string asset,Vector3 pos,bool esSabio,string titulo="",string contenido="")
    {
        if(GameObject.Find(name)!=null)return;
        Sprite sprite=Resources.Load<Sprite>(asset);
        if(sprite==null){Debug.LogError("Sprite no encontrado: "+asset);return;}
        GameObject o=new GameObject(name,typeof(SpriteRenderer));o.transform.position=pos;
        o.transform.localScale=new Vector3(1.7f,1.7f,1);
        SpriteRenderer sr=o.GetComponent<SpriteRenderer>();sr.sprite=sprite;sr.sortingOrder=35;
        if(esSabio)o.AddComponent<NPCDialogue>();
        else
        {
            InteractableObject objeto=o.AddComponent<InteractableObject>();
            // El contenido tiene valores por defecto editables; se asigna por el montaje guiado.
            objeto.Configurar(titulo,contenido);
        }
    }
    // Muros físicos visibles, situados fuera del recorrido de los 5 pergaminos.
    // Permiten demostrar P1 sin crear obstáculos que requieran salto.
    private void CrearLimite(string name, float x)
    {
        if (GameObject.Find(name) != null) return;
        GameObject muro = new GameObject(name, typeof(SpriteRenderer), typeof(BoxCollider2D));
        muro.transform.position = new Vector3(x, 0.1f, 0f);
        muro.transform.localScale = new Vector3(1.65f, 1.45f, 1f);

        SpriteRenderer dibujo = muro.GetComponent<SpriteRenderer>();
        dibujo.sprite = Resources.Load<Sprite>("Previo2/MuroPiedra");
        dibujo.sortingOrder = 34;
        if (dibujo.sprite == null)
            Debug.LogError("No se pudo cargar el sprite del muro de piedra.");

        BoxCollider2D colision = muro.GetComponent<BoxCollider2D>();
        colision.isTrigger = false;
        colision.size = new Vector2(1.0f, 3.0f);
        colision.offset = Vector2.zero;
    }
}
