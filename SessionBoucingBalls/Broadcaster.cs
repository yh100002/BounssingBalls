using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Diagnostics;
using SessionBoucingBalls.Models;

namespace SessionBoucingBalls
{
    public interface IBroadcaster
    {
        void Broadcast(IEnumerable<BallObject> ballObjectsMoved);
        void BroadcastCreate(IEnumerable<BallObject> ballObjects);
        void BroadcastRemove(string id);
    }

    public class Broadcaster :IBroadcaster
    {
        private readonly static Lazy<Broadcaster> _instance = new Lazy<Broadcaster>(() => new Broadcaster());
        private readonly IHubContext _hubContext;

        public Broadcaster()
        {
            // Save our hub context so we can easily use it to send to its connected clients
            _hubContext = GlobalHost.ConnectionManager.GetHubContext<BallHub>();
        }

        public void Broadcast(IEnumerable<BallObject> ballObjectsMoved)
        {
            try
            {
                // Tell the clients the new position of moved objects
                foreach (var ballObject in ballObjectsMoved)
                {
                    // This is how we can access the Clients property                     
                    _hubContext.Clients.All.updateBallObjectPosition(ballObject);
                    //string msg = string.Format("{0}:{1}:{2},{3}", ballObject.Id, ballObject.LastUpdatedBy, ballObject.Left, ballObject.Top);
                    //Trace.WriteLine(msg);
                    ballObject.Moved = false;
                }
            }
            catch(Exception)
            {

            }          
        }     

        public void BroadcastCreate(IEnumerable<BallObject> ballObjects)
        {           
            try
            {                
                foreach (var ballObject in ballObjects)
                {                    
                    _hubContext.Clients.All.createBallObject(ballObject);                   
                }
            }
            catch (Exception)
            {

            }
        }

        public void BroadcastRemove(string id)
        {
            try
            {
                _hubContext.Clients.All.removeBallObject(id);
            }
            catch (Exception)
            {

            }
        }
    }
}