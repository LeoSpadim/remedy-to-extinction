using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RemedioButtons : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    public Remedio remedio;

    [SerializeField] private int amount = 5;

    [SerializeField] private GameObject infoPanel, infoText;

    GameObject panel;

    Canvas canvas;

    public static RemedioButtons[] remedioButtons;


    Button botao;
    void Awake()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
        botao = GetComponent<Button>();
        remedioButtons = FindObjectsOfType<RemedioButtons>();
    }

    void Start()
    {
        text.text = remedio.nome;
    }

    void Update()
    {
        if(amount <= 0)
        {
            botao.interactable = false;
            amount = 0;
            // codigo para mostrar que remedio acabou
        }
        else
        {
            botao.interactable = true;
        }
        if(IsMouseOverUIWithIgnores() && panel == null && !DragUI.isDragging)
        {
            panel = Instantiate(infoPanel, Camera.main.ScreenToWorldPoint(Input.mousePosition),
             Quaternion.identity);
             panel.transform.SetParent(canvas.transform, false);

            TMP_Text txt = Instantiate(infoText, Camera.main.ScreenToWorldPoint
                (Input.mousePosition), Quaternion.identity).GetComponent<TMP_Text>();
            txt.transform.SetParent(panel.transform, false);
            txt.text = amount.ToString();

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

    public void SelectRemedio()
    {
        AudioManager.Singleton.PlaySound("sfx_pen_writing");

        LevelManager.Singleton.remedioSelecionado = remedio;
        LevelManager.Singleton.SelectRemedioButtons(this);
        Debug.Log("Selecionou Remedio " + remedio.nome);
    }

    public static bool AcabaramRemedios() 
    {
        bool tinhaRemedio = false;
        foreach (var botao in remedioButtons)
        {
            if(botao.amount <= 0) {continue;}
            tinhaRemedio = true;
            break;
        }
        return tinhaRemedio;
    }

    public int GetAmountRemedio()
    {
        return amount;
    }

    public bool ConsomeRemedio()
    {
        bool b = false;
        if(amount == 1)
        {
            amount--;
            b = true;
        }
        else if(amount < 1)
        b = false;
        else
        {
            amount--;
            b = false;
        }
        amount = Mathf.Clamp(amount, 0, 99);
        return b;
    }

    public void AumentaRemedio(int quantiaAumenta)
    {
        amount += quantiaAumenta;
        amount = Mathf.Clamp(amount, 0, 99);
    }

}
