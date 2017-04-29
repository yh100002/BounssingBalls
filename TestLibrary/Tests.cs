using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;
using SessionBoucingBalls;
using SessionBoucingBalls.Models;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR;
using Moq;
using System.Dynamic;
using System.Diagnostics;


////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//Unit Test module should not be enough. I have to improve and refact this as well.
namespace TestLibrary
{
    public class Tests
    {
        [Fact]
        public void AddBallTest()
        {
            // Arrange
            var groupManagerMock = new Mock<IGroupManager>();
            var connectionId = Guid.NewGuid().ToString();
            var groupsJoined = new List<string>();
            groupManagerMock.Setup(g => g.Add(connectionId, It.IsAny<string>()))
                            .Returns(Task.FromResult<object>(null))
                            .Callback<string, string>((cid, groupToJoin) =>
                                groupsJoined.Add(groupToJoin));
            var mockClients = new Mock<IHubCallerConnectionContext<dynamic>>();           
            
            var hub = new BallHub();
            hub.Groups = groupManagerMock.Object;
            hub.Context = new HubCallerContext(request: null, connectionId: connectionId);
            hub.Clients = mockClients.Object;
                      

            hub.AddBallObject(connectionId);
            int cnt = hub.GetTotalConnectionCount();
            int con = BallHub.connection_count;

            Assert.True(cnt == 1);
        }

        [Fact]
        public void BroadcatTest()
        {
            var called = false;
            var hub = new BallHub();
            hub.AddBallObject(Guid.NewGuid().ToString());
            hub.AddBallObject(Guid.NewGuid().ToString());
            hub.AddBallObject(Guid.NewGuid().ToString());
            int cnt = hub.GetTotalConnectionCount();

            var mockBroadCastor = new Mock<IBroadcaster>();            
            dynamic br = new ExpandoObject();
            br.Broadcast = new Action<IEnumerable<BallObject>>((ball) =>
            {
                called = true;
                Debug.WriteLine("{0}", ball);
            });

            Assert.True(called);
        }

        [Fact]
        public void OnConnectedTest()
        {

        }

        [Fact]
        public void OnDisConnectedTest()
        {

        }
    }


}
