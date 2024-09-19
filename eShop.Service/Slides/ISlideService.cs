using eShop.ViewModels.Slides;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Service.Slides
{
    public interface ISlideService
    {
        Task<List<SlideVm>> GetAll();
    }
}
