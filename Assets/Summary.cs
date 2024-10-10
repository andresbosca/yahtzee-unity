using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class Summary : MonoBehaviour
{
    public Component Dice1;
    public Component Dice2;
    public Component Dice3;
    public Component Dice4;
    public Component Dice5;

    public Component RerollButton;

    public Text Text;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Dice1.TryGetComponent<GuessDiceSide>(out var diceSide1);
        Dice2.TryGetComponent<GuessDiceSide>(out var diceSide2);
        Dice3.TryGetComponent<GuessDiceSide>(out var diceSide3);
        Dice4.TryGetComponent<GuessDiceSide>(out var diceSide4);
        Dice5.TryGetComponent<GuessDiceSide>(out var diceSide5);
        RerollButton.TryGetComponent<RerollButton>(out var rerollButton);


        if (diceSide1.UpperSide == 0 &&
            diceSide2.UpperSide == 0 &&
            diceSide3.UpperSide == 0 &&
            diceSide4.UpperSide == 0 &&
            diceSide5.UpperSide == 0)
        {
            return;
        }

        var sum = diceSide1.UpperSide +
            diceSide2.UpperSide +
            diceSide3.UpperSide +
            diceSide4.UpperSide +
            diceSide5.UpperSide;

        var dados = new int[] {
            diceSide1.UpperSide,
            diceSide2.UpperSide,
            diceSide3.UpperSide,
            diceSide4.UpperSide,
            diceSide5.UpperSide
        };
        int pontuacaoOnes = CalcularPontuacaoSimples(dados, 1);
        int pontuacaoTwos = CalcularPontuacaoSimples(dados, 2);
        int pontuacaoThrees = CalcularPontuacaoSimples(dados, 3);
        int pontuacaoFours = CalcularPontuacaoSimples(dados, 4);
        int pontuacaoFives = CalcularPontuacaoSimples(dados, 5);
        int pontuacaoSixes = CalcularPontuacaoSimples(dados, 6);
        int pontuacaoThreeOfAKind = CalcularTrincaOuMaior(dados, 3);
        int pontuacaoFourOfAKind = CalcularTrincaOuMaior(dados, 4);
        int pontuacaoFullHouse = CalcularFullHouse(dados);
        int pontuacaoSmallStraight = CalcularSequencia(dados, 4);
        int pontuacaoLargeStraight = CalcularSequencia(dados, 5);
        int pontuacaoYahtzee = CalcularYahtzee(dados);

        var allPoints = new int[]
        {
            pontuacaoOnes,
            pontuacaoTwos,
            pontuacaoThrees,
            pontuacaoFours,
            pontuacaoFives,
            pontuacaoSixes,
            pontuacaoThreeOfAKind,
            pontuacaoFourOfAKind,
            pontuacaoFullHouse,
            pontuacaoSmallStraight,
            pontuacaoLargeStraight,
            pontuacaoYahtzee,
        };

        var maxPoints = allPoints.Max();

        Text.text = "Sum: " + maxPoints;
    }  
    
    // Função para calcular pontuação simples (ex: todos os 1's somados)
    static int CalcularPontuacaoSimples(int[] dados, int numero)
    {
        return dados.Count(d => d == numero) * numero;
    }

    // Função para calcular Trinca ou Quadra
    static int CalcularTrincaOuMaior(int[] dados, int repeticoes)
    {
        foreach (var numero in dados.GroupBy(d => d))
        {
            if (numero.Count() >= repeticoes)
                return dados.Sum();
        }
        return 0;
    }

    // Função para calcular Full House
    static int CalcularFullHouse(int[] dados)
    {
        var agrupados = dados.GroupBy(d => d).OrderByDescending(g => g.Count());
        if (agrupados.First().Count() == 3 && agrupados.Last().Count() == 2)
        {
            return 25; // Full House vale 25 pontos
        }
        return 0;
    }

    // Função para calcular sequência pequena (4 números consecutivos)
    static int CalcularSequencia(int[] dados, int tamanho)
    {
        var sequenciasValidas = new int[][]
        {
            new int[] {1, 2, 3, 4},
            new int[] {2, 3, 4, 5},
            new int[] {3, 4, 5, 6}
        };

        foreach (var seq in sequenciasValidas)
        {
            if (seq.All(s => dados.Contains(s)))
            {
                return tamanho == 4 ? 30 : 40;
            }
        }
        return 0;
    }

    // Função para calcular Yahtzee (5 números iguais)
    static int CalcularYahtzee(int[] dados)
    {
        return dados.Distinct().Count() == 1 ? 50 : 0;
    }
}
