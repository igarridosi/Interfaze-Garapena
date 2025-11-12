using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using QuestPDF.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPV_Sistema.Models;

namespace TPV_Sistema.Services
{
    public class TicketDocument : IDocument
    {
        private readonly Eskaera _eskaera;
        private readonly double _subtotala;
        private readonly double _bezZenbatekoa;

        public TicketDocument(Eskaera eskaera, double subtotala, double bezZenbatekoa)
        {
            _eskaera = eskaera;
            _subtotala = subtotala;
            _bezZenbatekoa = bezZenbatekoa;
        }

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                // Orriaren marjinak eta tamaina (ticket baten antzera)
                page.ContinuousSize(80, Unit.Millimetre);
                page.Margin(2, Unit.Millimetre);
                page.DefaultTextStyle(style => style.FontSize(8));

                // Goiburua
                page.Header().Element(ComposeHeader);

                // Edukia (produktuen taula)
                page.Content().Element(ComposeContent);

                // Oina (totalak)
                page.Footer().Element(ComposeFooter);
            });
        }

        void ComposeHeader(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text("Oiartzungo Elkartea").Bold().FontSize(14);
                    column.Item().Text(_eskaera.Data.ToString("yyyy-MM-dd HH:mm:ss"));
                });
            });
        }

        void ComposeContent(IContainer container)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(3); // Produktua
                    columns.ConstantColumn(30);  // Kant.
                    columns.RelativeColumn();  // Prezioa
                    columns.RelativeColumn();  // Totala
                });

                // Taularen goiburua
                table.Header(header =>
                {
                    header.Cell().Text("Produktua").Bold();
                    header.Cell().AlignCenter().Text("Kant.").Bold();
                    header.Cell().AlignRight().Text("Prezioa").Bold();
                    header.Cell().AlignRight().Text("Totala").Bold();
                });

                // Lerro bakoitza
                foreach (var lerroa in _eskaera.Lerroak)
                {
                    table.Cell().Text(lerroa.Produktua.Izena);
                    table.Cell().AlignCenter().Text(lerroa.Kantitatea.ToString());
                    table.Cell().AlignRight().Text($"{lerroa.PrezioaUnitateko:C}");
                    table.Cell().AlignRight().Text($"{lerroa.LerroarenTotala:C}");
                }
            });
        }

        void ComposeFooter(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
                column.Spacing(5);

                // Oinarria (Subtotala)
                column.Item().AlignRight().Text($"Oinarria: {_subtotala:C}");

                // BEZ-a
                column.Item().AlignRight().Text($"BEZ-a (%21): {_bezZenbatekoa:C}");

                // TOTALA
                column.Item().AlignRight().Text($"TOTALA: {_eskaera.Guztira:C}").Bold().FontSize(14);

                column.Item();
                column.Spacing(10);
                column.Item().AlignCenter().Text("Eskerrik asko!").Italic();
            });
        }
    }
}
