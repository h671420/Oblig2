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
        List<SpaceObject> spaceObjects = SpaceSim.Utils.fetchSpaceObjects();
        bool running = true;
        int day = 0;
        while (running)
        {
            Console.WriteLine("Day:"+day+"\nPlease return one of the following:\n"
                + "\t1. to select a new day\n"
                + "\t2. to inspect a spaceobject\n"
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
                        day = int.Parse(input); 
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("The value couldn't be converted to a whole number\nHit enter to return");
                        Console.ReadLine();
                        break;
                    }
                    break;

                case "2":
                    break;

                case "3":
                    Console.Clear();
                    foreach (var item in spaceObjects)
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
