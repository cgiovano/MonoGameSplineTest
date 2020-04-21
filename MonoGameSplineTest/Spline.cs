using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameSplineTest
{
    class Spline
    {
        private Point2D[] _controlPointList;
        private Texture2D _pixelControlPointTexture, _pixelLineTexture;
        private List<Vector2> _controlPoints;
        private int _selectedIndex = 0;
        private List<Vector2[]> _splinePoints;
        private KeyboardState _ks1, _ks2;

        public Spline(Point2D[] pointList, Texture2D controlPointTex, Texture2D lineTexture)
        {
            _controlPointList = pointList;
            _pixelControlPointTexture = controlPointTex;
            _pixelLineTexture = lineTexture;
            _splinePoints = new List<Vector2[]>();
        }

        //Get the catmull-rom spline
        private void GetCatmullRomSpline()
        {
            // The number of points between the control Points
            int numPoints = 20;

            //For Catmull-Rom spline we need four control points to calculate the curve
            if (_controlPointList.Length < 4)
                throw new ArgumentException("The Catmull-Rom spline needs at least 4 control points");

            // Create the spline. Loops through the control Point list and add the line points into a new list.
            // The control point list must be subtracted by 3, so you'll get just the 2 necessary points to calculate the spline. Remember that the
            // lenght of an array is zero based, so, if you have an array of 4 elements, the length of that array is 5.
            for (int i = 0; i < _controlPointList.Length - 3; i++)
            {
                _controlPoints = new List<Vector2>();

                for (int j = 0; j < numPoints; j++)
                    _controlPoints.Add(GetPointOnCurvePosition(_controlPointList[i], _controlPointList[i + 1], _controlPointList[i + 2], _controlPointList[i + 3], .05f*j)); //(1f / numPoints) * j)

                _splinePoints.Add(_controlPoints.ToArray());
            }

            _controlPoints.Clear();
        }

        //Calculate the point of the line curve, returns a vector2 to be used by whatever you want to.
        private Vector2 GetPointOnCurvePosition(Point2D p0, Point2D p1, Point2D p2, Point2D p3, float t)
        {
            Vector2 ret = new Vector2();

            float t2 = t * t;
            float t3 = t2 * t;

            ret.X = 0.5f * ((2.0f * p1.x) +
            (-p0.x + p2.x) * t +
            (2.0f * p0.x - 5.0f * p1.x + 4 * p2.x - p3.x) * t2 +
            (-p0.x + 3.0f * p1.x - 3.0f * p2.x + p3.x) * t3);

            ret.Y = 0.5f * ((2.0f * p1.y) +
            (-p0.y + p2.y) * t +
            (2.0f * p0.y - 5.0f * p1.y + 4 * p2.y - p3.y) * t2 +
            (-p0.y + 3.0f * p1.y - 3.0f * p2.y + p3.y) * t3);

            return (ret);
        }


        public void Update(GameTime gameTime)
        {
            // Hack for one shot key press.
            // Select the control points.
            _ks1 = Keyboard.GetState();

            if (_ks1.IsKeyDown(Keys.S) && _ks2.IsKeyUp(Keys.S) && _selectedIndex < (_controlPointList.Length - 1))
                _selectedIndex++;
            if (_ks1.IsKeyDown(Keys.A) && _ks2.IsKeyUp(Keys.A) && _selectedIndex > 0)
                _selectedIndex--;

            _ks2 = _ks1;

            // Move the control points
            if (_ks1.IsKeyDown(Keys.Up))
                _controlPointList[_selectedIndex].y -= 1;
            if (_ks1.IsKeyDown(Keys.Down))
                _controlPointList[_selectedIndex].y += 1;
            if (_ks1.IsKeyDown(Keys.Right))
                _controlPointList[_selectedIndex].x += 1;
            if (_ks1.IsKeyDown(Keys.Left))
                _controlPointList[_selectedIndex].x -= 1;

            // The spline must be calculate in every frame, mainly if are using an object that is moving around the space
            // In this case, we are moving the control points that affects in real time the curvature of our Spline.
            GetCatmullRomSpline();
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice gd)
        {
            gd.Clear(Color.Black);
            spriteBatch.Begin();
            
            // Draw the points (between each control point) of line curve
            for (int i = 0; i < _splinePoints.Count; i++)
            {
                for (int j = 0; j < _splinePoints[i].Length; j++)
                    spriteBatch.Draw(_pixelLineTexture, _splinePoints[i][j], Color.Yellow);
            }

            for (int i = 0; i < _controlPointList.Length; i++)
            {
                // Changes the color of the selected control point.
                if (_selectedIndex == i)
                    spriteBatch.Draw(_pixelLineTexture, new Vector2(_controlPointList[i].x, _controlPointList[i].y), null, Color.CadetBlue, 0, Vector2.Zero, 3f, SpriteEffects.None, 0);
                else
                    spriteBatch.Draw(_pixelLineTexture, new Vector2(_controlPointList[i].x, _controlPointList[i].y), null, Color.Red, 0, Vector2.Zero, 3f, SpriteEffects.None, 0);
            }

            spriteBatch.End();

            // This is much important!!! We need clear our list of line points
            for (var i = 0; i < _splinePoints.Count; i++)
                _splinePoints.RemoveAt(i);
        }
    }
}
