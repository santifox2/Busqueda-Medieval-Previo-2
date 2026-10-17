using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

// HUD y ventanas pedagógicas. Se construyen con UGUI para evitar dependencias externas.
public class GameHUDManager : MonoBehaviour
{
    public static GameHUDManager Instance { get; private set; }
    public bool BloqueaMovimiento { get { return dialogoActivo || informacionActiva || completado; } }
    public bool DialogoActivo { get { return dialogoActivo; } }
    public bool InformacionActiva { get { return informacionActiva; } }
    public int Recolectados { get { return recolectados; } }

    private int recolectados;
    private const int Meta = 5;
    private bool dialogoActivo, informacionActiva, completado;
    private Font fuente;
    private Text contador, objetivo, aviso, nombreDialogo, textoDialogo, tituloInfo, textoInfo;
    private Image[] casillas = new Image[Meta];
    private GameObject panelDialogo, panelInfo, panelCompletado;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(this); return; }
        Instance = this;
        fuente = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        CrearInterfaz();
        RefrescarHUD();
    }
    private void OnDestroy() { if (Instance == this) Instance = null; }
    public void AddCollectible(int cantidad)
    {
        if (completado || cantidad <= 0) return;
        recolectados = Mathf.Min(Meta, recolectados + cantidad);
        RefrescarHUD();
        if (recolectados >= Meta)
        {
            completado = true;
            panelDialogo.SetActive(false);
            panelInfo.SetActive(false);
            dialogoActivo = informacionActiva = false;
            panelCompletado.SetActive(true);
            aviso.text = "¡Misión completada!";
        }
    }
    public void MostrarAviso(string mensaje) { if (!dialogoActivo && !informacionActiva && !completado) aviso.text = mensaje; }
    public void LimpiarAviso() { if (!dialogoActivo && !informacionActiva && !completado) aviso.text = "A / D: mover  |  E: interactuar"; }
    public bool AbrirDialogo(string nombre, string mensaje)
    {
        // P5: nunca abrir un segundo diálogo sobre otro panel activo.
        if (dialogoActivo || informacionActiva || completado) return false;
        dialogoActivo = true;
        panelDialogo.SetActive(true);
        nombreDialogo.text = nombre;
        textoDialogo.text = mensaje;
        aviso.text = "E: continuar diálogo";
        return true;
    }
    public void ActualizarDialogo(string nombre, string mensaje)
    {
        if (!dialogoActivo) return;
        nombreDialogo.text = nombre;
        textoDialogo.text = mensaje;
    }
    public void CerrarDialogo()
    {
        dialogoActivo = false;
        if (panelDialogo != null) panelDialogo.SetActive(false);
        LimpiarAviso();
    }
    public bool AbrirInformacion(string titulo, string contenido)
    {
        // P5: evitar paneles informativos duplicados o superpuestos.
        if (dialogoActivo || informacionActiva || completado) return false;
        informacionActiva = true;
        tituloInfo.text = titulo;
        textoInfo.text = contenido;
        panelInfo.SetActive(true);
        aviso.text = "E: cerrar información";
        return true;
    }
    public void CerrarInformacion()
    {
        informacionActiva = false;
        if (panelInfo != null) panelInfo.SetActive(false);
        LimpiarAviso();
    }
    private void RefrescarHUD()
    {
        if (contador != null) contador.text = "PERGAMINOS  " + recolectados + " / " + Meta;
        if (objetivo != null) objetivo.text = recolectados < Meta ? "MISIÓN: Recupera cinco manuscritos históricos" : "MISIÓN CUMPLIDA";
        for (int i = 0; i < Meta; i++) if (casillas[i] != null)
            casillas[i].color = i < recolectados ? new Color(0.99f, 0.78f, 0.32f) : new Color(0.27f, 0.33f, 0.39f);
    }
    private void CrearInterfaz()
    {
        GameObject root = new GameObject("CanvasHUD", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        Canvas canvas = root.GetComponent<Canvas>(); canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler escala = root.GetComponent<CanvasScaler>();
        escala.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        escala.referenceResolution = new Vector2(1920, 1080);
        escala.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        escala.matchWidthOrHeight = 0.5f;
        if (FindFirstObjectByType<EventSystem>() == null)
            new GameObject("SistemaEventos", typeof(EventSystem), typeof(StandaloneInputModule));

        RectTransform cabecera = Panel(root.transform, "FranjaMision", new Color(0.075f,0.105f,0.17f,0.92f));
        Anchors(cabecera, new Vector2(0,1), new Vector2(1,1), new Vector2(0.5f,1));
        cabecera.offsetMin = new Vector2(0,-130); cabecera.offsetMax = Vector2.zero;
        objetivo = Texto(cabecera, "TextoObjetivo", "", 29, TextAnchor.MiddleLeft, new Color(1,.88f,.62f));
        Rect(objetivo.rectTransform, new Vector2(.34f,.68f), new Vector2(.63f,.56f),new Vector2(1180,72));
        contador = Texto(cabecera,"ContadorPergaminos", "", 32,TextAnchor.MiddleRight,Color.white);
        Rect(contador.rectTransform,new Vector2(.94f,.68f),new Vector2(1,.5f),new Vector2(500,72));
        for (int i=0;i<Meta;i++)
        {
            RectTransform celda = Panel(cabecera,"Inventario_"+(i+1), new Color(.25f,.30f,.38f));
            Rect(celda,new Vector2(.63f+i*.042f,.19f),new Vector2(.5f,.5f),new Vector2(50,44));
            casillas[i] = celda.GetComponent<Image>();
            Text simbolo = Texto(celda,"IconoPergamino","◆",29,TextAnchor.MiddleCenter,Color.white);
            Rect(simbolo.rectTransform,new Vector2(.5f,.5f),new Vector2(.5f,.5f),new Vector2(46,42));
        }
        aviso = Texto(root.transform,"AvisoInteraccion","A / D: mover  |  E: interactuar",27,TextAnchor.MiddleCenter,new Color(1,.89f,.66f));
        Anchors(aviso.rectTransform,new Vector2(.5f,0),new Vector2(.5f,0),new Vector2(.5f,0));
        aviso.rectTransform.anchoredPosition = new Vector2(0,78); aviso.rectTransform.sizeDelta = new Vector2(1300,70);

        panelDialogo = Ventana(root.transform,"DialoguePanel", "DIÁLOGO CON EL SABIO", out nombreDialogo,out textoDialogo);
        panelInfo = Ventana(root.transform,"InfoDisplayPanel", "LECTURA HISTÓRICA",out tituloInfo,out textoInfo);
        panelCompletado = new GameObject("PanelMisionCumplida",typeof(RectTransform),typeof(Image));
        panelCompletado.transform.SetParent(root.transform,false);
        RectTransform fin=panelCompletado.GetComponent<RectTransform>();
        Anchors(fin,new Vector2(.5f,.5f),new Vector2(.5f,.5f),new Vector2(.5f,.5f));
        fin.sizeDelta = new Vector2(1050,610);
        fin.GetComponent<Image>().color = new Color(.06f,.12f,.21f,.98f);
        Text titulo=Texto(fin,"TituloVictoria","¡MISIÓN CUMPLIDA!",63,TextAnchor.MiddleCenter,new Color(1,.82f,.42f));
        Rect(titulo.rectTransform,new Vector2(.5f,.79f),new Vector2(.5f,.5f),new Vector2(900,100));
        Text mensaje=Texto(fin,"DescripcionVictoria","Has recuperado los cinco manuscritos.\nConocer y conservar la historia protege nuestra memoria cultural.\n¡Habla con el sabio para continuar aprendiendo!",34,TextAnchor.MiddleCenter,Color.white);
        Rect(mensaje.rectTransform,new Vector2(.5f,.49f),new Vector2(.5f,.5f),new Vector2(940,240));
        Boton(fin,"BotonReiniciar","VOLVER A JUGAR",new Vector2(-237,-207),()=>SceneManager.LoadScene("GameScene"));
        Boton(fin,"BotonMenu","MENÚ PRINCIPAL",new Vector2(237,-207),()=>SceneManager.LoadScene("MainMenu"));
        panelDialogo.SetActive(false); panelInfo.SetActive(false); panelCompletado.SetActive(false);
    }
    private GameObject Ventana(Transform root, string name, string baseTitle, out Text titulo, out Text cuerpo)
    {
        GameObject panel = new GameObject(name,typeof(RectTransform),typeof(Image));
        panel.transform.SetParent(root,false); RectTransform rect=panel.GetComponent<RectTransform>();
        Anchors(rect,new Vector2(.5f,0),new Vector2(.5f,0),new Vector2(.5f,0));
        rect.sizeDelta=new Vector2(1410,345); rect.anchoredPosition=new Vector2(0,285);
        panel.GetComponent<Image>().color=new Color(.08f,.12f,.21f,.97f);
        titulo = Texto(rect,"NombreOrador",baseTitle,41,TextAnchor.MiddleLeft,new Color(1,.8f,.41f));
        Rect(titulo.rectTransform,new Vector2(.48f,.81f),new Vector2(.5f,.5f),new Vector2(1280,62));
        cuerpo = Texto(rect,"ContenidoEducativo","",34,TextAnchor.MiddleLeft,Color.white);
        Rect(cuerpo.rectTransform,new Vector2(.5f,.47f),new Vector2(.5f,.5f),new Vector2(1290,180));
        Text ayuda=Texto(rect,"AyudaTecla","E: continuar / cerrar",23,TextAnchor.MiddleRight,new Color(.8f,.9f,.96f));
        Rect(ayuda.rectTransform,new Vector2(.83f,.12f),new Vector2(.5f,.5f),new Vector2(450,45));
        return panel;
    }
    private void Boton(Transform parent,string name,string label,Vector2 position,UnityEngine.Events.UnityAction action)
    {
        GameObject o =new GameObject(name,typeof(RectTransform),typeof(Image),typeof(Button));
        o.transform.SetParent(parent,false);RectTransform r=o.GetComponent<RectTransform>();
        Rect(r,new Vector2(.5f,.5f),new Vector2(.5f,.5f),new Vector2(385,88));r.anchoredPosition=position;
        o.GetComponent<Image>().color=new Color(.87f,.67f,.36f);
        Button b=o.GetComponent<Button>();b.onClick.AddListener(action);
        Text t=Texto(r,"Texto",label,31,TextAnchor.MiddleCenter,new Color(.12f,.14f,.2f));
        Rect(t.rectTransform,new Vector2(.5f,.5f),new Vector2(.5f,.5f),new Vector2(375,83));
    }
    private static RectTransform Panel(Transform parent,string name,Color color)
    {
        GameObject o=new GameObject(name,typeof(RectTransform),typeof(Image));o.transform.SetParent(parent,false);
        o.GetComponent<Image>().color=color;return o.GetComponent<RectTransform>();
    }
    private Text Texto(Transform parent,string name,string content,int size,TextAnchor alignment,Color color)
    {
        GameObject o=new GameObject(name,typeof(RectTransform),typeof(Text));o.transform.SetParent(parent,false);
        Text t=o.GetComponent<Text>();t.font=fuente;t.fontSize=size;t.alignment=alignment;t.text=content;t.color=color;
        t.horizontalOverflow=HorizontalWrapMode.Wrap;t.verticalOverflow=VerticalWrapMode.Truncate;t.raycastTarget=false;
        return t;
    }
    private static void Anchors(RectTransform r,Vector2 min,Vector2 max,Vector2 pivot)
    {r.anchorMin=min;r.anchorMax=max;r.pivot=pivot;r.anchoredPosition=Vector2.zero;}
    private static void Rect(RectTransform r,Vector2 anchor,Vector2 pivot,Vector2 size)
    { Anchors(r,anchor,anchor,pivot);r.sizeDelta=size; }
}
