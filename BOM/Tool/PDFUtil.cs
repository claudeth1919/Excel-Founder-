using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using BOM.Model;
using BOM.Tool;
using System.Windows.Forms;

namespace BOM.Tool
{
    public static class PDFUtil
    {
        private static readonly string CURRENT_PATH = Path.GetDirectoryName(Application.ExecutablePath);
        private static readonly iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);


        public static void CreatePreOrder(Order order)
        {
            Document doc = new Document(PageSize.LETTER);
            // Indicamos donde vamos a guardar el documento
            string path =  $@"{CURRENT_PATH}\PreOrder_{order.Id}.pdf";
            Util.DeleteFileIfExist(path);

            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(path, FileMode.Create));

            // Le colocamos el título y el autor
            // **Nota: Esto no será visible en el documento
            doc.AddTitle("PDF");
            doc.AddCreator("ALDS");

            // Abrimos el archivo
            doc.Open();

            AddHeader(doc,order, writer);

            const int FLAG_ITEM_LIMIT_FOR_NEW_PAGE = 10;
            if (order.MaterialList != null)
            {
                string lastMaterialPrinted = String.Empty;
                PdfPTable lastTable = null;
                int counter = 0;
                foreach (MaterialOrder material in order.MaterialList)
                {
                    //PdfPTable headertable = GetHeaderTable();
                    
                    Paragraph header = new Paragraph($"{material.OriginalCode}    ({material.ProviderName})");
                    //tittle.SpacingAfter = 5;
                    PdfPTable table;
                    if (lastMaterialPrinted != material.OriginalCode)
                    {
                        if (lastTable != null)
                        {
                            doc.Add(lastTable);
                            doc.Add(Chunk.NEWLINE);
                            if (FLAG_ITEM_LIMIT_FOR_NEW_PAGE == counter)
                            {
                                doc.NewPage();
                                AddHeader(doc, order, writer);
                                counter = 0;
                            }
                        }
                        
                        doc.Add(header);
                        table = GetMaterialTable();
                    }
                    else
                    {
                        table = lastTable;
                    }
                        
                    // Llenamos la tabla con información
                    PdfPCell clCode = new PdfPCell(new Phrase(material.OriginalCode, _standardFont))
                    {
                        BorderWidth = 0
                    };

                    PdfPCell clWantedAmount = new PdfPCell(new Phrase($"{material.WantedAmount}  {material.BOMUnit}", _standardFont))
                    {
                        BorderWidth = 0
                    };

                    PdfPCell clAvailableAmount = new PdfPCell(new Phrase($"{material.AvailableAmount}  {material.WarehouseUnit}", _standardFont))
                    {
                        BorderWidth = 0
                    };
                    PdfPCell clLocation = new PdfPCell(new Phrase(material.Location, _standardFont))
                    {
                        BorderWidth = 0
                    };
                    PdfPCell clProvider = new PdfPCell(new Phrase(material.ProviderName, _standardFont))
                    {
                        BorderWidth = 0
                    };

                    PdfPCell clKlt = new PdfPCell(new Phrase(material.Ktl, _standardFont))
                    {
                        BorderWidth = 0
                    };
                    PdfPCell clOriginName = new PdfPCell(new Phrase(material.OriginName, _standardFont))
                    {
                        BorderWidth = 0
                    };
                    PdfPCell clFoundAmount = new PdfPCell(new Phrase(String.Empty, _standardFont))
                    {
                        BorderWidth = 1,
                        BorderWidthBottom = 1f,
                        BorderWidthTop = 1f,
                        PaddingBottom = 1f,
                        PaddingLeft = 1f,
                        PaddingTop = 1f
                    };
                    PdfPCell clChecked = new PdfPCell(new Phrase(String.Empty, _standardFont))
                    {
                        BorderWidth = 1,
                        BorderWidthBottom = 1f,
                        BorderWidthTop = 1f,
                        PaddingBottom = 1f,
                        PaddingLeft = 1f,
                        PaddingTop = 1f
                    };
                    // Añadimos las celdas a la tabla
                    
                    table.AddCell(clWantedAmount);
                    table.AddCell(clAvailableAmount);

                    table.AddCell(clOriginName);
                    table.AddCell(clKlt);
                    table.AddCell(clLocation);
                    
                    table.AddCell(clFoundAmount);
                    table.AddCell(clChecked);

                    //headertable.AddCell(clCode);
                    //headertable.AddCell(clProvider);
                    //doc.Add(headertable);
                    lastTable = table;

                    
                    lastMaterialPrinted = material.OriginalCode;
                    counter++;
                }
                if (lastTable != null)
                {
                    doc.Add(lastTable);
                    doc.Add(Chunk.NEWLINE);
                }

            }
            
            //Close PDF


