﻿using System;
using Microsoft.ServiceBus.Messaging;

namespace Hangfire.Azure.ServiceBusQueue
{
    public class ServiceBusQueueOptions
    {
        public ServiceBusQueueOptions()
        {
            this.CheckAndCreateQueues = true;
            this.LoopReceiveTimeout = TimeSpan.FromMilliseconds(500);
        }

        /// <summary>
        /// Gets or sets the prefix that will be prepended to all queue names in
        /// the service bus (e.g. if the prefix is "a-prefix-" then the default queue
        /// will be named "a-prefix-default" in Azure)
        /// </summary>
        public string QueuePrefix { get; set; }

        /// <summary>
        /// Configures a queue on construction, for example setting maximum message
        /// size or default TTL.
        /// </summary>
        public Action<QueueDescription> Configure { get; set; }

        /// <summary>
        /// Gets or sets a value which indicates whether or not to automatically create and
        /// configure queues.
        /// </summary>
        /// <remarks>
        /// On initialisation if this property is <cc>true</cc> we will create and check all queues
        /// immediately, otherwise we delay the creation of the queue clients until they are first
        /// requested.
        /// </remarks>
        public bool CheckAndCreateQueues { get; set; }

        public string ConnectionString { get; set; }

        public string[] Queues { get; set; }

        /// <summary>
        /// Gets or sets a timeout that is used between loop runs of receiving messages from Azure Service Bus. This is the timeout
        /// used when waiting on the last queue before looping around again (does not apply when only a single-queue exists).
        /// </summary>
        /// <remarks>
        /// Typically a lower value is desired to keep the throughput of message processing high. A lower timeout means more calls to
        /// Azure Service Bus which can increase costs, especially on an under-utilised server with few jobs.
        /// </remarks>
        /// <value>Defaults to <c>TimeSpan.FromMilliseconds(500)</c></value>
        public TimeSpan LoopReceiveTimeout { get; set; }

        internal string GetQueueName(string name)
        {
            if (QueuePrefix != null)
            {
                return QueuePrefix + name;
            }

            return name;
        }

        internal void Validate()
        {
            if (ConnectionString == null)
                throw new InvalidOperationException("Must supply ConnectionString to ServiceBusQueueOptions");

            if (Queues == null)
                throw new InvalidOperationException("Must supply Queues to ServiceBusQueueOptions");

            if (Queues.Length == 0)
                throw new InvalidOperationException("Must supply at least one queue in Queues property of ServiceBusQueueOptions");
        }
    }
}