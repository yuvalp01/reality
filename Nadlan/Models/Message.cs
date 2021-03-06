﻿using Nadlan.Models.Issues;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Nadlan.Models
{
    [Table("messages")]
    public class Message
    {
        public int Id { get; set; }

        public string TableName { get; set; }
        public string Content { get; set; }
        public DateTime DateStamp { get; set; }

        //public Guid UserId { get; set; }
        public string UserName { get; set; }
        public bool IsRead { get; set; }
        public int ParentId { get; set; }
        //[JsonIgnore]
        //public virtual Issue Issue { get; set; }

        public bool IsDeleted { get; set; }

    }
}
