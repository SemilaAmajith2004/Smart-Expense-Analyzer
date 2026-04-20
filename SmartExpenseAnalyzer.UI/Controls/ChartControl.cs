using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using SmartExpenseAnalyzer.Core.Models;

namespace SmartExpenseAnalyzer.UI.Controls;

public class ChartControl : Control {
    private Dictionary<string, decimal> _data = new();
    private readonly Color[] _colors = {
        Color.FromArgb(0, 150, 255), // Primary blue
        Color.SpringGreen, 
        Color.Crimson, 
        Color.Gold, 
        Color.MediumOrchid, 
        Color.DarkOrange, 
        Color.Turquoise
    };

    public ChartControl() {
        DoubleBuffered = true;
        ResizeRedraw = true;
    }

    public void UpdateData(IEnumerable<Expense> expenses) {
        // Group by category, ignore blanks
        _data = expenses.GroupBy(e => string.IsNullOrWhiteSpace(e.Category) ? "Uncategorized" : e.Category)
                        .ToDictionary(g => g.Key, g => g.Sum(e => e.Amount));
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e) {
        base.OnPaint(e);
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

        if (_data.Count == 0) {
            TextRenderer.DrawText(e.Graphics, "No Expenses Yet", Font, ClientRectangle, ForeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            return;
        }

        decimal total = _data.Values.Sum();
        if (total == 0) return;

        float startAngle = -90f; // Start from top 12 o'clock
        int colorIndex = 0;
        
        int padding = 20;
        int legendHeight = _data.Count * 25 + 10;
        int maxPieSize = Math.Min(Width - (padding * 2), Height - legendHeight - padding * 2); 
        if (maxPieSize <= 0) return;

        var rect = new Rectangle((Width - maxPieSize) / 2, padding, maxPieSize, maxPieSize);

        // Draw Pie Segments
        foreach (var kvp in _data) {
            float sweepAngle = (float)(kvp.Value / total) * 360f;
            // Prevent GDI+ exceptions on tiny fractional angles
            if (sweepAngle < 0.1f) sweepAngle = 0.1f; 
            
            using (var brush = new SolidBrush(_colors[colorIndex % _colors.Length])) {
                e.Graphics.FillPie(brush, rect, startAngle, sweepAngle);
            }
            startAngle += sweepAngle;
            colorIndex++;
        }

        // Draw inner circle for "Donut" effect (matches UI bgColor)
        int holeSize = (int)(maxPieSize * 0.55f);
        var holeRect = new Rectangle(rect.X + (maxPieSize - holeSize)/2, rect.Y + (maxPieSize - holeSize)/2, holeSize, holeSize);
        // Form's background is Color.FromArgb(30, 30, 36) defined as parent's BackColor passed down or hardcoded here:
        using (var bgBrush = new SolidBrush(Color.FromArgb(30, 30, 36))) {
            e.Graphics.FillEllipse(bgBrush, holeRect);
        }
        string totalTxt = $"Total\n${total:0.00}";
        using (var font = new Font("Segoe UI", 12F, FontStyle.Bold)) {
            TextRenderer.DrawText(e.Graphics, totalTxt, font, holeRect, ForeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        // Draw Legend vertically below the chart
        int legendX = padding;
        int legendY = rect.Bottom + 20;
        colorIndex = 0;
        
        foreach (var kvp in _data) {
            using (var brush = new SolidBrush(_colors[colorIndex % _colors.Length])) {
                e.Graphics.FillRectangle(brush, legendX, legendY, 15, 15);
            }
            float percent = (float)(kvp.Value / total) * 100f;
            string text = $"{kvp.Key} - ${kvp.Value:0.00} ({percent:0.0}%)";
            TextRenderer.DrawText(e.Graphics, text, Font, new Point(legendX + 25, legendY), ForeColor);
            
            legendY += 25;
            colorIndex++;
        }
    }
}
