using Blockcore.Platform.Networking.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Blockcore.Platform.Networking.Entities
{
   public class HubInfo : BaseEntity
   {
      public string Name { get; set; }

      public IPEndPoint ExternalEndpoint { get; set; }

      public IPEndPoint InternalEndpoint { get; set; }

      public string FirstName { get; set; }

      public ConnectionTypes ConnectionType { get; set; }

      public List<IPAddress> InternalAddresses = new List<IPAddress>();

      [NonSerialized] //server use only
      public TcpClient Client;

      [NonSerialized] //server use only
      public bool Initialized;

      public HubInfo()
      {

      }

      public HubInfo(HubInfoMessage message)
      {
         Name = message.Name;
         Id = message.Id;
         FirstName = message.FirstName;

         if (!string.IsNullOrWhiteSpace(message.ExternalEndpoint))
         {
            ExternalEndpoint = IPEndPoint.Parse(message.ExternalEndpoint);
         }

         if (!string.IsNullOrWhiteSpace(message.InternalEndpoint))
         {
            InternalEndpoint = IPEndPoint.Parse(message.InternalEndpoint);
         }

         ConnectionType = Enum.Parse<ConnectionTypes>(message.ConnectionType);

         if (message.InternalAddresses != null)
         {
            foreach (string addr in message.InternalAddresses)
            {
               InternalAddresses.Add(IPAddress.Parse(addr));
            }
         }
      }

      public bool Update(HubInfoMessage message)
      {
         if (Id == message.Id)
         {
            //foreach (PropertyInfo P in message.GetType().GetProperties())
            //{
            //    if (P.GetValue(message) != null)
            //    {
            //        this.GetType().GetProperty(P.Name).SetValue(this, P.GetValue(message));
            //    }
            //}

            Name = message.Name;
            ConnectionType = Enum.Parse<ConnectionTypes>(message.ConnectionType);
            FirstName = message.FirstName;

            // It is very important to only set value if it is different than null, 
            // we must ensure we don't loose previously set IP address that we have discovered and connected to.
            if (!string.IsNullOrWhiteSpace(message.ExternalEndpoint))
            {
               ExternalEndpoint = IPEndPoint.Parse(message.ExternalEndpoint);
            }

            if (!string.IsNullOrWhiteSpace(message.InternalEndpoint))
            {
               InternalEndpoint = IPEndPoint.Parse(message.InternalEndpoint);
            }

            if (message.InternalAddresses != null && message.InternalAddresses.Count > 0)
            {
               InternalAddresses.Clear();

               foreach (string addr in message.InternalAddresses)
               {
                  InternalAddresses.Add(IPAddress.Parse(addr));
               }
            }
         }

         return (Id == message.Id);
      }

      public bool Update(HubInfo message)
      {
         if (Id == message.Id)
         {
            // It is very important to only set value if it is different than null, 
            // we must ensure we don't loose previously set IP address that we have discovered and connected to.
            foreach (PropertyInfo P in message.GetType().GetProperties())
            {
               if (P.GetValue(message) != null)
               {
                  P.SetValue(this, P.GetValue(message));
               }
            }

            if (message.InternalAddresses != null && message.InternalAddresses.Count > 0)
            {
               InternalAddresses.Clear();
               InternalAddresses.AddRange(message.InternalAddresses);
            }
         }

         return (Id == message.Id);
      }

      public override string ToString()
      {
         if (ExternalEndpoint != null)
            return Name + " (" + ExternalEndpoint.Address + ")";
         else
            return Name + " (UDP Endpoint Unknown)";
      }

      public HubInfo Simplified()
      {
         var msg = new HubInfo();

         msg.Name = Name;
         msg.Id = Id;
         msg.ExternalEndpoint = ExternalEndpoint;
         msg.InternalEndpoint = InternalEndpoint;

         return msg;
      }

      public override BaseMessage ToMessage()
      {
         var msg = new HubInfoMessage();

         msg.Name = Name;
         msg.Id = Id;
         msg.ExternalEndpoint = ExternalEndpoint?.ToString();
         msg.InternalEndpoint = InternalEndpoint?.ToString();
         msg.ConnectionType = ConnectionType.ToString();
         msg.InternalAddresses = InternalAddresses.Select(a => a.ToString()).ToList();

         return msg;
      }
   }
}
