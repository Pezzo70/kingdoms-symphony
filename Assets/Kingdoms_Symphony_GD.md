#### Informações Símbolos

-   @ . . . Verificar a arte e o layout necessário.
-   ! . . . Anotação importante

### Menu

O menu irá conter quatro botões (@) : Play (Jogar), Scrolls (Pergaminhos), Tips (Dicas) e Options (Configuração) - é necessário que ao ocorrer o _hover_ o botão fique com a opacidade máxima e aumente um pouco de tamanho, logo, a opacidade de todos os botões deve estar abaixo da máxima ao carregar a cena (aumentando somente se ocorrer o _hover_ daquele botão em específico).

-   O botão de Play irá carregar a UI de seleção de personagens que deverá conter as personagens jogáveis dispostas com os banners, barra de experiência e nível atua (@). Após clicar no banner, iniciar o _loading_ de uma nova _run_.
    -   Comportamentos esperados: ao passar o mouse por cima (_hover_) do banner, este deve aumentar um pouco de tamanho; o background do menu deverá estar com _blur_ nesta tela.
-   O botão de Scrolls deverá carregar uma lista de todos os pergaminhos (@) desbloqueados, os bloqueados deverão aparecer com uma _tint_ escura em sua cor base e não podem ser clicados.
    -   Comportamentos esperados: _hover_ altera a opacidade e tamanho (assim como em outros elementos anteriores); background do menu deverá estar com _blur_; ao clicar no pergaminho, deverá abrir o _scroll_ completo (@) com as informações deste (é o mesmo que aparece _in game_).
-   O botão de Tips deverá carregar uma lista de dicas que irão sendo desbloqueadas conforme o jogador desbloqueia (poderá se desbloqueada através de eventos específicos ou por nível do personagem). A dica é divida entre dicas contras montros e dicas de teoria musical e composição.
    -   Comportamento esperado: repetir comportamento do _hover_ existente no conteúdo do botão de scrolls; as dicas que irão ser listadas são somente aquelas que foram desbloqueadas, não há necessidade de listar as dicas não desbloqueadas como fazemos com os _scrolls_; as dicas de monstros possuem layout diferente das de teoria musical (@).
-   O botão de Options é basicamente um menu com alguns botões (@) para ajustar o modo de tela, tamanho da resolução, volume da música, ambientação e VFX, trocar o idioma e mostrar os controles.
    ( ! ) Todos os botões compartilham do mesmo comportamento de opacidade e aumento de tamanho após _hover_, seria interessante criar uma classe base "botão" ou um _script_ para atribuição deste comportamento em todos os botões.
    ( ! ) Mais adiante, irei falar um pouco sobre como funcional os _scrolls_ e as dicas, mas é importante deixar claro que há necessidade do _designer_ de criar novas dicas ou novos _scrolls_ ou alterar os já existentes através do inspetor.
    ( ! ) Adicionar sistema de idioma editável sem ser por código para o conteúdo escrito do jogo.

### Player

A personagem terá as seguintes características:

-   Nome;
-   Instrumento;
-   Coleção de _scrolls_ (desbloqueadas conforme o nível da personagem);
-   Como atributos que evoluem de acordo com o nível da personagem:
    -   Moral: corresponde à "vida" da personagem, caso seja zerada, o _player_ perde a _run_ atual.
    -   Mana: são cristais que são gastos ao utilizar _scrolls_ - de acordo com o preço do "cast" do mesmo.
    -   Compassos: quantidade de compassos que podem ser criados no sistema de partitura.
-   Em relação a HUD, teremos algo similar aquilo que é apresentado em "South Park: The Stick of Truth", uma _portrait_ no canto esquerdo superior, com a barra de moral e os cristais de mana, além disso, ao ser o turno do jogador (irei falar sobre o combate mais adiante), deverá aparecer os botões ao lado da personagem do jogador - o botão de _scrolls_, para listar todas _scrolls_ e o botão de partitura (@). Após todas as ações, o jogador pode clicar no seu personagem para dar "play" e executar seu turno... todos os efeitos são aplicados à todos os monstros em cena - uma música não pode ser direcionada à apenas um inimigo, não faz muito sentido.
    -   Comportamentos esperados: após seleção dos _scrolls_, deverá aparecer o nome dos _scrolls_ selecionados ao lado esquerdo da tela (ao realizar o _hover_ neste texto, deve aparecer a descrição completa deste).

