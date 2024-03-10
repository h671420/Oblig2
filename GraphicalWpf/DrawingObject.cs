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
    public class DrawingObject
    {
        public SpaceObject spaceObject { get; set; }
        public Ellipse ellipse { get; set; }
        public List<DrawingObject> children { get; set; }

        public DrawingObject(SpaceObject spaceObject, Ellipse ellipse)
        {
            this.spaceObject = spaceObject;
            this.ellipse = ellipse;
            Color color = (Color)ColorConverter.ConvertFromString(spaceObject.color);
            ellipse.Fill = new SolidColorBrush(color);
            this.children = new List<DrawingObject>();
        }
        public void Draw(SolarsystemCanvas solarsystemCanvas)
        {
            double factor = solarsystemCanvas.solarsystem.calculateRadiusExThis(solarsystemCanvas.selectedObject.spaceObject) * 2 / solarsystemCanvas.Width * 1.05;
            ellipse.Height = ellipse.Width = solarsystemCanvas.getScale()* spaceObject.radius / factor;
            Position canvasPosition = SolarsystemPosToCanvasPos(spaceObject, solarsystemCanvas);
            Canvas.SetTop(ellipse, canvasPosition.Y);
            Canvas.SetLeft(ellipse, canvasPosition.X);
            foreach (DrawingObject child in children) { child.Draw(solarsystemCanvas); }
        }
        private Position SolarsystemPosToCanvasPos(SpaceObject spaceObject, SolarsystemCanvas solarsystemCanvas)
        {
            double systemDiameter = solarsystemCanvas.solarsystem.calculateRadiusExThis(solarsystemCanvas.selectedObject.spaceObject)*2;
            double factor = systemDiameter / solarsystemCanvas.Width * 1.05;
            Position CanvasPos = new Position();
            CanvasPos.X = -ellipse.Width / 2 + (spaceObject.position.X-solarsystemCanvas.selectedObject.spaceObject.position.X) / factor + solarsystemCanvas.Width / 2;
            CanvasPos.Y = -ellipse.Height / 2 + (spaceObject.position.Y - solarsystemCanvas.selectedObject.spaceObject.position.Y) / factor + solarsystemCanvas.Height / 2;
            return CanvasPos;
        }
    }
}
