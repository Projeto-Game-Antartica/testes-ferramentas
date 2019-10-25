using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegrasText {


    private readonly static string[] regras = 
        {
        "Nunca jogue nada no mar.",
        "Quando estiver visitando o continente, não deixe nenhuma sujeira, nem leve \"lembrancinhas\".",
        "Quando próximo à vida selvagem, caminhe lentamente e com cuidado, faça o máximo de silêncio possível e mantenha distância apropriada dos animais (mínimo de 5 metros).",
        "Se observar mudança no comportamento de algum animal é sinal de que você está muito perto!",
        "Jamais toque nos animais.",
        "Sempre dê aos animais a prioridade de passagem e não bloqueie suas rotas de acesso ao mar.",
        "Lembre-se de que os animais são os habitantes enquanto nós somos visitantes. Tenha cuidado!",
        "Evite caminhar em qualquer vegetação, incluindo musgos e líquens, pois são muito frágeis e  crescem muito lentamente.",
        "A fim de impedir a introdução de espécies não-nativas  e doenças, você deverá lavar cuidadosamente as suas botas e limpar todos os equipamentos antes de trazê-los para a Antártica.",
        "Você deverá lavar sua bota de borracha antes de retornar ao barco.",
        "Não perca suas espigas de milho.",
        "Não caminhe sobre as trilhas dos pinguins.",
        "Não ande em geleiras ou largos campos de neve sem experiência e o equipamento apropriado. Há um risco real de cair em fendas escondidas.",
        "Não alimente os animais nem deixe restos de comida ao redor.",
        "Não remova nem toque em equipamentos científicos ou marcadores, nem toque neles.",
        "Não atrapalhe o trabalho dos cientistas nos laboratórios ou na atividade de campo.",
        "Os animais ficam sensíveis durante a época do acasalamento até o nascimento dos filhotes. Não ultrapasse a margem das colônias e observe-as a distância.",
        "Somente entre nos refúgios de emergência quando necessário. Se você precisar pegar um equipamento ou alimento do refúgio, avise a autoridade mais próxima quando a emergência acabar.",
        "Se possível, coloque tendas e equipamentos na neve ou em áreas já usadas.",
        "Cuidado para não pisar em nenhum objeto importante ou fóssil quando andar por sítios arqueológicos ou paleontológicos.",
        "Se você encontrar um objeto histórico ou fóssil que pode não ter sido encontrado pela equipe de pesquisadores, não pegue o item encontrado. Avise o líder da expedição.",
        "Você deve levar uma garrafa de alumínio para captar água para beber.",
        "Tome cuidado para não perder a touca ou as luvas.",
        "Tome cuidado para não deixar cair nem  esquecer peças do seu equipamento fotográfico.",
        "Sempre deixe uma placa marcando sua localização.",
        "Caso entre em um refúgio ou uma estação de pesquisa, deixe sua assinatura com data na parede interna.",
        "Arme seu guarda sol, apenas em locais planos.",
        "Catalogue as pedras que trouxer com você, registrando latitude e longitude de onde foi retirada.",
        "Caso necessite tocar em algum animal, sempre tire suas luvas.",
        "Antes de tocar os animais, lave as mãos no mar ou em água de degelo, para eliminar qualquer impureza."
        };

    public static string GetRegra(int index)
    {
        return regras[index];
    }
}
