﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Milkshake.Models.Interfaces;

namespace Milkshake
{
    public class InstanceManager<T> where T : class, IInstanceBase, new()
    {
        public string InstanceFolder { get; private set; } = "Milkshake";
        //public string? Vips { get; private set; }

        private readonly T _instance = new T();

        private readonly MilkshakeService _service;

        public InstanceManager(MilkshakeService service)
        {
            _service = service;
        }

        public InstanceManager<T> CreateInstance()
        {
            Directory.CreateDirectory($"{_service.Options.BasePath}/{InstanceFolder}");
            return this;
        }

        public InstanceManager<T> SetVipList(IReadOnlyList<string> vip)
        {
            _instance.Vips = string.Join(';', vip);
            return this;
        }

        public InstanceManager<T> SetInstanceFolder(string? folderName = null)
        {
            InstanceFolder = folderName ?? _instance.ContextId.ToString();
            return this;
        }

        public T Build()
        {
            //_instance.Vips = Vips;
            return _instance;
        }
    }
}
