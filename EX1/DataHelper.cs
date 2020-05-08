using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EX1
{
    public struct InDataSet
    {
        public List<int> uid;
        public List<List<int>> productIds;
    }
    class DataHelper
    {
        public static InDataSet inDataSet = new InDataSet();

        // 从文件读入数据
        static public void SetData(string path)
        {
            inDataSet.productIds = new List<List<int>>();
            inDataSet.uid = new List<int>();
            string[] data = File.ReadAllLines(path, Encoding.Default);
            int count = 0;
            foreach (string unitItem in data)
            {
                string[] unit = unitItem.Split(' ');
                inDataSet.uid.Add(count);
                count += 1;
                List<int> tmp = new List<int>();
                foreach (string item in unit.Take(unit.Length - 1))
                    tmp.Add(int.Parse(item));
                inDataSet.productIds.Add(tmp);
            }
        }

        static public void WriteToFile(string path,List<Apriori> aprioriSets)
        {
            if (File.Exists(path)) File.Delete(path);
            if (aprioriSets.Count != 0)
                foreach (var item in aprioriSets)
                    item.WriteToFile(path);
            else File.WriteAllText(path, "None Result.");
        }

        static public void WriteToFile(string path, List<FPTree> fpTreeSets)
        {
            if (File.Exists(path)) File.Delete(path);
            if (fpTreeSets.Count != 0)
                foreach (var item in fpTreeSets)
                    item.WriteToFile(path);
            else File.WriteAllText(path, "None Result.");
        }
    }
}
