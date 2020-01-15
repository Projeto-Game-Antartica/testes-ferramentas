using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EinsteinCardContents {

    public string Azul1 = "Objeto de estudo: Baleias Jubarte.";
    public string Azul2 = "Objetivo: Fotoidentificação.";
    public string Azul3 = "Materiais: Câmera fotográfica, zoom e catálogo de fotos.";
    public string Azul4 = "Cuidados do cientista cidadão: Não tirar fotos borradas; categorizar corretamente o animal (identificado ou não identificado).";

    public string Laranja1 = "Objeto de estudo: Gelo";
    public string Laranja2 = "Objetivo: Compreender as propriedades físicas dos gelos marinhos, sua interação com a atmosfera e os processos biológicos.";
    public string Laranja3 = "Materiais: Câmera fotográfica; imagens de referência.";
    public string Laranja4 = "Cuidados do cientista cidadão: qualidade das fotos, precisãodas análises, evitar contaminação com metais.";

    public string Roxo1 = "Objeto de estudo: Aves Antárticas";
    public string Roxo2 = "Objetivo: Documentar a distribuição, a abundância, o uso do habitat e as tendências comportamentais desses animais.";
    public string Roxo3 = "Metodologia: Fotografar, identificar a espécie, realizar contagem e registrar a localização.";
    public string Roxo4 = "Cuidados do cientista cidadão: qualidade das fotos, registrar localização e realizar a contagem dos animais no mesmo horário diariamente.";

    public string Verde1 = "Objeto de estudo: Plânctons.";
    public string Verde2 = "Objetivo: Monitorar a abundância desses organismos nos fiordes da península antártica.";
    public string Verde3 = "Materiais: Rede de Coleta, Garrafas, bombas e filtro.";
    public string Verde4 = "Cuidados do cientista cidadão: medir a transparência da água, evitar a queda de objetos no mar.";

    public string Vermelho1 = "Objeto de estudo: Pinguins.";
    public string Vermelho2 = "Objetivo: Entender a dinâmica populacional das espécies, para guiar os esforços de conservação.";
    public string Vermelho3 = "Metodologia: analisar imagens de satélite, identificar e registrar a presença de pinguins.";
    public string Vermelho4 = "Materiais: Computador com acesso a internet; Imagens de referência.";

    public string GetText(string cardName)
    {
        switch (cardName)
        {
            case "azul1":
                return Azul1;
            case "azul2":
                return Azul2;
            case "azul3":
                return Azul3;
            case "azul4":
                return Azul4;

            case "laranja1":
                return Laranja1;
            case "laranja2":
                return Laranja2;
            case "laranja3":
                return Laranja3;
            case "laranja4":
                return Laranja4;

            case "roxo1":
                return Roxo1;
            case "roxo2":
                return Roxo2;
            case "roxo3":
                return Roxo3;
            case "roxo4":
                return Roxo4;

            case "verde1":
                return Verde1;
            case "verde2":
                return Verde2;
            case "verde3":
                return Verde3;
            case "verde4":
                return Verde4;

            case "vermelho1":
                return Vermelho1;
            case "vermelho2":
                return Vermelho2;
            case "vermelho3":
                return Vermelho3;
            case "vermelho4":
                return Vermelho4;

            default:
                return "check card name";

        }
    }
}
