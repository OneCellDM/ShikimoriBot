using BotShared.ShikimoriSharp.Classes;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using VkNet;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;
using BotShared.ShikimoriSharp.Information;
using System.Xml.Linq;
using VkNet.Model.Keyboard;
using VkNet.Model.RequestParams.Market;
using BotShared;

public class Program
{
    public static BotMessageHandler botMessageHandler;
    public static async Task Main(string[] args)
    {
        botMessageHandler = new BotMessageHandler(new BotClient(151115257));

        var startRes = false;

        while (startRes == false)
        {
          
                await botMessageHandler.Init();
                startRes = await botMessageHandler.Start();
        }


        while (true)
        {
            Console.WriteLine("/stop остановить бота");
            if (Console.ReadLine() == "/stop")
            {
                await botMessageHandler.Stop();
                break;

            }
        }
    }

    public class BotClient : IBot<VkApi>
    {

        private KeyboardBuilder KeyboardBuilder = new KeyboardBuilder();

        private Random random = new Random();
        private bool LpStart = true;
        private string[] _supportedPhotoTypes = new[] { "jpg", "jpeg", "png" };

        public event IBot.LongPoll MessageLongPollEvent;

        private ulong PubId { get; set; }
        public VkApi BotApi { get; set; }

        public BotClient(ulong publicId)
        {
           
            this.PubId = publicId;
        }

      
        public async Task Init()
        {
            var token = File.ReadAllText("Token.txt");
            BotApi = new VkApi();
            await BotApi.AuthorizeAsync(new ApiAuthParams()
            {
                Settings= VkNet.Enums.Filters.Settings.All,
                AccessToken = token,
            });

        }
        private async Task SendMessage(long chatId, string message)
        {
            await BotApi.Messages.SendAsync(new MessagesSendParams()
            {
                PeerId = chatId,
                Message = message,
                RandomId = random.NextInt64()
            });
        }
        private async Task SendPhotos(long chatId, string message, string[] photoUrls)
        {



            

            var attachments = new List<MediaAttachment>();

                foreach (var photoUrl in photoUrls)
                {

                    string fileName = photoUrl?.Split('/')?.Last()?.Split('?')?.FirstOrDefault();

                    var fileExtension = Path.GetExtension(fileName).Remove(0, 1);

                    if (_supportedPhotoTypes.Contains(fileExtension) == false)
                        continue;

                    var uploadServer = await BotApi.Photo.GetMessagesUploadServerAsync((long?)PubId);

                    using (HttpClient httpClient = new HttpClient())
                    {
                        var photoStream = await httpClient.GetStreamAsync(photoUrl);

                        using (var multipartFormContent = new MultipartFormDataContent())
                        {

                            var photoStreamContent = new StreamContent(photoStream);

                            photoStreamContent.Headers.ContentType = new MediaTypeHeaderValue($"image/{fileExtension}");


                            multipartFormContent.Add(photoStreamContent, name: "file", fileName: fileName);


                            var response = await httpClient.PostAsync(uploadServer.UploadUrl, multipartFormContent);
                            if (response.IsSuccessStatusCode)
                            {
                                var readData = await response.Content.ReadAsStringAsync();
                                var savedPhoto = await BotApi.Photo.SaveMessagesPhotoAsync(readData);
                                if (savedPhoto != null && savedPhoto.Count > 0)
                                {
                                    attachments.Add(savedPhoto.First());
                                }
                            }

                        }
                    }

                }

                if (attachments.Count > 0)
                {

                    await BotApi.Messages.SendAsync(new MessagesSendParams()
                    {
                        PeerId = chatId,
                        Message = message,
                        RandomId = random.NextInt64(),
                        Attachments = attachments

                    });
                }
                else  await SendMessage(chatId, message);
           
            
        }

        public async Task SendAnimeDetailAsync(long chatId, long animeId)
        {
           
                var anime = await ShikimoriApiHandler.GetAnimeInfo(animeId);

                if (anime.Item2 != null)
                {
                    await SendPhotos(chatId, anime.Item1, anime.Item2.ToArray());
                }
                else await SendMessage(chatId, anime.Item1);
            
           


        }

