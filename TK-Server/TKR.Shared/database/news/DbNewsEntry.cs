using Newtonsoft.Json;
using System;

namespace TKR.Shared.database.news
{
    public struct DbNewsEntry
    {
        [JsonIgnore] public DateTime Date;

        public string Icon;
        public string Link;
        public string Text;
        public string Title;
    }
}
