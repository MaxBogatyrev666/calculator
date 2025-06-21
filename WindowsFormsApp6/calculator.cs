using System;
using System.Drawing;
using System.Windows.Forms;

namespace CalculatorApp
{
    public partial class Calculator : Form
    {
        private double firstNumber = 0;
        private double secondNumber = 0;
        private string operation = "";
        private bool isOperationPressed = false;
        private bool isEqualPressed = false;

        // Элементы интерфейса
        private TextBox displayTextBox;
        private Label historyLabel;

        public Calculator()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // Настройка формы
            this.Text = "Калькулятор";
            this.Size = new Size(320, 480);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.LightGray;

            // Поле для отображения результата
            displayTextBox = new TextBox();
            displayTextBox.Size = new Size(280, 30);
            displayTextBox.Location = new Point(15, 40);
            displayTextBox.Font = new Font("Arial", 14, FontStyle.Bold);
            displayTextBox.TextAlign = HorizontalAlignment.Right;
            displayTextBox.ReadOnly = true;
            displayTextBox.Text = "0";
            displayTextBox.BackColor = Color.White;
            this.Controls.Add(displayTextBox);

            // Метка для истории операций
            historyLabel = new Label();
            historyLabel.Size = new Size(280, 20);
            historyLabel.Location = new Point(15, 15);
            historyLabel.Font = new Font("Arial", 9);
            historyLabel.TextAlign = ContentAlignment.MiddleRight;
            historyLabel.Text = "";
            historyLabel.ForeColor = Color.Gray;
            this.Controls.Add(historyLabel);

            CreateButtons();
        }

        private void CreateButtons()
        {
            // Массив с текстом кнопок
            string[,] buttonTexts = {
                {"C", "CE", "⌫", "÷"},
                {"7", "8", "9", "×"},
                {"4", "5", "6", "−"},
                {"1", "2", "3", "+"},
                {"±", "0", ".", "="}
            };

            int buttonWidth = 65;
            int buttonHeight = 50;
            int startX = 15;
            int startY = 80;
            int spacing = 5;

            for (int row = 0; row < 5; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    Button btn = new Button();
                    btn.Text = buttonTexts[row, col];
                    btn.Size = new Size(buttonWidth, buttonHeight);
                    btn.Location = new Point(
                        startX + col * (buttonWidth + spacing),
                        startY + row * (buttonHeight + spacing)
                    );
                    btn.Font = new Font("Arial", 12, FontStyle.Bold);
                    btn.Click += Button_Click;

                    // Цветовая схема
                    if (IsOperatorButton(btn.Text))
                    {
                        btn.BackColor = Color.Orange;
                        btn.ForeColor = Color.White;
                    }
                    else if (btn.Text == "=" || btn.Text == "C" || btn.Text == "CE" || btn.Text == "⌫")
                    {
                        btn.BackColor = Color.DarkGray;
                        btn.ForeColor = Color.White;
                    }
                    else
                    {
                        btn.BackColor = Color.White;
                        btn.ForeColor = Color.Black;
                    }

                    this.Controls.Add(btn);
                }
            }

