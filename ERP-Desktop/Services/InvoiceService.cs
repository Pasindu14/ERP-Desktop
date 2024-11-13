using ERP_Desktop.DBModels;
using ERP_Desktop.Helpers;
using ERP_Desktop.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP_Desktop.Services
{
    public class InvoiceService
    {
        private readonly ERPDesktopContext _context;

        public InvoiceService(ERPDesktopContext context)
        {
            _context = context;
        }

        // Create a new invoice with line items
        public async Task<bool> CreateInvoiceAsync(tblInvoiceMaster invoice, List<tblInvoiceLine> lineItems)
        {
            // Check if an invoice with the same number already exists
            bool invoiceExists = await _context.tblInvoiceMaster.AnyAsync(i => i.invoice_number == invoice.invoice_number);
            if (invoiceExists)
            {
                StatusMessageHelper.ShowMessage("An invoice with this number already exists. Please use a different invoice number.", true);
                return false;
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Add the invoice
                _context.tblInvoiceMaster.Add(invoice);
                await _context.SaveChangesAsync();

                // Process each line item
                foreach (var lineItem in lineItems)
                {
                    // Fetch the product to update its stock
                    var product = await _context.tblProductMaster.FirstOrDefaultAsync(p => p.prod_code == lineItem.prod_code);
                    if (product == null)
                    {
                        await transaction.RollbackAsync();
                        StatusMessageHelper.ShowMessage($"Product with code {lineItem.prod_code} not found. Invoice creation aborted.", true);
                        return false;
                    }

                    // Check if there's enough stock to fulfill the invoice
                    if (product.stock < lineItem.quantity)
                    {
                        await transaction.RollbackAsync();
                        StatusMessageHelper.ShowMessage($"Insufficient stock for product {product.prod_name} (Code: {product.prod_code}). Available stock: {product.stock}.", true);
                        return false;
                    }

                    // Deduct stock
                    product.stock -= lineItem.quantity;

                    // Associate each line item with the new invoice ID and add to the context
                    lineItem.invoice_id = invoice.invoice_id; // Set the foreign key
                    _context.tblInvoiceLine.Add(lineItem);
                }

                // Save changes to update the stock and add line items
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                StatusMessageHelper.ShowMessage("Invoice created successfully.", false);
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                StatusMessageHelper.ShowMessage($"An error occurred while creating the invoice: {ex.Message}", true);
                return false;
            }
        }



        // Fetch all invoices with their line items
        public async Task<List<tblInvoiceMaster>> FetchAllInvoicesAsync()
        {
            return await _context.tblInvoiceMaster
                .Include(invoice => invoice.tblInvoiceLine) // Include related line items
                .ToListAsync();
        }

        // Fetch a single invoice by ID with its line items
        public async Task<tblInvoiceMaster?> FetchInvoiceByIdAsync(int invoiceId)
        {
            return await _context.tblInvoiceMaster
                .Include(invoice => invoice.tblInvoiceLine)
                .FirstOrDefaultAsync(i => i.invoice_id == invoiceId);
        }

        // Update an existing invoice and its line items
        public async Task<bool> UpdateInvoiceAsync(tblInvoiceMaster updatedInvoice, List<tblInvoiceLine> updatedLineItems)
        {
            var existingInvoice = await _context.tblInvoiceMaster
                .Include(i => i.tblInvoiceLine)
                .FirstOrDefaultAsync(i => i.invoice_id == updatedInvoice.invoice_id);

            if (existingInvoice == null)
                return false;

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Update basic invoice details
                existingInvoice.invoice_number = updatedInvoice.invoice_number;
                existingInvoice.invoice_date = updatedInvoice.invoice_date;
                existingInvoice.total_amount = updatedInvoice.total_amount;
                existingInvoice.status = updatedInvoice.status;

                // Remove old line items
                _context.tblInvoiceLine.RemoveRange(existingInvoice.tblInvoiceLine);

                // Add updated line items
                foreach (var lineItem in updatedLineItems)
                {
                    lineItem.invoice_id = updatedInvoice.invoice_id;
                    _context.tblInvoiceLine.Add(lineItem);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                StatusMessageHelper.ShowMessage($"An error occurred while updating the invoice: {ex.Message}", true);
                return false;
            }
        }

        // Delete an invoice and its line items
        public async Task<bool> DeleteInvoiceAsync(int invoiceId)
        {
            var invoice = await _context.tblInvoiceMaster
                .Include(i => i.tblInvoiceLine)
                .FirstOrDefaultAsync(i => i.invoice_id == invoiceId);

            if (invoice == null)
                return false;

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Remove the line items first
                _context.tblInvoiceLine.RemoveRange(invoice.tblInvoiceLine);
                _context.tblInvoiceMaster.Remove(invoice);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                StatusMessageHelper.ShowMessage($"An error occurred while deleting the invoice: {ex.Message}", true);
                return false;
            }
        }


        public async Task<List<InvoiceLineItem>> FetchInvoiceLinesByInvoiceIdAsync(int invoiceId)
        {
            return await (from line in _context.tblInvoiceLine
                          join product in _context.tblProductMaster
                          on line.prod_code equals product.prod_code
                          where line.invoice_id == invoiceId
                          select new InvoiceLineItem
                          {
                              ProdCode = line.prod_code,
                              Quantity = line.quantity,
                              CurrentPrice = line.current_price,
                              OldPrice = line.old_price,
                              LineTotal = line.line_total,
                              ProductName = product.prod_name // Assuming this is the column for product name
                          }).ToListAsync();
        }


        // Fetch invoices within a specific date range
        public async Task<List<tblInvoiceMaster>> FetchInvoicesByDateRangeAsync(DateTime fromDate, DateTime toDate)
        {
            // Convert DateTime to DateOnly
            var fromDateOnly = DateOnly.FromDateTime(fromDate);
            var toDateOnly = DateOnly.FromDateTime(toDate);

            return await _context.tblInvoiceMaster
                .Where(invoice => invoice.invoice_date >= fromDateOnly && invoice.invoice_date <= toDateOnly)
                .Include(invoice => invoice.tblInvoiceLine) // Optionally include related line items
                .ToListAsync();
        }


        public async Task<List<DailySalesReport>> FetchDailySalesReportAsync(DateTime startDate, DateTime endDate)
        {
            var fromDateOnly = DateOnly.FromDateTime(startDate);
            var toDateOnly = DateOnly.FromDateTime(endDate);

            return await (from line in _context.tblInvoiceLine
                          join product in _context.tblProductMaster
                          on line.prod_code equals product.prod_code
                          join invoice in _context.tblInvoiceMaster
                          on line.invoice_id equals invoice.invoice_id
                          where invoice.invoice_date >= fromDateOnly && invoice.invoice_date <= toDateOnly
                          group new { line, product } by invoice.invoice_date into dateGroup
                          select new DailySalesReport
                          {
                              Date = dateGroup.Key,
                              TotalQuantity = dateGroup.Sum(g => g.line.quantity),
                              TotalSales = Math.Round(dateGroup.Sum(g => g.line.line_total), 2),
                              TotalProfit = Math.Round(dateGroup.Sum(g => (g.line.current_price - g.product.prod_cost_price) * g.line.quantity) ?? 0, 2)
                          }).ToListAsync();
        }


    }
}