#### Scrolls

Os _scrolls_ são mini missões que podem ser selecionadas ao custo de mana que ao serem realizadas aplicam novos efeitos em ataques ou executar comportamentos de defesa como cura, invunerabilidade, etc. O jogador poderá abrir a listagem de _scrolls_ disponíveis (@) e inspecionar (ao clicar com o botão esquerdo do mouse) para ver mais detalhes (@), caso queira selecionar de fato basta segurar o botão esquerdo do mouse por "X" segundos e irá gastar a mana para utilizar o pergaminho. Vale ressaltar que alguns pergaminhos podem ser utilizados mais de uma vez. Características:

-   Nome;
-   Descrição e Efeito;
-   Custo.

#### Partitura

Ao selecionar para abrir a partitura, irá ser carregado na porção inferior e central da tela, a partitura interativa, nela, serão necessários alguns comportamentos:

-   Com o _mouse wheel_ deverá trocar a notação atual que será disposta na partitura ao clicar na linha - verificar se vale a pena adicionar os números do teclado para mudança do tempo;
-   Ao clicar na clave, deverá trocar de clave (terá apenas clave de Sol e Fá);
-   Cada uma das linhas da partitura deverá contabilizar o tempo - importante ressaltar que inicialmente tudo será 4/4, logo em uma linha do compasso não deverá ter mais de 4 tempos com as notações.
-   Alguns botões (@): retirar última nota selecionada (CTRL-Z também deve aplicar o mesmo efeito), anterior e próximo compasso (teclas Q e E consecutivamente devem aplicar o mesmo efeito), adicionar e deletar compasso (teclas A e D consecutivamente devem aplicar o mesmo efeito) e limpar tudo (tecla C deve aplicar o mesmo efeito).
    ( ! ) As notas terão impacto direto na moral dos inimigos - e os _scrolls_ devem ser validados através da alocação das notas.

### Fases e Inimigos

( ! ) O dano nos inimigos é por nota, contudo, o dano da note é divido pelo _length_ dela, ou seja, se o jogador adicionar várias notas 1/32, o dano de cada nota que seria, por exemplo, 10, será dividido por 32, logo, cada nota daria 0.3125 - na prática isso faz com que, ocorra um balanceamento e não há "vantagem real" em adicionar diversas notas visto que o dano será o mesmo do que, por exemplo, adicionar uma única nota com _length_ de 1, onde, 10 / 1 = 10. Há um limite de dano máximo em relação ao dano total (soma dos compassos) - esse limite só pode ser ultrapassado através da utilização de _scrolls_, ou seja, todo dano adicional advindo dos _scrolls_ podem ultrapassar o dano máximo. Notações de pausa não causam dano - alguns _scrolls_ irão requerer a utilização dessas notas. O dano por nota e total deverá ser manipulável para fazer o balanceamento.
( ! ) Importante que as variáveis de dano, porcentagem, pontos de XP e afins sejam modificáveis a fim de poder balancear futuramente.
( ! ) Para cada ataque terá uma chance de X% para que este ocorra, aumentando a chance em Y% deste ocorrer para cada vez que ele não for escolhido.
( ! ) É interessante mostrar o impacto positivo e negativo sobre os valores de combate a fim de explicitar ao jogador o que fora modificado - talvez adicionar o texto com esquema de cores, por exemplo, caso seja uma modificação positiva, o dano mostrado estará em verde, caso seja algo negativo, em vermelho, caso seja o valor normal, uma cor cinza.
( ! ) Os ataque irão aparecer na aba de dicas contra monstros com uma descrição simplificada.

#### Floresta (1)

