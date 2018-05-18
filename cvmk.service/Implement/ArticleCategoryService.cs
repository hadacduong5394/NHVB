using cvmk.context.domain;
using cvmk.service.Interface;
using hddata.DBFactory;
using hddata.RepositoryPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cvmk.service.Implement
{
    public class ArticleCategoryService: BaseService<ArticleCategory, int>, IArticleCategoryService
    {
        public ArticleCategoryService(IDbFactory factory): base(factory)
        {

        }
    }
}