            // Дополнительные кнопки
            CreateAdditionalButtons(startX, startY + 5 * (buttonHeight + spacing) + 10);
        }


        private void CreateAdditionalButtons(int startX, int startY)
        {
            string[] additionalButtons = { "√", "x²", "1/x", "%" };
            int buttonWidth = 65;
            int buttonHeight = 40;
            int spacing = 5;

            for (int i = 0; i < additionalButtons.Length; i++)
            {
                Button btn = new Button();
                btn.Text = additionalButtons[i];
                btn.Size = new Size(buttonWidth, buttonHeight);
                btn.Location = new Point(startX + i * (buttonWidth + spacing), startY);
                btn.Font = new Font("Arial", 10, FontStyle.Bold);
                btn.BackColor = Color.LightBlue;
                btn.ForeColor = Color.Black;
                btn.Click += Button_Click;
                this.Controls.Add(btn);
            }
        }

        private bool IsOperatorButton(string text)
        {
            return text == "+" || text == "−" || text == "×" || text == "÷";
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            string buttonText = button.Text;

            try
            {
                switch (buttonText)
                {
                    case "C":
                        ClearAll();
                        break;
                    case "CE":
                        ClearEntry();
                        break;
                    case "⌫":
                        Backspace();
                        break;
                    case "=":
                        CalculateResult();
                        break;
                    case "+":
                    case "−":
                    case "×":
                    case "÷":
                        SetOperation(buttonText);
                        break;
                    case "√":
                        CalculateSquareRoot();
                        break;
                    case "x²":
                        CalculateSquare();
                        break;
                    case "1/x":
                        CalculateReciprocal();
                        break;
                    case "%":
                        CalculatePercent();
                        break;
                    case "±":
                        ChangeSign();
                        break;
                    case ".":
                        AddDecimalPoint();
                        break;
                    default:
                        if (char.IsDigit(buttonText[0]))
                            AddDigit(buttonText);
                        break;
                }
            }
            catch (Exception ex)
            {
                displayTextBox.Text = "Ошибка";
                historyLabel.Text = "";
            }
        }

        private void ClearAll()
        {
            displayTextBox.Text = "0";
            historyLabel.Text = "";
            firstNumber = 0;
            secondNumber = 0;
            operation = "";
            isOperationPressed = false;
            isEqualPressed = false;
        }

        private void ClearEntry()
        {
            displayTextBox.Text = "0";
        }

        private void Backspace()
        {
            if (displayTextBox.Text.Length > 1)
            {
                displayTextBox.Text = displayTextBox.Text.Substring(0, displayTextBox.Text.Length - 1);
            }
            else
            {
                displayTextBox.Text = "0";
            }
        }

        private void AddDigit(string digit)
        {
            if (displayTextBox.Text == "0" || isOperationPressed || isEqualPressed)
            {
                displayTextBox.Text = digit;
                isOperationPressed = false;
                isEqualPressed = false;
            }
            else
            {
                displayTextBox.Text += digit;
            }
        }


        private void AddDecimalPoint()
        {
            if (isOperationPressed || isEqualPressed)
            {
                displayTextBox.Text = "0.";
                isOperationPressed = false;
                isEqualPressed = false;
            }
            else if (!displayTextBox.Text.Contains("."))
            {
                displayTextBox.Text += ".";
            }
        }

        private void SetOperation(string op)
        {
            if (!isEqualPressed && operation != "" && !isOperationPressed)
            {
                CalculateResult();
            }

            firstNumber = Convert.ToDouble(displayTextBox.Text);
            operation = op;
            isOperationPressed = true;
            isEqualPressed = false;

            historyLabel.Text = $"{firstNumber} {operation}";
        }

        private void CalculateResult()
        {
            if (operation == "" || isOperationPressed) return;

            secondNumber = Convert.ToDouble(displayTextBox.Text);
            double result = 0;

            switch (operation)
            {
                case "+":
                    result = firstNumber + secondNumber;
                    break;
                case "−":
                    result = firstNumber - secondNumber;
                    break;
                case "×":
                    result = firstNumber * secondNumber;
                    break;
                case "÷":
                    if (secondNumber != 0)
                        result = firstNumber / secondNumber;
                    else
                        throw new DivideByZeroException();
                    break;
            }

            displayTextBox.Text = result.ToString();
            historyLabel.Text = $"{firstNumber} {operation} {secondNumber} =";

            firstNumber = result;
            operation = "";
            isEqualPressed = true;
        }

        private void CalculateSquareRoot()
        {
            double number = Convert.ToDouble(displayTextBox.Text);
            if (number >= 0)
            {
                double result = Math.Sqrt(number);
                displayTextBox.Text = result.ToString();
                historyLabel.Text = $"√({number}) =";
                isEqualPressed = true;
            }
            else
            {
                throw new ArgumentException("Отрицательное число под корнем");
            }
        }

        private void CalculateSquare()
        {
            double number = Convert.ToDouble(displayTextBox.Text);
            double result = number * number;
            displayTextBox.Text = result.ToString();
            historyLabel.Text = $"({number})² =";
            isEqualPressed = true;
        }

        private void CalculateReciprocal()
        {
            double number = Convert.ToDouble(displayTextBox.Text);
            if (number != 0)
            {
                double result = 1 / number;
                displayTextBox.Text = result.ToString();
                historyLabel.Text = $"1/({number}) =";
                isEqualPressed = true;
            }
            else
            {
                throw new DivideByZeroException();
            }
        }

        private void CalculatePercent()
        {
            double number = Convert.ToDouble(displayTextBox.Text);
            double result = number / 100;
            displayTextBox.Text = result.ToString();
            historyLabel.Text = $"{number}% =";
            isEqualPressed = true;
        }

        private void ChangeSign()
        {
            double number = Convert.ToDouble(displayTextBox.Text);
            number = -number;
            displayTextBox.Text = number.ToString();
        }
    }

    // Главный класс программы
    public class Program
    {
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Calculator());
        }
    }
}

