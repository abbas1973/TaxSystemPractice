using Application.Features.Base;
using AutoMapper;
using Base.Application.Mapping;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile(Assembly assembly)
        {
            ApplyMappingsFromAssembly(assembly);
        }

        public ILogger<MappingProfile> Logger { get; }

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            // این تایپ ها جنریک هستند و نیاز به مپ شدن ندارند
            var NotAllowedType = new List<Type>()
            {
                typeof(ICreateCommand<>),
                typeof(CreateCommand<>),
                typeof(IUpdateCommand<>),
                typeof(UpdateCommand<>)
            };

            var types = assembly.GetExportedTypes()
                .Where(t =>
                    !NotAllowedType.Contains(t)
                    && t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
                .ToList();

            foreach (var type in types)
            {
                Logger?.LogInformation(type.FullName);
                var instance = Activator.CreateInstance(type);
                var methodInfo = type.GetMethod("Mapping")
                    ?? type.GetInterface("IMapFrom`1").GetMethod("Mapping");

                methodInfo.Invoke(instance, new object[] { this });
            }
        }
    }
}
