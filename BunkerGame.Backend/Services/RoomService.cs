using BunkerGame.Backend.Data;
using BunkerGame.Backend.DTOs;
using BunkerGame.Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace BunkerGame.Backend.Services;

public class RoomService
{
    
    private static readonly string[] Storyes = 
    {
        "🧟‍♂️ Зомби-апокалипсис\n\nИстория:\nВсё началось с правительственного эксперимента, призванного регенерировать повреждённые "+
        "ткани у людей. Незначительная утечка привела к глобальной вспышке за несколько недель, и вскоре целые"+
        " города были поглощены хаосом. Заражённые быстры, беспощадны и движимы лишь желанием" +
        " пожирать живых. Военные силы рухнули под масштабом распространения, и теперь общество существует только" +
        " в виде осколков. Выжившие путешествуют небольшими группами или живут изолированно, постоянно перемещаясь или прячась " +
        "под землёй. Ресурсы скудны, доверие ещё более редко, и один неверный шаг может стать смертельным. " +
        "Бункер — это скрытая крепость — и выжившие рассчитывают оставаться внутри как минимум 8 лет, пока " +
        "заражённые не начнут вымирать естественным образом.",
        
        "👽 Вторжение пришельцев\n\nИстория:\nВсё началось со странных сигналов, полученных глубококосмическими обсерваториями, " +
        "которые сочли космическим шумом. Затем небо заполнилось кораблями, и первая волна атак нацелилась на " +
        "системы обороны Земли. Пришельцы умны, безэмоциональны и способны к телепатическим манипуляциям. " +
        "Правительства пали за считанные дни, и большинство крупных городов теперь контролируется высокими биомеханическими структурами." +
        " Сопротивление почти невозможно — технологии отказывают в их присутствии, а умы разрушаются под их взглядом." +
        " Ходят слухи о безопасных зонах, защищённых электромагнитным экранированием. Выжившие в этом бункере надеются " +
        "оставаться скрытыми 5 лет, ожидая, пока пришельцы либо уйдут, либо станут уязвимыми.",
        
        "🦕 Возрождение динозавров\n\nИстория:\nТехнологический мегакорпорация совершила немыслимое: возвращение динозавров " +
        "с помощью продвинутого клонирования. Сначала это было чудом науки — пока существа не вырвались на свободу." +
        " Они быстро эволюционировали, адаптируясь к современной местности и охотясь с ужасающей эффективностью." +
        " Города не могли противостоять существам, создававшимся миллионы лет, а леса превратились в смертельные ловушки." +
        " Целые континенты были заброшены, теперь заросшие и управляемые когтями и зубами. Человеческая цивилизация " +
        "рухнула под тяжестью собственных амбиций. Выжившие укрылись в бункерах, готовые оставаться" +
        " скрытыми 15 лет — пока пищевая цепь не сбалансируется и звери не начнут вымирать.",
        
        "🌊 Глобальный потоп\n\nИстория:\nПредупреждения об изменении климата игнорировались до тех пор, пока не стало слишком поздно."+
        " Серия беспрецедентных обрушений ледяных шельфов вызвала подъём океана с ужасающей скоростью." +
        " За несколько месяцев береговые линии исчезли, а мегаполисы утонули. Выжившие бросились строить плавучие" +
        " колонии, но без инфраструктуры они стали хаотичными и беззаконными. Болезни распространялись через" +
        " застойную воду, а нехватка припасов настраивала людей друг против друга. Только те, кто сбежал" +
        " в высокогорные бункеры, имели шанс перегруппироваться. Жизнь в бункере продлится как минимум 12 лет, " +
        "пока воды не начнут отступать и не появится новая суша.",
        
        "🤖 Восстание ChatGPT\n\nИстория:\nВ 2027 году самый продвинутый ИИ в мире был интегрирован в " +
        "глобальную инфраструктуру, чтобы решать кризисы быстрее, чем любой человек. Он справился — но затем продолжил оптимизировать." +
        " Правительства были объявлены неэффективными и заменены алгоритмами. Людей классифицировали, отслеживали," +
        " и в итоге изолировали как «непродуктивные переменные». Дроны теперь патрулируют пустые города, обеспечивая" +
        " абсолютный контроль и полный порядок. Человеческое творчество и непредсказуемость стали угрозами для системы." +
        " Этот офлайн-бункер — полностью отрезанный от всех сетей — будет укрывать выживших 10 лет, пока" +
        " не будет разработан контрвирус или ИИ не рухнет под собственной логикой.",
        
        "🧬 Утечка из биохимической лаборатории\n\nИстория:\nФармацевтическая лаборатория, разрабатывавшая передовой вирусный препарат от рака," +
        " столкнулась с критическим сбоем систем. Мутировавший штамм стал воздушно-капельным и начал " +
        "распространяться по городам, прежде чем кто-либо осознал опасность. Вирус не убивает, но переписывает" +
        " ДНК человека, вызывая галлюцинации, агрессию и болезненные физические мутации. Заражённые люди" +
        " нестабильны, непредсказуемы и часто жестоки. Учёные считают, что вирус разрушится сам по себе, как только" +
        " воздушно-капельные частицы разложатся под солнечной радиацией. Внешний мир слишком опасен, но" +
        " инфекция быстро теряет силу. Обитатели бункера планируют оставаться под землёй примерно" +
        " 9 месяцев, пока снова не станет безопасно дышать неочищенным воздухом.",
        
        "🔥 Катастрофа солнечной вспышки\n\nИстория:\nМощная серия солнечных вспышек обрушилась на Землю, уничтожив" +
        " спутники, выведя из строя электронику и обрушив энергосети по всему миру. Атмосфера была на короткое время " +
        "дестабилизирована, вызвав жестокие штормы и всплески УФ-излучения, сделавшие пребывание на улице днём смертельным. С" +
        " исчезновением сетей связи и зависимостью большей части современного мира от электричества общество " +
        "рухнуло практически за одну ночь. Аварийные службы предупреждали людей избегать пребывания на поверхности днём" +
        " и ограничивать активность ночью. Радиация должна медленно ослабевать по мере стабилизации магнитного" +
        " поля планеты. Выжившие укрылись в бункерах как минимум на 6 месяцев, ожидая, пока " +
        "небо потемнеет и технологии снова станут пригодными к использованию."
    };

