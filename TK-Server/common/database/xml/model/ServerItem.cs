using System.Xml.Linq;

namespace common.database.xml.model
{
    public struct ServerItem
    {
        public bool AdminOnly { get; set; }
        public string DNS { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public string Name { get; set; }
        public int Port { get; set; }
        public double Usage { get; set; }
        public string UsageText { get; set; }

        public XElement ToXml() => new XElement("Server",
            new XElement("Name", Name),
            new XElement("DNS", DNS),
            new XElement("Port", Port),
            new XElement("Lat", Lat),
            new XElement("Long", Long),
            new XElement("Usage", Usage),
            new XElement("AdminOnly", AdminOnly),
            new XElement("UsageText", UsageText)
        );
    }
}
