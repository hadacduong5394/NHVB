using hdcontext.AdminDomain.Abtracttions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cvmk.context.domain
{
    [Table("ArticleCategories")]
    public class ArticleCategory : Auditable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public int? ParentId { get; set; }

        [MaxLength(256)]
        public string CatName { get; set; }

        [MaxLength(256)]
        public string URL { get; set; }
    }
}