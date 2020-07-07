﻿using System;
using System.Collections.Generic;

namespace Nadlan.Models.Issues
{
    public class Issue
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public DateTime DateOpen { get; set; }
        public DateTime? DateClose { get; set; }

        public int ApartmentId { get; set; }
        public Apartment Apartment { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
    }


}