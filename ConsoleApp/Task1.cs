// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using SpaceSim;

public class Astronomy
{
    public static void Main()
    {
        List<SpaceObject> solarSystem = new List<SpaceObject>
        {
            new Star("Sun"),
            new Planet("Mercury"),
            new Planet("Venus"),
            new Planet("Terra"),
            new Moon("The Moon")
        };
        foreach (SpaceObject obj in solarSystem)
        {
            obj.printInfo();
        }
        Console.ReadLine();
    }
}
