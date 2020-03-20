using System.Collections.Generic;

public class CardsDescription
{
    private const string orca = "A \"baleia assassina\" é, na verdade, um golfinho. Alimenta-se de focas, pinguins etc.";
    private const string peixe_gelo = "Este peixe vive a mil metros de profundidade. Possui o sangue transparente.";
    private const string baleia_minke = "Esta baleia pode ser vista levantando a cabeça fora d'água.";
    private const string baleia_azul = "Esta é a maior baleia podendo atingir 30m. Atualmente, está em perigo de extinção.";
    private const string baleia_jubarte = "Esta baleia alimenta-se na Antártica no verão e procria em águas tropicais e subtropicais no  inverno.";
    private const string baleia_fin = "Esta é a segunda maior baleia do mundo, podendo atingir 22 m.";
    private const string krill = "É base da cadeia alimentar de muitos animais marinhos do oceano austral.";
    private const string algas = "Grupo de seres vivos capazes de produzir sua própria fonte de alimento.";
    private const string zooplancton = "Pequenos animais que estão passivamente a deriva do movimento da água.";

    private const string foca_caranguejeira = "Esta foca pode atingir até 2 m de comprimento.";
    private const string lobo_marinho = "Este mamífero mergulha para se alimentar a 180 m.";
    private const string pinguim_adelia = "Uma das únicas espécies de pinguim que fazem ninho no continente.";
    private const string pinguim_imperador = "Este pinguim pode chegar a 1,22 m. É capaz de pescar em profundidades de até 250 m.";
    private const string pinguim_antartico = "É conhecido como Pinguim de barbicacho devido à faixa preta ao redor do queixo.";
    private const string pinguim_papua = "Este pinguim pesa em média 6,5 kg, com altura entre 75 a 90 cm.";
    private const string pinguim_rei = "Possui uma mancha dourada na região auricular, e em torno do peito.";
    private const string pinguim_macaroni = "A população deste animal é cerca de 18 milhões de indivíduos.";
    private const string skua = "Esta ave alimenta-se de ovos, pinguins e de outras aves.";

    private static Dictionary<string, string> cardsAudiodescription = new Dictionary<string, string>
    {
        {"orca","Imagem de animal mamífero  com coloração preta na maior parte do corpo e a mancha branca no ventre, ao redor dos olhos e próximo a barbatana." },
        {"peixe gelo","Imagem de peixe mesclado nas cores azul e verde, com manchas amareladas e olho preto." },
        {"baleia minke","Imagem de uma baleia de corpo fino, cabeça estreita e pontiaguda. Sua coloração é cinza-escura no dorso e branca na região da barriga. Possui uma nadadeira dorsal que fica no final do dorso, próximo as nadadeiras traseiras." },
        {"baleia azul","Imagem de uma baleia com coloração azul acinzentada e manchas claras. Possui a cabeça larga, nadadeiras peitorais finas." },
        {"baleia jubarte","Imagem de uma baleia com coloração  na cor marrom com as nadadeiras peitorais esbranquiçadas e nadadeira dorsal pequena, formando apenas uma pequena elevação em seu dorso. No peitoral possui manchas que lembram riscos, levemente esbranquiçados." },
        {"baleia fin","Imagem de uma baleia de corpo longilíneo e aerodinâmico com coloração assimétrica, exibindo um gradiente cinza-claro a partir da região dorsal até as laterais do corpo, caracterizadas por tons mais escuros e amarronzados. A parte inferior de seu corpo, suas nadadeiras e os lóbulos inferiores da nadadeira caudal, são brancos." },
        {"krill","Imagem um crustáceo semelhante ao camarão, possui exoesqueleto transparente, olhos pretos, duas antenas e vários pares de patas." },
        {"algas","Imagem de um organismo semelhante a uma planta,  com coloração verde dentro da água." },
        {"zooplâncton","Imagem de animal com corpo transparente, duas antenas e nadadeiras traseiras." },
        {"foca caranguejeira","Imagem de uma foca com corpo delgado com cabeça e focinho largos,  pelagem coloração clara, com tons de cinza escuro a cinza prateado, com manchas de formatos variados nas laterais do corpo." },
        {"lobo marinho","Imagem de um lobo de corpo delgado, coloração cinza, focinho fino e pontudo, orelhas visíveis." },
        {"pinguim adelia","Imagem de um pinguim com a cabeça, a face, as costas e as nadadeiras pretas, peito e a parte da frente toda branca e bico e pés rosados." },
        {"pinguim imperador","Imagem de um pinguim com plumagem multicolorida: cinza-azulado nas costas, branco no abdômen, preto na cabeça e barbatanas. Esta espécie apresenta também uma faixa alaranjada em torno dos ouvidos." },
        {"pinguim antartico","Imagem de um pinguim com o dorso preto e o peito e abdômen brancos. Possui uma faixa preta em volta do seu queixo, sua íris tem um tom marrom-alaranjado." },
        {"pinguim papua","Imagem de um pinguim com o dorso preto e o peito e abdômen brancos,  mancha branca que lhe percorre a cabeça e pelo bico e pés de um laranja vivo." },
        {"pinguim rei","Imagem de um pinguim com dorso  predominantemente na cor cinza, cabeça preta, orelhas e bico na cor laranja e peito nas cores amarela e branca. " },
        {"pinguim macaroni","Imagem de um pinguim com peito branco, cabeça,  pescoço e dorso pretos. As asas são pretas-azuladas na superfície superior, com uma risca branca na parte posterior, e brancas na superfície inferior. Possui uma crista amarela que cresce a partir da testa e prolonga-se horizontalmente até à nuca. O bico é grande e laranja-acastanhado, íris vermelha. As pernas e patas são cor-de-rosa." },
        {"skua","Imagem de uma ave, de plumagem marrom-escura, peito claro e bico preto." }
    };


    public static string GetCardText(string cardName)
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

    public static string GetCardAudioDescription(string key)
    {
        return cardsAudiodescription[key];
    }
}
