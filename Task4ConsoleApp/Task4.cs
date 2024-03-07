using SpaceSim;

using System;
using System.Collections.Generic;
using System.IO;

using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Runtime.InteropServices;
using static OfficeOpenXml.ExcelErrorValue;
using System.Xml.Linq;

class Task4
{
    public static void Main()
    {
        Solarsystem solarsystem = new Solarsystem();    
        SpaceObject spaceObject = solarsystem.star;
        bool running = true;
        while (running)
        {
            Console.WriteLine("Day:" + solarsystem.getDay()+ ", Selected object: " + spaceObject.name);
            spaceObject.printInfo();
            Console.WriteLine("Please return one of the following:\n"
                + "\t1. Select a new day\n"
                + "\t2. Select a different spaceobject\n"
                + "\t3. to do a printout of all spaceobjects\n"
                + "\t0. to exit program");

            string input = Console.ReadLine()!;
            switch (input)
            {
                case "1":
                    Console.Write("Please return a new day: ");
                    input = Console.ReadLine()!;
                    try 
                    {
                        solarsystem.setDay(int.Parse(input));
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("The value couldn't be converted to a whole number\nHit enter to return");
                        Console.ReadLine();
                        break;
                    }
                    break;

                case "2":
                    Console.WriteLine("Please return the name of the spaceobject");
                    input=Console.ReadLine()!;
                    SpaceObject? newSpaceObject= solarsystem.spaceObjects.Find((o) => o.name == input);
                    if (newSpaceObject == null)
                    {
                        Console.WriteLine("No such object");
                        Console.ReadLine();
                    }
                    else
                        spaceObject = newSpaceObject;
                    break;

                case "3":
                    Console.Clear();
                    foreach (var item in solarsystem.spaceObjects)
                    {
                        item.printInfo();
                    }
                    Console.WriteLine("Hit enter to return");
                    Console.ReadLine();
                    break;
                case "0":
                    running = false;
                    break;  
            }
            Console.Clear();
        }
    }
}
