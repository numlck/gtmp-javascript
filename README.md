# JavascriptConnector
Use Javascript server side for GTMP/GTANetwork

Example

```javascript

var r = require("filename.js")

API.onChatCommand.connect(function(sender, command, e){
	 API.consoleOutput("Example Command "+ command);

	if(command.startsWith("/s ")){
		players = API.getPlayersInRadiusOfPlayer(50, sender)
		
			for (var x = 0; x < players.Count; x++){
				API.sendChatMessageToPlayer(players[x], "~h~"+players[x].socialClubName+" says: "+ command.replace("/s ", ""));
			}	


		e.Cancel = true;
	}
	if(command.startsWith("/io ")){
		lib.System.IO.File.WriteAllText("io_test.json", JSON.stringify({
			"test":123
		}));
	}
})
```
