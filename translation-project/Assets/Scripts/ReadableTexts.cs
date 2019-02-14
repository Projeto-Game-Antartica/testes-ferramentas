using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Class to encapsulate all readable (by screenreader) texts that will be used in the game.
 * format: mission_textname
 */
public class ReadableTexts {

    public static string languagemenu_instructions = "Selecione o idioma do jogo. Há dois botões, o primeiro com a bandeira do Brasil" +
                                    "referindo-se ao idioma portugûes e o segundo uma bandeira do Reino Unido" +
                                    "referindo-se ao idioma inglês. Utilize as setas para cima ou baixo ou a tecla TAB" +
                                    "para navegar pelos botões. Utilize a tecla ENTER para selecioná-los.";

    public static string mainmenu_instructions = "Menu principal do jogo. Utilize as setas cima ou baixo ou a tecla TAB para navegação" +
                                    "a tecla enter para selecionar os itens.";

    public static string optionmenu_instructions = "Menu de opções do jogo. Utilize as setas cima ou baixo ou a tecla TAB para navegação" +
                                    "as teclas direita ou esquerda para mudança de opções a tecla enter para selecionar os itens.";
    
    public static string playmenu_instructions = "Escolhas de minigames do jogo. Utilize as setas cima ou baixo ou a tecla TAB para navegação" +
                                    "a tecla enter para selecionar os itens.";

    public static string glossary_instructions = "Glossário em Português-Brasil, Libras e de sons.Para repetir as instruções, aperte a tecla F1 a qualquer momento. " +
                                    "As letras estão separadas em botões onde há duas linhas contendo treze letras em ordem alfabética. Ao selecionar a letra, " +
                                    "palavras iniciando com essa letra irão aparecer em forma de botões.Inicialmente, estará selecionado o primeiro item da lista " +
                                    "de palavras, navegue utilizando as setas para cima ou para baixo. Ao selecionar uma palavra, será descrito seu significado, " +
                                    "aperte ENTER para ouvir o som característico, BARRA DE ESPAÇO para pausar e aperte a tecla F3 para ouvir a descrição novamente. " +
                                    "Para selecionar o alfabeto aperte a tecla F2 e navegue utilizando a tecla TAB ou as setas direcionais para direita ou esquerda. " +
                                    "Pressione a tecla ESC para selecionar o botão de retornar e após, aperte ENTER para sair.";

    public static string foto_instructions = "Para repetir as instruções, aperte a tecla F1. Para abrir o painel de instruções, aperte ESC. Ao final das instruções, " +
                                    "dois botões poderão ser selecionados, um para jogar e outro para sair. Utilize as setas para direita e esquerda para navegar " +
                                    "por eles e ENTER para selecionar. A missão constitui de tirar fotos de duas baleias e catalogá-las no projeto ciência cidadã. " +
                                    "Utilize as teclas direcionais(setas para cima, baixo, direita e esquerda) para navegar pelo cenário e encontrar as baleias." +
                                    "Para ativar a câmera, aperte a tecla F, use as teclas MAIS e MENOS do teclado numérico para dar zoom e, para fotografar, quando a câmera " +
                                    "estiver ativa, utilize a BARRA DE ESPAÇO.O jogo iniciará assim que selecionar o botão JOGAR.";

    public static string foto_sceneDescription = "Você está localizado dentro de um navio olhando na direção do mar. As teclas direcionais movimentam como " +
                                    "a direção do seu olhar, ou seja, você está movimentando o rosto para cima, baixo, direita ou esquerda. Ao fundo do oceano," +
                                    "na parte superior, aparece o sol e um céu azul com poucas nuvens. Caso olhe para direita, encontrará, ao fundo, montanhas  " +
                                    "cobertas de gelo. Caso olhe para baixo, aparece a borda do navio, composta por uma parte de vidro e um apoio de madeira. " +
                                    "Olhando para cima, encontrará gaivotas voando pelo céu. As baleias se encontram navegando pelo oceano onde, ocasionalmente, " +
                                    "mostrarão suas caudas, emitindo um som característico.";

    public static string foto_catalogDescription = "Painel de catalogo das baleias. Pressione a tecla F1 para repetir as instruções. Neste painel aparece a foto da baleia que" +
                                    "foi tirada agora, a data e hora, a organização e a sua localização com latitude, longitude e também um mapa." +
                                    "Pressione a tecla ESC para fechar o catálogo e voltar ao jogo para tirar novas fotos.";

    public static string quiz_instructions = "Inicio do Jogo. Utilize as setas cima ou baixo ou a tecla TAB" +
                                    "para navegar entre as opções de resposta e a tecla ENTER para selecioná-las.";
}
