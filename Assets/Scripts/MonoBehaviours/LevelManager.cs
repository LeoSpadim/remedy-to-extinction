using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public const int META_DINHEIRO = 200;
    const int MAXINSATISFACAO = 4;
    const int MAXNIVELDOENCAS = 2;
    const float P_INFECTADOS_INICIAIS = 30f, P_MINIMO_INFEC = 10f, P_MAXIMO_INFEC = 70f;
    public static LevelManager Singleton
    {
        get
        {
            if(levelManager == null)
            {
                levelManager = FindObjectOfType<LevelManager>();
            }
            return levelManager;
        }
    }
    private static LevelManager levelManager;
    public int laudoClicado, nivelDoencas;
    public int dinheiroTotal {get; private set;}
    public int acumuloDinheiroDia {get; private set;}
    public int dia {get; private set;}
    public int objetivoPacientes {get; private set;} = QTD_PACIENTES_INICIAIS;
    public bool ganhouDia = false;
    [HideInInspector] public Remedio remedioSelecionado;
    public Doencas[] doencas;
    public Doenca doencaSelecionada;
    public List<Paciente> pacientesReincidentes {get; private set;} = new List<Paciente>();
    public Paciente pacienteAtual {get; private set;}
    public Button b_gerarReceita;
    public GameObject[] pacientesPrefabs;
    public Animator blackOutAnim, jornalAnim;
    public Populacao populacao = new Populacao(P_INFECTADOS_INICIAIS, P_MINIMO_INFEC, P_MAXIMO_INFEC);
    public UnityEvent onGerouReceita, onSpawnouPaciente, onTerminouDia, onComecouNovoDia;
    public Action<int, int> OnGanharDinheiro;
    public bool perdeu {get; private set;} = false;
    public int nivelInsatisfacaoDia {get; private set;} = 0;
    public bool isOnRevoltaEvent {get; private set;} = false;
    public GameObject fiscal;
    public bool spawnouFiscal {get; private set;} = false;
    public RemedioButtons[] remedioButtons;
    public int nivelRodizio {get; private set;}
    public float tempoDoDia {get; private set;} = 300f;
    public bool isTutorial = false;

    private List<Paciente> pacientesSpawnados = new List<Paciente>();
    private List<int> filaTipoDoenca = new List<int>();
    private List<int> diaDoencaAdicionada = new List<int>();
    private int objetivoAnterior = 3;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private TMP_Text bio_text, sintomas_text, historico_text, jornal_text;
    private float p_diaAnterior = P_INFECTADOS_INICIAIS;
    private RemedioButtons botaoRemedioSelecionado;

    private int metaDiasAumentaNivel = DIAS_PARA_AUMENTO_NIVEL;

    public const int DIAS_PARA_AUMENTO_NIVEL = 3;

    const int QTD_PACIENTES_INICIAIS = 3;

    public UnityEvent onPacienteSaiuSubscribe, onPacienteEntrouSubscribe;

    public List<int> GetFilaTIpoDoenca()
    {
        return filaTipoDoenca;
    }

    public void TriggaOnPacienteSaiu() 
    {
        onPacienteSaiuSubscribe?.Invoke();
    }
    public void TriggaOnPacienteEntrou() 
    {
        onPacienteEntrouSubscribe?.Invoke();
    }

    public void AddDoencaToFila(int tipo)
    {
        if(tipo > doencas.Length) {Debug.LogError(
            "Tentou passar tipo de doenca inexistente"); return;}

        filaTipoDoenca.Add(tipo);
        diaDoencaAdicionada.Add(dia);
    }

    public void ReabastecerRemedios(int[] qntd)
    {
        AudioManager.Singleton.PlaySound("sfx_recieve_pills");

        for (int i = 0; i < remedioButtons.Length; i++)
        {
            remedioButtons[i].AumentaRemedio(qntd[i]);
        }
    }

    public void StartRevoltaEvent()
    {
        isOnRevoltaEvent = true;
       // ZeraInsatisfacao();
    }

    public void EndRevoltaEvent()
    {
        isOnRevoltaEvent = false;
    }

    public void SelectRemedioButtons(RemedioButtons r)
    {
        botaoRemedioSelecionado = r;
    }

    public void GanharDinheiro(int dinheiro)
    {
        acumuloDinheiroDia += dinheiro;
    }

    public void AumentaObjetivoPaciente(int ganho)
    {
        objetivoPacientes += ganho;
        objetivoPacientes = Mathf.Clamp(objetivoPacientes, 6, pacientesPrefabs.Length - 4);
    }

    public void SetPacienteAtual(Paciente paciente)
    {
        pacienteAtual = paciente;
    }

    public bool IsPovoInsatisfeito()
    {
        return nivelInsatisfacaoDia > MAXINSATISFACAO;
    }

    public void GerarReceita()
    {
        if(doencaSelecionada == null || remedioSelecionado == null)
        {
            Debug.LogError("nada selecionado");
            return;
        }
        if(botaoRemedioSelecionado.ConsomeRemedio())
        {
            botaoRemedioSelecionado.GetComponent<Button>().interactable = false;
            // codigo para mostrar que remedio acabou
        }

        pacienteAtual.TentarCurar(doencaSelecionada, remedioSelecionado);
        pacienteAtual.transform.parent.GetComponent<PacienteAnimHandler>().PlayExitPaciente();
        objetivoPacientes--;
        onGerouReceita?.Invoke();
        doencaSelecionada = null;
        remedioSelecionado = null;
        ChecaGanhaFase();
        if(!RemedioButtons.AcabaramRemedios()) 
        {
            AcabaDia();
        }
    }

    void Start()
    {
        objetivoAnterior = objetivoPacientes;
        SpawnPaciente();
    }

    void SpawnPaciente()
    {
        if(ganhouDia || isTutorial) {return;}
        if(!spawnouFiscal)
        {
            Instantiate(fiscal, spawnPoint.position
            , Quaternion.identity);
            spawnouFiscal = true;
            return;
        }
        if(pacientesReincidentes.Count > 0 && pacientesReincidentes[0].ultimoDiaAtendido != dia)
        {
            ReativarPaciente(pacientesReincidentes[0]);
            pacienteAtual = pacientesReincidentes[0];

            if(!CheckForDoencasNaFila(pacienteAtual)) {pacienteAtual.GerarDoenca();}

            CuidarText(pacienteAtual);

            pacientesReincidentes.RemoveAt(0);
            onSpawnouPaciente?.Invoke();
            return;
        }

        int randomIndex = GerarRandomIndexPaciente();

        int extra = 0;

        Paciente p = AcharPacientePorId(randomIndex);

        int quebra = 0;

        while(p != null && p.ultimoDiaAtendido == dia && quebra < 80) // se esse paciente achado ja apareceu
        { // hoje
            if(quebra > 78) 
            {
                AcabaDia();
                Debug.LogError("Acabou forcado");
                return;
            }
            extra++; // aumenta a seed
            randomIndex = GerarRandomIndexPaciente(extra); // gera outro numero
            p = AcharPacientePorId(randomIndex); // troca o paciente
            quebra++;
        }

        if(p != null) // se esse paciente ja foi spawnado
        {
            // rehabilita o paciente
            ReativarPaciente(p);
        }
        else
        {
            // spawna o paciente
            p = Instantiate(pacientesPrefabs[randomIndex], spawnPoint.position
            , Quaternion.identity).GetComponentInChildren<Paciente>();
            p.SetUltimoDia(dia);
            p.bio.GerarBio();
            p.GerarDoenca();
            p.SetIndexIdentificador(randomIndex);
            pacientesSpawnados.Add(p);
        }
        CheckForDoencasNaFila(p);
        pacienteAtual = p;

        CuidarText(p);

        nivelInsatisfacaoDia += p.CalcularInSatisfacao();

        onSpawnouPaciente?.Invoke();
    }

    private bool CheckForDoencasNaFila(Paciente p)
    {
        if(filaTipoDoenca.Count > 0 && diaDoencaAdicionada[0] < dia)
        {
            UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);
            int doencaRandom = UnityEngine.Random.Range(0, doencas[filaTipoDoenca[0]].doencas.Length);
            p.SetDoenca(doencas[filaTipoDoenca[0]].doencas[doencaRandom]);
            filaTipoDoenca.RemoveAt(0);
            return true;
        }
        return false;
    }

    private void CuidarText(Paciente p)
    {
        bio_text.text = "Nome: " + p.bio.nome + "\n" + "Ocupacao: " + p.bio.ocupacao.nome + "\n" + "Destrito: " +
        p.bio.ocupacao.classe + "\n" + "Descrição: " + p.bio.descricao;
        sintomas_text.text = "Sintomas:";
        for (int i = 0; i < p.doenca.getSintomas.tiposSintomas.Length; i++)
        {
            sintomas_text.text += "\n";
            sintomas_text.text += p.doenca.getSintomas.tiposSintomas[i];
            sintomas_text.text = sintomas_text.text.Replace("_", " ");
        }
        if(p.historico.doencas != null)
        {
            historico_text.text = "Doença:    Remédio:   Efetividade:";
            for (int i = 0; i < p.historico.doencas.Count; i++)
            {
                historico_text.text += "\n" + p.historico.doencas[i] + "         " +
                p.historico.remedios[i] + "                " + p.historico.efetividade[i];
            }
        }
    }

    private void ChecaGanhaFase()
    {
        if(objetivoPacientes <= 0)  // se ganhou o dia
        {
            AcabaDia();
        }
    }

    public void ComecaDia()
    {
        blackOutAnim.Play("BlackOut");
        jornalAnim.Play("Jornal_exit");
        ganhouDia = false;
        spawnouFiscal = false;
        dia++;
        nivelRodizio++;
        TimeManager.Singleton.timer.Reset();
        if(nivelRodizio > nivelDoencas)
        {
            nivelRodizio = 0;
        }
        if(dia == metaDiasAumentaNivel) 
        {
            nivelDoencas++;
            metaDiasAumentaNivel += DIAS_PARA_AUMENTO_NIVEL;
        }
        nivelDoencas = Mathf.Clamp(nivelDoencas, 0, 2);
        List<LaudoButtons> buttons = LaudoButtons.BotoesDoencasDesseNivel(nivelDoencas);
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].DesbloqueaBotao();
        }
        foreach (var botao in LaudoButtons.laudoButtons)
        {
            botao.ChecarBotao();
        }
        SpawnPaciente();
        onComecouNovoDia?.Invoke();
    }

    private void ZeraInsatisfacao()  // chamado no fiscal
    {
        nivelInsatisfacaoDia = 0;
    }

    private void AcabaDia()
    {
        dinheiroTotal += acumuloDinheiroDia;
        OnGanharDinheiro?.Invoke(dinheiroTotal, META_DINHEIRO);
        acumuloDinheiroDia = 0;

        if(dinheiroTotal >= META_DINHEIRO) {SceneManager.LoadScene(2);}

        objetivoPacientes = 0;
        Debug.Log("Ganhou o dia!");
        ganhouDia = true;
        objetivoPacientes = (objetivoAnterior + 2); // aumenta a meta
        objetivoPacientes = Mathf.Clamp(objetivoPacientes, QTD_PACIENTES_INICIAIS, pacientesPrefabs.Length);
        objetivoAnterior = objetivoPacientes; // seta um novo anterior
        filaTipoDoenca.Clear(); // limpa a lista de pedidos de doencas, pois o dia acabou
        // (so pra evitar confusao msm)
        if(populacao.IsAbove())
        {
            jornal_text.text = JornalAboveString();
            perdeu = true;
        }
        else if(populacao.IsBelow())
        {
            jornal_text.text = JornaBelowString();
            perdeu = true;
        }
        else
        {
            jornal_text.text = JornalRelatorio();
        }
        p_diaAnterior = populacao.GetPopulacaoInfectadaPercent();
        onTerminouDia?.Invoke();
    }

    private string JornalRelatorio()
    {
        if(p_diaAnterior < populacao.GetPopulacaoInfectadaPercent())
        {
            return "O governo reporta que o numero de pacientes infectados pela praga subiu para "
            + populacao.GetPopulacaoInfectadaPercent() + "%, se continuarmos assim, não teremos chance.";
        }
        else if(p_diaAnterior > populacao.GetPopulacaoInfectadaPercent())
        {
            return "Informamos que de acordo com os dados oficiais, o numero de infectados pela praga desceu para "
            + populacao.GetPopulacaoInfectadaPercent() + "%, com esperança, podemos vencer essa luta!";
        }
        else
        {
            return "Hoje tivemos uma establização rara! A praga nem se espalhou, e nem diminuiu! " +
            "Os números reportados esta manhã, indicam uma quantia de " + populacao.GetPopulacaoInfectadaPercent()
            + "%!";
        }
    }

    private string JornaBelowString()
    {
        return "Euforia! com menos de " + P_MINIMO_INFEC + "% de pessoas infectadas, " +
        "finalmente todos os distritos estão livre de risco contra a praga! O governo informa "
        + "que não precisa mais de seu médico especialista, por isso foi cortado!";
    }

    private string JornalAboveString()
    {
        return "Com mais de " + P_MAXIMO_INFEC + "% de pessoas infectadas" +
        " a situação da praga se agravou demais, por isso o governo finalmente " +
        "tomou a decisão de cortar o médico de seu cargo";
    }

    private void ReativarPaciente(Paciente p)
    {
        p.transform.parent.GetComponent<PacienteAnimHandler>().PlayEnterPaciente();
        p.gameObject.SetActive(true);
        p.SetUltimoDia(dia);
        p.GerarDoenca();
    }

    private int GerarRandomIndexPaciente(int extra = 0)
    {
        UnityEngine.Random.InitState(System.DateTime.Now.Millisecond + extra);
        int randomIndex = UnityEngine.Random.Range(0, pacientesPrefabs.Length);

        return randomIndex;
    }

    public Paciente AcharPacientePorId(int id)
    {
        for (int i = 0; i < pacientesSpawnados.Count; i++)
        {
            if(pacientesSpawnados[i].indexIdentificador == id) {
                return pacientesSpawnados[i];}
        }
        return null;
    }

    public void ActivateReceitaButton()
    {
        if(isTutorial){return;}
        b_gerarReceita.gameObject.SetActive(true);
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void AcabaDiaRepentino()
    {
        if(ganhouDia || isTutorial) {return;}
        AudioManager.Singleton.PlaySound("sfx_alarm");

        pacientesReincidentes.Add(pacienteAtual);
        pacienteAtual.transform.parent.GetComponent<PacienteAnimHandler>().PlayExitPaciente();
        DialogueUI.Singleton.CloseDialogueBox();
        AcabaDia();
    }

    void OnEnable()
    {
        TimeManager.Singleton.timer.OnTimerEnd += AcabaDiaRepentino;
        PacienteAnimHandler.onLeftRoom += SpawnPaciente;
        DialogueUI.onStoppedDialog += ActivateReceitaButton;
        Fiscal.onSaiuFiscal += ZeraInsatisfacao;
        PacienteAnimHandler.onStartedLeavingRoom += TriggaOnPacienteSaiu;
        PacienteAnimHandler.onChegouSalaNormal += TriggaOnPacienteEntrou;
    }

    void OnDisable()
    {
        if(TimeManager.Singleton)
        TimeManager.Singleton.timer.OnTimerEnd -= AcabaDiaRepentino;
        PacienteAnimHandler.onLeftRoom -= SpawnPaciente;
        DialogueUI.onStoppedDialog -= ActivateReceitaButton;
        Fiscal.onSaiuFiscal -= ZeraInsatisfacao;
        PacienteAnimHandler.onStartedLeavingRoom -= TriggaOnPacienteSaiu;
        PacienteAnimHandler.onChegouSalaNormal -= TriggaOnPacienteEntrou;
    }

}
