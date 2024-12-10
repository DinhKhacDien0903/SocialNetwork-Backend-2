using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Domain.Entities
{
    public class NotificationEntity:BaseEntity
    {
        [Key]
        public string Id { set; get; }=Guid.NewGuid().ToString();

        //public string? PostId { set; get; }

        public string UserId { get; set; }

        public string ReceiverId { get; set; }

        public string Content {  set; get; }

        public bool IsRead {  set; get; }

        public string Type {  set; get; }

        public bool IsDelete {  set; get; }=false;

        [ForeignKey("UserId")]
        public UserEntity? User { set; get; }

        [ForeignKey("ReceiverId")]
        public UserEntity? Receiver { set; get; }


    }
}