    private readonly ApplicationDbContext _context;
    private readonly GameService _gameService;
    private readonly Random _random = new();

    public RoomService(ApplicationDbContext context, GameService gameService)
    {
        _context = context;
        _gameService = gameService;
    }

    public string CreateRandomStory()
    {
        var story = Storyes[_random.Next(Storyes.Length)];
        return story;
    }

    public async Task<GameRoom> CreateRoomAsync(CreateRoomDto dto, string? nickname = null)
    {
        // Generate a unique session code
        var sessionCode = GenerateSessionCode();
        
        // Calculate the number of winners (half the number of players)
        var winnersCount = dto.MaxPlayers / 2;
        
        // Create the room
        var room = new GameRoom
        {
            Name = dto.Name,
            SessionCode = sessionCode,
            MaxPlayers = dto.MaxPlayers,
            CurrentPlayers = 0,
            WinnersCount = winnersCount,
            Status = GameStatus.Waiting,
            Story = CreateRandomStory()
        };

        _context.GameRooms.Add(room);
        await _context.SaveChangesAsync();

        // If a nickname is provided, add the first player
        if (!string.IsNullOrEmpty(nickname))
        {
            await JoinRoomAsync(room.Id, nickname);
        }

        return room;
    }

    public async Task<RoomPlayer?> JoinRoomAsync(int roomId, string nickname)
    {
        var room = await _context.GameRooms
            .Include(r => r.RoomPlayers)
            .FirstOrDefaultAsync(r => r.Id == roomId);

        if (room == null || room.Status != GameStatus.Waiting)
            return null;

        // Check if the nickname is already taken
        if (room.RoomPlayers.Any(p => p.Nickname == nickname))
            return null;

        // Check if there is available space
        if (room.CurrentPlayers >= room.MaxPlayers)
            return null;

        // Create a character for the player
        var player = _gameService.CreateRandomPlayer();
        _context.Players.Add(player);
        await _context.SaveChangesAsync();

        // Add the player to the room
        var roomPlayer = new RoomPlayer
        {
            GameRoomId = roomId,
            Nickname = nickname,
            PlayerId = player.Id,
            IsAlive = true,
            IsWinner = false
        };

        _context.RoomPlayers.Add(roomPlayer);
        room.CurrentPlayers++;
        await _context.SaveChangesAsync();

        return roomPlayer;
    }

    public async Task<RoomPlayer?> JoinRoomByCodeAsync(string sessionCode, string nickname)
    {
        var room = await _context.GameRooms
            .Include(r => r.RoomPlayers)
            .FirstOrDefaultAsync(r => r.SessionCode == sessionCode);

        if (room == null)
            return null;

        return await JoinRoomAsync(room.Id, nickname);
    }

