using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EX1
{
    class Entry
    {
        static void Main()
        {
            // interface
            string[] filePaths = new string[]{
                @"C:\Users\13614\OneDrive\4\数据挖掘\实验\EX1\T1014D1K.dat",
                @"C:\Users\13614\OneDrive\4\数据挖掘\实验\EX1\T1014D10K.dat",
                @"C:\Users\13614\OneDrive\4\数据挖掘\实验\EX1\T1014D50K.dat",
                @"C:\Users\13614\OneDrive\4\数据挖掘\实验\EX1\T1014D100K.dat",
            };
            int useK;
            int[][] minSup = new int[][] {
                new int[]{6,8,10 } ,
                new int[]{60,80,100},
                new int[]{300,400,500},
                new int[]{600,800,1000},
            };
            useK = 0;
            int useMinSup = minSup[useK][1];
            string inPath = filePaths[useK];

            // 从文件读入数据，写到内存里
            DataHelper.SetData(inPath);

            AP(inPath, useMinSup);

            Console.WriteLine("end");
            Console.ReadLine();
        }
        static void AP(string inPath, int useMinSup)
        {
            // 设置最小支持度
            Apriori.minSup = useMinSup;
            // 求得频繁项集
            DateTime time1 = DateTime.Now;
            List<Apriori> allSets = Apriori.GetAllSets();
            DateTime time2 = DateTime.Now;
            string a = "花费时间：" + Environment.NewLine +
                "分钟:" + (time2 - time1).TotalMinutes.ToString() + Environment.NewLine +
                "秒：" + (time2 - time1).TotalSeconds.ToString() + Environment.NewLine;
            // 写入文件
            string aprioriOutPath = inPath.Substring(0, inPath.LastIndexOf(@".")) + "_minsup_" + useMinSup.ToString() + @".txt";
            DataHelper.WriteToFile(aprioriOutPath, allSets);
            // 展示支持度最高的频繁项集
            Apriori.ShowMaxSup(allSets);
            Console.Write(a);
        }
    }
}
