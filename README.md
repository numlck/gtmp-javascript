# JavascriptConnector
Use Javascript server side for GTMP/GTANetwork


To `require` a file, the file needs to add functions/variables to `exports` inside that file.


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
		lib.System.IO.File.WriteAllText("JavascriptConnector.json", JSON.stringify({
			"test":123
		}));
	}
})
```
