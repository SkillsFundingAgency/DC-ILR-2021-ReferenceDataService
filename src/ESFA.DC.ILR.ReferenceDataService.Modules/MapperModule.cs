using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Interface;
using ESFA.DC.ILR.ReferenceDataService.Data.Population.Mapper.Message;

namespace ESFA.DC.ILR.ReferenceDataService.Modules
{
    public class MapperModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<EmpIdMapper>().As<IMessageMapper<IReadOnlyCollection<int>>>();
            containerBuilder.RegisterType<EpaOrgIdMapper>().As<IMessageMapper<IReadOnlyCollection<string>>>();
            containerBuilder.RegisterType<LearnAimRefMapper>().As<IMessageMapper<IReadOnlyCollection<string>>>();
            containerBuilder.RegisterType<LearningProviderUkprnMapper>().As<IMessageMapper<int>>();
            containerBuilder.RegisterType<PostcodesMapper>().As<IMessageMapper<IReadOnlyCollection<string>>>();
            containerBuilder.RegisterType<StdCodeMapper>().As<IMessageMapper<IReadOnlyCollection<int>>>();
            containerBuilder.RegisterType<UkprnsMapper>().As<IMessageMapper<IReadOnlyCollection<int>>>();
            containerBuilder.RegisterType<UlnMapper>().As<IMessageMapper<IReadOnlyCollection<long>>>();
        }
    }
}
