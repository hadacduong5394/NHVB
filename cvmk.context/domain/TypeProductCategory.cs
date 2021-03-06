﻿using hdcontext.AdminDomain.Abtracttions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cvmk.context.domain
{
    [Table("TypeProductCategories")]
    public class TypeProductCategory : Auditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

        public int ComId { get; set; }
    }
}