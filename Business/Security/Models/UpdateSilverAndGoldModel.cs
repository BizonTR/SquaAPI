using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Security.Models
{
    public class UpdateSilverAndGoldModel
    {
        [DisplayName("id")]
        public string Id { get; set; }

        [DisplayName("silverAmount")]
        public double SilverAmount { get; set; }

        [DisplayName("goldAmount")]
        public double GoldAmount { get; set; }
    }
}
