using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        static float a, c, d;
        static char b;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btn1_Click(object sender, EventArgs e)
        {
            if ((txtDisplay.Text == "+") || (txtDisplay.Text == "-") || (txtDisplay.Text == "*") || (txtDisplay.Text == "/"))
            {
                txtDisplay.Text = "";
                txtDisplay.Text = txtDisplay.Text + btn1.Text;
            }
            else
                txtDisplay.Text = txtDisplay.Text + btn1.Text;
        }

        protected void btn2_Click(object sender, EventArgs e)
        {
            if ((txtDisplay.Text == "+") || (txtDisplay.Text == "-") || (txtDisplay.Text == "*") || (txtDisplay.Text == "/"))
            {
                txtDisplay.Text = "";
                txtDisplay.Text = txtDisplay.Text + btn2.Text;
            }
            else
                txtDisplay.Text = txtDisplay.Text + btn2.Text;
        }

        protected void btn3_Click(object sender, EventArgs e)
        {
            if ((txtDisplay.Text == "+") || (txtDisplay.Text == "-") || (txtDisplay.Text == "*") || (txtDisplay.Text == "/"))
            {
                txtDisplay.Text = "";
                txtDisplay.Text = txtDisplay.Text + btn3.Text;
            }
            else
                txtDisplay.Text = txtDisplay.Text + btn3.Text;
        }

        protected void btn4_Click(object sender, EventArgs e)
        {
            if ((txtDisplay.Text == "+") || (txtDisplay.Text == "-") || (txtDisplay.Text == "*") || (txtDisplay.Text == "/"))
            {
                txtDisplay.Text = "";
                txtDisplay.Text = txtDisplay.Text + btn4.Text;
            }
            else
                txtDisplay.Text = txtDisplay.Text + btn4.Text;
        }

        protected void btn5_Click(object sender, EventArgs e)
        {
            if ((txtDisplay.Text == "+") || (txtDisplay.Text == "-") || (txtDisplay.Text == "*") || (txtDisplay.Text == "/"))
            {
                txtDisplay.Text = "";
                txtDisplay.Text = txtDisplay.Text + btn5.Text;
            }
            else
                txtDisplay.Text = txtDisplay.Text + btn5.Text;
        }

        protected void btn6_Click(object sender, EventArgs e)
        {
            if ((txtDisplay.Text == "+") || (txtDisplay.Text == "-") || (txtDisplay.Text == "*") || (txtDisplay.Text == "/"))
            {
                txtDisplay.Text = "";
                txtDisplay.Text = txtDisplay.Text + btn6.Text;
            }
            else
                txtDisplay.Text = txtDisplay.Text + btn6.Text;
        }

        protected void btn7_Click(object sender, EventArgs e)
        {
            if ((txtDisplay.Text == "+") || (txtDisplay.Text == "-") || (txtDisplay.Text == "*") || (txtDisplay.Text == "/"))
            {
                txtDisplay.Text = "";
                txtDisplay.Text = txtDisplay.Text + btn7.Text;
            }
            else
                txtDisplay.Text = txtDisplay.Text + btn7.Text;
        }

        protected void btn8_Click(object sender, EventArgs e)
        {
            if ((txtDisplay.Text == "+") || (txtDisplay.Text == "-") || (txtDisplay.Text == "*") || (txtDisplay.Text == "/"))
            {
                txtDisplay.Text = "";
                txtDisplay.Text = txtDisplay.Text + btn8.Text;
            }
            else
                txtDisplay.Text = txtDisplay.Text + btn8.Text;
        }

        protected void btn9_Click(object sender, EventArgs e)
        {
            if ((txtDisplay.Text == "+") || (txtDisplay.Text == "-") || (txtDisplay.Text == "*") || (txtDisplay.Text == "/"))
            {
                txtDisplay.Text = "";
                txtDisplay.Text = txtDisplay.Text + btn9.Text;
            }
            else
                txtDisplay.Text = txtDisplay.Text + btn9.Text;
        }

        protected void btn0_Click(object sender, EventArgs e)
        {
            if ((txtDisplay.Text == "+") || (txtDisplay.Text == "-") || (txtDisplay.Text == "*") || (txtDisplay.Text == "/"))
            {
                txtDisplay.Text = "";
                txtDisplay.Text = txtDisplay.Text + btn0.Text;
            }
            else
                txtDisplay.Text = txtDisplay.Text + btn0.Text;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            a = Convert.ToInt32(txtDisplay.Text);
            txtDisplay.Text = "";
            b = '+';
            txtDisplay.Text += b;
        }

        protected void btnSub_Click(object sender, EventArgs e)
        {
            a = Convert.ToInt32(txtDisplay.Text);
            txtDisplay.Text = "";
            b = '-';
            txtDisplay.Text += b;
        }

        protected void btnMul_Click(object sender, EventArgs e)
        {
            a = Convert.ToInt32(txtDisplay.Text);
            txtDisplay.Text = "";
            b = '*';
            txtDisplay.Text += b;
        }

        protected void btnDiv_Click(object sender, EventArgs e)
        {
            a = Convert.ToInt32(txtDisplay.Text);
            txtDisplay.Text = "";
            b = '/';
            txtDisplay.Text += b;
        }

        protected void btnEq_Click(object sender, EventArgs e)
        {
            c = Convert.ToInt32(txtDisplay.Text);
            txtDisplay.Text = "";
            if (b == '/')
            {
                d = a / c;
                txtDisplay.Text += d;
                a = d;
            }
            else if (b == '+')
            {
                d = a + c;
                txtDisplay.Text += d;
                a = d;
            }
            else if (b == '-')
            {
                d = a - c;
                txtDisplay.Text += d;
                a = d;
            }
            else
            {
                d = a * c;
                txtDisplay.Text += d;
                a = d;
            }
        }

        protected void btnClr_Click(object sender, EventArgs e)
        {
            txtDisplay.Text = "";
        }
    }
}