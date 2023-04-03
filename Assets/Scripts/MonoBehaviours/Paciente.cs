using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paciente : MonoBehaviour
{
    public Doenca doenca;

    public HistoricoPaciente historico;

    public BioPaciente bio;

    public int retornosConsecutivos {get; private set;}

    private int nivelDoencaAnterior = -1;

    public int indexIdentificador {get; private set;} = -1;

    public int ultimoDiaAtendido {get; private set;} = -1;

    const float sensibilidadePercent = 60f;

    public int dialogueIndex = 0;
    //public static DialogueObject randomPhrase;

    /*public void RandomPhrase()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        int dialogueIndex = Random.Range(0, bio.personality.dialogues.Length);
        randomPhrase = bio.personality.dialogues[dialogueIndex];
    }*/

    public DialogueObject RandomPhrase()
    {
       // Random.InitState(System.DateTime.Now.Millisecond);
      //  dialogueIndex = Random.Range(0, bio.personality.dialogues.Length);
      DialogueObject d = bio.personality.dialogues[dialogueIndex];
      dialogueIndex++;
      if(dialogueIndex > bio.personality.dialogues.Length - 1) 
      {
          dialogueIndex = 0;
      }
     // dialogueIndex = Mathf.Clamp(dialogueIndex, 0, bio.personality.dialogues.Length - 1);
      return d;
    }

    /*public DialogueObject SpecificPhrase()
    {
        return bio.personality.dialogues[dialogueIndex];
    }*/

    public void SetIndexIdentificador(int index)
    {
        indexIdentificador = index;
    }

    public void SetUltimoDia(int dia)
    {
        ultimoDiaAtendido = dia;
    }

    public void SetDoenca(Doenca doenca)
    {
        this.doenca = doenca;
    }

    public Doenca GerarDoenca()
    {
        Doencas[] doencas = LevelManager.Singleton.doencas;
        int indexLevelDoenca = 0;
        if(nivelDoencaAnterior == -1) // se nao estava doente
        {
            Random.InitState(System.DateTime.Now.Millisecond);
            indexLevelDoenca = Random.Range(0, LevelManager.Singleton.nivelDoencas + 1);
            // o index de doenca vai ser de um nivel permitido
        }
        else {indexLevelDoenca = nivelDoencaAnterior;}
        // caso seja reincidente da doenca, voltara com uma de mesmo nivel

        Random.InitState(System.DateTime.Now.Millisecond);
        int index = Random.Range(0, doencas[indexLevelDoenca].doencas.Length);

        doenca = doencas[indexLevelDoenca].doencas[index];
        nivelDoencaAnterior = doenca.getNivelDoenca;
        LevelManager.Singleton.populacao.AddDoentes(2.5f);
        Debug.LogError("Gerando doenca " + doenca + " " + index);
        return doenca;
    }

    public bool TentarCurar(Doenca doencaLaudo, Remedio remedio)
    {
        if(doenca == null) {Debug.LogError("Paciente sem doenca"); return false;}

        LaudoButtons.BotaoComEssaDoenca(doenca).DesbloqueaBotao();

        EfeitosColaterais(remedio); // ja da os efeitos colaterais se houver algum

        bool curou = false;

        if(remedio.getTipoRemedio == Remedio.REMEDIOTYPE.MUTAGENICO || remedio.getTipoRemedio == Remedio.REMEDIOTYPE.PLACEBO)
        {
            Debug.LogError("Remedio com efeito");
        } // se nao e do tipo cura ja vaza

        else if(doencaLaudo != doenca || doenca.getNivelDoenca > remedio.getNivelDoenca)
        {
            Debug.LogError("Laudo Errado ou doenca forte demais");
            // se nao der laudo certo ou der remedio de nivel menor, so vaza
        }

        else if(doenca.getNivelDoenca < remedio.getNivelDoenca)  // se o nivel do remedio
        { // for maior
            Debug.LogError("Remedio forte demais");
            Random.InitState(System.DateTime.Now.Millisecond);
            float randomChance = Random.Range(0, 101);
            if(randomChance <= sensibilidadePercent)
            {
                EvoluirDoenca(remedio); // tem uma chance de evoluir a doenca
            }
            else
            curou = Curar(); // caso nao evolua, curou
            // sensibilidade
        }

        else if(doenca.getNivelDoenca == remedio.getNivelDoenca)
        {
            Debug.LogError("Curou!");
            curou = Curar();
        }

    switch(curou)
    {
        case true:
        LevelManager.Singleton.populacao.SubtractDoentes(8f);
        break;

        case false: // se nao curou
        retornosConsecutivos++; // avisa que vai ficar consecutivamente
        LevelManager.Singleton.pacientesReincidentes.Add(this); // se adiciiona na
        LevelManager.Singleton.populacao.AddDoentes((1.2f) * MultiplicadorClasseReverso());
        // fila pra voltar
        break;
    }
    historico.AddHistorico(doencaLaudo.nome, remedio.nome, curou); // adiciona o ocorrido
    // no historico

        return curou; // retorna se curou ou nao
    }

    private bool Curar()
    {
        int money = doenca.pagamento;
        if(LevelManager.Singleton.isOnRevoltaEvent) 
        {
            money /= 2;
        }
        LevelManager.Singleton.GanharDinheiro(money); // se curou, da dinheiro
        nivelDoencaAnterior = -1; // nao ha nivel na doenca anterior
        retornosConsecutivos = 0; // nao esta mais retornando consecutivamente
        if(LevelManager.Singleton.pacientesReincidentes.Contains(this))
        {
            LevelManager.Singleton.pacientesReincidentes.Remove(this);
        }
        doenca = null; // tira a doenca
        return true;
    }

    int EvoluirDoenca(Remedio remedio)
    {
      //  if(remedio.getTipoRemedio != Remedio.REMEDIOTYPE.MUTAGENICO) {return;}
        // se o remedio dado nao for mutagenico
        nivelDoencaAnterior++; // aumenta o nivel da doenca anterior

        nivelDoencaAnterior = Mathf.Clamp(nivelDoencaAnterior, 0
        , LevelManager.Singleton.doencas.Length - 1); // da a certeza de que
        // o nivel nao passa do maximo
        LevelManager.Singleton.populacao.AddDoentes((0.5f) * MultiplicadorClasseReverso());
        return nivelDoencaAnterior;

        // mais pessoas devem chegar com o nivel equivalente a um a mais que essa
    }

    void EfeitosColaterais(Remedio remedio)
    {
        switch(remedio.getTipoRemedio)
        {
            case Remedio.REMEDIOTYPE.PLACEBO:
                for (int i = 0; i < 2; i++)
                {
                    LevelManager.Singleton.AddDoencaToFila(nivelDoencaAnterior);
                } // mais pessoas devem chegar com o nivel equivalente a essa
            break;
            case Remedio.REMEDIOTYPE.MUTAGENICO:
                EvoluirDoenca(remedio);
                for (int i = 0; i < 2; i++)
                {
                    LevelManager.Singleton.AddDoencaToFila(nivelDoencaAnterior);
                }
            break;
        }
    }

    public int CalcularInSatisfacao() // calcula o quao bem esse paciente esta
    {
        return retornosConsecutivos * MultiplicadorClasse();
    }

    public int MultiplicadorClasseReverso() 
    {
        int multiplicadorClasse = 1;

        switch(bio.ocupacao.classe)
        {
            case Ocupacao.CLASSE.I:
                multiplicadorClasse = 1;
            break;
            case Ocupacao.CLASSE.II:
                multiplicadorClasse = 2;
            break;
            case Ocupacao.CLASSE.III:
                multiplicadorClasse = 3;
            break;
            case Ocupacao.CLASSE.X:
                multiplicadorClasse = 1;
            break;
        }
        return multiplicadorClasse;
    }

    public int MultiplicadorClasse() 
    {
        int multiplicadorClasse = 1;

        switch(bio.ocupacao.classe)
        {
            case Ocupacao.CLASSE.I:
                multiplicadorClasse = 3;
            break;
            case Ocupacao.CLASSE.II:
                multiplicadorClasse = 2;
            break;
            case Ocupacao.CLASSE.III:
                multiplicadorClasse = 1;
            break;
            case Ocupacao.CLASSE.X:
                multiplicadorClasse = 1;
            break;
        }
        return multiplicadorClasse;
    }

}
