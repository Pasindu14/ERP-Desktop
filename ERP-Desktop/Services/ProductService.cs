using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP_Desktop.DBModels;
using ERP_Desktop.Helpers;
using Microsoft.EntityFrameworkCore;

namespace ERP_Desktop.Services
{
    public class ProductService
    {
        private readonly ERPDesktopContext _context;

        public ProductService(ERPDesktopContext context)
        {
            _context = context;
        }

        // Fetch all products
        public async Task<List<tblProductMaster>> FetchAllProductsAsync()
        {
            return await _context.tblProductMaster.ToListAsync();
        }

        // Method to fetch a single product by Product Code
        public async Task<tblProductMaster?> FetchProductByCodeAsync(int prodCode)
        {
            return await _context.tblProductMaster.FirstOrDefaultAsync(p => p.prod_code == prodCode);
        }


        // Insert a new product
        public async Task<bool> InsertProductAsync(tblProductMaster product)
        {
            try
            {
                // Check if a product with the same code already exists
                bool isDuplicate = await _context.tblProductMaster
                    .AnyAsync(p => p.prod_code == product.prod_code);

                if (isDuplicate)
                {
                    StatusMessageHelper.ShowMessage("Product code already exists. Please use a different code.", true);
                    return false;
                }

                _context.tblProductMaster.Add(product);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                StatusMessageHelper.ShowMessage($"An error occurred while adding the product: {ex.Message}", true);
                return false;
            }
        }

        // Update an existing product
        public async Task<bool> UpdateProductAsync(tblProductMaster product)
        {
            var existingProduct = await _context.tblProductMaster
                .FirstOrDefaultAsync(p => p.prod_code == product.prod_code);

            if (existingProduct == null)
                return false;

            // Check if any field has changed before updating
            bool isModified = existingProduct.prod_code_usergen != product.prod_code_usergen ||
                              existingProduct.prod_name != product.prod_name ||
                              existingProduct.prod_desc != product.prod_desc ||
                              existingProduct.prod_cost_price != product.prod_cost_price ||
                              existingProduct.prod_sales_price != product.prod_sales_price ||
                              existingProduct.prod_cat != product.prod_cat ||
                              existingProduct.prod_status != product.prod_status;

            if (!isModified)
            {
                // No changes were made, so skip the update and return success
                return true;
            }

            // Apply changes
            existingProduct.prod_code_usergen = product.prod_code_usergen;
            existingProduct.prod_name = product.prod_name;
            existingProduct.prod_desc = product.prod_desc;
            existingProduct.prod_cost_price = product.prod_cost_price;
            existingProduct.prod_sales_price = product.prod_sales_price;
            existingProduct.prod_cat = product.prod_cat;
            existingProduct.prod_status = product.prod_status;

            return await _context.SaveChangesAsync() > 0;
        }

        // Delete a product by ProdCode
        public async Task<bool> DeleteProductAsync(int prodCode)
        {
            var product = await _context.tblProductMaster.FirstOrDefaultAsync(p => p.prod_code == prodCode);

            if (product == null)
                return false;

            _context.tblProductMaster.Remove(product);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<tblProductMaster?> FetchProductStockByCodeAsync(int prodCode)
        {
            return await _context.tblProductMaster
                .Where(p => p.prod_code == prodCode)
                .Select(p => new tblProductMaster
                {
                    prod_code = p.prod_code,
                    prod_code_usergen = p.prod_code_usergen,
                    prod_name = p.prod_name,
                    prod_desc = p.prod_desc,
                    prod_cost_price = p.prod_cost_price,
                    prod_sales_price = p.prod_sales_price,
                    prod_cat = p.prod_cat,
                    stock = p.stock,  // Fetches current stock
                    prod_status = p.prod_status
                })
                .FirstOrDefaultAsync();
        }
    }
}
