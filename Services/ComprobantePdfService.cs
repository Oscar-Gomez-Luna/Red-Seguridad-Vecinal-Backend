using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Globalization;
using System.IO;

public class ComprobantePdfService
{
    private readonly string _logoPath;
    private readonly string _nombreNegocio;

    public ComprobantePdfService(string logoPath, string nombreNegocio = "RED DE SEGURIDAD VECINAL")
    {
        _logoPath = logoPath;
        _nombreNegocio = nombreNegocio;
    }

    public byte[] GenerarPdfTicket(Pago pago, List<DetallePago> detalles)
    {
        // Tamaño carta
        var pageSize = PageSize.Letter;
        using var ms = new MemoryStream();
        var document = new Document(pageSize, 50, 50, 40, 40);
        var writer = PdfWriter.GetInstance(document, ms);
        document.Open();

        // Colores corporativos
        var colorVerde = new BaseColor(0, 102, 102); // Verde azulado
        var colorGrisOscuro = new BaseColor(51, 51, 51);

        // Fuentes
        var baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
        var baseFontBold = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, false);
        var baseFontItalic = BaseFont.CreateFont(BaseFont.HELVETICA_OBLIQUE, BaseFont.CP1252, false);

        var fontLogo = new Font(baseFontBold, 10, Font.BOLD, colorVerde);
        var fontTitle = new Font(baseFontBold, 14, Font.BOLD, BaseColor.Black);
        var fontSubtitle = new Font(baseFontItalic, 10, Font.ITALIC, BaseColor.Black);
        var fontSectionTitle = new Font(baseFontBold, 11, Font.BOLD, BaseColor.Black);
        var fontNormal = new Font(baseFont, 10, Font.NORMAL, BaseColor.Black);
        var fontSmall = new Font(baseFont, 9, Font.NORMAL, BaseColor.Black);
        var fontTableHeader = new Font(baseFontBold, 9, Font.BOLD, BaseColor.Black);
        var fontPendiente = new Font(baseFontBold, 9, Font.BOLD, BaseColor.Red);

        // Logo - CORREGIDO con mejor manejo de errores
        if (File.Exists(_logoPath))
        {
            try
            {
                var img = Image.GetInstance(_logoPath);
                img.Alignment = Element.ALIGN_CENTER;
                img.ScaleToFit(120f, 120f);
                document.Add(img);
            }
            catch (Exception ex)
            {
                // Log del error para debugging
                Console.WriteLine($"Error al cargar logo: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine($"Logo no encontrado en: {_logoPath}");
        }

        // Nombre del negocio
        document.Add(new Paragraph(_nombreNegocio, fontLogo)
        {
            Alignment = Element.ALIGN_CENTER,
            SpacingAfter = 10f
        });

        // Título
        document.Add(new Paragraph("COMPROBANTE DE PAGO", fontTitle)
        {
            Alignment = Element.ALIGN_CENTER,
            SpacingAfter = 5f
        });

        // Fecha
        document.Add(new Paragraph($"Fecha: {pago.FechaPago:dd 'de' MMMM, yyyy}", fontSubtitle)
        {
            Alignment = Element.ALIGN_CENTER,
            SpacingAfter = 15f
        });

        // Línea separadora
        document.Add(new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(1f, 100f, BaseColor.Black, Element.ALIGN_CENTER, -2)))
        {
            SpacingAfter = 10f
        });

        // INFORMACIÓN DEL RESIDENTE
        document.Add(new Paragraph("INFORMACIÓN DEL RESIDENTE", fontSectionTitle)
        {
            SpacingAfter = 8f
        });

        var usuario = pago.Usuario;
        var persona = usuario?.Persona;

        string nombreCompleto = persona != null
            ? $"{persona.Nombre} {persona.ApellidoPaterno} {persona.ApellidoMaterno}"
            : $"Usuario #{pago.UsuarioID}";

        string direccion = usuario != null
            ? $"{usuario.Calle}, Número. #{usuario.NumeroCasa}" : "Sin dirección";
        string telefono = usuario?.Persona.Telefono ?? "Sin teléfono";
        string correo = usuario?.Persona.Email ?? "Sin correo";

