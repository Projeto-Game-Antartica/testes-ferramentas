using System.Collections.Generic;

public class CardsDescription
{
    private const string orca = "A baleia assassina é, na verdade, um golfinho. Alimenta-se de focas, pinguins etc.";
    private const string peixe_gelo = "Este peixe vive a mil metros de profundidade. Possui o sangue transparente.";
    private const string baleia_minke = "Esta baleia pode ser vista levantando a cabeça fora d’água.";
    private const string baleia_azul = "Esta é a maior baleia podendo atingir 30m. Atualmente, está em perigo de extinção.";
    private const string baleia_jubarte = "Esta baleia alimenta-se na Antártica no verão e procria em águas tropicais e subtropicais no  inverno.";
    private const string baleia_fin = "Esta é a segunda maior baleia do mundo, podendo atingir  22 m.";
    private const string krill = "Alimento para muitos  animais antárticos, contribui para a produção de oxigênio.";
    private const string algas = "Grupo de seres vivos capaz de produzir seu próprio oxigênio";
    private const string zooplancton = "Pequenos animais sem capacidade de vencer a coluna d'água para se mover.";

    private const string foca_caranguejeira = "Esta foca pode atingir até 2 m de comprimento.";
    private const string lobo_marinho = "Este mamífero mergulha para se alimentar a 180 m.";
    private const string pinguim_adelia = "Uma das únicas espécies de pinguim que fazem ninho no continente.";
    private const string pinguim_imperador = "Este pinguim pode chegar a 1,22 m. É capaz de pescar em profundidades de até 250 m.";
    private const string pinguim_antartico = "É conhecido como Pinguim de barbicacho devido à faixa preta ao redor do queixo.";
    private const string pinguim_papua = "Este pinguim pesa em média 6,5 kg, com altura entre 75 a 90 cm.";
    private const string pinguim_rei = "Possui uma mancha dourada na região auricular, e em torno do peito.";
    private const string pinguim_macaroni = "A população deste animal é cerca de 18 milhões de indivíduos.";
    private const string skua = "Esta ave alimenta-se de ovos, pinguins e de outras aves.";


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
        else if (cardName.Contains("orcaText"))
            return orca;
        else if (cardName.Contains("foca_caranguejeiraText"))
            return foca_caranguejeira;
        else if (cardName.Contains("lobo_marinhoText"))
            return lobo_marinho;
        else if (cardName.Contains("pinguim_adeliaText"))
            return pinguim_adelia;
        else if (cardName.Contains("pinguim_imperadorText"))
            return pinguim_imperador;
        else if (cardName.Contains("pinguim_antarticoText"))
            return pinguim_antartico;
        else if (cardName.Contains("pinguim_papuaText"))
            return pinguim_papua;
        else if (cardName.Contains("pinguim_reiText"))
            return pinguim_rei;
        else if (cardName.Contains("pinguim_macaroniText"))
            return pinguim_macaroni;
        else if (cardName.Contains("skuaText"))
            return skua;
        else
            return null;
    }
}
