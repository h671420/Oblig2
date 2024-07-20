using System;
using System.IO;
using System.Drawing;

using OfficeOpenXml;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Reflection;

namespace SpaceSim
{
    public class SpaceObject
    {
        public string name { get; set; }
        public string color { get; set; }   
        public double radius { get; set; }
        public Position position { get; set; }
        public List<Rotational> children { get; set; }

        public SpaceObject(string name, string color, double radius)
        {
            this.name = name;
            this.color = color;
            this.radius = radius;
            this.position = new();
            this.children = new List<Rotational>();
        }
        public virtual void CalculatePosition(double days)
        {
            foreach (var item in children)
            {
                item.CalculatePosition(days);
            }
        }
        public virtual void printInfo()
        {
            Console.Write
                (
                "\n" +
                "Name: " + name + "  " +
                "Type: " + this.GetType().Name + "  " +
                "children: " + children.Count + "\n" +
                "\t" + "Body data->  " +
                "Color: " + color + " " +
                "Radius: " + radius + "  " +
                "Position: [" + (int)position.X + "," + (int)position.Y + "]" + "  " +
                 "\n"
                );
        }
        public void printInfoWithChildren()
        {
            this.printInfo();
            foreach (var item in children)
            {
                item.printInfo();
            }
        }
    }
    public class Rotational : SpaceObject
    {
        public double orbitalRadius { get; set; }
        public double orbitalPeriod { get; set; }
        public SpaceObject anchor { get; set; }

        public Rotational(
                string name, string color, double radius,
            double OrbitalRadius, double orbitalPeriod, SpaceObject anchor
            ) : base(name, color, radius)
        {
            this.anchor = anchor;
            this.orbitalRadius = OrbitalRadius;
            this.orbitalPeriod = orbitalPeriod;
        }
        public override void printInfo()
        {
            base.printInfo();
            Console.Write
                (
                "\t" +
                "Orbital data->  " +
                "Barycenter: " + anchor.name + "  " +
                "Orbital radius: " + orbitalRadius + "  " +
                "Orbital period " + orbitalPeriod + "\n"
                );
        }


        public override void CalculatePosition(double days)
        {
            double d = 2 * Math.PI * (days / this.orbitalPeriod);
            position.X = anchor.position.X + this.orbitalRadius * Math.Cos(d);
            position.Y = anchor.position.Y + this.orbitalRadius * Math.Sin(d);
            base.CalculatePosition(days);
        }
    }
    public class Star : SpaceObject
    {
        public Star(string name, string color, double radius) : base(name, color, radius) { }

    }
    public class Planet : Rotational
    {
        public Planet
            (string name, string color, double radius,
            double OrbitalRadius, double orbitalPeriod, SpaceObject anchor) :
            base(name, color, radius, OrbitalRadius, orbitalPeriod, anchor)
        { }

    }
    public class Moon : Rotational
    {
        public Moon
            (string name, string color, double radius,
            double OrbitalRadius, double orbitalPeriod, SpaceObject anchor) :
            base(name, color, radius, OrbitalRadius, orbitalPeriod, anchor)
        { }
    }
    public class Solarsystem
    {
        private double day;
        public void setDay(double day)
        {
            this.day = day;
            star.CalculatePosition(day);
        }
        public double getDay()
        {
            return day;
        }

        public List<SpaceObject> spaceObjects { get; set; }
        public Star star { get; set; }

        public Solarsystem()
        {
            day = 0;
            spaceObjects = Utils.fetchSpaceObjects();
            Star? starObject = spaceObjects.Find(o => o.GetType() == typeof(Star)) as Star;
            if (starObject != null)
                star = starObject;
            else
                throw new Exception("No star!");
            star.CalculatePosition(day);
        }
        public double calculateRadius()
        {
            Rotational furthestPlanet = star.children.First();

            foreach (var item in star.children)
            {
                if (item.orbitalRadius > furthestPlanet.orbitalRadius)
                    furthestPlanet = item;
            }
            Rotational furthestMoon = furthestPlanet.children.First();
            foreach (var item in furthestPlanet.children)
            {
                if (item.orbitalRadius > furthestMoon.orbitalRadius)
                    furthestMoon = item;
            }
            return furthestPlanet.orbitalRadius + furthestMoon.orbitalRadius;
        }
        public double calculateRadius(SpaceObject spaceObject)
        {
            double radiusThis;
            if (!(spaceObject is SpaceSim.Rotational))
                radiusThis = 0;
            else
            {
                Rotational rotational = (SpaceSim.Rotational)spaceObject;
                radiusThis = rotational.orbitalRadius;
            }
            double radiusCurrentChild = 0;
            foreach (Rotational child in spaceObject.children)
            {
                double radiusNextChild = calculateRadius(child);
                if (radiusNextChild > radiusCurrentChild)
                    radiusCurrentChild = radiusNextChild;
            }
            return radiusCurrentChild+radiusThis;
        }
        public double calculateRadiusExThis(SpaceObject spaceObject)
        {
            double span;
            if (!(spaceObject is SpaceSim.Rotational)) 
                span = calculateRadius(spaceObject);
            else {
                Rotational rotational = ( SpaceSim.Rotational)spaceObject;
                span = calculateRadius(rotational)-rotational.orbitalRadius;
            }
            if (span == 0)
                span = spaceObject.radius * 1.5;
            return span;
        }
    }


    public class Utils
    {
        public static List<SpaceObject> fetchSpaceObjects()
        {
            string filepath = AppDomain.CurrentDomain.BaseDirectory;
            filepath = Path.GetFullPath(Path.Combine(filepath, @"..\..\..\.."));
            filepath = Path.Combine(filepath, "Library", "Data", "Planets.xlsx");


            List<SpaceObject> spaceObjects = new();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelWorksheet ark1 = new ExcelPackage(filepath).Workbook.Worksheets.First();

            int row = 2;
            String name;
            do
            {
                String Class = (string)ark1.Cells[row, 1].Value;
                name = (string)ark1.Cells[row, 2].Value;
                String color = (string)ark1.Cells[row, 3].Value;
                double Radius = ark1.Cells[row, 4].GetValue<double>();
                String parentName = (string)ark1.Cells[row, 5].Value;
                double OrbitalRadius = ark1.Cells[row, 6].GetValue<double>()*1000;
                double OrbitalPeriod = ark1.Cells[row, 7].GetValue<double>();

                SpaceObject? parent = spaceObjects.Find((o) => o.name == parentName);

                switch (Class)
                {
                    case "Star":
                        Star star = new(name, color, Radius);
                        spaceObjects.Add(star);
                        break;
                    case "Planet":
                        if (parent != null)
                        {
                            Planet planet = new(name, color, Radius, OrbitalRadius, OrbitalPeriod, parent);
                            parent.children.Add(planet);
                            spaceObjects.Add(planet);
                        }
                        break;
                    case "Moon":
                        if (parent != null)
                        {
                            Moon moon = new(name, color, Radius, OrbitalRadius, OrbitalPeriod, parent);
                            parent.children.Add(moon);
                            spaceObjects.Add(moon);
                        }
                        break;
                }
                row++;

            } while (name != null);
            return spaceObjects;
        }
    }
    public class Position
    {
        public double X { get; set; }
        public double Y { get; set; }
        public Position() { X = 0; Y = 0; }
    }
}
