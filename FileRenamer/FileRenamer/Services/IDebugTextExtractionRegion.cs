using iText.Kernel.Geom;

namespace FileRenamer.Services
{
    public interface IDebugTextExtractionRegion
    {
        void DrawRectangleArea(Rectangle rectangle, string inputFilePath);
    }
}
