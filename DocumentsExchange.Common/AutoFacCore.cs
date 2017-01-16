﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;

namespace DocumentsExchange.Common
{
   public class AutoFacCore
    {
        public static IContainer Core => _core.Value;

        private static readonly Lazy<IContainer> _core = new Lazy<IContainer>(CreateCore);

        private static readonly List<IModule> _modules = new List<IModule>();
        private static readonly List<Action<ContainerBuilder>> _extensions = new List<Action<ContainerBuilder>>();

        public static void Init(params IModule[] modules)
        {
            _modules.Clear();
            _modules.AddRange(modules);
        }

        public static void Extend(Action<ContainerBuilder> extension)
        {
            _extensions.Add(extension);
        }

        private static IContainer CreateCore()
        {
            var builder = new ContainerBuilder();

            _modules.ForEach(m => builder.RegisterModule(m));
            _extensions.ForEach(e => e(builder));

            return   builder.Build();
        }
        
    }
}
