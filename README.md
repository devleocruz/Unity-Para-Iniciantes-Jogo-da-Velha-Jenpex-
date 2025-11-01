# Unity-Para-Iniciantes-Jogo-da-Velha-Jenpex

Um jogo da velha (tic-tac-toe) feito em Unity como projeto de estudo.  
O objetivo é praticar lógica de jogo e interface simples em C#.

<p align="center">
  <img src="https://github.com/devleocruz/Unity-Para-Iniciantes-Jogo-da-Velha-Jenpex-/blob/main/Hierarquia/Tela%20Inicial.png" alt="1020px" width="720px"> <br/>
</p>

## 🕹 Tecnologias / Stack

- Unity (versão usada: 6.2)
- C#
- Canva
- TextMesh Pro (UI)

## Funcionalidades
- Tabuleiro 3x3 clicável
- Dois jogadores locais (X e O)
- Verificação de vitória ou empate
- Reiniciar partida sem reiniciar o jogo inteiro  
- Bloqueia jogadas depois que alguém vence  
- Mostra mensagem de "X venceu", "O venceu", "Empate"

## 🗂 Estrutura do Projeto

Principais pastas em `Assets/`:

### `Scenes/`
- `SampleScene` → Cena principal do jogo.

### `Scripts/`
- `JogoDaVelhaBackEnd.cs` → Lógica principal do jogo (tabuleiro, turno, vitória e regras)

### `Imagens/`
- Arte e sprites usados na interface.

### `TextMesh Pro/`
- Fontes e materiais de texto usados na UI.

---

### Objetos principais na cena (`Hierarchy`)

- `Main Camera`
- `Directional Light`
- `Global Volume`
- `FrontEnd`
- `EventSystem`
- `Scripts` → GameObject que contém os componentes de lógica do jogo

### FrontEnd
<p align="center">
 <img src="https://github.com/devleocruz/Unity-Para-Iniciantes-Jogo-da-Velha-Jenpex-/blob/main/Hierarquia/Hierarquia.png" alt="162px" width="368,74px"><br/>
</p>

## Script C#
### Bibliotecas 
```csharp
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
```
- `UnityEngine` e `UnityEngine.UI` → acesso ao Unity, GameObjects, componentes de UI (Image, etc).
- `TMPro`→ TextMesh Pro para escrever textos bonitos na tela.
- `System.Text.RegularExpressions` → usado pra separar o nome do botão e descobrir qual célula foi clicada usando Regex.

### Variáveis de estado do jogo
```csharp
public class JogoDaVelhaBackEnd : MonoBehaviour
{
    bool jogador1 = true; // true = X, false = O
    int[,] matrizJogoDaVelha = new int[3, 3]; // 0 = vazio, 1 = X, 2 = O
    bool estadoJogo = false;
    int turnos = 0;
    int resultado = 0;
```
- `jogador1`: guarda de quem é a vez. `true` significa jogador X, `false` jogador O.
- `matrizJogoDaVelha`: é o tabuleiro 3x3.
  - 0 = casa vazia
  - 1 = X
  - 2 = O
- `estadoJogo`: se `true`, o jogo acabou (alguém ganhou ou deu empate) e ninguém pode mais jogar.
- `turnos`: conta quantas jogadas já foram feitas (usado pra detectar empate quando chega em 9).
- `resultado`: 0 = ninguém venceu ainda, 1 = X venceu, 2 = O venceu.

### Referências ligadas pelo Inspector
  ```csharp
    [SerializeField] TextMeshProUGUI textoVezJogador;
    [SerializeField] GameObject painel;
    [SerializeField] GameObject botaoResetar;
    [SerializeField] GameObject botaoResetarVitoria;
    [SerializeField] Sprite imagemX;
    [SerializeField] Sprite imagemO;
  ```
Esses campos aparecem no Inspector da Unity porque têm `[SerializeField]`.
Eles conectam o script com a interface do jogo sem precisar ser `public`.
- `textoVezJogador`: onde você mostra mensagens tipo "Jogador X venceu!"
- `painel`: objeto que contém o grid (os botões do tabuleiro).
- `botaoResetar`: botão de reset normal (empate).
- `botaoResetarVitoria`: botão de reset quando tem vencedor (pode ser outro layout).
- `imagemX` e `imagemO`: sprites usados para desenhar X e O na célula clicada.
  
### Método `Selecao(GameObject botaoClicado)`
```csharp
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
```
O que acontece aqui:

- Esse método é chamado quando o jogador clica num botão da grade.
- Se `estadoJogo` já é `true`, significa que o jogo acabou → ele sai e ignora clique.
- Ele checa se o botão já tem imagem X ou O. Se já tiver, não deixa sobrescrever.
- Ele lê o nome do botão (ex: `"1,2"`) e separa com `Regex.Split` pra descobrir qual posição do tabuleiro foi clicada.
- Se for a vez do jogador1 → marca `1` na matriz e coloca sprite do X.
- Se for o outro → marca `2` e coloca sprite do O.
- Faz um `print()`` do tabuleiro no console (debug).
- Chama `VerificarVencedor()` pra ver se alguém ganhou.
- Se alguém ganhou (`resultado != 0`) ou se já deu 9 turnos (`turnos == 9`), o jogo trava (`estadoJogo = true`) e chama `Resetar()` pra mostrar as opções de reiniciar.
- Repara no detalhe: você alterna `jogador1 = !jogador1;` na mão, trocando a vez.
  
### Método `VerificarVencedor()`
```csharp

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
```
Aqui ele checa todas as possibilidades de vitória:
- 3 em uma linha
- 3 em uma coluna
- 3 em diagonal

Se encontrar, retorna `1` (X) ou `2` (O).
Se não encontrou vencedor, aumenta `turnos` e retorna `0`.

Esse retorno é guardado em `resultado`.

### Método `ReiniciarJogo()`
```csharp
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
```
Isso limpa tudo para começar outra partida:
- Zera a matriz.
- Reseta pra jogador X começar.
- Destrava o jogo (`estadoJogo = false`).
- Zera contador de turnos.
- Limpa os sprites de todas as células do tabuleiro, colocando `null` (vazio visualmente).

### Método `Resetar()`
```csharp
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
```
Esse método controla a UI pós-jogo:
- Se `resultado == 0` → ninguém ganhou, foi empate. Mostra o botão de “reset normal”.
- Se `resultado == 1` ou `2` → alguém ganhou. Mostra o botão de “reset vitória” e escreve na tela quem venceu.
