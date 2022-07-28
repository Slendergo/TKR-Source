namespace common.database
{
    public class DbInfo
    {
        public string auth { get; set; } = "";
        public string host { get; set; } = "127.0.0.1";
        public int index { get; set; } = 0;
        public int port { get; set; } = 6379;
    }
}
