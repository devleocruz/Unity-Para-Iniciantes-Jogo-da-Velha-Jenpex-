using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class JogoDaVelhaBackEnd : MonoBehaviour
{
    bool jogador1 = true; // true = X, false = O
    int[,] matrizJogoDaVelha = new int[3, 3]; // 0 = vazio, 1 = X, 2 = O
    bool estadoJogo = false;
    int turnos = 0;
    int resultado = 0;

    [SerializeField] TextMeshProUGUI textoVezJogador;
    [SerializeField] GameObject painel;
    [SerializeField] GameObject botaoResetar;
    [SerializeField] GameObject botaoResetarVitoria;
    [SerializeField] Sprite imagemX;
    [SerializeField] Sprite imagemO;

    public void Selecao(GameObject botaoClicado)

    {
        if (estadoJogo)
        {
            return;
        }

        if (botaoClicado.GetComponent<Image>().sprite == imagemX || botaoClicado.GetComponent<Image>().sprite == imagemO)
        {
            return; // Impede que o jogador escolha uma posição já ocupada
        }

        string[] posicoes = Regex.Split(botaoClicado.name, ",");
        if (jogador1)
        {
            matrizJogoDaVelha[int.Parse(posicoes[0]), int.Parse(posicoes[1])] = 1; // Marca a posição na matriz como ocupada pelo jogador 1 (X)
            botaoClicado.GetComponent<Image>().sprite = imagemX;
            jogador1 = false;
        }
        else
        {
            matrizJogoDaVelha[int.Parse(posicoes[0]), int.Parse(posicoes[1])] = 2; // Marca a posição na matriz como ocupada pelo jogador 2 (O)
            botaoClicado.GetComponent<Image>().sprite = imagemO;
            jogador1 = true;
        }

        for (int i = 0; i < matrizJogoDaVelha.GetLength(0); i++)
        {
            print(matrizJogoDaVelha[i, 0] + " " + matrizJogoDaVelha[i, 1] + " " + matrizJogoDaVelha[i, 2]);
        }
        resultado = VerificarVencedor();
        if (resultado != 0 || turnos == 9)
        {
            estadoJogo = true;
            Resetar();
        } 
    }

    int VerificarVencedor()
    {
        // Verifica linhas e colunas
        for (int i = 0; i < 3; i++)
        {
            if (matrizJogoDaVelha[i, 0] != 0 && matrizJogoDaVelha[i, 0] == matrizJogoDaVelha[i, 1] && matrizJogoDaVelha[i, 1] == matrizJogoDaVelha[i, 2])
            {
                return matrizJogoDaVelha[i, 0]; // Retorna o jogador vencedor (1 ou 2)
            }
            if (matrizJogoDaVelha[0, i] != 0 && matrizJogoDaVelha[0, i] == matrizJogoDaVelha[1, i] && matrizJogoDaVelha[1, i] == matrizJogoDaVelha[2, i])
            {
                return matrizJogoDaVelha[0, i]; // Retorna o jogador vencedor (1 ou 2)
            }
        }
        // Verifica diagonais
        if (matrizJogoDaVelha[0, 0] != 0 && matrizJogoDaVelha[0, 0] == matrizJogoDaVelha[1, 1] && matrizJogoDaVelha[1, 1] == matrizJogoDaVelha[2, 2])
        {
            return matrizJogoDaVelha[0, 0]; // Retorna o jogador vencedor (1 ou 2)
        }
        if (matrizJogoDaVelha[0, 2] != 0 && matrizJogoDaVelha[0, 2] == matrizJogoDaVelha[1, 1] && matrizJogoDaVelha[1, 1] == matrizJogoDaVelha[2, 0])
        {
            return matrizJogoDaVelha[0, 2]; // Retorna o jogador vencedor (1 ou 2)
        }turnos++;
        return 0; // Nenhum vencedor ainda
    }

    public void ReiniciarJogo()
    {
        matrizJogoDaVelha = new int[3, 3];
        jogador1 = true;
        estadoJogo = false;
        turnos = 0;
        Resetar();
        for (int i = 0; i < painel.transform.childCount; i++)
        {
            painel.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>().sprite = null;
        }
    }

    void Resetar()
    {
        if (resultado == 0)
        {
            botaoResetar.SetActive(estadoJogo);
            return;
        }
        botaoResetarVitoria.SetActive(estadoJogo);
        textoVezJogador.text = "Jogador '" + (resultado == 1 ? "X" : "O") + "' venceu!";
    }
}