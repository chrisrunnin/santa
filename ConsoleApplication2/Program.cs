﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
            Program prog = new Program();
#pragma warning disable CS4014 // Da dieser Aufruf nicht abgewartet wird, wird die Ausführung der aktuellen Methode fortgesetzt, bevor der Aufruf abgeschlossen ist
            prog.santaAsync();
#pragma warning restore CS4014 // Da dieser Aufruf nicht abgewartet wird, wird die Ausführung der aktuellen Methode fortgesetzt, bevor der Aufruf abgeschlossen ist
            Thread.Sleep(120000);
        }

        List<Task> reindeerTask = new List<Task>();
        List<Task> elfTask = new List<Task>();
        List<Thread> abortions = new List<Thread>();

        // ############################## SANTA THE HANDLER ##############################

        private async Task santaAsync()
        {
            bool flag = true;
            Task elfs, reindeer, task;
            sendElfsToWork();

            elfs = elfsAsync();

            while (true)
            {
                christmasWork();
                releaseTheReindeer();

                if (!flag) { 
                    elfs = elfsAsync();
                    flag = false;
                }
                reindeer = reindeerAsync();
                task = await Task.WhenAny(elfs, reindeer);

                while(task != reindeer) { 
                    problemSolving();
                    sendElfsToWork();
                    elfs = elfsAsync();
                    task = await Task.WhenAny(elfs, reindeer);
                    if(task == reindeer)
                    {
                        flag = true;
                    }
                }
            }
        }

        // ############################## START TASKS ##############################

        private void releaseTheReindeer()
        {
            Console.WriteLine("Releasing the Reindeer...");
            for (int i = 0; i < 9; i++)
            {
                Random r = new Random();
                reindeerTask.Add(Task.Run(() => reindeerVacation()));
                Thread.Sleep(r.Next(1, 100));
            }
            Console.WriteLine("Zischhhh");
        }

        private void sendElfsToWork()
        {
            Console.WriteLine("Sending elfs back to work...");
            if (!elfTask.Any())
            {
                for (int i = 0; i < 12; i++)
                {
                    Random r = new Random();
                    elfTask.Add(Task.Run(() => elfWork()));
                    Thread.Sleep(r.Next(1, 100));
                }
            }
            else
            {
                while (elfTask.Count < 12)
                {
                    elfTask.Add(Task.Run(() => elfWork()));
                }
            }
            Console.WriteLine("All elfs back at work");
        }

        // ############################## AWAIT TASKS ##############################
        
        private async Task reindeerAsync()
        {

            await Task.WhenAll(reindeerTask);
            reindeerTask.Clear();
            Console.WriteLine("REINDEER: Wake up Santa!");

        }

        private async Task elfsAsync()
        {
            int i = 0;
            while (i < 3)
            {
                Task t = await Task.WhenAny(elfTask);
                elfTask.Remove(t);
                i++;
            }
            Console.WriteLine("ELFS: Wake up Santa!");
        }

        // ############################## ACTUAL TASKS ##############################

        private Task reindeerVacation()
        {
            return Task.Run(() => {
                Random r = new Random();
                Thread.Sleep(r.Next(3000, 10000));
                Console.WriteLine(" . . . . . . . . . . . . . . . . . . . .Reindeer is back");
            });
        }

        private Task elfWork()
        {
            return Task.Run(() => {
                abortions.Add(Thread.CurrentThread);
                Random r = new Random();
                Thread.Sleep(r.Next(3000, 100000));
                Console.WriteLine(". . . . . . . . . . . . . . . . . . . .Elf has a problem");
            });
        }

        // ############################## MOST IMPORTANT CODE ##############################

        private void christmasWork()
        {
            Console.WriteLine("Doing Christmas work...");
            Thread.Sleep(3000);
            Console.WriteLine("Back from Christmas trip");
        }

        private void problemSolving()
        {
            Console.WriteLine("Taking care of Elf problems...");
            Thread.Sleep(3000);
            Console.WriteLine("Problems fixed");
        }
    }

}