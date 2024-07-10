using minesweeper;
using Newtonsoft.Json;
using System;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Windows.Input;


namespace ConsoleApp1
{

    public static class Global_vars
    {
        public static List<int> boardsize = new() { 8, 8 }; // y, x
        public static int fieldsclear = 0;
        public static List<List<int>> fields_opened = new();
        public static List<List<int>> flaglist = new();
        public static List<List<string>> nums = new();
        public static List<ConsoleColor> colors = new()
        {
            ConsoleColor.Gray,
            ConsoleColor.Blue,
            ConsoleColor.Cyan,
            ConsoleColor.Green,
            ConsoleColor.Yellow,
            ConsoleColor.Magenta,
            ConsoleColor.DarkMagenta,
            ConsoleColor.DarkRed,
        };
        public static Dictionary<string, dynamic> jsontransfer = new();
        public static int gameState = new();
    }
    internal class Program
    {

        static int Accseslist(List<List<int>> list2check, int x, int y)
        {
            int counter = 0;
            try
            {
                if (list2check[y][x] == 1)
                {
                    counter = 1;
                }

            }
            catch
            {

            }
            return counter;
        }
        static string Checkpos(List<List<int>> list2check, int x, int y)
        {
            int counter = 0;

            if (list2check[y][x] == 1)
            {
                return "M";
            }

            counter += Accseslist(list2check, x - 1, y + 1);
            counter += Accseslist(list2check, x, y + 1);
            counter += Accseslist(list2check, x + 1, y + 1);
            counter += Accseslist(list2check, x - 1, y);
            counter += Accseslist(list2check, x, y);
            counter += Accseslist(list2check, x + 1, y);
            counter += Accseslist(list2check, x - 1, y - 1);
            counter += Accseslist(list2check, x, y - 1);
            counter += Accseslist(list2check, x + 1, y - 1);

            string count = counter.ToString();
            return count;
        }

