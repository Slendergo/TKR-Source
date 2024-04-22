using System.Linq;
using System.Threading.Tasks;
using TKR.Shared.discord.model;

namespace TKR.Shared.discord
{
    public static class DiscordExtensions
    {
        public static bool CanSendLootNotification(this DiscordIntegration discord, int stars, string className) => discord.classes.Any(_ => _.name.ToLower().Equals(className)) && discord.stars.FindStar(stars).HasValue;

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
            RankingType rank,
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

            var cInfo = discord.classes.FirstOrDefault(classInfo => classInfo.name.Equals(className.ToLower()));
            if (string.IsNullOrWhiteSpace(cInfo.name))
            {
                logger.Log.Error($"Invalid Class Emoji: {className}");
                cInfo = discord.classes.FirstOrDefault(classInfo => classInfo.name.Equals("wizard"));
            }

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
                        Title = $"<:{star.Value.name}:{star.Value.id}> {(rank > RankingType.Regular && rank <= RankingType.Supporter5 ? $"(Supporter #{rank})" : "")}) {playerName}",
                        Description = $"Player looted **{itemName}** at {worldName}, __{info.name}__!",
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

        public static DiscordEmbedBuilder? MakeDeathAnnounce(
            this DiscordIntegration discord,
            ServerInfo info,
            int players,
            int maxPlayers,
            bool isDungeon,
            string bagImage,
            string playerName,
            int rank,
            int stars,
            string className,
            int level,
            int fame,
            bool upgradeEnabled,
            int maxedStats,
            string killer)
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
                            Name = $"RIP!",
                            Icon = discord.webhookResourcesURL + bagImage,
                        },
                        Title = $"<:{star.Value.name}:{star.Value.id}> (D-{rank / 10}) {playerName}",
                        Description = $"Has been killed by __**{killer}**__!",
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
                                Value = $"{maxedStats}{(upgradeEnabled ? "/16" : "/8")}",
                                InLine = true
                            }
                        },
                        Thumbnail = new DiscordThumbnail() { Url = discord.webhookResourcesURL + discord.webhookLogoImage },
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
            if (string.IsNullOrWhiteSpace(webhook))
                return;                
            using var discordWebhook = new DiscordWebhook(webhook);
            await discordWebhook.SendAsync(builder);
        }
    }
}