        // Crear fuente en negrita para las etiquetas
        var fontBold = new Font(baseFontBold, 10, Font.BOLD, BaseColor.Black);

        // Agregar campos con etiquetas en negrita
        var nombreParagraph = new Paragraph();
        nombreParagraph.Add(new Chunk("Nombre: ", fontBold));
        nombreParagraph.Add(new Chunk(nombreCompleto, fontNormal));
        document.Add(nombreParagraph);

        var direccionParagraph = new Paragraph();
        direccionParagraph.Add(new Chunk("Dirección: ", fontBold));
        direccionParagraph.Add(new Chunk(direccion, fontNormal));
        document.Add(direccionParagraph);

        var telefonoParagraph = new Paragraph();
        telefonoParagraph.Add(new Chunk("Teléfono: ", fontBold));
        telefonoParagraph.Add(new Chunk(telefono, fontNormal));
        document.Add(telefonoParagraph);

        var correoParagraph = new Paragraph();
        correoParagraph.Add(new Chunk("Correo: ", fontBold));
        correoParagraph.Add(new Chunk(correo, fontNormal));
        document.Add(correoParagraph);

        document.Add(new Paragraph(" ", fontNormal));

        bool esMantenimiento = detalles.Any(d => d.CargoMantenimientoID.HasValue || d.CargoMantenimiento != null);
        bool esServicio = detalles.Any(d => d.CargoServicioID.HasValue || d.CargoServicio != null);

        string tituloPago;
        if (esMantenimiento && esServicio)
        {
            tituloPago = "PAGO DE MANTENIMIENTO Y SERVICIOS";
        }
        else if (esMantenimiento)
        {
            tituloPago = "PAGO DE MANTENIMIENTO";
        }
        else if (esServicio)
        {
            tituloPago = "PAGO DE SERVICIO";
        }
        else
        {
            tituloPago = "PAGO";
        }

        // Ícono y título de pago
        var iconTable = new PdfPTable(2) { WidthPercentage = 100f };
        iconTable.SetWidths(new float[] { 5f, 95f });
        iconTable.SpacingAfter = 5f;

        // Celda con símbolo +
        var iconCell = new PdfPCell(new Phrase("+", new Font(baseFontBold, 16, Font.BOLD, colorVerde)))
        {
            Border = Rectangle.BOX,
            BorderColor = colorVerde,
            BorderWidth = 1.5f,
            Padding = 3f,
            HorizontalAlignment = Element.ALIGN_CENTER,
            VerticalAlignment = Element.ALIGN_MIDDLE
        };
        iconTable.AddCell(iconCell);

        // USAR EL TÍTULO DINÁMICO
        var titleCell = new PdfPCell(new Phrase(tituloPago, fontSectionTitle))
        {
            Border = Rectangle.NO_BORDER,
            Padding = 5f,
            VerticalAlignment = Element.ALIGN_MIDDLE
        };
        iconTable.AddCell(titleCell);

        document.Add(iconTable);

        // Tabla de conceptos - AHORA CON 4 COLUMNAS
        var table = new PdfPTable(4) { WidthPercentage = 100f };
        table.SetWidths(new float[] { 25f, 35f, 20f, 20f }); // Concepto, Descripción, Monto Pagado, Adeudo
        table.SpacingAfter = 5f;

        // Encabezados
        table.AddCell(CreateTableCell("Concepto", fontTableHeader, Element.ALIGN_CENTER, true));
        table.AddCell(CreateTableCell("Descripción", fontTableHeader, Element.ALIGN_CENTER, true));
        table.AddCell(CreateTableCell("Monto Pagado", fontTableHeader, Element.ALIGN_CENTER, true));
        table.AddCell(CreateTableCell("Adeudo", fontTableHeader, Element.ALIGN_CENTER, true));

