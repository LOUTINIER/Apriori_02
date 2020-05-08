using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EX1
{
    struct FreqSet
    {
        public List<int> supCount;
        public List<List<int>> freqSet;
    }
    class Apriori
    {
        private int K;
        private FreqSet freqSet = new FreqSet();
        public static int minSup;
        private Apriori(int k)
        {
            K = k;
            freqSet.freqSet = new List<List<int>>();
            freqSet.supCount = new List<int>();
        }

        public void WriteToFile(string path)
        {
            StringBuilder a = new StringBuilder();
            a.Append(K + "项集：(" + freqSet.supCount.Count + "项)" + Environment.NewLine);
            int i = 0;
            foreach (List<int> items in freqSet.freqSet)
            {
                a.Append("{");
                bool isFirst = true;
                foreach (int item in items)
                {
                    if (isFirst) isFirst = false;
                    else a.Append(",");
                    a.Append(item);
                }
                a.Append("}" + "   sup_Count=" + freqSet.supCount[i] + Environment.NewLine);
                i += 1;
            }
            File.AppendAllText(path, a.ToString());
        }
        public void PrintSet()
        {
            StringBuilder a = new StringBuilder();
            a.Append(K + "项集：" + Environment.NewLine);
            foreach(List<int> items in freqSet.freqSet)
            { 
                a.Append("{");
                bool isFirst = true;
                foreach(int item in items)
                {
                    if (isFirst) isFirst = false;
                    else a.Append(",");
                    a.Append(item);
                }
                a.Append("}" + Environment.NewLine);
            }
            Console.Write(a);
        }
        static public List<Apriori> GetAllSets()
        {
            List<Apriori> allSets = new List<Apriori>();
            Apriori set = new Apriori(1);
            set.GetFirstItemset();
            while (set.freqSet.freqSet.Count > 0)
            {
                Console.WriteLine("频繁" + set.K + "项集个数：" + set.freqSet.freqSet.Count);
                allSets.Add(set);
                set = set.GetNextItemset();
            }
            return allSets;
        }

        // 展示支持度最大的项集
        static public void ShowMaxSup(List<Apriori> allSets)
        {
            foreach (Apriori unit in allSets)
            {
                int maxSup = 0;
                List<int> maxSets = new List<int>();
                for (int i = 0; i < unit.freqSet.supCount.Count; ++i)
                    if (unit.freqSet.supCount[i] > maxSup)
                        maxSup = unit.freqSet.supCount[i];
                for (int i = 0; i < unit.freqSet.supCount.Count; ++i)
                    if (unit.freqSet.supCount[i] == maxSup)
                        maxSets.Add(i);

                StringBuilder a = new StringBuilder();
                a.Append("频繁" + unit.K + "项集最大支持度为：" + maxSup + "，对应的频繁项集为：" + Environment.NewLine);
                foreach (int i in maxSets)
                {
                    a.Append("{");
                    bool isFirst = true;
                    foreach (int item in unit.freqSet.freqSet[i])
                    {
                        if (isFirst) isFirst = false;
                        else a.Append(",");
                        a.Append(item);
                    }
                    a.Append("}" + Environment.NewLine);
                }
                Console.Write(a);
            }
        }



        // 私有方法

        private void GetFirstItemset()
        {
            freqSet.freqSet.Clear();
            freqSet.supCount.Clear();
            List<int> appearedValue = new List<int>();
            K = 1;
            // Apriori retObj = new Apriori(1);
            // 对每一个出现的项计数得到频繁1项集
            for (int k = 0; k < DataHelper.inDataSet.uid.Count; ++k)
            {
                for (int l = 0; l < DataHelper.inDataSet.productIds[k].Count; ++l)
                {
                    if (!appearedValue.Contains(DataHelper.inDataSet.productIds[k][l]))
                    {
                        if (appearedValue.Count != 0)
                        {
                            int left = 0, right = appearedValue.Count - 1;
                            while (left <= right)
                            {
                                int middle = (left + right) / 2;
                                if (DataHelper.inDataSet.productIds[k][l] < appearedValue[middle]) right = middle - 1;
                                else left = middle + 1;
                            }
                            appearedValue.Insert(left, DataHelper.inDataSet.productIds[k][l]);
                            freqSet.supCount.Insert(left, 1);
                        }
                        else
                        {
                            appearedValue.Add(DataHelper.inDataSet.productIds[k][l]);
                            freqSet.supCount.Add(1);
                        }
                    }
                    else
                    {
                        freqSet.supCount[appearedValue.IndexOf(DataHelper.inDataSet.productIds[k][l])] += 1;
                    }
                }
            }
            // 剪掉非频繁项
            for (int i = 0; i < appearedValue.Count;)
            {
                if (freqSet.supCount[i] >= minSup)
                {
                    freqSet.freqSet.Add(new List<int> { appearedValue[i] });
                    i += 1;
                }
                else
                {
                    freqSet.supCount.RemoveAt(i);
                    appearedValue.RemoveAt(i);
                }
            }
        }

        private Apriori GetNextItemset()
        {
            Apriori retObj = new Apriori(K + 1);
            retObj.GetCandidateSet(this); // this扩展到retObj的候选项集
            retObj.CalSupCount();
            return retObj;
        }

        // 计算支持度并依据支持度剪枝
        private void CalSupCount()
        {
            // 计算支持度
            // 遍历每个原始数据
            for (int i = 0; i < freqSet.freqSet.Count; ++i)
                for (int j = 0; j < DataHelper.inDataSet.productIds.Count; ++j)
                    if (ContainSet(DataHelper.inDataSet.productIds[j], freqSet.freqSet[i])) freqSet.supCount[i] += 1;

            // 减枝
            for (int i = 0; i < freqSet.freqSet.Count;)
            {
                if (freqSet.supCount[i] < minSup)
                {
                    freqSet.supCount.RemoveAt(i);
                    freqSet.freqSet.RemoveAt(i);
                }
                else i += 1;
            }
        }

        private void GetCandidateSet(Apriori SrcSet)
        {
            // 组合得到K+1集
            // 组合方法是两个项0~k-2的元素相等的话，那么就能产生新项{0~k-2，第一项k-1，第二项k-1}
            // 找出频繁项集内的两个项尝试组合
            for (int i = 0; i < SrcSet.freqSet.freqSet.Count; ++i)
                for (int j = i + 1; j < SrcSet.freqSet.freqSet.Count; ++j)
                    // 对于每个频繁项集内的元素
                    for (int k = 0; k < SrcSet.K; ++k)
                        // 如果两个项第k个元素相等
                        if (SrcSet.freqSet.freqSet[i][k] != SrcSet.freqSet.freqSet[j][k])
                            // 如果k不是最后一个，直接跳过
                            if (k != SrcSet.K - 1) break;
                            else
                            {
                                // 组合的结果unit
                                List<int> unit = new List<int>();
                                // 组合生成候选项集的项
                                for (int g = 0; g < SrcSet.K + 1; ++g)
                                {
                                    if (g != SrcSet.K)
                                        unit.Add(SrcSet.freqSet.freqSet[i][g]);
                                    else
                                        unit.Add(SrcSet.freqSet.freqSet[j][g - 1]);
                                }
                                if (!SrcSet.IsInFreq(unit))
                                {
                                    freqSet.freqSet.Add(unit);
                                    freqSet.supCount.Add(0);
                                }
                            }
        }

        // 判断C的子项集里面是否含有this没有的项
        private bool IsInFreq(List<int> C)
        {
            for (int i = 0; i < C.Count; ++i)
            {
                // 求出C的子集
                List<int> sub = new List<int>();
                for (int j = 0; j < C.Count; ++j)
                    if (j != C.Count - i - 1) sub.Add(C[j]);
                // 如果有一个子集没有在this的频繁项集中出现，那C是不频繁的
                if (!HaveSet(freqSet.freqSet, sub)) { return true; }
            }
            return false;
        }

        // 判断a里面有没有b
        private bool HaveSet(List<List<int>> a, List<int> b)
        {
            // 对于a的每一项
            for (int i = 0; i < a.Count; ++i)
            {
                int cnt = 0;
                // 判断b集里的每一个元素是否在目标项内出现过
                for (int j = 0; j < b.Count; ++j)
                    if (a[i].Contains(b[j])) cnt += 1;
                // 如果有一个a的项内，b的元素全都出现过，那确实a的项有b
                if (cnt == b.Count) return true;
            }
            return false;
        }

        // 判断a项有没有b的所有元素
        private bool ContainSet(List<int> a, List<int> b)
        {
            int cnt = 0;
            for (int j = 0; j < b.Count; ++j)
                if (a.Contains(b[j])) cnt += 1;
            if (cnt == b.Count) return true;
            else return false;
        }
    }
}
