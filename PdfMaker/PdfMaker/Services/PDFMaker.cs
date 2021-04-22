using PdfMaker.Models;
using PdfSharpCore;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using PdfSharpCore.Utils;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PdfMaker.Services
{
    public class PDFMaker : ImageSharpImageSource<Rgba32>
    {
        public async Task<PdfDocument> CreatePdf(IEnumerable<FilesResult> files, CancellationToken cancellationToken)
        {
            if (files != null && files.Any())
            {
                return await Task.Run(async () =>
                {
                    var orderedFiles = files.OrderBy(s => s.FileName);
                    var document = new PdfDocument();
                    cancellationToken.ThrowIfCancellationRequested();
                    lock (orderedFiles)
                    {
                        foreach (var item in orderedFiles)
                        {
                            lock (item)
                            {
                                Parallel.Invoke(() =>
                                {
                                    cancellationToken.ThrowIfCancellationRequested();
                                    PdfPage page = document.AddPage();
                                    lock (page)
                                    {
                                        using XImage image = XImage.FromFile(item.FullPath);
                                        page.Orientation = PageOrientation.Portrait;

                                        Enum.TryParse(Settings.PdfPageSize, out PageSize pageSize);
                                        page.Size = pageSize;
                                        cancellationToken.ThrowIfCancellationRequested();
                                        XGraphics graphics = XGraphics.FromPdfPage(page, XGraphicsUnit.Presentation);
                                        graphics.DrawImage(image, new XPoint(0, 0));
                                    }
                                });
                            }
                        }
                    }
                    cancellationToken.ThrowIfCancellationRequested();
                    return await Task.FromResult(document);
                });
            }
            return await Task.FromResult<PdfDocument>(default);
        }

    }
}
