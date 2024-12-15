using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    
    internal class Vectors
    {
        private int x;
        private int y;

        public Vectors(int x,int y)
        {
            this.x = x;
            this.y = y;
        }
        public bool compareTo(Vectors vector)
        {
            if (x == vector.getX() && y == vector.getY())
            {
                return true;
            }
            else return false;
        }
        public Vectors move(int x, int y)
        {
           Vectors temp =  new Vectors(this.x + x, this.y + y);
            if (temp.getX() >= 0 && temp.getX() <= 7 && temp.getY() >= 0 && temp.getX() <= 7)
            {
                return temp;
            }
            else
            {
                return null;
            }
        }

        public int getX()
        {
            return x;
        }
        public int getY()
        {
            return y;
        }
        public bool rangeCheck()
        {

            return x >= 0 && x <= 7 && y >= 0 && y <= 7;

        }
        public String toString()
        {
            return x + "," + y;
        }
    }
}
