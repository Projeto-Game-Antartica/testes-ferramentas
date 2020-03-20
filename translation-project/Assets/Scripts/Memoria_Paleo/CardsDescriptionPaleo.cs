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

    private static Dictionary<string, string> cardsAudiodescription = new Dictionary<string, string>
    {
        {"amnóide","Imagem de uma animal que está no fundo do mar, tem aparência de um molusco, coloração marrom claro e detalhes em marrom escuro, possui longos tentáculos em frente aos olhos." },
        {"antarctopelta","Imagem de dinossauro, de pé em uma árvore.  Possui coloração marrom, dois espinhos sobre os ombros, com pequenas, cauda longa e pequenas placas brancas cobrindo as costas." },
        {"floresta da antártica","Imagem de floresta com árvores muito altas, vegetação abundante, pequena trilha com pedras cinzas no caminho." },
        {"fóssil","Imagem de um fóssil de um peixe." },
        {"plesiossauro","Imagem de réptil que está no fundo do mar atacando um peixe. Possui coloração acinzentada, cabeça pequena e estreita, pescoço fino, comprido e corpo amplo." },
        {"pterossauro","Imagem de réptil voador, conhecido como Pterossauro. Coloração marrom acinzentada, comprido bico, com uma única crista em cima da cabeça e longas asas." },
        {"artefatos","Imagem de um é um sarcófago e da estátua Bastet, um no lado do outro." },
        {"cerâmicas","Imagem de vaso de cerâmica marrom com desenhos." },
        {"trinisaura","Imagem de um animal na cor marrom, com quatro patas e focinho em forma de bico." }
    };


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

    public static string GetCardAudioDescription(string key)
    {
        return cardsAudiodescription[key];
    }
}
