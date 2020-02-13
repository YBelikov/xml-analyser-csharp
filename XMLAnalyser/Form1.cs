using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Xsl;
using System.Xml;

namespace XMLAnalyser
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string filePath = @"D:\CSharp\XMLAnalyser\XMLAnalyser\planets.xml";
        List<Planet> planets = new List<Planet>();

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FillAllDropDownLists();
        }

        private void FillAllDropDownLists()
        {
            
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            XmlElement root = doc.DocumentElement;
            XmlNodeList nodes = root.SelectNodes("Planet");
            for(int i = 0; i < nodes.Count; ++i)
            {
                XmlNode n = nodes.Item(i);
                FillItems(n);
            }
        }
        private void FillItems(XmlNode node)
        {
            if (!comboBox1.Items.Contains(node.SelectSingleNode("@EquatorialRadius").Value))
            {
                comboBox1.Items.Add(node.SelectSingleNode("@EquatorialRadius").Value);
            }

            if (!comboBox2.Items.Contains(node.SelectSingleNode("@MassInEarthMass").Value))
            {
                comboBox2.Items.Add(node.SelectSingleNode("@MassInEarthMass").Value);
            }

            if (!comboBox3.Items.Contains(node.SelectSingleNode("@Name").Value))
            {
                comboBox3.Items.Add(node.SelectSingleNode("@Name").Value);
            }

            if (!comboBox4.Items.Contains(node.SelectSingleNode("@NumberOfNaturalSatellites").Value))
            {
                comboBox4.Items.Add(node.SelectSingleNode("@NumberOfNaturalSatellites").Value);
            }


        }
        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            Planet planet = CreatePlanet();
            if (radioButton1.Checked)
            {
                IStrategy currentStrategy = new DOM(filePath);
                planets = currentStrategy.Algorithm(planet, filePath);
                Output(planets);
            }else if (radioButton3.Checked)
            {
                IStrategy currentStrategy = new LINQ(filePath);
                planets = currentStrategy.Algorithm(planet, filePath);
                Output(planets);

            }else if (radioButton2.Checked)
            {
                IStrategy currentStrategy = new SAX(filePath);
                planets = currentStrategy.Algorithm(planet, filePath);
                Output(planets);
            }

        }
        private Planet CreatePlanet()
        {
            string[] parameters = new string[4];
            if (checkBox1.Checked) parameters[0] = Convert.ToString(comboBox1.Text);
            if (checkBox2.Checked) parameters[1] = Convert.ToString(comboBox2.Text);
            if (NameBox.Checked) parameters[2] = Convert.ToString(comboBox3.Text);
            if (checkBox4.Checked) parameters[3] = Convert.ToString(comboBox4.Text);
            return new Planet(parameters);
        }
        private void Output(List<Planet> planets)
        {
            int i = 0;
            foreach(Planet p in planets)
            {
                richTextBox1.AppendText(++i + "." + '\n');
                richTextBox1.AppendText("Name: " + p.name + '\n');
                richTextBox1.AppendText("Equatorial radius: " + p.equatorialRadius + '\n');
                richTextBox1.AppendText("Mass in Earth masses: " + p.massInEarthsMass + '\n');
                richTextBox1.AppendText("Number of natural satellites: " + p.numberOfNaturalSatellites + '\n');
                richTextBox1.AppendText("=======================================================\n");
            }
        }

        private void Clear_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            NameBox.Checked = false;
            checkBox4.Checked = false;
            comboBox3.Text = null;
            comboBox1.Text = null;
            comboBox2.Text = null;
            comboBox4.Text = null;
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!comboBox1.Enabled) comboBox1.Enabled = true;
            else
            {
                comboBox1.Text = "";
                comboBox1.Enabled = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (!comboBox2.Enabled) comboBox2.Enabled = true;
            else
            {
                comboBox2.Text = "";
                comboBox2.Enabled = false;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (!comboBox4.Enabled) comboBox4.Enabled = true;
            else
            {
                comboBox4.Enabled = false;
                comboBox4.Text = "";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Transform();
        }
        private void Transform()
        {
            string xslPath = @"D:\CSharp\XMLAnalyser\XMLAnalyser\XSLTFile1.xslt";
            string xmlPath = @"D:\CSharp\XMLAnalyser\XMLAnalyser\planets.xml";
            string htmlPath = @"D:\CSharp\XMLAnalyser\XMLAnalyser\planets.html";
            XslCompiledTransform xct = new XslCompiledTransform();
            xct.Load(xslPath);
            xct.Transform(xmlPath, htmlPath);
        }

        private void NameBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!comboBox3.Enabled) comboBox3.Enabled = true;
            else
            {
                comboBox3.Enabled = false;
                comboBox3.Text = "";
            }

        }
    }
}

