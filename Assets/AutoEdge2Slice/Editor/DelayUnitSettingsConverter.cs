using System;
using System.Xml;
using System.Xml.Linq;

namespace AutoEdge2Slice.Editor
{
    public static class DelayUnitSettingsConverter
    {
        internal static float ToUnit(XDocument document)
        {
            var delayUnitXElement = document.Root?.Element("DelayUnit");
            if (delayUnitXElement == null) throw new XmlException();
            var delayUnitNumber = int.Parse(delayUnitXElement.Value);
            return ToUnit(delayUnitNumber);
        }
        
        internal static float ToUnit(int delayUnitNumber)
        {
            return delayUnitNumber switch
            {
                2 => 1000,
                1 => 100,
                0 => 60,
                7 => 30,
                6 => 24,
                5 => 20,
                4 => 15,
                3 => 10,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}