-   Monstros:
    -   Out of Tune Goblin (Goblin Desafinado):
        -   Lore: Ao redor do Reino Sinfônico existia uma comunidade de goblins cantores cuja cultura se baseava em cantigas afinadíssimas que contavam a história do povo goblinóide. Após a invasão dos alienígenas, estes utilizaram de mecanismos que fizeram com que os goblins perdessem as lindas vozes que tinham, causando revolta e um emergente senso de fúria. Como não compartilhavam mais suas histórias através da música, perderam sua essência e o senso social, voltando, rapidamente, à serem seres violentos e intolerantes.
        -   Ataque:
            -   Empurrão (Shove): ataque do tipo "Impacto", reduz a moral do jogador em X.
            -   Palmas sem Ritmo (Rythmless Claps): ataque do tipo "Impacto", reduz a moral do jogador em X e o faz perder um compasso (caso o jogador só tenha um compasso, aplicar somente o dano) por Y turnos.
        -   Vantagens:
            -   Goblin's Will: Mitigação em X% do dano quando o jogador utiliza acordes em quaisquer compassos.
        -   Desvantagens:
            -   Hateful Melodies: Melodias com notas adjacentes causam X% de dano adicional.
    -   Stepless Werewolf (Lobisomem Sem Ritmo):
        -   Lore: Os lobisomens do Mundo de Sol são conhecidos por todas as terras por suas habilidades de dança. Após a chegada dos extraterrestres, os sons e músicas foram quase extintas - com exceção dos poucos e corajosos musicistas que buscam restaurar a cultura musical - o que causou uma dança sem ritmo algum. A fama e a beleza das danças dos lobisomens fora perdida e, agora, estes vagam sem rumo, de forma descompassada e raivosa pela floresta.
        -   Ataque:
            -   Garras sem Ritmo (Rythmless Claws): ataque do tipo "Cortante", reduz a moral do jogador em X e tem uma chance de Y% de aplicar um efeito que, em cada turno do jogador, este recebe Z de dano por W turnos (cumulativo).
            -   Passos Desvirtuosos (Wayward Steps): ataque do tipo "Impacto", reduz a moral do jogador em X.
        -   Vantagens:
            -   Werewolf's Will: Mitigação em X% quando o jogador toca notas adjacentes em quaisquer compassos.
        -   Desvantagens:
            -   Can't Pause Howls: Melodias com pausas entre as notas causam X% de dano adicional.

#### Masmorra (2)

-   Monstros:
    -   Unshaken Bones (Ossos Paralizados):
        -   Lore: Muitas masmorras no Reino Sinfônico possuem seres misteriosos outrora cheios de vida. Algumas masmorras ecoam o som de queixos batendo, como se houvesse centenas de seres com um frio tremendo lá embaixo. Mesmo antes da chegada dos visitantes extraterrestres, os Ossos Paralizados nunca gostaram de som, afinal, sons altíssimos faziam com que eles se desmontassem devido às vibrações. Logo, os esqueletos sempre foram seres maléficos e cheios de raiva contra os musicistas.
        -   Ataque:
            -   Luvas Descontroladas (Uncontrollable Gloves): ataque do tipo "Impacto", reduz a moral do jogador em X e possui chance de Y% de causar um efeito de _stun_ no jogador onde o número de notas afetadas será de Z.
                -   ( ! ) O efeito de _stun_ funciona da seguinte maneira: após a alocação de todas as notas na partitura, ao iniciar à tocar, algumas notas não irão ser tocadas, como se a tecla estivesse emperrada, a corda desencaixada, etc.
            -   Cabeçada Oca (Airhead Headbutt): ataque do tipo "Impacto", reduz a moral do jogador em X e possui Y% de chance de diminuir em Z% o dano causado pelo jogador no próximo turno.
        -   Vantagens:
            -   Bones's Will: Mitigação em X% quando houver qualquer nota do acorde de Sol Maior ou Dó Maior (chance de 50%/50%) multiplicado pelo número de notas alocadas.
        -   Desvantagens:
            -   Shakes Too Much: Utilização de notas do acorde de Dó Sustenido e Fá Sustenido causam X% de dano adicional - caso ocorra um Arpejo, a % do dano adicional será incrementada em Y.
    -   Soundless Shadows (Sombras Silenciosas):
        -   Lore: Um dia essas criaturas estranhas fizeram parte do Clã de Musicistas, entretanto, com a chegada das criaturas de outros planetas, estes se entregaram e se corromperam pelo vislumbre das tecnologias alienígenas.
        -   Ataque:
            -   Silenciar a Mente (Silence the Mind): ataque do tipo "Mental", aumenta o custo de mana de todos os "scrolls" em X por Y turnos. Chance de Z% de obrigar a utilização de W notas de pausa no turno do jogador.
            -   Dedos Venenosos (Poisoned Fingers): ataque do tipo "Cortante", reduz a moral do jogador em X e tem uma chance de Y% de aplicar um efeito que, em cada turno do jogador, este recebe Z de dano por W turnos (cumulativo).
        -   Vantagens:
            -   Mind's Will: Mitigação completa de dano caso tenha ao menos X notas de pausa na partitura.
        -   Desvantagens:
            -   Headaches: Utilização de notas da escala de Si Menor causam X% de dano adicional.

