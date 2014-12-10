using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Way2Test1.Dictionary;
using Way2Test1.Interface;

namespace Way2Test1 {

    class Program {

        static ConsoleInterface Interface;

        static void Main(string[] args) {

            WebServiceAccessList al = WebServiceAccessList.Load();
            al.Clear();
            WebServiceAccessList.Save(al);


            //Interface = new ConsoleInterface(SearchKeyWorkOnServer);
            //Interface.Show();


        }

        static int[] SearchKeyWorkOnServer(string keyword) {

            return new int[]{1, 1};
        }
    }

}
