﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Akka.Actor;
using Akka.Event;
using Akka.Routing;
using AkkaDemo.Common;
using AkkaDemo.Common.Actors;
using AkkaDemo.Common.Messages;

namespace AkkaDemo.ClientUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ActorSystem _clientActorSystem = ActorSystem.Create("akkaDemo");
        private readonly IActorRef _api;
        private int _jobId = 1;

        public MainWindow()
        {
            InitializeComponent();
            _api = _clientActorSystem.ActorOf(Props.Empty.WithRouter(FromConfig.Instance), "api");
        }

        private void LogButton_Click(object sender, RoutedEventArgs e)
        {
            var message = new LogEntryMessage("UI-Client", LogEventType.Info, LogMessage.Text);
            _api.Tell(message);
        }

        private void ScheduleJobButton_Click(object sender, RoutedEventArgs e)
        {
            AddStatusLog($"Sending job #{ _jobId }.");

            var job = new ReportMessage(_jobId++, ReportTitle.Text);

            ReportTitle.SelectAll();

            Task.Run(async () =>
                           {
                               var scheduledJob = _api.Ask(job);
                               var ack = await scheduledJob;
                               AddStatusLog(ack.ToString());
                           });
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            var firstOp = Int32.Parse(FirstOperand.Text);
            var secondOp = Int32.Parse(SecondOperand.Text);
            AddStatusLog($"Calculating with: {firstOp}, {secondOp}");
            var msg = new CalcMessage(firstOp, secondOp);
            Task.Run(async () =>
            {
                var calcJob = _api.Ask(msg);
                var ack = await calcJob;
                var result = ((CalcResultMessage)ack).Result;
                AddStatusLog($"Result = {result}");
            });
        }

        private void AddStatusLog(string msg)
        {
            StatusBox.Dispatcher
                     .BeginInvoke(DispatcherPriority.Render, new Action(() => { StatusBox.AppendText(Environment.NewLine + msg); }));
        }
    }
}
