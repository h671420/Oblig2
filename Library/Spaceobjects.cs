using System;
using System.Drawing;

using OfficeOpenXml;
//using OfficeOpenXml.Style;
//using System.Runtime.InteropServices;
//using static OfficeOpenXml.ExcelErrorValue;
//using System.Xml.Linq;

namespace SpaceSim
{
    public class SpaceObject
    {
        public string name { get; set; }
        public int radius { get; set; }
        public Color color { get; set; }
        public int xPos { get; set; }
        public int yPos { get; set; }

        public SpaceObject(string name)
        {
            this.name = name;
        }
        public virtual void printInfo() {
            Console.Write("Hello from spaceobject! ");
        }
    }
    public class Rotational:SpaceObject
    {       
        public int orbitalRadius {  get; set; }
        public int orbitalPeriod { get; set; }
        
        public int rotationalPeriod { get; set; }
        
        public SpaceObject? anchor { get; set; }

        public Rotational(string name): base(name){}
        public override void printInfo() {
            base.printInfo();
            Console.Write("Hello from rotational! ");
        }

        /*
        public void Orbitalcalc(double years)
        {
            double d = years / (2 * Math.PI);
            //tid = 0 dager
            xVal = anchor.xVal + this.orbitalRadius * (int)Math.Sin(d);
            yVal = anchor.yVal + this.orbitalRadius * (int)Math.Cos(d);

        }
        */
    }
    public class Star : Rotational
    {
        public Star(String name):base(name) { }
        public override void printInfo()
        {
            base.printInfo();
            Console.WriteLine("Hello from Star: " + name);
        }

    }
    public class Planet : Rotational
    {
        public Planet(String name) : base(name) { }
        public override void printInfo()
        {
            base.printInfo();
            Console.WriteLine("Hello from Planet: " + name);
        }
    }
    public class Moon : Rotational
    {
        public Moon(String name) : base(name) { }
        public override void printInfo()
        {
            base.printInfo();
            Console.WriteLine("Hello from Moon: "+name);
            
        }
    }

    public class Utils
    {
        public static List<SpaceObject> fetchSpaceObjects()
        {
            string filepath = "C:\\Users\\Admin\\OneDrive\\Skrivebord\\Skolegreier\\V2024\\DAT154\\Assignments\\Assignment 2\\Planets.xlsx";
            List<SpaceObject> spaceObjects = new();

            ExcelWorksheet ark1 = new ExcelPackage(filepath).Workbook.Worksheets.First();

            int row = 2;
            String name = (string)ark1.Cells[row, 1].Value;
            String Class = (string)ark1.Cells[row, 2].Value;
            String parent = (string)ark1.Cells[row, 3].Value;
            Double OrbitalRadius = ark1.Cells[row, 4].GetValue<Double>();
            Double OrbitalPeriod = ark1.Cells[row, 5].GetValue<Double>();

            while (name != null)
            {
                switch (Class)
                {
                    case "Star":
                        Star star = new Star(name);
                        spaceObjects.Add(star);
                        break;
                    case "Planet":
                        Planet planet = new(name);
                        spaceObjects.Add(planet);
                        break;
                    case "Moon":
                        Moon moon = new(name);
                        spaceObjects.Add(moon);
                        break;
                }
                row++;
                name = (string)ark1.Cells[row, 1].Value;
                Class = (string)ark1.Cells[row, 2].Value;
                parent = (string)ark1.Cells[row, 3].Value;
                OrbitalRadius = ark1.Cells[row, 4].GetValue<Double>();
                OrbitalPeriod = ark1.Cells[row, 5].GetValue<Double>();
            }
            return spaceObjects;
        }
    }
}
