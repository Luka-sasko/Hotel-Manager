using HotelManager.Common;
using HotelManager.Model;
using HotelManager.Model.Common;
using HotelManager.Repository.Common;
using HotelManager.Service.Common;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Linq;
using System.Net.Configuration;
using System.Configuration;
using System.Web;
using Microsoft.AspNet.Identity;

namespace HotelManager.Service
{
    public class ReceiptService : IReceiptService
    {
        private readonly IReceiptRepository _receiptRepository;
        private readonly IServiceInvoiceRepository _invoiceServiceRepository;

        public ReceiptService(IReceiptRepository receiptRepository, IServiceInvoiceRepository invoiceServiceRepository)
        {
            _receiptRepository = receiptRepository;
            _invoiceServiceRepository = invoiceServiceRepository;
        }

        public async Task<PagedList<IReceipt>> GetAllAsync([FromUri]ReceiptFilter filter, Sorting sorting, Paging paging)
        {
            try
            {
                return await _receiptRepository.GetAllAsync(filter, sorting, paging);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<IInvoiceReceipt> GetByIdAsync(Guid id)
        {
            try
            {
                return await _receiptRepository.GetByIdAsync(id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<PagedList<IServiceInvoiceHistory>> GetServiceInvoiceByInvoiceIdAsync(Guid id, Sorting sorting)
        {
            try
            {
                return await _invoiceServiceRepository.GetServiceInvoiceByInvoiceIdAsync(id,sorting);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            return await _invoiceServiceRepository.DeleteAsync(id);
        }

        public async Task<PagedList<IServiceInvoice>> GetAllInvoiceServiceAsync([FromUri]Sorting sorting, Paging paging)
        {
            try
            {
                return await _invoiceServiceRepository.GetAllInvoiceServiceAsync(sorting, paging);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<string> CreateInvoiceServiceAsync(IServiceInvoice serviceInvoice)
        {
            var userId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
            serviceInvoice.Id = Guid.NewGuid();
            serviceInvoice.CreatedBy = userId;
            serviceInvoice.UpdatedBy = userId;
            serviceInvoice.DateCreated = DateTime.UtcNow;
            serviceInvoice.DateUpdated = DateTime.UtcNow;
            serviceInvoice.IsActive = true;
            try
            {
                return await _invoiceServiceRepository.CreateInvoiceServiceAsync(serviceInvoice);
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public async Task<Invoice> CreateInvoiceAsync(Invoice invoice)
        {
            try { 
                return await    _receiptRepository.CreateInvoiceAsync(invoice);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<Invoice> PutTotalPriceAsync(Guid invoiceId,InvoiceUpdate invoiceUpdate)
        {
            try
            {
                return await _receiptRepository.PutTotalPriceAsync(invoiceId,invoiceUpdate);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

       

        public async Task<bool> SendReceiptAsync(Guid id)
        {
            IInvoiceReceipt receipt = await _receiptRepository.GetByIdAsync(id);
            if (receipt == null)
            {
                throw new Exception("Receipt not found");
            }

            InvoicePaid invoicePaid = new InvoicePaid
            {
                IsPaid = true,
                UpdatedBy = Guid.Parse(HttpContext.Current.User.Identity.GetUserId()),
                DateUpdated = DateTime.UtcNow
            };

            try
            {
                bool result = await _receiptRepository.PutIsPaidAsync(id, invoicePaid);
                if (!result)
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            PdfDocument pdfDocument = await CreateReceiptPdf(receipt);
            MemoryStream memoryStream = SavePdfToMemoryStream(pdfDocument);

            await SendEmailWithAttachment(receipt, memoryStream);

            return true;
        }

        private async Task<PdfDocument> CreateReceiptPdf(IInvoiceReceipt receipt)
        {
            PdfDocument pdfDocument = new PdfDocument();
            PdfPage page = pdfDocument.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);

            var services = await _invoiceServiceRepository.GetServiceInvoiceByInvoiceIdAsync(receipt.Id, null);
            AddReceiptContent(gfx, receipt, services.Items);

            return pdfDocument;
        }

        private void AddReceiptContent(XGraphics gfx, IInvoiceReceipt receipt, List<IServiceInvoiceHistory> services)
        {
            XFont titleFont = new XFont("Arial", 18, XFontStyle.Bold);
            XFont regularFont = new XFont("Arial", 12);
            XFont boldFont = new XFont("Arial", 12, XFontStyle.Bold);

            gfx.DrawString("Receipt", titleFont, XBrushes.Black, new XRect(0, 20, gfx.PageSize.Width, 0), XStringFormats.TopCenter);

            gfx.DrawString($"Receipt Number: {receipt.InvoiceNumber}", regularFont, XBrushes.Black, 50, 70);
            gfx.DrawString($"Issue Date: {DateTime.Now:dd.MM.yyyy}", regularFont, XBrushes.Black, 50, 90);

            gfx.DrawString($"Check-In Date: {receipt.Reservation.CheckInDate:dd.MM.yyyy}", regularFont, XBrushes.Black, 50, 110);
            gfx.DrawString($"Check-Out Date: {receipt.Reservation.CheckOutDate:dd.MM.yyyy}", regularFont, XBrushes.Black, 50, 130);
            gfx.DrawString($"Room Number: {receipt.RoomNumber}", regularFont, XBrushes.Black, 50, 150);

            gfx.DrawString("Guest Information:", regularFont, XBrushes.Black, 50, 180);
            gfx.DrawString($"Name: {receipt.User.FirstName} {receipt.User.LastName}", regularFont, XBrushes.Black, 50, 200);
            gfx.DrawString($"Email: {receipt.User.Email}", regularFont, XBrushes.Black, 50, 220);

            gfx.DrawString("\nReservation Details:", regularFont, XBrushes.Black, 50, 270);
            gfx.DrawString("Reservation number: " + receipt.Reservation.ReservationNumber, regularFont, XBrushes.Black, 50, 290);
            gfx.DrawString($"Number of Nights: {receipt.Reservation.CheckOutDate.Subtract(receipt.Reservation.CheckInDate).Days}", regularFont, XBrushes.Black, 50, 310);
            gfx.DrawString($"Price per Night: {receipt.Reservation.PricePerNight:0.00}€", regularFont, XBrushes.Black, 50, 330);
            gfx.DrawString($"Subtotal: {receipt.TotalPrice:0.00}€", boldFont, XBrushes.Black, 50, 350);
            int yOffset = 370;
            if (receipt.Discount != null)
            {
                gfx.DrawString($"Discount: {receipt.Discount.Code} ({receipt.Discount.Percent}%)", regularFont, XBrushes.Black, 50, 370);
                receipt.TotalPrice -= receipt.TotalPrice * receipt.Discount.Percent / 100;
                gfx.DrawString($"Subtotal with discount: {receipt.TotalPrice:0.00}€", boldFont, XBrushes.Black, 50, 390);
                yOffset = 410;

            }

            gfx.DrawString("\nServices (price):", regularFont, XBrushes.Black, 50, yOffset);
            yOffset +=20;
            var servicesMap = new Dictionary<string, int>();
            decimal subTotal = 0;

            foreach (var service in services)
            {
                if (servicesMap.ContainsKey(service.ServiceName))
                {
                    servicesMap[service.ServiceName] += service.Quantity;
                }
                else
                {
                    servicesMap.Add(service.ServiceName, service.Quantity);
                }
                subTotal += service.Quantity * service.Price;
            }

            foreach (var entry in servicesMap)
            {
                var service = services.First(s => s.ServiceName == entry.Key);
                gfx.DrawString($"{entry.Key} ({service.Price}€) x {entry.Value}", regularFont, XBrushes.Black, 50, yOffset);
                yOffset += 20;
            }

            gfx.DrawString($"Subtotal: {subTotal}€", boldFont, XBrushes.Black, 50, yOffset);

            gfx.DrawString("Total Price:", boldFont, XBrushes.Black, 50, yOffset + 30);
            gfx.DrawString($"{receipt.TotalPrice + subTotal}€", boldFont, XBrushes.Black, 50, yOffset + 50);
        }

        private MemoryStream SavePdfToMemoryStream(PdfDocument pdfDocument)
        {
            MemoryStream memoryStream = new MemoryStream();
            pdfDocument.Save(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return memoryStream;
        }

        private async Task SendEmailWithAttachment(IInvoiceReceipt receipt, MemoryStream memoryStream)
        {
            using (MailMessage mailMessage = new MailMessage("hotel@example.com", receipt.User.Email))
            {
                mailMessage.Subject = "Receipt  " + receipt.InvoiceNumber;
                mailMessage.Body = $"Dear {receipt.User.FirstName} {receipt.User.LastName}, \nThank you for choosing our hotel for your recent stay. " +
                    $"It was a pleasure to have you as our guest.\nYou can find a copy of your receipt in attachment.";

                mailMessage.Attachments.Add(new Attachment(memoryStream, "receipt.pdf", "application/pdf"));

                using (SmtpClient smtpClient = new SmtpClient())
                {
                    var smtpSection = ConfigurationManager.GetSection("system.net/mailSettings/smtp") as SmtpSection;
                    if (smtpSection != null)
                    {
                        smtpClient.Host = smtpSection.Network.Host;
                        smtpClient.Port = smtpSection.Network.Port;
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = new NetworkCredential(smtpSection.Network.UserName, smtpSection.Network.Password);
                        smtpClient.EnableSsl = smtpSection.Network.EnableSsl;
                    }
                    else
                    {
                        throw new ConfigurationErrorsException("SMTP settings are missing in the configuration file.");
                    }

                    await smtpClient.SendMailAsync(mailMessage);
                }
            }
        }
    }
}