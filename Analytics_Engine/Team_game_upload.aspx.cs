﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Team_game_upload : System.Web.UI.Page
{   
    List<string> link_list = new List<string>();
    protected void Page_Load(object sender, EventArgs e)
    {
        //List<string> link_list = new List<string>();
        int counter = 0;
        string line;
        string line1;
        System.IO.StreamReader file =
            new System.IO.StreamReader(@"E:/Teams/Alaska_Anchorage_Links.txt");
        while ((line = file.ReadLine()) != null)
        {
            link_list.Add(line);
            counter++;
        }
        file.Close();
        //Label1.Text = link_list[26];
         
        for (int i = 0; i < link_list.Count; i++)
        {   
            line1=link_list[i];
            link_upload(line1);
        }
    }

    protected void link_upload(string link_list)
    {
        TextBox2.Text = "";
        Worker worker = new Worker();
        string url_main = link_list;
        string[] url = new string[4];
        url[0] = url_main;
        if (url_main.Contains("?org_id"))
        {
            string[] temp = { "?org_id" };
            temp = url_main.Split(temp, StringSplitOptions.RemoveEmptyEntries);
            url_main = temp[0];
        }
        url[1] = url_main + "?period_no=1";
        url[2] = url_main + "?period_no=2";
        url[3] = url_main.Replace("index", "play_by_play");
        string[] half = new string[3];
        half[0] = "full";
        half[1] = "1";
        half[2] = "2";


        for (int i = 0; i < 4; i++)
        {

            if (i == 0)
                worker.deletePlayer();
            if (i < 3)
            {
                bool tableExist = false;

                tableExist = mainProcess(worker, url[i], i, half[i]);


                if (tableExist)
                    break;
            }
            if (i == 2)
                worker.execBoxScoreCreationWarehouse();
            if (i == 3)
            {
                try
                {
                    processPlayByPlay(worker, url[i], i);
                }
                catch (System.Net.WebException we)
                {
                    TextBox2.Text += "\r\nunable to process Play By Play due to source server error!";

                }
            }


        }


    }


    protected void TextBox2_TextChanged(object sender, EventArgs e)
    {

    }

    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void processPlayByPlay(Worker worker, string url, int count)
    {
        string sc = worker.getUrl(url);
        String team1_data, team2_data;
        string[,] game_time1 = new string[1, 2];
        string[] words; //storing tables
        string[] st = { "<table " };
        string[] tr_tokenizer3 = { "</table>" };
        int game_id = 0;
        if (HttpContext.Current.Session["game_id"] != null)
            game_id = (int)HttpContext.Current.Session["game_id"];

        string[,] teamDict = (string[,])HttpContext.Current.Session["team_dict"];


        //splitting into tables
        words = sc.Split(st, StringSplitOptions.RemoveEmptyEntries);

        team2_data = words[words.Length - 1];  //last table
        team1_data = words[words.Length - 3];  // 3rd last table

        string[] team1_data_1 = team1_data.Split(tr_tokenizer3, StringSplitOptions.RemoveEmptyEntries);
        string team1_str = "<HTML><BODY> <table " + team1_data_1[0] + "</table> </BODY></HTML>";

        string[] team2_data_1 = team2_data.Split(tr_tokenizer3, StringSplitOptions.RemoveEmptyEntries);
        string team2_str = "<HTML><BODY> <table " + team2_data_1[0] + "</table> </BODY></HTML>";



        worker.processPlayByPlay(team1_str, teamDict, game_id, "1");
        worker.processPlayByPlay(team2_str, teamDict, game_id, "2");

    }
    protected bool mainProcess(Worker worker, string url, int count, string half)
    {

        HttpContext.Current.Session["playing_half"] = half;
        //string sc = System.IO.File.ReadAllText(@"D:\1.HTML");
        //  StreamWriter sw = new StreamWriter("D:/web1.txt");
        //   System.Console.WriteLine(sc);
        //  sw.Write(sc);

        string sc = worker.getUrl(url);
        String team1, team2, team_table, game_table = "";
        string[,] game_time1 = new string[1, 2];
        string[] words; //storing tables
        string[] st = { "<table " };
        //splitting into tables
        words = sc.Split(st, StringSplitOptions.RemoveEmptyEntries);

        string[] tr_tokenizer3 = { "</table>" };

        team1 = words[words.Length - 1];  //last table
        team2 = words[words.Length - 2];  //2nd last table

        team_table = words[1];   // 1st table.

        //dynamically assigning table with game date data
        foreach (string temp in words)
        {
            if (temp.Contains("Game Date:"))
                game_table = temp;
        }

        //creating table string for table 1
        string[] tr_team1_1 = team1.Split(tr_tokenizer3, StringSplitOptions.RemoveEmptyEntries);
        string team1_str = "<HTML><BODY> <table " + tr_team1_1[0] + "</table> </BODY></HTML>";

        //creating table string for team name table - 1st table
        string[] tr_team_name = team_table.Split(tr_tokenizer3, StringSplitOptions.RemoveEmptyEntries);
        string team_name_str = "<HTML><BODY> <table " + tr_team_name[0] + "</table> </BODY></HTML>";

        //game detail processing
        string game_detail = worker.processGameTable("<HTML><BODY> <table " + game_table + "</BODY></HTML>");

        // extractign team name - team id and assignning to session
        string[,] teamDict = worker.processTeamNameTable(team_name_str);
        HttpContext.Current.Session["team_dict"] = teamDict;


        if (game_detail != null)
        {
            game_time1 = Worker.getDateTime(game_detail);
            HttpContext.Current.Session["game_date"] = game_time1[0, 0];
            HttpContext.Current.Session["game_time"] = game_time1[0, 1];
        }


        int tableExist = 0;
        //check only once if the match exist in the database
        if (count == 0)
            tableExist = worker.getMaxCount((string)HttpContext.Current.Session["game_date"], (string)HttpContext.Current.Session["game_time"], teamDict[0, 1], teamDict[1, 1]);



        if (tableExist < 1)
        {
            if (count == 0)
            {
                int game_id = worker.insertGameData((string)HttpContext.Current.Session["game_date"], (string)HttpContext.Current.Session["game_time"], teamDict[0, 1], teamDict[1, 1]);
                HttpContext.Current.Session["game_id"] = game_id;
            }
            string season = DropDownList1.SelectedValue;
            HttpContext.Current.Session["season"] = season;
            //Processing player tables
            string s, s1 = "";
            s = worker.processTeamTable("<HTML><BODY> <table " + team2 + "</BODY></HTML>");
            s1 = worker.processTeamTable(team1_str);
            TextBox2.Text = "The match has been processed successfully!\r\nMatch details: \r\n" +
              "Date: " + HttpContext.Current.Session["game_date"] + "\r\nTime: " + HttpContext.Current.Session["game_time"] +
              "\r\n" + teamDict[0, 0] + " VS " + teamDict[1, 0] + "";
            return false;
        }
        else
        {
            TextBox2.Text = "The match already exists in the database!\r\nMatch details: \r\n" +
              "Date: " + HttpContext.Current.Session["game_date"] + "\r\nTime: " + HttpContext.Current.Session["game_time"] +
              "\r\n" + teamDict[0, 0] + " VS " + teamDict[1, 0] + "";
            return true;
        }


    }
    protected void Button1_Click1(object sender, EventArgs e)
    {

    }
}