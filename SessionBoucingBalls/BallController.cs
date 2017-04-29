using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using SessionBoucingBalls.Models;


namespace SessionBoucingBalls
{
    public class BallController
    {
       
        private const double DegreeToRadians = Math.PI / 180;
        private const int LeftBounds = 0;
        private const int RightBounds = 1000;
        private const int BottomBounds = 0;
        private const int TopBounds = 500;
        public BallObject _ballObject;
        private int _angle;
        private float _speed;        
        private readonly Random random = new Random();
        public Rectangle _bound = new Rectangle(0,0,0,0);

        public BallController(BallObject ballObject)
        {            
            ballObject.Left = RandomNumber(0,100);
            ballObject.Top = RandomNumber(0,100);

            _ballObject = ballObject;
            _speed = 8f;            
            _angle = 45;           
            
            UpdateBound();
        }

        private void UpdateBound()
        {
            _bound.X = (int)_ballObject.Left;
            _bound.Y = (int)_ballObject.Top;
            _bound.Width = 20;
            _bound.Height = 20;
        }

        private void HandleWallCollision()
        {
            if (_ballObject.Left <= LeftBounds || _ballObject.Left >= RightBounds)
            {                
                HandleVerticalWallCollision();
            }

            if (_ballObject.Top <= BottomBounds || _ballObject.Top >= TopBounds)
            {                
                HandleHorizontalWallCollision();
            }

        }

        private void HandleVerticalWallCollision()
        {            
            switch (_angle)
            {
                case 45: _angle = 135; break;
                case 135: _angle = 45; break;
                case 225: _angle = 315; break;
                case 315: _angle = 225; break;
            }
        }

        public void HandleHorizontalWallCollision()
        {   
            switch (_angle)
            {
                case 45: _angle = 315; break;
                case 135: _angle = 225; break;
                case 225: _angle = 135; break;
                case 315: _angle = 45; break;
            }
        }

        public void BallCollision(BallController otherBall)
        {
            if(this._bound.IntersectsWith(otherBall._bound) == true)
            {
                HandleVerticalWallCollision();
            }
        }

        //based on trigonometry, but this should be improved. Somtimes wrong calculations.
        public void Update()
        {
            HandleWallCollision();

            _ballObject.Left += Math.Cos(_angle * DegreeToRadians) * _speed;
            _ballObject.Top += Math.Sin(_angle * DegreeToRadians) * _speed;
            _ballObject.Moved = true;

            UpdateBound();                 
                
        }
      
        private int RandomNumber(int min, int max)
        {
            return random.Next(max - min) + min;
        }

        private string RandomColor()
        {
            Color randomColor = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
            return "#" + randomColor.R.ToString("X2") + randomColor.G.ToString("X2") + randomColor.B.ToString("X2"); ;
        }

    }
}