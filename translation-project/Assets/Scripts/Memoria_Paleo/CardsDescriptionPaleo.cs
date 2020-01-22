using System.Collections.Generic;

public class CardsDescriptionPaleo
{
    private const string amnoide = "Fóssil de animal marinho. Pareciam os moluscos.";
    private const string antarctopelta = "Primeiro dinossauro descoberto na Antártica.";
    private const string floresta_tropical3 = "Abundante fauna e flora. Árvores com até 4 metros de comprimento e alguns animais com 5 metros de comprimento.";
    private const string fossil = "Objeto de estudo da Paleontologia.";
    private const string plesiossauro = "Réptil aquático confundido com dinossauro.";
    private const string pterossauro = "Réptil voador confundido com dinossauro.";
    private const string artefatos = "São da história humana, é cultura.";
    private const string ceramicas = "Seres humanos produziram.";
    private const string trinisaura = "Dinossauros, possuíam quatro patas.";


    public static string GetCardDescriptionPaleo(string cardName)
    {
        if (cardName.Contains("amnoideText"))
            return amnoide;
        else if (cardName.Contains("antarctopeltaText"))
            return antarctopelta;
        else if (cardName.Contains("floresta_tropical3Text"))
            return floresta_tropical3;
        else if (cardName.Contains("fossilText"))
            return fossil;
        else if (cardName.Contains("plesiossauroText"))
            return plesiossauro;
        else if (cardName.Contains("pterossauroText"))
            return pterossauro;
        else if (cardName.Contains("artefatosText"))
            return artefatos;
        else if (cardName.Contains("ceramicasText"))
            return ceramicas;
        else if (cardName.Contains("trinisauraText"))
            return trinisaura;
        else
            return null;
    }
}
