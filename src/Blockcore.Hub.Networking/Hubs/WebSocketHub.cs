using System;
using System.Collections.Generic;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Blockcore.Hub.Networking.Managers;
using Blockcore.Platform.Networking.Messages;
using System.Security.Cryptography.X509Certificates;
using Blockcore.Platform.Networking.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Blockcore.Hub.Networking.Hubs
{
   public class WebSocketHub : Microsoft.AspNetCore.SignalR.Hub
   {
      private readonly ILogger<WebSocketHub> log;
      private readonly CommandDispatcher commandDispatcher;
      private readonly HubManager hubManager;

      public WebSocketHub(ILogger<WebSocketHub> log, CommandDispatcher commandDispatcher, HubManager hubManager)
      {
         this.log = log;
         this.commandDispatcher = commandDispatcher;
         this.hubManager = hubManager;
      }

      /// <summary>
      /// Basic echo method that can be used to verify connection.
      /// </summary>
      /// <param name="message">Any message to echo back.</param>
      /// <returns>Returns the same message supplied.</returns>
      public void Broadcast(string publickey, string message)
      {
         var msg = new Message { From = publickey, To = "Everyone", Content = message, RecipientId = 1 };

         hubManager.SendMessageTCP(msg);
         //return Clients.Caller.SendAsync("Message", message);
      }

      //public Task Command(string type, string command, object[]? args)
      //{
      //   string result = commandDispatcher.Execute(type, command, args);
      //   return Clients.Caller.SendAsync("Command", result);
      //}
   }
}
