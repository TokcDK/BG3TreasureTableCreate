﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BG3TreasureTableCreate
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var items = new List<string>();
            foreach (var fileName in new[] { "Armor", "Weapon" })
            {
                var file = fileName + ".txt";
                if (!File.Exists(file)) continue;

                using (var reader = new StreamReader(file))
                {
                    string line;
                    while ((line = reader.ReadLine()) != default)
                    {
                        line = line.Trim();

                        if (!line.StartsWith("new entry \"")) continue;
                        if (!line.EndsWith("\"")) continue;

                        items.Add(line.Substring(1, line.Length - 2));
                    }
                }
            }

            if (items.Count == 0) return;

            // main table
            var treasureTable = new List<string>
            {
                "new treasuretable \"AMX_LOOT_1\"",
                "new subtable \"1,1\""
            };
            foreach (var item in items)
            {
                treasureTable.Add($"object category \"I_{item}\",1,0,0,0,0,0,0,0");
            }
            treasureTable.Add("");

            // add to loot lists
            foreach (var item in new[]
            {
                ("Gold_Meager", ";0,199"),
                ("Gold_Pocket_Poor", ";0,99"),
                ("Gold_Pocket_Modest", ";0,49"),
                ("Gold_Pocket_Normal", ";0,19"),
                ("Gold_Pocket_Opulent", ";0,1"),
                ("Gold_Combat_Leader_Weak", ";0,2"),
                ("Gold_Combat_Leader_Medium", ";0,1"),
                ("Gold_Combat_Leader_Strong", ""),
            })
            {
                treasureTable.Add($"new treasuretable \"{item.Item1}\"");
                treasureTable.Add("CanMerge 1");
                treasureTable.Add($"new subtable \"1,1{item.Item2}\"");
                treasureTable.Add("object category \"T_AMX_LOOT_1\",1,0,0,0,0,0,0,0");
                treasureTable.Add("");
            }

            File.WriteAllLines("TreasureTable.txt", treasureTable);
        }
    }
}