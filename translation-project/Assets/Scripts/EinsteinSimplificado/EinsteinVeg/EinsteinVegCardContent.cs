using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// must create images with the names on it

public class EinsteinVegCardContent {

    private const string azul1 = "Representantes: musgos";
    private const string azul2 = "Estrutura: caulóide";
    private const string azul3 = "Fixação/ Movimentação: rizóides";
    private const string azul4 = "Tamanho: de 1 cm até 5 cm";

    private const string laranja1 = "Representantes: mapa";
    private const string laranja2 = "Estrutura: talo e hífas";
    private const string laranja3 = "Fixação/ Movimentação: apressório e hífas";
    private const string laranja4 = "Tamanho: microscópico até alguns cm";

    private const string roxo1 = "Representantes: degelo";
    private const string roxo2 = "Estrutura: uni. - a própria célula multi. - talo";
    private const string roxo3 = "Fixação/ Movimentação: uni. - flagelos multi. - apressório";
    private const string roxo4 = "Tamanho: microscópico até 60 m";

    private const string verde1 = "Representantes: erva-pilosa-antártica";
    private const string verde2 = "Estrutura: caule";
    private const string verde3 = "Fixação/ Movimentação: raíz";
    private const string verde4 = "Tamanho: de 1 mm até 100 m";

    private const string vermelho1 = "Briófitas";
    private const string vermelho2 = "Líquens";
    private const string vermelho3 = "Algas";
    private const string vermelho4 = "Angiospermas";

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
