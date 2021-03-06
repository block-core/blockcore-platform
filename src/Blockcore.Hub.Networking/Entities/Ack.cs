using Blockcore.Platform.Networking.Messages;

namespace Blockcore.Platform.Networking.Entities
{
   public class Ack : BaseEntity
   {
      public string RecipientId { get; set; }

      public bool Response { get; set; }

      public Ack(string senderId)
      {
         Id = senderId;
      }

      public Ack(AckMessage message)
      {
         Id = message.Id;
         RecipientId = message.RecipientId;
         Response = message.Response;
      }

      public override BaseMessage ToMessage()
      {
         var msg = new AckMessage(Id);

         msg.RecipientId = RecipientId;
         msg.Response = Response;

         return msg;
      }
   }
}
