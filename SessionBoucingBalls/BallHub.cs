using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using SessionBoucingBalls.Models;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;

namespace SessionBoucingBalls
{    
    // This class is the medium that the client JavaScript uses to signal the server
    public class BallHub : Hub
    {
        // Is set via the constructor on each creation
        private ServerContoller _serverController;
        public static int connection_count;
        // Created for every connected client (which is why ServerController needs to be a singleton)
        public BallHub() : this(ServerContoller.Instance)
        {
        }

        public BallHub(ServerContoller serverController)
        {
            _serverController = serverController;
        }

        public void AddBallObject(string id)
        {
            _serverController.AddBallObject(id);
        }
       
        public int GetTotalConnectionCount()
        {
            return _serverController._ballObjects.Count;
        }

        public override Task OnConnected()
        {            
            AddBallObject(Context.ConnectionId);
            connection_count++;
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            // stopCalled:
            //     true, if stop was called on the client closing the connection gracefully; false,
            //     if the connection has been lost for longer than the Microsoft.AspNet.SignalR.Configuration.IConfigurationManager.DisconnectTimeout.
            //     Timeouts can be caused by clients reconnecting to another SignalR server in scaleout.        
            _serverController.RemoveBallObject(Context.ConnectionId);
            connection_count--;
            return base.OnDisconnected(stopCalled);
        }
    }
    
}