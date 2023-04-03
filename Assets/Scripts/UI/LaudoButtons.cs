using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LaudoButtons : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    public Doenca doenca;
    private Button botao;

    [SerializeField] private GameObject infoPanel, infoText;

    public bool isBloqueado {get; private set;} = true;

    GameObject panel;

    Canvas canvas;

    public static LaudoButtons[] laudoButtons;

  //  public Transform spawnInfo;

    void Awake()
    {
        if(doenca.getNivelDoenca == 0) {isBloqueado = false;}
        laudoButtons = FindObjectsOfType<LaudoButtons>();
        botao = GetComponent<Button>();
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
    }

    void Start()
    {
        ChecarBotao();
    }

    public static LaudoButtons BotaoComEssaDoenca(Doenca d)
    {
        foreach (var laudo in laudoButtons)
        {
            if(laudo.doenca == d) {return laudo;}
        }
        return null;
    }


    void Update()
    {
      //  ChecarBotao();
        if(IsMouseOverUIWithIgnores() && panel == null && !isBloqueado && !DragUI.isDragging)
        {
            panel = Instantiate(infoPanel, Camera.main.ScreenToWorldPoint(Input.mousePosition),
             Quaternion.identity);
             panel.transform.SetParent(canvas.transform, false);
             for (int i = 0; i < doenca.getSintomas.tiposSintomas.Length; i++)
            {
                TMP_Text txt = Instantiate(infoText, Camera.main.ScreenToWorldPoint
                (Input.mousePosition), Quaternion.identity).GetComponent<TMP_Text>();
                txt.transform.SetParent(panel.transform, false);
                txt.text = "";
                txt.text += doenca.getSintomas.tiposSintomas[i];
                txt.text = txt.text.Replace("_", " ");
            }
            // Vector2 position;
            // RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform) canvas.transform, Input.mousePosition, canvas.worldCamera, out position);
            // Image img = panel.GetComponent<Image>();
            // Vector3 offset = new Vector3(img.rectTransform.sizeDelta.x / 300, (img.rectTransform.sizeDelta.y / 70) * -1, 0);
            // Debug.LogError(offset);
            // panel.transform.position = offset + canvas.transform.TransformPoint(position);
            panel.transform.position = transform.position + new Vector3(2f, 0, 0);
        }
        else if((!IsMouseOverUIWithIgnores() || DragUI.isDragging) && panel != null)
        {
            Destroy(panel);
        }
    }

    public bool isMouseOverButton()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    private bool IsMouseOverUIWithIgnores()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> raycastResultList = new List<RaycastResult>();

        EventSystem.current.RaycastAll(pointerEventData, raycastResultList);
        for (int i = 0; i < raycastResultList.Count; i++)
        {
            if(raycastResultList[i].gameObject != gameObject)
            {
                raycastResultList.RemoveAt(i);
                i--;
            }
        }
        return raycastResultList.Count > 0;
    }

    public void DesbloqueaBotao()
    {
        isBloqueado = false;
    }

    private Vector3 GetMousePosition()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> raycastResultList = new List<RaycastResult>();

        EventSystem.current.RaycastAll(pointerEventData, raycastResultList);
        for (int i = 0; i < raycastResultList.Count; i++)
        {
            if(raycastResultList[i].gameObject != gameObject)
            {
                raycastResultList.RemoveAt(i);
                i--;
            }
        }
        if(raycastResultList.Count > 0)
        {
            return raycastResultList[0].screenPosition;
        }
        return Vector3.zero;
    }

    public void ChecarBotao()
    {
        if(IsBloqueado()) {return;}
        text.text = doenca.nome;
        botao.interactable = true;
    }

    public static List<LaudoButtons> BotoesDoencasDesseNivel(int nivel)
    {
        List<LaudoButtons> buttons = new List<LaudoButtons>();

        foreach (var laudo in laudoButtons)
        {
            if(laudo.doenca.getNivelDoenca == nivel) {buttons.Add(laudo);}
        }
        return buttons;
    }

    private bool IsBloqueado()
    {
        if(isBloqueado)
        {
            text.text = "BLOQUEADO";
            botao.interactable = false;
            return true;
        }
        return false;
    }

    public void SelectLaudo()
    {
        AudioManager.Singleton.PlaySound("sfx_pen_writing");

        LevelManager.Singleton.doencaSelecionada = doenca;
        Debug.Log("Selecionou Doenca " + doenca.nome);
    }
}
