using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace WebPushNotification.Core.Database.Pocos
{
    [TableName("PushSubscriptions")]
    [PrimaryKey("Id", autoIncrement = true)]
    public class PushSubscription
    {
        [Column("Id")]
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }

        public string PushEndpoint { get; set; }
        public string PushP256DH { get; set; }
        [Column("PushAuth")]
        public string PushAuth { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
