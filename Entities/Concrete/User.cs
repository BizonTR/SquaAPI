using Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class User : IdentityUser, IEntity
    {
        public string? CompletedLevels { get; set; }
        public double SilverAmount { get; set; }
        public double GoldAmount { get; set; }
        public string? PurchasedItems { get; set; }
        public string? FriendList { get; set; }
        public string? ProfilePhoto { get; set; }
        public string? UserRole { get; set; }
    }
}