        static List<List<int>> MoveAndPrint(List<List<int>> coordinatelist, int x, int y)
        {
            List<List<int>> ints = new List<List<int>>();
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    try
                    {
                        string tempstorage = Checkpos(coordinatelist, x - j, y - i);
                        if (Int32.Parse(tempstorage) == 0)
                        {
                            List<int> templist = new List<int>();
                            templist.Add(x - j);
                            templist.Add(y - i);
                            ints.Add(new List<int>(templist));
                        }
                        Global_vars.nums[y - i][x - j] = tempstorage;
                        Global_vars.fields_opened[y - i][x - j] = 1;
                        if (CheckEnd())
                        {
                            Global_vars.gameState = 1;
                        }
                    }
                    catch
                    {

                    }

                }
            }
            return ints;
        }

        static bool Open0(List<List<int>> coordinatelist, int x, int y)
        {
            List<List<int>> checkedlist = new();
            int o_x = x; 
            int o_y = y;
            List<List<int>> list2check0 = new();
            if (coordinatelist[y][x] == 0)
            {
                list2check0 = MoveAndPrint(coordinatelist, x, y);
                List<int> templist = new() { o_x, o_y };
                checkedlist.Add(new List<int>(templist));

            }
        check40:
            List<List<int>> list2check0temp = new();

            if (list2check0.Count != 0)
            {
                foreach (List<int> item in list2check0)
                {
                    List<int> templist = new();
                    int found = 0;

                    foreach (List<int> checkitem in checkedlist)
                    {
                        if (checkitem[0] == item[0] && checkitem[1] == item[1])
                        {
                            found = 1;
                            goto skip_foreach;
                        }

                    }
                    if (found == 0)
                    {
                        checkedlist.Add(new List<int> { item[0], item[1] });
                    }

                    List<List<int>> list2check0temp2 = MoveAndPrint(coordinatelist, item[0], item[1]);
                    foreach (List<int> ints in list2check0temp2)
                    {
                        list2check0temp.Add(ints);
                    }
                skip_foreach:
                    int skip = 0;
                }
            }

            if (list2check0temp.Count != 0)
            {
                list2check0 = list2check0temp;
                list2check0temp.Clear();
                goto check40;
            }
            return true;
        }
        static void Checklist(List<List<int>> mainlist)
        {
            for (int y1 = 0; y1 < mainlist.Count(); y1++)
            {
                for (int x1 = 0; x1 < mainlist[y1].Count(); x1++)
                {
                    int count = 0;
                    if (mainlist[y1][x1] == 1)
                    {

                        Console.Write("M");
                        Console.Write(" ");
                    }
                    else
                    {
                        count += Accseslist(mainlist, x1 - 1, y1 + 1);
                        count += Accseslist(mainlist, x1, y1 + 1);
                        count += Accseslist(mainlist, x1 + 1, y1 + 1);
                        count += Accseslist(mainlist, x1 - 1, y1);
                        count += Accseslist(mainlist, x1, y1);
                        count += Accseslist(mainlist, x1 + 1, y1);
                        count += Accseslist(mainlist, x1 - 1, y1 - 1);
                        count += Accseslist(mainlist, x1, y1 - 1);
                        count += Accseslist(mainlist, x1 + 1, y1 - 1);

                        //Console.Write(count);
                    }
                }

            }
        }
        static bool CheckEnd()
        {
            Global_vars.fieldsclear = 0;

            foreach (List<int> sublist in Global_vars.fields_opened)
            {
                foreach (int value in sublist)
                {
                    if (value == 1)
                    {
                        Global_vars.fieldsclear++;
                    }
                }
            }
            foreach (List<int> sublist in Global_vars.flaglist)
            {
                foreach (int value in sublist)
                {
                    if (value == 1)
                    {
                        Global_vars.fieldsclear++;
                    }
                }
            }
            if (Global_vars.fieldsclear / (Global_vars.boardsize[0] * Global_vars.boardsize[1]) == 1)
            {
                Global_vars.gameState = 1;
                return true;
            }
            return false;
        }

        static (bool, int xF, int yF) Openaround(List<List<int>> coordinatelist, int x, int y)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    try
                    {
                        if (Global_vars.fields_opened[y - i][x - j] == 0)
                        {

                            string tempstring = Checkpos(coordinatelist, x - j, y - i);
                            if (tempstring == "M" && Global_vars.flaglist[y - i][x - j] == 0)
                            {
                                return (true, x - j, y - i);
                            }
                            else if (tempstring != "M" && Global_vars.flaglist[y - i][x - j] == 0)
                            {

                                Global_vars.nums[y - i][x - j] = tempstring;
                                Global_vars.fields_opened[y - i][x - j] = 1;
                                if (CheckEnd())
                                {
                                    Global_vars.gameState = 1;
                                }
                            }
                        }
                    }
                    catch { }
                }
            }

            return (false, x, y);
        }
        public class ResFile
        {
            public string Hash { set; get; }
            public int Size { set; get; }
        }

        public class ResRoot
        {
            public Dictionary<string, ResFile> Files { set; get; }
        }

        public class Jsontest
        {
            public int Id { set; get; }
            public dynamic Val { set; get; }
            public List<int> Boardsize { set; get; }
            public List<List<int>> Flaglist { set; get; }
            public List<List<int>> Fields_opened { set; get; }
            public List<List<int>> CoordinateList { set; get; }
            public List<List<string>> Nums { set; get; }
            public int GameState { set; get; }

        }

        static bool CheckAction(string Recived)
        {
            return true;
        }

        static bool ipc_update(BinaryReader br, BinaryWriter bw)
        {
            while(true)
            {
                try
                {
                    var len = (int)br.ReadUInt32();            // Read string length
                    var str = new string(br.ReadChars(len));    // Read string

                    //use functions
                    Console.Clear();
                    Console.ResetColor();
                    Console.WriteLine(str);
                    Dictionary<string, string> dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(str);

                    if (dic["mode"] == "-2")
                    {
                        End();
                    }
                    else if (dic["mode"] == "-1")
                    {
                        restart();
                    }

                    else if (Int32.TryParse(dic["btnNr"], out int xy))
                    {
                        if (xy >= 0)
                        {
                            int x = (xy) % Global_vars.boardsize[0];
                            int y = (xy) / Global_vars.boardsize[0];

                        
                            Console.WriteLine(x);
                            Console.WriteLine(y);
                            if (dic["mode"] == "0")
                            {
                                Buttonpress(x, y);
                            }
                            else if (dic["mode"] == "1")
                            {
                                SetFlag(x, y);
                            }
                        }

                    }
                    if (CheckEnd())
                    {
                        Global_vars.gameState = 1;
                    }
                    Jsontest jsontest = new()
                    {
                        Id = 1,
                        Val = Global_vars.jsontransfer["seed"],
                        Boardsize = Global_vars.jsontransfer["boardsize"],
                        Flaglist = Global_vars.flaglist,
                        Fields_opened = Global_vars.fields_opened,
                        CoordinateList = Global_vars.jsontransfer["cnl"],
                        Nums = Global_vars.nums,
                        GameState = Global_vars.gameState,
                    };

                    string temp = JsonConvert.SerializeObject(jsontest);
                    temp = "    " + temp;
                    var buf = Encoding.ASCII.GetBytes(temp);     // Get ASCII byte array
                    bw.Write(buf);                              // Write string

                    break;
                }
                catch (Exception ex){}
            }



            return true;
        }
        static bool End()
        {
            Environment.Exit(0);
            return true;
        }

        static bool restart()
        {
            Random rnd = new();
            Global_vars.gameState = 0;
            Global_vars.boardsize[0] = rnd.Next(8, 20);
            Global_vars.boardsize[1] = rnd.Next(8, 20);
            Global_vars.jsontransfer.Clear();
            Global_vars.jsontransfer.Add("boardsize", Global_vars.boardsize);

            Global_vars.gameState = 0;
            Console.ResetColor();
            Console.Clear();
            Console.SetWindowSize(Global_vars.boardsize[0] * 2 + 3, Global_vars.boardsize[1] + 4);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Black;
            //Console.ForegroundColor = ConsoleColor.White;
            Test Test = new();
            List<List<int>> coordinatelist = Test.Function();

            Global_vars.jsontransfer.Add("cnl", coordinatelist);

            Checklist(coordinatelist);

            List<int> templist = new();
            Global_vars.fields_opened.Clear();
            Global_vars.flaglist.Clear();
            for (int i = 0; i < Global_vars.boardsize[0]; i++)
            {
                templist.Add(0);
            }
            for (int i = 0; i < Global_vars.boardsize[1]; i++)
            {
                Global_vars.fields_opened.Add(new List<int>(templist));
                Global_vars.flaglist.Add(new List<int>(templist));
            }

            List<string> templist2 = new();
            Global_vars.nums.Clear();
            for (int i = 0; i < Global_vars.boardsize[0]; i++)
            {
                templist2.Add(" ");
            }
            for (int i = 0; i < Global_vars.boardsize[1]; i++)
            {
                Global_vars.nums.Add(new List<string>(templist2));
            }


            Global_vars.jsontransfer.Add("fOpen", Global_vars.fields_opened);
            Global_vars.jsontransfer.Add("fList", Global_vars.flaglist);
            Console.SetCursorPosition(0, 0);
            bool found = false;
            for (int i = 0; i <= coordinatelist.Count - 1; i++)
            {
                for (int j = 0; j <= coordinatelist[0].Count - 1; j++)
                {
                    if (coordinatelist[i][j] == 0)
                    {
                        if (int.Parse(Checkpos(coordinatelist, j, i)) == 0)
                        {
                            Open0(coordinatelist, j, i);
                            Console.SetCursorPosition(j * 2, i);
                            break;
                        }

                    }
                }
                if (found)
                {
                    break;
                }
            }
            return true;
        }

        static bool Buttonpress(int x, int y)
        {
            string current_pos = Checkpos(Global_vars.jsontransfer["cnl"], x, y);

            if (current_pos == "0")
            {
                Open0(Global_vars.jsontransfer["cnl"], x, y);
                if (CheckEnd())
                {
                    Global_vars.gameState = 1;
                }
            }
            else if (Global_vars.fields_opened[y][x] == 1)
            {
                var temp = Openaround(Global_vars.jsontransfer["cnl"], x, y);
                if (temp.Item1)
                {
                    Global_vars.gameState = -1;
                    //Console.SetCursorPosition(temp.xF * 2, temp.yF);
                    return false;
                }
                else if (CheckEnd())
                {
                    Global_vars.gameState = 1;
                }
            }
            else if (Global_vars.flaglist[y][x] == 1)
            {
                return true;
            }
            else if (current_pos == "M")
            {
                for (int i = 0; i < Global_vars.jsontransfer["cnl"].Count; i++)
                {
                    for (int j = 0; j < Global_vars.jsontransfer["cnl"][i].Count; j++)
                    {
                        if (Global_vars.jsontransfer["cnl"][i][j] == 1)
                        {
                            Global_vars.nums[y][x] = "M";
                            Global_vars.gameState = -1;
                        }
                    }
                }
                return true;
            }
            else
            {
                Global_vars.fields_opened[y][x] = 1;

                Global_vars.nums[y][x] = current_pos;
                if (CheckEnd())
                {
                    Global_vars.gameState = 1;
                }
            }
            return true;
        }

        static bool SetFlag(int x, int y)
        {

            if (Global_vars.fields_opened[y][x] == 1)
            {
                return true;
            }
            else if (Global_vars.flaglist[y][x] == 1)
            {
                Global_vars.flaglist[y][x] = 0; 
                return true;
            }
            else
            {
                Global_vars.flaglist[y][x] = 1;
                if (CheckEnd())
                {
                    Global_vars.gameState = 1;
                }
                return true;
            }

        }

        [STAThread]
        static void Main(string[] args)
        {
            UInt64 Errors = 0;

            string test = "{\"btnNr\": \"22\", \"mode\": \"0\"}";
            Dictionary<string, string> dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(test);


            foreach (string k  in dic.Keys)
            {
                Console.WriteLine(k);
                Console.WriteLine(dic[k]);
            }

            var server = new NamedPipeServerStream("NPtest", PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);

            Console.WriteLine("Waiting for connection...");
            server.WaitForConnection();

            Console.WriteLine("Connected.");
            var br = new BinaryReader(server);
            var bw = new BinaryWriter(server);

        Reset:
            Random rnd = new();
            Global_vars.gameState = 0;
            Global_vars.boardsize[0] = rnd.Next(8, 20);
            Global_vars.boardsize[1] = rnd.Next(8, 20);
            Global_vars.jsontransfer.Clear();
            Global_vars.jsontransfer.Add("boardsize", Global_vars.boardsize);

            Console.ResetColor();
            Console.Clear();
            Console.SetWindowSize(Global_vars.boardsize[0] * 2 + 3, Global_vars.boardsize[1] + 4);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Black;
            //Console.ForegroundColor = ConsoleColor.White;
            Test Test = new();
            List<List<int>> coordinatelist = Test.Function();
            
            Global_vars.jsontransfer.Add("cnl", coordinatelist);

            Checklist(coordinatelist);

            List<int> templist = new();
            Global_vars.fields_opened.Clear();
            Global_vars.flaglist.Clear();
            for (int i = 0; i < Global_vars.boardsize[0]; i++)
            {
                templist.Add(0);
            }
            for (int i = 0; i < Global_vars.boardsize[1]; i++)
            {
                Global_vars.fields_opened.Add(new List<int>(templist));
                Global_vars.flaglist.Add(new List<int>(templist));
            }

            List<string> templist2 = new();
            Global_vars.nums.Clear();
            for (int i = 0; i < Global_vars.boardsize[0]; i++)
            {
                templist2.Add(" ");
            }
            for (int i = 0; i < Global_vars.boardsize[1]; i++)
            {
                Global_vars.nums.Add(new List<string>(templist2));
            }


            Global_vars.jsontransfer.Add("fOpen", Global_vars.fields_opened);
            Global_vars.jsontransfer.Add("fList", Global_vars.flaglist);
            Console.SetCursorPosition(0, 0);
            for (int i = 0; i <= coordinatelist.Count - 1; i++)
            {
                for (int j = 0; j <= coordinatelist[0].Count - 1; j++)
                {
                    if (coordinatelist[i][j] == 0)
                    {
                        if (int.Parse(Checkpos(coordinatelist, j, i)) == 0)
                        {
                            Open0(coordinatelist, j, i);
                            Console.SetCursorPosition(j * 2, i);
                            goto start;
                        }

                    }
                }
            }

        start:
            while (true)
            {
                ipc_update(br, bw);/*
                while (Keyboard.IsKeyDown(Key.Up))
                {
                    int consoley = Console.CursorTop;
                    int consolex = Console.CursorLeft;
                    try
                    {
                        Console.SetCursorPosition(consolex, consoley - 1);
                    }
                    catch { }
                    System.Threading.Thread.Sleep(200);
                }

                while (Keyboard.IsKeyDown(Key.Down))
                {
                    int consoley = Console.CursorTop;
                    int consolex = Console.CursorLeft;
                    if (consoley >= Global_vars.boardsize[1] - 1)
                    {
                        continue;
                    }
                    try
                    {
                        Console.SetCursorPosition(consolex, consoley + 1);
                    }
                    catch { }
                    System.Threading.Thread.Sleep(200);
                }

                while (Keyboard.IsKeyDown(Key.Left))
                {
                    int consoley = Console.CursorTop;
                    int consolex = Console.CursorLeft;
                    try
                    {
                        Console.SetCursorPosition(consolex - 2, consoley);
                    }
                    catch { }
                    System.Threading.Thread.Sleep(200);
                }

                while (Keyboard.IsKeyDown(Key.Right))
                {
                    int consoley = Console.CursorTop;
                    int consolex = Console.CursorLeft;
                    if (consolex / 2 >= Global_vars.boardsize[0] - 1)
                    {
                        continue;
                    }
                    try
                    {
                        Console.SetCursorPosition(consolex + 2, consoley);
                    }
                    catch { }
                    System.Threading.Thread.Sleep(200);
                }
                while (Keyboard.IsKeyDown(Key.Space))
                {
                    while (!Keyboard.IsKeyUp(Key.Space))
                    {
                    }
                    Console.ResetColor();
                    int consoley = Console.CursorTop;
                    int consolex = Console.CursorLeft;
                    string current_pos = Checkpos(coordinatelist, consolex / 2, consoley);

                    if (current_pos == "0")
                    {
                        Open0(coordinatelist, consolex / 2, consoley);
                    }
                    else if (Global_vars.fields_opened[consoley][consolex / 2] == 1)
                    {
                        var temp = Openaround(coordinatelist, consolex / 2, consoley);
                        if (temp.Item1)
                        {
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.SetCursorPosition(temp.xF * 2, temp.yF);
                            Console.Write("M");
                            goto Endwhile;
                        }
                    }
                    else if (Global_vars.flaglist[consoley][consolex / 2] == 1)
                    {
                        break;
                    }
                    else if (current_pos == "M")
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.Black;
                        for (int i = 0; i < coordinatelist.Count; i++)
                        {
                            for (int j = 0; j < coordinatelist[i].Count; j++)
                            {
                                if (coordinatelist[i][j] == 1)
                                {
                                    Console.SetCursorPosition(j * 2, i);
                                    Console.Write("M");
                                }
                            }
                        }
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write(current_pos);

                        goto Endwhile;
                    }
                    else
                    {
                        Console.ForegroundColor = Global_vars.colors[Int32.Parse(current_pos)];

                        Console.Write(current_pos);
                        Global_vars.fields_opened[consoley][consolex / 2] = 1;

                        if (CheckEnd())
                        {
                            goto Endwhile;
                        }
                        //TODO: test

                    }
                    Console.SetCursorPosition(consolex, consoley);

                }
                while (Keyboard.IsKeyDown(Key.F))
                {
                    while (!Keyboard.IsKeyUp(Key.F)) { }
                    int consoley = Console.CursorTop;
                    int consolex = Console.CursorLeft;
                    if (Global_vars.fields_opened[consoley][consolex / 2] == 1)
                    {
                        break;
                    }
                    else if (Global_vars.flaglist[consoley][consolex / 2] == 1)
                    {
                        Global_vars.flaglist[consoley][consolex / 2] = 0;
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                        string tempvar = Checkpos(coordinatelist, consolex / 2, consoley);

                        if (Global_vars.fields_opened[consoley][consolex / 2] == 1)
                        {
                            Console.ForegroundColor = Global_vars.colors[Int32.Parse(tempvar)];

                            Console.Write(tempvar);
                        }
                        else
                        {
                            Console.Write(" ");
                        }
                    }
                    else
                    {
                        Global_vars.flaglist[consoley][consolex / 2] = 1;
                        Console.ResetColor();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("F");
                        if (CheckEnd())
                        {
                            goto Endwhile;
                        }
                    }

                    Console.SetCursorPosition(consolex, consoley);
                }
                while (Keyboard.IsKeyDown(Key.Escape))
                {
                    goto End;
                }*/
            }
        Endwhile:
            Console.ResetColor();
            Console.SetCursorPosition(0, coordinatelist.Count + 1);
            Console.WriteLine("ENDED");
            while (true)
            {
                while (Keyboard.IsKeyDown(Key.Space))
                {
                    goto Reset;
                }
                while (Keyboard.IsKeyDown(Key.Escape))
                {
                    goto End;
                }
                

            }

        End:
            Console.WriteLine("Closed");
            server.Close();
            server.Dispose();
        }
    }
}