        // Detalles
        decimal subtotal = 0;
        foreach (var d in detalles)
        {
            string concepto = "Cargo";
            string descripcion = "Descripción del cargo";
            decimal adeudo = 0;

            if (d.CargoMantenimiento is not null)
            {
                concepto = "Mantenimiento";
                descripcion = d.CargoMantenimiento.Concepto;
                adeudo = d.CargoMantenimiento.SaldoPendiente; // Adeudo después del pago
            }
            else if (d.CargoServicio is not null)
            {
                concepto = "Servicio";
                descripcion = d.CargoServicio.Concepto;
                adeudo = d.CargoServicio.SaldoPendiente; // Adeudo después del pago
            }
            else
            {
                if (d.CargoMantenimientoID.HasValue)
                    descripcion = $"Mantenimiento #{d.CargoMantenimientoID}";
                else if (d.CargoServicioID.HasValue)
                    descripcion = $"Servicio #{d.CargoServicioID}";
            }

            table.AddCell(CreateTableCell(concepto, fontSmall, Element.ALIGN_LEFT, false));
            table.AddCell(CreateTableCell(descripcion, fontSmall, Element.ALIGN_LEFT, false));
            table.AddCell(CreateTableCell(d.MontoAplicado.ToString("C2", CultureInfo.GetCultureInfo("es-MX")),
                fontSmall, Element.ALIGN_RIGHT, false));

            // Mostrar adeudo en rojo si es mayor a 0
            var fontAdeudo = adeudo > 0 ? fontPendiente : fontSmall;
            table.AddCell(CreateTableCell(adeudo.ToString("C2", CultureInfo.GetCultureInfo("es-MX")),
                fontAdeudo, Element.ALIGN_RIGHT, false));

            subtotal += d.MontoAplicado;
        }

        // Fila de subtotal (solo si es diferente del total)
        if (subtotal != pago.MontoTotal)
        {
            var cellSubtotalLabel = new PdfPCell(new Phrase("Subtotal de los costos", fontNormal))
            {
                Colspan = 2,
                Border = Rectangle.TOP_BORDER,
                BorderWidthTop = 1f,
                Padding = 5f,
                HorizontalAlignment = Element.ALIGN_RIGHT
            };
            table.AddCell(cellSubtotalLabel);

            var cellSubtotalValue = new PdfPCell(new Phrase(subtotal.ToString("C2", CultureInfo.GetCultureInfo("es-MX")), fontNormal))
            {
                Border = Rectangle.TOP_BORDER,
                BorderWidthTop = 1f,
                Padding = 5f,
                HorizontalAlignment = Element.ALIGN_RIGHT
            };
            table.AddCell(cellSubtotalValue);

            // Celda vacía para la columna de Adeudo
            var cellEmpty = new PdfPCell(new Phrase("", fontNormal))
            {
                Border = Rectangle.TOP_BORDER,
                BorderWidthTop = 1f,
                Padding = 5f
            };
            table.AddCell(cellEmpty);
        }

        // Fila de total
        var cellTotalLabel = new PdfPCell(new Phrase("Total de los costos", fontSectionTitle))
        {
            Colspan = 2,
            Border = subtotal == pago.MontoTotal ? Rectangle.TOP_BORDER : Rectangle.NO_BORDER,
            BorderWidthTop = subtotal == pago.MontoTotal ? 1f : 0f,
            Padding = 5f,
            HorizontalAlignment = Element.ALIGN_RIGHT
        };
        table.AddCell(cellTotalLabel);

        var cellTotalValue = new PdfPCell(new Phrase(pago.MontoTotal.ToString("C2", CultureInfo.GetCultureInfo("es-MX")), fontSectionTitle))
        {
            Border = subtotal == pago.MontoTotal ? Rectangle.TOP_BORDER : Rectangle.NO_BORDER,
            BorderWidthTop = subtotal == pago.MontoTotal ? 1f : 0f,
            Padding = 5f,
            HorizontalAlignment = Element.ALIGN_RIGHT
        };
        table.AddCell(cellTotalValue);

        // Celda vacía para la columna de Adeudo en la fila de total
        var cellTotalEmpty = new PdfPCell(new Phrase("", fontNormal))
        {
            Border = subtotal == pago.MontoTotal ? Rectangle.TOP_BORDER : Rectangle.NO_BORDER,
            BorderWidthTop = subtotal == pago.MontoTotal ? 1f : 0f,
            Padding = 5f
        };
        table.AddCell(cellTotalEmpty);

        document.Add(table);

