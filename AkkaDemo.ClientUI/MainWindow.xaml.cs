using System;
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
using AkkaDemo.Common;
using AkkaDemo.Common.Messages;

namespace AkkaDemo.ClientUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ActorSystem _clientActorSystem = ActorSystem.Create("UIClient");
        private readonly ActorSelection _logger;
        private readonly ActorSelection _reporter;
        private int _jobId = 1;

        public MainWindow()
        {
            InitializeComponent();

            var serverLocation = ConfigurationManager.AppSettings["serverLocation"];
            _logger = _clientActorSystem.ActorSelection($"akka.tcp://{ serverLocation }/user/LogCoordinator");
            _reporter = _clientActorSystem.ActorSelection($"akka.tcp://{ serverLocation }/user/Report");
        }

        private void LogButton_Click(object sender, RoutedEventArgs e)
        {
            var message = new LogEntryMessage("UI-Client", LogEventType.Info, LogMessage.Text);
            _logger.Tell(message);
        }

        private void ScheduleJobButton_Click(object sender, RoutedEventArgs e)
        {
            AddStatusLog($"Sending job #{ _jobId }.");

            var job = new ReportMessage
                      {
                          JobId = _jobId++,
                          ReportTitle = ReportTitle.Text
                      };

            Task.Run(async () =>
                           {
                               var scheduledJob = _reporter.Ask(job);
                               var ack = await scheduledJob;
                               AddStatusLog(ack.ToString());
                           });
        }

        private void AddStatusLog(string msg)
        {
            StatusBox.Dispatcher
                     .BeginInvoke(DispatcherPriority.Render, new Action(() => { StatusBox.AppendText(Environment.NewLine + msg); }));
        }
    }
}
