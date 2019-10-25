using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EinsteinCardContent {

    private const string azul1 = "Objeto de estudo: Baleias Jubarte.";
    private const string azul2 = "Objetivo: Fotoidentificação.";
    private const string azul3 = "Materiais: Câmera fotográfica, zoom e catálogo de fotos.";
    private const string azul4 = "Cuidados do cientista cidadão: Não tirar fotos borradas; categorizar corretamente o animal (identificado ou não identificado).";

    private const string laranja1 = "Objeto de estudo: Gelo";
    private const string laranja2 = "Objetivo: Compreender as propriedades físicas dos gelos marinhos, sua interação com a atmosfera e os processos biológicos.";
    private const string laranja3 = "Materiais: Câmera fotográfica; imagens de referência.";
    private const string laranja4 = "Cuidados do cientista cidadão: qualidade das fotos, precisãodas análises, evitar contaminação com metais.";

    private const string roxo1 = "Objeto de estudo: Aves Antárticas";
    private const string roxo2 = "Objetivo: Documentar a distribuição, a abundância, o uso do habitat e as tendências comportamentais desses animais.";
    private const string roxo3 = "Metodologia: Fotografar, identificar a espécie, realizar contagem e registrar a localização.";
    private const string roxo4 = "Cuidados do cientista cidadão: qualidade das fotos, registrar localização e realizar a contagem dos animais no mesmo horário diariamente.";

    private const string verde1 = "Objeto de estudo: Plânctons.";
    private const string verde2 = "Objetivo: Monitorar a abundância desses organismos nos fiordes da península antártica.";
    private const string verde3 = "Materiais: Rede de Coleta, Garrafas, bombas e filtro.";
    private const string verde4 = "Cuidados do cientista cidadão: medir a transparência da água, evitar a queda de objetos no mar.";

    private const string vermelho1 = "Objeto de estudo: Pinguins.";
    private const string vermelho2 = "Objetivo: Entender a dinâmica populacional das espécies, para guiar os esforços de conservação.";
    private const string vermelho3 = "Metodologia: analisar imagens de satélite, identificar e registrar a presença de pinguins.";
    private const string vermelho4 = "Materiais: Computador com acesso a internet; Imagens de referência.";

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

            case "roxo1":
                return roxo1;
            case "roxo2":
                return roxo2;
            case "roxo3":
                return roxo3;
            case "roxo4":
                return roxo4;

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