/* savespace
            Testmethod();
            Console.WriteLine("Please enter length");
            double length = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("Please enter width");
            double width = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("something: {0}", width * length);
            Console.ReadLine();



            string length;
            double lengthint;
            string width;
            double widthint;
            string heigth;
            double heightint;
            Console.WriteLine("Volumenberechnung.");
            Console.WriteLine("Geben sie die Länge ein");
            while (true)
            {
                length = Console.ReadLine();

                if(double.TryParse(length, out lengthint))
                {
                    Console.WriteLine("success");
                    break;
                }
                else{ Console.WriteLine("please try again"); }
            }

            Console.WriteLine("Geben sie die Breite ein");
            while (true)
            {
                width = Console.ReadLine();

                if (double.TryParse(width, out widthint))
                {
                    Console.WriteLine("success");
                    break;
                }
                else { Console.WriteLine("please try again"); }
            }

            Console.WriteLine("Geben sie die Höhe ein");
            while (true)
            {
                heigth = Console.ReadLine();

                if (double.TryParse(heigth, out heightint))
                {
                    Console.WriteLine("success");
                    break;
                }
                else { Console.WriteLine("please try again"); }
            }
            double fläche = lengthint * widthint;
            Console.WriteLine("Fläche: {0}", fläche);
            Console.WriteLine("Volumen: {0}", fläche*heightint);
*/