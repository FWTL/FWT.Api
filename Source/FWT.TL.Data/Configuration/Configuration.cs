using FWT.TL.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FWT.TL.Data.Configuration
{
    public class TelegramSessionConfiguration : EntityTypeConfiguration<TelegramSession>
    {
        public TelegramSessionConfiguration()
        {
            Property(x => x.Id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
        }
    }
}
