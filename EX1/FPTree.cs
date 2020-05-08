using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EX1
{ 
    class Freq1Set:IComparable<Freq1Set>
    {
        public int supCount;
        public int freq1;
        public Freq1Set(int f,int s) { freq1 = f;supCount = s; }
        public int CompareTo(Freq1Set o)
        {
            if (supCount != o.supCount) return supCount.CompareTo(o.supCount);
            else return 0;
        }
    }
    class FPTree
    {
        static public int minSup;
        List<Freq1Set> freq1Set = new List<Freq1Set>();
        public FPTree()
        {
            InitialSet();
        }

        public void WriteToFile(string path)
        {
            
        }

        // 获取频繁1项集并且按支持度排序
        private void InitialSet()
        {
            for (int i = 0; i < DataHelper.inDataSet.uid.Count; ++i)
                foreach (var j in DataHelper.inDataSet.productIds[i])
                {
                    if (freq1Set.Count != 0)
                        for (int k = 0; k < freq1Set.Count; ++k)
                        {
                            if (freq1Set[k].freq1 == j) { freq1Set[k].supCount += 1; break; }
                            freq1Set.Add(new Freq1Set(j, 1));// ?
                        }
                    else freq1Set.Add(new Freq1Set(j, 1));
                }
            // 排序
            freq1Set.Sort();
            foreach (var i in freq1Set) Console.WriteLine(i.freq1.ToString() + i.supCount.ToString());
        }
    }
}
