using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Arquebuse
{
    public partial class Form1 : Form
    {
        const int N = 4; //Total number of bullets
        const int trials = 100000;//total number of trials
        int count = 0;//total number of positivetrials
        //double res = 0;
        double[] v=new double[N+1];
        public double[,] times=new double[N+1,N+1];
        Random rnd = new Random();
        public Form1()
        {
            InitializeComponent(); //label1.Text = "work";
        }

        private void button1_Click(object sender, EventArgs e)
        {
           // label1.Text = "work";
            count = 0;
            for (int i = 0; i < trials; i++)
            {
                init();
                for (int j = 0; j < N/2+1; j++)//there are at most N/2 collisions; not optimal, I know
                {
                    check();//remove the first collision and the corresponding bullets from the problem
                }
 
                if (answer())//if there remain bullets
                {
                    count++;
                }
            }
            //res = count;
            button1.Text = count.ToString();//output total number of positive trials
        }

        public double intersect(int i, int j)//time of collision of bullets i and j
        {
            if ((i < j) && (v[i] < v[j]))
            {
                return ((j - 1) * v[j] - (i - 1) * v[i]) / (v[j] - v[i]);
            }
            else
            {
                return -1.0;
            }
        }
        private void init()
        {
            button1.Text = "Click me!";
            for (int i = 1; i < N+1; i++)
            {
                v[i] = rnd.NextDouble();//random velocities
            }
            for (int i = 1; i <N+1 ; i++)
            {   
                for (int j = 1; j < N+1; j++)
                {
                    times[i, j] = intersect(i,j);
                }

            }

        }

        private bool check()
        {
            int istar = 0;
            int jstar = 0;
            double cur = -1 ;
            bool haspos = false;//true if there is at least one collision
            for (int i = 1; i < N+1; i++)
            {
                for (int j = 1; j < N+1; j++)
                {
                    if (times[i, j] > 0)
                    {
                        haspos = true;
                        cur = times[i, j];
                    }
                }
            }
            if (haspos)
            {
                for (int i = 1; i < N + 1; i++)
                {
                    for (int j = 1; j < N + 1; j++)
                    {
                        // haspos |= (times[i, j] > 0);
                        if ((times[i, j] > 0)&&(cur>=times[i,j]))
                        {
                            cur = times[i, j];
                            istar = i;
                            jstar = j;
                        }
                    }
                }
                for (int i = 1; i < N+1; i++)
                {
                    times[i, istar] = -2.0;
                    times[istar, i] = -2.0;
                    times[jstar, i] = -2.0;
                    times[i, jstar] = -2.0;
                    //time of collision: -2 if at least bullet in this couple was removed by our algorithm
                    // -1 for i=j; if it is still there, then the bullet is still in the consideration
                    //positive time: it means this bullet might collide with at least one another bullet 

                }
            }
            return haspos;
        }
        private bool answer()
        {
            bool answ=false;
            for (int i = 1; i < N + 1; i++)
            {
                for (int j = 1; j < N + 1; j++)
                {
                    if (times[i, j] > -1.5)//meaning that the bullets i and j are not removed by the algorithm 
                    {
                        answ = true; 
                    }
                       
                }

            }
            return answ;
        }
    }
}
