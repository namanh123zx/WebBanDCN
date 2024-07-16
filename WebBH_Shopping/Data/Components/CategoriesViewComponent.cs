using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebBH_Shopping.Data.Components
{
    public class CategoriesViewComponent : ViewComponent
    {
        private readonly WebBH_ShoppingContext _context;

        public CategoriesViewComponent(WebBH_ShoppingContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync() => View(await _context.CategoryModel.ToListAsync());

       
    }
}
