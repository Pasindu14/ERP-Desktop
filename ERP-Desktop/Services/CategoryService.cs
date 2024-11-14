using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP_Desktop.DBModels;
using ERP_Desktop.Helpers;
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

        public async Task<List<tblCategoryMaster>> FetchActiveCategoriesAsync()
        {
            return await _context.tblCategoryMaster
                                 .Where(category => category.cat_status == 1)
                                 .ToListAsync();
        }

        public async Task<bool> InsertCategoryAsync(tblCategoryMaster category)
        {
            try
            {
                // Check if a category with the same code already exists
                bool isDuplicate = await _context.tblCategoryMaster
                    .AnyAsync(c => c.cat_code == category.cat_code);

                if (isDuplicate)
                {
                    StatusMessageHelper.ShowMessage("Category code already exists. Please use a different code.", true);
                    return false;
                }

                _context.tblCategoryMaster.Add(category);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                StatusMessageHelper.ShowMessage($"An error occurred while adding the category: {ex.Message}", true);
                return false;
            }
        }

        // Update an existing category
        public async Task<bool> UpdateCategoryAsync(tblCategoryMaster category)
        {
            var existingCategory = await _context.tblCategoryMaster
                .FirstOrDefaultAsync(c => c.cat_code == category.cat_code);

            if (existingCategory == null)
                return false;

            // Check if any field has changed before updating
            bool isModified = existingCategory.cat_name != category.cat_name || existingCategory.cat_status != category.cat_status;

            if (!isModified)
            {
                // No changes were made, so skip the update and return success
                return true;
            }

            // Apply changes
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
