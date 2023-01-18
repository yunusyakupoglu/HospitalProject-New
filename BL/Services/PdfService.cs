using BL.IServices;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class PdfService : IPdfService
    {
        public async Task<bool> GenerateAsync(string patientNameSurname, string doctorName, string appointmentDate, string appointmentTime, string note)
        {
            try
            {
                iTextSharp.text.Document pdfDocument = new iTextSharp.text.Document();
                PdfWriter.GetInstance(pdfDocument, new FileStream(@"C:rapor2.pdf", FileMode.Create));
                pdfDocument.Open();
                pdfDocument.Add(new Paragraph(patientNameSurname));
                pdfDocument.Add(new Paragraph(doctorName));
                pdfDocument.Add(new Paragraph(appointmentDate));
                pdfDocument.Add(new Paragraph(appointmentTime));
                pdfDocument.Add(new Paragraph(note));
                pdfDocument.Close();

                return true;
            }
            catch (Exception e)
            {

                throw;
            }

        }

    }
}