        public async Task SendMangaDetailAsync(long chatId, long mangaId)
        {
           
                var manga = await ShikimoriApiHandler.GetMangaInfo(mangaId);

                if (manga.poster != null)
                {
                    await SendPhotos(chatId, manga.text, new[] { manga.poster });
                }
                else await SendMessage(chatId, manga.text);
          

        }

        public async Task SendRandomAnimeAsync(long chatId)
        {
           
            var anime =  await ShikimoriApiHandler.GetRandomAnime();
            await BotApi.Messages.SendAsync(new MessagesSendParams()
            {
                PeerId = chatId,
                Message = anime.Item2,
                Keyboard = new KeyboardBuilder()
                            .AddButton(BotConstans.GetAnime.Title,
                                       $"{BotConstans.GetAnime.Command} {anime.Item1}")
                            .SetInline(true).Build(),
                RandomId = random.NextInt64()
            }) ;
        }

        public async Task SendRandomMangaAsync(long chatId)
        {
             
            var anime = await ShikimoriApiHandler.GetRandomManga();
            await BotApi.Messages.SendAsync(new MessagesSendParams()
            {
                PeerId = chatId,
                Message = anime.Item2,
                Keyboard = new KeyboardBuilder()
                            .AddButton(BotConstans.GetManga.Title,
                                       $"{BotConstans.GetManga.Command} {anime.Item1}")
                            .SetInline(true).Build(),

                RandomId = random.NextInt64(),
            });
        }
        public async Task SendDefaultMessageAsync(long chatId)
        {
            MessageKeyboard keyboard = new KeyboardBuilder()
                .AddButton(BotConstans.RandomAnime.Title, BotConstans.RandomAnime.Command)
                .SetInline(true)
                .AddButton(BotConstans.RandomManga.Title, BotConstans.RandomManga.Command)
                .SetInline(true)
                .Build();
           
           

            

            await BotApi.Messages.SendAsync(new MessagesSendParams()
            {
                PeerId = chatId,
                Message= BotConstans.InputAction,
                RandomId = random.NextInt64(),
                Keyboard=keyboard,
            });
        }
        public async Task<bool> Start()
        {
            LpStart = true;
            try
            {
                _= Task.Run(async () =>
                {
                    while (LpStart) // Бесконечный цикл, получение обновлений
                    {
                        try
                        {
                            var s = BotApi.Groups.GetLongPollServer(PubId);
                            var poll = BotApi.Groups.GetBotsLongPollHistory(
                               new BotsLongPollHistoryParams()
                               { Server = s.Server, Ts = s.Ts, Key = s.Key, Wait = 25 });

                            if (poll?.Updates == null) continue;

                            foreach (var a in poll.Updates)
                            {
                                if (a != null)
                                {

                                    if (a.Type == GroupUpdateType.MessageNew)
                                    {
                                        string? message = null;
                                        long? userID = null;
                                        var msgNew = a.MessageNew?.Message;

                                        if (msgNew != null)
                                        {
                                            userID = msgNew.PeerId;
                                            if (string.IsNullOrEmpty(msgNew.Payload))
                                            {
                                                message = msgNew.Text;

                                            }
                                            else
                                            {
                                                message = msgNew.Payload.Split(":").Last();
                                                var start = message.IndexOf("/");
                                                var end = message.IndexOf('"', start);
                                                message = message.Substring(start, end - start);


                                            }
                                        }

                                        if (string.IsNullOrEmpty(message) || userID == null)
                                        {
                                            continue;
                                        }

                                        MessageLongPollEvent?.Invoke(userID, message);


                                    }
                                }
                            }
                        }
                        catch(Exception ex) {
                            Console.WriteLine("Error: " + ex.Message);
                        }
                    }
                });
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
           
        }

        public async Task<bool> Stop()
        {
            LpStart = false;
            return true;
        }

      
    }
}
