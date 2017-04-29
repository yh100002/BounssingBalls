$(function ()
{
    var ballHub = $.connection.ballHub,
   
    totalCount = 0,   
    ballObjects = [];
    // The BallHub on the server will be able to call this function
    ballHub.client.createBallObject = function (ballObject)
    {
        console.log('createBallObject==========>' + ballObject.id + ':' + ballObject.left + ',' + ballObject.top);
        var found = false;
        for (var index in ballObjects)
        {
            if (ballObjects[index].id == ballObject.id)
            {
                found = true;
                break;
            }
        }

        if (found == false)
        {
            AddBallObject(ballObject.id);
        }
        
    }
    // The BallHub on the server will be able to call this function
    ballHub.client.removeBallObject = function (id)
    {
        //console.log('removeBallObject==========>' + ballObject.id + ':' + ballObject.left + ',' + ballObject.top);
        $("#" + id).remove();

        ballHub.server.getTotalConnectionCount().done(function (result) {
            totalCount = result;
            document.getElementById("message").innerHTML = totalCount + " connected";
        });
    }
    // The BallHub on the server will be able to call this function
    ballHub.client.updateBallObjectPosition = function (ballObject)
    {
        //console.log(ballObject.id + ':' + ballObject.left + ',' + ballObject.top);
        for (var index in ballObjects)
        {
            if (ballObjects[index].id == ballObject.id)
            {
                ballObjects[index].left = ballObject.left;
                ballObjects[index].top = ballObject.top;            
                $("#" + ballObject.id).animate({ left: ballObject.left, top: ballObject.top }, { duration: 100, queue: false });
                //console.log(ballObjects[index].id + ':' + ballObjects[index].left + ',' + ballObjects[index].top);
                //console.log(ballObjects[index].element.offsetWidth);
                //console.log(ballObject.id + ':' + ballObject.left + ',' + ballObject.top);
            }
            //console.log(ballObjects[index].id + ':' + ballObjects[index].left + ',' + ballObjects[index].top);
        }
    };

    // Enables logging (in the console, and shows what transport is being used)
    $.connection.hub.logging = true;
    // Start the connection
    $.connection.hub.start().done(function ()
    {   
            
    });

    function AddBallObject(name)
    {      
        var newElement = document.createElement('div');
        newElement.id = name;
        newElement.className = "ball";        
        document.getElementById('area').appendChild(newElement);

        var ballObject =
        {
            left: 0,
            top: 0,
            width: document.getElementById(name).offsetWidth,
            height: document.getElementById(name).offsetHeight,
            id: name,
            moved: false,
            element: newElement
        };

        ballObjects.push(ballObject);

        ballHub.server.getTotalConnectionCount().done(function (result) {
            totalCount = result;
            document.getElementById("message").innerHTML = totalCount + " connected";
        });
    }
   

});