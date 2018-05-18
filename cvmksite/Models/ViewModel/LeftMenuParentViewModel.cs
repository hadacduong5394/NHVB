using cvmk.service.Interface;
using hdcore;
using System.Collections.Generic;
using System.Linq;

namespace cvmksite.Models.ViewModel
{
    public class LeftMenuParentViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UI_SREF { get; set; }
        public string Icon { get; set; }

        public IList<LeftMenuParentViewModel> Childs
        {
            get
            {
                var lSrv = IoC.Resolve<ILeftMenuService>();
                return lSrv.GetChilds(this.Id).Select(m => new LeftMenuParentViewModel
                {
                    Id = m.Id,
                    Name = m.Name,
                    UI_SREF = m.UI_SREF
                }).ToList();
            }
        }
    }
}