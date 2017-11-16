using Discord;
using Discord.Commands;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MadSweetty
{
    class MyBot
    {
        DiscordClient discord;
        public static bool bucle = true;
        public static int time_said = 0;
        public MyBot()
        {
            discord = new DiscordClient(x =>
            {
                x.LogLevel = LogSeverity.Info;
                x.LogHandler = Log;
            });

            discord.UsingCommands(x =>
            {
                x.PrefixChar = '%';
                x.AllowMentionPrefix = true;
            });

            var commands = discord.GetService<CommandService>();

            commands.CreateCommand("h").Do(async (e) => {
                await e.Channel.SendMessage("write %startCB [how many minutes(more than 3 minutes) left since 'now' to appear boss ANOUNCEMENT]");
                await e.Channel.SendMessage("write %stopCB [to stop all the announcementes of the bot in this channel]");
            });
            commands.CreateCommand("version").Do(async (e) => {
                await e.Channel.SendMessage("Bot for Celestial Basin alarm, Version 1.0, written by sshazoxt " +
                    "http://static.bladeandsoul.com/uploads/articles/images/Celestial_Basin_01.jpg " +
                    "Commands:[startIn]:Time in with will start the count of 43 minutes");
            });
            commands.CreateCommand("startCB").Parameter("timetostart", ParameterType.Required).Do(async (e) => {
                await e.Channel.SendMessage($":16:```post msg for CB Boss will start in {e.GetArg("timetostart")} minutes```");
                time_said = Convert.ToInt32(e.GetArg("timetostart"))*60*1000;
                bucle = true;
                bool done = await Timer(e, Convert.ToInt32(e.GetArg("timetostart"))*60*1000);
            });
            commands.CreateCommand("stopCB").Do(async (e) => {
                bucle = false;
                await e.Channel.SendMessage("All msg are stoped");
            });
            commands.CreateCommand("showtimesaid").Do(async (e) => {
                await e.Channel.SendMessage($"the time u said was {time_said}");
            });

            discord.ExecuteAndWait(async () =>
            {
                await discord.Connect("", TokenType.Bot);
            });
        }

        private void Log(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
        
        public async Task<bool> Timer(CommandEventArgs e, int time)
        {
            bool done = false;
            int time_def = 0;
            while (bucle == true)
            {
                done = false;
                await Task.Delay(time + time_def - 3*60*1000);//3minutes
                
                done = true;
                if (bucle){
                    await e.Channel.SendMessage("CB boss ***ANOUNCEMENT*** in 3 minutes, 6 minutes till the boss appears");
                }else{break;}
                done = false;
                await Task.Delay(3*60*1000);//3minutes
                done = true;
                if (bucle){
                    await e.Channel.SendMessage("CB boss ***WILL APPEARS*** in 3 minutes");
                }else { break; }
                
                done = false;
                await Task.Delay(1*60*1000);//1minute
                done = true;
                if (bucle){
                    await e.Channel.SendMessage("CB boss will appears in less than 2 minutes");
                }else { break; }
                time = 0;
                time_def = 39*60*1000;
            }
            return done;
        }

    }
}
