using PROJECT_SPROS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spros_SE
{
    public partial class Form1 : Form
    {

        public Form1() //основная форма
        {
            InitializeComponent();
            TitleScreen TS = new TitleScreen(); //инициализация титульного экрана
            TS.ShowDialog();
        }
        public void StartForm()
        {
            Application.Run(new PROJECT_SPROS.TitleScreen()); //вывод титульного экрана перед запуском программы
        }
        private void помощьToolStripMenuItem_Click(object sender, EventArgs e) //ToolStripMenu - верхняя панель. 
        {
            HELP help = new HELP(); //вывод окна «Помощь»
            help.ShowDialog();
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About about = new About(); //вывод окна «О программе»: сведения об авторе программы и заказчике (руководителе)
            about.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear(); //предварительная очистка listBox1 - окна с результатами вычислений
            if (String.IsNullOrEmpty(textBox1.Text)) //если поле с количеством лет пустое...
            {
                MessageBox.Show("Не введено количество лет. Попробуйте ещё раз!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); //вывод сообщения об ошибке
            }
            if (String.IsNullOrEmpty(textBox2.Text)) //если поле со значениями показателей пустое...
            {
                MessageBox.Show("Не введены показатели. Попробуйте ещё раз!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); //вывод сообщения об ошибке
            }
            try //конструкция try-catch «ловит» программные ошибки и делает их некритичными для работы программы
            {
                int n = int.Parse(textBox1.Text); // ввод периода времени, по которому делается прогноз
                string[] sT = textBox2.Text.Split(';'); //ввод данных через точку с запятой
                if (n > sT.Length) //в случае, если введено показателей меньше, чем нужно
                {
                    MessageBox.Show("Введено меньше показателей, чем требуется. Попробуйте еще раз!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); //выводится окно с ошибкой
                    return; //возврат в программу
                }
                if (n < sT.Length) //в случае, если введено показателей больше, чем нужно
                {
                    MessageBox.Show("Введено больше показателей, чем требуется. Попробуйте еще раз!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); //выводится окно с ошибкой
                    return; //возврат в программу
                }
                double[] T = new double[n]; //массив показателей

                for (int i = 0; i < n; i++)
                    T[i] = Convert.ToDouble(sT[i]); //в него вносятся числовые значения (уже без точек с запятой)
                MessageBox.Show("Показатели спроса вычислены успешно.", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information); //если все данные введены корректно, выводится следующее сообщение

                double s1, s2, s3, s4, s5; //переменные для вычисления сумм

                s1 = 0;
                s2 = 0;
                s3 = 0;
                s4 = 0;
                s5 = 0;

                for (int i = 1; i <= n; i++) //ищутся эти самые суммы
                {
                    s1 = s1 + Math.Pow(i, 2);
                }

                for (int i = 0; i < n; i++)
                {
                    s2 = s2 + T[i];
                }

                for (int i = 1; i <= n; i++)
                {
                    s3 = s3 + (i * T[i - 1]);
                }

                for (int i = 1; i <= n; i++)
                {
                    s4 = s4 + Math.Pow(i, 4);
                }

                for (int i = 1; i <= n; i++)
                {
                    s5 = s5 + (Math.Pow(i, 2) * T[i - 1]);
                }

                double[,] MatrixCoef = new double[3, 3] //система уравнений
                {
                        {n, 0, s1},
                        
                        {0, s1, 0},

                        {s1, 0, s4}
                };
                double[] FreeCoef = new double[3] //свободные члены
                {
                        s2,
                        
                        s3,
                        
                        s5
                };
                
                Gauss.Solve(n, MatrixCoef, FreeCoef); //метод класса, вычисляющий коэффициенты a0,a1 и a2 (в скобках указаны принимаемые параметры)

                double a0, a1, a2; //коэффициенты

                a0 = Math.Round(Gauss.Result[0], 2); //метод класса возвращает вычисленные показатели, а тут они округляются до сотых. (для удобства)
                a1 = Math.Round(Gauss.Result[1], 2); //и тут
                a2 = Math.Round(Gauss.Result[2], 2); //и тут

                int m; //объем массива с будущими показателями спроса на ТНП
                double[] Y = new double[n + 16]; //сам массив будущих показателей спроса на ТНП

                for (m = (n + 1); m < (n + 16); m++) 
                {
                    Y[m] = a0 + (a1 * m) + (a2 * Math.Pow(m, 2)); //прогнозируется спрос...
                    listBox1.Items.Add($"год {m}: " + Math.Round(Y[m], 2).ToString()); //сюда вносятся полученные результаты

                    /* Всё! */
                }
            }
            catch (FormatException) //продолжение конструкции try-catch (случай, если данные введены в неверном формате)
            {
                MessageBox.Show("Данные введены неверно. Попробуйте еще раз!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); //выводится сообщение об ошибке
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        /* СОЗДАННЫЙ МНОЙ КЛАСС! */
        /* Gauss - класс, который вычисляет коэффициенты a0, a1 и a2, необходимые для дальнейших вычислений (на математическом языке - корни СЛАУ, которые ищутся по м.Гаусса) */

        public static class Gauss 
        {
            public static double[] Result { get; set; } //массив с корнями СЛАУ

            /*  МЕТОД КЛАССА, КОТОРЫЙ ПРОИЗВОДИТ ВЫЧИСЛЕНИЯ A0, A1 И A2 — Gauss.Solve()  */

            public static double[] Solve(int n, double[,] MatrixCoef, double[] FreeCoef)
                {
                    double Multi1, Multi2;
                    Result = new double[3];

                    int MatrixRow = MatrixCoef.GetLength(0);
                    int MatrixCol = MatrixCoef.GetLength(1);

                    for (int k = 0; k < 3; k++) //прямой ход МГ
                    {
                        for (int l = k + 1; l < 3; l++)
                        {
                            Multi1 = MatrixCoef[l, k] / MatrixCoef[k, k];
                            for (int i = k; i < 3; i++)
                            {
                                MatrixCoef[l, i] = MatrixCoef[l, i] - Multi1 * MatrixCoef[k, i];
                            }
                            FreeCoef[l] = FreeCoef[l] - Multi1 * FreeCoef[k];
                        }
                    }

                    for (int k = 3 - 1; k >= 0; k--) //обратный ход МГ
                    {
                        Multi1 = 0;
                        for (int l = k; l < 3; l++)
                        {
                            Multi2 = MatrixCoef[k, l] * Result[l];
                            Multi1 += Multi2;
                        }
                        Result[k] = (FreeCoef[k] - Multi1) / MatrixCoef[k, k];
                    }

                return Result; //возврат массива с корнями СЛАУ                
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}