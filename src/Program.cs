using System;
using System.Collections.Generic;
using NATS.Client;
using System.Text;
using NLog;
using NLog.Config;
using openrmf_msg_controls.Models;
using openrmf_msg_controls.Classes;
using openrmf_msg_controls.Database;
using Newtonsoft.Json;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace openrmf_msg_controls
{
    class Program
    {

        private static int GetFirstIndex(string term) {
            int space = term.IndexOf(" ");
            int period = term.IndexOf(".");
            if (space < 0 && period < 0)
                return -1;
            else if (space > 0 && period > 0 && space < period ) // see which we hit first
                return space;
            else if (space > 0 && period > 0 && space > period )
                return period;
            else if (space > 0) 
                return space;
            else 
                return period;
        }

        static void Main(string[] args)
        {
            LogManager.Configuration = new XmlLoggingConfiguration($"{AppContext.BaseDirectory}nlog.config");

            var logger = LogManager.GetLogger("openrmf_msg_controls");
            ControlsDBContext _context;

            // Create a new connection factory to create a connection.
            ConnectionFactory cf = new ConnectionFactory();

            // Creates a live connection to the default NATS Server running locally
            IConnection c = cf.CreateConnection(Environment.GetEnvironmentVariable("natsserverurl"));
            var options = new DbContextOptionsBuilder<ControlsDBContext>().UseInMemoryDatabase("ControlSet").Options;
            _context = new ControlsDBContext(options);

            // setup the internal database
            ControlsLoader.LoadControlsXML(_context);

            // send back a full listing of controls based on the filter passed in
            EventHandler<MsgHandlerEventArgs> getControls = (sender, natsargs) =>
            {
                try {
                    // print the message
                    logger.Info("New NATS subject: {0}", natsargs.Message.Subject);
                    logger.Info("New NATS data: {0}",Encoding.UTF8.GetString(natsargs.Message.Data));
                    var listing = _context.ControlSets.ToList();
                    var result = new List<ControlSet>(); // put all results in here
                    Filter filter = JsonConvert.DeserializeObject<Filter>(Encoding.UTF8.GetString(natsargs.Message.Data));
                    if (listing != null) {
                        // figure out the impact level filter
                        if (filter.impactLevel.Trim().ToLower() == "low")
                            result = listing.Where(x => x.lowimpact).ToList();
                        else if (filter.impactLevel.Trim().ToLower() == "moderate")
                            result = listing.Where(x => x.moderateimpact).ToList();
                        else if (filter.impactLevel.Trim().ToLower() == "high")
                            result = listing.Where(x => x.highimpact).ToList();

                        // include things that are not P0 meaning not used, and that there is no low/moderate/high designation
                        // these should always be included where the combination of all "false" and not P0 = include them
                        result.AddRange(listing.Where(x => x.priority != "P0" && 
                            !x.lowimpact && !x.moderateimpact && !x.highimpact ).ToList());

                        // see if the PII  filter is true, and if so add in the PII family by appending that to the result from above
                        if (filter.pii) {
                            result.AddRange(listing.Where(x => !string.IsNullOrEmpty(x.family) && x.family.ToLower() == "pii").ToList());
                        }
                    }
                    // now publish it back out w/ the reply subject
                    string msg = JsonConvert.SerializeObject(result);
                    // publish back out on the reply line to the calling publisher
                    logger.Info("Sending back compressed Checklist Data");
                    c.Publish(natsargs.Message.Reply, Encoding.UTF8.GetBytes(Compression.CompressString(msg)));
                    c.Flush(); // flush the line
                }
                catch (Exception ex) {
                    // log it here
                    logger.Error(ex, "Error retrieving checklist record for artifactId {0}", Encoding.UTF8.GetString(natsargs.Message.Data));
                }
            };
            
            // The simple way to create an asynchronous subscriber
            // is to simply pass the event in.  Messages will start
            // arriving immediately.
            logger.Info("setting up the openRMF control subscription by filter");
            IAsyncSubscription asyncNew = c.SubscribeAsync("openrmf.controls", getControls);
        }
    }
}