#### Caverna (3)

-   Monstros:
    -   Silenced Claws (Garras Silenciadas):
        -   Lore: Seres horrendos que aparentam ter um passado tortuoso e obscuro visto que nenhum nativo do Reino Sinfônico havia visto criaturas assim antes. Fofocas locais dizem que tais criaturas são os bichos de estimação dos extraterrestres invasores - o que pode fazer sentido visto as bandagens que impendem estes de emitirem qualquer som cujo volume poderia vir à incomodar.
        -   Ataque:
            -   Cortando Cordas (Cutting Strings): ataque do tipo "Cortante", reduz a moral do jogador em X e impede do jogador utilizar Y linhas da partitura.
            -   Arranhando e Controlando (Scratching and Controlling): ataque do tipo "Cortante", reduz a moral do jogador em X e tem Y% de chance de reduzir Z de mana no próximo turno do jogador.
        -   Vantagens:
            -   Claw's Will: Mitigação de dano em X% ao utilizar a Clave de Sol.
        -   Desvantagens:
            -   Smoothing the Claws: Utilização da Clave de Fá causam X% de dano adicional.
    -   Sound Watcher (Observador de Sons):
        -   Lore: Criaturas enigmáticas cuja origem é desconhecida - contudo, estas começaram a aparecer em diversas cavernas logo após a chegada dos alienígenas. Muitos alegam ter visões um pouco atormentadoras após se depararem com uma dessas criaturas - "[...] os olhos, eu os vejo em todos os lugares [...]".
        -   Ataque:
            -   Tentáculos Pegajosos (Sticky Tentacles): ataque do tipo "Cósmico", impede o jogador de utilizar X linhas de partitura e possui Y% de chance de forçar o jogador a utilizar notações limitadas até 1/2 (1/1, 1/2).
            -   Olhos Julgadores (Judge Eyes): ataque do tipo "Mental", reduz a moral do jogador em X e aumenta o custo de mana de todos os "scrolls" em Y por Z turnos.
        -   Vantagens:
            -   Healing Eyes: A criatura irá se curar em X% caso o jogador utilize notações até 1/2.
            -   Watcher's Will: Mitigação de dano em X% ao utilizar a Clave de Fá.
        -   Desvantagens:
            -   Right At the Eyes: Utilização da Clave de Sol causam X% de dano adicional.

#### Nave Soterrada (4)

