using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Level: IEntity
    {
        public string Id { get; set; }
        public int CreatorUserId { get; set; }
        public DateTime createDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public bool IsPublished { get; set; }
        public string LevelJson { get; set; }
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
        public string? LikedUsersList { get; set; }
        public string? DislikedUsersList { get; set; }
    }
}
