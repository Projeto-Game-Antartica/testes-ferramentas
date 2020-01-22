using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EinsteinCardContent {

    private const string azul1 = "Objeto de estudo: Baleias.";
    private const string azul2 = "Metodologia: Captura e análise de fotos.";
    private const string azul3 = "Objetivo: Mapear o comportamento e migração das baleias.";
    private const string azul4 = "Materiais necessários: Câmera fotográfica, zoom e catálogo de fotos.";

    private const string laranja1 = "Objeto de estudo: Paleontologia.";
    private const string laranja2 = "Metodologia: Escavação e identificação de material fossilizado.";
    private const string laranja3 = "Objetivo: Investigar as características geológicas e biológicas do passado.";
    private const string laranja4 = "Materiais necessários: Martelo geológico e pincel.";

    private const string verde1 = "Objeto de estudo: Aves Austrais.";
    private const string verde2 = "Metodologia: Observar e analisar.";
    private const string verde3 = "Objetivo: Compreender a distribuição, a abundância e o uso do habitat.";
    private const string verde4 = "Materiais necessários: Binóculos e catálogo de imagens.";

    private const string vermelho1 = "Objeto de estudo: Vegetação.";
    private const string vermelho2 = "Metodologia: Coletar amostras para análise em laboratório e observação local.";
    private const string vermelho3 = "Objetivo: Caracterizar a diversidade e vegetal e reconhecer seus padrões de distribuição em gradientes ambientais.";
    private const string vermelho4 = "Materiais necessários: Sacos de papel, quadrante, espátula e GPS.";

    public static string GetText(string cardName)
    {
        switch (cardName)
        {
            case "azul1":
                return azul1;
            case "azul2":
                return azul2;
            case "azul3":
                return azul3;
            case "azul4":
                return azul4;

            case "laranja1":
                return laranja1;
            case "laranja2":
                return laranja2;
            case "laranja3":
                return laranja3;
            case "laranja4":
                return laranja4;

            case "verde1":
                return verde1;
            case "verde2":
                return verde2;
            case "verde3":
                return verde3;
            case "verde4":
                return verde4;

            case "vermelho1":
                return vermelho1;
            case "vermelho2":
                return vermelho2;
            case "vermelho3":
                return vermelho3;
            case "vermelho4":
                return vermelho4;

            default:
                return "check card name";

        }
    }
}