            doc.Close();
            writer.Close();
            System.Diagnostics.Process.Start(path);
        }

        public static void CreateAssignedOrderreport(Order order)
        {
            Document doc = new Document(PageSize.LETTER);
            // Indicamos donde vamos a guardar el documento
            string path =  $@"{CURRENT_PATH}\Order_{order.Id}.pdf";
            Util.DeleteFileIfExist(path);

            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(path, FileMode.Create));

            // Le colocamos el título y el autor
            // **Nota: Esto no será visible en el documento
            doc.AddTitle("PDF");
            doc.AddCreator("ALDS");

            // Abrimos el archivo
            doc.Open();

            AddHeader(doc,order, writer);

            const int FLAG_ITEM_LIMIT_FOR_NEW_PAGE = 10;
            if (order.MaterialList != null)
            {
                string lastMaterialPrinted = String.Empty;
                PdfPTable lastTable = null;
                int counter = 0;
                foreach (MaterialOrder material in order.MaterialList)
                {
                    //PdfPTable headertable = GetHeaderTable();
                    
                    Paragraph header = new Paragraph($"{material.OriginalCode}    ({material.ProviderName})");
                    //tittle.SpacingAfter = 5;
                    PdfPTable table;
                    if (lastMaterialPrinted != material.OriginalCode)
                    {
                        if (lastTable != null)
                        {
                            doc.Add(lastTable);
                            doc.Add(Chunk.NEWLINE);
                            if (FLAG_ITEM_LIMIT_FOR_NEW_PAGE == counter)
                            {
                                doc.NewPage();
                                AddHeader(doc, order, writer);
                                counter = 0;
                            }
                        }
                        
                        doc.Add(header);
                        table = GetTakenMaterialTable();
                    }
                    else
                    {
                        table = lastTable;
                    }
                        
                    // Llenamos la tabla con información
                    PdfPCell clCode = new PdfPCell(new Phrase(material.OriginalCode, _standardFont))
                    {
                        BorderWidth = 0
                    };

                    PdfPCell clChosenAmount = new PdfPCell(new Phrase($"{material.ChosenAmount} {material.Unit}", _standardFont))
                    {
                        BorderWidth = 0
                    };

                    PdfPCell clLocation = new PdfPCell(new Phrase(material.Location, _standardFont))
                    {
                        BorderWidth = 0
                    };
                    PdfPCell clProvider = new PdfPCell(new Phrase(material.ProviderName, _standardFont))
                    {
                        BorderWidth = 0
                    };

                    PdfPCell clKlt = new PdfPCell(new Phrase(material.Ktl, _standardFont))
                    {
                        BorderWidth = 0
                    };
                    PdfPCell clOriginName = new PdfPCell(new Phrase(material.OriginName, _standardFont))
                    {
                        BorderWidth = 0
                    };
                    // Añadimos las celdas a la tabla
                    
                    table.AddCell(clChosenAmount);

                    table.AddCell(clOriginName);
                    table.AddCell(clKlt);
                    table.AddCell(clLocation);
                    

                    //headertable.AddCell(clCode);
                    //headertable.AddCell(clProvider);
                    //doc.Add(headertable);
                    lastTable = table;

                    
                    lastMaterialPrinted = material.OriginalCode;
                    counter++;
                }
                if (lastTable != null)
                {
                    doc.Add(lastTable);
                    doc.Add(Chunk.NEWLINE);
                }

            }
            
            //Close PDF


            doc.Close();
            writer.Close();
            System.Diagnostics.Process.Start(path);
        }

        private static void AddHeader(Document doc, Order order, PdfWriter writer)
        {
            // Escribimos el encabezamiento en el documento
            string tittleString = $"Order N° {order.Id}";
            Paragraph tittle = new Paragraph(tittleString);
            //tittle.SpacingAfter = 5;
            doc.Add(tittle);

            // Add the header anchor to the page 


            //OnStartPage(writer, doc, tittleString);


            Barcode128 code128 = new Barcode128();
            code128.Code = order.Id.ToString();
            PdfContentByte cb = writer.DirectContent;
            cb.Rectangle(doc.PageSize.Width - 90f, 830f, 100f, 100f);
            cb.Stroke();
            Image img = code128.CreateImageWithBarcode(cb, BaseColor.BLACK, BaseColor.BLACK);

            doc.Add(img);

            doc.Add(Chunk.NEWLINE);
            Paragraph excelName = new Paragraph($"{GetProyectName(order.ExcelPath)}");
            //tittle.SpacingAfter = 5;
            doc.Add(excelName);


            // Creamos una tabla que contendrá el nombre, apellido y país
            // de nuestros visitante.
            doc.Add(Chunk.NEWLINE);
        }

        public static void OnStartPage(PdfWriter writer, Document document, string watermarkText)
        {
            float fontSize = 80;
            float xPosition = 300;
            float yPosition = 400;
            float angle = 45;
            try
            {
                PdfContentByte under = writer.DirectContentUnder;
                BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, BaseFont.EMBEDDED);
                under.BeginText();
                under.SetColorFill(BaseColor.LIGHT_GRAY);
                under.SetFontAndSize(baseFont, fontSize);
                under.ShowTextAligned(PdfContentByte.ALIGN_CENTER, watermarkText, xPosition, yPosition, angle);
                under.EndText();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }

        public static string GetProyectName(string excelPath)
        {
            string projectName = String.Empty;
            if (excelPath == null) return String.Empty;
            excelPath = excelPath.Replace(@"\\", "");
            string[] stringParts = excelPath.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
            try
            {
                if (stringParts[0].ToLower() == "schaeffler.com")
                {
                    projectName += stringParts[9];
                    projectName +=$" ({stringParts[12]})";
                    return projectName;
                }
            }
            catch (Exception ex)
            {

            }
            
            try
            {
                projectName = stringParts[4];
            }
            catch (Exception e)
            {
                return projectName;
            }
            try
            {
                projectName += $" ({stringParts[7]})";
            }
            catch (Exception e)
            {
                return projectName;
            }
            
            return projectName;
        }


        private static PdfPTable GetHeaderTable()
        {
            PdfPTable headertable = new PdfPTable(2)
            {
                WidthPercentage = 50
            };
            headertable.SetWidths(new float[] { 6f, 6f });
            PdfPCell clCode = new PdfPCell(new Phrase("Code", _standardFont))
            {
                BorderWidth = 0,
            };
            PdfPCell clProvider = new PdfPCell(new Phrase("Supplier", _standardFont))
            {
                BorderWidth = 0,
            };
            headertable.AddCell(clCode);
            headertable.AddCell(clProvider);
            return headertable;
        }

        private static PdfPTable GetMaterialTable()
        {

            PdfPTable table = new PdfPTable(7)
            {
                WidthPercentage = 100
            };

            PdfPCell clWantedAmount = new PdfPCell(new Phrase("Desired Amount", _standardFont))
            {
                BorderWidth = 0,
                BorderWidthBottom = 0.75f
            };

            PdfPCell clAvailableAmount = new PdfPCell(new Phrase("Available Amount", _standardFont))
            {
                BorderWidth = 0,
                BorderWidthBottom = 0.75f
            };

            PdfPCell clLocation = new PdfPCell(new Phrase("Location", _standardFont))
            {
                BorderWidth = 0,
                BorderWidthBottom = 0.75f
            };

            PdfPCell clKlt = new PdfPCell(new Phrase("KLT", _standardFont))
            {
                BorderWidth = 0,
                BorderWidthBottom = 0.75f
            };
            PdfPCell clOriginName = new PdfPCell(new Phrase("Origin Name", _standardFont))
            {
                BorderWidth = 0,
                BorderWidthBottom = 0.75f
            };
            PdfPCell clFoundAmount = new PdfPCell(new Phrase("Found Amount", _standardFont))
            {
                BorderWidth = 0,
                BorderWidthBottom = 0.75f
            };
            PdfPCell clChecked = new PdfPCell(new Phrase("Checked", _standardFont))
            {
                BorderWidth = 0,
                BorderWidthBottom = 0.75f
            };

            // Añadimos las celdas a la tabla


            table.AddCell(clWantedAmount);
            table.AddCell(clAvailableAmount);
            table.AddCell(clOriginName);
            table.AddCell(clKlt);
            table.AddCell(clLocation);
            table.AddCell(clFoundAmount);
            table.AddCell(clChecked);
            return table;
        }
        private static PdfPTable GetTakenMaterialTable()
        {

            PdfPTable table = new PdfPTable(4)
            {
                WidthPercentage = 100
            };

            PdfPCell clWantedAmount = new PdfPCell(new Phrase("Chosen Amount", _standardFont))
            {
                BorderWidth = 0,
                BorderWidthBottom = 0.75f
            };
            

            PdfPCell clLocation = new PdfPCell(new Phrase("Location", _standardFont))
            {
                BorderWidth = 0,
                BorderWidthBottom = 0.75f
            };

            PdfPCell clKlt = new PdfPCell(new Phrase("KLT", _standardFont))
            {
                BorderWidth = 0,
                BorderWidthBottom = 0.75f
            };
            PdfPCell clOriginName = new PdfPCell(new Phrase("Origin Name", _standardFont))
            {
                BorderWidth = 0,
                BorderWidthBottom = 0.75f
            };

            // Añadimos las celdas a la tabla


            table.AddCell(clWantedAmount);
            table.AddCell(clOriginName);
            table.AddCell(clKlt);
            table.AddCell(clLocation);
            return table;
        }

    }
}
