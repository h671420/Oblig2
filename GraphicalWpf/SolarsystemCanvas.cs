using SpaceSim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphicalWpf
{
    public class SolarsystemCanvas : Canvas
    {
        public Solarsystem solarsystem { get; set; }

        public DrawingObject selectedObject { get; set; }

        private double scale;
        public void setScale(double exponent)
        {
            this.scale = Math.Pow(1.1,exponent);
        }
        public double getScale() {
            return scale;
        }
        public SolarsystemCanvas(double WindowWidth, double WindowHeight, double scale)
        {   
            this.ClipToBounds = true;
            setScale(scale);
            this.solarsystem = new();
            this.Background = new SolidColorBrush(Colors.Black);
            this.selectedObject = createDrawingObject(solarsystem.star);
        }

        private DrawingObject createDrawingObject(SpaceObject spaceObject)
        {
            Ellipse ellipse = new Ellipse();
            this.Children.Add(ellipse);
            DrawingObject newDrawingObject = new DrawingObject(spaceObject, ellipse);
            foreach (var child in spaceObject.children)
            {
                newDrawingObject.children.Add(createDrawingObject(child));
            }
            return newDrawingObject;
        }

        public void Draw(double newSideLength)
        {
            this.Width = this.Height = newSideLength;
            this.selectedObject.Draw(this);
        }
    }
}
