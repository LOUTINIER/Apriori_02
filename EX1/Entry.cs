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
            int[] minSup = new int[] { 15, 600, 800, 1000 };
            int useMinSup = minSup[0];
            string inPath = filePaths[0];

            // 从文件读入数据，写到内存里
            DataHelper.SetData(inPath);

            //AP(inPath, filePaths, useMinSup);
            FP(inPath, useMinSup);

            Console.WriteLine("end");
            Console.ReadLine();
        }
        static void AP(string inPath, int useMinSup)
        {
            // 设置最小支持度
            Apriori.minSup = useMinSup;
            // 求得频繁项集
            List<Apriori> allSets = Apriori.GetAllSets();
            // 写入文件
            string aprioriOutPath = inPath.Substring(0, inPath.LastIndexOf(@".")) + "_minsup_" + useMinSup.ToString() + @".txt";
            DataHelper.WriteToFile(aprioriOutPath, allSets);
            // 展示支持度最高的频繁项集
            Apriori.ShowMaxSup(allSets);
        }
        static void FP(string inPath, int useMinSup)
        {
            FPTree a = new FPTree();
        }
    }
}
