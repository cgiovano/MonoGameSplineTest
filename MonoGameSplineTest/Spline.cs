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

        KeyboardState ks1, ks2;

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
            //For Catmull-Rom spline we need four control points to calculate the curve

            int numPoints = 4;

            if (_controlPointList.Length < 4)
                throw new ArgumentException("The Catmull-Rom spline needs at least 4 control points");

            for (int i = 0; i < _controlPointList.Length - 3; i++)
            {
                _controlPoints = new List<Vector2>();

                for (int j = 0; j < 20; j++)
                {
                    _controlPoints.Add(GetPointOnCurvePosition(_controlPointList[i], _controlPointList[i + 1], _controlPointList[i + 2], _controlPointList[i + 3], .05f*j)); //(1f / numPoints) * j)
                }

                _splinePoints.Add(_controlPoints.ToArray());
            }

            _controlPoints.Clear();
        }

        //Calculate the line
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
            ks1 = Keyboard.GetState();

            if (ks1.IsKeyDown(Keys.S) && ks2.IsKeyUp(Keys.S) && _selectedIndex < (_controlPointList.Length - 1))
            {
                _selectedIndex++;
            }
            if (ks1.IsKeyDown(Keys.A) && ks2.IsKeyUp(Keys.A) && _selectedIndex > 0)
            {
                _selectedIndex--;
            }

            ks2 = ks1;


            if (ks1.IsKeyDown(Keys.Up))
            {
                _controlPointList[_selectedIndex].y -= 1;
            }
            if (ks1.IsKeyDown(Keys.Down))
            {
                _controlPointList[_selectedIndex].y += 1;
            }
            if (ks1.IsKeyDown(Keys.Right))
            {
                _controlPointList[_selectedIndex].x += 1;
            }
            if (ks1.IsKeyDown(Keys.Left))
            {
                _controlPointList[_selectedIndex].x -= 1;
            }

            GetCatmullRomSpline();
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice gd)
        {
            gd.Clear(Color.Black);
            spriteBatch.Begin();
            
            for (int i = 0; i < _splinePoints.Count; i++)
            {
                for (int j = 0; j < _splinePoints[i].Length; j++)
                {
                    spriteBatch.Draw(_pixelLineTexture, _splinePoints[i][j], Color.Yellow);
                }
            }

            for (int i = 0; i < _controlPointList.Length; i++)
            {
                if (_selectedIndex == i)
                    spriteBatch.Draw(_pixelLineTexture, new Vector2(_controlPointList[i].x, _controlPointList[i].y), null, Color.CadetBlue, 0, Vector2.Zero, 3f, SpriteEffects.None, 0);
                else
                    spriteBatch.Draw(_pixelLineTexture, new Vector2(_controlPointList[i].x, _controlPointList[i].y), null, Color.Red, 0, Vector2.Zero, 3f, SpriteEffects.None, 0);
            }

            spriteBatch.End();

            for (var i = 0; i < _splinePoints.Count; i++)
            {
                _splinePoints.RemoveAt(i);
            }
        }
    }
}
