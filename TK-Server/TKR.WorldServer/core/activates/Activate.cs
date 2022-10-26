using NLog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TKR.Shared;
using TKR.Shared.resources;
using TKR.WorldServer.core.objects;
using TKR.WorldServer.core.structures;
using TKR.WorldServer.utils;

namespace TKR.WorldServer.core.activates
{
    public enum CenterTypes : byte
    {
        player,
        mouse
    }

    public enum TargetTypes : byte
    {
        player,
        enemy,
    }

    public abstract class ActivateCreatorBase
    {
        public abstract Activate Create(Entity go, XElement options);
    }

    public class ActivateCreator<T> : ActivateCreatorBase where T : Activate
    {
        public bool ReturnsBool { get; private set; }

        public ActivateCreator(bool returnsBool)
        {
            ReturnsBool = returnsBool;
        }

        public override Activate Create(Entity go, XElement options) => (Activate)Activator.CreateInstance(typeof(T), go, options, ReturnsBool);
    }

    public class Activate
    {
        private static Dictionary<string, ActivateCreatorBase> ActivateNameMap { get; set; }

        protected Player Host { get; }
        protected XElement Options { get; set; }
        public bool ReturnsBool { get; }

        public Activate(Player host, XElement options, bool returnsBool)
        {
            Host = host;
            Options = options;
            ReturnsBool = returnsBool;
        }

        public static Activate New(XElement kind, Player host)
        {
            if (ActivateNameMap == null)
                ActivateNameMap = new Dictionary<string, ActivateCreatorBase>()
                {
                    { "BulletNova", new ActivateCreator<BulletNovaActivate>(false) },
                    { "IncrementStat", new ActivateCreator<IncrementStatActivate>(true) }
                };

            if (!ActivateNameMap.ContainsKey(kind.Value))
            {
                //host.GameWorld.ChatManager.SendInfo($"Unrecognized Activate: {kind.Value}", host);
                StaticLogger.Instance.Warn($"ERROR: Unrecognized Activate: {kind.Value}");
                return null;
            }

            return ActivateNameMap[kind.Value].Create(host, kind);
        }

        public virtual void OnEquip(Item item) { }
        public virtual void Execute(Item item, ref Position usePosition) { }
        public virtual bool ExecuteBool(Item item, ref Position usePosition) => true;

        protected double ModifyWisMod(double input, int power = 1)
        {
            var wisdom = Host.Stats[6];

            double x;
            if (wisdom < 30)
                x = input;
            else
            {
                var dx = input < 0 ? -1 : 1; //expect 1
                var dy = input * wisdom / 150.0 + input * dx; // expects 7.5
                dy = Math.Floor(dy * Math.Pow(10, power)) / Math.Pow(10, power); //remains 7.5
                x = double.Parse(dy - (int)dy * dx >= 1 / Math.Pow(10, power) * dx ? dy.ToString("N1") : dy.ToString("N0"));
            }
            return x;
        }

        protected static double GetFloatArgument(XElement options, string name, float defaultValue) => options.GetAttribute(name, defaultValue);
        protected static int GetIntArgument(XElement options, string name, int defaultValue) => options.GetAttribute(name, defaultValue);
        protected static uint GetHexIntArgument(XElement elem, string name, uint defaultValue)
        {
            var attr = elem.Attribute(name);
            return attr != null ? uint.Parse(attr.Value.StartsWith("0x") ? attr.Value[2..] : attr.Value.StartsWith("#") ? attr.Value[1..] : attr.Value, NumberStyles.HexNumber) : defaultValue;
        }
        protected static bool GetBooleanArgument(XElement options, string name, bool defaultValue)
        {
            var o = options.Attribute(name);
            return o == null ? defaultValue : o.Value != "0" && o.Value != "false";
        }
        protected static string GetStringArgument(XElement options, string name, string defaultValue) => options.GetAttribute(name, defaultValue);
    }
}