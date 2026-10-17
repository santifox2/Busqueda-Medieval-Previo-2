using UnityEngine;

// Diálogo educativo de cuatro intervenciones. Las frases son editables desde el Inspector.
public class NPCDialogue : MonoBehaviour
{
    [SerializeField] private string npcName = "Sabio Medieval";
    [SerializeField, TextArea(2,5)] private string[] dialogueLines = {
        "¡Bienvenido, joven caballero! En esta región se han perdido cinco manuscritos históricos.",
        "En la Edad Media muchos textos se copiaban a mano. Los monasterios fueron importantes centros de escritura.",
        "Busca los cinco pergaminos y consulta los dos puntos de lectura del camino.",
        "Regresa cuando hayas reunido los cinco. ¡El conocimiento es nuestro mayor tesoro!"
    };
    private bool cerca, hablando;
    private int indice;
    private TextMesh indicador;
    private void Awake()
    {
        BoxCollider2D c=GetComponent<BoxCollider2D>();if (c==null)c=gameObject.AddComponent<BoxCollider2D>();
        c.isTrigger=true;c.size=new Vector2(2.35f,2.5f);
        indicador=CrearIndicador("E: HABLAR");indicador.gameObject.SetActive(false);
    }
    private TextMesh CrearIndicador(string mensaje)
    {
        GameObject label=new GameObject("IndicadorFlotante_E");label.transform.SetParent(transform,false);
        label.transform.localPosition=new Vector3(0,1.1f,-.2f);
        TextMesh t=label.AddComponent<TextMesh>();t.text=mensaje;t.anchor=TextAnchor.MiddleCenter;
        t.alignment=TextAlignment.Center;t.fontSize=45;t.characterSize=.12f;t.color=new Color(1f,.86f,.47f);
        return t;
    }
    private void Update()
    {
        if (!cerca || !Input.GetKeyDown(KeyCode.E) || GameHUDManager.Instance==null) return;
        GameHUDManager ui=GameHUDManager.Instance;
        if (ui.InformacionActiva)return;
        if (!hablando)
        {
            if (ui.DialogoActivo || dialogueLines==null || dialogueLines.Length==0)return;
            hablando=ui.AbrirDialogo(npcName,dialogueLines[0]);indice=0;
            if (hablando)indicador.gameObject.SetActive(false);
        }
        else
        {
            indice++;
            if (indice<dialogueLines.Length)ui.ActualizarDialogo(npcName,dialogueLines[indice]);
            else Terminar();
        }
    }
    private void Terminar()
    {
        hablando=false;
        if(GameHUDManager.Instance!=null)GameHUDManager.Instance.CerrarDialogo();
        if(indicador!=null) indicador.gameObject.SetActive(cerca);
    }
    private void OnTriggerEnter2D(Collider2D c)
    {
        if(!c.CompareTag("Player"))return;
        cerca=true;if(indicador!=null && !hablando)indicador.gameObject.SetActive(true);
        if(GameHUDManager.Instance!=null)GameHUDManager.Instance.MostrarAviso("Sabio Medieval: presiona E para hablar");
    }
    private void OnTriggerExit2D(Collider2D c)
    {
        if(!c.CompareTag("Player"))return;
        cerca=false;if(indicador!=null)indicador.gameObject.SetActive(false);
        if(hablando)Terminar();else if(GameHUDManager.Instance!=null)GameHUDManager.Instance.LimpiarAviso();
    }
}
