using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP_Desktop.DBModels;
using Microsoft.EntityFrameworkCore;

namespace ERP_Desktop.Services
{
    public class CategoryService
    {
        private readonly ERPDesktopContext _context;

        public CategoryService(ERPDesktopContext context)
        {
            _context = context;
        }

        // Fetch all categories
        public async Task<List<tblCategoryMaster>> FetchAllCategoriesAsync()
        {
            return await _context.tblCategoryMaster.ToListAsync();
        }

        // Insert a new category
        public async Task<bool> InsertCategoryAsync(tblCategoryMaster category)
        {
            _context.tblCategoryMaster.Add(category);
            return await _context.SaveChangesAsync() > 0;
        }

        // Update an existing category
        public async Task<bool> UpdateCategoryAsync(tblCategoryMaster category)
        {
            var existingCategory = await _context.tblCategoryMaster
                .FirstOrDefaultAsync(c => c.cat_code == category.cat_code);

            if (existingCategory == null)
                return false;

            existingCategory.cat_name = category.cat_name;
            existingCategory.cat_status = category.cat_status;

            return await _context.SaveChangesAsync() > 0;
        }

        // Delete a category by CatCode
        public async Task<bool> DeleteCategoryAsync(string catCode)
        {
            var category = await _context.tblCategoryMaster.FirstOrDefaultAsync(c => c.cat_code==catCode);

            if (category == null)
                return false;

            _context.tblCategoryMaster.Remove(category);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
