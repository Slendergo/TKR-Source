using common.discord.model;
using common.isc.data;
using System.Linq;
using System.Threading.Tasks;

namespace common.discord
{
    public static class DiscordExtensions
    {
        public static bool CanSendLootNotification(this DiscordIntegration discord, int stars, string className) => discord.classes.Any(_ => _.name.ToLower().Equals(className)) && discord.stars.FindStar(stars).HasValue;

        public static bool CanSendRealmEventNotification(this DiscordIntegration discord, string eventName) => discord.realmEvents.Any(realmEvent => realmEvent.name.Equals(eventName));

        public static StarModel? FindStar(this StarModel[] stars, int star)
        {
            for (var i = 0; i < stars.Length; i++)
                if (stars[i].range.Length == 1)
                {
                    // white star -> 70 stars
                    if (star == stars[i].range[0])
                        return stars[i];
                }
                else
                {
                    if (star >= stars[i].range[0] && star <= stars[i].range[1])
                        return stars[i];
                }

            return null;
        }

        public static DiscordEmbedBuilder MakeEventBuilder(this DiscordIntegration discord, string serverName, string worldName, string eventName)
        {
            var realmEvent = discord.realmEvents.First(rEvent => rEvent.name.Equals(eventName));

            return new DiscordEmbedBuilder
            {
                Username = discord.botName,
                Avatar = discord.webhookResourcesURL + discord.webhookBotImage,
                Content = $"<@&{realmEvent.id}>",
                Embeds = new[]
                {
                    new DiscordEmbed()
                    {
                        Author = new DiscordEmbedAuthor()
                        {
                            Name = serverName,
                            Icon = discord.webhookResourcesURL + discord.webhookOryx1Image
                        },
                        Title = "Realm event spawned!",
                        Description = $"Oryx spawned **{eventName}** on realm __{worldName}__!",
                        Color = 0x7289DA,
                        Thumbnail = new DiscordThumbnail() { Url = discord.webhookResourcesURL + discord.webhookLogoImage },
                        Image = new DiscordImage() { Url = discord.webhookResourcesURL + realmEvent.image },
                        Footer = new DiscordEmbedFooter()
                        {
                            Text = $"{discord.botName} - Game Notification!",
                            Icon = discord.webhookResourcesURL + discord.webhookBotImage
                        }
                    }
                }
            };
        }

        public static DiscordEmbedBuilder? MakeLootBuilder(
            this DiscordIntegration discord,
            ServerInfo info,
            string worldName,
            int players,
            int maxPlayers,
            bool isDungeon,
            string rarity,
            string bagImage,
            string bagBanner,
            string playerName,
            int rank,
            int stars,
            string itemName,
            string className,
            int level,
            int fame,
            int maxedStats
            )
        {
            var star = discord.stars.FindStar(stars);

            if (!star.HasValue)
            {
                logger.Log.Error($"Unhandled star value: {stars}");
                return null;
            }

            var cInfo = discord.classes.First(classInfo => classInfo.name.Equals(className.ToLower()));

            return new DiscordEmbedBuilder()
            {
                Username = discord.botName,
                Avatar = discord.webhookResourcesURL + discord.webhookBotImage,
                Content = "",
                Embeds = new[]
                {
                    new DiscordEmbed()
                    {
                        Author = new DiscordEmbedAuthor()
                        {
                            Name = $"{rarity} Loot!",
                            Icon = discord.webhookResourcesURL + bagImage,
                        },
                        Title = $"<:{star.Value.name}:{star.Value.id}> (D-{rank / 10}) {playerName}",
                        Description = $"Player looted **{itemName}** at {worldName}, __{info.name}__!",
                        Color = 0x7289DA,
                        Fields = new[]
                        {
                            new  DiscordEmbedField()
                            {
                                Name = "In server:",
                                Value = $"{info.players}/{info.maxPlayers}",
                                InLine = true
                            },
                            new DiscordEmbedField()
                            {
                                Name = $"In {(isDungeon ? "dungeon" : "realm")}:",
                                Value = $"{players}/{maxPlayers}",
                                InLine = true
                            },
                            new DiscordEmbedField()
                            {
                                Name = "Class:",
                                Value = $"{className} <:{cInfo.name}:{cInfo.id}>",
                                InLine = true
                            },
                            new DiscordEmbedField()
                            {
                                Name = "Level:",
                                Value = level.ToString(),
                                InLine = true
                            },
                            new DiscordEmbedField()
                            {
                                Name = "Fame:",
                                Value = fame.ToString(),
                                InLine = true
                            },
                            new DiscordEmbedField()
                            {
                                Name = "Stats:",
                                Value = $"{maxedStats}/16",
                                InLine = true
                            }
                        },
                        Thumbnail = new DiscordThumbnail() { Url = discord.webhookResourcesURL + discord.webhookLogoImage },
                        Image = new DiscordImage() { Url = discord.webhookResourcesURL + bagBanner },
                        Footer = new DiscordEmbedFooter()
                        {
                            Text = $"{discord.botName} - Game Notification!",
                            Icon = discord.webhookResourcesURL + discord.webhookBotImage
                        }
                    }
                }
            };
        }

        public static DiscordEmbedBuilder MakeOryxBuilder(this DiscordIntegration discord, ServerInfo info, string worldName, int players, int maxPlayers)
        {
            var oryx = discord.realmEvents.First(realmEvent => realmEvent.name.Equals("Oryx"));

            return new DiscordEmbedBuilder
            {
                Username = discord.botName,
                Avatar = discord.webhookResourcesURL + discord.webhookBotImage,
                Content = $"<@&{oryx.id}>",
                Embeds = new[]
                {
                    new DiscordEmbed()
                    {
                        Author = new DiscordEmbedAuthor()
                        {
                            Name = info.name,
                            Icon = discord.webhookResourcesURL + discord.webhookOryx2Image
                        },
                        Title = "Realm is closing!",
                        Description = $"Oryx is preparing to close realm __{worldName}__ soon!",
                        Color = 0x7289DA,
                        Fields = new[]
                        {
                            new DiscordEmbedField()
                            {
                                Name = "In server:",
                                Value = $"{info.players}/{info.maxPlayers}",
                                InLine = true
                            },
                            new DiscordEmbedField()
                            {
                                Name = "In realm:",
                                Value = $"{players}/{maxPlayers}",
                                InLine = true
                            }
                        },
                        Thumbnail = new DiscordThumbnail() { Url = discord.webhookResourcesURL + discord.webhookLogoImage },
                        Image = new DiscordImage() { Url = discord.webhookResourcesURL + oryx.image },
                        Footer = new DiscordEmbedFooter()
                        {
                            Text = $"{discord.botName} - Game Notification!",
                            Icon = discord.webhookResourcesURL + discord.webhookBotImage
                        }
                    }
                }
            };
        }

        public static async Task SendWebhook(this DiscordIntegration discord, string webhook, DiscordEmbedBuilder builder)
        {
            using (var discordWebhook = new DiscordWebhook(webhook))
                await discordWebhook.SendAsync(builder);
        }
    }
}
