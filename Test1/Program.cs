using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using xxlib;

public class Program
{
    static void Main(string[] args)
    {
        xxlib.Random rnd = new xxlib.Random(0);
        for (int i = 0; i < 1000000; i++)
        {
            rnd.Next();
        }

        for (int i = 0; i < 10; i++)
        {
            Console.WriteLine(rnd.Next() + "");
        }
    }
}