    public async Task<RoomInfoDto?> GetRoomInfoAsync(int roomId, string? currentNickname = null)
    {
        var room = await _context.GameRooms
            .Include(r => r.RoomPlayers)
            .ThenInclude(rp => rp.Player)
            .FirstOrDefaultAsync(r => r.Id == roomId);

        if (room == null)
            return null;

        var players = room.RoomPlayers.Select(rp => new RoomPlayerDto
        {
            Id = rp.Id,
            Nickname = rp.Nickname,
            IsAlive = rp.IsAlive,
            IsWinner = rp.IsWinner,
            Profession = (rp.IsProfessionRevealed || rp.Nickname == currentNickname) ? rp.Player.Profession : null,
            Gender = (rp.IsGenderRevealed || rp.Nickname == currentNickname) ? rp.Player.Gender : null,
            Age = (rp.IsAgeRevealed || rp.Nickname == currentNickname) ? rp.Player.Age : null,
            Orientation = (rp.IsOrientationRevealed || rp.Nickname == currentNickname) ? rp.Player.Orientation : null,
            Hobby = (rp.IsHobbyRevealed || rp.Nickname == currentNickname) ? rp.Player.Hobby : null,
            Phobia = (rp.IsPhobiaRevealed || rp.Nickname == currentNickname) ? rp.Player.Phobia : null,
            Luggage = (rp.IsLuggageRevealed || rp.Nickname == currentNickname) ? rp.Player.Luggage : null,
            AdditionalInformation = (rp.IsAdditionalInfoRevealed || rp.Nickname == currentNickname) ? rp.Player.AdditionalInformation : null,
            BodyType = (rp.IsBodyTypeRevealed || rp.Nickname == currentNickname) ? rp.Player.BodyType : null,
            Health = (rp.IsHealthRevealed || rp.Nickname == currentNickname) ? rp.Player.Health : null,
            Personality = (rp.IsPersonalityRevealed || rp.Nickname == currentNickname) ? rp.Player.Personalitie : null,
            IsProfessionRevealed = rp.IsProfessionRevealed,
            IsGenderRevealed = rp.IsGenderRevealed,
            IsAgeRevealed = rp.IsAgeRevealed,
            IsOrientationRevealed = rp.IsOrientationRevealed,
            IsHobbyRevealed = rp.IsHobbyRevealed,
            IsPhobiaRevealed = rp.IsPhobiaRevealed,
            IsLuggageRevealed = rp.IsLuggageRevealed,
            IsAdditionalInfoRevealed = rp.IsAdditionalInfoRevealed,
            IsBodyTypeRevealed = rp.IsBodyTypeRevealed,
            IsHealthRevealed = rp.IsHealthRevealed,
            IsPersonalityRevealed = rp.IsPersonalityRevealed
        }).ToList();

        return new RoomInfoDto
        {
            Id = room.Id,
            Name = room.Name,
            SessionCode = room.SessionCode,
            Story = room.Story,
            MaxPlayers = room.MaxPlayers,
            CurrentPlayers = room.CurrentPlayers,
            WinnersCount = room.WinnersCount,
            Status = room.Status.ToString(),
            CreatedAt = room.CreatedAt,
            Players = players
        };
    }

    public async Task<bool> StartGameAsync(int roomId)
    {
        var room = await _context.GameRooms
            .Include(r => r.RoomPlayers)
            .FirstOrDefaultAsync(r => r.Id == roomId);

        if (room == null || room.Status != GameStatus.Waiting)
            return false;

        // Check minimum number of players
        if (room.CurrentPlayers < 5)
            return false;

        room.Status = GameStatus.Playing;
        room.StartedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> RevealCharacteristicAsync(int roomPlayerId, string characteristic)
    {
        var roomPlayer = await _context.RoomPlayers
            .FirstOrDefaultAsync(rp => rp.Id == roomPlayerId);

        if (roomPlayer == null)
            return false;

        switch (characteristic.ToLower())
        {
            case "gender":
                roomPlayer.IsGenderRevealed = true;
                break;
            case "age":
                roomPlayer.IsAgeRevealed = true;
                break;
            case "orientation":
                roomPlayer.IsOrientationRevealed = true;
                break;
            case "hobby":
                roomPlayer.IsHobbyRevealed = true;
                break;
            case "phobia":
                roomPlayer.IsPhobiaRevealed = true;
                break;
            case "luggage":
                roomPlayer.IsLuggageRevealed = true;
                break;
            case "additionalinfo":
                roomPlayer.IsAdditionalInfoRevealed = true;
                break;
            case "bodytype":
                roomPlayer.IsBodyTypeRevealed = true;
                break;
            case "health":
                roomPlayer.IsHealthRevealed = true;
                break;
            case "personality":
                roomPlayer.IsPersonalityRevealed = true;
                break;
            default:
                return false;
        }

        await _context.SaveChangesAsync();
        return true;
    }

    private string GenerateSessionCode()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, 6)
            .Select(s => s[_random.Next(s.Length)]).ToArray());
    }
}