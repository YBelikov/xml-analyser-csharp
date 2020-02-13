using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Runtime;
using System.Xml;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XMLAnalyser
{

    class Planet
    {
        public string name;
        public string equatorialRadius;
        public string massInEarthsMass;
        public string numberOfNaturalSatellites;
        public Planet() { }
        public Planet(string[] data)
        {
            name = data[2];
            equatorialRadius = data[0];
            massInEarthsMass = data[1];
            numberOfNaturalSatellites = data[3];
        }
        #region Comparison
        public bool Compare(Planet planet)
        {
            if (this.name == planet.name && this.equatorialRadius == planet.equatorialRadius && this.massInEarthsMass == planet.massInEarthsMass
                && this.numberOfNaturalSatellites == planet.numberOfNaturalSatellites) return true;
            else return false;
        }
        #endregion Comparison

    }
    interface IStrategy
        {
            List<Planet> Algorithm(Planet planet, string path);
        }

    class LINQ : IStrategy
    {
        List<Planet> result = new List<Planet>();
        XDocument doc = new XDocument();
        public LINQ(string path)
        {
            doc = XDocument.Load(path);
        }
        public List<Planet> Algorithm(Planet planet, string path)
        {
            List<XElement> matches = (from val in doc.Descendants("Planet")
                                      where ((planet.name == null || planet.name == val.Attribute("Name").Value) &&
                                      (planet.equatorialRadius == null || planet.equatorialRadius == val.Attribute("EquatorialRadius").Value) &&
                                      (planet.massInEarthsMass == null || planet.massInEarthsMass == val.Attribute("MassInEarthMass").Value) &&
                                      (planet.numberOfNaturalSatellites == null || planet.numberOfNaturalSatellites == val.Attribute("NumberOfNaturalSatellites").Value))
                                      select val).ToList();
            foreach(XElement element in matches)
            {
                try
                {
                    Planet p = new Planet
                    {
                        name = element.Attribute("Name").Value,
                        equatorialRadius = element.Attribute("EquatorialRadius").Value,
                        massInEarthsMass = element.Attribute("MassInEarthMass").Value,
                        numberOfNaturalSatellites = element.Attribute("NumberOfNaturalSatellites").Value,

                    };
                    result.Add(p);
                }
                catch { }
            }
            return result;
        }
    }

    class DOM : IStrategy
    {
        XmlDocument doc = new XmlDocument();
        public DOM(string path)
        {
            doc.Load(path);
        }
        public List<Planet> Algorithm(Planet planet, string path)
        {
            List<List<Planet>> result = new List<List<Planet>>();
            if((planet.name == null) && (planet.equatorialRadius == null) && (planet.massInEarthsMass == null) && (planet.numberOfNaturalSatellites == null))
            {
                return Error(doc);
            }
            if (planet.name != null) result.Add(SearchByAttribute("Planet", "Name", planet.name, doc));
            if (planet.equatorialRadius != null) result.Add(SearchByAttribute("Planet", "EquatorialRadius", planet.equatorialRadius, doc));
            if (planet.massInEarthsMass != null) result.Add(SearchByAttribute("Planet", "MassInEarthMass", planet.massInEarthsMass, doc));
            if (planet.numberOfNaturalSatellites != null) result.Add(SearchByAttribute("Planet", "NumberOfNaturalSatellites", planet.numberOfNaturalSatellites, doc));
            return Cross(result, planet);
        }
        public List<Planet> Error(XmlDocument doc)
        {
            List<Planet> result = new List<Planet>();
            XmlNodeList list = doc.SelectNodes("//" + "Planet");
            foreach(XmlNode node in list)
            {
                result.Add(Info(node));
            }
            return result;
        }
        public Planet Info(XmlNode node)
        {
            Planet planet = new Planet();
            planet.name = node.Attributes.GetNamedItem("Name").Value;
            planet.equatorialRadius = node.Attributes.GetNamedItem("EquatorialRadius").Value;
            planet.massInEarthsMass = node.Attributes.GetNamedItem("MassInEarthMass").Value;
            planet.numberOfNaturalSatellites = node.Attributes.GetNamedItem("NumberOfNaturalSatellites").Value;
            return planet;
        }
        public List<Planet> SearchByAttribute(string nodeName, string attribute, string myTemplate, XmlDocument doc)
        {
            List<Planet> find = new List<Planet>();
            if(myTemplate != null)
            {
                XmlNodeList list = doc.SelectNodes("//" + nodeName + "[@" + attribute + "=\"" + myTemplate + "\"]");
                foreach(XmlNode e in list)
                {
                    find.Add(Info(e));
                }
            }
            return find;
        }
        public List<Planet> Cross(List<List<Planet>> list, Planet planet)
        {
            List<Planet> result = new List<Planet>();
            List<Planet> clear = CheckNodes(list, planet);
            foreach(Planet pl in clear)
            {
                bool isIn = false;
                foreach(Planet p in result)
                {
                    if (p.Compare(pl))
                    {
                        isIn = true;
                    }
                }
                if (!isIn) result.Add(pl);
            }
            return result;
        }
        public List<Planet> CheckNodes(List<List<Planet>> list, Planet planet)
        {
            List<Planet> newResult = new List<Planet>();
            foreach(List<Planet> elem in list)
            {
                foreach(Planet p in elem)
                {
                    if ((planet.name == p.name || planet.name == null) && (planet.equatorialRadius == p.equatorialRadius || planet.equatorialRadius == null) &&
                        (planet.massInEarthsMass == p.massInEarthsMass || planet.massInEarthsMass == null) && (planet.numberOfNaturalSatellites == p.numberOfNaturalSatellites || planet.numberOfNaturalSatellites == null)
                        )
                    {
                        newResult.Add(p);
                    }
                }
            }
            return newResult;
        }
    }

    class SAX : IStrategy
    {
        private List<Planet> lastResult = null;
        private XmlReader reader;
        public SAX(string path)
        {
            reader = XmlReader.Create(path);
        }

        public List<Planet> Algorithm(Planet planet, string path)
        {
            List<Planet> result = new List<Planet>();
            Planet pl = null;
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (reader.Name == "Planet")
                        {
                            pl = new Planet();
                            while (reader.MoveToNextAttribute())
                            {
                                if (reader.Name == "Name")
                                {
                                    pl.name = reader.Value;

                                }
                                else if (reader.Name == "EquatorialRadius")
                                {
                                    pl.equatorialRadius = reader.Value;
                                }
                                else if (reader.Name == "MassInEarthMass")
                                {
                                    pl.massInEarthsMass = reader.Value;
                                }
                                else if (reader.Name == "NumberOfNaturalSatellites")
                                {
                                    pl.numberOfNaturalSatellites = reader.Value;
                                }


                            }
                            result.Add(pl);
                            
                        }
                        break;
                }
            }
            lastResult = Filter(result, planet);
            return lastResult;
        }
        public List<Planet> Filter(List<Planet> allRes, Planet planet)
        {
            List<Planet> newResult = new List<Planet>();
            if(allRes != null)
            {
                foreach(Planet p in allRes)
                {
                    if((p.name == planet.name || planet.name == null) &&(p.equatorialRadius == planet.equatorialRadius || planet.equatorialRadius == null)
                        &&(p.massInEarthsMass == planet.massInEarthsMass || planet.massInEarthsMass == null) &&
                        (p.numberOfNaturalSatellites == planet.numberOfNaturalSatellites || planet.numberOfNaturalSatellites == null))
                    {
                        newResult.Add(p);
                    }
                }
            }
            return newResult;
        }
    }
    
        

    


    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
