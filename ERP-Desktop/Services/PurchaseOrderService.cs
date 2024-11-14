using ERP_Desktop.DBModels;
using ERP_Desktop.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP_Desktop.Services
{
    public class PurchaseOrderService
    {
        private readonly ERPDesktopContext _context;

        public PurchaseOrderService(ERPDesktopContext context)
        {
            _context = context;
        }

        // Create a new purchase order with line items
        public async Task<bool> CreatePurchaseOrderAsync(tblPurchaseOrderMaster purchaseOrder, List<tblPurchaseOrderLine> lineItems)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Add the purchase order
                _context.tblPurchaseOrderMaster.Add(purchaseOrder);
                await _context.SaveChangesAsync();

                // Process each line item
                foreach (var lineItem in lineItems)
                {
                    // Fetch the product to update its stock
                    var product = await _context.tblProductMaster.FirstOrDefaultAsync(p => p.prod_code == lineItem.prod_code);
                    if (product == null)
                    {
                        await transaction.RollbackAsync();
                        StatusMessageHelper.ShowMessage($"Product with code {lineItem.prod_code} not found. Purchase order creation aborted.", true);
                        return false;
                    }

                    // Increase stock based on the quantity in the purchase order
                    product.stock += lineItem.quantity;

                    // Associate each line item with the new purchase order ID and add to the context
                    lineItem.purchase_order_id = purchaseOrder.purchase_order_id;
                    _context.tblPurchaseOrderLine.Add(lineItem);
                }

                // Save changes to update the stock and add line items
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                StatusMessageHelper.ShowMessage("Purchase order created successfully.", false);
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                StatusMessageHelper.ShowMessage($"An error occurred while creating the purchase order: {ex.Message}", true);
                return false;
            }
        }

        // Fetch all purchase orders with their line items
        public async Task<List<tblPurchaseOrderMaster>> FetchAllPurchaseOrdersAsync()
        {
            return await _context.tblPurchaseOrderMaster
                .Include(po => po.tblPurchaseOrderLine)
                .ToListAsync();
        }

        // Fetch a single purchase order by ID with its line items
        public async Task<tblPurchaseOrderMaster?> FetchPurchaseOrderByIdAsync(int purchaseOrderId)
        {
            return await _context.tblPurchaseOrderMaster
                .Include(po => po.tblPurchaseOrderLine)
                .FirstOrDefaultAsync(po => po.purchase_order_id == purchaseOrderId);
        }

        public async Task<List<tblPurchaseOrderMaster>> FetchPurchaseOrdersByDateRangeAsync(DateTime fromDate, DateTime toDate)
        {
            var fromDateOnly = DateOnly.FromDateTime(fromDate);
            var toDateOnly = DateOnly.FromDateTime(toDate);

            return await _context.tblPurchaseOrderMaster
                .Where(po => po.purchase_order_date >= fromDateOnly && po.purchase_order_date <= toDateOnly)
                .Include(po => po.tblPurchaseOrderLine) // Include line items
                .ToListAsync();
        }

        // Update an existing purchase order and its line items
        public async Task<bool> UpdatePurchaseOrderAsync(tblPurchaseOrderMaster updatedPurchaseOrder, List<tblPurchaseOrderLine> updatedLineItems)
        {
            var existingPurchaseOrder = await _context.tblPurchaseOrderMaster
                .Include(po => po.tblPurchaseOrderLine)
                .FirstOrDefaultAsync(po => po.purchase_order_id == updatedPurchaseOrder.purchase_order_id);

            if (existingPurchaseOrder == null)
                return false;

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Update basic purchase order details
                existingPurchaseOrder.purchase_order_number = updatedPurchaseOrder.purchase_order_number;
                existingPurchaseOrder.purchase_order_date = updatedPurchaseOrder.purchase_order_date;
                existingPurchaseOrder.total_amount = updatedPurchaseOrder.total_amount;
                existingPurchaseOrder.status = updatedPurchaseOrder.status;

                // Adjust stock based on existing line items
                foreach (var lineItem in existingPurchaseOrder.tblPurchaseOrderLine)
                {
                    var product = await _context.tblProductMaster.FirstOrDefaultAsync(p => p.prod_code == lineItem.prod_code);
                    if (product != null)
                    {
                        product.stock -= lineItem.quantity; // Revert stock based on existing items
                    }
                }

                // Remove old line items
                _context.tblPurchaseOrderLine.RemoveRange(existingPurchaseOrder.tblPurchaseOrderLine);

                // Add updated line items and adjust stock
                foreach (var lineItem in updatedLineItems)
                {
                    var product = await _context.tblProductMaster.FirstOrDefaultAsync(p => p.prod_code == lineItem.prod_code);
                    if (product == null)
                    {
                        await transaction.RollbackAsync();
                        StatusMessageHelper.ShowMessage($"Product with code {lineItem.prod_code} not found. Purchase order update aborted.", true);
                        return false;
                    }

                    // Increase stock based on updated quantity
                    product.stock += lineItem.quantity;

                    lineItem.purchase_order_id = updatedPurchaseOrder.purchase_order_id;
                    _context.tblPurchaseOrderLine.Add(lineItem);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                StatusMessageHelper.ShowMessage($"An error occurred while updating the purchase order: {ex.Message}", true);
                return false;
            }
        }

        // Delete a purchase order and its line items
        public async Task<bool> DeletePurchaseOrderAsync(int purchaseOrderId)
        {
            var purchaseOrder = await _context.tblPurchaseOrderMaster
                .Include(po => po.tblPurchaseOrderLine)
                .FirstOrDefaultAsync(po => po.purchase_order_id == purchaseOrderId);

            if (purchaseOrder == null)
                return false;

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Revert stock for each line item
                foreach (var lineItem in purchaseOrder.tblPurchaseOrderLine)
                {
                    var product = await _context.tblProductMaster.FirstOrDefaultAsync(p => p.prod_code == lineItem.prod_code);
                    if (product != null)
                    {
                        product.stock -= lineItem.quantity;
                    }
                }

                // Remove line items and the purchase order
                _context.tblPurchaseOrderLine.RemoveRange(purchaseOrder.tblPurchaseOrderLine);
                _context.tblPurchaseOrderMaster.Remove(purchaseOrder);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                StatusMessageHelper.ShowMessage($"An error occurred while deleting the purchase order: {ex.Message}", true);
                return false;
            }
        }

        // Fetch all line items for a specific purchase order
        public async Task<List<tblPurchaseOrderLine>> FetchPurchaseOrderLinesByOrderIdAsync(int purchaseOrderId)
        {
            return await _context.tblPurchaseOrderLine
                .Where(line => line.purchase_order_id == purchaseOrderId)
                .ToListAsync();
        }


        public async Task<List<DailyPurchaseReport>> FetchDailyPurchaseReportAsync(DateTime startDate, DateTime endDate)
        {
            var fromDateOnly = DateOnly.FromDateTime(startDate);
            var toDateOnly = DateOnly.FromDateTime(endDate);

            return await (from line in _context.tblPurchaseOrderLine
                          join product in _context.tblProductMaster
                          on line.prod_code equals product.prod_code
                          join purchaseOrder in _context.tblPurchaseOrderMaster
                          on line.purchase_order_id equals purchaseOrder.purchase_order_id
                          where purchaseOrder.purchase_order_date >= fromDateOnly && purchaseOrder.purchase_order_date <= toDateOnly
                          group new { line, product } by purchaseOrder.purchase_order_date into dateGroup
                          select new DailyPurchaseReport
                          {
                              Date = dateGroup.Key,
                              TotalQuantity = dateGroup.Sum(g => g.line.quantity),
                              TotalPurchaseAmount = Math.Round(dateGroup.Sum(g => g.line.line_total), 2),
                          }).ToListAsync();
        }
    }
}
