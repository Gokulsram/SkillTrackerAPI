using Enyim.Caching;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SkillTracker.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SkillTracker.AWSServerless.RabbitMQ.Receive
{
    public class UserSkillUpdateReceiver : BackgroundService
    {
        private IModel _channel;
        private IConnection _connection;
        private readonly string _hostname;
        private readonly string _queueName;
        private readonly string _username;
        private readonly string _password;
        const string _uerProfile = "UserProfile";
        private readonly IMemcachedClient _memCache;

        public UserSkillUpdateReceiver(IOptions<RabbitMqConfiguration> rabbitMqOptions, IMemcachedClient memCache)
        {
            Console.WriteLine("Tets 1");
            _hostname = rabbitMqOptions.Value.Hostname;
            _queueName = rabbitMqOptions.Value.QueueName;
            _username = rabbitMqOptions.Value.UserName;
            _password = rabbitMqOptions.Value.Password;
            _memCache = memCache;
            InitializeRabbitMqListener();
        }
        private void InitializeRabbitMqListener()
        {
            try
            {
                Console.WriteLine("Tets 2");
                Uri uri = new Uri(_hostname);

                var factory = new ConnectionFactory
                {
                    Uri = uri,
                    //HostName = _hostname,
                    //HostName = "localhost",
                    //Port = 15672,
                    //Port = 5671,
                    UserName = _username,
                    Password = _password
                };
                Console.WriteLine("Tets 3 - Gokul");
                _connection = factory.CreateConnection();
                Console.WriteLine("Tets 3-1");
                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
                Console.WriteLine("Tets 3-2");
                _channel = _connection.CreateModel();
                Console.WriteLine("Tets 4");
                _channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                Console.WriteLine("Tets 5");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Tets 3-3" + ex.StackTrace);
            }
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            Console.WriteLine("Tets 6");
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                var userSkillModel = JsonConvert.DeserializeObject<UserSkill>(content);
                Console.WriteLine("Tets 7");
                try
                {
                    Console.WriteLine("Tets 8");
                    var listProfiles = _memCache.Get<List<UserSkill>>(_uerProfile);
                    if (listProfiles == null)
                        listProfiles = new List<UserSkill>();
                    listProfiles.Add(userSkillModel);
                    _memCache.Set(_uerProfile, listProfiles, 24 * 40 * 60);
                    Console.WriteLine("Tets 9");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                Console.WriteLine("Tets 10");
                _channel.BasicAck(ea.DeliveryTag, false);
            };
            consumer.Shutdown += OnConsumerShutdown;
            consumer.Registered += OnConsumerRegistered;
            consumer.Unregistered += OnConsumerUnregistered;
            consumer.ConsumerCancelled += OnConsumerCancelled;

            _channel.BasicConsume(_queueName, false, consumer);

            return Task.CompletedTask;
        }

        private void OnConsumerCancelled(object sender, ConsumerEventArgs e)
        {
        }

        private void OnConsumerUnregistered(object sender, ConsumerEventArgs e)
        {
        }

        private void OnConsumerRegistered(object sender, ConsumerEventArgs e)
        {
        }

        private void OnConsumerShutdown(object sender, ShutdownEventArgs e)
        {
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