-   Monstros:
    -   Alien Captain (Capitã Alienígena):
        -   Lore: Determinada em acabar com toda música e som em todo o universo, a Capitã viaja para diferentes planetas em busca de instaurar sua ditadura silenciosa. Enfurecida e equipada com diferentes tecnologias, seu objetivo é silenciar todos aqueles em seu caminho.
        -   Ataque:
            -   Esmagamento Sônico (Sonic Crush): ataque do tipo "Impacto" e "Mental", reduz a moral do jogador em X e impede qualquer tipo de cura ou proteção no próximo turno do jogador.
            -   Anulação Harmônica (Harmonic Nullification): ataque do tipo "Impacto" e "Côsmico", reduz a moral do jogador em X e altera a posição de algumas notas ao serem tocadas.
        -   Vantagens:
            -   Captain's Will: A criatura mitigará o dano em X% de notas até 1/4.
        -   Desvantagens:
            -   Hateful Modes: A criatura receberá X% de dano adicional caso o jogador siga o modo Dórico.
    -   Abyssal Visitor (Visitante Abissal):
        -   Lore: Portais inimagináveis foram abertos após a colisão da nave extraterrestre no Mundo de Sol. O horror em algumas figuras que surgiram resultaram em um pavor inigualável em toda população do Reino Sinfônico. Tal ser, anteriormente, era considerado apenas um mito... mas sua chegada fora confirmada oficialmente pelo Clã de Musicistas. Tristeza, pavor e loucura são sinônimos deste ser maléfico e seu inevitável ódio pela alegria que a música traz, o tornou um dos grandes vilões contra o Mundo de Sol.
        -   Ataque:
            -   Força Abissal (Abysmal Force): ataque do tipo "Cortante" e "Cósmico", reduz a moral do jogador em X, possui uma chance de Y% de aplicar um efeito que, em cada turno do jogador, este recebe Z de dano por W turnos e possui J% de chance de queimar K pergaminhos do jogador no próximo turno.
            -   Mente Atormentada (Troubled Mind): ataque do tipo "Cósmico" e "Mental", para cada mana gasta pelo jogador reduzir a moral do jogador em X, possui chance de Y% de gastar Z manas automaticamente no próximo turno do jogador.
        -   Vantagens:
            -   Ph'nglui mglw'nafh Cthulhu R'lyeh wgah'nagl fhtagn: Ao utilizar todos os compassos disponíveis, o Visitante Abissal ficará enfurecido aumentado o dano em X% por Y turnos.
        -   Desvantagens:
            -   I'm not my father: Criatura receberá X% de dano caso o jogador siga o modo Frígio.

##### Efeitos

-   Efeitos de Ataque (Nomes apenas indicativos):
    -   Dano/Damage: dar Dano X
    -   Limitação Harmônica/Harmonic Limitation: não pode usar Y Compassos (o jogador deve ter ao mínimo 1)
    -   Desgaste Moral/Moral Decay: perde X de moral a cada turno por Z turnos.
    -   Atordoamento/Stun: chance de Y% de algumas notas não serem tocadas.
    -   Cansaço/Fatigue: chance de Z% diminui o dano total causado em X%
    -   Explosão Mental/Mind Blowing: aumenta o custo de mana de todos os _scrolls_ em X por Y turnos - caso seja aplicado este efeito novamente, adicionar somente o número de turnos sobre o efeito.
    -   Limitação Sonora/Sound Limitation: não poder usar N linhas da partitura (em ambas claves) no próximo turno.
    -   Extração Mental/Mental Extraction: Y% de chance de reduzir Z de mana no próximo turno do jogador.
    -   Negação/Denial: impede qualquer tipo de cura ou proteção no próximo turno do jogador.
    -   Confusão/Confusion: altera a posição de algumas notas ao serem tocadas.

### Almanaque Scrolls, Dicas Musicais e Dicas de Monstros:

Cada uma das _runs_ permitem o _trigger_ de eventos específicos de acordo com ações do jogador, ademais, tais eventos desbloqueiam novas dicas. Caso, em uma única _run_, mais de um evento seja _triggado_ deverá ser desbloqueado, aleatoriamente, somente uma dica para aquela _run_ - um evento cuja dica já fora desbloqueada não pode ser considerado em _runs_ futuras. Algumas dicas podem necessitar de múltiplos eventos ou eventos repetidos para serem desbloqueadas. Ao desbloquear uma dica, esta deverá aparecer no menu após clicar no botão de _tips_ - alertar o jogador que uma nova dica foi desbloqueada (utilizar de um pop up genérico estilo menus do South Park and the Stick of Truth, entretanto, com opacidade menor - o popup deverá aparecer algo meio meta game, ou seja, não terá uma arte no contexto do jogo mas será parecido como uma notificação). O mesmo se aplicar as dicas de monstros, entretanto, para todas os eventos _triggados_ de dicas de monstros, desbloquear todas as dicas - ou seja, diferente das dicas de teoria musical, não é limitada à uma dica de monstro por _run_.

