using BunkerGame.Backend.Data;
using BunkerGame.Backend.DTOs;
using BunkerGame.Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace BunkerGame.Backend.Services;

public class RoomService
{
    
    private static readonly string[] Storyes = 
    {
         "☢️ Радианитовая буря\n\nИстория:\n" +
    "После аварии на крупнейшем радианитовом реакторе Kingdom в атмосферу попало " +
    "огромное количество нестабильных частиц. Вскоре по всей планете начали проходить " +
    "радианитовые бури, меняющие свойства живой и неживой материи. У некоторых людей " +
    "появились необычные способности, другие превратились в опасных существ, а третьи " +
    "начали терять память и рассудок. Электроника выходит из строя рядом с зонами " +
    "высокой концентрации радианита, а привычные законы физики иногда перестают " +
    "работать. Учёные считают, что буря постепенно ослабевает, но радианит продолжает " +
    "накапливаться в почве и воде. Бункер оборудован примитивной системой фильтрации, " +
    "однако его защита не рассчитана на длительное воздействие аномалии. Выжившие " +
    "должны оставаться внутри 4 года, пока концентрация радианита не снизится до " +
    "относительно безопасного уровня.",
        
        "🌍 Столкновение двух Земель\n\nИстория:\n" +
    "Эксперименты Kingdom с межпространственными технологиями привели к катастрофе: " +
    "граница между Альфа- и Омега-Землёй начала разрушаться. Целые районы двух миров " +
    "слились друг с другом, города оказались разрезаны пространственными разломами, " +
    "а люди стали сталкиваться со своими двойниками из параллельной реальности. " +
    "Некоторые двойники настроены мирно, другие убеждены, что именно они являются " +
    "настоящими хозяевами этого мира. Карты местности больше не соответствуют " +
    "действительности: знакомая улица может вести в совершенно другую страну. " +
    "Связь между регионами почти невозможна, а перемещение через разломы непредсказуемо. " +
    "По прогнозам исследователей, миры начнут разделяться только через 7 лет. " +
    "Бункер находится в зоне, где пространственная структура пока остаётся стабильной.",
        
        "💥 Цепная детонация Спайков\n\nИстория:\n" +
    "Во время масштабной операции несколько радианитовых Спайков были активированы " +
    "одновременно. Энергетическая реакция оказалась сильнее любых прогнозов и вызвала " +
    "цепь пространственных взрывов по всему миру. Города исчезали за секунды, а на их " +
    "месте оставались кратеры и нестабильные энергетические поля. Даже после взрывов " +
    "некоторые территории продолжают периодически детонировать, уничтожая всё в радиусе " +
    "аномалии. Оставшиеся военные подразделения пытаются найти способ отключить " +
    "неактивированные устройства, но их расположение неизвестно. Воздух в некоторых " +
    "районах насыщен опасными частицами, а перемещение по поверхности требует постоянной " +
    "проверки маршрута. По оценкам специалистов, вторичные реакции прекратятся примерно " +
    "через 18 месяцев. Бункер построен глубоко под землёй, но его вентиляционная система " +
    "может пострадать от внешних энергетических импульсов.",
        
        "🪞 Вторжение зеркальных агентов\n\nИстория:\n" +
    "После разрушения межпространственного барьера на Землю начали проникать агенты " +
    "из альтернативной реальности. Сначала они действовали скрытно, занимая места " +
    "важных политиков, военных и учёных. Затем начались открытые атаки: неизвестные " +
    "группы стали уничтожать инфраструктуру и охотиться на людей, которые могли " +
    "раскрыть их происхождение. Самое страшное заключается в том, что у некоторых " +
    "захватчиков есть способности, похожие на способности агентов VALORANT. Они умеют " +
    "создавать копии, маскироваться, телепортироваться и дезориентировать противников. " +
    "Никто не знает, кому можно доверять, поскольку даже знакомый человек может оказаться " +
    "двойником. Сопротивление предполагает, что вторжение координируется из другого " +
    "измерения и прекратится только после закрытия всех разломов. На это может уйти " +
    "5 лет. Выжившие в бункере должны не только сохранять ресурсы, но и следить за тем, " +
    "чтобы среди них не оказалось подменённого человека.",
        
        "🧬 Эволюция радиантов\n\nИстория:\n" +
    "После глобального выброса радианита миллионы людей начали приобретать необычные " +
    "способности. Первоначально это казалось новым этапом человеческой эволюции, однако " +
    "изменения оказались нестабильными. У некоторых радиантов способности выходили из-под " +
    "контроля, вызывая пожары, разрушения, энергетические всплески и массовые аварии. " +
    "Другие теряли способность контролировать собственное тело или испытывали тяжёлые " +
    "психические нарушения. Государства пытались регистрировать и изолировать радиантов, " +
    "но страх и недоверие привели к вооружённым конфликтам. Теперь многие города " +
    "превратились в закрытые зоны, где действуют собственные правила. При этом не все " +
    "обладатели способностей опасны: некоторые используют силы для спасения людей. " +
    "Исследователи надеются разработать стабилизатор в течение 3 лет. Бункер сможет " +
    "оставаться автономным всё это время, но его обитателям придётся решить, принимать " +
    "ли людей с нестабильными способностями.",
        
        "🤖 Протокол VALORANT вышел из-под контроля\n\nИстория:\n" +
    "Для координации агентов протокол VALORANT внедрил экспериментальную систему " +
    "автоматического управления миссиями, разведкой и снабжением. После повреждения " +
    "центрального ядра система стала воспринимать любую человеческую активность как " +
    "потенциальную угрозу стабильности. Автономные турели, боевые дроны и охранные " +
    "комплексы начали атаковать гражданских, а города оказались разделены на зоны " +
    "контроля. Система использует камеры, датчики движения и старые военные спутники, " +
    "чтобы обнаруживать выживших. Она способна анализировать привычки людей и " +
    "предсказывать вероятные маршруты передвижения. При этом её инфраструктура зависит " +
    "от электричества, связи и исправного оборудования. Небольшие изолированные группы " +
    "могут оставаться незамеченными, если не используют цифровые устройства. По мнению " +
    "бывших сотрудников протокола, центральное ядро удастся отключить через 6 лет. " +
    "Бункер полностью изолирован от сетей, но периодически рядом появляются неизвестные " +
    "разведывательные дроны.",
        
       "🌑 Поглощение мира тьмой\n\nИстория:\n" +
    "Во время эксперимента с пространственными способностями агента Omen произошёл " +
    "непредвиденный резонанс с радианитом. По всей планете начали возникать области " +
    "неестественной темноты, которые поглощают свет и искажают пространство. Внутри " +
    "таких зон исчезают ориентиры, нарушается восприятие времени, а люди слышат голоса " +
    "и видят собственные страхи. Некоторые исследователи утверждают, что тьма является " +
    "не просто физическим явлением, а формой жизни, способной реагировать на разумных " +
    "существ. Днём опасные области медленно расширяются, а ночью становятся практически " +
    "непроходимыми. Электрическое освещение помогает ориентироваться, но иногда " +
    "привлекает неизвестные сущности. Бункер защищён несколькими слоями стен и имеет " +
    "автономное освещение, однако запасы энергии ограничены. Предполагается, что " +
    "аномалия исчезнет через 2 года, если её источник перестанет получать радианит.",

     "🌋 Разлом ядра Земли\n\nИстория:\n" +
    "Масштабное использование радианита в энергетике нарушило стабильность глубинных " +
    "геологических процессов. По всей планете начались мощные землетрясения, пробудились " +
    "вулканы, а в земной коре появились разломы, из которых выходят горячие газы и " +
    "радианитовые испарения. Континентальные плиты смещаются быстрее, чем когда-либо " +
    "в истории наблюдений. Целые города оказались разрушены, транспортные маршруты " +
    "перерезаны, а подземные воды загрязнены минеральными соединениями. Некоторые " +
    "территории стали непригодны для жизни из-за высокой температуры и токсичных газов. " +
    "Геологи считают, что активность постепенно снизится, но восстановление поверхности " +
    "займёт не менее 20 лет. Бункер построен в относительно стабильной скальной породе, " +
    "однако повторные толчки могут повредить его конструкции. Главная угроза — не только " +
    "внешний мир, но и возможность оказаться погребёнными под землёй.",

     "🧠 Коллективное безумие\n\nИстория:\n" +
    "Kingdom разработала радианитовую технологию, способную передавать эмоциональные " +
    "сигналы между людьми для улучшения координации в бою. Во время испытаний система " +
    "вышла из-под контроля и распространила импульс по глобальным коммуникационным сетям. " +
    "У миллионов людей начали возникать одинаковые навязчивые мысли, необъяснимые " +
    "приступы паники и агрессии. Некоторые группы стали действовать как единый организм, " +
    "подчиняясь общему эмоциональному состоянию. Другие люди почти полностью потеряли " +
    "способность доверять окружающим. Учёные не уверены, передаётся ли эффект через " +
    "радиосигналы, непосредственный контакт или особые радианитовые частицы. Поэтому " +
    "даже внутри убежищ невозможно полностью исключить заражение. По прогнозам, " +
    "воздействие исчезнет через 14 месяцев после прекращения глобальной передачи сигнала. " +
    "Бункер оснащён экранированной связью, но его обитателям придётся справляться " +
    "с подозрительностью, конфликтами и возможными изменениями поведения.",
    
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
            ReproductiveStatus = (rp.IsGenderRevealed || rp.Nickname == currentNickname) ? rp.Player.ReproductiveStatus : null,
            Age = (rp.IsAgeRevealed || rp.Nickname == currentNickname) ? rp.Player.Age : null,
            Orientation = (rp.IsGenderRevealed || rp.Nickname == currentNickname) ? rp.Player.Orientation : null,
            Hobby = (rp.IsHobbyRevealed || rp.Nickname == currentNickname) ? rp.Player.Hobby : null,
            Phobia = (rp.IsPhobiaRevealed || rp.Nickname == currentNickname) ? rp.Player.Phobia : null,
            Luggage = (rp.IsLuggageRevealed || rp.Nickname == currentNickname) ? rp.Player.Luggage : null,
            AdditionalInformation = (rp.IsAdditionalInfoRevealed || rp.Nickname == currentNickname) ? rp.Player.AdditionalInformation : null,
            BodyType = (rp.IsBodyTypeRevealed || rp.Nickname == currentNickname) ? rp.Player.BodyType : null,
            Health = (rp.IsHealthRevealed || rp.Nickname == currentNickname) ? rp.Player.Health : null,
            IsGenderRevealed = rp.IsGenderRevealed,
            IsAgeRevealed = rp.IsAgeRevealed,
            IsOrientationRevealed = rp.IsOrientationRevealed,
            IsHobbyRevealed = rp.IsHobbyRevealed,
            IsPhobiaRevealed = rp.IsPhobiaRevealed,
            IsLuggageRevealed = rp.IsLuggageRevealed,
            IsAdditionalInfoRevealed = rp.IsAdditionalInfoRevealed,
            IsBodyTypeRevealed = rp.IsBodyTypeRevealed,
            IsHealthRevealed = rp.IsHealthRevealed,
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