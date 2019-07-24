using System.Collections.Generic;

public class CardsDescription
{
    private const string orca = "Conhecida como \"baleia assassina\", é na verdade um golfinho que se alimenta de focas, polvos e peixes.";
    private const string peixe_gelo = "É um animal  que vive a mil metros de profundidade, possui o sangue transparente como água e é capaz de viver.";
    private const string baleia_minke = "Esta baleia pode ser vista levantando a cabeça verticalmente para fora d’água, no entanto é incomum levantar a cauda para fora d’água quando submerge";
    private const string baleia_azul = "A maior baleia que se tem conhecimento, podendo atingir 30m. Atualmente está em perigo de extinção.";
    private const string baleia_jubarte = "É encontrada em  praticamente todo o Hemisfério Sul. Passa o verão se alimentando na Antártida e se dirige para águas tropicais e subtropicais durante o inverno para procriar.";
    private const string baleia_fin = "Considerada a segunda maior baleia do mundo, atinge 22 m, com peso médio de 45 toneladas e pode de viver mais de 100 anos.";
    private const string krill = "Pequeno animal similar ao camarão. É alimento para grande parte dos animais antárticos.";
    private const string algas = "Grupo de seres vivos capaz de produzir seu próprio oxigênio";
    private const string zooplancton = "Pequenos animais sem capacidade de vencer a coluna d'água para se mover.";

    public static string GetCardDescription(string cardName)
    {
        if (cardName.Contains("orcaText"))
            return orca;
        else if (cardName.Contains("peixe_geloText"))
            return peixe_gelo;
        else if (cardName.Contains("baleia_minkeText"))
            return baleia_minke;
        else if (cardName.Contains("baleia_azulText"))
            return baleia_azul;
        else if (cardName.Contains("baleia_jubarteText"))
            return baleia_jubarte;
        else if (cardName.Contains("baleia_finText"))
            return baleia_fin;
        else if (cardName.Contains("krillText"))
            return krill;
        else if (cardName.Contains("algasText"))
            return algas;
        else if (cardName.Contains("zooplanctonText"))
            return zooplancton;
        else
            return null;
    }
}