( ! ) Desbloquear _card_ do monstro apenas após o primeiro encontro com ele.

-   XP: fórmula matemática baseada (na real é cópia mesmo kkk) no sistema de experiência do Runescape, proporcionando algo exponencial de acordo com o _level_ numa progressão bem interessante.
-   https://runescape.fandom.com/wiki/Experience

```C#
	   float total = 0;
	   float level = 1;
       float baseExperiencePerLevel = 522;

        for (int i = 1; i <= level; i++)
        {
            total += (float)Math.Floor(i + baseExperiencePerLevel * Math.Pow(2, i / 7.0));
        }
       
		float final = (int)Math.Floor(total / 4);

        Debug.WriteLine(final);
```

-   Scrolls:
    -   ( ! ) Para cada _scroll_ bem sucedido, deverá ser adicionado um valor de XP (o valor de XP é definido como uma prop no próprio _scroll_) para o cálculo de XP do fim da _run_.
    -   ( ! ) Todos os _scrolls_ devem possuir a possibilidade de mudar o custo de mana, valor dos efeitos e tempo de espera para fins de balanceamento.
    -   ( ! ) Escalas, acordes, notas, etc, aleatórias devem ser escolhidas no início do turno do jogador - pode usar qualquer tipo de algoritmo para escolha.
    -   ( ! ) Os valores aleatórios deverão ser explicitados na descrição durante o JOGO, ou seja, o texto na indica indicará que será aleatório, o texto no jogo especificará o valor aleatório.
    -   Roddie (por nível): a ideia principal é destacar alguns conceitos básicos de teoria musical e técnicas específicas de piano... para facilitar a experiência do jogador, boa parte dos efeitos dos _scrolls_ são relacionados à poderes de cura, mitigação de dano ou invunerabilidade. Para cada nível upado, a personagem aumenta a vida máxima, mana máxima, número de compassos disponíveis e desbloqueia novos _scrolls_ - gostaria de ter a possibilidade de alterar manualmente para cada nível visto que pode ter nível que não ganhe nenhuma mana adicional.
        1.  -   Primeiras Notas Maiores
                Objetivo: Reproduza notas de uma escala maior aleatória. (Texto na dica)
                Efeito: Aumenta temporariamente a mitigação de dano em X% por cada nota da escala - máximo de Y%.
                Tempo de Espera (Número de turnos para poder utilizar novamente): 1
                Custo: Z mana.
            -   Explorando Melodias Maiores
                Objetivo: Em um compasso, improvise uma melodia em uma escala maior aleatória. (Texto na dica)
                Efeito: Aumenta o dano por nota pertencente à escala em X% - máximo dano bônus total de Y%.
                Tempo de Espera (Número de turnos para poder utilizar novamente): 1
                Custo: Z mana.
        2.  -   Pausas para Descansar
                Objetivo: para cada compasso, adicione pausas equivalentes à uma batida completa (1/4).
                Efeito: Cura a moral em X%, no máximo de Y%.
                Tempo de Espera (Número de turnos para poder utilizar novamente): 3
                Custo: Z mana.
            -   Mudança de Estratégia
                Objetivo: utilize a Clave de Fá.
                Efeito: Reduz o tempo de espera em X turnos de todos os _scrolls_, não inclui ele mesmo.
                Tempo de Espera (Número de turnos para poder utilizar novamente): 3
                Custo: Z mana.
        3.  -   Entre Tons
                Objetivo: em uma escala maior aleatória, em um compasso, adicione somente os semitons.
                Efeito: para cada semitom adicionado, adicione X de dano no dano total - máximo de Y de dano adicional.
                Tempo de Espera (Número de turnos para poder utilizar novamente): 2
                Custo: Z mana.
            -   Entre Semitons
                Objetivo: em uma escala maior aleatória, em um compasso, adicione somente os tons.
                Efeito: para cada tom adicionado, adicione X de dano no dano total - máximo de Y de dano adicional.
                Tempo de Espera (Número de turnos para poder utilizar novamente): 2
                Custo: Z mana.
        4.  -   Mais Acordes
                Objetivo: adicione em um único compasso até três acordes aleatórios - os acordes são tríades - requer lista de acordes válidos.
                Efeito: para cada acorde adicionado, cause X de dano adicional por turno durante Y turnos.
                Tempo de Espera (Número de turnos para poder utilizar novamente): 3 turnos (de forma prática tentarei sempre adicionar o Y + 1, mas poderei rever isso).
                Custo: Z mana.
            -   Força da Tônica
                Objetivo: em um único compasso adicione uma semibreve (nota cheia) tônica de um acorde aleatório.
                Efeito: causa um dano X massivo (DANO REAL, não é bloqueado ou mitigado).
                Tempo de Espera (Número de turnos para poder utilizar novamente): 5 turnos.
                Custo: Z mana.
        5.  -   Ambiguidade
                Objetivo: em compassos diferentes, adicione notas enarmônicas.
                Efeito: cure X de moral para cada nota adicionada, máximo de Y de cura.
                Tempo de Espera (Número de turnos para poder utilizar novamente): 4 turnos.
                Custo: Z mana.
            -   Passagem e Controle
                Objetivo: em um único compasso adicione as notas subdominantes e dominantes de um acorde tríade aleatório - requer lista de acordes válidos.
                Efeito: para cada nota adicionada, receba X de mana adicional no próximo turno - máximo de Y manas.
                Tempo de Espera (Número de turnos para poder utilizar novamente): 6 turnos.
                Custo: Z mana.
        6.  -   Progresso Comum
                Objetivo: em três diferentes compassos, realize as progressões de acorde I-iv-V-I partindo de três acordes (I) aleatórios - em qualquer ordem.
                Efeito: para cada progressão realizada, diminua em X a percentagem de mitigação de dano do inimigo por Y turnos - diminuição máximo sendo igual a Z.
                Tempo de Espera (Número de turnos para poder utilizar novamente): 4 turnos.
                Custo: Z mana.
        7.  -   Cura entre Arpejos
                Objetivo: realize o arpejo de três acordes acordes aleatórios em três compassos distintos na ordem designada.
                Efeito: elimina todos os efeitos negativos e cura a moral em X.
                Tempo de Espera (número de turnos para poder utilizar novamente): 8 turnos.
                Custo: Z mana.
        8.  -   Entre Escalas
                Objetivo: em quatro compassos distintos, adicione somente notas de quatro escalas maiores aleatórias.
                Efeito: causa atordoamento no inimigo - um dos ataques do inimigo fica desabilitado por X turnos - efeito não cumulativo.
                Tempo de Espera (número de turnos para poder utilizar novamente): 6 turnos.
                Custo: Z mana.
        9.  -   Melódico
                Objetivo: utilize metade de seus compassos disponíveis para compor uma melodia - não pode haver sobreposição de notas e a soma da duração de todas as notas de cada compasso devem equivaler ao tempo do compasso.
                Efeito: o dano de cada nota é multiplicado por X% - sem limite.
                Tempo de Espera (número de turnos para poder utilizar novamente): 8 turnos.
                Custo: Z mana.
        10. -   Pianista com Modos
                Objetivo: em todos os compassos disponíveis, componha utilizando apenas as notas seguindo um Modo aleatório a partir de uma nota musical aleatória (qualquer nota é válida) - as notas podem estar em uma ordem não subsequente.
                Efeito: causa X de dano massivo, cura em Y a moral e por Z turnos aplica W% de mitigação de dano.
                Tempo de Espera (número de turnos para poder utilizar novamente): 10 turnos.
                Custo: Z mana.
