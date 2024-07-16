using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebBH_Shopping.Data.Components
{
    public class BrandsViewComponent : ViewComponent
    {
        private readonly WebBH_ShoppingContext _context;

        public BrandsViewComponent(WebBH_ShoppingContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync() => View(await _context.BrandModel.ToListAsync());

       
    }
}
