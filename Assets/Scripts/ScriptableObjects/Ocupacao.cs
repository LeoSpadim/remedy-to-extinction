using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Nova Ocupacao", menuName = "Objects/Ocupacao")]
public class Ocupacao : ScriptableObject
{
    public string nome;
    public enum CLASSE
    {
        I,
        II,
        III,
        X
    }

    public CLASSE classe;
}