-   Dicas:
    -   Notações: Notações musicais são símbolos utilizados para representar sons e elementos musicais em partituras. Existem diferentes tipos de notações musicais, cada uma com sua função específica. Algumas das notações musicais mais comuns incluem:
        -   Notas: altura e duração dos sons.
        -   Pausas: indicam os momentos de silêncio na música, correspondendo às durações das notas.
        -   Claves: são símbolos colocados no início de uma pauta para indicar a altura das notas, exemplos: clave de Sol e Fá.
        -   Sinais de alteração: alteram a altura de uma nota em um semitom para cima (sustenido) ou para baixo (bemol).
    -   Métrica: é o padrão de agrupamento de batidas em compassos na música. Cada compasso tem um número definido de batidas, determinado pelo denominador de uma fração de compasso (como 4/4, 3/4, 6/8, etc.)
    -   Melodia: é a parte da música que se destaca acima dos acordes e do ritmo, variando em seu tamanho, forma, altura, timbre e intensidade. Lembre-se que uma das características mais importantes da melodia é que as notas são tocadas individualmente e sequencialmente. Ao escutar uma música, tente assoviar ou cantarolar aquilo que você acha que se destaca, provavelmente, será a melodia.
    -   Tom: um tom é uma unidade de medida que representa a altura de um som - na escala musical ocidental, um tom corresponde a uma distância de dois semitons.
    -   Semitom: um semitom corresponde à menor distância entre dois sons consecutivos em uma escala cromática.
    -   Notas Enarmônicas: duas notas que possuem o mesmo som mas nomes diferentes, como Dó sustenido (C#) e Ré bemol (Db), estão separadas por um semitom.
    -   Acordes: um acorde é um conjunto de três ou mais notas diferentes tocadas simultaneamente, fornecem a base para a harmonia e a estrutura tonal em músicas de todos os tipos. Os acordes mais básicos são tríades, que consistem em três notas: a tônica, a terça e a quinta.
    -   Progressão de Acordes: uma progressão de acordes é uma sequência de acordes que ocorre em uma música, as progressões de acordes são uma parte fundamental da composição musical e são responsáveis por criar tensão, resolução e uma sensação de movimento na música. Existem muitas progressões de acordes comuns que são amplamente utilizadas em músicas de diversos gêneros, por exemplo, a progressão I-IV-V-I (tônica, subdominante, dominante, tônica).
    -   Tônica: a tônica é o grau tonal principal e central em uma tonalidade ou progressão de acordes proporciona uma sensação de estabilidade e repouso.
    -   Subdominante: possui sentido de suspensão com pouca tensão, criar uma sensação de movimento em direção à tônica e adiciona interesse harmônico sem criar a mesma tensão que a dominante.
    -   Dominante: possui sentido de suspensão com mais tensão, crucial para criar tensão e preparar a resolução de volta à tônica.
    -   Escalas: uma escala musical é uma sequência ordenada de notas musicais que segue um padrão específico de intervalos entre as notas - onde tais intervalos entre as notas em uma escala determinam o seu padrão tonal, por exemplo, na escala maior, o padrão de intervalos é tom-tom-semitom-tom-tom-tom-semitom.
    -   Modos: os modos musicais são escalas que derivam da escala diatônica, mas começam em diferentes graus dessa escala, resultando em padrões de intervalos únicos e sonoridades distintas
        -   Jônio (Modo Maior): é o primeiro modo da escala diatônica e começa na tônica.
        -   Dórico: começa no segundo grau da escala diatônica e tem um padrão de intervalos de tom-semitom-tom-tom-tom-semitom-tom.
        -   Frígio: começa no terceiro grau da escala diatônica e tem um padrão de intervalos de semitom-tom-tom-tom-semitom-tom-tom.
        -   Lídio: começa no quarto grau da escala diatônica e tem um padrão de intervalos de tom-tom-tom-semitom-tom-tom-semitom.
        -   Mixolídio: começa no quinto grau da escala diatônica e tem um padrão de intervalos de tom-tom-semitom-tom-tom-semitom-tom.
        -   Eólio (Modo Menor Natural): Começa no sexto grau da escala diatônica e tem um padrão de intervalos de tom-semitom-tom-tom-semitom-tom-tom.
    -   Arpejo: um arpejo é uma sequência de notas de um acorde tocadas uma após a outra, em vez de simultaneamente.

