using AutoMapper;
using MemoryBox.Application.Utils.Pagination;
using MemoryBox.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MemoryBox.Application.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
            CreateMap(typeof(PaginatedList<>), typeof(IPaginatedList<>)).ConvertUsing(typeof(PaginatedListConverter<,>));
        }

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var mapFromType = typeof(IMapFrom<>);

            var mappingMethodName = nameof(IMapFrom<object>.Mapping);

            bool HasInterface(Type t) => t.IsGenericType && t.GetGenericTypeDefinition() == mapFromType;

            var types = assembly.GetExportedTypes().Where(t => t.GetInterfaces().Any(HasInterface)).ToList();

            var argumentTypes = new Type[] { typeof(Profile) };

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);

                var methodInfo = type.GetMethod(mappingMethodName);

                if (methodInfo != null)
                {
                    methodInfo.Invoke(instance, new object[] { this });
                }
                else
                {
                    var interfaces = type.GetInterfaces().Where(HasInterface).ToList();

                    if (interfaces.Count > 0)
                    {
                        foreach (var @interface in interfaces)
                        {
                            var interfaceMethodInfo = @interface.GetMethod(mappingMethodName, argumentTypes);

                            interfaceMethodInfo.Invoke(instance, new object[] { this });
                        }
                    }
                }
            }
        }
    }

    // Custom converter để ánh xạ từ PaginatedList<TSource> sang IPaginatedList<TDestination>
    public class PaginatedListConverter<TSource, TDestination> : ITypeConverter<PaginatedList<TSource>, IPaginatedList<TDestination>>
    {
        public IPaginatedList<TDestination> Convert(PaginatedList<TSource> source, IPaginatedList<TDestination> destination, ResolutionContext context)
        {
            // Ánh xạ từng item trong danh sách
            var items = context.Mapper.Map<IReadOnlyCollection<TDestination>>(source.Items);

            // Trả về đối tượng IPaginatedList<TDestination> mới
            return new PaginatedList<TDestination>(items, source.TotalItems, source.CurrentPage, source.PageSize);
        }
    }
}
