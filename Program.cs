

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Game.Helpers;
using Microsoft.Xna.Framework;






void StartGame(){
    var game = new Game.UnamedGame();
    game.Run();
}
void StartServer(){
    var server = new Server.Server();
    server.Start();
}


bool server = false;
foreach(string arg in args){
    if(arg == "server"){
        server = true;
    }
}
   if(server){
await Task.Run(() => StartServer());
}

StartGame();