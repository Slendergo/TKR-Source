using Newtonsoft.Json;
using System;

namespace common.database
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
