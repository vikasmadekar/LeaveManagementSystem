using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using LeaveManagementSystem.Models;
using System.IO;
using LeaveManagementSystem.PDFHelper; // ✅ Correct namespace


namespace LeaveManagementSystem.PDFHelper // ✅ Updated to match folder
{
    public static class PDF_Generator
    {
        public static byte[] GenerateEmployeePdf(Employe employee) // ✅ FIXED method name
        {
            using var ms = new MemoryStream();
            using var writer = new PdfWriter(ms);
            using var pdf = new PdfDocument(writer);
            var document = new Document(pdf);

            document.Add(new Paragraph("Employee Detail Report")
                .SetFontSize(18)
                .SetBold()
                .SetTextAlignment(TextAlignment.CENTER));

            document.Add(new Paragraph(" "));

            var table = new Table(2).UseAllAvailableWidth();
            table.AddCell("ID");
            table.AddCell(employee.EmployeId.ToString());
            table.AddCell("Full Name");
            table.AddCell(employee.Name);
            table.AddCell("Designation");
            table.AddCell(employee.Designation);
            table.AddCell("Passaword");
            table.AddCell(employee.Password.ToString("C"));
            table.AddCell("Email");
            table.AddCell(employee.Email);
            table.AddCell("Department");
            table.AddCell(employee.Department);
            table.AddCell(employee.Role);
            table.AddCell("Role");
            table.AddCell(employee.RefreshToken);
            table.AddCell("RefreshToken");


            document.Add(table);
            document.Close();

            return ms.ToArray();
        }
    }
}
