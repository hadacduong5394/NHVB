using cvmk.context.domain.Material;
using hddata.RepositoryPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cvmk.service.Interface
{
    public interface IMaterialService: IBaseService<Material, int>
    {
        IList<Material> GetbyFilter(int comId, string code, string name, int currentPage, int pageSize, out int total);

        IList<Material> GetMaterials(int comId, string keyword, int currentPage, int pageSize, out int total);

        bool Create(Material entity, out string message);

        bool Edit(Material entity, out string message);

        bool Delete(int id, out string message);

        IList<Material> GetMaterialForImport(int com_id);

        Material GetbyCode(string code, int com_id);
    }
}
