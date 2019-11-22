using System.Collections.Generic;

public class CardsDescriptionPaleo
{
    private const string amnoide = "A baleia assassina é, na verdade, um golfinho. Alimenta-se de focas, pinguins etc.";
    private const string antarctopelta = "Este peixe vive a mil metros de profundidade. Possui o sangue transparente.";
    private const string floresta_tropical3 = "Esta baleia pode ser vista levantando a cabeça fora d’água.";
    private const string fossil = "Esta é a maior baleia podendo atingir 30m. Atualmente, está em perigo de extinção.";
    private const string plesiossauro = "Esta baleia alimenta-se na Antártica no verão e procria em águas tropicais e subtropicais no  inverno.";
    private const string pterossauro = "Esta é a segunda maior baleia do mundo, podendo atingir  22 m.";

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
        else
            return null;
    }
}
