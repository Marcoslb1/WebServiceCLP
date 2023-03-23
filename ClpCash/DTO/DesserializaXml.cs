using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace ClpCash.DTO
{
    [XmlRoot(ElementName = "var")]
    public class Var
    {
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "value")]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "data")]
    public class Data
    {
        [XmlElement(ElementName = "var")]
        public Var Var { get; set; }
    }

}