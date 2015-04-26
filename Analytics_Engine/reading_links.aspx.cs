using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class reading_links : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        List<string> link_list = new List<string>();

        //string[] link_array;

        int counter = 0;
        string line;

        // Read the file and display it line by line.
        System.IO.StreamReader file =
            new System.IO.StreamReader(@"E:/Teams/SFU_Links.txt");
        while ((line = file.ReadLine()) != null)
        {
            link_list.Add(line);
            //System.Console.WriteLine(link_list[counter]);
            counter++;
        }
        file.Close();
        //System.Console.WriteLine("There were {0} lines.", counter);
        // Suspend the screen.
        //System.Console.ReadLine();
        Label1.Text = link_list[0];
    }
}