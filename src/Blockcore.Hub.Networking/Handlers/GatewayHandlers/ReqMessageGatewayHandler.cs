using Blockcore.Hub.Networking.Managers;
using Blockcore.Hub.Networking.Services;
using Blockcore.Platform.Networking.Entities;
using Blockcore.Platform.Networking.Messages;
using System.Net;
using System.Net.Sockets;

namespace Blockcore.Platform.Networking.Handlers.GatewayHandlers
{
   public class ReqMessageGatewayHandler : IGatewayMessageHandler, IHandle<ReqMessage>
   {
      private readonly GatewayManager manager;

      public ReqMessageGatewayHandler(GatewayManager manager)
      {
         this.manager = manager;
      }

      public void Process(BaseMessage message, ProtocolType Protocol, IPEndPoint EP = null, TcpClient Client = null)
      {
         ReqMessage req = (ReqMessage)message;

         HubInfo hubInfo = manager.Connections.GetConnection(req.RecipientId);

         if (hubInfo != null)
         {
            manager.SendTCP(new Req(req), hubInfo.Client);
         }
      }
   }
}
