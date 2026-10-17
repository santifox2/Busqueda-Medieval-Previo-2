using UnityEngine;

// Puntos de lectura con texto pedagógico editable en el Inspector.
public class InteractableObject : MonoBehaviour
{
    [SerializeField] private string objectTitle="Historia Medieval";
    [SerializeField, TextArea(3,8)] private string educationalContent="Los manuscritos medievales eran copiados a mano.";
    private bool cerca, abierto;
    private TextMesh indicador;
    private void Awake()
    {
        BoxCollider2D c=GetComponent<BoxCollider2D>();if(c==null)c=gameObject.AddComponent<BoxCollider2D>();
        c.isTrigger=true;c.size=new Vector2(2.4f,2.5f);
        GameObject texto=new GameObject("IndicadorFlotante_E");texto.transform.SetParent(transform,false);
        texto.transform.localPosition=new Vector3(0,1.12f,-.2f);
        indicador=texto.AddComponent<TextMesh>();indicador.text="E: LEER";indicador.anchor=TextAnchor.MiddleCenter;
        indicador.alignment=TextAlignment.Center;indicador.fontSize=45;indicador.characterSize=.12f;
        indicador.color=new Color(1f,.86f,.47f);texto.SetActive(false);
    }
    private void Update()
    {
        if(!cerca || !Input.GetKeyDown(KeyCode.E) || GameHUDManager.Instance==null)return;
        GameHUDManager ui=GameHUDManager.Instance;
        if(ui.DialogoActivo)return;
        if(abierto){abierto=false;ui.CerrarInformacion();indicador.gameObject.SetActive(true);}
        else if(!ui.InformacionActiva)
        {
            abierto=ui.AbrirInformacion(objectTitle,educationalContent);
            if(abierto)indicador.gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.CompareTag("Player"))return;
        cerca=true;if(!abierto)indicador.gameObject.SetActive(true);
        if(GameHUDManager.Instance!=null)GameHUDManager.Instance.MostrarAviso("Punto educativo: presiona E para leer");
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if(!other.CompareTag("Player"))return;
        cerca=false;indicador.gameObject.SetActive(false);
        if(abierto && GameHUDManager.Instance!=null){GameHUDManager.Instance.CerrarInformacion();abierto=false;}
        else if(GameHUDManager.Instance!=null)GameHUDManager.Instance.LimpiarAviso();
    }
    public void Configurar(string titulo, string contenido) { objectTitle=titulo; educationalContent=contenido; }
}