        // RESUMEN DE PAGO
        document.Add(new Paragraph("RESUMEN DE PAGO", fontSectionTitle)
        {
            SpacingBefore = 15f,
            SpacingAfter = 8f
        });

        var resumenTable = new PdfPTable(4) { WidthPercentage = 100f };
        resumenTable.SetWidths(new float[] { 25f, 25f, 25f, 25f });
        resumenTable.SpacingAfter = 15f;

        resumenTable.AddCell(CreateTableCell("Total a pagar", fontTableHeader, Element.ALIGN_CENTER, true));
        resumenTable.AddCell(CreateTableCell("Pago realizado", fontTableHeader, Element.ALIGN_CENTER, true));
        resumenTable.AddCell(CreateTableCell("Restante", fontTableHeader, Element.ALIGN_CENTER, true));
        resumenTable.AddCell(CreateTableCell("Estado", fontTableHeader, Element.ALIGN_CENTER, true));

        // Calcular totales para ESTE comprobante específico
        decimal pagoRealizadoAhora = pago.MontoTotal;
        decimal saldoPendienteTotal = 0;
        decimal montoOriginalTotal = 0;

        foreach (var d in detalles)
        {
            if (d.CargoMantenimiento is not null)
            {
                // Para mantenimiento: saldo pendiente DESPUÉS del pago + lo que se pagó ahora = total original
                saldoPendienteTotal += d.CargoMantenimiento.SaldoPendiente;
                montoOriginalTotal += d.CargoMantenimiento.SaldoPendiente + d.MontoAplicado;
            }
            else if (d.CargoServicio is not null)
            {
                // Para servicio: saldo pendiente DESPUÉS del pago + lo que se pagó ahora = total original
                saldoPendienteTotal += d.CargoServicio.SaldoPendiente;
                montoOriginalTotal += d.CargoServicio.SaldoPendiente + d.MontoAplicado;
            }
        }

        // Si no hay saldo pendiente, significa que se pagó todo
        decimal totalAPagar = montoOriginalTotal > 0 ? montoOriginalTotal : pagoRealizadoAhora;
        decimal restante = saldoPendienteTotal;

        string estado = restante > 0 ? "PENDIENTE" : "PAGADO";
        var fontEstado = restante > 0 ? fontPendiente : new Font(baseFontBold, 9, Font.BOLD, new BaseColor(0, 128, 0));

        resumenTable.AddCell(CreateTableCell(totalAPagar.ToString("C2", CultureInfo.GetCultureInfo("es-MX")),
            fontSmall, Element.ALIGN_CENTER, false));
        resumenTable.AddCell(CreateTableCell(pagoRealizadoAhora.ToString("C2", CultureInfo.GetCultureInfo("es-MX")),
            fontSmall, Element.ALIGN_CENTER, false));
        resumenTable.AddCell(CreateTableCell(restante.ToString("C2", CultureInfo.GetCultureInfo("es-MX")),
            fontSmall, Element.ALIGN_CENTER, false));
        resumenTable.AddCell(CreateTableCell(estado, fontEstado, Element.ALIGN_CENTER, false));

        document.Add(resumenTable);
        // INFORMACIÓN ADICIONAL
        document.Add(new Paragraph("INFORMACIÓN ADICIONAL", fontSectionTitle)
        {
            SpacingAfter = 8f
        });

        document.Add(new Paragraph($"Método de pago: Transferencia bancaria", fontNormal));
        document.Add(new Paragraph($"Referencia de Pago: {pago.FolioUnico}", fontNormal));
        document.Add(new Paragraph($"Comentarios: Pago realizado. Gracias por su cooperación.", fontNormal));

        document.Close();
        writer.Close();

        return ms.ToArray();
    }

    private PdfPCell CreateTableCell(string text, Font font, int align, bool isHeader)
    {
        var cell = new PdfPCell(new Phrase(text, font))
        {
            Border = Rectangle.BOX,
            BorderWidth = 1f,
            BorderColor = BaseColor.Black,
            Padding = 5f,
            HorizontalAlignment = align,
            VerticalAlignment = Element.ALIGN_MIDDLE,
            BackgroundColor = isHeader ? new BaseColor(240, 240, 240) : BaseColor.White
        };
        return cell;
    }
}