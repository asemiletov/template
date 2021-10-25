using LementPro.Server.SvcTemplate.Common.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LementPro.Server.Common.Repository.Abstract;

namespace LementPro.Server.SvcTemplate.Repository.Entities
{
    [Table("blockWork")]
    public class BlockWorkEntity : AEntity<long>
    {
        [MaxLength(64)]
        public string Name { get; set; }
        
        [MaxLength(256)]
        public string Description { get; set; }

        [EnumDataType(typeof(BlockWorkStatus))]
        public BlockWorkStatus Status { get; set; }

        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset DateUpdated { get; set; }
    }
}
