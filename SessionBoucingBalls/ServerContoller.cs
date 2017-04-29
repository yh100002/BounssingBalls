using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SessionBoucingBalls.Models;
using System.Threading;

namespace SessionBoucingBalls
{
   //singletone to keep the instance
    public class ServerContoller
    {
        private readonly static Lazy<ServerContoller> _instance = new Lazy<ServerContoller>(() => new ServerContoller());
        private readonly TimeSpan BroadcastInterval;        
        private Broadcaster _broadcaster;
        public List<BallObject> _ballObjects;
        private Timer _broadcastLoop;
        private List<BallController> _balls;
    
        public static ServerContoller Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        private ServerContoller()
        {
            // We're going to broadcast to all clients a maximum of 25 times per second
            BroadcastInterval = TimeSpan.FromMilliseconds(40);

            _ballObjects = new List<BallObject>();
            _broadcaster = new Broadcaster();

            _balls = new List<BallController>();
        }

        public void AddBallObject(string id)
        {
            lock(this)
            {
                if (!_ballObjects.Any(g => g.Id == id))
                {
                    var ballObject = new BallObject { Id = id };
                    _ballObjects.Add(ballObject);
                    var ball = new BallController(ballObject);
                    _balls.Add(ball);

                    _broadcaster.BroadcastCreate(_ballObjects);

                    // Now that we got balls, start the broadcast loop, and update all the objects that moved
                    _broadcastLoop = new Timer(
                                                Update,
                                                null,
                                                BroadcastInterval,
                                                BroadcastInterval);
                }
            }            
        }

        public void RemoveBallObject(string id)
        {
            lock(this)
            {
                var itemToRemove = _ballObjects.Single(r => r.Id == id);
                _ballObjects.Remove(itemToRemove);
                var temp = _balls.Single(r => r._ballObject == itemToRemove);
                _balls.Remove(temp);
                _broadcaster.BroadcastRemove(id);
            }
          
        }      

        private void checkCollisionBalls()
        {
            foreach (var ball1 in _balls)
            {
                foreach (var ball2 in _balls)
                {
                    if(ball1 != ball2)
                    {
                        ball1.BallCollision(ball2);
                    }
                }
            }
        }

        private void Update(object state)
        {            
            foreach (var ball in _balls)
            {
                ball.Update();
            }
            checkCollisionBalls();

            _broadcaster.Broadcast(_ballObjects.Where(g => g.Moved));
        }

    }
}
