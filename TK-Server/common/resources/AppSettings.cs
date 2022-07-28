using System.Xml.Linq;

namespace common.resources
{
    public class AppSettings
    {        
        public readonly string MenuMusic;
        public readonly string DeadMusic;
        public readonly int MaxStackablePotions;
        public readonly int UseExternalPayments;


        public NewAccounts NewAccounts;
        public NewCharacters NewCharacters;
        public XElement Xml;

        public AppSettings(string dir)
        {
            XElement e = XElement.Parse(Utils.Read(dir));

            Xml = e;
            MenuMusic = e.GetValue<string>("MenuMusic");
            DeadMusic = e.GetValue<string>("DeadMusic");
            UseExternalPayments = e.GetValue<int>("UseExternalPayments");
            MaxStackablePotions = e.GetValue<int>("MaxStackablePotions");


            var newAccounts = e.Element("NewAccounts");
            NewAccounts = new NewAccounts(e.Element("NewAccounts"));
            newAccounts.Remove(); // don't export with /app/init

            var newCharacters = e.Element("NewCharacters");
            NewCharacters = new NewCharacters(e.Element("NewCharacters"));
            newCharacters.Remove();
        }
    }
}
