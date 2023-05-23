using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Milkshake.Models.Interfaces;

namespace Milkshake
{
    public class InstanceBuilder<T> where T : class, IInstanceBase, new()
    {
        public string InstanceFolder { get; private set; } = "Milkshake";
        //public string? Vips { get; private set; }

        private readonly T _instance = new T();

        private readonly MilkshakeService _service;

        public InstanceBuilder(MilkshakeService service)
        {
            _service = service;
        }

        public InstanceBuilder<T> CreateInstance()
        {
            Directory.CreateDirectory($"{_service.Options.BasePath}/cache");
            Directory.CreateDirectory($"{_service.Options.BasePath}/{InstanceFolder}");
            Directory.CreateDirectory($"{_service.Options.BasePath}/{InstanceFolder}/source");
            Directory.CreateDirectory($"{_service.Options.BasePath}/{InstanceFolder}/template");
            return this;
        }

        public InstanceBuilder<T> WithVip(ulong user)
        {
            _instance.Vips += $";{user}";
            return this;
        }

        public InstanceBuilder<T> WithVip(IEnumerable<ulong> users)
        {
            _instance.Vips += ";" + string.Join(';', users);
            return this;
        }

        public InstanceBuilder<T> SetInstanceFolder(string? folderName = null)
        {
            InstanceFolder = folderName ?? _instance.ContextId.ToString();
            CreateInstance();
            return this;
        }

        public T Build()
        {
            //_instance.Vips = Vips;
            return _instance;
        }
    }
}
