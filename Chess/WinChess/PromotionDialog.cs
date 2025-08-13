// Chess/WinChess/PromotionDialog.cs
using System.Windows.Forms;

namespace WinChess
{
    public enum PromotionChoice { Queen, Rook, Bishop, Knight }

    public class PromotionDialog : Form
    {
        public PromotionChoice Choice { get; private set; } = PromotionChoice.Queen;

        public PromotionDialog(bool isWhite)
        {
            Text = isWhite ? "Promote White Pawn" : "Promote Black Pawn";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MinimizeBox = MaximizeBox = false;
            ClientSize = new System.Drawing.Size(210, 95);

            var btnQ = new Button { Text = "Queen", Left = 10, Top = 10, Width = 90 };
            var btnR = new Button { Text = "Rook", Left = 110, Top = 10, Width = 90 };
            var btnB = new Button { Text = "Bishop", Left = 10, Top = 50, Width = 90 };
            var btnN = new Button { Text = "Knight", Left = 110, Top = 50, Width = 90 };

            btnQ.Click += (_, __) => { Choice = PromotionChoice.Queen; DialogResult = DialogResult.OK; };
            btnR.Click += (_, __) => { Choice = PromotionChoice.Rook; DialogResult = DialogResult.OK; };
            btnB.Click += (_, __) => { Choice = PromotionChoice.Bishop; DialogResult = DialogResult.OK; };
            btnN.Click += (_, __) => { Choice = PromotionChoice.Knight; DialogResult = DialogResult.OK; };

            Controls.AddRange(new Control[] { btnQ, btnR, btnB, btnN });
            AcceptButton = btnQ;
        }
    }
}
